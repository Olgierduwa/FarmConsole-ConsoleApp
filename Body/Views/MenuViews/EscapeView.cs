using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class EscapeView : ComponentService
    {
        public static void Display()
        {
            Endl(3);
            H2(LS.Navigation("game pause label"));

            Endl((Console.WindowHeight - 15) / 2 - 5);

            GroupStart(0);
                GroupStart(3);
                Endl(1);
                TextBox(LS.Navigation("continue button"));
                TextBox(LS.Navigation("save game button"));
                TextBox(LS.Navigation("back to menu button"));
                TextBox(LS.Navigation("settings button"));
                TextBox(LS.Navigation("help button"));
                GroupEnd();
            GroupEnd();

            PrintList();

            DangerMessage = LS.Text("exit question");
            ComponentsDisplayed.Clear();
            ComponentsDisplayed.Add(GetComponentByName("GS", 2));
        }
    }
}
