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
            Endl(3);
            H2(StringService.Get("game pause label"));

            Endl(Console.WindowHeight / 3 - 6);

            GroupStart(0);
                GroupStart(3);
                Endl(1);
                TextBox(StringService.Get("continue button"));
                TextBox(StringService.Get("save game button"));
                TextBox(StringService.Get("back to menu button"));
                TextBox(StringService.Get("settings button"));
                TextBox(StringService.Get("help button"));
                GroupEnd();
            GroupEnd();

            PrintList();

            DangerMessage = exitQuestion;
            ComponentsDisplayed = new List<CM>();
            ComponentsDisplayed.Add(GetComponentByName("GS", 2));
        }
    }
}
