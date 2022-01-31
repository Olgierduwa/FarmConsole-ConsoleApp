using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FarmConsole.Body.Services.MainServices
{
    class ComponentService : ComponentEngine
    {
        protected static string DangerMessage;
        protected static List<CM> ComponentsDisplayed = new List<CM>();
        protected static ViewModel View;

        public static void Clean(bool ConsoleClear = false, bool RestoreCaptions = false, bool WithCleaning = true)
        {
            if (View != null)
            {
                if (WithCleaning && ComponentsDisplayed.Count == 0) View.DisplayPixels();
                else foreach (var CD in ComponentsDisplayed)
                        View.DisplayPixels(CD.Pos, CD.Size);

                ClearList(false);
                ComponentsDisplayed.Clear();
            }
            else ClearList(WithCleaning);

            if (ConsoleClear) Console.Clear();
            if (RestoreCaptions) Captions();
        }
        public static void UpdateMenuSelect(int CS, int PS, int MenuCount, int GID = 2, int Prop = 13) => UpdateSelect(CS, PS, MenuCount, GID, Prop);
        public static void UpdateMenuSelectOnly(int CS, int PS, int MenuCount, int GID = 2, int Prop = 13) => UpdateSelect(CS, PS, MenuCount, GID, Prop, false);
        public static void UpdateMenuSlider(int OID, int Count, int Movement, int GID = 3) => UpdateSlider(GID, OID, Count, Movement);
        public static void UpdateMenuTextBox(int OID, string Text, int GID = 2, int Margin = 3) => UpdateTextBox(GID, OID, Text, Margin);
        public static int GetSliderValue(int CS) => ComponentEngine.GetSliderValue(CS);
        public static void RepairDamageRemoval(int LastOID = 1, int GID = 2)
        {
            if (View != null)
            {
                int[] view = GetEndingListView("TB", GID, LastOID);
                if (view[0] != 0 || view[1] != 0 || view[2] != 0 || view[3] != 0)
                    View.DisplayPixels(new Point(view[0], view[1]), new Size(view[2], view[3]));
            }
        }
        public static void DisplayLastListElement(int GID = 2, int PropHeight = 15)
        {
            if (View != null)
            {
                var component = GetComponentByName("TB", GID, SettingsService.GetSettings.Count + 2);
                int showCount = (Console.WindowHeight - PropHeight) / 3;
                int Y = SettingsService.GetSettings.Count + 2 - showCount < 0 ? component.Pos.Y :
                    component.Pos.Y - (SettingsService.GetSettings.Count + 2 - showCount) * 3;
                if (component.IsVisible == true) View.DisplayPixels(new Point(component.Pos.X, Y), component.Size);
            }
        }
        public static void DisplayView() => PrintList();
        public static void DisableView(bool IsEnable = false) => DisableList(IsEnable);
        public static void SetProgressBar(int GID, int OID, int Procent) => ComponentEngine.SetProgressBar(GID, OID, Procent);
        public static bool? Danger(string Text, bool ClearBeforeEnableView = true)
        {
            SoundService.Play("K2");
            List<CM> TemporaryList = ComponentList.ToList();
            DisableView();
            ClearList(false);
            Endl(1);
            GroupStart(0);

            GroupStart(Console.WindowWidth / 2, Console.WindowWidth);
            Endl((Console.WindowHeight - Text.Split('\n').Length - 8) / 2);
            TextBoxLines(Text.Split('\n'), 33, true, ColorService.GetColorByName("Red"));
            var textbox = GetComponentByName("TBL");
            GroupEnd();

            GroupStart(Console.WindowWidth / 2 - 9, Console.WindowWidth);
            Endl((Console.WindowHeight + textbox.Size.Height - 4) / 2);
            TextBox(LS.Navigation("no", " Q"), 15, true, ColorService.GetColorByName("Red"), Margin: 0);
            GroupEnd();

            GroupStart(Console.WindowWidth / 2 + 9, Console.WindowWidth);
            Endl((Console.WindowHeight + textbox.Size.Height - 4) / 2);
            TextBox(LS.Navigation("yes", " E"), 15, true, ColorService.GetColorByName("Red"), Margin: 0);
            GroupEnd();

            GroupEnd();
            PrintList();

            List<CM> ComponentsToClear = new List<CM>
            {
                GetComponentByName("GS", 2),
                GetComponentByName("GS", 3),
                GetComponentByName("GS", 4)
            };

            bool? choice = null;
            while (choice == null)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Escape:
                    case ConsoleKey.Q: SoundService.Play("K3"); choice = false; break;
                    case ConsoleKey.E: SoundService.Play("K2"); choice = true; break;
                }
            }

            if (ClearBeforeEnableView)
                if (View != null) foreach (var c in ComponentsToClear)
                        View.DisplayPixels(c.Pos, c.Size);
                else ClearList();
            else ClearList(false);

            ComponentList = TemporaryList.ToList();
            DisableView(true);
            //PrintList();
            return choice;
        }
        public static void GoodNews(string Text, string Label = "", string EndLineSign = ":", bool ReturnView = true)
        {
            Label = Label != "" ? Label : LS.Navigation("excellent");
            Label = Label.ToUpper() + EndLineSign;
            List<CM> TemporaryList = ComponentList.ToList();
            DisableView();
            ClearList(false);

            string[] text, text2;
            int Width = 40;

            text = new string[] { " ", Label, " " };
            text2 = GetTextBoxView(Text, Width);
            text = text.Concat(text2.Concat(new string[] { " " }).ToArray()).ToArray();

            int Height = text2.Length + 6;

            Endl((Console.WindowHeight - Height) / 2);
            GroupStart(3);
            TextBoxLines(text, Width, Background: ColorService.GetColorByName("LimeD"));
            GroupEnd();
            PrintList();

            List<CM> ComponentsToClear = new List<CM> { GetComponentByName("TBL") };
            Console.ReadKey(true);

            if (ReturnView)
            {
                if (View != null)
                    foreach (var c in ComponentsToClear)
                        View.DisplayPixels(c.Pos, c.Size);
                else ClearList();

                ComponentList = TemporaryList.ToList();
                DisableView(true);
            }
        }
        public static void Warning(string Text, string Label = "")
        {
            Label = Label != "" ? Label : LS.Navigation("warning");
            Label = Label.ToUpper() + ":";
            List<CM> TemporaryList = ComponentList.ToList();
            DisableView();
            ClearList(false);

            string[] text, text2;
            int Width = 40;

            text = new string[] { " ", Label, " " };
            text2 = GetTextBoxView(Text, Width);
            text = text.Concat(text2.Concat(new string[] { " " }).ToArray()).ToArray();

            int Height = text2.Length + 6;

            Endl((Console.WindowHeight - Height) / 2);
            GroupStart(3);
            TextBoxLines(text, Width, Background: ColorService.GetColorByName("Red"));
            GroupEnd();
            PrintList();

            List<CM> ComponentsToClear = new List<CM> { GetComponentByName("TBL") };
            Console.ReadKey(true);

            if (View != null) foreach (var c in ComponentsToClear)
                    View.DisplayPixels(c.Pos, c.Size);
            else ClearList();

            ComponentList = TemporaryList.ToList();
            DisableView(true);
            //PrintList();
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
