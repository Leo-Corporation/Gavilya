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

using PeyrSharp.Core;
using PeyrSharp.Env;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Xml.Serialization;

namespace Gavilya.Classes;
public static class ThemeManager
{
	public static void ChangeTheme(ThemeInfo themeInfo, string path)
	{
		App.Current.Resources.MergedDictionaries.Clear();
		ResourceDictionary resourceDictionary = new();
		resourceDictionary.Source = new($@"{path}\{themeInfo.FilePath}");
		App.Current.Resources.MergedDictionaries.Add(resourceDictionary);
	}

	/// <summary>
	/// Gets a <see cref="List"/> of installed <see cref="ThemeInfo"/>.
	/// </summary>
	/// <returns>The installed themes</returns>
	public static List<(ThemeInfo, string)> GetInstalledThemes()
	{
		string themePath = $@"{FileSys.AppDataPath}\Gavilya\Themes\";
		// If there is no themes
		if (!Directory.Exists(themePath))
		{
			Directory.CreateDirectory(themePath);
			return new()
			{
				(new ThemeInfo(Properties.Resources.Default, "Dark.xaml", Definitions.Version), "")
			};
		}

		List<(ThemeInfo, string)> installedThemes = new()
		{
			(new ThemeInfo(Properties.Resources.Default, "Dark.xaml", Definitions.Version), "")
		};
		// If there are themes installed
		foreach (var subdir in Directory.GetDirectories(themePath))
		{
			if (File.Exists($@"{subdir}\theme.manifest"))
			{
				try
				{
					var xmlSerializer = new XmlSerializer(typeof(ThemeInfo));
					StreamReader streamReader = new($@"{subdir}\theme.manifest");
					ThemeInfo theme = (ThemeInfo)xmlSerializer.Deserialize(streamReader);

					
					installedThemes.Add((theme, subdir));
				} catch { }
			}
		}
		return installedThemes;
	}

	public static void InstallTheme(string path)
	{
		string guid = GuidGen.Generate(new GuidOptions(32, false, false, false));
		ZipFile.ExtractToDirectory(path, $@"{FileSys.AppDataPath}\Gavilya\Themes\{guid}");
	}

	public static ThemeInfo GetThemeInfoFromPath(string path)
	{
		var xmlSerializer = new XmlSerializer(typeof(ThemeInfo));
		StreamReader streamReader = new(path);
		return (ThemeInfo)xmlSerializer.Deserialize(streamReader);
	}
}
