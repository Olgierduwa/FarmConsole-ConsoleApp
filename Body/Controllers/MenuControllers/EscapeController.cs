using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Text;
using FarmConsole.Body.Views.MenuViews;
using FarmConsole.Body.Services;

namespace FarmConsole.Body.Controlers
{
    public class EscapeController : MainControllerService
    {
        public static void Open()
        {
            lastScreen = "Escape";
            int selected = 1, selCount = 5, selStart = 1;

            EscapeView.Show();
            EscapeView.UpdateSelect(selected, selected, selCount);
            while (openScreen == "Escape")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; EscapeView.UpdateSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; EscapeView.UpdateSelect(selected, selected - 1, selCount); } break;
                        case ConsoleKey.Escape: S.Play("K3"); openScreen = escapeScreen; EscapeView.ClearList(); break;
                        case ConsoleKey.Q: S.Play("K3"); openScreen = escapeScreen; break;
                        case ConsoleKey.E:
                            switch (selected)
                            {
                                case 1: S.Play("K3"); openScreen = escapeScreen; break;
                                case 2: S.Play("K2"); openScreen = "Save"; break;
                                case 3: if (EscapeView.Warning() == true) { openScreen = lastScreen = "Menu"; GameInstance = new GameInstanceModel(); } break;
                                case 4: S.Play("K2"); openScreen = "Options"; break;
                                case 5: S.Play("K2"); openScreen = "Help"; break;
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
            EscapeView.ClearList();
        }
    }
}
