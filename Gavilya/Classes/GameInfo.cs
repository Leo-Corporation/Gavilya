using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Gavilya.Classes
{
    /// <summary>
    /// Informations that a "Game" countains.
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
        public Image Icon { get; set; }
    }
}
