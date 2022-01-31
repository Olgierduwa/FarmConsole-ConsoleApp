using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Text;
using FarmConsole.Body.Views.MenuViews;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Views.GameViews;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Controllers.CentralControllers;

namespace FarmConsole.Body.Controllers.MenuControllers
{
    class EscapeController : HeadController
    {
        public static void Open()
        {
            LastScreen = "Escape";
            int Selected = 1, selCount = 5, selStart = 1;

            ComponentService.SetView = MapEngine.Map;
            EscapeView.Display();
            ComponentService.UpdateMenuSelect(Selected, Selected, selCount);
            while (OpenScreen == "Escape")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (Selected > selStart) { SoundService.Play("K1"); Selected--; ComponentService.UpdateMenuSelect(Selected, Selected + 1, selCount); } break;
                        case ConsoleKey.S: if (Selected < selCount) { SoundService.Play("K1"); Selected++; ComponentService.UpdateMenuSelect(Selected, Selected - 1, selCount); } break;
                        case ConsoleKey.Q:
                        case ConsoleKey.Escape: SoundService.Play("K3"); OpenScreen = EscapeScreen; ComponentService.SetView = null; break;
                        case ConsoleKey.G: SettingsService.GODMOD = !SettingsService.GODMOD; SoundService.Play("K3"); break;
                        case ConsoleKey.E:
                            switch (Selected)
                            {
                                case 1: SoundService.Play("K3"); OpenScreen = EscapeScreen; ComponentService.SetView = null; break;
                                case 2: SoundService.Play("K2"); OpenScreen = "Save"; break;
                                case 3:
                                    if (ComponentService.Danger(LS.Text("exit question"), false) == true)
                                    {
                                        OpenScreen = LastScreen = "Menu";
                                        ComponentService.SetView = null;
                                        MapEngine.Map = null;
                                        GameInstance = null;
                                        ComponentService.Clean(true, true);
                                        ColorService.ColorVisibility = true;
                                    }
                                    break;
                                case 4: SoundService.Play("K2"); OpenScreen = "Settings"; break;
                                case 5: SoundService.Play("K2"); OpenScreen = "Help"; break;
                            }
                            break;
                    }
                }
                else if ((DateTime.Now - Previously).TotalMilliseconds >= FrameTime)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Previously = DateTime.Now;
                }
            }
            ComponentService.Clean();
        }
    }
}
