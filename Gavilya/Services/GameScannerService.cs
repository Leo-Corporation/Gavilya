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

using Gavilya.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Gavilya.Services;

public class GameScannerService
{
	public static List<ExecutableViewModel> ScanForExecutables(string directory, GameEditionViewModel gameEditionViewModel)
	{
		List<ExecutableViewModel> executableFiles = [];

		try
		{
			// Recursively search for files with .exe extension in the directory and its subdirectories
			foreach (string filePath in Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories).Where(file => file.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)))
			{
				string fileName = Path.GetFileName(filePath);
				FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath); // Get the version
				executableFiles.Add(new(string.IsNullOrEmpty(fileVersionInfo.ProductName) ? fileName : fileVersionInfo.ProductName, filePath, gameEditionViewModel));
			}

			return executableFiles;
		}
		catch
		{
			return executableFiles;
		}
	}
}
