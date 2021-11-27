using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class OptionsView : MenuManager
    {
        public static void Display()
        {
            ClearList(false);
            string[] names = XF.GetOptionsNames();
            int optionsCount = SettingsService.GetOptionsCount();
            int freeSpace = (Console.WindowHeight - 18);
            int showCount = freeSpace / 3;
            int endlCount = 3;
            if (showCount > optionsCount) endlCount += (showCount - optionsCount) * 3 / 2;

            Endl(3);
            H2(StringService.Get("option label"));
            GroupStart(0);

            GroupStart(2);
            Endl(endlCount);
            for (int i = 0; i < optionsCount; i++)
                if (i <= showCount) TextBox(StringService.Get(names[i]));
                else TextBox(StringService.Get(names[i]), show: false);

            if (optionsCount < showCount) TextBox(StringService.Get("restore default button"));
            else TextBox(StringService.Get("restore default button"), show: false);
            GroupEnd();

            GroupStart(4);
            Endl(endlCount);
            Slider(6, SettingsService.GetOptionViewById(0));
            Slider(6, SettingsService.GetOptionViewById(1));
            Slider(6, SettingsService.GetOptionViewById(2));
            Slider(6, SettingsService.GetOptionViewById(3));
            //slider(6, OPTIONS.getOptionViewById(4));
            GroupEnd();

            GroupStart(Console.WindowWidth / 2 - 10, Console.WindowWidth);
            Endl(Console.WindowHeight - 12);
            TextBox(StringService.Get("reject button", " Q"), 19, margin: 0);
            GroupEnd();
            GroupStart(Console.WindowWidth / 2 + 11, Console.WindowWidth);
            Endl(Console.WindowHeight - 12);
            TextBox(StringService.Get("save button", " E"), 19, margin: 0);
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
