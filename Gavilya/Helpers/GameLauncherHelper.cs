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
	private readonly Game _game;
	private readonly GameList _games;
	private readonly DispatcherTimer _dispatcherTimer;
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
		// Changes the default monitor if needed
		if (_game.DefaultMonitor != null && _game.DefaultMonitor.DeviceID != "-1") DesktopMonitorHelper.SetDefaultMonitor(_game.DefaultMonitor);

		// Check location if the game is a Win32 app
		if (_game.GameType == GameType.Win32 && !File.Exists(_game.Command)) return false; // Abort
		if (_game.GameType == GameType.Steam && !CanLaunchSteamGame(_game)) return false;
		if (_game.GameType == GameType.UWP && _game.Command.Split("!").Length < 2) return false;

		_game.LastTimePlayed = Sys.UnixTime;
		OnGameUpdatedEvent?.Invoke(this, new(_game));

		_games[_games.IndexOf(_game)] = _game;

		if (_game.GameType == GameType.Steam) Process.Start("cmd", "/c start " + _game.Command);
		if (_game.GameType == GameType.Win32) Process.Start(_game.Command);
		if (_game.GameType == GameType.UWP) Process.Start("explorer.exe", _game.Command);

		_dispatcherTimer.Start();

		return true;
	}

	/// <summary>
	/// This method only works on Win32 Games.
	/// </summary>
	/// <returns></returns>
	public bool LaunchAsAdmin()
	{
		// Check location if the game is a Win32 app
		if (_game.GameType != GameType.Win32) return false;
		if (!File.Exists(_game.Command)) return false; // Abort

		_game.LastTimePlayed = Sys.UnixTime;
		OnGameUpdatedEvent?.Invoke(this, new(_game));

		_games[_games.IndexOf(_game)] = _game;
		Sys.ExecuteAsAdmin(_game.Command);
		_dispatcherTimer.Start();

		return true;
	}

	private static bool CanLaunchSteamGame(Game game)
	{
		// Detect if a steam game you are trying to run is installed.
		try
		{
			String steamAppKeyFormat = @"SOFTWARE\Valve\Steam\Apps\" + game.Command.Replace("steam://rungameid/", "");
			RegistryKey steamAppKey = Registry.CurrentUser.OpenSubKey(steamAppKeyFormat);
			string isSteamGameInstalled = steamAppKey.GetValue("Installed").ToString();

			if (isSteamGameInstalled != "1")
			{
				MessageBox.Show(Properties.Resources.SteamAppNotInstalled, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error);
				return false;
			}
		}
		catch
		{
			return false;
		}

		return true; // Return true, Steam can launch game.
	}

	public class GameEventArgs(Game game) : EventArgs
	{
		public Game Game { get; init; } = game;
	}
}
