using FarmConsole.Body.Model.Helpers;
using FarmConsole.Body.View.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.View.GUI
{
    public static class AnimationsGUI
    {
        public static ViewTools vt1 = new ViewTools();
        public static void Centrum(int graphicID)
        {
            vt1.Endl((Console.WindowHeight - XF.GetGraphic(graphicID).Length) / 2 - 1);
            vt1.GroupStart(3);
            vt1.Endl(1);
            vt1.Graphic(XF.GetGraphic(graphicID), color: ConsoleColor.Black);
            vt1.GroupEnd();
        }
        public static void Part(int graphicID, int column, int columns, ConsoleColor color = ConsoleColor.Black)
        {
            vt1.ClearList();
            vt1.Endl((Console.WindowHeight - XF.GetGraphic(graphicID).Length) / 2 - 1);
            vt1.GroupStart(column, columns);
            vt1.Endl(1);
            vt1.Graphic(XF.GetGraphic(graphicID), color: color);
            vt1.GroupEnd(columns);
            vt1.PrintList();
        }
    }
}
