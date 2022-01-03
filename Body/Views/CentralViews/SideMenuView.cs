using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FarmConsole.Body.Views
{
    class SideMenuView : MenuManager
    {
        private static List<CM> LeftMenu;
        private static List<CM> RightMenu;
        private static readonly int MinWidth = 30;

        public static CM DisplayRightMenu(List<ProductModel> component_list, string title, int selected, bool extended = false, bool search = false)
        {
            ClearList(false);

            int searchHeight = search ? 3 : 0;
            int Width = Console.WindowWidth / 5 - 1;
            Width = Width < MinWidth ? MinWidth : Width;
            int Height = component_list.Count == 0 ? 6 : component_list.Count * 3 + searchHeight > Console.WindowHeight - 13 ?
                Console.WindowHeight - 9 : component_list.Count * 3 + 4 + searchHeight;
            
            Endl(Console.WindowHeight - Height - 4);
            GroupStart(Console.WindowWidth - Width / 2, Console.WindowWidth);
            RightBar(Height, Width);
            GraphicBox(new string[] { " " });

            GroupStart(Console.WindowWidth - Width / 2, Console.WindowWidth);
            GraphicBox(new string[] { title.ToUpper(), "" }, color: ColorService.GetColorByName("gray3"));
            if (component_list.Count > 0)
            {
                if(search) TextBox(StringService.Get("search"), Width, margin: 0, foreground: ColorService.GetColorByName("Yellow"));
                for (int i = 0; i < component_list.Count; i++)
                    if (i + searchHeight / 3 < (Console.WindowHeight - 13) / 3)
                        TextBox(component_list[i].Amount + "x " + component_list[i].ObjectName, Width, margin: 0);
                    else TextBox(component_list[i].Amount + "x " + component_list[i].ObjectName, Width, false, margin: 0);
            }
            else GraphicBox(new string[] { StringService.Get("nothing here") });
            GroupEnd();

            GroupStart(Console.WindowWidth - Width * 3 / 4, Console.WindowWidth);
            Endl(Height - 2);
            if (extended) TextBox(StringService.Get("go back", " Q"), Width / 2 - 2, background: ColorService.GetColorByName("gray3"), margin: 0);
            else TextBox(StringService.Get("choose", " Q"), Width / 2 - 2, background: ColorService.GetColorByName("gray3"), margin: 0);
            GroupEnd();

            GroupStart(Console.WindowWidth - Width / 4 - 1, Console.WindowWidth);
            Endl(Height - 2);
            if (extended) TextBox(StringService.Get("choose", " E"), Width / 2 - 1, background: ColorService.GetColorByName("gray3"), margin: 0);
            else TextBox(StringService.Get("escape", " E"), Width / 2 - 1, background: ColorService.GetColorByName("gray3"), margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();
            UpdateMenuSelect(selected, selected, component_list.Count);

            RightMenu = ComponentList.ToList();

            return GetComponentByName("RB");
        }
        public static CM DisplayLeftMenu(string[] component_list, string title, int selected, bool extended = false)
        {
            ClearList(false);
            Endl(5);
            GroupStart(0);

            int Width = Console.WindowWidth / 5 - 1;
            Width = Width < MinWidth ? MinWidth : Width;
            int Height = component_list.Length == 0 ? 6 : component_list.Length * 3 > Console.WindowHeight - 13 ?
                Console.WindowHeight - 13 : component_list.Length * 3 + 5;

            LeftBar(Height, Width);
            GroupStart(Width/2, Console.WindowWidth);
            GraphicBox(new string[] { title.ToUpper(), "" }, color: ColorService.GetColorByName("gray3"));
            if (component_list.Length > 0)
            {
                for (int i = 0; i < component_list.Length; i++)
                if (i < (Console.WindowHeight - 13) / 3)
                    TextBox(StringService.Get(component_list[i]), Width, margin: 0);
                else TextBox(StringService.Get(component_list[i]), Width, false, margin: 0);
            }
            else GraphicBox(new string[] { StringService.Get("cant do anything") });
            GroupEnd();

            GroupStart(Width / 4 + 1, Console.WindowWidth);
            Endl(Height - 2);
            if (extended) TextBox(StringService.Get("do", " Q"), Width / 2 - 2, background: ColorService.GetColorByName("gray3"), margin: 0);
            else TextBox(StringService.Get("escape", " Q"), Width / 2 - 2, background: ColorService.GetColorByName("gray3"), margin: 0);
            GroupEnd();

            GroupStart(Width * 3 / 4, Console.WindowWidth);
            Endl(Height - 2);
            if (extended) TextBox(StringService.Get("go back", " E"), Width / 2 - 1, background: ColorService.GetColorByName("gray3"), margin: 0);
            else TextBox(StringService.Get("do", " E"), Width / 2 - 1, background: ColorService.GetColorByName("gray3"), margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();
            UpdateMenuSelect(selected, selected, component_list.Length);

            LeftMenu = ComponentList.ToList();

            return GetComponentByName("LB");
        }
        public static CM DisplayCenterMenu(string content)
        {
            ClearList(false);

            string[] text, text2;
            int Width = 40;

            text = new string[] { " ", StringService.Get("warning"), " " };
            text2 = TextBoxView(content, Width);
            text = text.Concat(text2.Concat(new string[] { " " }).ToArray()).ToArray();
            
            int Height = text2.Length + 6;

            Endl((Console.WindowHeight - Height) / 2);
            GroupStart(3);
            TextBoxLines(text, Width, color1: ColorService.GetColorByName("Red"));
            GroupEnd();
            PrintList();

            return GetComponentByName("TBL");
        }
        public static CM DisplaySearchRightMenu(List<ProductModel> component_list, string title, string SearchedPhrase)
        {
            ClearList(false);

            int Width = Console.WindowWidth / 5 - 1;
            Width = Width < MinWidth ? MinWidth : Width;
            int Height = component_list.Count == 0 ? 10 : component_list.Count * 3 + 3 > Console.WindowHeight - 13 ?
                Console.WindowHeight - 9 : component_list.Count * 3 + 7;

            Endl(Console.WindowHeight - Height - 4);
            GroupStart(Console.WindowWidth - Width / 2, Console.WindowWidth);
            RightBar(Height, Width);
            GraphicBox(new string[] { "".PadRight(Width) });

            GroupStart(Console.WindowWidth - Width/2, Console.WindowWidth);
            GraphicBox(new string[] { title.ToUpper(), "" }, color: ColorService.GetColorByName("gray3"));
            TextBox(SearchedPhrase, Width, margin: 0, foreground: ColorService.GetColorByName("Yellow"), background: ColorService.GetColorByName("Yellow"));
            if (component_list.Count > 0)
            {
                for (int i = 0; i < component_list.Count; i++)
                    if (i + 1 < (Console.WindowHeight - 13) / 3)
                        TextBox(component_list[i].Amount + "x " + component_list[i].ObjectName, Width, margin: 0);
                    else TextBox(component_list[i].Amount + "x " + component_list[i].ObjectName, Width, false, margin: 0);
            }
            else { Endl(1); GraphicBox(new string[] { StringService.Get("nothing here") }); }
            GroupEnd();

            GroupStart(Console.WindowWidth - Width / 2, Console.WindowWidth);
            Endl(Height - 2);
            TextBox(StringService.Get("finish filtering", " Q"), Width - 6, background: ColorService.GetColorByName("gray3"), margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();

            return GetComponentByName("RB");
        }
        public static void RestoreLeftMenu() => ComponentList = LeftMenu.ToList();
        public static void RestoreRightMenu() => ComponentList = RightMenu.ToList();
    }
}
