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
using System;
using System.Diagnostics;
using System.IO;

namespace Gavilya.AppLauncher
{
	class Program
	{
		static void Main(string[] args)
		{
			string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Gavilya"; // %APPDATA%\Gavilya
			try
			{
				string path = File.ReadAllText(AppDataPath + @"\LatestGameOpened.txt"); // Get the path
				if (File.Exists(path)) // Check if the path exist
				{
					Process.Start(path);
				}
				else
				{
					throw new FileNotFoundException("Specified file wasn't found.", path); // Error
				}
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red; // Set the color of the console to red
				Console.WriteLine($"Error:\n\n{ex.Message}"); // Write the error
				Console.ResetColor(); // Reset the console color
				Console.Read(); // Wait for the user to close the window
			}
		}
	}
}
