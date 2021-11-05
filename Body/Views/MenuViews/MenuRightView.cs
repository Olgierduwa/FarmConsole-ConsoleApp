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
        public static void Display(List<ProductModel> component_list, int selected, bool extended = false, bool print = true, bool search = false)
        {
            ClearList(false);
            Endl(6);
            GroupStart(5);
            RightBar();
            GraphicBox(new string[] { " " });

            GroupStart(5);
            GraphicBox(new string[] { "Ekwipunek", "" }, color: ConsoleColor.Gray);
            if (component_list.Count > 0)
            {
                if(search) TextBox(XF.GetString("search"), Console.WindowWidth / 5 - 1, margin: 0, foreground: ConsoleColor.Yellow);
                for (int i = 0; i < component_list.Count; i++)
                    if (i < (Console.WindowHeight - 13) / 3)
                        TextBox(component_list[i].Amount + "x " + component_list[i].ProductName, Console.WindowWidth / 5 - 1, margin: 0);
                    else TextBox(component_list[i].Amount + "x " + component_list[i].ProductName, Console.WindowWidth / 5 - 1, false, margin: 0);
            }
            else GraphicBox(new string[] { "NIC TU NIE MA" });
            GroupEnd();

            GroupStart(9, 10);
            Endl(Console.WindowHeight - 12);
            if (extended) TextBox("WRÓĆ Q", Console.WindowWidth / 10 - 1, background: ConsoleColor.Gray, margin: 0);
            else TextBox("WTBIERZ Q", Console.WindowWidth / 10 - 1, background: ConsoleColor.Gray, margin: 0);
            GroupEnd();

            GroupStart(10, 10);
            Endl(Console.WindowHeight - 12);
            if (extended) TextBox("WYBIERZ E", Console.WindowWidth / 10 - 1, background: ConsoleColor.Gray, margin: 0);
            else TextBox("WYJDŹ E", Console.WindowWidth / 10 - 1, background: ConsoleColor.Gray, margin: 0);
            GroupEnd();

            GroupEnd();
            if (print) PrintList();
            UpdateSelect(selected, selected, component_list.Count);
        }

        public static void Search(List<ProductModel> component_list, string SearchedPhrase)
        {
            ClearList(false);
            Endl(6);
            GroupStart(5);
            RightBar();
            GraphicBox(new string[] { "".PadRight(Console.WindowWidth / 5 - 1) });

            GroupStart(5);
            GraphicBox(new string[] { "Ekwipunek", "" }, color: ConsoleColor.Gray);
            TextBox(SearchedPhrase, Console.WindowWidth / 5 - 1, margin: 0, foreground: ConsoleColor.Yellow, background: ConsoleColor.Yellow);
            if (component_list.Count > 0)
            {
                for (int i = 0; i < component_list.Count; i++)
                    if (i < (Console.WindowHeight - 13) / 3)
                        TextBox(component_list[i].Amount + "x " + component_list[i].ProductName, Console.WindowWidth / 5 - 1, margin: 0);
                    else TextBox(component_list[i].Amount + "x " + component_list[i].ProductName, Console.WindowWidth / 5 - 1, false, margin: 0);
            }
            else { Endl(1); GraphicBox(new string[] { "NIC TU NIE MA" }); }
            GroupEnd();

            GroupStart(5);
            Endl(Console.WindowHeight - 12);
            TextBox("ZAKOŃCZ FILTR Q", Console.WindowWidth / 5 - 5, background: ConsoleColor.Gray, margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();
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
