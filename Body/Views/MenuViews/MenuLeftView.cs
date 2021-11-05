using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    public class MenuLeftView : MainViewService
    {
        public static void Display(string[] component_list, string Title, int selected, bool extended = false, bool print = true)
        {
            ClearList(false);
            Endl(6);
            GroupStart(0);
            LeftBar();

            GroupStart(1);
            GraphicBox(new string[] { Title, "" }, color: ConsoleColor.Gray);
            for (int i = 0; i < component_list.Length; i++)
                if (i < (Console.WindowHeight - 13) / 3)
                    TextBox(component_list[i], Console.WindowWidth / 5 - 1, margin: 0);
                else TextBox(component_list[i], Console.WindowWidth / 5 - 1, false, margin: 0);
            GroupEnd();

            GroupStart(1, 10);
            Endl(Console.WindowHeight - 12);
            if (extended) TextBox("WYKONAJ Q", Console.WindowWidth / 10 - 1, background: ConsoleColor.Gray, margin: 0);
            else TextBox("WYJDŹ Q", Console.WindowWidth / 10 - 1, background: ConsoleColor.Gray, margin: 0);
            GroupEnd();

            GroupStart(2, 10);
            Endl(Console.WindowHeight - 12);
            if (extended) TextBox("WRÓĆ E", Console.WindowWidth / 10 - 1, background: ConsoleColor.Gray, margin: 0);
            else TextBox("WYKONAJ E", Console.WindowWidth / 10 - 1, background: ConsoleColor.Gray, margin: 0);
            GroupEnd();

            GroupEnd();
            if(print) PrintList();
            UpdateSelect(selected, selected, component_list.Length);
        }

        public static void Clean()
        {
            ClearList(false);
            Endl(6);
            GroupStart(1);
            LeftBar(extra: 1);
            GroupEnd();
            ClearList();
        }
    }
}
