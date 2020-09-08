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
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Gavilya.Classes
{
    internal class GameSaver
    {
        /// <summary>
        /// Save a list of games.
        /// </summary>
        /// <param name="games">The games to save.</param>
        internal void Save(List<GameInfo> games)
        {
            List<GameInfo> gameInfos = games;
            XmlSerializer xmlSerializer = new XmlSerializer(gameInfos.GetType()); // XML Serializer

            if (!Directory.Exists(AppDataPath + @"\Gavilya")) // If the directory doesn't exist
            {
                Directory.CreateDirectory(AppDataPath + @"\Gavilya"); // Create the directory
            }

            StreamWriter streamWriter = new StreamWriter(AppDataPath + @"\Gavilya\Games.gav"); // The place where the file is gonna be writen
            xmlSerializer.Serialize(streamWriter, games); // Create the file
            streamWriter.Dispose();
        }

        /// <summary>
        /// Load the saved games into a <see cref="List{GameInfo}"/>.
        /// </summary>
        internal void Load()
        {
            if (File.Exists(AppDataPath + @"\Gavilya\Games.gav")) // If there is a save file
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<GameInfo>)); // XML Serializer
                StreamReader streamReader = new StreamReader(AppDataPath + @"\Gavilya\Games.gav"); // The place where the file is gonna be read

                Definitions.Games = (List<GameInfo>)xmlSerializer.Deserialize(streamReader); // Re-create each game info
                streamReader.Dispose();
            }
        }

        private string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }
}
