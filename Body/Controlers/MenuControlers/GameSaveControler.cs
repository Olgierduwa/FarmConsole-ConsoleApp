using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;
using FarmConsole.Body.Views.MenuViews;

namespace FarmConsole.Body.Controlers
{
    public class GameSaveControler : MenuControlerService
    {
        public static void Open()
        {
            SaveModel[] saves = XF.GetSaves();
            int selected = 1, selCount = saves.Length + 1, selStart = 1;
            GameSaveView.Show(saves); GameSaveView.UpdateSelect(selected, selected, selCount);
            while (openScreen == "Save")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GameSaveView.UpdateSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GameSaveView.UpdateSelect(selected, selected - 1, selCount); } break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: S.Play("K3"); openScreen = "Escape"; break;
                        case ConsoleKey.E:
                            if (selected == 1 || GameSaveView.Warning() == true)
                            {
                                save.Update(selected - 1);
                                saves = XF.GetSaves();
                                selected = 1;
                                selCount = saves.Length + 1;
                                GameSaveView.ClearList();
                                GameSaveView.Show(saves);
                                GameSaveView.UpdateSelect(1, 1, selCount);
                            }
                            break;
                    }
                    GameSaveView.SetPreview(saves, selected);
                }
                else if ((DateTime.Now - Now).TotalMilliseconds >= 50)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Now = DateTime.Now;
                }
            }
            GameSaveView.ClearList();
        }
    }
}
