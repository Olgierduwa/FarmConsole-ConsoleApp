using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FarmConsole.Body.Views.GameViews
{
    class SideMenuView : ComponentService
    {
        private static List<CM> LeftMenu;
        private static List<CM> RightMenu;
        private static readonly int MinWidth = 32;
        public static CM DisplayLeftMenu(string[] component_list, string title, int Selected, bool extended = false)
        {
            ClearList(false);
            Endl(5);
            GroupStart(0);

            int Width = Console.WindowWidth / 5 - 1;
            Width = Width < MinWidth ? MinWidth : Width;
            int Height = component_list.Length == 0 ? 1 :
                component_list.Length * 3 > Console.WindowHeight - 14 ?
                (Console.WindowHeight - 14) / 3 : component_list.Length;

            LeftBar(Height * 3 + 5, Width);
            GroupStart(Width / 2, Console.WindowWidth);
            GraphicBox(new string[] { title.ToUpper(), "" }, color: ColorService.GetColorByName("gray3"));
            if (component_list.Length > 0)
            {
                for (int i = 0; i < component_list.Length; i++)
                    if (i < Height)
                        TextBox(LS.Action(component_list[i]), Width, margin: 0);
                    else TextBox(LS.Action(component_list[i]), Width, false, margin: 0);
            }
            else TextBox(LS.Action("cant do anything"), Width, background: ColorService.BackgroundColor, foreground: ColorService.GetColorByName("gray3"));
            GroupEnd();

            GroupStart(Width / 4 + 1, Console.WindowWidth);
            Endl(Height * 3 + 3);
            if (extended) TextBox(LS.Navigation("do", " [Q]"), (Width - 1) / 2, background: ColorService.GetColorByName("gray3"), margin: 0);
            else TextBox(LS.Navigation("escape", " [Q]"), (Width - 1) / 2, background: ColorService.GetColorByName("gray3"), margin: 0);
            GroupEnd();

            GroupStart(Width * 3 / 4, Console.WindowWidth);
            Endl(Height * 3 + 3);
            if (extended) TextBox(LS.Navigation("go back", " [E]"), (Width - 1) / 2, background: ColorService.GetColorByName("gray3"), margin: 0);
            else TextBox(LS.Navigation("do", " [E]"), (Width - 1) / 2, background: ColorService.GetColorByName("gray3"), margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();
            UpdateMenuSelect(Selected, Selected, component_list.Length);

            LeftMenu = ComponentList.ToList();

            return GetComponentByName("LB");
        }
        public static CM DisplayCenterMenu(string content, string title)
        {
            ClearList(false);

            string[] text, text2;
            int Width = 40;

            Color color = title == "" ? ColorService.GetColorByName("Red") : ColorService.GetColorByName("Cyan");
            title = title == "" ? LS.Navigation("warning") : LS.Object(title);
            text = new string[] { " ", title, " " };
            if (content.Split('\n').Length > 1) text2 = content.Split('\n');
            else text2 = TextBoxView(content, Width);
            text = text.Concat(text2.Concat(new string[] { " " }).ToArray()).ToArray();

            int Height = text2.Length + 6;

            Endl((Console.WindowHeight - Height) / 2);
            GroupStart(3);
            TextBoxLines(text, Width, color1: color);
            GroupEnd();
            PrintList();

            return GetComponentByName("TBL");
        }
        public static CM DisplayRightMenu(List<ProductModel> component_list, string title, int Selected, bool extended = false, string filter = "")
        {
            ClearList(false);

            int Width = Console.WindowWidth / 5 - 1;
            Width = Width < MinWidth ? MinWidth : Width;
            int Height = component_list.Count == 0 ? 2 :
                component_list.Count * 3 + 3 > Console.WindowHeight - 14 ?
                (Console.WindowHeight - 14) / 3 : component_list.Count + 1;

            Endl(Console.WindowHeight - Height * 3 - 9);
            GroupStart(Console.WindowWidth - Width / 2, Console.WindowWidth);
            RightBar(Height * 3 + 5, Width);
            GraphicBox(new string[] { "".PadRight(Width) });

            GroupStart(Console.WindowWidth - Width / 2, Console.WindowWidth);
            GraphicBox(new string[] { title.ToUpper(), "" }, color: ColorService.GetColorByName("gray3"));
            string SearchText = filter != "" ? filter : LS.Navigation("search");
            TextBox(SearchText, Width, margin: 0, foreground: ColorService.GetColorByName("Yellow"));
            if (component_list.Count > 0)
            {
                for (int i = 0; i < component_list.Count; i++)
                    if (i * 3 + 3 < Height * 3)
                        TextBox(component_list[i].Amount + "x " + LS.Object(component_list[i].ObjectName), Width, margin: 0);
                    else TextBox(component_list[i].Amount + "x " + LS.Object(component_list[i].ObjectName), Width, false, margin: 0);
            }
            else TextBox(LS.Action("nothing here"), Width, background: ColorService.BackgroundColor, foreground: ColorService.GetColorByName("gray3"));
            GroupEnd();

            if (filter != "")
            {
                GroupStart(Console.WindowWidth - Width / 2, Console.WindowWidth);
                Endl(Height * 3 + 3);
                TextBox(LS.Navigation("finish filtering", " [TAB]"), Width - 6, background: ColorService.GetColorByName("gray3"), margin: 0);
                GroupEnd();
            }
            else
            {
                GroupStart(Console.WindowWidth - Width * 3 / 4 + 1, Console.WindowWidth);
                Endl(Height * 3 + 3);
                if (extended) TextBox(LS.Navigation("go back", " [Q]"), (Width - 1) / 2, background: ColorService.GetColorByName("gray3"), margin: 0);
                else TextBox(LS.Navigation("choose", " [Q]"), (Width - 1) / 2, background: ColorService.GetColorByName("gray3"), margin: 0);
                GroupEnd();

                GroupStart(Console.WindowWidth - Width / 4, Console.WindowWidth);
                Endl(Height * 3 + 3);
                if (extended) TextBox(LS.Navigation("choose", " [E]"), (Width - 1) / 2, background: ColorService.GetColorByName("gray3"), margin: 0);
                else TextBox(LS.Navigation("escape", " [E]"), (Width - 1) / 2, background: ColorService.GetColorByName("gray3"), margin: 0);
                GroupEnd();
            }

            GroupEnd();
            PrintList();
            UpdateMenuSelect(Selected, Selected, component_list.Count + 1, Prop: 14);
            if (filter == "") RightMenu = ComponentList.ToList();

            return GetComponentByName("RB");
        }
        public static void RestoreLeftMenu() => ComponentList = LeftMenu.ToList();
        public static void RestoreRightMenu() => ComponentList = RightMenu.ToList();
    }
}
