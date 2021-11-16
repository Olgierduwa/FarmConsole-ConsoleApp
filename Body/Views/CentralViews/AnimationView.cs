using FarmConsole.Body.Engines;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views
{
    public class AnimationView : ComponentEngine
    {
        public static void Centrum(string[] GraphicView)
        {
            ClearList(false);
            Endl((Console.WindowHeight - GraphicView.Length) / 2 - 1);
            GroupStart(3);
            Endl(1);
            GraphicBox(GraphicView, color: ConsoleColor.Black);
            GroupEnd();
        }
        public static void DisplayGraphic(string[] GraphicView, int column, int columns, int row = 5, ConsoleColor color = ConsoleColor.Black, bool cleaning = false)
        {
            ClearList(cleaning);
            Endl((Console.WindowHeight - GraphicView.Length) * row / 10 - 1);
            GroupStart(column, columns);
            Endl(1);
            GraphicBox(GraphicView, color: color);
            GroupEnd(columns);
            PrintList();
        }
        public static void PopUp(string[] text, ConsoleColor color)
        {
            ClearList();
            Endl(Console.WindowHeight - text.Length - 3);
            GroupStart(2);
            GraphicBox(text, color: color);
            GroupEnd();
            PrintList();
        }
    }
}
