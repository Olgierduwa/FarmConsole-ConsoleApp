using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class HelpView : MenuManager
    {
        public static void Display()
        {
            Endl(3);
            H2(StringService.Get("help label"));
            Endl(Console.WindowHeight / 8);
            GroupStart(0);
            GroupStart(2);
            TextBox(StringService.Get("control label"), 44);
            Endl(1);
            TextBox(controlsText, 44);
            GroupEnd();
            GroupStart(4);
            TextBox(StringService.Get("from the author label"), 44);
            Endl(1);
            TextBox(fromAuthorText, 44);
            GroupEnd();
            GroupEnd();

            PrintList();
            //showComponentList();

            ComponentsDisplayed = new List<CM>();
            ComponentsDisplayed.Add(GetComponentByName("GS",2));
            ComponentsDisplayed.Add(GetComponentByName("GS",3));
        }
    }
}
