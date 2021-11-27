using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class GameLoadView : MenuManager
    {
        public static void Display(GameInstanceModel[] saves)
        {
            int savesCount = saves.Length;
            int freeSpace = Console.WindowHeight - 15;
            int showCount = freeSpace / 3;
            int endlCount = 3;
            int detailsHeight = (Console.WindowHeight - 24) / 2;
            if (showCount >= savesCount + 1) endlCount += (showCount - savesCount + 1) * 3 / 2;

            Endl(3);
            H2(StringService.Get("load game label"));

            GroupStart(0);

            GroupStart(2);
            if ((savesCount + 1) * 3 <= 17) Endl(detailsHeight);
            else Endl(endlCount);
            TextBox(StringService.Get("empty save button"));
            for (int i = 0; i < savesCount; i++)
                if (i < showCount) TextBox(saves[i].UserName);
                else TextBox(saves[i].UserName, 40, false);
            GroupEnd();

            GroupStart(4);
            Endl(detailsHeight);
            TextBox(StringService.Get("new game button", " E"));
            GroupEnd();
            GroupStart(Console.WindowWidth * 4 / 5 - Console.WindowWidth / 10 - 10, Console.WindowWidth);
            Endl(detailsHeight + 14);
            TextBox(StringService.Get("delete save button", " D"), 19, false, margin: 0);
            GroupEnd();
            GroupStart(Console.WindowWidth * 4 / 5 - Console.WindowWidth / 10 + 11, Console.WindowWidth);
            Endl(detailsHeight + 14);
            TextBox(StringService.Get("continue button", " E"), 19, false, margin: 0);
            GroupEnd();

            GroupStart(Console.WindowWidth / 2, Console.WindowWidth);
            Endl(detailsHeight + 1);
            TextBox(deleteQuestion, 33, false, ColorService.GetColorByName("Red"));
            GroupEnd();
            GroupStart(Console.WindowWidth / 2 - 9, Console.WindowWidth);
            Endl(detailsHeight + 9);
            TextBox(StringService.Get("no", " Q"), 15, false, ColorService.GetColorByName("Red"), margin: 0);
            GroupEnd();
            GroupStart(Console.WindowWidth / 2 + 9, Console.WindowWidth);
            Endl(detailsHeight + 9);
            TextBox(StringService.Get("yes", " E"), 15, false, ColorService.GetColorByName("Red"), margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();
            //vt1.showComponentList();
            FocusGroupID = 6;
        }
        public static void SetPreview(GameInstanceModel[] saves, int selected)
        {
            if (selected > 1)
            {
                var save = saves[selected - 2];
                UpdateBox(3, 1, ". . ." + " ---------------------------------- " +
                StringService.Get("nickname label") + " - " + save.UserName.ToString() + " ---------------------------------- " +
                StringService.Get("lvl label") + " - " + save.LVL.ToString() + " ---------------------------------- " +
                StringService.Get("wallet label") + " - " + save.Wallet.ToString() + " ---------------------------------- " +
                StringService.Get("lastplay label") + " - " + save.Lastplay.ToString() + " ---------------------------------- . . .");
                Showability(4, 1, true);
                Showability(5, 1, true);
            }
            else
            {
                UpdateBox(3, 1, StringService.Get("new game button", " E"));
                Showability(3, 3, false);
                Showability(4, 1, false);
                Showability(5, 1, false);
            }
        }
    }
}
