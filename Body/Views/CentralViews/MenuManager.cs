using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class MenuManager : ComponentEngine
    {
        protected static string DangerMessage;
        protected static List<CM> ComponentsDisplayed = new List<CM>();
        protected static ViewModel View;

        public static void Clean(bool ConsoleClear = false, bool RestoreCaptions = false, bool WithCleaning = true)
        {
            if (View != null)
            {
                if (WithCleaning && ComponentsDisplayed.Count == 0) View.DisplayPixels();
                else foreach (var CD in  ComponentsDisplayed)
                    View.DisplayPixels(CD.Pos, CD.Size);

                ClearList(false);
                ComponentsDisplayed.Clear();
            }
            else ClearList(WithCleaning);

            if (ConsoleClear) Console.Clear();
            if (RestoreCaptions) Captions(); 
        }
        public static void UpdateMenuSelect(int CS, int PS, int MenuCount, int IDG = 2, int Prop = 13) => UpdateSelect(CS, PS, MenuCount, IDG, Prop);
        public static void UpdateMenuSelectOnly(int CS, int PS, int MenuCount, int IDG = 2, int Prop = 13) => UpdateSelect(CS, PS, MenuCount, IDG, Prop, false);
        public static void UpdateMenuSlider(int IDO, int Count, int Movement, int IDG = 3) => UpdateSlider(IDG, IDO, Count, Movement);
        public static void UpdateMenuTextBox(int IDO, string text, int IDG = 2, int margin = 3) => UpdateTextBox(IDG, IDO, text, margin);
        public static int GetSliderValue(int CS) => ComponentEngine.GetSliderValue(CS);
        public static void DisplayLastListElement(int PropHeight = 15)
        {
            if (View != null)
            {
                var component = GetComponentByName("TB", 3, SettingsService.GetSettings.Count + 2);
                int showCount = (Console.WindowHeight - PropHeight) / 3;
                int Y = SettingsService.GetSettings.Count + 2 - showCount < 0 ? component.Pos.Y :
                    component.Pos.Y - (SettingsService.GetSettings.Count + 2 - showCount) * 3;
                if (component.IsVisible == true) View.DisplayPixels(new Point(component.Pos.X, Y), component.Size);
            }
        }
        public static bool? Danger(bool ClearBefore = true)
        {
            S.Play("K2");
            List<CM> TemporaryList = ComponentList.ToList();
            DisableView();
            ClearList(false);
            Endl(1);
            GroupStart(0);

            GroupStart(Console.WindowWidth / 2, Console.WindowWidth);
            Endl((Console.WindowHeight - DangerMessage.Split('\n').Length - 8) / 2);
            TextBoxLines(DangerMessage.Split('\n'), 33, true, ColorService.GetColorByName("Red"));
            var textbox = GetComponentByName("TBL");
            GroupEnd();

            GroupStart(Console.WindowWidth / 2 - 9, Console.WindowWidth);
            Endl((Console.WindowHeight + textbox.Size.Height - 4) / 2);
            TextBox(LS.Navigation("no", " Q"), 15, true, ColorService.GetColorByName("Red"), margin: 0);
            GroupEnd();

            GroupStart(Console.WindowWidth / 2 + 9, Console.WindowWidth);
            Endl((Console.WindowHeight + textbox.Size.Height - 4) / 2);
            TextBox(LS.Navigation("yes", " E"), 15, true, ColorService.GetColorByName("Red"), margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();

            List<CM> ComponentsToClear = new List<CM>();
            ComponentsToClear.Add(GetComponentByName("GS", 2));
            ComponentsToClear.Add(GetComponentByName("GS", 3));
            ComponentsToClear.Add(GetComponentByName("GS", 4));

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

            if (ClearBefore)
                if (View != null) foreach (var c in ComponentsToClear)
                        View.DisplayPixels(c.Pos, c.Size);
                else ClearList();
            else ClearList(false);

            ComponentList = TemporaryList.ToList();
            PrintList();
            return choice;
        }
        public static void GoodNews(string content, string label = "")
        {
            label = label != "" ? label : LS.Navigation("excellent");
            label = label.ToUpper() + ":";
            List <CM> TemporaryList = ComponentList.ToList();
            DisableView();
            ClearList(false);

            string[] text, text2;
            int Width = 40;

            text = new string[] { " ", label, " " };
            text2 = TextBoxView(content, Width);
            text = text.Concat(text2.Concat(new string[] { " " }).ToArray()).ToArray();

            int Height = text2.Length + 6;

            Endl((Console.WindowHeight - Height) / 2);
            GroupStart(3);
            TextBoxLines(text, Width, color1: ColorService.GetColorByName("LimeD"));
            GroupEnd();
            PrintList();

            List<CM> ComponentsToClear = new List<CM>();
            ComponentsToClear.Add(GetComponentByName("TBL"));
            Console.ReadKey(true);

            if (View != null) foreach (var c in ComponentsToClear)
                    View.DisplayPixels(c.Pos, c.Size);
            else ClearList();

            ComponentList = TemporaryList.ToList();
            PrintList();
        }
        public static void Warning(string content, string label = "")
        {
            label = label != "" ? label : LS.Navigation("warning");
            label = label.ToUpper() + ":";
            List <CM> TemporaryList = ComponentList.ToList();
            DisableView();
            ClearList(false);

            string[] text, text2;
            int Width = 40;

            text = new string[] { " ", label, " " };
            text2 = TextBoxView(content, Width);
            text = text.Concat(text2.Concat(new string[] { " " }).ToArray()).ToArray();

            int Height = text2.Length + 6;

            Endl((Console.WindowHeight - Height) / 2);
            GroupStart(3);
            TextBoxLines(text, Width, color1: ColorService.GetColorByName("Red"));
            GroupEnd();
            PrintList();

            List<CM> ComponentsToClear = new List<CM>();
            ComponentsToClear.Add(GetComponentByName("TBL"));
            Console.ReadKey(true);

            if (View != null) foreach (var c in ComponentsToClear)
                    View.DisplayPixels(c.Pos, c.Size);
            else ClearList();

            ComponentList = TemporaryList.ToList();
            PrintList();
        }
        public static void Captions()
        {
            H1(LS.Navigation("title"));
            Endl(Console.WindowHeight - 3);
            Foot(LS.Navigation("foot"));
            PrintList();
            ClearList(false);
        }
        public static MapModel SetView
        {
            set
            {
                MapModel map = value;
                if (map != null)
                {
                    Size s = map.View.GetSize;
                    View = new ViewModel(new PixelModel[s.Width, s.Height], s.Width, s.Height, map.View.GetPosition);
                    foreach (var word in map.View.GetWords)
                        View.SetWord(word.Position.X, word.Position.Y, word.Content, word.Color);
                }
                else View = null;
            }
        }
    }
}
