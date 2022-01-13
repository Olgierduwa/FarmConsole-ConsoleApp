using FarmConsole.Body.Controlers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.GameViews;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Controllers.GameControllers
{
    class PlotExtendingController : MainController
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
            int amount = tileCount ;
            if (GameInstance.WalletFunds >= amount)
            {
                GameInstance.WalletFunds -= amount;
                OpenScreen = EscapeScreen;
                switch(direction)
                {
                    case "extend up": GameInstance.ExpandMap("Farm", new Point(-1, -1)); break;
                    case "extend down": GameInstance.ExpandMap("Farm", new Point(1, 1)); break;
                    case "extend right": GameInstance.ExpandMap("Farm", new Point(-1, 1)); break;
                    case "extend left": GameInstance.ExpandMap("Farm", new Point(1, -1)); break;
                }
                S.Play("K3");
                MenuManager.GoodNews(LS.Navigation("land ownership document"));
            }
            else MenuManager.Warning(LS.Action("no money in wallet"));
        }

        public static void Open()
        {
            int size = GameInstance.GetMap("Farm").MapSize;
            tilePrice = 20 * GameInstance.LVL;
            tileCount = size + size - 1;
            direction = "extend down";
            MenuManager.SetView = MapEngine.Map;
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
                        case ConsoleKey.Escape: OpenScreen = EscapeScreen; S.Play("K2"); break;
                        case ConsoleKey.W: UpdateSelected("extend up"); break;
                        case ConsoleKey.S: UpdateSelected("extend down"); break;
                        case ConsoleKey.D: UpdateSelected("extend right"); break;
                        case ConsoleKey.A: UpdateSelected("extend left"); break;
                        case ConsoleKey.E: Extending(); break;
                    }
                }
            }
            MenuManager.Clean();
        }
    }
}
