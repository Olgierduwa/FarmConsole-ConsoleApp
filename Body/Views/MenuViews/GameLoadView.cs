using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class GameLoadView : ComponentService
    {
        public static void Display(GameInstanceModel[] saves)
        {
            ClearList(false);
            int savesCount = saves.Length;
            int freeSpace = Console.WindowHeight - 15;
            int showCount = freeSpace / 3;
            int endlCount = 3;
            int detailsHeight = (Console.WindowHeight - 24) / 2;
            if (showCount >= savesCount + 1) endlCount += (showCount - savesCount + 1) * 3 / 2;

            Endl(3);
            H2(LS.Navigation("load game label"));

            GroupStart(0);

            GroupStart(2);
            if ((savesCount + 1) * 3 <= 17) Endl(detailsHeight);
            else Endl(endlCount);
            TextBox(LS.Navigation("empty save button"));
            for (int i = 0; i < savesCount; i++)
                if (i < showCount) TextBox(saves[i].UserName);
                else TextBox(saves[i].UserName, 40, false);
            GroupEnd();

            GroupStart(4);
            Endl(detailsHeight);
            TextBox(LS.Navigation("new game button", " E"));
            GroupEnd();

            GroupStart(Console.WindowWidth * 4 / 5 - Console.WindowWidth / 10 - 10, Console.WindowWidth);
            Endl(detailsHeight + 14);
            TextBox(LS.Navigation("delete save button", " D"), 19, false, Margin: 0);
            GroupEnd();

            GroupStart(Console.WindowWidth * 4 / 5 - Console.WindowWidth / 10 + 11, Console.WindowWidth);
            Endl(detailsHeight + 14);
            TextBox(LS.Navigation("continue button", " E"), 19, false, Margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();
            //vt1.showComponentList();
        }

        public static void SetPreview(GameInstanceModel[] saves, int Selected)
        {
            if (Selected > 1)
            {
                var save = saves[Selected - 2];
                UpdateTextBox(3, 1, ". . ." + " ---------------------------------- " +
                LS.Navigation("nickname label") + " - " + save.UserName.ToString() + " ---------------------------------- " +
                LS.Navigation("lvl label") + " - " + save.LVL.ToString() + " ---------------------------------- " +
                LS.Navigation("wallet label") + " - " + save.WalletFunds.ToString() + " ---------------------------------- " +
                LS.Navigation("lastplay label") + " - " + save.Lastplay.ToString() + " ---------------------------------- . . .");
                SetShowability(4, 1, true);
                SetShowability(5, 1, true);
            }
            else
            {
                UpdateTextBox(3, 1, LS.Navigation("new game button", " E"));
                SetShowability(4, 1, false);
                SetShowability(5, 1, false);
            }
        }
    }
}
