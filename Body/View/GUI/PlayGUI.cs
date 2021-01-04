using FarmConsole.Body.Model.Helpers;
using FarmConsole.Body.Model.Objects;
using FarmConsole.Body.View.Components;
using System;
using System.Collections.Generic;

namespace FarmConsole.Body.View.GUI
{
    public static partial class GUI
    {
        public static void Play(string name)
        {
            vt1.H1(title);
            vt1.H2("Witaj w grze " + name + "!");
            vt1.Foot(foot);
            vt1.PrintList();

        }
        public static void LeftMenu(List<string> component_list, bool extended = false)
        {
            vt2.Endl(6);
            vt2.GroupStart(0);

            vt2.GroupStart(1);
            vt2.Endl(2);
            for (int i = 0; i < component_list.Count; i++)
                if (i < (Console.WindowHeight - 13) / 3)
                    vt2.TextBox(component_list[i], Console.WindowWidth / 5 - 1, margin: 0);
                else vt2.TextBox(component_list[i], Console.WindowWidth / 5 - 1, false, margin: 0);
            vt2.GroupEnd();

            vt2.GroupStart(1);
            vt2.LeftBar();
            vt2.GroupEnd();

            vt2.GroupStart(1, 10);
            vt2.Endl(Console.WindowHeight - 12);
            if (extended) vt2.TextBox("WYKONAJ Q", Console.WindowWidth / 10 - 1, color1: ConsoleColor.Gray, margin:0);
            else          vt2.TextBox("WYJDŹ Q", Console.WindowWidth / 10 - 1, color1: ConsoleColor.Gray, margin:0);
            vt2.GroupEnd();

            vt2.GroupStart(2, 10);
            vt2.Endl(Console.WindowHeight - 12);
            if (extended) vt2.TextBox("WRÓĆ E", Console.WindowWidth / 10 - 1, color1: ConsoleColor.Gray, margin: 0);
            else          vt2.TextBox("WYKONAJ E", Console.WindowWidth / 10 - 1, color1: ConsoleColor.Gray, margin: 0);
            vt2.GroupEnd();

            vt2.GroupEnd();
            vt2.PrintList();
            //vt2.showComponentList();
        }
        public static void RightMenu(List<Product>[] component_list, int category, bool extended = false)
        {
            vt3.ClearList();
            vt3.Endl(6);
            vt3.GroupStart(0);

                vt3.GroupStart(5);
                vt3.RightBar();
                vt3.GroupEnd();

                vt3.GroupStart(9, 10);
                vt3.Endl(Console.WindowHeight - 12);
                if(extended) vt3.TextBox("WRÓĆ Q", Console.WindowWidth / 10 - 1, color1: ConsoleColor.Gray, margin: 0) ;
                else         vt3.TextBox("WTBIERZ Q", Console.WindowWidth / 10 - 1, color1: ConsoleColor.Gray, margin: 0) ;
                vt3.GroupEnd();

                vt3.GroupStart(10, 10);
                vt3.Endl(Console.WindowHeight - 12);
                if (extended) vt3.TextBox("WYBIERZ E", Console.WindowWidth / 10 - 1, color1: ConsoleColor.Gray, margin: 0);
                else          vt3.TextBox("WYJDŹ E", Console.WindowWidth / 10 - 1, color1: ConsoleColor.Gray, margin: 0);
                vt3.GroupEnd();

                if(component_list[category].Count == 0)
                {
                    vt3.GroupStart(5);
                    vt3.Endl(3);
                    vt3.GraphicBox(new string[] { "NIC TU NIE MA" } );
                    vt3.GroupEnd();
                }

                vt3.GroupStart(5);
                vt3.GraphicBox(new string[] { XF.GetProductCategoryName()[category], "", }, color: ConsoleColor.Gray);
                for (int i = 0; i < component_list[category].Count; i++)
                    if (i < (Console.WindowHeight - 13) / 3)
                        vt3.TextBox(component_list[category][i].amount + "x " + component_list[category][i].name, Console.WindowWidth / 5 - 1, margin: 0);
                    else vt3.TextBox(component_list[category][i].amount + "x " + component_list[category][i].name, Console.WindowWidth / 5 - 1, false, margin: 0);
                vt3.GroupEnd();

            vt3.GroupEnd();
            vt3.PrintList();
        }
    }
}
