using FarmConsole.Body.Controllers.CentralControllers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controllers.MenuControllers
{
    class SettingsController : HeadController
    {
        private static List<SettingModel> Settings;
        private static int Selected;
        private static int selCount;
        private static bool Edited;
        private static bool ChangedLanguage;

        private static void UpdateSelect(int value)
        {
            if (value < 0)
            {
                if (Selected > 0)
                {
                    SoundService.Play("K1");
                    Selected--;
                    ComponentService.UpdateMenuSelect(Selected + 1, Selected + 2, selCount, 2, 15);
                    ComponentService.UpdateMenuSelectOnly(Selected + 1, Selected + 2, selCount, 3, 15);
                    ComponentService.DisplayLastListElement(3);
                }
            }
            else
            {
                if (Selected < selCount - 1)
                {
                    SoundService.Play("K1");
                    Selected++;
                    ComponentService.UpdateMenuSelect(Selected + 1, Selected, selCount, 2, 15);
                    ComponentService.UpdateMenuSelectOnly(Selected + 1, Selected, selCount, 3, 15);
                    ComponentService.DisplayLastListElement(3);
                }
            }
        }
        private static void UpdateSlider(int value)
        {
            if (Selected < selCount - 2 && Settings[Selected].SetSliderValue(value))
            {
                Edited = true;
                SoundService.Play("K3");
                switch (Settings[Selected].Key)
                {
                    case "set effects volume": SoundService.SetSoundVolume(); break;
                    case "set music volume": SoundService.SetMusicVolume(); break;
                }
                ComponentService.UpdateMenuSlider(Selected + 1, Settings[Selected].GetMaxSliderValue, value);
            }
            else if (Selected == selCount - 2)
            {
                Edited = true;
                ChangedLanguage = true;
                SettingsService.SetLanguage(value);
                ComponentService.UpdateMenuTextBox(Selected + 1, SettingsService.GetLanguage(), 3);
            }
        }
        private static void UpdateSettings()
        {
            SoundService.Play("K2");
            if (Selected == selCount - 1) { SettingsService.RestoreDefaultSettings(); Edited = true; }
            else SettingsService.SaveSettings();

            if (ChangedLanguage)
            {
                ChangedLanguage = false;
                SettingsService.LoadLanguages();
            }

            if (Edited)
            {
                ComponentService.Clean(true);
                WindowService.SetWindow();
                ComponentService.Captions();
                if (LastScreen != "Menu") GameInstance.ReloadMaps(EscapeScreen);

                SettingsView.Display();
                ComponentService.SetView = MapEngine.Map;
                Selected = 0;
                ComponentService.UpdateMenuSelect(Selected + 1, Selected + 1, selCount);
                ComponentService.DisplayLastListElement(3);
                Edited = false;
            }
        }
        private static void GoBack()
        {
            SettingsService.LoadSettings();
            SoundService.SetSoundVolume();
            SoundService.SetMusicVolume();
            SoundService.Play("K3");
            OpenScreen = LastScreen;
        }

        public static void Open()
        {
            Edited = ChangedLanguage = false;
            Settings = SettingsService.GetSettings;
            Selected = 0; selCount = Settings.Count + 2;
            SettingsView.Display(); ComponentService.UpdateMenuSelect(Selected + 1, Selected + 1, selCount);
            ComponentService.DisplayLastListElement(3);
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
            ComponentService.Clean();
        }
    }
}
