using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class SettingsView : MenuManager
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
                else TextBox(Settings[i].Name, show: false);

            if (Settings.Count < showCount) TextBox(LS.Navigation("set language"));
            else TextBox(LS.Navigation("set language"), show: false);

            if (Settings.Count + 1 < showCount) TextBox(LS.Navigation("set restore default"));
            else TextBox(LS.Navigation("set restore default"), show: false);

            GroupEnd();

            GroupStart(4);
            Endl(endlCount);
            for (int i = 0; i < Settings.Count; i++)
                if (i < showCount) Slider(Settings[i].GetMaxSliderValue, Settings[i].GetSliderValue);
                else Slider(Settings[i].GetMaxSliderValue, Settings[i].GetSliderValue, show: false);

            if (Settings.Count < showCount) TextBox(SettingsService.GetLanguage());
            else TextBox(SettingsService.GetLanguage(), show: false);

            if (Settings.Count + 1 < showCount) TextBox("");
            else TextBox("", show: false);

            GroupEnd();

            GroupStart(Console.WindowWidth / 2 - 10, Console.WindowWidth);
            Endl(Console.WindowHeight - 12);
            TextBox(LS.Navigation("reject button", " Q"), 19, margin: 0);
            GroupEnd();

            GroupStart(Console.WindowWidth / 2 + 11, Console.WindowWidth);
            Endl(Console.WindowHeight - 12);
            TextBox(LS.Navigation("save button", " E"), 19, margin: 0);
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
