using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using FarmConsole.Body.Views.MenuViews;
using FarmConsole.Body.Views.LocationViews;
using FarmConsole.Body.Engines;

namespace FarmConsole.Body.Controlers
{
    class LocationController : MainController
    {
        public static void Open(string Location)
        {
            bool ShiftPressed;
            GameView.Display(GameInstance.UserName);
            MapManager.InitMap(GameInstance.GetMap(Location));
            MapManager.ShowMap();
            SideMenuController.Init(GameInstance.GetMap(Location).Scale);
            EscapeScreen = Location;
            while (OpenScreen == Location)
            {
                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    if (cki.Modifiers == ConsoleModifiers.Shift) ShiftPressed = true; else ShiftPressed = false;

                    switch (cki.Key)
                    {
                        case ConsoleKey.Escape: MapManager.Drop(false); OpenScreen = "Escape"; S.Play("K2"); break;
                        case ConsoleKey.Q: SideMenuController.Open(cki.Key); break;
                        case ConsoleKey.E: SideMenuController.Open(cki.Key); break;

                        case ConsoleKey.W: MapEngine.MoveStandPosition(new Point(-1, 0), ShiftPressed); break;
                        case ConsoleKey.S: MapEngine.MoveStandPosition(new Point(1, 0), ShiftPressed); break;
                        case ConsoleKey.A: MapEngine.MoveStandPosition(new Point(0, -1), ShiftPressed); break;
                        case ConsoleKey.D: MapEngine.MoveStandPosition(new Point(0, 1), ShiftPressed); break;

                        case ConsoleKey.UpArrow: MapEngine.MoveMapPosition(new Point(-1, -1)); break;
                        case ConsoleKey.DownArrow: MapEngine.MoveMapPosition(new Point(1, 1)); break;
                        case ConsoleKey.LeftArrow: MapEngine.MoveMapPosition(new Point(1, -1)); break;
                        case ConsoleKey.RightArrow: MapEngine.MoveMapPosition(new Point(-1, 1)); break;

                        case ConsoleKey.R: MapManager.GlobalMapInit(); MapEngine.ReloadMap(); break;
                        case ConsoleKey.F: GameInstance.SetMap(Location, MapEngine.Map); GameService.GrowingUp();
                                           MapManager.InitMap(GameInstance.GetMap(Location)); MapManager.ShowMap(); break;
                    }
                }
                else if ((DateTime.Now - Previously).TotalMilliseconds >= FrameTime)
                {
                    PopUp(POPUPID, POPUPTEXT);

                    if(Action.IsInProcess) SideMenuService.DoAction();
                    Previously = DateTime.Now;
                }
            }
            GameInstance.SetMap(Location, MapEngine.Map);
            MapManager.HideMap();
            GameView.Clean();
        }
    }
}