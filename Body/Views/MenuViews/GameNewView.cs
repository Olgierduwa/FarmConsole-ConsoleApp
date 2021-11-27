using FarmConsole.Body.Engines;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class GameNewView : MenuManager
    {
        public static void Display()
        {
            Endl(3);
            H2(StringService.Get("new game label"));
            GroupStart(0);

            int Height = Console.WindowHeight / 2 - 13;

            GroupStart(4,13);
            Endl(Height);
            TextBox(StringService.Get("nickname label"), 32);
            TextBox(StringService.Get("gender label"), 32);
            TextBox(StringService.Get("level of difficulty label"), 32);
            TextBox(StringService.Get("avatar label"), 32);
            GroupEnd();

            GroupStart(Console.WindowWidth * 15 / 22, Console.WindowWidth);
            Endl(Height);
            TextBox("Olgierduwa", 50);
            Slider(4, 2, 18);
            Slider(4, 2, 18);
            GroupEnd();

            GroupStart(Console.WindowWidth * 15 / 22 - 18, Console.WindowWidth);
            Endl(Height + 3);
            TextBox(StringService.Get("woman button"), 13, foreground: ColorService.GetColorByName("gray"), margin: 0);
            TextBox(StringService.Get("easy button"), 13, foreground: ColorService.GetColorByName("gray"), margin: 0);
            GroupEnd();

            GroupStart(Console.WindowWidth * 15 / 22 + 19, Console.WindowWidth);
            Endl(Height + 3);
            TextBox(StringService.Get("man button"), 13, foreground: ColorService.GetColorByName("gray"), margin:0);
            TextBox(StringService.Get("hard button"), 13, foreground: ColorService.GetColorByName("gray"), margin:0);
            GroupEnd();

            GroupStart(Console.WindowWidth * 15 / 22 - 13, Console.WindowWidth);
            Endl(Height + 9);
            TextBox(StringService.Get("random button", " A"), 24, margin: 0);
            GroupEnd();

            GroupStart(Console.WindowWidth * 15 / 22 + 13, Console.WindowWidth);
            Endl(Height + 9);
            TextBox(StringService.Get("self button", " D"), 24, margin:0);
            GroupEnd();

            GroupStart(Console.WindowWidth / 2 - 10, Console.WindowWidth);
            Endl(Console.WindowHeight - 12);
            TextBox(StringService.Get("go back", " Q"), 19, margin: 0);
            GroupEnd();

            GroupStart(Console.WindowWidth / 2 + 11, Console.WindowWidth);
            Endl(Console.WindowHeight - 12);
            TextBox(StringService.Get("continue button", " E"), 19, margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();
            //showComponentList();
        }
    }
}
