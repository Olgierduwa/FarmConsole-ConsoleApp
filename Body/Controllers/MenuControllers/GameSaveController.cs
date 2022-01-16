using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Text;
using FarmConsole.Body.Views.MenuViews;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Controllers.CentralControllers;

namespace FarmConsole.Body.Controllers.MenuControllers
{
    class GameSaveController : HeadController
    {
        public static void Open()
        {
            GameInstanceModel[] saves = XF.GetGameInstances();
            int Selected = 1, prevSelected = 1, selCount = saves.Length + 1, selStart = 1;
            GameSaveView.DisplayList(saves); ComponentService.UpdateMenuSelect(Selected, Selected, selCount);
            //GameSaveView.DisplayReview(saves, Selected);
            while (OpenScreen == "Save")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (Selected > selStart) { SoundService.Play("K1"); Selected--; ComponentService.UpdateMenuSelect(Selected, Selected + 1, selCount); } break;
                        case ConsoleKey.S: if (Selected < selCount) { SoundService.Play("K1"); Selected++; ComponentService.UpdateMenuSelect(Selected, Selected - 1, selCount); } break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: SoundService.Play("K3"); OpenScreen = "Escape"; break;
                        case ConsoleKey.E:
                            if (Selected == 1 || ComponentService.Danger() == true)
                            {
                                GameInstance.Lastmap = EscapeScreen;
                                GameInstance.Update(Selected - 1);
                                saves = XF.GetGameInstances();
                                Selected = 1;
                                selCount = saves.Length + 1;
                                ComponentService.Clean();
                                GameSaveView.DisplayList(saves);
                                ComponentService.UpdateMenuSelect(1, 1, selCount);
                            }
                            break;
                    }
                    if (Selected != prevSelected)
                    {
                        GameSaveView.DisplayReview(saves, Selected, selCount);
                        prevSelected = Selected;
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
