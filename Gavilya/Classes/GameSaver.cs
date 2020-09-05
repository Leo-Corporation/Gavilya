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
