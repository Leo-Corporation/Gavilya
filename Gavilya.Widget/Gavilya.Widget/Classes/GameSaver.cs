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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Gavilya.Widget.Classes
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

            if (!Directory.Exists(Definitions.AppDataPath + @"\Gavilya")) // If the directory doesn't exist
            {
                Directory.CreateDirectory(Definitions.AppDataPath + @"\Gavilya"); // Create the directory
            }

            StreamWriter streamWriter = new StreamWriter(Definitions.AppDataPath + @"\Gavilya\Games.gav"); // The place where the file is gonna be writen
            xmlSerializer.Serialize(streamWriter, games); // Create the file
            streamWriter.Dispose();
        }

        /// <summary>
        /// Exports the games.
        /// </summary>
        /// <param name="games">The games to export.</param>
        /// <param name="path">The path where the games are going to be exported.</param>
        internal void Export(List<GameInfo> games, string path)
        {
            try
            {
                List<GameInfo> gameInfos = games;
                XmlSerializer xmlSerializer = new XmlSerializer(gameInfos.GetType()); // XML Serializer
                StreamWriter streamWriter = new StreamWriter(path); // The place where the file is going to be exported
                xmlSerializer.Serialize(streamWriter, gameInfos); // Create the file
                streamWriter.Dispose();
            }
            catch (Exception ex)
            {
                
            }
        }

        /// <summary>
        /// Imports games.
        /// </summary>
        /// <param name="path">The path of the <c>.gav</c> file.</param>
        /// <param name="isFirstRun">Indicates if it is the first run of the program.</param>
        internal void Import(string path, bool isFirstRun = false)
        {
            try
            {
                if (File.Exists(path))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<GameInfo>)); // XML Serializer
                    StreamReader streamReader = new StreamReader(path); // The path of the file

                    Definitions.Games = (List<GameInfo>)xmlSerializer.Deserialize(streamReader); // Re-create each GameInfo
                    streamReader.Dispose();

                    Save(Definitions.Games); // Save the games
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
