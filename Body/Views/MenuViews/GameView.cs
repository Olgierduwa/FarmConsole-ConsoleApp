using FarmConsole.Body.Engines;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class GameView : MenuManager
    {
        public static void Display(string name)
        {
            Endl(3);
            H2(StringService.Get("hello player") + ", " + name + "!");
            PrintList();
        }
    }
}
