using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    public class OptionsView : MenuViewService
    {
        public static void Show()
        {
            ClearList(false);
            string[] names = XF.GetOptionsNames();
            int optionsCount = SettingsService.GetOptionsCount();
            int freeSpace = (Console.WindowHeight - 18);
            int showCount = freeSpace / 3;
            int endlCount = 3;
            if (showCount > optionsCount) endlCount += (showCount - optionsCount) * 3 / 2;

            H1(title);
            H2("Ustaw Spersonalizowane Opcje");
            GroupStart(0);

            GroupStart(2);
            Endl(endlCount);
            for (int i = 0; i < optionsCount; i++)
                if (i <= showCount) TextBox(names[i]);
                else TextBox(names[i], show: false);

            if (optionsCount < showCount) TextBox("Przywróć Domyślne Ustawienia");
            else TextBox("Przywróć Domyślne Ustawienia", show: false);
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
            TextBox("Q / Odrzuć", 19, margin: 0);
            GroupEnd();
            GroupStart(Console.WindowWidth / 2 + 11, Console.WindowWidth);
            Endl(Console.WindowHeight - 12);
            TextBox("E / Zapisz", 19, margin: 0);
            GroupEnd();

            GroupEnd();
            Foot(foot);
            PrintList();
            //showComponentList();
        }
    }
}
