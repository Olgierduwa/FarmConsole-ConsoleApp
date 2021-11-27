using FarmConsole.Body.Engines;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controlers
{
    class OptionsController : MainController
    {
        public static void Open()
        {
            int[] opt = SettingsService.GetOptionsView();
            int selected = 1, selCount = SettingsService.GetOptionsCount() + 1, selStart = 1, MaxSliderValue = 6;
            OptionsView.Display(); OptionsView.UpdateMenuSelect(selected, selected, selCount);
            while (OpenScreen == "Options")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; OptionsView.UpdateMenuSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; OptionsView.UpdateMenuSelect(selected, selected - 1, selCount); } break;

                        case ConsoleKey.A:
                            if (selected < selCount && opt[selected - 1] > 0)
                            {
                                switch (selected)
                                {
                                    case 3: S.SetSoundVolume(opt[selected - 1]); break;
                                    case 4: S.SetMusicVolume(opt[selected - 1]); break;
                                }
                                OptionsView.UpdateMenuSlider(selected, MaxSliderValue, -1);
                                opt[selected - 1]--;
                                S.Play("K3");
                            }
                            break;

                        case ConsoleKey.D:
                            if (selected < selCount && opt[selected - 1] < MaxSliderValue)
                            {
                                switch (selected)
                                {
                                    case 3: S.SetSoundVolume(opt[selected - 1]); break;
                                    case 4: S.SetMusicVolume(opt[selected - 1]); break;
                                }
                                OptionsView.UpdateMenuSlider(selected, MaxSliderValue, 1);
                                opt[selected - 1]++;
                                S.Play("K2");
                            }
                            break;

                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: S.Play("K3"); S.SetSoundVolume(); S.SetMusicVolume(); OpenScreen = LastScreen; break;
                        case ConsoleKey.E: S.Play("K2"); MenuManager.Clean(true);
                            if (selected != selCount) SettingsService.SaveOptions(opt); else SettingsService.ResetOptions();
                            MenuManager.Captions(); if(LastScreen != "Menu") GameInstance.ReloadMaps(EscapeScreen); OptionsView.Display();
                            selected = 1; OptionsView.UpdateMenuSelect(selected, selected, selCount); opt = SettingsService.GetOptionsView(); break;
                    }
                }
                else if ((DateTime.Now - Previously).TotalMilliseconds >= FrameTime)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Previously = DateTime.Now;
                }
            }
            OptionsView.Clean();
        }
    }
}
