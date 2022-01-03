using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controlers
{
    class MenuController : MainController
    {
        public static void Open()
        {
            int selected = 1, selCount = 4, selStart = 1, sleep = 1000;

            MenuView.Display();
            MenuView.UpdateMenuSelect(selected, selected, selCount);
            while (OpenScreen == "Menu")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        //case ConsoleKey.R: POPUPID = 1; POPUPSTAGE = 0; break;
                        case ConsoleKey.T: OpenScreen = "Intro"; break;
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; MenuView.UpdateMenuSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; MenuView.UpdateMenuSelect(selected, selected - 1, selCount); } break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: if (MenuView.Danger() == true) OpenScreen = "Close"; break;
                        case ConsoleKey.G: SettingsService.GODMOD = !SettingsService.GODMOD; break;
                        case ConsoleKey.E:
                            S.Play("K2"); switch (selected)
                            {
                                case 1: OpenScreen = "NewGame"; break;
                                case 2: OpenScreen = "Load"; break;
                                case 3: OpenScreen = "Settings"; break;
                                case 4: OpenScreen = "Help"; break;
                            }
                            break;
                    }
                }
                else if ((DateTime.Now - Previously).TotalMilliseconds >= sleep)
                {
                    sleep = 50;
                    PopUp(POPUPID, POPUPTEXT);
                    Previously = DateTime.Now;
                }
            }
            MenuView.Clean();
        }
    }
}
