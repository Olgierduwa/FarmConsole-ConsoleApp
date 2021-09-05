using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    public class GameNewView : MainViewService
    {
        public static void Show()
        {
            H1(title);
            H2("Rozpocznij Nowa Giereczke");
            GroupStart(0);

            GroupStart(3);
            Endl(Console.WindowHeight / 5);
            TextBox("Kobieta");
            TextBox("Mezczyzna");
            GroupEnd();

            GroupStart(Console.WindowWidth / 2 - 10, Console.WindowWidth);
            Endl(Console.WindowHeight - 12);
            TextBox("Q / Powrot", 19, margin: 0);
            GroupEnd();
            GroupStart(Console.WindowWidth / 2 + 11, Console.WindowWidth);
            Endl(Console.WindowHeight - 12);
            TextBox("E / Enter", 19, margin: 0);
            GroupEnd();

            GroupEnd();
            Foot(foot);
            PrintList();
            //showComponentList();
        }
    }
}
