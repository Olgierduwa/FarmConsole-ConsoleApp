using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controlers
{
    public class MenuControler : MenuControlerService
    {
        public static void Open()
        {
            int selected = 1, selCount = 4, selStart = 1, sleep = 1000;

            MenuView.Show();
            MenuView.UpdateSelect(selected, selected, selCount);
            while (openScreen == "Menu")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.R: POPUPID = 1; POPUPSTAGE = 0; break;
                        case ConsoleKey.T: openScreen = "Intro"; break;
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; MenuView.UpdateSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; MenuView.UpdateSelect(selected, selected - 1, selCount); } break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: if (MenuView.Warning() == true) openScreen = "Close"; break;
                        case ConsoleKey.E:
                            S.Play("K2"); switch (selected)
                            {
                                case 1: openScreen = "NewGame"; break;
                                case 2: openScreen = "Load"; break;
                                case 3: openScreen = "Options"; break;
                                case 4: openScreen = "Help"; break;
                            }
                            break;
                    }
                }
                else if ((DateTime.Now - Now).TotalMilliseconds >= sleep)
                {
                    sleep = 50;
                    PopUp(POPUPID, POPUPTEXT);
                    Now = DateTime.Now;
                }
            }
            MenuView.ClearList();
        }
    }
}
