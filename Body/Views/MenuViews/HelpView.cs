using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class HelpView : ComponentService
    {
        public static void Display()
        {
            Endl(3);
            H2(LS.Navigation("help label"));
            Endl(Console.WindowHeight / 8);
            GroupStart(0);
            GroupStart(2);
            TextBox(LS.Navigation("control label"), 44);
            Endl(1);
            TextBoxLines(LS.Text("controls").Split('\n'), 44);
            GroupEnd();
            GroupStart(4);
            TextBox(LS.Navigation("about label"), 44);
            Endl(1);
            TextBoxLines(LS.Text("about").Split('\n'), 44);
            GroupEnd();
            GroupEnd();

            PrintList();
            //showComponentList();

            ComponentsDisplayed.Clear();
            ComponentsDisplayed.Add(GetComponentByName("GS",2));
            ComponentsDisplayed.Add(GetComponentByName("GS",3));
        }
    }
}
