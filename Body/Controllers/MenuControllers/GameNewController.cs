using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Text;
using FarmConsole.Body.Views.MenuViews;
using FarmConsole.Body.Services;

namespace FarmConsole.Body.Controlers
{
    class GameNewController : MainController
    {
        public static void Open()
        {
            int selected = 1, selCount = 4, selStart = 1, MaxSliderValue = 4;
            GameNewView.Display(); GameNewView.UpdateMenuSelect(selected, selected, selCount);
            while (OpenScreen == "NewGame")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GameNewView.UpdateMenuSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GameNewView.UpdateMenuSelect(selected, selected - 1, selCount); } break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: S.Play("K3"); OpenScreen = "Menu"; break;
                        case ConsoleKey.E:
                            {
                                S.Play("K2");
                                int Difficulty = GameNewView.GetSliderValue(2);
                                int Gender = GameNewView.GetSliderValue(3);
                                GameInstance = new GameInstanceModel("Olgierd", Difficulty, Gender);
                                OpenScreen = "Farm";
                            }
                            break;

                        case ConsoleKey.A:
                            if (selected == selCount) ; // generowanie losowego awatara
                            else if (selected > 1 && GameNewView.GetSliderValue(selected) > 0)
                            { GameNewView.UpdateMenuSlider(selected, MaxSliderValue, -1); S.Play("K3"); }
                            break;

                        case ConsoleKey.D:
                            if (selected == selCount) ; // generowanie wlasnego awatera
                            else if (selected > 1 && GameNewView.GetSliderValue(selected) < MaxSliderValue)
                            { GameNewView.UpdateMenuSlider(selected, MaxSliderValue, 1); S.Play("K2"); }
                            break;
                    }
                }
                else if ((DateTime.Now - Previously).TotalMilliseconds >= FrameTime)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Previously = DateTime.Now;
                }
            }
            GameNewView.Clean();
        }
    }
}
