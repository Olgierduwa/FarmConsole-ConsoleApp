using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Text;
using FarmConsole.Body.Views.MenuViews;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Controllers.CentralControllers;

namespace FarmConsole.Body.Controllers.MenuControllers
{
    class GameLoadController : HeadController
    {
        public static void Open()
        {
            GameInstanceModel[] saves = XF.GetGameInstances();
            int Selected = 1, selCount = saves.Length + 1, selStart = 1;
            GameLoadView.Display(saves); ComponentService.UpdateMenuSelect(Selected, Selected, selCount);
            while (OpenScreen == "Load")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (Selected > selStart) { SoundService.Play("K1"); Selected--; ComponentService.UpdateMenuSelect(Selected, Selected + 1, selCount); } break;
                        case ConsoleKey.S: if (Selected < selCount) { SoundService.Play("K1"); Selected++; ComponentService.UpdateMenuSelect(Selected, Selected - 1, selCount); } break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: SoundService.Play("K3"); OpenScreen = "Menu"; break;
                        case ConsoleKey.E:
                            SoundService.Play("K2");
                            switch (Selected)
                            {
                                case 1: OpenScreen = "NewGame"; break;
                                default: GameInstance = new GameInstanceModel(Selected - 1); OpenScreen = GameInstance.Lastmap; break;
                            }
                            break;
                        case ConsoleKey.D:
                            if (Selected > 1 && ComponentService.Danger(LS.Text("delete question")) == true)
                            {
                                saves[Selected - 2].Delete();
                                saves = XF.GetGameInstances();
                                Selected = 1;
                                selCount = saves.Length + 1;
                                ComponentService.Clean();
                                GameLoadView.Display(saves);
                                ComponentService.UpdateMenuSelect(1, 1, selCount);
                            }
                            break;
                    }
                    GameLoadView.SetPreview(saves, Selected);
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
