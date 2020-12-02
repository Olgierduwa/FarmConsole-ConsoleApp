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
        public static void LeftMenu(string[] component_list)
        {
            vt2.Endl(6);
            vt2.GroupStart(0);
            vt2.GroupStart(1);
            vt2.LeftBar();
            vt2.GroupEnd();
            vt2.GroupStart(1);
            vt2.Endl(2);
            for (int i = 0; i < component_list.Length; i++)
                if (i < (Console.WindowHeight - 17) / 3)
                    vt2.TextBox(component_list[i], 30);
                else vt2.TextBox(component_list[i], 30, false);
            vt2.GroupEnd();
            vt2.GroupEnd();
            vt2.DoubleButtonBot(1, "D", "Wybierz");
            vt2.PrintList();
            //vt2.showComponentList();
        }
        public static void RightMenu(string[] component_list)
        {
            vt3.Endl(6);
            vt3.GroupStart(0);
            vt3.GroupStart(5);
            vt3.RightBar();
            vt3.GroupEnd();
            vt3.GroupStart(5);
            vt3.Endl(2);
            for (int i = 0; i < component_list.Length; i++)
                if (i < (Console.WindowHeight - 17) / 3)
                    vt3.TextBox(component_list[i], 30);
                else vt3.TextBox(component_list[i], 30, false);
            vt3.GroupEnd();
            vt3.GroupEnd();
            vt3.DoubleButtonBot(5, "Wybierz", "D");
            vt3.PrintList();
            //vt3.showComponentList();
        }
    }
}
