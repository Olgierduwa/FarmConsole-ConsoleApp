using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controlers
{
    public class OptionsControler : MenuControlerService
    {
        public static void Open()
        {
            int[] opt = SettingsService.GetOptionsView();
            int selected = 1, selCount = SettingsService.GetOptionsCount() + 1, selStart = 1, R = 6;
            OptionsView.Show(); OptionsView.UpdateSelect(selected, selected, selCount);
            while (openScreen == "Options")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; OptionsView.UpdateSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; OptionsView.UpdateSelect(selected, selected - 1, selCount); } break;

                        case ConsoleKey.A:
                            if (selected < selCount && opt[selected - 1] > 0)
                            {
                                switch (selected)
                                {
                                    case 3: S.SetSoundVolume(opt[selected - 1]); break;
                                    case 4: S.SetMusicVolume(opt[selected - 1]); break;
                                }
                                OptionsView.UpdateSlider(3, selected, R, --opt[selected - 1]);
                                S.Play("K3");
                            }
                            break;

                        case ConsoleKey.D:
                            if (selected < selCount && opt[selected - 1] < R)
                            {
                                switch (selected)
                                {
                                    case 3: S.SetSoundVolume(opt[selected - 1]); break;
                                    case 4: S.SetMusicVolume(opt[selected - 1]); break;
                                }
                                OptionsView.UpdateSlider(3, selected, R, ++opt[selected - 1]);
                                S.Play("K2");
                            }
                            break;

                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: S.Play("K3"); S.SetSoundVolume(); S.SetMusicVolume(); openScreen = lastScreen; break;
                        case ConsoleKey.E:
                            S.Play("K2"); OptionsView.ClearList(); if (selected == selCount) SettingsService.ResetOptions(); else SettingsService.SaveOptions(opt);
                            OptionsView.Show(); selected = 1; OptionsView.UpdateSelect(selected, selected, selCount); opt = SettingsService.GetOptionsView(); break;
                    }
                }
                else if ((DateTime.Now - Now).TotalMilliseconds >= 50)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Now = DateTime.Now;
                }
            }
            OptionsView.ClearList();
        }
    }
}
