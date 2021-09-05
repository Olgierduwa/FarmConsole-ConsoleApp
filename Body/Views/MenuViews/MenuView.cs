using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    public class MenuView : MainViewService
    {
        public static void Show()
        {
            H1(title);
            Endl((Console.WindowHeight - 20) / 2);

            GroupStart(0);

            GroupStart(2);
            Endl(1);
            TextBox("Zacznij Rozgrywke");
            TextBox("Kontynuuj Rozrywke");
            TextBox("Ustaw Wlasne Opcje");
            TextBox("Poznaj Zasady Gry");
            GroupEnd();

            GroupStart(4);
            Endl(1);
            TextBox("\"Swietny Tytul!!\"");
            TextBox("\"Oto najlpesza gra konsolowa, o jakiej slyszal swiat!\"");
            GroupEnd();

            GroupStart(Console.WindowWidth / 2, Console.WindowWidth);
            Endl(2);
            TextBox(exitQuestion, 33, false, ConsoleColor.Red);
            GroupEnd();
            GroupStart(Console.WindowWidth / 2 - 9, Console.WindowWidth);
            Endl(8);
            TextBox("Q / NIE", 15, false, ConsoleColor.Red, margin: 0);
            GroupEnd();
            GroupStart(Console.WindowWidth / 2 + 9, Console.WindowWidth);
            Endl(8);
            TextBox("E / TAK", 15, false, ConsoleColor.Red, margin: 0);
            GroupEnd();

            GroupEnd();

            Foot(foot);
            PrintList();
            //vt1.ShowComponentList();
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
    }
}
