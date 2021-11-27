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

        public static CM DisplayRightMenu(List<ProductModel> component_list, string title, int selected, bool extended = false, bool search = false)
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
            GraphicBox(new string[] { title.ToUpper(), "" }, color: ColorService.GetColorByName("Gray"));
            if (component_list.Count > 0)
            {
                if(search) TextBox(StringService.Get("search"), Console.WindowWidth / 5 - 1, margin: 0, foreground: ColorService.GetColorByName("Yellow"));
                for (int i = 0; i < component_list.Count; i++)
                    if (i + searchHeight / 3 < (Console.WindowHeight - 14) / 3)
                        TextBox(component_list[i].Amount + "x " + component_list[i].ProductName, Console.WindowWidth / 5 - 1, margin: 0);
                    else TextBox(component_list[i].Amount + "x " + component_list[i].ProductName, Console.WindowWidth / 5 - 1, false, margin: 0);
            }
            else GraphicBox(new string[] { StringService.Get("nothing here") });
            GroupEnd();

            GroupStart(9, 10);
            Endl(Height - 2);
            if (extended) TextBox(StringService.Get("go back", " Q"), Console.WindowWidth / 10 - 1, background: ColorService.GetColorByName("Gray"), margin: 0);
            else TextBox(StringService.Get("choose", " Q"), Console.WindowWidth / 10 - 1, background: ColorService.GetColorByName("Gray"), margin: 0);
            GroupEnd();

            GroupStart(10, 10);
            Endl(Height - 2);
            if (extended) TextBox(StringService.Get("choose", " E"), Console.WindowWidth / 10 - 1, background: ColorService.GetColorByName("Gray"), margin: 0);
            else TextBox(StringService.Get("escape", " E"), Console.WindowWidth / 10 - 1, background: ColorService.GetColorByName("Gray"), margin: 0);
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
            int Height = component_list.Length == 0 ? 6 : component_list.Length * 3 > Console.WindowHeight - 14 ?
                Console.WindowHeight - 14 : component_list.Length * 3 + 5;

            LeftBar(Height);
            GroupStart(1);
            GraphicBox(new string[] { title.ToUpper(), "" }, color: ColorService.GetColorByName("Gray"));
            if (component_list.Length > 0)
            {
                for (int i = 0; i < component_list.Length; i++)
                if (i < (Console.WindowHeight - 14) / 3)
                    TextBox(StringService.Get(component_list[i]), Width, margin: 0);
                else TextBox(StringService.Get(component_list[i]), Width, false, margin: 0);
            }
            else GraphicBox(new string[] { StringService.Get("cant do anything") });
            GroupEnd();

            GroupStart(1, 10);
            Endl(Height - 2);
            if (extended) TextBox(StringService.Get("do", " Q"), Console.WindowWidth / 10 - 1, background: ColorService.GetColorByName("Gray"), margin: 0);
            else TextBox(StringService.Get("escape", " Q"), Console.WindowWidth / 10 - 1, background: ColorService.GetColorByName("Gray"), margin: 0);
            GroupEnd();

            GroupStart(2, 10);
            Endl(Height - 2);
            if (extended) TextBox(StringService.Get("go back", " E"), Console.WindowWidth / 10 - 1, background: ColorService.GetColorByName("Gray"), margin: 0);
            else TextBox(StringService.Get("do", " E"), Console.WindowWidth / 10 - 1, background: ColorService.GetColorByName("Gray"), margin: 0);
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
            int Height = component_list.Count == 0 ? 10 : component_list.Count * 3 + 3 > Console.WindowHeight - 14 ?
                Console.WindowHeight - 9 : component_list.Count * 3 + 5 + 3;

            Endl(Console.WindowHeight - Height - 4);
            GroupStart(5);

            RightBar(Height);
            GraphicBox(new string[] { "".PadRight(Console.WindowWidth / 5 - 1) });

            GroupStart(5);
            GraphicBox(new string[] { title.ToUpper(), "" }, color: ColorService.GetColorByName("Gray"));
            TextBox(SearchedPhrase, Width, margin: 0, foreground: ColorService.GetColorByName("Yellow"), background: ColorService.GetColorByName("Yellow"));
            if (component_list.Count > 0)
            {
                for (int i = 0; i < component_list.Count; i++)
                    if (i + 1 < (Console.WindowHeight - 14) / 3)
                        TextBox(component_list[i].Amount + "x " + component_list[i].ProductName, Width, margin: 0);
                    else TextBox(component_list[i].Amount + "x " + component_list[i].ProductName, Width, false, margin: 0);
            }
            else { Endl(1); GraphicBox(new string[] { StringService.Get("nothing here") }); }
            GroupEnd();

            GroupStart(5);
            Endl(Height - 2);
            TextBox(StringService.Get("finish filtering", " Q"), Console.WindowWidth / 5 - 5, background: ColorService.GetColorByName("Gray"), margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();

            return GetComponentByName("RB");
        }
        public static void RestoreLeftMenu() => ComponentList = LeftMenu.ToList();
        public static void RestoreRightMenu() => ComponentList = RightMenu.ToList();
    }
}
