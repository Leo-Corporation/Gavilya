using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Gavilya.Classes
{
    /// <summary>
    /// Informations that a "Game" contains.
    /// </summary>
    public class GameInfo
    {
        /// <summary>
        /// Name of the game.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The location of the game.
        /// </summary>
        public string FileLocation { get; set; }

        /// <summary>
        /// The version of the game.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// The location of the Icon of the game.
        /// </summary>
        public string IconFileLocation { get; set; }

        /// <summary>
        /// The icon of the game.
        /// </summary>
        //public Image Icon;

        /// <summary>
        /// The last time the game was launched.
        /// </summary>
        public int LastTimePlayed { get; set; }

        /// <summary>
        /// The total time the game was played.
        /// </summary>
        public int TotalTimePlayed { get; set; }

        /// <summary>
        /// <see cref="true"/> if the game is a favorite, <see cref="false"/> if not.
        /// </summary>
        public bool IsFavorite { get; set; }
    }
}
