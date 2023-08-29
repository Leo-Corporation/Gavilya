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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gavilya.Services;
public class GameMigrationService
{
	private readonly string _filePath;

	public GameMigrationService(string filePath)
    {
		_filePath = filePath;
	}

	public GameList Migrate()
	{
		// 1. Load old games format file
		List<GameInfo> gameInfos = new();

		XmlSerializer xmlSerializer = new(typeof(List<GameInfo>)); // XML Serializer
		StreamReader streamReader = new(_filePath); // The place where the file is gonna be read

		gameInfos = (List<GameInfo>)xmlSerializer.Deserialize(streamReader) ?? new(); // Re-create each game info
		streamReader.Dispose();

		// 2. Iterate over each game and convert it to the new format
		var games = gameInfos.Select(g => new Game()
		{
			Name = g.Name,
			ProcessName = g.ProcessName,
			CoverFilePath = g.IconFileLocation,
			CheckIfRunning = g.AlwaysCheckIfRunning,
			Command = g.FileLocation,
			Description = g.Description,
			IsFavorite = g.IsFavorite,
			IsHidden = g.IsHidden ?? false,
			LastTimePlayed = g.LastTimePlayed,
			TotalTimePlayed = g.TotalTimePlayed,
			RawgId = g.RAWGID,
			GameType = GetGameType(g.IsSteam, g.IsUWP),
		});

		return new GameList(games);
	}

	private GameType GetGameType(bool isSteam, bool isUwp)
	{
		if (isUwp) return GameType.UWP;
		if (isSteam) return GameType.Steam;
		return GameType.Win32;
	}
}


/// <summary>
/// Do not use except for migrating games.
/// </summary>
public class GameInfo
{
	/// <summary>
	/// Description of the game.
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	/// Name of the game.
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// The location of the game.
	/// </summary>
	public string FileLocation { get; set; }

	/// <summary>
	/// The version of the game.
	/// </summary>
	public string Version { get; set; }

	/// <summary>
	/// The location of the Icon of the game.
	/// </summary>
	public string IconFileLocation { get; set; }

	/// <summary>
	/// The name of the process to look for when the game is started. Empty by default.
	/// </summary>
	public string ProcessName { get; set; }

	/// <summary>
	/// <para>The RAWG id of the game.</para>
	/// <para>If the Id is <c>-1</c>, then there is no RAWG id associated.</para>
	/// </summary>
	public int RAWGID { get; set; }

	/// <summary>
	/// The last time the game was launched.
	/// </summary>
	public int LastTimePlayed { get; set; }

	/// <summary>
	/// The total time the game was played.
	/// </summary>
	public int TotalTimePlayed { get; set; }

	/// <summary>
	/// <see cref="true"/> if the game is a favorite, <see cref="false"/> if not.
	/// </summary>
	public bool IsFavorite { get; set; }

	/// <summary>
	/// True if Gavilya should always check if the game is running.
	/// </summary>
	public bool AlwaysCheckIfRunning { get; set; }

	/// <summary>
	/// True if the Game is a Xbox/UWP game.
	/// </summary>
	public bool IsUWP { get; set; }

	/// <summary>
	/// True if the game uses Steam URI to launch/or requires Steam.
	/// </summary>
	public bool IsSteam { get; set; }

	/// <summary>
	/// True if the game shouldn't display.
	/// </summary>
	public bool? IsHidden { get; set; }
}