using FarmConsole.Body.Services;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controlers.MenuControlers
{
    static class AnimationController
    {

        private readonly static ConsoleColor[] colors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));
        private static int[] GetColors(string effectColor)
        {
            int[] idColors = new int[] { 8, 7, 15, 7, 8, 0 };
            switch (effectColor)
            {
                case "purple": idColors = new int[] { 0, 5, 13, 5, 0, 0 }; break;
                case "red": idColors = new int[] { 0, 4, 12, 4, 0, 0 }; break;
                case "yellow": idColors = new int[] { 0, 6, 14, 6, 0, 0 }; break;
                case "green": idColors = new int[] { 0, 2, 10, 2, 0, 0 }; break;
                case "blue": idColors = new int[] { 1, 3, 11, 3, 1, 0 }; break;
                case "white": idColors = new int[] { 8, 7, 15, 7, 8, 0 }; break;
            }
            return idColors;
        }

        public static void CrossEffect(int graphicID, string effectColor = "red", double middleTime = 1600)
        {
            AnimationView.Centrum(graphicID);
            DateTime Now = DateTime.Now;
            int[] idColors = GetColors(effectColor);
            double[] sleep = new double[] { 500, 100, middleTime, 100, 500 };
            int currentColorId = 0;
            int stage = 0;

            while (stage < 5)
            {
                if (Console.KeyAvailable) Console.ReadKey(true);
                if ((DateTime.Now - Now).TotalMilliseconds >= sleep[stage])
                {
                    switch (stage)
                    {
                        case 0: stage++; break;
                        case 1:
                            AnimationView.UpdatGraphic(1, 1, color: colors[idColors[currentColorId]]);
                            currentColorId++;
                            if (currentColorId > 2) stage++;
                            break;
                        case 2: stage++; break;
                        case 3:
                            AnimationView.UpdatGraphic(1, 1, color: colors[idColors[currentColorId]]);
                            currentColorId++;
                            if (currentColorId > 5) stage++;
                            break;
                        case 4: stage++; break;
                    }
                    Now = DateTime.Now;
                }
            }
            AnimationView.ClearList();
        }

        public static void SlideEffect(int graphicID, string effectColor = "purple", double middleTime = 2000)
        {
            DateTime Now = DateTime.Now;
            double[] sleep = new double[] { 30, middleTime, 30 };
            int[] idColors = GetColors(effectColor);
            int currentColorId = 0;
            int stage = 0;
            int columns = 9;
            int extraColumn = Convert.ToInt32(Convert.ToDouble(XF.GetGraphic(graphicID)[0].Length / 2) / Convert.ToDouble(Console.WindowWidth / columns));
            int column = 0 - extraColumn;

            while (stage < 3)
            {
                if (Console.KeyAvailable) Console.ReadKey(true);
                if ((DateTime.Now - Now).TotalMilliseconds >= sleep[stage])
                {
                    switch (stage)
                    {
                        case 0:
                            AnimationView.Column(graphicID, column, columns, colors[idColors[currentColorId]]);
                            column++;
                            if (column >= 0 && column % 3 == 1) currentColorId++;
                            if (currentColorId > 5) currentColorId = 5;
                            if (column == (columns + 1) / 2 + 1) stage++;
                            break;
                        case 1: stage++; break;
                        case 2:
                            AnimationView.Column(graphicID, column, columns, colors[idColors[currentColorId]]);
                            column++;
                            if (column % 3 == 1) currentColorId++;
                            if (currentColorId > 5) currentColorId = 5;
                            if (column == columns + extraColumn + 2) stage++;
                            break;
                    }
                    Now = DateTime.Now;
                }
            }
            AnimationView.ClearList();
        }

        public static int PopUp(string[] text, int stage = 0, string effectColor = "green", double middleTime = 3000)
        {
            int[] idColors = GetColors(effectColor);
            int currentColorId = 0, limit = 1;
            stage++;

            if (stage < 1 * limit) { currentColorId = 0; text = new string[] { text[0], text[1] }; }
            else if (stage < 2 * limit) { currentColorId = 1; text = new string[] { text[0], text[1], text[2] }; }
            else if (stage < 3 * limit + middleTime / 50) { currentColorId = 2; text = new string[] { text[0], text[1], text[2], text[3] }; }
            else if (stage < 4 * limit + middleTime / 50) { currentColorId = 3; text = new string[] { text[0], text[1], text[2] }; }
            else if (stage < 5 * limit + middleTime / 50) { currentColorId = 4; text = new string[] { text[0], text[1] }; }
            else if (stage < 6 * limit + middleTime / 50) { currentColorId = 5; stage = -1; text = new string[] { text[0] }; }

            AnimationView.PopUp(text, colors[idColors[currentColorId]]);
            return stage;
        }
    }
}
