using FarmConsole.Body.Model.Helpers;
using FarmConsole.Body.Model.Objects;
using FarmConsole.Body.View.Components;
using System;
using System.Collections.Generic;

namespace FarmConsole.Body.View.GUI
{
    public static partial class GUI
    {
        public static void Play()
        {
            vt1.H1(title);
            vt1.H2("WITAJ W GRZE!");
            vt1.Foot(foot);
            vt1.PrintList();

        }
        public static void LeftMenu(List<string> component_list)
        {
            vt2.Endl(6);
            vt2.GroupStart(0);
            vt2.GroupStart(1);
            vt2.LeftBar();
            vt2.GroupEnd();
            vt2.GroupStart(1);
            vt2.Endl(2);
            for (int i = 0; i < component_list.Count; i++)
                if (i < (Console.WindowHeight - 17) / 3)
                    vt2.TextBox(component_list[i], Console.WindowWidth / 5 - 2);
                else vt2.TextBox(component_list[i], Console.WindowWidth / 5 - 2, false);
            vt2.GroupEnd();

            vt2.GroupStart(1,10);
            vt2.Endl(Console.WindowHeight - 14);
            vt2.TextBox("<< A", Console.WindowWidth / 10 - 1);
            vt2.GroupEnd();

            vt2.GroupStart(2,10);
            vt2.Endl(Console.WindowHeight - 14);
            vt2.TextBox("D >>", Console.WindowWidth / 10 - 1);
            vt2.GroupEnd();

            vt2.GroupEnd();
            vt2.PrintList();
            //vt2.showComponentList();
        }
        public static void RightMenu(List<Product>[] component_list, int category)
        {
            vt3.ClearList();
            vt3.Endl(6);
            vt3.GroupStart(0);

                vt3.GroupStart(5);
                vt3.RightBar();
                vt3.GroupEnd();

                vt3.GroupStart(5);
                vt3.Graphic(new string[] { "".PadRight(XF.GetProductCategoryName()[category].Length + 1, '─'), XF.GetProductCategoryName()[category] });
                for (int i = 0; i < component_list[category].Count; i++)
                    if (i < (Console.WindowHeight - 17) / 3)
                        vt3.TextBox(component_list[category][i].name, Console.WindowWidth / 5 - 1);
                    else vt3.TextBox(component_list[category][i].name, Console.WindowWidth / 5 - 1, false);
                vt3.GroupEnd();

                if(component_list[category].Count == 0)
                {
                    vt3.GroupStart(5);
                    vt3.Endl(3);
                    vt3.Graphic(new string[] { "NIC TU NIE MA" } );
                    vt3.GroupEnd();
                }

                vt3.GroupStart(9, 10);
                vt3.Endl(Console.WindowHeight - 14);
                vt3.TextBox("<< A", Console.WindowWidth / 10 - 1);
                vt3.GroupEnd();

                vt3.GroupStart(10, 10);
                vt3.Endl(Console.WindowHeight - 14);
                vt3.TextBox("D >>", Console.WindowWidth / 10 - 1);
                vt3.GroupEnd();

            vt3.GroupEnd();

            vt3.PrintList();
            //vt3.showComponentList();
        }
    }
}
