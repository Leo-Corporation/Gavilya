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

using PeyrSharp.Env;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gavilya.Models;
public class ProfileList : ObservableCollection<Profile>
{
	public string SelectedProfileUuid { get; set; }
    public ProfileList() : base()
    {
        
    }

    public void Save()
    {
		XmlSerializer xmlSerializer = new(GetType());
		if (!Directory.Exists($@"{FileSys.AppDataPath}\Léo Corporation\Gavilya")) // If the directory doesn't exist
		{
			Directory.CreateDirectory($@"{FileSys.AppDataPath}\Léo Corporation\Gavilya"); // Create the directory
		}
		StreamWriter streamWriter = new($@"{FileSys.AppDataPath}\Léo Corporation\Gavilya\Profiles.g4v");
		xmlSerializer.Serialize(streamWriter, this);
		streamWriter.Dispose();
	}

	public void Load()
	{
		if (!File.Exists($@"{FileSys.AppDataPath}\Léo Corporation\Gavilya\Profiles.g4v"))
		{
			Add(new(Environment.UserName));
			SelectedProfileUuid = this[0].ProfileUuid;
			Save();
			return;
		}
		XmlSerializer xmlSerializer = new(GetType());
		StreamReader streamReader = new($@"{FileSys.AppDataPath}\Léo Corporation\Gavilya\Profiles.g4v");
		ProfileList loadedProfiles = (ProfileList)xmlSerializer.Deserialize(streamReader) ?? new() { new(Environment.UserName) };
		Clear(); // Clear existing profiles

		foreach (Profile profile in loadedProfiles)
		{
			Add(profile); // Add each loaded profile
		}

		streamReader.Dispose();
	}
}
