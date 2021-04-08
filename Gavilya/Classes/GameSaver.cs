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
using System.Windows;
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
            string filePath = Definitions.Profiles[Definitions.Settings.CurrentProfileIndex].SaveFilePath;

            List<GameInfo> gameInfos = games;
            XmlSerializer xmlSerializer = new(gameInfos.GetType()); // XML Serializer

            if (!Directory.Exists(AppDataPath + @"\Gavilya")) // If the directory doesn't exist
            {
                Directory.CreateDirectory(AppDataPath + @"\Gavilya"); // Create the directory
            }

            StreamWriter streamWriter = new(filePath); // The place where the file is gonna be writen
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
                XmlSerializer xmlSerializer = new(gameInfos.GetType()); // XML Serializer
                StreamWriter streamWriter = new(path); // The place where the file is going to be exported
                xmlSerializer.Serialize(streamWriter, gameInfos); // Create the file
                streamWriter.Dispose();
                MessageBox.Show(Properties.Resources.ExportSuccess, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Information); // Success
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Properties.Resources.ErrorOccurred}:\n{ex.Message}", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error); // Error
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
                    XmlSerializer xmlSerializer = new(typeof(List<GameInfo>)); // XML Serializer
                    StreamReader streamReader = new(path); // The path of the file

                    Definitions.Games = (List<GameInfo>)xmlSerializer.Deserialize(streamReader); // Re-create each GameInfo
                    streamReader.Dispose();

                    Save(Definitions.Games); // Save the games

                    if (!isFirstRun)
                    {
                        Global.ReloadAllPages(); // Reload all the pages
                        Definitions.MainWindow.PageContent.Content = Definitions.GamesCardsPages; // Change page
                    }

                    MessageBox.Show(Properties.Resources.ImportSuccess, Properties.Resources.MainWindowTitle, MessageBoxButton.OK, MessageBoxImage.Information); // Success
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{Properties.Resources.ErrorOccurred}:\n{ex.Message}", Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error); // Error
            }
        }

        /// <summary>
        /// Load the saved games into a <see cref="List{GameInfo}"/>.
        /// </summary>
        internal void Load()
        {
            string filePath = Definitions.Profiles[Definitions.Settings.CurrentProfileIndex].SaveFilePath;
            if (File.Exists(filePath)) // If there is a save file
            {
                XmlSerializer xmlSerializer = new(typeof(List<GameInfo>)); // XML Serializer
                StreamReader streamReader = new(filePath); // The place where the file is gonna be read

                Definitions.Games = (List<GameInfo>)xmlSerializer.Deserialize(streamReader); // Re-create each game info
                streamReader.Dispose();
            }
        }

        private string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }
}
