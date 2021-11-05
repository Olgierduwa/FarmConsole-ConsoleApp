using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.LocationViews;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Controlers
{
    public class HouseController : MainControllerService
    {
        public static void Open()
        {
            bool ShiftPressed;
            GameView.Show(GameInstance.UserName);
            HouseView.InitializeHouseView(GameInstance.HouseMap);
            SideMenuController.Initializate(1);
            MapView.ShowMap();
            escapeScreen = "House";
            while (openScreen == "House")
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
                    }
                }
                else if ((DateTime.Now - Now).TotalMilliseconds >= 50)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Now = DateTime.Now;
                }
            }
            GameInstance.HouseMap = MapService.GetMap();
            GameView.Clean();
            MapView.HideMap();
        }
    }
}
