using FarmConsole.Body.Engines;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    public class EscapeView : ComponentEngine
    {
        public static void Show()
        {
            H1(title);
            Endl(Console.WindowHeight / 5);

            GroupStart(0);
            GroupStart(3);
            Endl(1);
            TextBox("Kontynuuj");
            TextBox("Zapisz Gre");
            TextBox("Wróc do Menu");
            TextBox("Ustawienia");
            TextBox("Samouczek");

            GroupStart(Console.WindowWidth / 2, Console.WindowWidth);
            Endl(4);
            TextBox(exitQuestion, 33, false, ConsoleColor.Red);
            GroupStart(Console.WindowWidth / 2 - 9, Console.WindowWidth);
            Endl(10);
            TextBox("Q / NIE", 15, false, ConsoleColor.Red, margin: 0);
            GroupEnd();
            GroupStart(Console.WindowWidth / 2 + 9, Console.WindowWidth);
            Endl(10);
            TextBox("E / TAK", 15, false, ConsoleColor.Red, margin: 0);
            GroupEnd();

            GroupEnd();
            GroupEnd();
            GroupEnd();

            Foot(foot);
            PrintList();
            //vt1.showComponentList();
        }
        public static bool? Warning()
        {
            S.Play("K2");
            int focus = 3;
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
