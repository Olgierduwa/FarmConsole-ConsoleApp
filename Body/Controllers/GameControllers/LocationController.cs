﻿using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Views.GameViews;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Services.GameServices;
using FarmConsole.Body.Controllers.CentralControllers;

namespace FarmConsole.Body.Controllers.GameControllers
{
    class LocationController : HeadController
    {
        private static bool ShiftPressed;
        private static string Location;

        private static void MoveStandPosition(string Direction)
        {
            if (!PlayerMovementAxis) switch (Direction)
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
            GameService.DisplayFieldName();
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
            MapService.InitMap(map);
            MapService.ShowMap();
            GameService.DisplayFieldName();
        }
        private static void LeaveLocation(string Location)
        {
            EscapeScreen = Location;
            MapEngine.Map.LastVisitDate = GameInstance.GameDate;
            GameInstance.SetMap(Location, MapEngine.Map);
            ComponentService.Clean(WithCleaning: false);
            if (GameInstance.IsLocation(OpenScreen)) MapService.HideMap();
            else MapService.HideMap(false);
        }

        public static void Open(string _Location)
        {
            Location = _Location;
            //HelpService.CLEAR_TIMERS();
            EnterLocation(Location);
            GameService.DisplayStats();
            while (OpenScreen == Location)
            {
                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    if (cki.Modifiers == ConsoleModifiers.Shift) ShiftPressed = true; else ShiftPressed = false;

                    switch (cki.Key)
                    {
                        case ConsoleKey.Escape: MapService.Drop(false); OpenScreen = "Escape"; SoundService.Play("K2"); break;
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

                        default: GOODMOD(cki); break;
                    }
                    GameService.DisplayStats();
                    GameService.DisplayFieldName();
                }
                else if ((DateTime.Now - Previously).TotalMilliseconds >= FrameTime)
                {
                    PopUp(POPUPID, POPUPTEXT);

                    if (Action.IsInProcess)
                    {
                        if (!ActionService.MakeAction())
                        {
                            SideMenuController.ShowResult();
                            Console.ReadKey(true);
                            SideMenuController.HideSideMenu("C");
                        }
                        GameService.DisplayFieldName();
                    }
                    Previously = DateTime.Now;
                }
            }
            LeaveLocation(Location);
        }

        private static void GOODMOD(ConsoleKeyInfo cki)
        {
            if(SettingsService.GODMOD) switch(cki.Key)
            {
                case ConsoleKey.D1: MapEngine.FPM = 1; break;
                case ConsoleKey.D2: MapEngine.FPM = 2; break;
                case ConsoleKey.D3: MapEngine.FPM = 3; break;
                case ConsoleKey.D4: MapEngine.FPM = 6; break;

                case ConsoleKey.Z: GameService.Die(); break;
                case ConsoleKey.X: ActionService.RESULT = LS.Action("done"); ActionService.GetTired(30); break;

                case ConsoleKey.R:
                    GameInstance.SetMapSupply(Location); GameInstance.SetMap(Location, MapEngine.Map);
                    MapService.GlobalMapInit(); MapEngine.ReloadMapView(); break;

                case ConsoleKey.F:
                    GameInstance.SetMap(Location, MapEngine.Map); GameService.GrowingUp();
                    MapService.InitMap(GameInstance.GetMap(Location)); MapService.ShowMap(); break;
            }
        }
    }
}