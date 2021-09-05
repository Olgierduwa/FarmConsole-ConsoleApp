using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;
using FarmConsole.Body.Views.MenuViews;

namespace FarmConsole.Body.Controlers
{
    public class GameLoadController : MainControllerService
    {
        public static void Open()
        {
            SaveModel[] saves = XF.GetSaves();
            int selected = 1, selCount = saves.Length + 1, selStart = 1;
            GameLoadView.Show(saves); GameLoadView.UpdateSelect(selected, selected, selCount);
            while (openScreen == "Load")
            {

                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GameLoadView.UpdateSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GameLoadView.UpdateSelect(selected, selected - 1, selCount); } break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: S.Play("K3"); openScreen = "Menu"; break;
                        case ConsoleKey.E: S.Play("K2"); switch (selected) { case 1: openScreen = "NewGame"; break; default: MainControllerService.save.Load(selected - 1); openScreen = "Farm"; break; } break;
                        case ConsoleKey.D:
                            if (selected > 1 && GameLoadView.Warning() == true)
                            {
                                saves[selected - 2].Delete();
                                saves = XF.GetSaves();
                                selected = 1;
                                selCount = saves.Length + 1;
                                GameLoadView.ClearList();
                                GameLoadView.Show(saves);
                                GameLoadView.UpdateSelect(1, 1, selCount);
                            }
                            break;
                    }
                    GameLoadView.SetPreview(saves, selected);
                }
                else if ((DateTime.Now - Now).TotalMilliseconds >= 50)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Now = DateTime.Now;
                }
            }
            GameLoadView.ClearList();
        }
    }
}
