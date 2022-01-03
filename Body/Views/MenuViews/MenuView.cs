using FarmConsole.Body.Engines;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class MenuView : MenuManager
    {
        public static void Display()
        {
            Endl(Console.WindowHeight / 2 - 9);
            GroupStart(0);

                GroupStart(2);
                Endl(1);
                TextBox(StringService.Get("start game button"));
                TextBox(StringService.Get("load game button"));
                TextBox(StringService.Get("settings button"));
                TextBox(StringService.Get("help button"));
                GroupEnd();

                GroupStart(4);
                Endl(1);
                TextBox("\"" + StringService.Get("menu text 1") + "\"");
                TextBox("\"" + StringService.Get("menu text 2") + "\"");
                GroupEnd();

            GroupEnd();
            PrintList();
            //vt1.ShowComponentList();
            DangerMessage = exitQuestion;
        }
    }
}
