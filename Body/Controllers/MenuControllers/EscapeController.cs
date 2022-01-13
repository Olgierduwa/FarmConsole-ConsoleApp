using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Text;
using FarmConsole.Body.Views.MenuViews;
using FarmConsole.Body.Services;
using FarmConsole.Body.Engines;

namespace FarmConsole.Body.Controlers
{
    class EscapeController : MainController
    {
        public static void Open()
        {
            LastScreen = "Escape";
            int selected = 1, selCount = 5, selStart = 1;

            MenuManager.SetView = MapEngine.Map;
            EscapeView.Display();
            EscapeView.UpdateMenuSelect(selected, selected, selCount);
            while (OpenScreen == "Escape")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; EscapeView.UpdateMenuSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; EscapeView.UpdateMenuSelect(selected, selected - 1, selCount); } break;
                        case ConsoleKey.Q:
                        case ConsoleKey.Escape: S.Play("K3"); OpenScreen = EscapeScreen; MenuView.SetView = null; break;
                        case ConsoleKey.G: SettingsService.GODMOD = !SettingsService.GODMOD; S.Play("K3"); break;
                        case ConsoleKey.E:
                            switch (selected)
                            {
                                case 1: S.Play("K3"); OpenScreen = EscapeScreen; MenuView.SetView = null; break;
                                case 2: S.Play("K2"); OpenScreen = "Save"; break;
                                case 3: if (EscapeView.Danger(false) == true) { OpenScreen = LastScreen = "Menu"; MenuView.SetView = null;
                                        MapEngine.Map = null; GameInstance = null; GameView.Clean(true, true); } break;
                                case 4: S.Play("K2"); OpenScreen = "Settings"; break;
                                case 5: S.Play("K2"); OpenScreen = "Help"; break;
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
            EscapeView.Clean();
        }
    }
}
