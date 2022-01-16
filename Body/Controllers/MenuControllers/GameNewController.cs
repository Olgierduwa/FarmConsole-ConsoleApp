using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Text;
using FarmConsole.Body.Views.MenuViews;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Controllers.CentralControllers;

namespace FarmConsole.Body.Controllers.MenuControllers
{
    class GameNewController : HeadController
    {
        private static int Selected;
        private static int selCount;
        private static int selStart;
        private static string NickName;
        private static string DefaultNickName;

        private static void StartNewGame()
        {
            SoundService.Play("K2");
            int Gender = ComponentService.GetSliderValue(2);
            int Difficulty = ComponentService.GetSliderValue(3);
            GameInstance = new GameInstanceModel(NickName, Difficulty, Gender);
            OpenScreen = GameInstance.Lastmap;
        }
        private static void UpdateSelect(int value)
        {
            SoundService.Play("K1");
            ComponentService.UpdateMenuSelect(Selected + value, Selected, selCount);
            Selected += value;
        }
        private static void UpdateSlider(int value)
        {
            int MaxSliderValue = 4;
            if (Selected > 1)
                if (ComponentService.GetSliderValue(Selected) + value >= 0 && ComponentService.GetSliderValue(Selected) + value <= MaxSliderValue)
                { ComponentService.UpdateMenuSlider(Selected, MaxSliderValue, value); SoundService.Play("K3"); }
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
            ComponentService.UpdateMenuTextBox(1, NickName, 3);
        }

        public static void Open()
        {
            Selected = 1;
            selCount = 4;
            selStart = 1;
            DefaultNickName = LS.Navigation("default nickname");
            NickName = DefaultNickName;
            GameNewView.Display(NickName); ComponentService.UpdateMenuSelect(Selected, Selected, selCount);
            while (OpenScreen == "NewGame")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (Selected > selStart) UpdateSelect(-1); break;
                        case ConsoleKey.S: if (Selected < selCount) UpdateSelect(+1); break;
                        case ConsoleKey.A: if (Selected == selCount) GenerateAvatar(); else UpdateSlider(-1); break;
                        case ConsoleKey.D: if (Selected == selCount) GenerateAvatar(); else UpdateSlider(+1); break;
                        case ConsoleKey.E: if (Selected == selStart) EditNickName(); else StartNewGame(); break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: SoundService.Play("K3"); OpenScreen = "Menu"; break;
                    }
                }
                else if ((DateTime.Now - Previously).TotalMilliseconds >= FrameTime)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Previously = DateTime.Now;
                }
            }
            ComponentService.Clean();
        }
    }
}
