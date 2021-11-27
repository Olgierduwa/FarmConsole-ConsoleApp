using FarmConsole.Body.Services;
using FarmConsole.Body.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FarmConsole.Body.Controlers
{
    static class AnimationController
    {
        private static string[] GraphicView;
        private static int[] ColumnDivision;
        private static int[] ColorIds;
        private static readonly double[] CrossSleepTime = new double[] { 50, 2000, 50 };
        private static readonly double[] SlideSleepTime = new double[] { 20, 1500, 20 };
        private static readonly double[] PopupSleepTime = new double[] { 10, 1000, 10 };
        private static int Stage;
        private static int CurrentColorID;
        private static DateTime Now;

        public static void Effect(string Effect = "", string GN = null, string[] GV = null, int[] CD = null, int[] CID = null, double? ST = null, int? S = null)
        {
            GraphicView = GN != null ? XF.GetGraphic(GN) : GV ?? (new string[] { "" });
            ColumnDivision = CD ?? (new int[] { 50, 100 });
            ColorIds = CID ?? ColorService.WhiteEffect;
            CrossSleepTime[1] = ST == null ? CrossSleepTime[1] : (double)ST;
            SlideSleepTime[1] = ST == null ? SlideSleepTime[1] : (double)ST;
            PopupSleepTime[1] = ST == null ? PopupSleepTime[1] : (double)ST;
            Stage = S != null ? (int)S : 0;
            CurrentColorID = 0;
            Now = DateTime.Now;
            switch (Effect)
            {
                case "CrossEffect": CrossInEffect(); CrossOutEffect(); break;
                case "CrossInEffect": CrossInEffect(); break;
                case "CrossOutEffect": CrossOutEffect(); break;
                case "HorizontalSlideEffect": HorizontalSlideInEffect(); HorizontalSlideOutEffect(); break;
                case "VerticalSlideEffect": VerticalSlideInEffect(); VerticalSlideOutEffect(); break;
                case "VerticalSlideInEffect": VerticalSlideInEffect(); break;
                case "VerticalSlideOutEffect": VerticalSlideOutEffect(); break;
            }
        }
        public static void DateChangeEffect(string OldDate, string NewDate)
        {
            string[] OldDateValues = OldDate.Split('-');
            string[] NewDateValues = NewDate.Split('-');
            bool[] DifferentValues = new bool[3];
            int[] columns = new int[] { 23, 56, 83 };
            
            for (int i = 0; i < 3; i++)
                if (OldDateValues[i] != NewDateValues[i])
                    DifferentValues[i] = true;

            List<string[]> OldGraphicViews = new List<string[]>();
            List<string[]> NewGraphicViews = new List<string[]>();
            for (int i = 0; i < OldDateValues.Length; i++)
            {
                OldGraphicViews.Add(XF.GetGraphic(OldDateValues[i][0].ToString()));
                NewGraphicViews.Add(XF.GetGraphic(NewDateValues[i][0].ToString()));
                for (int j = 1; j < OldDateValues[i].Length; j++)
                {
                    OldGraphicViews[i] = ComponentViewService.ExtendGraphis(OldGraphicViews[i], XF.GetGraphic(OldDateValues[i][j].ToString()));
                    NewGraphicViews[i] = ComponentViewService.ExtendGraphis(NewGraphicViews[i], XF.GetGraphic(NewDateValues[i][j].ToString()));
                }
            }

            Effect("CrossInEffect", GV: OldGraphicViews[0],     CD: new int[] { columns[0], 100 }, ST: 0);
            Effect("CrossInEffect", GV: XF.GetGraphic("/"),  CD: new int[] { 43, 100 }, ST: 0);
            Effect("CrossInEffect", GV: OldGraphicViews[1],     CD: new int[] { columns[1], 100 }, ST: 0);
            Effect("CrossInEffect", GV: XF.GetGraphic("/"),  CD: new int[] { 70, 100 }, ST: 0);
            Effect("CrossInEffect", GV: OldGraphicViews[2],     CD: new int[] { columns[2], 100 }, ST: 500);
            
            for (int i = OldGraphicViews.Count - 1; i >= 0; i--)
                if(DifferentValues[i])
                {
                    Effect("VerticalSlideOutEffect", GV: OldGraphicViews[i], CD: new int[] { columns[i], 100 }, ST: 0);
                    Effect("VerticalSlideInEffect", GV: NewGraphicViews[i], CD: new int[] { columns[i], 100 }, ST: 500);
                }

            Effect("CrossOutEffect", GV: NewGraphicViews[0], CD: new int[] { columns[0], 100 });
            Effect("CrossOutEffect", GV: XF.GetGraphic("/"), CD: new int[] { 43, 100 });
            Effect("CrossOutEffect", GV: NewGraphicViews[1], CD: new int[] { columns[1], 100 });
            Effect("CrossOutEffect", GV: XF.GetGraphic("/"), CD: new int[] { 70, 100 });
            Effect("CrossOutEffect", GV: NewGraphicViews[2], CD: new int[] { columns[2], 100 });
            Thread.Sleep(500);
        }
        public static int PopUp()
        {
            int currentColorId = 0, limit = 1;

            if (Stage < 1 * limit) { currentColorId = 0; GraphicView = new string[] { GraphicView[0], GraphicView[1] }; }
            else if (Stage < 2 * limit) { currentColorId = 1; GraphicView = new string[] { GraphicView[0], GraphicView[1], GraphicView[2] }; }
            else if (Stage < 3 * limit + PopupSleepTime[1] / 50) { currentColorId = 2; GraphicView = new string[] { GraphicView[0], GraphicView[1], GraphicView[2], GraphicView[3] }; }
            else if (Stage < 4 * limit + PopupSleepTime[1] / 50) { currentColorId = 3; GraphicView = new string[] { GraphicView[0], GraphicView[1], GraphicView[2] }; }
            else if (Stage < 5 * limit + PopupSleepTime[1] / 50) { currentColorId = 4; GraphicView = new string[] { GraphicView[0], GraphicView[1] }; }
            else if (Stage < 6 * limit + PopupSleepTime[1] / 50) { currentColorId = 5; Stage = -1; GraphicView = new string[] { GraphicView[0] }; }

            AnimationView.PopUp(AnimationController.GraphicView, ColorService.GetColorByID(ColorIds[currentColorId]));
            return Stage;
        }

        private static void CrossInEffect()
        {
            while (Stage < 2)
            {
                if (Console.KeyAvailable) Console.ReadKey(true);
                if ((DateTime.Now - Now).TotalMilliseconds >= CrossSleepTime[Stage])
                {
                    AnimationView.DisplayGraphic(GraphicView, ColumnDivision[0], ColumnDivision[1], 5, ColorService.GetColorByID(ColorIds[CurrentColorID]));
                    if (CurrentColorID == 2 || CurrentColorID == 3) Stage++;
                    else CurrentColorID++;
                    Now = DateTime.Now;
                }
            }
            AnimationView.Clean(false);
        }
        private static void CrossOutEffect()
        {
            Stage = 2;
            CurrentColorID = 2;
            while (Stage < 3)
            {
                if (Console.KeyAvailable) Console.ReadKey(true);
                if ((DateTime.Now - Now).TotalMilliseconds >= CrossSleepTime[Stage])
                {
                    AnimationView.DisplayGraphic(GraphicView, ColumnDivision[0], ColumnDivision[1], 5, ColorService.GetColorByID(ColorIds[CurrentColorID]));
                    if (CurrentColorID == 5) Stage++;
                    CurrentColorID++;
                    Now = DateTime.Now;
                }
            }
            AnimationView.Clean();
        }
        private static void HorizontalSlideInEffect()
        {
            int C = 8;
            while (Stage < 2)
            {
                if (Console.KeyAvailable) Console.ReadKey(true);
                if ((DateTime.Now - Now).TotalMilliseconds >= SlideSleepTime[Stage])
                {
                    AnimationView.DisplayGraphic(GraphicView, ColumnDivision[0] - C * 10, ColumnDivision[1], 5, ColorService.GetColorByID(ColorIds[CurrentColorID]), true);
                    if (C == 6 || C == 2) CurrentColorID++;
                    if (C == 0) { Stage++; C++; }
                    C--;
                    Now = DateTime.Now;
                }
            }
            AnimationView.Clean(false);
        }
        private static void HorizontalSlideOutEffect()
        {
            int C = 0;
            Stage = 2;
            CurrentColorID = 2;
            while (Stage < 3)
            {
                if (Console.KeyAvailable) Console.ReadKey(true);
                if ((DateTime.Now - Now).TotalMilliseconds >= SlideSleepTime[Stage])
                {
                    AnimationView.DisplayGraphic(GraphicView, ColumnDivision[0] + C * 10, ColumnDivision[1], 5, ColorService.GetColorByID(ColorIds[CurrentColorID]), true);
                    if (C == 3 || C == 5) CurrentColorID++;
                    if (C == 8) Stage++;
                    C++;
                    Now = DateTime.Now;
                }
            }
            AnimationView.Clean();
        }
        private static void VerticalSlideInEffect()
        {
            int R = 2;
            while (Stage < 2)
            {
                if (Console.KeyAvailable) Console.ReadKey(true);
                if ((DateTime.Now - Now).TotalMilliseconds >= SlideSleepTime[Stage])
                {
                    AnimationView.DisplayGraphic(GraphicView, ColumnDivision[0], ColumnDivision[1], R, ColorService.GetColorByID(ColorIds[CurrentColorID]), true);
                    if (R == 3 || R == 4) CurrentColorID++;
                    if (R == 5) Stage++;
                    else R++;
                    Now = DateTime.Now;
                }
            }
            AnimationView.Clean(false);
        }
        private static void VerticalSlideOutEffect()
        {
            int R = 5;
            Stage = 2;
            CurrentColorID = 2;
            while (Stage < 3)
            {
                if (Console.KeyAvailable) Console.ReadKey(true);
                if ((DateTime.Now - Now).TotalMilliseconds >= SlideSleepTime[Stage])
                {
                    AnimationView.DisplayGraphic(GraphicView, ColumnDivision[0], ColumnDivision[1], R, ColorService.GetColorByID(ColorIds[CurrentColorID]), true);
                    if (R == 6 || R == 7) CurrentColorID++;
                    if (R == 9) Stage++;
                    else R++;
                    Now = DateTime.Now;
                }
            }
            AnimationView.Clean();
        }
    }
}
