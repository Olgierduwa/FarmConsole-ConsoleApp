using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using FarmConsole.Body.Views.MenuViews;
using FarmConsole.Body.Views.LocationViews;

namespace FarmConsole.Body.Controlers
{
    public class FarmController : MainControllerService
    {
        public static void Open()
        {
            HelpService.CLEAR_TIMERS();
            bool ShiftPressed;
            GameView.Show(GameInstance.UserName);
            FarmView.InitializeFarmView(GameInstance.FarmMap);
            SideMenuController.Initializate(0);
            MapView.ShowMap();
            escapeScreen = "Farm";
            while (openScreen == "Farm")
            {
                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    if (cki.Modifiers == ConsoleModifiers.Shift) ShiftPressed = true; else ShiftPressed = false;

                    switch (cki.Key)
                    {
                        case ConsoleKey.Escape: MapView.Drop(false); openScreen = "Escape"; S.Play("K2"); break;
                        case ConsoleKey.Q: SideMenuController.Open(cki.Key); break;
                        case ConsoleKey.E: SideMenuController.Open(cki.Key); break;

                        case ConsoleKey.W: MapService.MoveStandPosition(new Point(-1, 0), ShiftPressed); break;
                        case ConsoleKey.S: MapService.MoveStandPosition(new Point(1, 0), ShiftPressed); break;
                        case ConsoleKey.A: MapService.MoveStandPosition(new Point(0, -1), ShiftPressed); break;
                        case ConsoleKey.D: MapService.MoveStandPosition(new Point(0, 1), ShiftPressed); break;

                        case ConsoleKey.UpArrow: MapService.MoveMapPosition(new Point(-1, -1)); break;
                        case ConsoleKey.DownArrow: MapService.MoveMapPosition(new Point(1, 1)); break;
                        case ConsoleKey.LeftArrow: MapService.MoveMapPosition(new Point(1, -1)); break;
                        case ConsoleKey.RightArrow: MapService.MoveMapPosition(new Point(-1, 1)); break;

                        case ConsoleKey.R: MapView.GlobalMapInitializate(); MapView.HideMap(); MapView.ShowMap(); break;
                        case ConsoleKey.F: GameService.GrowingUp(); FarmView.InitializeFarmView(GameInstance.FarmMap); MapView.ShowMap(); break;
                    }
                }
                else if ((DateTime.Now - Now).TotalMilliseconds >= 50)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Now = DateTime.Now;
                }
            }
            GameInstance.FarmMap = MapService.GetMap();
            GameView.Clean();
            MapView.HideMap();
        }
    }
}