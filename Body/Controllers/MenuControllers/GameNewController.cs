using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Text;
using FarmConsole.Body.Views.MenuViews;
using FarmConsole.Body.Services;

namespace FarmConsole.Body.Controlers
{
    public class GameNewController : MainController
    {
        public static void Open()
        {
            int selected = 1, selCount = 2, selStart = 1;
            GameNewView.Show(); GameNewView.UpdateSelect(selected, selected, selCount);
            while (openScreen == "NewGame")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GameNewView.UpdateSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GameNewView.UpdateSelect(selected, selected - 1, selCount); } break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: S.Play("K3"); openScreen = "Menu"; break;
                        case ConsoleKey.E:
                            switch (selected)
                            {
                                case 1: S.Play("K2"); openScreen = "Farm"; GameInstance = new GameInstanceModel("Asia"); break;
                                case 2: S.Play("K2"); openScreen = "Farm"; GameInstance = new GameInstanceModel("Olgierd"); break;
                            }
                            break;
                    }
                }
                else if ((DateTime.Now - Now).TotalMilliseconds >= 50)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Now = DateTime.Now;
                }
            }
            GameNewView.ClearList();
        }
    }
}
