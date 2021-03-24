using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    public class HelpView : MenuViewService
    {
        public static void Show()
        {
            H1(title);
            H2("Nie wiesz o co chodzi?");
            Endl(Console.WindowHeight / 9);
            GroupStart(0);
            GroupStart(2);
            TextBox("Sterowanie", 44);
            Endl(1);
            TextBox(controlsText, 44);
            GroupEnd();
            GroupStart(4);
            TextBox("Od Autora", 44);
            Endl(1);
            TextBox(fromAuthorText, 44);
            GroupEnd();
            GroupEnd();

            Foot(foot);
            PrintList();
            //showComponentList();
        }
    }
}
