using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FarmConsole.Body.Views
{
    public class SideMenuView : ComponentEngine
    {
        private static List<CM> LeftMenu;
        private static List<CM> RightMenu;

        public static Point DisplayRightMenu(List<ProductModel> component_list, string title, int selected, bool extended = false, bool search = false)
        {
            ClearList(false);

            int searchHeight = search ? 3 : 0;
            int Width = Console.WindowWidth / 5 - 1;
            int Height = component_list.Count == 0 ? 6 : component_list.Count * 3 + searchHeight > Console.WindowHeight - 14 ?
                Console.WindowHeight - 9 : component_list.Count * 3 + 5 + searchHeight;
            
            Endl(Console.WindowHeight - Height - 4);
            GroupStart(5);

            RightBar(Height);
            GraphicBox(new string[] { " " });

            GroupStart(5);
            GraphicBox(new string[] { title.ToUpper(), "" }, color: ConsoleColor.Gray);
            if (component_list.Count > 0)
            {
                if(search) TextBox(XF.GetString("search"), Console.WindowWidth / 5 - 1, margin: 0, foreground: ConsoleColor.Yellow);
                for (int i = 0; i < component_list.Count; i++)
                    if (i + searchHeight / 3 < (Console.WindowHeight - 14) / 3)
                        TextBox(component_list[i].Amount + "x " + component_list[i].ProductName, Console.WindowWidth / 5 - 1, margin: 0);
                    else TextBox(component_list[i].Amount + "x " + component_list[i].ProductName, Console.WindowWidth / 5 - 1, false, margin: 0);
            }
            else GraphicBox(new string[] { "NIC TU NIE MA" });
            GroupEnd();

            GroupStart(9, 10);
            Endl(Height - 2);
            if (extended) TextBox("WRÓĆ Q", Console.WindowWidth / 10 - 1, background: ConsoleColor.Gray, margin: 0);
            else TextBox("WTBIERZ Q", Console.WindowWidth / 10 - 1, background: ConsoleColor.Gray, margin: 0);
            GroupEnd();

            GroupStart(10, 10);
            Endl(Height - 2);
            if (extended) TextBox("WYBIERZ E", Console.WindowWidth / 10 - 1, background: ConsoleColor.Gray, margin: 0);
            else TextBox("WYJDŹ E", Console.WindowWidth / 10 - 1, background: ConsoleColor.Gray, margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();
            UpdateSelect(selected, selected, component_list.Count - 5);

            RightMenu = ComponentList.ToList();

            return new Point(Width + 1, Height + 2);
        }
        public static Point DisplayLeftMenu(string[] component_list, string title, int selected, bool extended = false)
        {
            ClearList(false);
            Endl(5);
            GroupStart(0);

            int Width = Console.WindowWidth / 5 - 1;
            int Height = component_list.Length == 0 ? 6 : component_list.Length * 3 > Console.WindowHeight - 14 ?
                Console.WindowHeight - 14 : component_list.Length * 3 + 5;

            LeftBar(Height);
            GroupStart(1);
            GraphicBox(new string[] { title.ToUpper(), "" }, color: ConsoleColor.Gray);
            if (component_list.Length > 0)
            {
                for (int i = 0; i < component_list.Length; i++)
                if (i < (Console.WindowHeight - 14) / 3)
                    TextBox(component_list[i], Width, margin: 0);
                else TextBox(component_list[i], Width, false, margin: 0);
            }
            else GraphicBox(new string[] { "NIC Z TYM NIE ZROBISZ" });
            GroupEnd();

            GroupStart(1, 10);
            Endl(Height - 2);
            if (extended) TextBox("WYKONAJ Q", Console.WindowWidth / 10 - 1, background: ConsoleColor.Gray, margin: 0);
            else TextBox("WYJDŹ Q", Console.WindowWidth / 10 - 1, background: ConsoleColor.Gray, margin: 0);
            GroupEnd();

            GroupStart(2, 10);
            Endl(Height - 2);
            if (extended) TextBox("WRÓĆ E", Console.WindowWidth / 10 - 1, background: ConsoleColor.Gray, margin: 0);
            else TextBox("WYKONAJ E", Console.WindowWidth / 10 - 1, background: ConsoleColor.Gray, margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();
            UpdateSelect(selected, selected, component_list.Length);

            LeftMenu = ComponentList.ToList();

            return new Point(Width, Height + 2);
        }
        public static Point DisplayCenterMenu(string content)
        {
            ClearList(false);

            string[] text, text2;
            int Width = 40;

            text = new string[] { " ", "OSTRZEZENIE!", " " };
            text2 = TextBoxView(content, Width);
            text = text.Concat(text2.Concat(new string[] { " " }).ToArray()).ToArray();
            
            int Height = text2.Length + 6;

            Endl((Console.WindowHeight - Height) / 2);
            GroupStart(3);
            TextBoxLines(text, Width, color1: ConsoleColor.Red);
            GroupEnd();
            PrintList();

            return new Point(Width, Height + 2);
        }
        public static Point DisplaySearchRightMenu(List<ProductModel> component_list, string title, string SearchedPhrase)
        {
            ClearList(false);

            int Width = Console.WindowWidth / 5 - 1;
            int Height = component_list.Count == 0 ? 10 : component_list.Count * 3 + 3 > Console.WindowHeight - 14 ?
                Console.WindowHeight - 9 : component_list.Count * 3 + 5 + 3;

            Endl(Console.WindowHeight - Height - 4);
            GroupStart(5);

            RightBar(Height);
            GraphicBox(new string[] { "".PadRight(Console.WindowWidth / 5 - 1) });

            GroupStart(5);
            GraphicBox(new string[] { title.ToUpper(), "" }, color: ConsoleColor.Gray);
            TextBox(SearchedPhrase, Width, margin: 0, foreground: ConsoleColor.Yellow, background: ConsoleColor.Yellow);
            if (component_list.Count > 0)
            {
                for (int i = 0; i < component_list.Count; i++)
                    if (i + 1 < (Console.WindowHeight - 14) / 3)
                        TextBox(component_list[i].Amount + "x " + component_list[i].ProductName, Width, margin: 0);
                    else TextBox(component_list[i].Amount + "x " + component_list[i].ProductName, Width, false, margin: 0);
            }
            else { Endl(1); GraphicBox(new string[] { "NIC TU NIE MA" }); }
            GroupEnd();

            GroupStart(5);
            Endl(Height - 2);
            TextBox("ZAKOŃCZ FILTR Q", Console.WindowWidth / 5 - 5, background: ConsoleColor.Gray, margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();

            return new Point(Width + 1, Height + 1);
        }
        public static void RestoreLeftMenu() => ComponentList = LeftMenu.ToList();
        public static void RestoreRightMenu() => ComponentList = RightMenu.ToList();


        public static void Clean()
        {
            ClearList(false);
        }
    }
}
