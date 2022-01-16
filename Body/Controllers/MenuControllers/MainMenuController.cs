using FarmConsole.Body.Controllers.CentralControllers;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controllers.MenuControllers
{
    class MainMenuController : HeadController
    {
        public static void Open()
        {
            int Selected = 1, selCount = 4, selStart = 1, sleep = 1000;

            MenuView.Display();
            ComponentService.UpdateMenuSelect(Selected, Selected, selCount);
            while (OpenScreen == "Menu")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        //case ConsoleKey.R: POPUPID = 1; POPUPSTAGE = 0; break;
                        case ConsoleKey.T: OpenScreen = "Intro"; break;
                        case ConsoleKey.W: if (Selected > selStart) { SoundService.Play("K1"); Selected--; ComponentService.UpdateMenuSelect(Selected, Selected + 1, selCount); } break;
                        case ConsoleKey.S: if (Selected < selCount) { SoundService.Play("K1"); Selected++; ComponentService.UpdateMenuSelect(Selected, Selected - 1, selCount); } break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: if (ComponentService.Danger() == true) OpenScreen = "Close"; break;
                        case ConsoleKey.G: SettingsService.GODMOD = !SettingsService.GODMOD; SoundService.Play("K3"); break;
                        case ConsoleKey.E:
                            SoundService.Play("K2"); switch (Selected)
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
            ComponentService.Clean();
        }
    }
}
