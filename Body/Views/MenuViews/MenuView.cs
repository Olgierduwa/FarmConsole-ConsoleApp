using FarmConsole.Body.Engines;
using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class MenuView : ComponentService
    {
        public static void Display()
        {
            Endl(Console.WindowHeight / 2 - 9);
            GroupStart(0);

            GroupStart(2);
            Endl(1);
            TextBox(LS.Navigation("start game button"));
            TextBox(LS.Navigation("load game button"));
            TextBox(LS.Navigation("settings button"));
            TextBox(LS.Navigation("help button"));
            GroupEnd();

            GroupStart(4);
            Endl(1);
            TextBox("\"" + LS.Navigation("menu text 1") + "\"");
            TextBox("\"" + LS.Navigation("menu text 2") + "\"");
            GroupEnd();

            GroupEnd();
            PrintList();
        }
    }
}
