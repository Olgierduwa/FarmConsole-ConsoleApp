using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FarmConsole.Body.Services
{
    public class MenuViewService
    {
        public static readonly string title = XF.GetString(0);
        public static readonly string foot = XF.GetString(1);
        public static readonly string controlsText = XF.GetText(1);
        public static readonly string fromAuthorText = XF.GetText(2);
        public static readonly string exitQuestion = XF.GetText(100);
        public static readonly string deleteQuestion = XF.GetText(101);
        public static readonly string updateQuestion = XF.GetText(102);
        public static readonly string exitQuestion3 = XF.GetText(103);

        private static readonly List<CC> CLIST = new List<CC>();
        private const ConsoleColor static_base_color = ConsoleColor.DarkGray;
        private const ConsoleColor static_content_color = ConsoleColor.White;

        #region ComponentsControl
        public static void UpdateSelect(int c1, int c0, int count, int id_group = 2, int rangeProp = 13)
        {
            if (count == 0) return;
            int idStartFrom = 3, t = 0;
            while (CLIST[idStartFrom - 1].ID_group != id_group) idStartFrom++;
            int range = (Console.WindowHeight - rangeProp) / CLIST[idStartFrom + 1].Height;
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
            CLIST[idStartFrom + c1].Base_color = ConsoleColor.Yellow;
            foreach (CC c in CLIST) if (c.ID_group == id_group && c.ID_object > 0) switch (c.Show)
                    {
                        case true: Print(c, t * c.Height); break;
                        case false: t++; break;
                        case null: t = 0; break;
                    }
            //ShowComponentList();
        }
        public static void UpdateItemList(List<ProductModel>[] component_list, int category, int id_group = 5)
        {
            int t = 0, idStartFrom = 4, count = CLIST.Count;
            while (CLIST[idStartFrom].ID_group != id_group) idStartFrom++;
            CC name = CLIST[idStartFrom + 1]; name.Base_color = ConsoleColor.DarkGray;
            name.View = new string[] { "".PadRight(name.View[0].Length, '─') }; name.Height = 1;
            foreach (CC c in CLIST) if (c.ID_group == id_group && c.ID_object >= 0) switch (c.Show)
                    {
                        case true: Clear(c, t * c.Height); break;
                        case false: t++; break;
                        case null: t = 0; break;
                    }
            for (int i = idStartFrom + 1; i < count; i++) CLIST.RemoveAt(idStartFrom + 1);
            GraphicBox(new string[] { XF.GetProductCategoryName()[category], "" }, color: ConsoleColor.Gray);
            Print(name);
            Print(CLIST[++idStartFrom]);

            if (component_list[category].Count == 0)
            {
                idStartFrom += 2;
                Endl(1);
                GraphicBox(new string[] { "NIC TU NIE MA" });
                Print(CLIST[idStartFrom]);
            }
            for (int i = 0; i < component_list[category].Count; i++)
                if (i < (Console.WindowHeight - 13) / 3)
                {
                    idStartFrom++;
                    TextBox(component_list[category][i].amount + "x " + component_list[category][i].name, Console.WindowWidth / 5 - 1, margin: 0);
                    Print(CLIST[idStartFrom]);
                }
                else TextBox(component_list[category][i].name, Console.WindowWidth / 5 - 1, false, margin: 0);
            GroupEnd();
            GroupEnd();
        }
        public static void UpdateBox(int id_group, int id_object, string text)
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
            string[] view1 = new string[] { AsciiPartViewService.Top(width) };
            string[] view2 = AsciiPartViewService.Sides(content);
            string[] view3 = new string[] { AsciiPartViewService.Bot(width) };
            string[] view = view1.Concat(view2.Concat(view3).ToArray()).ToArray();
            if (CLIST[id].Height != view.Length) Clear(CLIST[id]);
            CLIST[id].View = view;
            CLIST[id].Height = view.Length;
            Print(CLIST[id]);
        }
        public static void UpdateSlider(int id_group, int id_object, int count, int value)
        {
            for (int i = 0; i < CLIST.Count; i++)
                if (CLIST[i].ID_group == id_group && CLIST[i].ID_object == id_object)
                {
                    int tick = CLIST[i].Width / (count) * value + 1;
                    Console.SetCursorPosition(CLIST[i].PosX, CLIST[i].PosY);
                    Console.Write(" " + AsciiPartViewService.Fragment(CLIST[i].Width - 2, '_'));
                    Console.SetCursorPosition(CLIST[i].PosX + tick, CLIST[i].PosY);
                    Console.Write("||");
                    Console.SetCursorPosition(CLIST[i].PosX, CLIST[i].PosY + 1);
                    Console.Write("|" + AsciiPartViewService.Fragment(CLIST[i].Width - 2, '_') + "|");
                    Console.SetCursorPosition(CLIST[i].PosX + tick, CLIST[i].PosY + 1);
                    Console.Write("||");
                }
            Console.SetCursorPosition(0, 0);
        }
        public static void UpdatGraphic(int id_group, int id_object, string[] content = null, ConsoleColor color = static_base_color)
        {
            int id = 0;
            for (int i = 0; i < CLIST.Count; i++) if (CLIST[i].ID_group == id_group && CLIST[i].ID_object == id_object) { id = i; break; }
            if (content != null)
            {
                if (CLIST[id].Height != content.Length) Clear(CLIST[id]);
                CLIST[id].View = content;
                CLIST[id].Height = content.Length;
            }
            CLIST[id].Content_color = color;
            CLIST[id].Base_color = color;
            Print(CLIST[id]);
        }
        public static void ClearList(bool cleaning = true)
        {
            if (cleaning)
            {
                int t = 0;
                foreach (CC c in CLIST) switch (c.Show)
                    {
                        case true: Clear(c, t * c.Height); break;
                        case false: t++; break;
                        case null: t = 0; break;
                    }
            }
            CLIST.Clear();
        }
        public static void PrintList()
        {
            int t = 0;
            foreach (CC c in CLIST) switch (c.Show)
                {
                    case true: Print(c, t * c.Height); break;
                    case false: t++; break;
                    case null: t = 0; break;
                }
            Console.ResetColor();
        }
        private static void Clear(CC c, int y = 0)
        {
            for (int i = 0; i < c.Height; i++)
            {
                Console.SetCursorPosition(c.PosX, c.PosY + i - y);
                Console.Write(("").PadRight(c.Width, ' '));
            }
            Console.SetCursorPosition(0, 0);
        }
        private static void Print(CC c, int y = 0, bool enable = true)
        {
            ConsoleColor base_color = c.Base_color;
            ConsoleColor content_color = c.Content_color;
            if (enable == false) base_color = content_color = static_base_color;

            Console.ForegroundColor = base_color;
            for (int i = 0; i < c.Height; i++)
            {
                Console.SetCursorPosition(c.PosX, c.PosY + i - y);
                Console.Write(c.View[i]);
            }
            //Thread.Sleep(500);
            if (c.View[0].Length > 1)
            {
                Console.ForegroundColor = content_color;
                for (int i = 1; i < c.Height - 1; i++)
                {
                    Console.SetCursorPosition(c.PosX + 1, c.PosY + i - y);
                    Console.Write(c.View[i].Substring(1, c.View[i].Length - 2));
                }
            }
            Console.SetCursorPosition(0, 0);
        }
        public static void Focus(int id_group)
        {
            int t = 0;
            foreach (CC c in CLIST)
            {
                if ((c.ID_group >= id_group) && c.ID_object >= 0)
                {
                    c.Show = true;
                    Print(c);
                }
                else switch (c.Show)
                    {
                        case true: Print(c, t * c.Height, false); break;
                        case false: t++; break;
                        case null: t = 0; break;
                    }
            }
        }
        public static void Showability(int id_group, int id_object, bool show)
        {
            foreach (CC c in CLIST)
                if ((c.ID_group == id_group && id_object == 0 && c.ID_object > 0) || (c.ID_group == id_group && c.ID_object == id_object))
                    if (show == true) { c.Show = true; Print(c); } else { c.Show = false; Clear(c); }
        }
        private static void AddComponent(string name, string[] view, bool show = true, int prop = 0, ConsoleColor color1 = static_base_color, ConsoleColor color2 = static_content_color, bool cut = false)
        {
            int id_group = 0, id_object = 0, posY = 0, posX = 0, width = view[0].Length, height = view.Length, difference = 0;
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
                posY = CLIST.Last().PosY + CLIST.Last().Height;

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
                            posX = CLIST[i].PosX + ((CLIST[i].Width - width) / 2);
                            if (posX < 0)
                            {
                                if (cut)
                                {
                                    difference = 0 - posX;
                                    if (difference > width) difference = width - 1;
                                    for (int j = 0; j < view.Length; j++)
                                        view[j] = view[j].Substring(difference);
                                }
                                posX = 0;
                            }
                            else if (posX + width >= Console.WindowWidth)
                            {
                                if (cut)
                                {
                                    difference = Console.WindowWidth - posX;
                                    if (difference <= 0)
                                    {
                                        difference = 0;
                                        posX = Console.WindowWidth - 1;
                                    }
                                    for (int j = 0; j < view.Length; j++)
                                        view[j] = view[j].Substring(0, difference);

                                }
                                else posX = Console.WindowWidth - view[0].Length;
                            }
                        }
            }
            if (name == "EN") posX = 0;
            CLIST.Add(new CC(id_group, id_object, posX, posY, width, height, view, name, prop, show, color1, color2));
        }
        public static void GroupStart(int selColumn, int column = 5)
        {
            int posX = Console.WindowWidth * (selColumn - 1) / column, id_group = 1;
            int posY = CLIST.Last().PosY + CLIST.Last().Height;
            int counter = 0;
            for (int i = 0; i < CLIST.Count; i++)
            {
                if (CLIST[i].ID_object == -1) counter++;
                if (CLIST[i].ID_object == -2) counter--;
            }
            for (int i = CLIST.Count - 1; i > 0; i--)
                if (CLIST[i].ID_object == -1)
                {
                    if (selColumn > 0 && counter > 0) posY = CLIST[i].PosY + CLIST[i].Height;
                    id_group = CLIST[i].ID_group + 1; break;
                }
            CLIST.Add(new CC(id_group, -1, posX, posY, Console.WindowWidth / column, 0, new string[] { "" }, "GS", 0, null));
        }
        public static void GroupEnd(int column = 5)
        {
            int posX = CLIST.Last().PosX;
            int posY = CLIST.Last().PosY + CLIST.Last().Height;
            int id_group = CLIST.Last().ID_group;
            int group = 1;

            if (CLIST.Last().ID_object == -2)
                for (int i = CLIST.Count - 1; i > 0; i--)
                {
                    if (CLIST[i].ID_object == -1) group--;
                    if (CLIST[i].ID_object == -2)
                    {
                        group++;
                        if (posY < CLIST[i].PosY + CLIST[i].Height) posY = CLIST[i].PosY + CLIST[i].Height;
                    }
                    if (group == 0 || CLIST[i].ID_group == 0) { id_group = CLIST[i].ID_group; break; }
                }

            CLIST.Add(new CC(id_group, -2, posX, posY, Console.WindowWidth / column, 0, new string[] { "" }, "GE", 0, null));
        }
        #endregion

        #region ComponentsViews
        public static void H1(string text)
        {
            AddComponent("H1", new string[]
            {
                AsciiPartViewService.DoubleLine(),
                AsciiPartViewService.CenteredText(Console.WindowWidth, text),
                AsciiPartViewService.DoubleLine()
            });
        }
        public static void H2(string text)
        {
            AddComponent("H2", new string[]
            {
                AsciiPartViewService.CenteredText(Console.WindowWidth, text),
                AsciiPartViewService.SingleLine(),
            });
        }
        public static void Foot(string text)
        {
            string[] view = new string[]
            {
                AsciiPartViewService.SingleLine(),
                AsciiPartViewService.CenteredText(Console.WindowWidth, text),
                AsciiPartViewService.SingleLine(1)
            };
            CLIST.Add(new CC(0, 0, 0, Console.WindowHeight - 3, Console.WindowWidth, 3, view, "FT"));
        }
        public static void Endl(int x)
        {
            string[] view = new string[x];
            for (int i = 0; i < x; i++) view[i] = "";
            AddComponent("EN", view);
        }
        public static void GraphicBox(string[] content, bool show = true, ConsoleColor color = static_base_color)
        {
            AddComponent("GB", content, show, 0, color, color, true);
        }
        public static void TextBox(string text, int width = 40, bool show = true, ConsoleColor color1 = static_base_color, ConsoleColor color2 = static_content_color, int margin = 3)
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
            string[] view1 = new string[] { AsciiPartViewService.Top(width) };
            string[] view2 = AsciiPartViewService.Sides(content);
            string[] view3 = new string[] { AsciiPartViewService.Bot(width) };
            AddComponent("TB", view1.Concat(view2.Concat(view3).ToArray()).ToArray(), show, 0, color1, color2);
        }
        public static void TextBoxLines(string[] lines, int width = 40, bool show = true, ConsoleColor color1 = static_base_color, ConsoleColor color2 = static_content_color)
        {
            List<string> content = new List<string>();
            foreach (string line in lines)
                content.Add(("").PadRight((width - line.Length) / 2 - 1, ' ') + line + ("").PadRight(width - line.Length - (width - line.Length) / 2 - 1, ' '));
            string[] view1 = new string[] { AsciiPartViewService.Top(width) };
            string[] view2 = AsciiPartViewService.Sides(content);
            string[] view3 = new string[] { AsciiPartViewService.Bot(width) };
            AddComponent("TBL", view1.Concat(view2.Concat(view3).ToArray()).ToArray(), show, 0, color1, color2);
        }
        public static void Slider(int count, int value, bool show = true)
        {
            int width = 36, left = width / count * value, right = width - left;
            AddComponent("SL", new string[]
            {
                " " + AsciiPartViewService.Fragment(left, '_') + "||" + AsciiPartViewService.Fragment(right, '_') + " ",
                "|" + AsciiPartViewService.Fragment(left, '.') + "||" + AsciiPartViewService.Fragment(right, '.') + "|", ""
            }, show, 0, ConsoleColor.White);
        }
        public static void RightBar(bool show = true, int extra = 0)
        {
            string[] view1 = new string[] { AsciiPartViewService.Top(Console.WindowWidth / 5 + 2, true, false) };
            string[] view2 = AsciiPartViewService.SideRight(Console.WindowWidth / 5 + 1, Console.WindowHeight - 12 + extra);
            string[] view3 = new string[] { AsciiPartViewService.Bot(Console.WindowWidth / 5 + 2, true, false) };
            string[] view = view1.Concat(view2.Concat(view3).ToArray()).ToArray();
            AddComponent("RB", view, show);
        }
        public static void LeftBar(bool show = true, int extra = 0)
        {
            string[] view1 = new string[] { AsciiPartViewService.Top(Console.WindowWidth / 5 + 1, false, true) };
            string[] view2 = AsciiPartViewService.SideLeft(Console.WindowWidth / 5 + 0, Console.WindowHeight - 12 + extra);
            string[] view3 = new string[] { AsciiPartViewService.Bot(Console.WindowWidth / 5 + 1, false, true) };
            string[] view = view1.Concat(view2.Concat(view3).ToArray()).ToArray();
            AddComponent("LB", view, show);
        }
        public static void CenterBar(bool show = true)
        {
            string[] view1 = new string[] { AsciiPartViewService.Top(40, true, true) };
            string[] view2 = AsciiPartViewService.SideLeft(40, Console.WindowHeight - 10);
            string[] view3 = new string[] { AsciiPartViewService.Bot(40, true, true) };
            string[] view = view1.Concat(view2.Concat(view3).ToArray()).ToArray();
            AddComponent("CB", view, show);
        }
        public static string[] TextBoxView(string text, int width = 40, int margin = 3)
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
            string[] view1 = new string[] { AsciiPartViewService.Top(width) };
            string[] view2 = AsciiPartViewService.Sides(content);
            string[] view3 = new string[] { AsciiPartViewService.Bot(width) };
            return view1.Concat(view2.Concat(view3).ToArray()).ToArray();
        }
        #endregion

        public static void ShowComponentList()
        {
            int i = 3, od = 20;
            Console.SetCursorPosition(od, 0);
            Console.WriteLine("G | S |name| X | Y | W | H |prop|swow");
            foreach (CC c in CLIST)
            {
                i++;
                if (i + 2 > Console.WindowHeight) { Console.SetCursorPosition(od, i); Console.Write("> > > WIECEJ SIE NIE ZMIESCI < < <"); return; }
                Console.SetCursorPosition(od, i);
                Console.Write(c.ID_group + " | " + c.ID_object + " | " + c.Name + " | " + c.PosX + " | " + c.PosY + " | " + c.Width + " | " + c.Height + " | " + c.Prop + " | " + c.Show + "             ");
            }
        }
    }
}
