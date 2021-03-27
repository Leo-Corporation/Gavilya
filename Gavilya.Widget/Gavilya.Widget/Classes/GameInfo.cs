﻿/*
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gavilya.Widget.Classes
{
    /// <summary>
    /// Informations that a "Game" contains.
    /// </summary>
    public class GameInfo
    {
        /// <summary>
        /// Description of the game.
        /// </summary>
        public string Description { get; set; }

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
        /// The name of the process to look for when the game is started. Empty by default.
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// <para>The RAWG id of the game.</para>
        /// <para>If the Id is <c>-1</c>, then there is no RAWG id associated.</para>
        /// </summary>
        public int RAWGID { get; set; }

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

        /// <summary>
        /// The game available platforms.
        /// </summary>
        public List<SDK.RAWG.Platform> Platforms { get; set; }
    }
}
