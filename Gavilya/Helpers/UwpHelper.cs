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

using Gavilya.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Gavilya.Helpers;

public static class UwpHelper
{
	/// <summary>
	/// Returns asynchronously a list of <see cref="UwpApp"/> that are installed on the user's computer.
	/// </summary>
	/// <returns>A <see cref="List{T}"/> of <see cref="UwpApp"/>.</returns>
	public static async Task<List<UwpApp>> GetUwpAppsAsync()
	{
		try
		{
			ProcessStartInfo processInfo = new()
			{
				FileName = "powershell.exe",
				Arguments = $@"& Get-StartApps | ConvertTo-Json > $env:appdata\UwpApps.json",
				UseShellExecute = false,
				CreateNoWindow = true
			};

			Process process = new()
			{
				StartInfo = processInfo
			};
			process.Start();
			await process.WaitForExitAsync();

			string text = await File.ReadAllTextAsync($@"{PeyrSharp.Env.FileSys.AppDataPath}\UwpApps.json"); // Deserialize

			List<UwpApp> apps = JsonSerializer.Deserialize<List<UwpApp>>(text) ?? new(); // Get apps
			List<UwpApp> sortedApps = new(); // Create final list
			Dictionary<UwpApp, string> uwpApps = new(); // Create a dictionnary

			// Sort apps to only have UWP apps (they have a "!" in the AppID property)
			for (int i = 0; i < apps.Count; i++)
			{
				if (apps[i].AppID.Contains('!'))
				{
					uwpApps.Add(apps[i], apps[i].Name);
				}
			}

			// Sort alphabetically
			var sorted = from pair in uwpApps orderby pair.Value ascending select pair; // Sort

			foreach (KeyValuePair<UwpApp, string> pair1 in sorted)
			{
				sortedApps.Add(pair1.Key);
			}

			return sortedApps; // Return
		}
		catch (Exception ex)
		{
			return new() { new(ex.StackTrace, ex.Message) };
		}
	}
}
