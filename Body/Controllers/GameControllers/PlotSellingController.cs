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
    class PlotSellingController : HeadController
    {
        private static int Selected;

        private static void UpdateSelected(int value)
        {
            Selected += value;
            ComponentService.UpdateMenuSelect(Selected, Selected, 3);
        }
        private static void TryToSell()
        {
            int mapsize = 10;
            GameService.IncreaseInExperience(mapsize * mapsize);
        }


        public static void Open()
        {
            Selected = 1;
            ComponentService.SetView = MapEngine.Map;
            PlotSellingView.Display();

            while (OpenScreen == "PlotSelling")
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
                        case ConsoleKey.E: TryToSell(); break;
                        case ConsoleKey.Q: break;
                    }
                }
            }
            ComponentService.Clean();
        }
    }
}
