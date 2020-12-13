using FarmConsole.Body.Model.Helpers;
using FarmConsole.Body.View.GUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.View.Components
{
    static class Animations
    {
        public static void CrossEffect(int graphicID, string effectColor = "red", double middleTime = 1600)
        {
            AnimationsGUI.Centrum(graphicID);
            DateTime Now = DateTime.Now;

            ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));

            int[] idColors = new int[] { 4, 12, 14, 15, 14, 12, 4, 0 };
            switch(effectColor)
            {
                case "red":     idColors = new int[] { 4, 12,  14, 12, 4, 0 }; break;
                case "blue":    idColors = new int[] { 1,  3,  11,  3, 1, 0 }; break;
                case "green":   idColors = new int[] { 2, 10,  15, 10, 2, 0 }; break;
                case "purple":  idColors = new int[] { 5, 13, 15, 13, 5, 0 }; break;
                case "white":   idColors = new int[] { 8, 7, 15, 7, 8, 0 }; break;
            }

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
                            AnimationsGUI.vt1.UpdatGraphic(1, 1, color: colors[idColors[currentColorId]]);
                            currentColorId++;
                            if (currentColorId > 2) stage++;
                            break;
                        case 2: stage++; break;
                        case 3:
                            AnimationsGUI.vt1.UpdatGraphic(1, 1, color: colors[idColors[currentColorId]]);
                            currentColorId++;
                            if (currentColorId > 5) stage++;
                            break;
                        case 4: stage++; break;
                    }
                    Now = DateTime.Now;
                }
            }
            AnimationsGUI.vt1.ClearList();
        }

        public static void SlideEffect(int graphicID, string effectColor = "purple", double middleTime = 2000)
        {
            DateTime Now = DateTime.Now;
            double[] sleep = new double[] { 30, 2000, 30 };
            int[] idColors = new int[] { };
            ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
            switch (effectColor)
            {
                case "purple": idColors = new int[] { 0, 0, 5, 13, 5, 0, 0 }; break;
                case "red": idColors = new int[]    { 0, 0, 4, 12, 4, 0, 0 }; break;
                case "yellow": idColors = new int[] { 0, 0, 6, 14, 6, 0, 0 }; break;
                case "green": idColors = new int[]  { 0, 0, 2, 10, 2, 0, 0 }; break;
                case "blue": idColors = new int[]   { 0, 1, 3, 11, 3, 1, 0 }; break;
                case "white": idColors = new int[]  { 0, 8, 7, 15, 7, 8, 0 }; break;
            }
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
                    switch(stage)
                    {
                        case 0:
                            AnimationsGUI.Part(graphicID, column, columns, colors[idColors[currentColorId]]);
                            column++;
                            if (column>=0 && column % 2 == 0) currentColorId++;
                            if (currentColorId > 6) currentColorId = 6;
                            if (column == (columns + 1) / 2 + 1) stage++;
                            break;
                        case 1: stage++; currentColorId--; break;
                        case 2:
                            AnimationsGUI.Part(graphicID, column, columns, colors[idColors[currentColorId]]);
                            column++;
                            if (column % 2 == 1) currentColorId++;
                            if (currentColorId > 6) currentColorId = 6;
                            if (column == columns + extraColumn + 2) stage++;
                            break;
                    }
                    Now = DateTime.Now;
                }
            }
            AnimationsGUI.vt1.ClearList();
        }
    }
}
