using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    public class GameSaveView : ComponentEngine
    {
        public static void Show(GameInstanceModel[] saves)
        {
            int savesCount = saves.Length;
            int freeSpace = Console.WindowHeight - 15;
            int showCount = freeSpace / 3;
            int endlCount = 3;
            int detailsHeight = (Console.WindowHeight - 24) / 2;
            if (showCount >= savesCount + 1) endlCount += (showCount - savesCount + 1) * 3 / 2;

            H1(title);
            H2("Zapisz Giereczke");

            GroupStart(0);
            GroupStart(2);
            if ((savesCount + 1) * 3 <= 17) Endl(detailsHeight);
            else Endl(endlCount);
            TextBox("P U S T Y   Z A P I S");
            for (int i = 0; i < savesCount; i++)
                if (i < showCount)
                    TextBox(saves[i].UserName);
                else TextBox(saves[i].UserName, show: false);
            GroupEnd();

            GroupStart(4);
            Endl(detailsHeight);
            TextBox("E / Utwórz Nowy Zapis");
            Endl(11);
            TextBox("E / Nadpisz Gre", show: false);
            GroupEnd();

            GroupStart(Console.WindowWidth / 2, Console.WindowWidth);
            Endl(detailsHeight + 1);
            TextBox(updateQuestion, 33, false, ConsoleColor.Red);
            GroupEnd();
            GroupStart(Console.WindowWidth / 2 - 9, Console.WindowWidth);
            Endl(detailsHeight + 9);
            TextBox("Q / NIE", 15, false, ConsoleColor.Red, margin: 0);
            GroupEnd();
            GroupStart(Console.WindowWidth / 2 + 9, Console.WindowWidth);
            Endl(detailsHeight + 9);
            TextBox("E / TAK", 15, false, ConsoleColor.Red, margin: 0);
            GroupEnd();

            GroupEnd();
            Foot(foot);
            PrintList();
            //showComponentList();
        }
        public static bool? Warning()
        {
            S.Play("K2");
            int focus = 4;
            Focus(focus);
            bool? choice = null;
            while (choice == null)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Q: S.Play("K3"); choice = false; break;
                    case ConsoleKey.E: S.Play("K2"); choice = true; break;
                }
            }
            Showability(focus, 0, false);
            Showability(focus + 1, 0, false);
            Showability(focus + 2, 0, false);
            PrintList();
            return choice;
        }
        public static void SetPreview(GameInstanceModel[] saves, int selected)
        {
            if (selected > 1)
            {
                var save = saves[selected - 2];
                UpdateBox(3, 1, ". . ." + " ---------------------------------- " +
                "Nazwa Gracza - " + save.UserName.ToString() + " ---------------------------------- " +
                "Osiągniety Poziom - " + save.LVL.ToString() + " ---------------------------------- " +
                "Posiadany Majątek - " + save.Wallet.ToString() + " ---------------------------------- " +
                "Ostatni Zapis - " + save.Lastplay.ToString() + " ---------------------------------- . . .");
                Showability(3, 0, true);
            }
            else
            {
                UpdateBox(3, 1, "E / Rozpocznij Nową Grę");
                Showability(3, 3, false);
            }
        }
    }
}
