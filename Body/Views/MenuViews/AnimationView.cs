using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    public class AnimationView : MenuViewService
    {
        public static void Centrum(int graphicID)
        {
            Endl((Console.WindowHeight - XF.GetGraphic(graphicID).Length) / 2 - 1);
            GroupStart(3);
            Endl(1);
            GraphicBox(XF.GetGraphic(graphicID), color: ConsoleColor.Black);
            GroupEnd();
        }
        public static void Column(int graphicID, int column, int columns, ConsoleColor color = ConsoleColor.Black)
        {
            ClearList();
            Endl((Console.WindowHeight - XF.GetGraphic(graphicID).Length) / 2 - 1);
            GroupStart(column, columns);
            Endl(1);
            GraphicBox(XF.GetGraphic(graphicID), color: color);
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
