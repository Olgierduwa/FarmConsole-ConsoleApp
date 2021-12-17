using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Resources.Sounds;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class MenuManager : ComponentEngine
    {
        protected static int FocusGroupID;
        protected static List<CM> ComponentsDisplayed;
        public static void Clean(bool CompletelyClean = false, bool RestoreCaptions = false)
        {
            if (ComponentsDisplayed != null && MapEngine.Map != null)
            {
                foreach (var CD in ComponentsDisplayed)
                    MapEngine.ShowMapFragment(CD.Pos, CD.Size);
                ClearList(false);
                ComponentsDisplayed.Clear();
            }
            else ClearList();

            if (CompletelyClean) Console.Clear();
            if (RestoreCaptions) Captions(); 
        }
        public static void UpdateMenuSelect(int CS, int PS, int MenuCount) => UpdateSelect(CS, PS, MenuCount);
        public static void UpdateMenuSlider(int IDO, int Count, int Movement, int IDG = 3) => UpdateSlider(IDG, IDO, Count, Movement);
        public static int GetSliderValue(int CS) => ComponentEngine.GetSliderValue(CS);
        public static bool? Warning()
        {
            S.Play("K2");
            SetFocus(FocusGroupID);
            bool? choice = null;
            while (choice == null)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Escape:
                    case ConsoleKey.Q: S.Play("K3"); choice = false; break;
                    case ConsoleKey.E: S.Play("K2"); choice = true; break;
                }
            }
            SetShowability(FocusGroupID, 0, false);
            SetShowability(FocusGroupID + 1, 0, false);
            SetShowability(FocusGroupID + 2, 0, false);
            PrintList();
            return choice;
        }
        public static void Captions()
        {
            H1(title);
            Endl(Console.WindowHeight - 3);
            Foot(foot);
            PrintList();
            ClearList(false);
        }
    }
}
