using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;
using FarmConsole.Body.Views.MenuViews;

namespace FarmConsole.Body.Controlers
{
    class GameSaveController : MainController
    {
        public static void Open()
        {
            GameInstanceModel[] saves = XF.GetGameInstances();
            int selected = 1, selCount = saves.Length + 1, selStart = 1;
            GameSaveView.Display(saves); GameSaveView.UpdateMenuSelect(selected, selected, selCount);
            while (OpenScreen == "Save")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GameSaveView.UpdateMenuSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GameSaveView.UpdateMenuSelect(selected, selected - 1, selCount); } break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: S.Play("K3"); OpenScreen = "Escape"; break;
                        case ConsoleKey.E:
                            if (selected == 1 || GameSaveView.Warning() == true)
                            {
                                GameInstance.Update(selected - 1);
                                saves = XF.GetGameInstances();
                                selected = 1;
                                selCount = saves.Length + 1;
                                GameSaveView.Clean();
                                GameSaveView.Display(saves);
                                GameSaveView.UpdateMenuSelect(1, 1, selCount);
                            }
                            break;
                    }
                    GameSaveView.SetPreview(saves, selected);
                }
                else if ((DateTime.Now - Previously).TotalMilliseconds >= FrameTime)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Previously = DateTime.Now;
                }
            }
            GameSaveView.Clean();
        }
    }
}
