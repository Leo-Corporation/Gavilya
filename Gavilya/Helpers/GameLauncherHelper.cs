/*
MIT License

Copyright (c) Léo Corporation

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 
*/

using Gavilya.Enums;
using Gavilya.Models;
using Microsoft.Win32;
using PeyrSharp.Env;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace Gavilya.Helpers;
public class GameLauncherHelper
{
	private Game _game;
	private GameList _games;
	private DispatcherTimer _dispatcherTimer;
	private bool _gameStarted;

	public event EventHandler<GameEventArgs> OnGameUpdatedEvent;

	public GameLauncherHelper(Game game, GameList games)
	{
		_game = game;
		_games = games;
		_dispatcherTimer = new() { Interval = TimeSpan.FromSeconds(1) };
		_dispatcherTimer.Tick += (o, e) =>
		{
			string processName = _game.GameType switch
			{
				GameType.UWP => string.IsNullOrEmpty(_game.ProcessName) ? _game.Command : _game.ProcessName,
				_ => string.IsNullOrEmpty(_game.ProcessName) ? Path.GetFileNameWithoutExtension(_game.Command) : _game.ProcessName
			};

			if (!_gameStarted && Sys.IsProcessRunning(processName)) _gameStarted = true;
			if (_gameStarted && !Sys.IsProcessRunning(processName)) // If the game has been closed
			{
				int timeSpent = Sys.UnixTime - _game.LastTimePlayed;

				_game.TotalTimePlayed += timeSpent;
				_games[_games.IndexOf(_game)] = _game;
				_gameStarted = false;

				OnGameUpdatedEvent?.Invoke(this, new(_game));

				if (!_game.CheckIfRunning) _dispatcherTimer.Stop();
			}
		};
	}

	public bool Launch()
	{
		// Check location if the game is a Win32 app
		if (_game.GameType == GameType.Win32 && !File.Exists(_game.Command)) return false; // Abort
		if (_game.GameType == GameType.Steam && !CanLaunchSteamGame()) return false;

		_game.LastTimePlayed = Sys.UnixTime;
		OnGameUpdatedEvent?.Invoke(this, new(_game));

		_games[_games.IndexOf(_game)] = _game;
		Process.Start("explorer.exe", _game.Command);
		_dispatcherTimer.Start();

		return true;
	}

	private static bool CanLaunchSteamGame()
	{
		try
		{
			RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Valve\Steam\ActiveProcess");
			string result = key.GetValue("ActiveUser").ToString();
			if (result == "0")
			{
				MessageBox.Show(Properties.Resources.NotLoggedToSteam, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error); // If the user isn't logged to steam
				return false; // Return false, Steam cannot launch game.
			}

			// Check if Steam is not running
			if (!Sys.IsProcessRunning("Steam"))
			{
				// Show a message box
				MessageBox.Show(Properties.Resources.SteamNotRunning, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
				return false; // Return false, Steam cannot launch game.
			}
		}
		catch
		{
			return false;
		}

		return true; // Return true, Steam can launch game.
	}

	public class GameEventArgs : EventArgs
	{
		public Game Game { get; init; }

		public GameEventArgs(Game game)
		{
			Game = game;
		}
	}
}
