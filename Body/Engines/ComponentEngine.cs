using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
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
        protected static readonly string title = StringService.Get("title");
        protected static readonly string foot = StringService.Get("foot");
        protected static readonly string controlsText = XF.GetText(1);
        protected static readonly string fromAuthorText = XF.GetText(2);
        protected static readonly string exitQuestion = XF.GetText(100);
        protected static readonly string deleteQuestion = XF.GetText(101);
        protected static readonly string updateQuestion = XF.GetText(102);
        protected static readonly string exitQuestion3 = XF.GetText(103);

        private static List<CM> CLIST = new List<CM>();
        private static readonly Color static_base_color = ColorService.GetColorByName("gray3");
        private static readonly Color static_content_color = ColorService.GetColorByName("White");

        protected static List<CM> ComponentList { get => CLIST; set => CLIST = value; }
        protected static CM GetComponentByName(string name, int id_group = -1)
        {
            int index = 0;
            while (index < CLIST.Count) if (CLIST[index].Name == name && (id_group < 0 || id_group == CLIST[index].ID_group)) break; else index++;
            if (index < CLIST.Count) return CLIST[index];
            else return CLIST[0];
        }

        #region ComponentsControl
        protected static void UpdateSelect(int c1, int c0, int count, int id_group = 2, int rangeProp = 13)
        {
            if (count == 0) return;
            int idStartFrom = 3, t = 0;
            while (CLIST[idStartFrom - 1].ID_group != id_group) idStartFrom++;
            int range = (Console.WindowHeight - rangeProp) / CLIST[idStartFrom + 1].Size.Height;
            if (c0 < c1 && c0 < count + 1 - range)
            {
                CLIST[idStartFrom + c0].Show = false;
                CLIST[idStartFrom + c0 + range].Show = true;
            }
            else if (c0 >= c1 && c1 < count + 1 - range)
            {
                CLIST[idStartFrom + c1].Show = true;
                CLIST[idStartFrom + c1 + range].Show = false;
            }
            CLIST[idStartFrom + c0].Prop = 0;
            CLIST[idStartFrom + c0].Base_color = static_base_color;
            CLIST[idStartFrom + c1].Prop = 1;
            CLIST[idStartFrom + c1].Base_color = ColorService.GetColorByName("Yellow");
            foreach (CM c in CLIST) if (c.ID_group == id_group && c.ID_object > 0) switch (c.Show)
                    {
                        case true: Print(c, t * c.Size.Height); break;
                        case false: t++; break;
                        case null: t = 0; break;
                    }
            //ShowComponentList();
        }
        protected static void UpdateTextBox(int id_group, int id_object, string text)
        {
            int id = 0;
            for (int i = 0; i < CLIST.Count; i++)
                if (CLIST[i].ID_group == id_group && CLIST[i].ID_object == id_object) { id = i; break; }
            int width = CLIST[id].View[0].Length;
            string line = "";
            List<string> content = new List<string>();
            foreach (string word in text.Split(' '))
            {
                if (line.Length + word.Length <= width - 8) line += string.IsNullOrEmpty(line) ? word : " " + word;
                else if (word != "")
                {
                    content.Add(("").PadRight((width - line.Length) / 2 - 1, ' ') + line + ("").PadRight(width - line.Length - (width - line.Length) / 2 - 1, ' '));
                    line = " " + word;
                }
            }
            content.Add(("").PadRight((width - line.Length) / 2 - 1, ' ') + line + ("").PadRight(width - line.Length - (width - line.Length) / 2 - 1, ' '));
            string[] view1 = new string[] { ComponentViewService.Top(width) };
            string[] view2 = ComponentViewService.Sides(content);
            string[] view3 = new string[] { ComponentViewService.Bot(width) };
            string[] view = view1.Concat(view2.Concat(view3).ToArray()).ToArray();
            if (CLIST[id].Size.Height != view.Length) Clear(CLIST[id]);
            CLIST[id].View = view;
            CLIST[id].Size = new Size(CLIST[id].Size.Width, view.Length);
            Print(CLIST[id]);
        }
        protected static void UpdateSlider(int id_group, int id_object, int count, int movement)
        {
            for (int i = 0; i < CLIST.Count; i++)
                if (CLIST[i].ID_group == id_group && CLIST[i].ID_object == id_object)
                {
                    CLIST[i].Prop += movement;
                    int tick = (CLIST[i].Size.Width - 4) * CLIST[i].Prop / count + 1;
                    Console.SetCursorPosition(CLIST[i].Pos.X, CLIST[i].Pos.Y);
                    Console.Write(" " + ComponentViewService.Fragment(CLIST[i].Size.Width - 2, '_'));
                    Console.SetCursorPosition(CLIST[i].Pos.X + tick, CLIST[i].Pos.Y);
                    Console.Write("||");
                    Console.SetCursorPosition(CLIST[i].Pos.X, CLIST[i].Pos.Y + 1);
                    Console.Write("|" + ComponentViewService.Fragment(CLIST[i].Size.Width - 2, '_') + "|");
                    Console.SetCursorPosition(CLIST[i].Pos.X + tick, CLIST[i].Pos.Y + 1);
                    Console.Write("||");
                    break;
                }
            Console.SetCursorPosition(0, 0);
        }
        protected static int GetSliderValue(int id_object, int id_group = 3)
        {
            for (int i = 0; i < CLIST.Count; i++)
                if (CLIST[i].ID_group == id_group && CLIST[i].ID_object == id_object)
                    return CLIST[i].Prop;
            return 0;
        }
        protected static void SetFocus(int id_group)
        {
            int t = 0;
            foreach (CM c in CLIST)
            {
                if ((c.ID_group >= id_group) && c.ID_object >= 0)
                {
                    c.Show = true;
                    Print(c);
                }
                else switch (c.Show)
                    {
                        case true: Print(c, t * c.Size.Height, false); break;
                        case false: t++; break;
                        case null: t = 0; break;
                    }
            }
        }
        protected static void SetShowability(int id_group, int id_object, bool show)
        {
            foreach (CM c in CLIST)
                if ((c.ID_group == id_group && id_object == 0 && c.ID_object > 0) || (c.ID_group == id_group && c.ID_object == id_object))
                    if (show == true) { c.Show = true; Print(c); } else { c.Show = false; Clear(c); }
        }
        protected static void ClearList(bool cleaning = true)
        {
            if (cleaning)
            {
                int t = 0;
                foreach (CM c in CLIST) switch (c.Show)
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
            foreach (CM c in CLIST) switch (c.Show)
                {
                    case true: Print(c, t * c.Size.Height); break;
                    case false: t++; break;
                    case null: t = 0; break;
                }
        }
        private static void Clear(CM c, int y = 0)
        {
            for (int i = 0; i < c.Size.Height; i++)
                if (c.Pos.Y + i - y < Console.WindowHeight)
                {
                    Console.SetCursorPosition(c.Pos.X, c.Pos.Y + i - y);
                    Console.Write(("").PadRight(c.Size.Width, ' '));
                }
            Console.SetCursorPosition(0, 0);
        }
        private static void Print(CM c, int y = 0, bool enable = true)
        {
            Color base_color = c.Base_color;
            Color content_color = c.Content_color;
            if (enable == false) base_color = content_color = static_base_color;

            for (int i = 0; i < c.Size.Height; i++)
                if (c.Pos.Y + i - y < Console.WindowHeight)
                {
                    Console.SetCursorPosition(c.Pos.X, c.Pos.Y + i - y);
                    Console.Write(c.View[i].Pastel(base_color));
                }

            if (c.View[0].Length > 1)
                for (int i = 1; i < c.Size.Height - 1; i++)
                {
                    Console.SetCursorPosition(c.Pos.X + 1, c.Pos.Y + i - y);
                    Console.Write(c.View[i][1..^1].Pastel(content_color));
                }
            Console.SetCursorPosition(0, 0);
        }
        private static void AddComponent(string name, string[] view, bool show = true, int prop = 0, Color background = new Color(), Color foreground = new Color(), bool cut = false)
        {
            string[] componentView = (string[])view.Clone();
            int id_group = 0, id_object = 0;
            Size Size = new Size
            {
                Width = componentView[0].Length,
                Height = componentView.Length
            };
            Point Pos = new Point();
            if (CLIST.Count > 0)
            {
                /// ustalanie id_group ///
                id_group = CLIST.Last().ID_group;
                int group = 1;
                if (CLIST.Last().ID_object == -2)
                    for (int i = CLIST.Count - 1; i > 0; i--)
                    {
                        if (CLIST[i].ID_object == -2) group++;
                        if (CLIST[i].ID_object == -1) group--;
                        if (group == 0 || CLIST[i].ID_group == 0) { id_group = CLIST[i].ID_group; break; }
                    }
                /// ustalanie posY ///
                Pos.Y = CLIST.Last().ID_object > -1 ? CLIST.Last().Pos.Y + CLIST.Last().Size.Height : CLIST.Last().Pos.Y;

                /// ustalanie id_object ///
                for (int i = CLIST.Count - 1; i >= 0; i--)
                    if (CLIST[i].ID_group == id_group && CLIST[i].ID_object >= 0)
                    {
                        id_object = CLIST[i].ID_object + 1;
                        break;
                    }
                /// ustalanie posX ///
                if (id_group > 0)
                    for (int i = CLIST.Count - 1; i > 0; i--)
                        if (CLIST[i].ID_group == id_group && CLIST[i].ID_object == -1)
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
            CLIST.Add(new CM(id_group, id_object, Pos, Size, componentView, name, prop, show, background, foreground));
            //PrintList();
        }
        #endregion

        #region ComponentsViews
        protected static void GroupStart(int selColumn, int column = 5)
        {
            Point Pos = new Point
            {
                X = Console.WindowWidth * (selColumn - 1) / column,
                Y = CLIST.Last().Pos.Y + CLIST.Last().Size.Height
            };
            int id_group = 1;
            int counter = 0;
            for (int i = 0; i < CLIST.Count; i++)
            {
                if (CLIST[i].ID_object == -1) counter++;
                if (CLIST[i].ID_object == -2) counter--;
            }
            for (int i = CLIST.Count - 1; i > 0; i--)
                if (CLIST[i].ID_object == -1)
                {
                    if (selColumn > 0 && counter > 0) Pos.Y = CLIST[i].Pos.Y;
                    id_group = CLIST[i].ID_group + 1; break;
                }

            CLIST.Add(new CM(id_group, -1, Pos, new Size(Console.WindowWidth / column, 0), new string[] { "" }, "GS", 0, null));
        }
        protected static void GroupEnd(int column = 5)
        {
            Point Pos = new Point
            {
                X = CLIST.Last().Pos.X,
                Y = CLIST.Last().Pos.Y + CLIST.Last().Size.Height
            };
            int id_group = CLIST.Last().ID_group;
            int group = 1;

            if (CLIST.Last().ID_object == -2)
                for (int i = CLIST.Count - 1; i > 0; i--)
                {
                    if (CLIST[i].ID_object == -1) group--;
                    if (CLIST[i].ID_object == -2)
                    {
                        group++;
                        if (Pos.Y < CLIST[i].Pos.Y + CLIST[i].Size.Height) Pos.Y = CLIST[i].Pos.Y + CLIST[i].Size.Height;
                    }
                    if (group == 0 || CLIST[i].ID_group == 0) { id_group = CLIST[i].ID_group; break; }
                }

            int index = 0;
            Size size = new Size();
            while (CLIST[index].ID_group != id_group) index++;
            while (index < CLIST.Count && CLIST[index].ID_group == id_group)
            {
                size.Height += CLIST[index].Size.Height;
                if (CLIST[index].Size.Width > size.Width) size.Width = CLIST[index].Size.Width;
                index++;
            }
            index = 0;
            while (CLIST[index].ID_group != id_group) index++;
            Point PosS = CLIST[index].Pos;
            PosS.X -= (size.Width - CLIST[index].Size.Width) / 2;
            CLIST[index].Pos = PosS;
            CLIST[index].Size = size;

            CLIST.Add(new CM(id_group, -2, Pos, new Size(Console.WindowWidth / column, 0), new string[] { "" }, "GE", 0, null));
        }
        protected static void H1(string text)
        {
            AddComponent("H1", new string[]
            {
                ComponentViewService.DoubleLine(),
                ComponentViewService.CenteredText(Console.WindowWidth, text),
                ComponentViewService.DoubleLine()
            });
        }
        protected static void H2(string text)
        {
            AddComponent("H2", new string[]
            {
                ComponentViewService.CenteredText(Console.WindowWidth, text),
                ComponentViewService.SingleLine(),
            });
        }
        protected static void Foot(string text)
        {
            string[] view = new string[]
            {
                ComponentViewService.SingleLine(),
                ComponentViewService.CenteredText(Console.WindowWidth, text),
                ComponentViewService.SingleLine(1)
            };
            CLIST.Add(new CM(0, 0, new Point(0, Console.WindowHeight - 3), new Size(Console.WindowWidth, 3), view, "FT"));
        }
        protected static void Endl(int x)
        {
            string[] view = new string[x];
            for (int i = 0; i < x; i++) view[i] = "";
            AddComponent("EN", view);
        }
        protected static void TextBox(string text, int width = 40, bool show = true, Color background = new Color(), Color foreground = new Color(), int margin = 3)
        {
            string line = "";
            List<string> content = new List<string>();
            foreach (string word in text.Split(' '))
            {
                if (line.Length + word.Length <= width - (margin * 2 + 2)) line += string.IsNullOrEmpty(line) ? word : " " + word;
                else if (word != "")
                {
                    content.Add(("").PadRight((width - line.Length) / 2 - 1, ' ') + line + ("").PadRight(width - line.Length - (width - line.Length) / 2 - 1, ' '));
                    line = " " + word;
                }
            }
            int LeftLenght = (width - 2 - line.Length) / 2;
            int RightLenght = width - 2 - line.Length - (width - 2 - line.Length) / 2;
            RightLenght = RightLenght < 0 ? 0 : RightLenght;
            LeftLenght = LeftLenght < 0 ? 0 : LeftLenght;
            content.Add(("").PadRight(LeftLenght, ' ') + line + ("").PadRight(RightLenght, ' '));
            string[] view1 = new string[] { ComponentViewService.Top(width) };
            string[] view2 = ComponentViewService.Sides(content);
            string[] view3 = new string[] { ComponentViewService.Bot(width) };
            AddComponent("TB", view1.Concat(view2.Concat(view3).ToArray()).ToArray(), show, 0, background, foreground);
        }
        protected static void TextBoxLines(string[] lines, int width = 40, bool show = true, Color color1 = new Color(), Color color2 = new Color())
        {
            List<string> content = new List<string>();
            foreach (string line in lines)
                content.Add(("").PadRight((width - line.Length) / 2 - 1, ' ') + line + ("").PadRight(width - line.Length - (width - line.Length) / 2 - 1, ' '));
            string[] view1 = new string[] { ComponentViewService.Top(width) };
            string[] view2 = ComponentViewService.Sides(content);
            string[] view3 = new string[] { ComponentViewService.Bot(width) };
            AddComponent("TBL", view1.Concat(view2.Concat(view3).ToArray()).ToArray(), show, 0, color1, color2);
        }
        protected static string[] TextBoxView(string text, int width = 40, int margin = 3)
        {
            string line = "";
            List<string> content = new List<string>();
            foreach (string word in text.Split(' '))
            {
                if (line.Length + word.Length <= width - (margin * 2 + 2)) line += string.IsNullOrEmpty(line) ? word : " " + word;
                else if (word != "")
                {
                    content.Add(("").PadRight((width - line.Length) / 2 - 1, ' ') + line + ("").PadRight(width - line.Length - (width - line.Length) / 2 - 1, ' '));
                    line = " " + word;
                }
            }
            content.Add(("").PadRight((width - 2 - line.Length) / 2, ' ') + line + ("").PadRight(width - 2 - line.Length - (width - 2 - line.Length) / 2, ' '));
            return content.ToArray();
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
                " " + ComponentViewService.Fragment(left, '_') + "||" + ComponentViewService.Fragment(right, '_') + " ",
                "|" + ComponentViewService.Fragment(left, '.') + "||" + ComponentViewService.Fragment(right, '.') + "|", ""
            }, show, value, ColorService.GetColorByName("White"));
        }
        protected static void RightBar(int height, bool show = true)
        {
            string[] view1 = new string[] { ComponentViewService.Top(Console.WindowWidth / 5 + 2, true, false) };
            string[] view2 = ComponentViewService.SideRight(Console.WindowWidth / 5 + 1, height - 2);
            string[] view3 = new string[] { ComponentViewService.Bot(Console.WindowWidth / 5 + 2, true, false) };
            string[] view = view1.Concat(view2.Concat(view3).ToArray()).ToArray();
            AddComponent("RB", view, show);
        }
        protected static void LeftBar(int height, bool show = true)
        {
            string[] view1 = new string[] { ComponentViewService.Top(Console.WindowWidth / 5 + 1, false, true) };
            string[] view2 = ComponentViewService.SideLeft(Console.WindowWidth / 5 + 0, height - 2);
            string[] view3 = new string[] { ComponentViewService.Bot(Console.WindowWidth / 5 + 1, false, true) };
            string[] view = view1.Concat(view2.Concat(view3).ToArray()).ToArray();
            AddComponent("LB", view, show);
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
                Console.Write(c.ID_group + " | " + c.ID_object + " | " + c.Name + " | " + c.Pos.X + " | " + c.Pos.Y + " | " + c.Size.Width + " | " + c.Size.Height + " | " + c.Prop + " | " + c.Show + "             ");
            }
        }
    }
}
