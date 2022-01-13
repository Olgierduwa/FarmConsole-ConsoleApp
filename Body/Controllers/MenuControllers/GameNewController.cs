using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Text;
using FarmConsole.Body.Views.MenuViews;
using FarmConsole.Body.Services;

namespace FarmConsole.Body.Controlers
{
    class GameNewController : MainController
    {
        private static int selected;
        private static int selCount;
        private static int selStart;
        private static string NickName;
        private static string DefaultNickName;

        private static void StartNewGame()
        {
            S.Play("K2");
            int Difficulty = GameNewView.GetSliderValue(2);
            int Gender = GameNewView.GetSliderValue(3);
            GameInstance = new GameInstanceModel(NickName, Difficulty, Gender);
            OpenScreen = GameInstance.Lastmap;
        }
        private static void UpdateSelect(int value)
        {
            S.Play("K1");
            GameNewView.UpdateMenuSelect(selected + value, selected, selCount);
            selected += value;
        }
        private static void UpdateSlider(int value)
        {
            int MaxSliderValue = 4;
            if (selected > 1)
                if (value < 0)
                {
                    if (GameNewView.GetSliderValue(selected) > 0)
                    { GameNewView.UpdateMenuSlider(selected, MaxSliderValue, value); S.Play("K3"); }
                }
                else
                {
                    if (GameNewView.GetSliderValue(selected) < MaxSliderValue)
                    { GameNewView.UpdateMenuSlider(selected, MaxSliderValue, value); S.Play("K2"); }
                }
        }
        private static void GenerateAvatar()
        {

        }
        private static void EditNickName()
        {
            string DefaultString = ":";
            NickName = DefaultString;
            bool ShiftPressed;
            GameNewView.GetNewGameView();
            GameNewView.DisplayNickNameEditor(NickName);
            while (true)
            {
                var cki = Console.ReadKey(true);
                if (cki.Modifiers == ConsoleModifiers.Shift) ShiftPressed = true; else ShiftPressed = false;
                if (cki.Key == ConsoleKey.Q) break;
                else if (cki.Key == ConsoleKey.Backspace && NickName.Length > 0)
                {
                    NickName = NickName[0..^1];
                    if (NickName.Length == 0) NickName = DefaultString;
                }
                else if ((Convert.ToInt16(cki.Key) > 64 && Convert.ToInt16(cki.Key) < 91
                        || cki.Key == ConsoleKey.Spacebar) && NickName.Length < 32)
                {
                    if (NickName == DefaultString) NickName = "";
                    string Leter = ((char)Convert.ToInt16(cki.Key)).ToString();
                    NickName += ShiftPressed ? Leter : Leter.ToLower();
                }
                GameNewView.DisplayNickNameEditor(NickName);
            }
            if (NickName == ":") NickName = DefaultNickName;
            GameNewView.SetNewGameView();
            GameNewView.UpdateMenuTextBox(1, NickName, 3);
        }

        public static void Open()
        {
            selected = 1;
            selCount = 4;
            selStart = 1;
            DefaultNickName = LS.Navigation("default nickname");
            NickName = DefaultNickName;
            GameNewView.Display(NickName); GameNewView.UpdateMenuSelect(selected, selected, selCount);
            while (OpenScreen == "NewGame")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (selected > selStart) UpdateSelect(-1); break;
                        case ConsoleKey.S: if (selected < selCount) UpdateSelect(+1); break;
                        case ConsoleKey.A: if (selected == selCount) GenerateAvatar(); else UpdateSlider(-1); break;
                        case ConsoleKey.D: if (selected == selCount) GenerateAvatar(); else UpdateSlider(+1); break;
                        case ConsoleKey.E: if (selected == selStart) EditNickName(); else StartNewGame(); break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: S.Play("K3"); OpenScreen = "Menu"; break;
                    }
                }
                else if ((DateTime.Now - Previously).TotalMilliseconds >= FrameTime)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Previously = DateTime.Now;
                }
            }
            GameNewView.Clean();
        }
    }
}
