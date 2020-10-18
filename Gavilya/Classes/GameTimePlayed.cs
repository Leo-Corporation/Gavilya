using System;
using System.Collections.Generic;
using System.Text;

namespace Gavilya.Classes
{
    public class GameTimePlayed
    {
        /// <summary>
        /// The number of hours the game was played.
        /// </summary>
        public int Hours { get; set; }
        
        /// <summary>
        /// The number of minutes the game was played.
        /// </summary>
        public int Minutes { get; set; }

        /// <summary>
        /// The number of seconds the game was played.
        /// </summary>
        public int Seconds { get; set; }

        public static GameTimePlayed GetTimePlayed(int timePlayed)
        {
            int time = timePlayed; // The time played
            int hours, minutes, seconds; // Define the hours, minutes and seconds

            if (time / 3600 < 1) // If the game was played less than an hour
            {
                hours = 0; // Set hours to 0
            }
            else
            {
                hours = time / 3600; // Get the number of hours the game was played.
            }

            time = time - hours * 3600; // Get the remaining the time

            if (time / 60 < 1) // If the game was played less than a minute
            {
                minutes = 0; // Set minutes to 0
            }
            else
            {
                minutes = time / 60; // Get the number of minutes the game was played
            }

            time = time - minutes * 60; // Get the remaining time
            seconds = time; // The remaining time is seconds

            return new GameTimePlayed { Hours = hours, Minutes = minutes, Seconds = seconds }; // Return
        }
    }
}
