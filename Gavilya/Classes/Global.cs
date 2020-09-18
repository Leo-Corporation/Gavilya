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
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Gavilya.Classes
{
    /// <summary>
    /// A class that contains methods and more to do global things in the app.
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// Sets the window icon using <c>\Gavilya.ico</c>.
        /// </summary>
        /// <param name="window"><see cref="Window"/> to set the icon to.</param>
        public static void SetWindowIcon(Window window)
        {
            Uri icon = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"\Gavilya.ico"); // Define the path to the icon
            window.Icon = BitmapFrame.Create(icon); // Set the icon
        }
    }
}
