using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class EscapeView : MenuManager
    {
        public static void Display()
        {
            Endl(Console.WindowHeight / 3);

            GroupStart(0);
            GroupStart(3);
            Endl(1);
            TextBox(StringService.Get("continue button"));
            TextBox(StringService.Get("save game button"));
            TextBox(StringService.Get("back to menu button"));
            TextBox(StringService.Get("options button"));
            TextBox(StringService.Get("help button"));

            GroupStart(Console.WindowWidth / 2, Console.WindowWidth);
            Endl(4);
            TextBox(exitQuestion, 33, false, ColorService.GetColorByName("Red"));

            GroupStart(Console.WindowWidth / 2 - 9, Console.WindowWidth);
            Endl(10);
            TextBox(StringService.Get("no", " Q"), 15, false, ColorService.GetColorByName("Red"), margin: 0);
            GroupEnd();

            GroupStart(Console.WindowWidth / 2 + 9, Console.WindowWidth);
            Endl(10);
            TextBox(StringService.Get("yes", " E"), 15, false, ColorService.GetColorByName("Red"), margin: 0);
            GroupEnd();

            GroupEnd();
            GroupEnd();
            GroupEnd();

            PrintList();
            //vt1.showComponentList();

            ComponentsDisplayed = new List<CM>();
            ComponentsDisplayed.Add(GetComponentByName("GS", 2));
            FocusGroupID = 3;
        }
    }
}
