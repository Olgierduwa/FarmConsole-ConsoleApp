using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controlers
{
    class SettingsController : MainController
    {
        private static List<SettingModel> Settings;
        private static int selected;
        private static int selCount;
        private static bool Edited;
        private static bool ChangedLanguage;

        private static void UpdateSelect(int value)
        {
            if (value < 0)
            {
                if (selected > 0)
                {
                    S.Play("K1");
                    selected--;
                    SettingsView.UpdateMenuSelect(selected + 1, selected + 2, selCount, 2, 15);
                    SettingsView.UpdateMenuSelectOnly(selected + 1, selected + 2, selCount, 3, 15);
                    SettingsView.DisplayLastListElement();
                }
            }
            else
            {
                if (selected < selCount - 1)
                {
                    S.Play("K1");
                    selected++;
                    SettingsView.UpdateMenuSelect(selected + 1, selected, selCount, 2, 15);
                    SettingsView.UpdateMenuSelectOnly(selected + 1, selected, selCount, 3, 15);
                    SettingsView.DisplayLastListElement();
                }
            }
        }
        private static void UpdateSlider(int value)
        {
            if (selected < selCount - 2 && Settings[selected].SetSliderValue(value))
            {
                Edited = true;
                S.Play("K3");
                switch (Settings[selected].Key)
                {
                    case "set effects volume": S.SetSoundVolume(); break;
                    case "set music volume": S.SetMusicVolume(); break;
                }
                SettingsView.UpdateMenuSlider(selected + 1, Settings[selected].GetMaxSliderValue, value);
            }
            else if (selected == selCount - 2)
            {
                Edited = true;
                ChangedLanguage = true;
                SettingsService.SetLanguage(value);
                SettingsView.UpdateMenuTextBox(selected + 1, SettingsService.GetLanguage(), 3);
            }
        }
        private static void UpdateSettings()
        {
            S.Play("K2");
            if (selected == selCount - 1) { SettingsService.RestoreDefaultSettings(); Edited = true; }
            else SettingsService.SaveSettings();

            if (ChangedLanguage)
            {
                ChangedLanguage = false;
                SettingsService.LoadLanguages();
            }

            if (Edited)
            {
                MenuManager.Clean(true);
                WindowService.SetWindow();
                MenuManager.Captions();
                if (LastScreen != "Menu") GameInstance.ReloadMaps(EscapeScreen);

                SettingsView.Display();
                MenuManager.SetView = MapEngine.Map;
                selected = 0;
                SettingsView.UpdateMenuSelect(selected + 1, selected + 1, selCount);
                SettingsView.DisplayLastListElement();
                Edited = false;
            }
        }
        private static void GoBack()
        {
            SettingsService.LoadSettings();
            S.SetSoundVolume();
            S.SetMusicVolume();
            S.Play("K3");
            OpenScreen = LastScreen;
        }

        public static void Open()
        {
            Edited = ChangedLanguage = false;
            Settings = SettingsService.GetSettings;
            selected = 0; selCount = Settings.Count + 2;
            SettingsView.Display(); SettingsView.UpdateMenuSelect(selected + 1, selected + 1, selCount);
            SettingsView.DisplayLastListElement();
            while (OpenScreen == "Settings")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: UpdateSelect(-1); break;
                        case ConsoleKey.S: UpdateSelect(+1); break;
                        case ConsoleKey.A: UpdateSlider(-1); break;
                        case ConsoleKey.D: UpdateSlider(+1); break;
                        case ConsoleKey.E: UpdateSettings(); break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: GoBack(); break;
                    }
                }
                else if ((DateTime.Now - Previously).TotalMilliseconds >= FrameTime)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Previously = DateTime.Now;
                }
            }
            SettingsView.Clean();
        }
    }
}
