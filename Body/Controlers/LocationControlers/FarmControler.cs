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
    public class FarmControler : MenuControlerService
    {
        public static void Open()
        {
            int X, Y, invert = 1, FarmSize = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(save.GetMap().Length / 3)));
            bool SHOWFIELDID = false, ShiftPressed;
            X = Y = FarmSize / 2;
            GameView.Show(save.Name);
            FarmView.Show(save.GetMap());
            FarmView.MoveStandPosition(new Point(X + 1, Y + 1), new Point(X + 1, Y + 1), false);
            SideMenuControler.Initialize();

            while (openScreen == "Farm")
            {
                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    if (cki.Modifiers == ConsoleModifiers.Shift) ShiftPressed = true; else ShiftPressed = false;
                    if (SHOWFIELDID) FarmView.ShowFieldID();

                    switch (cki.Key)
                    {
                        case ConsoleKey.Escape: save.SetMap(FarmView.GetMap()); openScreen = "Escape"; S.Play("K2"); break;
                        case ConsoleKey.Q: SideMenuControler.Open(cki.Key); break;
                        case ConsoleKey.E: SideMenuControler.Open(cki.Key); break;

                        case ConsoleKey.W: if (X > 1 && FarmView.MoveStandPosition(new Point(X + 1, Y + 1), new Point(X, Y + 1), ShiftPressed)) X--; break;
                        case ConsoleKey.A: if (Y > 1 && FarmView.MoveStandPosition(new Point(X + 1, Y + 1), new Point(X + 1, Y), ShiftPressed)) Y--; break;
                        case ConsoleKey.S: if (X < FarmSize && FarmView.MoveStandPosition(new Point(X + 1, Y + 1), new Point(X + 2, Y + 1), ShiftPressed)) X++; break;
                        case ConsoleKey.D: if (Y < FarmSize && FarmView.MoveStandPosition(new Point(X + 1, Y + 1), new Point(X + 1, Y + 2), ShiftPressed)) Y++; break;

                        case ConsoleKey.DownArrow: FarmView.MoveMapPosition(new Point(0, -1 * invert)); break;
                        case ConsoleKey.UpArrow: FarmView.MoveMapPosition(new Point(0, 1 * invert)); break;
                        case ConsoleKey.RightArrow: FarmView.MoveMapPosition(new Point(-1 * invert, 0)); break;
                        case ConsoleKey.LeftArrow: FarmView.MoveMapPosition(new Point(1 * invert, 0)); break;

                        case ConsoleKey.M: invert *= -1; break;
                        case ConsoleKey.I: SHOWFIELDID = !SHOWFIELDID; break;
                    }
                }
                else if ((DateTime.Now - Now).TotalMilliseconds >= 50)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Now = DateTime.Now;
                }
            }
            GameView.ClearList();
            FarmView.ClearMap();
        }
    }
}