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
using LeoCorpLibrary;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Gavilya.Classes
{
	/// <summary>
	/// Methods for managing profiles.
	/// </summary>
	public static class ProfileManager
	{
		/// <summary>
		/// Create the default profile.
		/// </summary>
		public static void CreateDefaultProfile()
		{
			if (Definitions.Profiles is not null) // If not null
			{
				Definitions.Profiles.Add(
					new Profile
					{
						Name = Global.UserName,
						PictureFilePath = "_default",
						SaveFilePath = Env.AppDataPath + @"\Gavilya\Games.gav"
					});
			}
			else
			{
				Definitions.Profiles = new()
				{
					new Profile
					{
						Name = Global.UserName,
						PictureFilePath = "_default",
						SaveFilePath = Env.AppDataPath + @"\Gavilya\Games.gav"
					}
				};
			}
		}

		/// <summary>
		/// Saves profiles data in a XML .GAVPROFILES file.
		/// </summary>
		public static void SaveProfiles()
		{
			string AppDataPath = Env.AppDataPath; // Get %APPDATA% folder
			XmlSerializer xmlSerializer = new(Definitions.Profiles.GetType()); // XML Serializer

			if (!Directory.Exists(AppDataPath + @"\Gavilya")) // If the directory doesn't exist
			{
				Directory.CreateDirectory(AppDataPath + @"\Gavilya"); // Create the directory
			}

			StreamWriter streamWriter = new(AppDataPath + @"\Gavilya\Profiles.gavprofiles"); // The place where the file is going to be written
			xmlSerializer.Serialize(streamWriter, Definitions.Profiles);

			streamWriter.Dispose();
		}

		/// <summary>
		/// Loads profiles from a .GAVPROFILES file.
		/// </summary>
		public static void LoadProfiles()
		{
			string AppDataPath = Env.AppDataPath; // Get %APPDATA% folder
			if (File.Exists(AppDataPath + @"\Gavilya\Profiles.gavprofiles"))
			{
				XmlSerializer xmlSerializer = new(typeof(List<Profile>)); // XML Serializer
				StreamReader streamReader = new(AppDataPath + @"\Gavilya\Profiles.gavprofiles"); // Where the file is going to be read

				Definitions.Profiles = (List<Profile>)xmlSerializer.Deserialize(streamReader); // Read

				streamReader.Dispose();
			}
			else
			{
				CreateDefaultProfile();
				SaveProfiles();
			}
		}
	}
}
