using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class GameNewView : ComponentService
    {
        public static void Display(string NickName)
        {
            Endl(3);
            H2(LS.Navigation("new game label"));
            GroupStart(0);

            int Height = Console.WindowHeight / 2 - 13;

            GroupStart(4,13);
            Endl(Height);
            TextBox(LS.Navigation("nickname label"), 32);
            TextBox(LS.Navigation("gender label"), 32);
            TextBox(LS.Navigation("level of difficulty label"), 32);
            TextBox(LS.Navigation("avatar label"), 32);
            GroupEnd();

            GroupStart(Console.WindowWidth * 15 / 22, Console.WindowWidth);
            Endl(Height);
            TextBox(NickName, 50);
            Slider(4, 2, 18);
            Slider(4, 2, 18);
            GroupEnd();

            GroupStart(Console.WindowWidth * 15 / 22 - 18, Console.WindowWidth);
            Endl(Height + 3);
            TextBox(LS.Object("woman"), 13, foreground: ColorService.GetColorByName("gray3"), margin: 0);
            TextBox(LS.Navigation("easy"), 13, foreground: ColorService.GetColorByName("gray3"), margin: 0);
            GroupEnd();

            GroupStart(Console.WindowWidth * 15 / 22 + 19, Console.WindowWidth);
            Endl(Height + 3);
            TextBox(LS.Object("man"), 13, foreground: ColorService.GetColorByName("gray3"), margin:0);
            TextBox(LS.Navigation("hard"), 13, foreground: ColorService.GetColorByName("gray3"), margin:0);
            GroupEnd();

            GroupStart(Console.WindowWidth * 15 / 22 - 13, Console.WindowWidth);
            Endl(Height + 9);
            TextBox(LS.Navigation("random", " A"), 24, margin: 0);
            GroupEnd();

            GroupStart(Console.WindowWidth * 15 / 22 + 13, Console.WindowWidth);
            Endl(Height + 9);
            TextBox(LS.Navigation("self", " D"), 24, margin:0);
            GroupEnd();

            GroupStart(Console.WindowWidth / 2 - 10, Console.WindowWidth);
            Endl(Console.WindowHeight - 12);
            TextBox(LS.Navigation("cancel button ", " Q"), 19, margin: 0);
            GroupEnd();

            GroupStart(Console.WindowWidth / 2 + 11, Console.WindowWidth);
            Endl(Console.WindowHeight - 12);
            TextBox(LS.Navigation("continue button", " E"), 19, margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();
            //showComponentList();
        }
        public static void DisplayNickNameEditor(string defaultString)
        {
            ClearList(false);
            Endl(Console.WindowHeight / 2 - 8);
            GroupStart(Console.WindowWidth * 15 / 22, Console.WindowWidth);
            TextBox(defaultString, 50, background: ColorService.SelectionColor);
            GroupEnd();
            PrintList();
        }

        private static List<CM> NewGameView;
        public static void GetNewGameView() => NewGameView = ComponentList.ToList(); 
        public static void SetNewGameView() => ComponentList = NewGameView.ToList(); 
    }
}
