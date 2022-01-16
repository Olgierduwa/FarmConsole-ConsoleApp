using FarmConsole.Body.Controllers.CentralControllers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Services.GameServices;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Views.GameViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controllers.GameControllers
{
    class PlotBuyingController : HeadController
    {
        private static int amount;
        private static int Selected;

        private static void UpdateSelected(int value)
        {
            Selected += value;
        }
        private static void TryToBuy()
        {
            int mapsize = 10;
            GameService.IncreaseInExperience(mapsize * mapsize);
        }

        public static void Open()
        {
            Selected = 1;
            ComponentService.SetView = MapEngine.Map;
            PlotBuyingView.Display();

            while (OpenScreen == "PlotBuying")
            {
                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    switch (cki.Key)
                    {
                        case ConsoleKey.Tab:
                        case ConsoleKey.Escape: OpenScreen = EscapeScreen; SoundService.Play("K2"); break;
                        case ConsoleKey.W: UpdateSelected(+1); break;
                        case ConsoleKey.S: UpdateSelected(-1); break;
                        case ConsoleKey.D: break;
                        case ConsoleKey.A: break;
                        case ConsoleKey.E: break;
                        case ConsoleKey.Q: break;
                    }
                }
            }
            ComponentService.Clean();
        }
    }
}
