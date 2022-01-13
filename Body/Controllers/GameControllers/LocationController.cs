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
        private static bool ShiftPressed;

        private static void MoveStandPosition(string Direction)
        {
            if(!PlayerMovementAxis) switch (Direction)
            {
                case "UP": MapEngine.MoveStandPosition(new Point(0, -1), ShiftPressed); break;
                case "DOWN": MapEngine.MoveStandPosition(new Point(0, 1), ShiftPressed); break;
                case "LEFT": MapEngine.MoveStandPosition(new Point(1, 0), ShiftPressed); break;
                case "RIGHT": MapEngine.MoveStandPosition(new Point(-1, 0), ShiftPressed); break;
            }
            else switch (Direction)
            {
                case "UP": MapEngine.MoveStandPosition(new Point(-1, 0), ShiftPressed); break;
                case "DOWN": MapEngine.MoveStandPosition(new Point(1, 0), ShiftPressed); break;
                case "LEFT": MapEngine.MoveStandPosition(new Point(0, -1), ShiftPressed); break;
                case "RIGHT": MapEngine.MoveStandPosition(new Point(0, 1), ShiftPressed); break;
            }
            if (FieldNameVisibility) GameView.DisplayFieldName();
        }
        private static void MoveMapPosition(string Direction)
        {
            if (MapMovementAxis) switch (Direction)
                {
                    case "UP": MapEngine.MoveMapPosition(new Point(-1, -1)); break;
                    case "DOWN": MapEngine.MoveMapPosition(new Point(1, 1)); break;
                    case "LEFT": MapEngine.MoveMapPosition(new Point(1, -1)); break;
                    case "RIGHT": MapEngine.MoveMapPosition(new Point(-1, 1)); break;
                }
            else switch (Direction)
                {
                    case "UP": MapEngine.MoveMapPosition(new Point(1, 1)); break;
                    case "DOWN": MapEngine.MoveMapPosition(new Point(-1, -1)); break;
                    case "LEFT": MapEngine.MoveMapPosition(new Point(-1, 1)); break;
                    case "RIGHT": MapEngine.MoveMapPosition(new Point(1, -1)); break;
                }
        }
        private static void EnterLocation(string Location)
        {
            GameView.Display(GameInstance.UserName);
            MapModel map = GameInstance.GetMap(Location);
            SideMenuController.Init(map.Scale);
            MapManager.InitMap(map);
            MapManager.ShowMap();
            if (FieldNameVisibility) GameView.DisplayFieldName();
        }
        private static void LeaveLocation(string Location)
        {
            EscapeScreen = Location;
            MapEngine.Map.LastVisitDate = GameInstance.GameDate;
            GameInstance.SetMap(Location, MapEngine.Map);
            MenuManager.Clean(WithCleaning: false);
            if(GameInstance.IsLocation(OpenScreen)) MapManager.HideMap();
            else MapManager.HideMap(false);
        }

        public static void Open(string Location)
        {
            //HelpService.CLEAR_TIMERS();
            EnterLocation(Location);
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
                        case ConsoleKey.Tab: SideMenuController.Open(cki.Key); break;

                        case ConsoleKey.W: MoveStandPosition("UP"); break;
                        case ConsoleKey.S: MoveStandPosition("DOWN"); break;
                        case ConsoleKey.A: MoveStandPosition("LEFT"); break;
                        case ConsoleKey.D: MoveStandPosition("RIGHT"); break;

                        case ConsoleKey.UpArrow: MoveMapPosition("UP"); break;
                        case ConsoleKey.DownArrow: MoveMapPosition("DOWN"); break;
                        case ConsoleKey.LeftArrow: MoveMapPosition("LEFT"); break;
                        case ConsoleKey.RightArrow: MoveMapPosition("RIGHT"); break;

                        case ConsoleKey.D1: MapEngine.FPM = 1; break;
                        case ConsoleKey.D2: MapEngine.FPM = 2; break;
                        case ConsoleKey.D3: MapEngine.FPM = 3; break;
                        case ConsoleKey.D4: MapEngine.FPM = 6; break;

                        case ConsoleKey.R: GameInstance.SetMapSupply(Location); GameInstance.SetMap(Location, MapEngine.Map); 
                                           MapManager.GlobalMapInit(); MapEngine.ReloadMapView(); break;
                        case ConsoleKey.F: GameInstance.SetMap(Location, MapEngine.Map); GameService.GrowingUp();
                                           MapManager.InitMap(GameInstance.GetMap(Location)); MapManager.ShowMap(); break;
                    }

                    if (FieldNameVisibility) GameView.DisplayFieldName();
                }
                else if ((DateTime.Now - Previously).TotalMilliseconds >= FrameTime)
                {
                    PopUp(POPUPID, POPUPTEXT);

                    if (Action.IsInProcess)
                    {
                        SideMenuService.MakeAction();
                        if (FieldNameVisibility) GameView.DisplayFieldName();
                    }
                    Previously = DateTime.Now;
                }
            }
            LeaveLocation(Location);
        }
    }
}