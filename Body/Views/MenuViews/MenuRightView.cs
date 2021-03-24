using FarmConsole.Body.Models;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    public class MenuRightView : MenuViewService
    {
        public static void Show(List<ProductModel>[] component_list, int category, int selected, bool extended = false, bool refresh = false)
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

            if (component_list[category].Count == 0)
            {
                GroupStart(5);
                Endl(3);
                GraphicBox(new string[] { "NIC TU NIE MA" });
                GroupEnd();
            }

            GroupStart(5);
            GraphicBox(new string[] { XF.GetProductCategoryName()[category], "", }, color: ConsoleColor.Gray);
            for (int i = 0; i < component_list[category].Count; i++)
                if (i < (Console.WindowHeight - 13) / 3)
                    TextBox(component_list[category][i].amount + "x " + component_list[category][i].name, Console.WindowWidth / 5 - 1, margin: 0);
                else TextBox(component_list[category][i].amount + "x " + component_list[category][i].name, Console.WindowWidth / 5 - 1, false, margin: 0);
            GroupEnd();

            GroupEnd();
            if (!refresh) PrintList();
            UpdateSelect(selected, selected, component_list[category].Count, 5);
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
