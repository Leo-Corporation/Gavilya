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
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Gavilya.Classes
{
    /// <summary>
    /// Settings of Gavilya
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// The language of Gavilya.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Indicates if it is the first run of Gavilya.
        /// </summary>
        public bool IsFirstRun { get; set; }

        /// <summary>
        /// Indicates if the <see cref="MainWindow"/> is maximized or not.
        /// </summary>
        public bool IsMaximized { get; set; }

        /// <summary>
        /// The page id to check on startup.
        /// </summary>
        public int PageId { get; set; }
    }

    public static class SettingsSaver
    {
        /// <summary>
        /// Loads the settings into <see cref="Definitions.Settings"/>.
        /// </summary>
        public static void Load()
        {
            string AppDataPath = Env.GetAppDataPath(); // Get %APPDATA% folder
            if (File.Exists(AppDataPath + @"\Gavilya\Settings.gavsettings"))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings)); // XML Serializer
                StreamReader streamReader = new StreamReader(AppDataPath + @"\Gavilya\Settings.gavsettings"); // Where the file is going to be read

                Definitions.Settings = (Settings)xmlSerializer.Deserialize(streamReader); // Read

                streamReader.Dispose();
            }
            else
            {
                Definitions.Settings = new Settings { Language = "_default", IsFirstRun = true, IsMaximized = false, PageId = 0 };
                Save();
            }
        }

        /// <summary>
        /// Saves the settings into the <c>%APPDATA%</c> folder.
        /// </summary>
        public static void Save()
        {
            string AppDataPath = Env.GetAppDataPath(); // Get %APPDATA% folder
            XmlSerializer xmlSerializer = new XmlSerializer(Definitions.Settings.GetType()); // XML Serializer

            if (!Directory.Exists(AppDataPath + @"\Gavilya")) // If the directory doesn't exist
            {
                Directory.CreateDirectory(AppDataPath + @"\Gavilya"); // Create the directory
            }

            StreamWriter streamWriter = new StreamWriter(AppDataPath + @"\Gavilya\Settings.gavsettings"); // The place where the file is going to be written
            xmlSerializer.Serialize(streamWriter, Definitions.Settings);

            streamWriter.Dispose();
        }
    }
}
