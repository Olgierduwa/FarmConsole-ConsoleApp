using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class SettingsView : ComponentService
    {
        public static void Display()
        {
            ClearList(false);

            List<SettingModel> Settings = SettingsService.GetSettings;

            int showCount = (Console.WindowHeight - 15) / 3;
            int endlCount = 3;
            if (showCount > Settings.Count + 2) endlCount += (showCount - Settings.Count - 2) * 3 / 2;

            Endl(3);
            H2(LS.Navigation("setting label"));
            GroupStart(0);

            GroupStart(2);
            Endl(endlCount);
            for (int i = 0; i < Settings.Count; i++)
                if (i < showCount) TextBox(Settings[i].Name);
                else TextBox(Settings[i].Name, Show: false);

            if (Settings.Count < showCount) TextBox(LS.Navigation("set language"));
            else TextBox(LS.Navigation("set language"), Show: false);

            if (Settings.Count + 1 < showCount) TextBox(LS.Navigation("set restore default"));
            else TextBox(LS.Navigation("set restore default"), Show: false);

            GroupEnd();

            GroupStart(4);
            Endl(endlCount);
            for (int i = 0; i < Settings.Count; i++)
                if (i < showCount) Slider(Settings[i].GetMaxSliderValue, Settings[i].GetSliderValue);
                else Slider(Settings[i].GetMaxSliderValue, Settings[i].GetSliderValue, show: false);

            if (Settings.Count < showCount) TextBox(SettingsService.GetLanguage());
            else TextBox(SettingsService.GetLanguage(), Show: false);

            if (Settings.Count + 1 < showCount) TextBox("");
            else TextBox("", Show: false);

            GroupEnd();

            GroupStart(Console.WindowWidth / 2 - 10, Console.WindowWidth);
            Endl(Console.WindowHeight - 12);
            TextBox(LS.Navigation("reject button", " Q"), 19, Margin: 0);
            GroupEnd();

            GroupStart(Console.WindowWidth / 2 + 11, Console.WindowWidth);
            Endl(Console.WindowHeight - 12);
            TextBox(LS.Navigation("save button", " E"), 19, Margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();

            ComponentsDisplayed = new List<CM>();
            ComponentsDisplayed.Add(GetComponentByName("GS", 2));
            ComponentsDisplayed.Add(GetComponentByName("GS", 3));
            ComponentsDisplayed.Add(GetComponentByName("GS", 4));
            ComponentsDisplayed.Add(GetComponentByName("GS", 5));
            //showComponentList();
        }
    }
}
