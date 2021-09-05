using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    public class GameView : MainViewService
    {
        public static void Show(string name)
        {
            H1(title);
            H2("Witaj w grze " + name + "!");
            Foot(foot);
            PrintList();
        }
    }
}
