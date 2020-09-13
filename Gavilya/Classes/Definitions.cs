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
using Gavilya.Pages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Media;

namespace Gavilya.Classes
{
    /// <summary>
    /// Definitions.
    /// </summary>
    public static class Definitions
    {
        /// <summary>
        /// The gradient for the "HomeButtonBackColor".
        /// </summary>
        public static LinearGradientBrush HomeButtonBackColor
        {
            get
            {
                Color color1 = Color.FromRgb(181, 137, 224); // First color
                Color color2 = Color.FromRgb(133, 97, 197); // Second color

                GradientStopCollection gradientStops = new GradientStopCollection(); // A collection of GradientStop

                GradientStop gradientStop = new GradientStop(); // First GradientStop
                GradientStop gradientStop1 = new GradientStop(); // Second GradientStop

                gradientStop.Color = color1; // Color of the first GradientStop
                gradientStop.Offset = 0; // Offset of the first GradientStop

                gradientStop1.Color = color2; // Color of the second GradientStop
                gradientStop1.Offset = 1; // Offset of the second GradientStop

                gradientStops.Add(gradientStop); // Add the first GradientStop
                gradientStops.Add(gradientStop1); // Add the second GradientStop

                LinearGradientBrush linearGradientBrush = new LinearGradientBrush(); // Gradient to return
                linearGradientBrush.GradientStops = gradientStops;// Add the GradientStops to the corresponding property

                return linearGradientBrush; // Return the gradient
            }
        }

        /// <summary>
        /// Version of the software (Gavilya).
        /// </summary>
        public static string Version { get => "0.1.0.2009"; }

        /// <summary>
        /// The Main <see cref="System.Windows.Window"/> of the App.
        /// </summary>
        public static MainWindow MainWindow { get; set; }

        /// <summary>
        /// The <see cref="Pages.GamesCardsPages"/> of the <see cref="MainWindow"/>.
        /// </summary>
        public static GamesCardsPages GamesCardsPages { get; set; }

        /// <summary>
        /// The state of the checkboxes of all the <see cref="UserControls.GameCard"/>.
        /// </summary>
        public static bool IsGamesCardsPagesCheckBoxesVisible { get; set; }

        /// <summary>
        /// The games that are added to the <see cref="MainWindow"/>.
        /// </summary>
        public static List<GameInfo> Games = new List<GameInfo>();

        /// <summary>
        /// The link of the last version <see cref="string"/>.
        /// </summary>
        public static string LastVersionLink { get => "https://raw.githubusercontent.com/Leo-Corporation/LeoCorp-Docs/master/Liens/Update%20System/Gavilya/Version.txt"; }

        /// <summary>
        /// True if the menu is shown
        /// </summary>
        public static bool IsMenuShown { get; set; }
    }
}
