using FarmConsole.Body.Controllers.CentralControllers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Services.GameServices;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Views.GameViews;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Controllers.GameControllers
{
    class PlotExtendingController : HeadController
    {
        private static int tilePrice;
        private static int tileCount;
        private static string direction;
        
        private static void UpdateSelected(string _direction)
        {
            if(direction != _direction)
            {
                direction = _direction;
                PlotExtendingView.Display(tilePrice, tileCount, direction);
            }
        }
        private static void Extending()
        {
            int amount = tileCount * tilePrice;
            if (GameInstance.WalletFunds >= amount)
            {
                GameInstance.WalletFunds -= amount;
                GameService.IncreaseInExperience(tileCount);
                OpenScreen = EscapeScreen;
                switch(direction)
                {
                    case "extend up": GameInstance.ExpandMap("Farm", new Point(-1, -1)); break;
                    case "extend down": GameInstance.ExpandMap("Farm", new Point(1, 1)); break;
                    case "extend right": GameInstance.ExpandMap("Farm", new Point(-1, 1)); break;
                    case "extend left": GameInstance.ExpandMap("Farm", new Point(1, -1)); break;
                }
                SoundService.Play("K3");
                ComponentService.GoodNews(LS.Navigation("land ownership document"));
            }
            else ComponentService.Warning(LS.Action("no money in wallet"));
        }

        public static void Open()
        {
            int size = GameInstance.GetMap("Farm").MapSize;
            tilePrice = 20 * GameInstance.LVL;
            tileCount = size + size - 1;
            direction = "extend down";
            ComponentService.SetView = MapEngine.Map;
            PlotExtendingView.Display(tilePrice, tileCount, direction);

            while (OpenScreen == "PlotExtending")
            {
                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    switch (cki.Key)
                    {
                        case ConsoleKey.Q:
                        case ConsoleKey.Tab:
                        case ConsoleKey.Escape: OpenScreen = EscapeScreen; SoundService.Play("K2"); break;
                        case ConsoleKey.W: UpdateSelected("extend up"); break;
                        case ConsoleKey.S: UpdateSelected("extend down"); break;
                        case ConsoleKey.D: UpdateSelected("extend right"); break;
                        case ConsoleKey.A: UpdateSelected("extend left"); break;
                        case ConsoleKey.E: Extending(); break;
                    }
                }
            }
            ComponentService.Clean();
        }
    }
}
