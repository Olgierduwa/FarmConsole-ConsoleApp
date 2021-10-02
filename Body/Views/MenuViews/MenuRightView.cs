using FarmConsole.Body.Models;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    public class MenuRightView : MainViewService
    {
        public static void Show(List<ProductModel> component_list, int selected, bool extended = false)
        {
            ClearList(false);
            Endl(6);
            GroupStart(0);

            GroupStart(5);
            RightBar();
            GroupEnd();

            GroupStart(9, 10);
            Endl(Console.WindowHeight - 12);
            if (extended) TextBox("WRÓĆ Q", Console.WindowWidth / 10 - 1, color1: ConsoleColor.Gray, margin: 0);
            else TextBox("WTBIERZ Q", Console.WindowWidth / 10 - 1, color1: ConsoleColor.Gray, margin: 0);
            GroupEnd();

            GroupStart(10, 10);
            Endl(Console.WindowHeight - 12);
            if (extended) TextBox("WYBIERZ E", Console.WindowWidth / 10 - 1, color1: ConsoleColor.Gray, margin: 0);
            else TextBox("WYJDŹ E", Console.WindowWidth / 10 - 1, color1: ConsoleColor.Gray, margin: 0);
            GroupEnd();

            if (component_list.Count == 0)
            {
                GroupStart(5);
                Endl(3);
                GraphicBox(new string[] { "NIC TU NIE MA" });
                GroupEnd();
            }

            GroupStart(5);
            GraphicBox(new string[] { "Ekwipunek", "", }, color: ConsoleColor.Gray);
            for (int i = 0; i < component_list.Count; i++)
                if (i < (Console.WindowHeight - 13) / 3)
                    TextBox(component_list[i].Amount + "x " + component_list[i].ProductName, Console.WindowWidth / 5 - 1, margin: 0);
                else TextBox(component_list[i].Amount + "x " + component_list[i].ProductName, Console.WindowWidth / 5 - 1, false, margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();
            UpdateSelect(selected, selected, component_list.Count, 5);
        }

        public static void Clean()
        {
            ClearList(false);
            Endl(6);
            GroupStart(5);
            RightBar(extra: 1);
            GroupEnd();
            ClearList();
        }
    }
}
