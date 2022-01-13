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
    class GameSaveView : MenuManager
    {
        public static void DisplayList(GameInstanceModel[] saves)
        {
            int savesCount = saves.Length;
            int freeSpace = Console.WindowHeight - 15;
            int showCount = freeSpace / 3;
            int endlCount = 3;
            int detailsHeight = (Console.WindowHeight - 24) / 2;
            if (showCount >= savesCount + 1) endlCount += (showCount - savesCount + 1) * 3 / 2;

            ClearList(false);

            Endl(3);
            H2(LS.Navigation("save game label"));

            GroupStart(0);

            GroupStart(2);
            if ((savesCount + 1) * 3 <= 17) Endl(detailsHeight);
            else Endl(endlCount);
            TextBox(LS.Navigation("empty save button"));
            for (int i = 0; i < savesCount; i++)
                if (i < showCount)
                    TextBox(saves[i].UserName);
                else TextBox(saves[i].UserName, show: false);
            GroupEnd();

            GroupStart(4);
            Endl(detailsHeight);
            TextBox(LS.Navigation("new save button", " E"));
            Endl(11);
            TextBox(LS.Navigation("override save button", " E"), show: false);
            GroupEnd();

            GroupEnd();
            PrintList();
            //showComponentList();

            ComponentsDisplayed = new List<CM>();
            ComponentsDisplayed.Add(GetComponentByName("GS", 2));
            ComponentsDisplayed.Add(GetComponentByName("GS", 3));
            DangerMessage = LS.Text("update question");
        }
        public static void DisplayReview(GameInstanceModel[] saves, int selected, int selCount)
        {
            if (selected > 1)
            {
                var save = saves[selected - 2];
                UpdateTextBox(3, 1, ". . ." + " ---------------------------------- " +
                LS.Navigation("nickname label") + " - " + save.UserName.ToString() + " ---------------------------------- " +
                LS.Navigation("lvl label") + " - " + save.LVL.ToString() + " ---------------------------------- " +
                LS.Navigation("wallet label") + " - " + save.WalletFunds.ToString() + " ---------------------------------- " +
                LS.Navigation("lastplay label") + " - " + save.Lastplay.ToString() + " ---------------------------------- . . .");
                SetShowability(3, 0, true);
            }
            else
            {
                SetShowability(3, 3, false);
                GameSaveView.View.DisplayPixels(ComponentsDisplayed[1].Pos, ComponentsDisplayed[1].Size);
                GameSaveView.DisplayList(saves);
                GameSaveView.UpdateMenuSelect(selected, selected, selCount);
            }
        }
    }
}
