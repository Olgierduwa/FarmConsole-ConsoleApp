using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;
using FarmConsole.Body.Views.MenuViews;

namespace FarmConsole.Body.Controlers
{
    class GameLoadController : MainController
    {
        public static void Open()
        {
            GameInstanceModel[] saves = XF.GetGameInstances();
            int selected = 1, selCount = saves.Length + 1, selStart = 1;
            GameLoadView.Display(saves); GameLoadView.UpdateMenuSelect(selected, selected, selCount);
            while (OpenScreen == "Load")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GameLoadView.UpdateMenuSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GameLoadView.UpdateMenuSelect(selected, selected - 1, selCount); } break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: S.Play("K3"); OpenScreen = "Menu"; break;
                        case ConsoleKey.E: S.Play("K2");
                            switch (selected)
                            {
                                case 1: OpenScreen = "NewGame"; break;
                                default: GameInstance = new GameInstanceModel(); GameInstance.Load(selected - 1); OpenScreen = "Farm"; break;
                            }
                            break;
                        case ConsoleKey.D:
                            if (selected > 1 && GameLoadView.Warning() == true)
                            {
                                saves[selected - 2].Delete();
                                saves = XF.GetGameInstances();
                                selected = 1;
                                selCount = saves.Length + 1;
                                GameLoadView.Clean();
                                GameLoadView.Display(saves);
                                GameLoadView.UpdateMenuSelect(1, 1, selCount);
                            }
                            break;
                    }
                    GameLoadView.SetPreview(saves, selected);
                }
                else if ((DateTime.Now - Previously).TotalMilliseconds >= FrameTime)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Previously = DateTime.Now;
                }
            }
            GameLoadView.Clean();
        }
    }
}
