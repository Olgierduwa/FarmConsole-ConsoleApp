using FarmConsole.Body.Engines;
using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class AnimationView : ComponentService
    {
        public static void DisplayGraphic(string[] GraphicView, int column, int columns, int row = 5, Color color = new Color(), bool cleaning = false)
        {
            color = color == new Color() ? ColorService.GetColorByName("Black") : color;
            Clean(cleaning);
            Endl((Console.WindowHeight - GraphicView.Length) * row / 10 - 1);
            GroupStart(column, columns);
            Endl(1);
            GraphicBox(GraphicView, color: color);
            GroupEnd(columns);
            PrintList();
        }
        public static void PopUp(string[] text, Color color)
        {
            Clean();
            Endl(Console.WindowHeight - text.Length - 3);
            GroupStart(2);
            GraphicBox(text, color: color);
            GroupEnd();
            PrintList();
        }

        internal static void Clean(bool cleaning = true)
        {
            ClearList(cleaning);
        }
    }
}
