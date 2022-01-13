using FarmConsole.Body.Controlers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Views.GameViews;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controllers.GameControllers
{
    class PlotSellingController : MainController
    {
        private static int amount;
        private static int selected;

        private static void UpdateSelected(int value)
        {
            selected += value;
        }

        public static void Open()
        {
            selected = 1;
            MenuManager.SetView = MapEngine.Map;
            PlotSellingView.Display();

            while (OpenScreen == "PlotSelling")
            {
                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    switch (cki.Key)
                    {
                        case ConsoleKey.Tab:
                        case ConsoleKey.Escape: OpenScreen = EscapeScreen; S.Play("K2"); break;
                        case ConsoleKey.W: UpdateSelected(+1); break;
                        case ConsoleKey.S: UpdateSelected(-1); break;
                        case ConsoleKey.D: break;
                        case ConsoleKey.A: break;
                        case ConsoleKey.E: break;
                        case ConsoleKey.Q: break;
                    }
                }
            }
            MenuManager.Clean();
        }
    }
}
