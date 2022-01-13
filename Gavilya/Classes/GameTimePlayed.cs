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

namespace Gavilya.Classes;

public class GameTimePlayed
{
	/// <summary>
	/// The number of hours the game was played.
	/// </summary>
	public int Hours { get; set; }

	/// <summary>
	/// The number of minutes the game was played.
	/// </summary>
	public int Minutes { get; set; }

	/// <summary>
	/// The number of seconds the game was played.
	/// </summary>
	public int Seconds { get; set; }

	public static GameTimePlayed GetTimePlayed(int timePlayed)
	{
		int time = timePlayed; // The time played
		int hours, minutes, seconds; // Define the hours, minutes and seconds

		if (time / 3600 < 1) // If the game was played less than an hour
		{
			hours = 0; // Set hours to 0
		}
		else
		{
			hours = time / 3600; // Get the number of hours the game was played.
		}

		time -= hours * 3600; // Get the remaining the time

		if (time / 60 < 1) // If the game was played less than a minute
		{
			minutes = 0; // Set minutes to 0
		}
		else
		{
			minutes = time / 60; // Get the number of minutes the game was played
		}

		time -= minutes * 60; // Get the remaining time
		seconds = time; // The remaining time is seconds

		return new GameTimePlayed { Hours = hours, Minutes = minutes, Seconds = seconds }; // Return
	}
}
