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
using System;
using System.Collections.Generic;

namespace Gavilya.Models;

public class Game
{
	public string Name { get; set; }
	public string Description { get; set; }
	public string CoverFilePath { get; set; }
	public int LastTimePlayed { get; set; }
	public int TotalTimePlayed { get; set; }
	public string Command { get; set; }
	public string? ProcessName { get; set; }
	public bool CheckIfRunning { get; set; }
	public GameType GameType { get; set; }
	public bool IsFavorite { get; set; }
	public bool IsHidden { get; set; }
	public string Id { get; init; }
	public int RawgId { get; set; }
	public List<Tag>? Tags { get; set; }

	public Game()
	{
		Id = Guid.NewGuid().ToString();
		RawgId = -1;
		Tags = new();
	}

	public override bool Equals(object? obj)
	{
		return obj is Game game && Id == game.Id;
	}

	public override int GetHashCode()
	{
		HashCode hash = new();
		hash.Add(Name);
		hash.Add(Description);
		hash.Add(CoverFilePath);
		hash.Add(LastTimePlayed);
		hash.Add(TotalTimePlayed);
		hash.Add(Command);
		hash.Add(ProcessName);
		hash.Add(CheckIfRunning);
		hash.Add(GameType);
		hash.Add(IsFavorite);
		hash.Add(IsHidden);
		hash.Add(Id);
		hash.Add(RawgId);
		hash.Add(Tags);
		return hash.ToHashCode();
	}
}
