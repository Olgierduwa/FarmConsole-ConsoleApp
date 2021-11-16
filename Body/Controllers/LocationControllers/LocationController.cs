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
    public class LocationController : MainController
    {
        public static void Open(string Location)
        {
            bool ShiftPressed;
            GameView.Show(GameInstance.UserName);
            MapView.InitMap(GameInstance.GetMap(Location));
            SideMenuController.Init(GameInstance.GetMap(Location).Scale);
            MapView.ShowMap();
            escapeScreen = Location;
            while (openScreen == Location)
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

                        case ConsoleKey.W: MapEngine.MoveStandPosition(new Point(-1, 0), ShiftPressed); break;
                        case ConsoleKey.S: MapEngine.MoveStandPosition(new Point(1, 0), ShiftPressed); break;
                        case ConsoleKey.A: MapEngine.MoveStandPosition(new Point(0, -1), ShiftPressed); break;
                        case ConsoleKey.D: MapEngine.MoveStandPosition(new Point(0, 1), ShiftPressed); break;

                        case ConsoleKey.UpArrow: MapEngine.MoveMapPosition(new Point(-1, -1)); break;
                        case ConsoleKey.DownArrow: MapEngine.MoveMapPosition(new Point(1, 1)); break;
                        case ConsoleKey.LeftArrow: MapEngine.MoveMapPosition(new Point(1, -1)); break;
                        case ConsoleKey.RightArrow: MapEngine.MoveMapPosition(new Point(-1, 1)); break;

                        case ConsoleKey.R: MapView.GlobalMapInit(); MapView.HideMap(); MapView.ShowMap(); break;
                        case ConsoleKey.F: GameInstance.SetMap(Location,MapEngine.Map); GameService.GrowingUp();
                                           MapView.InitMap(GameInstance.GetMap(Location)); MapView.ShowMap(); break;
                    }
                }
                else if ((DateTime.Now - Now).TotalMilliseconds >= 50)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Now = DateTime.Now;
                }
            }
            GameInstance.SetMap(Location, MapEngine.Map);
            MapView.HideMap();
            GameView.Clean();
        }
    }
}