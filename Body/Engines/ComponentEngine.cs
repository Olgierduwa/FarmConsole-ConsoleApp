using FarmConsole.Body.Models;
using FarmConsole.Body.Services.MainServices;
using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FarmConsole.Body.Engines
{
    class ComponentEngine
    {
        private static List<CM> CLIST = new List<CM>();
        private static readonly Color static_base_color = ColorService.GetColorByName("gray3");
        private static readonly Color static_content_color = ColorService.GetColorByName("White");

        protected static List<CM> ComponentList { get => CLIST; set => CLIST = value; }
        protected static CM GetComponentByName(string Name, int GID = -1, int OID = -1) => CLIST[GetComponentID(Name, GID, OID)];
        protected static int GetGroupCount(int GID = 2)
        {
            int Index = 0;
            while (Index < CLIST.Count && GID != CLIST[Index].GroupID) Index++;
            int Count = -1;
            while (Index + 1 < CLIST.Count && GID == CLIST[Index + 1].GroupID && CLIST[Index + 1].ObjectID != -2) { Count++; Index++; }
            return Count;
        }
        protected static int GetSliderValue(int OID, int GID = 3) => CLIST[GetComponentID("", GID, OID)].Prop;
        protected static string[] GetTextBoxView(string Text, int Width = 40, int Margin = 3) => PatternService.SplitText(Text, Width, Margin).ToArray();
        protected static int[] GetEndingListView(string name, int GID, int LastOID)
        {
            int[] view = new int[4];
            int FirstIndex = GetComponentID(name, GID, LastOID);
            if (CLIST[FirstIndex].IsVisible == true)
            {
                int LastIndex = FirstIndex;
                while (CLIST[LastIndex + 1].ObjectID > 0 &&
                        CLIST[LastIndex + 1].GroupID == GID &&
                        CLIST[LastIndex + 1].IsVisible == true) LastIndex++;

                int y = LastOID - 1, p = 1;
                while (p < LastIndex && y > 0 && CLIST[LastIndex - p].IsVisible == true) { p++; y--; }
                y *= CLIST[LastIndex].Size.Height;

                view[0] = CLIST[FirstIndex].Pos.X;
                view[1] = CLIST[FirstIndex].Pos.Y - y;
                view[2] = CLIST[FirstIndex].Size.Width;
                view[3] = (LastIndex - FirstIndex + 1) * 3;
            }
            return view;
        }

        private static int GetComponentID(string Name = "", int GID = -1, int OID = -1)
        {
            int Index = 0;
            while (Index < CLIST.Count)
                if ((Name == "" || CLIST[Index].Name == Name) &&
                    (GID < 0 || GID == CLIST[Index].GroupID) &&
                    (OID < 0 || OID == CLIST[Index].ObjectID)) return Index;
                else Index++;
            throw new Exception();
        }
        private static void AddComponent(string name, string[] view, bool show = true, int prop = 0, Color background = new Color(), Color foreground = new Color(), bool cut = false)
        {
            string[] componentView = (string[])view.Clone();
            int GID = 0, OID = 0;
            Size Size = new Size
            {
                Width = componentView[0].Length,
                Height = componentView.Length
            };
            Point Pos = new Point();
            if (CLIST.Count > 0)
            {
                /// ustalanie GID ///
                GID = CLIST.Last().GroupID;
                int group = 1;
                if (CLIST.Last().ObjectID == -2)
                    for (int i = CLIST.Count - 1; i > 0; i--)
                    {
                        if (CLIST[i].ObjectID == -2) group++;
                        if (CLIST[i].ObjectID == -1) group--;
                        if (group == 0 || CLIST[i].GroupID == 0) { GID = CLIST[i].GroupID; break; }
                    }
                /// ustalanie posY ///
                Pos.Y = CLIST.Last().ObjectID > -1 ? CLIST.Last().Pos.Y + CLIST.Last().Size.Height : CLIST.Last().Pos.Y;

                /// ustalanie OID ///
                for (int i = CLIST.Count - 1; i >= 0; i--)
                    if (CLIST[i].GroupID == GID && CLIST[i].ObjectID >= 0)
                    {
                        OID = CLIST[i].ObjectID + 1;
                        break;
                    }
                /// ustalanie posX ///
                if (GID > 0)
                    for (int i = CLIST.Count - 1; i > 0; i--)
                        if (CLIST[i].GroupID == GID && CLIST[i].ObjectID == -1)
                        {
                            Pos.X = CLIST[i].Pos.X + ((CLIST[i].Size.Width - Size.Width) / 2);
                            int difference;
                            if (Pos.X < 0)
                            {
                                if (cut)
                                {
                                    difference = 0 - Pos.X;
                                    if (difference > Size.Width) difference = Size.Width - 1;
                                    for (int j = 0; j < componentView.Length; j++)
                                        componentView[j] = componentView[j][difference..];
                                }
                                Pos.X = 0;
                            }
                            else if (Pos.X + Size.Width >= Console.WindowWidth)
                            {
                                if (cut)
                                {
                                    difference = Console.WindowWidth - Pos.X;
                                    if (difference <= 0)
                                    {
                                        difference = 0;
                                        Pos.X = Console.WindowWidth - 1;
                                    }
                                    for (int j = 0; j < componentView.Length; j++)
                                        componentView[j] = componentView[j].Substring(0, difference);

                                }
                                else Pos.X = Console.WindowWidth - componentView[0].Length;
                            }
                        }
            }
            if (name == "EN") Pos.X = 0;
            CLIST.Add(new CM(GID, OID, Pos, Size, componentView, name, prop, show, true, background, foreground));
            //PrintList();
        }

        #region ComponentsControl
        protected static void UpdateSelect(int c1, int c0, int count, int GID = 2, int rangeProp = 13, bool Colorize = true)
        {
            if (count == 0 || CLIST.Count == 0) return;
            int idStartFrom = 3, t = 0;
            while (CLIST[idStartFrom - 1].GroupID != GID) idStartFrom++;
            int range = (Console.WindowHeight - rangeProp) / CLIST[idStartFrom + 1].Size.Height;
            if (c0 < c1 && c0 < count + 1 - range)
            {
                CLIST[idStartFrom + c0].IsVisible = false;
                CLIST[idStartFrom + c0 + range].IsVisible = true;
            }
            else if (c0 >= c1 && c1 < count + 1 - range)
            {
                CLIST[idStartFrom + c1].IsVisible = true;
                CLIST[idStartFrom + c1 + range].IsVisible = false;
            }
            if (Colorize)
            {
                CLIST[idStartFrom + c0].Prop = 0;
                CLIST[idStartFrom + c1].Prop = 1;
                CLIST[idStartFrom + c0].Base_color = static_base_color;
                CLIST[idStartFrom + c1].Base_color = ColorService.GetColorByName("Yellow");
            }
            foreach (CM c in CLIST) if (c.GroupID == GID && c.ObjectID > 0) switch (c.IsVisible)
                    {
                        case true: Print(c, t * c.Size.Height); break;
                        case false: t++; break;
                        case null: t = 0; break;
                    }
            //ShowComponentList();
        }
        protected static void UpdateTextBox(int GID, int OID, string Text, int Margin = 3)
        {
            int Index = GetComponentID("", GID, OID);
            int Width = CLIST[Index].View[0].Length;
            List<string> Content = PatternService.SplitText(Text, Width, Margin);
            string[] View = new string[] { PatternService.Top(Width) };
            View = View.Concat(PatternService.Sides(Content)).ToArray();
            View = View.Concat(new string[] { PatternService.Bot(Width) }).ToArray();
            if (CLIST[Index].Size.Height != View.Length) Clear(CLIST[Index]);
            CLIST[Index].View = View;
            CLIST[Index].Size = new Size(CLIST[Index].Size.Width, View.Length);

            int y = OID - 1, p = 1;
            while (p < Index && y > 0 && CLIST[Index - p].IsVisible == true) { p++; y--; }
            y *= CLIST[Index].Size.Height;

            if (CLIST[Index].IsVisible == true)
                if (CLIST[Index].IsEnable == true) Print(CLIST[Index], y, Enable: true);
                else Print(CLIST[Index], y, Enable: false);
        }
        protected static void UpdateSlider(int GID, int OID, int Count, int Movement)
        {
            int Index = GetComponentID("", GID, OID);
            int y = OID - 1, p = 1;
            while (p < Index && y > 0 && CLIST[Index - p].IsVisible == true) { p++; y--; }
            y *= CLIST[Index].Size.Height;

            if (CLIST[Index].IsVisible == true)
            {
                var c = CLIST[Index];
                c.Prop += Movement;
                int left = (c.Size.Width - 4) * c.Prop / Count, right = (c.Size.Width - 4) - left;
                c.View = new string[]
                {
                    " " + PatternService.Fragment(left, '_') + "||" + PatternService.Fragment(right, '_') + " ",
                    "|" + PatternService.Fragment(left, '.') + "||" + PatternService.Fragment(right, '.') + "|",
                    PatternService.Bot(c.Size.Width)
                };

                Print(CLIST[Index], y);
            }
        }
        protected static void SetProgressBar(int GID, int OID, int Procent, Color Background = new Color())
        {
            int Index = GetComponentID("", GID, OID);
            if (CLIST[Index].IsVisible == false) return;
            if (Procent >= 100) Procent = 100;
            if (Background == new Color()) Background = ColorService.GetColorByName("oranged");
            string FullContent = CLIST[Index].View[1][2..^2];
            string LeftContent = FullContent[0..(Procent * FullContent.Length / 100)];
            string RightContent = FullContent[(Procent * FullContent.Length / 100)..];
            LeftContent = LeftContent.PastelBg(Background);
            FullContent = " " + LeftContent + RightContent + " ";

            int y = OID - 1, p = 1;
            while (p < Index && y > 0 && CLIST[Index - p].IsVisible == true) { p++; y--; }
            y *= CLIST[Index].Size.Height;

            WindowService.Write(CLIST[Index].Pos.X + 1, CLIST[Index].Pos.Y + 1 - y, FullContent, CLIST[Index].Content_color);
        }
        protected static void SetShowability(int GID, int OID, bool Show)
        {
            int Index = GetComponentID("", GID, OID);
            CLIST[Index].IsVisible = Show;
            if (Show == true) Print(CLIST[Index]); 
            else Clear(CLIST[Index]); 
        }
        protected static void SetAvailability(int GID, int OID, bool Enable) => CLIST[GetComponentID("", GID, OID)].IsEnable = Enable;
        protected static void DisableList(bool IsEnable = false)
        {
            int t = 0;
            foreach (CM c in CLIST)
            {
                if(c.IsEnable != null) c.IsEnable = IsEnable;
                switch (c.IsVisible)
                {
                    case true: Print(c, t * c.Size.Height, IsEnable); break;
                    case false: t++; break;
                    case null: t = 0; break;
                }
            }
        }
        protected static void ClearList(bool Cleaning = true)
        {
            if (Cleaning)
            {
                int t = 0;
                foreach (CM c in CLIST) switch (c.IsVisible)
                    {
                        case true: Clear(c, t * c.Size.Height); break;
                        case false: t++; break;
                        case null: t = 0; break;
                    }
            }
            CLIST.Clear();
        }
        protected static void PrintList()
        {
            int t = 0;
            foreach (CM c in CLIST) switch (c.IsVisible)
                {
                    case true: Print(c, t * c.Size.Height, (bool)(c.IsEnable)); break;
                    case false: t++; break;
                    case null: t = 0; break;
                }
        }
        private static void Clear(CM c, int y = 0)
        {
            for (int i = 0; i < c.Size.Height; i++)
                WindowService.Write(c.Pos.X, c.Pos.Y + i - y, "".PadRight(c.Size.Width, ' '), new Color());
        }
        private static void Print(CM c, int y = 0, bool Enable = true)
        {
            Color base_color = c.Base_color;
            Color content_color = c.Content_color;
            if (Enable == false) base_color = content_color = static_base_color;

            for (int i = 0; i < c.Size.Height; i++)
                WindowService.Write(c.Pos.X, c.Pos.Y + i - y, c.View[i], base_color);

            if (c.View[0].Length > 1)
                for (int i = 1; i < c.Size.Height - 1; i++)
                    WindowService.Write(c.Pos.X + 1, c.Pos.Y + i - y, c.View[i][c.View[i].Length > 1 ? 1..^1 : 1..], content_color);
        }
        #endregion

        #region ComponentsViews
        protected static void GroupStart(int Column, int Columns = 5)
        {
            Point Pos = new Point
            {
                X = Console.WindowWidth * (Column - 1) / Columns,
                Y = CLIST.Last().Pos.Y + CLIST.Last().Size.Height
            };
            int GID = 1;
            int counter = 0;
            for (int i = 0; i < CLIST.Count; i++)
            {
                if (CLIST[i].ObjectID == -1) counter++;
                if (CLIST[i].ObjectID == -2) counter--;
            }
            for (int i = CLIST.Count - 1; i > 0; i--)
                if (CLIST[i].ObjectID == -1)
                {
                    if (Column > 0 && counter > 0) Pos.Y = CLIST[i].Pos.Y;
                    GID = CLIST[i].GroupID + 1; break;
                }

            CLIST.Add(new CM(GID, -1, Pos, new Size(Console.WindowWidth / Columns, 0), new string[] { "" }, "GS", 0, null, null));
        }
        protected static void GroupEnd(int Columns = 5)
        {
            Point Pos = new Point
            {
                X = CLIST.Last().Pos.X,
                Y = CLIST.Last().Pos.Y + CLIST.Last().Size.Height
            };
            int GID = CLIST.Last().GroupID;
            int group = 1;

            if (CLIST.Last().ObjectID == -2)
                for (int i = CLIST.Count - 1; i > 0; i--)
                {
                    if (CLIST[i].ObjectID == -1) group--;
                    if (CLIST[i].ObjectID == -2)
                    {
                        group++;
                        if (Pos.Y < CLIST[i].Pos.Y + CLIST[i].Size.Height) Pos.Y = CLIST[i].Pos.Y + CLIST[i].Size.Height;
                    }
                    if (group == 0 || CLIST[i].GroupID == 0) { GID = CLIST[i].GroupID; break; }
                }

            int index = 0;
            Size size = new Size();
            while (CLIST[index].GroupID != GID) index++;
            while (index < CLIST.Count && CLIST[index].GroupID == GID)
            {
                size.Height += CLIST[index].Size.Height;
                if (CLIST[index].Size.Width > size.Width) size.Width = CLIST[index].Size.Width;
                index++;
            }
            index = 0;
            while (CLIST[index].GroupID != GID) index++;
            Point PosS = CLIST[index].Pos;
            PosS.X -= (size.Width - CLIST[index].Size.Width) / 2;
            CLIST[index].Pos = PosS;
            CLIST[index].Size = size;

            CLIST.Add(new CM(GID, -2, Pos, new Size(Console.WindowWidth / Columns, 0), new string[] { "" }, "GE", 0, null, null));
        }
        protected static void H1(string Text)
        {
            AddComponent("H1", new string[]
            {
                PatternService.DoubleLine(),
                PatternService.CenteredText(Text, Console.WindowWidth),
                PatternService.DoubleLine()
            });
        }
        protected static void H2(string Text)
        {
            AddComponent("H2", new string[]
            {
                PatternService.CenteredText(Text, Console.WindowWidth),
                PatternService.SingleLine(),
            });
        }
        protected static void Foot(string Text)
        {
            string[] view = new string[]
            {
                PatternService.SingleLine(),
                PatternService.CenteredText(Text, Console.WindowWidth - 2),
                PatternService.SingleLine(1)
            };
            CLIST.Add(new CM(0, 0, new Point(0, Console.WindowHeight - 3), new Size(Console.WindowWidth, 3), view, "FT"));
        }
        protected static void Endl(int Height)
        {
            string[] view = new string[Height];
            for (int i = 0; i < Height; i++) view[i] = "";
            AddComponent("EN", view);
        }
        protected static void TextBox(string Text, int Width = 40, bool Show = true, Color Background = new Color(), Color Foreground = new Color(), int Margin = 3)
        {
            List<string> Content = PatternService.SplitText(Text, Width, Margin);
            string[] View = new string[] { PatternService.Top(Width) };
            View = View.Concat(PatternService.Sides(Content)).ToArray();
            View = View.Concat(new string[] { PatternService.Bot(Width) }).ToArray();
            if (Text == "") Background = Foreground = ColorService.BackgroundColor;
            AddComponent("TB", View, Show, 0, Background, Foreground);
        }
        protected static void TextBoxLines(string[] Lines, int Width = 40, bool Show = true, Color Background = new Color(), Color Foreground = new Color())
        {
            List<string> Content = PatternService.CenteredLines(Lines, Width - 2);
            string[] View = new string[] { PatternService.Top(Width) };
            View = View.Concat(PatternService.Sides(Content)).ToArray();
            View = View.Concat(new string[] { PatternService.Bot(Width) }).ToArray();
            AddComponent("TBL", View, Show, 0, Background, Foreground);
        }
        protected static void GraphicBox(string[] content, bool show = true, Color color = new Color())
        {
            AddComponent("GB", content, show, 0, color, color, true);
        }
        protected static void Slider(int count, int value, int width = 36, bool show = true)
        {
            int left = width * value / count, right = width - left;
            AddComponent("SL", new string[]
            {
                " " + PatternService.Fragment(left, '_') + "||" + PatternService.Fragment(right, '_') + " ",
                "|" + PatternService.Fragment(left, '.') + "||" + PatternService.Fragment(right, '.') + "|",
                PatternService.Bot(width + 4)
            }, show, value, ColorService.GetColorByName("White"));
        }
        protected static void BotBar(string Text, int Width = 40, int Height = -1, bool Show = true, Color Background = new Color(), Color Foreground = new Color(), int Margin = 3)
        {
            string[] View = new string[] { PatternService.Top(Width) };
            View = View.Concat(GetTextBoxView(Text, Width, Margin)).ToArray();
            for (int i = 1; i < View.Length; i++)
            {
                View[i] = View[i].Insert(0, "|");
                View[i] = View[i].Insert(View[i].Length, "|");
            }
            View = View.Concat(new string[] { PatternService.Bot(Width) }).ToArray();
            Height = Height < 0 ? View.Length : Height > View.Length ? View.Length : Height;
            string[] CuttedView = new string[Height];
            for (int i = 0; i < Height; i++) CuttedView[i] = View[i];
            AddComponent("BB", CuttedView, Show);
        }
        protected static void RightBar(int height, int width, bool show = true)
        {
            string[] view = new string[] { PatternService.Top(width + 2, true, false) };
            view = view.Concat(PatternService.SideRight(width + 1, 2)).ToArray();
            view = view.Concat(PatternService.SideRight(1, height - 6)).ToArray();
            view = view.Concat(PatternService.SideRight(width + 1, 2)).ToArray();
            view = view.Concat(new string[] { PatternService.Bot(width + 2, true, false) }).ToArray();
            AddComponent("RB", view, show);
        }
        protected static void LeftBar(int height, int width, bool show = true)
        {
            string[] View = new string[] { PatternService.Top(width + 2, false, true) };
            View = View.Concat(PatternService.SideLeft(width + 1, height - 2)).ToArray();
            View = View.Concat(new string[] { PatternService.Bot(width + 2, false, true) }).ToArray();
            AddComponent("LB", View, show);
        }
        #endregion

        protected static void ShowComponentList()
        {
            int i = 3, od = 20;
            Console.SetCursorPosition(od, 0);
            Console.WriteLine("G | S |name| X | Y | W | H |prop|swow");
            foreach (CM c in CLIST)
            {
                i++;
                if (i + 2 > Console.WindowHeight) { Console.SetCursorPosition(od, i); Console.Write("> > > WIECEJ SIE NIE ZMIESCI < < <"); return; }
                Console.SetCursorPosition(od, i);
                Console.Write(c.GroupID + " | " + c.ObjectID + " | " + c.Name + " | " + c.Pos.X + " | " + c.Pos.Y + " | " + c.Size.Width + " | " + c.Size.Height + " | " + c.Prop + " | " + c.IsVisible + "             ");
            }
        }
    }
}
