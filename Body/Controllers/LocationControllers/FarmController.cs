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
            GameView.Show(save.Name);
            FarmView.InitializeFarmView(save.GetMap());
            FarmView.ShowMap();
            SideMenuController.Initialize();
            while (openScreen == "Farm")
            {
                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    if (cki.Modifiers == ConsoleModifiers.Shift) ShiftPressed = true; else ShiftPressed = false;

                    switch (cki.Key)
                    {
                        case ConsoleKey.Escape: save.SetMap(FarmView.GetMap()); openScreen = "Escape"; S.Play("K2"); break;
                        case ConsoleKey.Q: SideMenuController.Open(cki.Key); break;
                        case ConsoleKey.E: SideMenuController.Open(cki.Key); break;

                        case ConsoleKey.W: FarmView.MoveStandPosition(new Point(-1, 0), ShiftPressed); break;
                        case ConsoleKey.S: FarmView.MoveStandPosition(new Point(1, 0), ShiftPressed); break;
                        case ConsoleKey.A: FarmView.MoveStandPosition(new Point(0, -1), ShiftPressed); break;
                        case ConsoleKey.D: FarmView.MoveStandPosition(new Point(0, 1), ShiftPressed); break;

                        case ConsoleKey.UpArrow: FarmView.MoveMapPosition(new Point(-1, -1)); break;
                        case ConsoleKey.DownArrow: FarmView.MoveMapPosition(new Point(1, 1)); break;
                        case ConsoleKey.LeftArrow: FarmView.MoveMapPosition(new Point(1, -1)); break;
                        case ConsoleKey.RightArrow: FarmView.MoveMapPosition(new Point(-1, 1)); break;
                    }
                }
                else if ((DateTime.Now - Now).TotalMilliseconds >= 50)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Now = DateTime.Now;
                }
            }
            GameView.ClearList();
            FarmView.HideMap();
        }
    }
}