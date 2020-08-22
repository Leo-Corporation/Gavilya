using System;
using System.Collections.Generic;
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
        /// Version of the software (Gavilya)
        /// </summary>
        public static string Version { get => "1.0.0.2009"; }
    }
}
