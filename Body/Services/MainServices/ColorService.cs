using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Services.MainServices
{
    static class ColorService
    {
        public static Color BackgroundColor;
        public static Color ForegroundColor;
        public static Color SelectionColor;
        public static bool ColorVisibility = true;

        private static List<ColorModel> Colors;
        public static void SetColorPalette()
        {
            Colors = new List<ColorModel>();
            foreach (string ColorString in XF.GetColors())
                Colors.Add(new ColorModel(ColorString));

            BackgroundColor = GetColorByName("black");
            ForegroundColor = GetColorByName("white");
            SelectionColor = GetColorByName("yellow");
        }
        public static Color GetColorByName(string colorName)
        {
            if (!ColorVisibility) colorName = "White";

            int index = 0;
            colorName = colorName.ToLower();
            while (index < Colors.Count && Colors[index].ColorName.ToLower() != colorName) index++;
            if (index != Colors.Count) return Colors[index].Color;
            else return Colors[0].Color;
        }
        public static Color GetColorByID(int colorID)
        {
            int index = 0;
            while (index < Colors.Count && Colors[index].ColorID != colorID) index++;
            if (index != Colors.Count) return Colors[index].Color;
            else return Colors[0].Color;
        }
        public static Color Brighter(Color color, int procent = 50)
        {
            if (color.R == color.G && color.G == color.B) procent = 100;
            int R = color.R + (255 - color.R) * procent / 100;
            int G = color.G + (255 - color.G) * procent / 100;
            int B = color.B + (255 - color.B) * procent / 100;
            color = Color.FromArgb(R, G, B);
            return color;
        }
        public static Color Darker(Color color, int procent = 40)
        {
            int R = color.R - color.R * procent / 100;
            int G = color.G - color.G * procent / 100;
            int B = color.B - color.B * procent / 100;
            color = Color.FromArgb(R, G, B);
            return color;
        }
        public static Color Greener(Color color, int procent = 20)
        {
            int R = color.R - color.R * procent / 100;
            int G = color.G + (255 - color.G) * procent / 100;
            int B = color.B - color.B * procent / 100;
            color = Color.FromArgb(R, G, B);
            return color;
        }
        public static Color Bluer(Color color, int procent = 20)
        {
            int R = color.R - color.R * procent / 100;
            int G = color.G - color.G * procent / 100;
            int B = color.B + (255 - color.B) * procent / 100;
            color = Color.FromArgb(R, G, B);
            return color;
        }
        public static Color Yellower(Color color, int procent = 70)
        {
            int R = color.R + (255 - color.R) * procent / 100;
            int G = color.G + (255 - color.G) * procent / 100;
            int B = color.B - color.B * procent / 100;
            color = Color.FromArgb(R, G, B);
            return color;
        }

        public static int[] PurpleEffect { get { return new int[] { 0, 5, 13, 5, 0, 0 }; } }
        public static int[] RedEffect { get { return new int[] { 0, 4, 12, 4, 0, 0 }; } }
        public static int[] YellowEffect { get { return new int[] { 0, 6, 14, 6, 0, 0 }; } }
        public static int[] GreenEffect { get { return new int[] { 0, 2, 10, 2, 0, 0 }; } }
        public static int[] BlueEffect { get { return new int[] { 1, 3, 11, 3, 1, 0 }; } }
        public static int[] WhiteEffect { get { return new int[] { 5, 3, 1, 3, 5, 7 }; } }


    }
}
