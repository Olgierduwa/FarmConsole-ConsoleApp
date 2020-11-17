using System;
using System.Collections.Generic;
using System.Linq;

namespace FarmConsole.Body.View.Components
{
    public class ViewTools
    {
        private const ConsoleColor static_base_color = ConsoleColor.DarkGray;
        private const ConsoleColor static_content_color = ConsoleColor.White;

        private readonly int column = 5;
        private readonly List<Component> CLIST = new List<Component>();

        private class Component
        {
            public int ID_group { get; }
            public int ID_object { get; }
            public int PosX { get; }
            public int PosY { get; }
            public int Width { get; }
            public int Height { get; set; }
            public string[] View { get; set; }
            public int Prop { get; set; }
            public string Name { get; set; } // jedynie do testów
            public bool? Show { get; set; }
            public ConsoleColor Base_color { get; set; }
            public ConsoleColor Content_color { get; set; }

            public Component(int id_group, int id_object, int posX, int posY, int width, int height, string[] view, string name, int prop = 0, bool? show = true,
                             ConsoleColor color1 = static_base_color, ConsoleColor color2 = static_content_color)
            {
                this.ID_group = id_group;
                this.ID_object = id_object;
                this.PosX = posX;
                this.PosY = posY;
                this.Width = width;
                this.Height = height;
                this.View = view;
                this.Prop = prop;
                this.Name = name;
                this.Show = show;
                this.Base_color = color1;
                this.Content_color = color2;
            }
        }

        public void UpdateList(int c1, int c0, int count, int id_group = 3)
        {
            int idStartFrom = 4;
            while (CLIST[idStartFrom-1].ID_group != id_group) idStartFrom++;
            Component first = CLIST[idStartFrom + 1];
            int range = (Console.WindowHeight - 17) / first.Height;
            if (c0 < c1) {
                if (c0 < count + 1 - range) {
                    CLIST[idStartFrom + c0].Show = false;
                    CLIST[idStartFrom + c0 + range].Show = true;
                }
            } else {
                if (c1 < count + 1 - range) {
                    CLIST[idStartFrom + c1].Show = true;
                    CLIST[idStartFrom + c1 + range].Show = false;
                }
            }

            CLIST[idStartFrom + c0].Prop = 0;
            CLIST[idStartFrom + c0].Base_color = static_base_color;
            CLIST[idStartFrom + c1].Prop = 1;
            CLIST[idStartFrom + c1].Base_color = ConsoleColor.Yellow;

            int shownOnList = 0, firstShow = -1, currentOnList = 0;
            foreach (Component c in CLIST)
            {
                if (c.ID_group == id_group && c.ID_object > 0)
                {
                    if (c.Show == true)
                    {
                        if (firstShow == -1) firstShow = idStartFrom - currentOnList;
                        Console.ForegroundColor = c.Base_color;
                        for (int i = 0; i < c.Height; i++)
                        {
                            Console.SetCursorPosition(c.PosX, first.PosY + shownOnList * first.Height + i);
                            Console.Write(c.View[i]);
                        }
                        Console.ForegroundColor = static_content_color;
                        for (int i = 1; i < c.Height - 1; i++)
                        {
                            Console.SetCursorPosition(c.PosX + 1, first.PosY + shownOnList * first.Height + i);
                            Console.Write(c.View[i].Substring(1, c.View[i].Length - 2));
                        }
                        shownOnList++;
                    }
                    currentOnList++;
                }
            }
            Console.SetCursorPosition(Console.WindowWidth-1, Console.WindowHeight-1);
            //showComponentList();
        }
        public void UpdateBox(int id_group, int id_object, string text)
        {
            int id = 0;
            for (int i = 0; i < CLIST.Count; i++)
                if (CLIST[i].ID_group == id_group && CLIST[i].ID_object == id_object) { id = i; break; }
            int L = CLIST[id].View[0].Length;
            string[] words = text.Split(' ');
            string line = "";
            List<string> content = new List<string>();
            foreach (string word in words)
            {
                if (line.Length + word.Length <= L - 8) line += string.IsNullOrEmpty(line) ? word : " " + word;
                else {
                    content.Add(("").PadRight((L - line.Length) / 2 - 1, ' ') + line + ("").PadRight(L - line.Length - (L - line.Length) / 2 - 1, ' '));
                    line = word;
                }
            }
            content.Add(("").PadRight((L - line.Length) / 2 - 1, ' ') + line + ("").PadRight(L - line.Length - (L - line.Length) / 2 - 1, ' '));
            string[] view1 = new string[] { ViewFragments.Top(L) };
            string[] view2 = ViewFragments.Sides(content);
            string[] view3 = new string[] { ViewFragments.Bot(L) };
            string[] view = view1.Concat(view2.Concat(view3).ToArray()).ToArray();
            if (CLIST[id].Height != view.Length)
            for (int i = 0; i < CLIST[id].Height; i++)
            {
                Console.SetCursorPosition(CLIST[id].PosX, CLIST[id].PosY + i);
                Console.Write(("").PadRight(CLIST[id].View[0].Length, ' '));
            }
            CLIST[id].View = view;
            CLIST[id].Height = view.Length;
            Print(CLIST[id]);
        }
        public void UpdateSlider(int idGroup, int idSel, int count, int value)
        {
            for (int i = 0; i < CLIST.Count; i++)
                if (CLIST[i].ID_group == idGroup && CLIST[i].ID_object == idSel)
                {
                    int tick = CLIST[i].Width / (count) * value + 1;
                    Console.SetCursorPosition(CLIST[i].PosX, CLIST[i].PosY);
                    Console.Write(" " + ViewFragments.Fragment(CLIST[i].Width - 2, '_'));
                    Console.SetCursorPosition(CLIST[i].PosX + tick, CLIST[i].PosY);
                    Console.Write("||");
                    Console.SetCursorPosition(CLIST[i].PosX, CLIST[i].PosY + 1);
                    Console.Write("|" + ViewFragments.Fragment(CLIST[i].Width - 2, '_') + "|");
                    Console.SetCursorPosition(CLIST[i].PosX + tick, CLIST[i].PosY + 1);
                    Console.Write("||");
                }
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
        }

        public void ClearList()
        {
            int t = 0;
            foreach (Component c in CLIST) switch(c.Show)
            {
                case true: Clear(c, t * c.Height); break;
                case false: t++; break;
                case null: t=0; break;
            }
            CLIST.Clear();
        }
        public void PrintList()
        {
            int t = 0;
            foreach (Component c in CLIST) switch (c.Show)
            {
                case true: Print(c, t * c.Height); break;
                case false: t++; break;
                case null: t=0; break;
            }
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
        }
        private void Clear(Component c, int y = 0)
        {
            for (int i = 0; i < c.Height; i++)
            {
                Console.SetCursorPosition(c.PosX, c.PosY + i - y);
                Console.Write(("").PadRight(c.Width,' '));
            }
        }
        private void Print(Component c, int y = 0, bool enable = true)
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

            if (c.View[0].Length > 0)
            {
                Console.ForegroundColor = content_color;
                for (int i = 1; i < c.Height - 1; i++)
                {
                    Console.SetCursorPosition(c.PosX + 1, c.PosY + i - y);
                    Console.Write(c.View[i].Substring(1,c.View[i].Length-2));
                }

                if (c.Name == "DB")
                {
                    Console.ForegroundColor = base_color;
                    int i = 1; while (c.View[1][i] != '|') i++;
                    Console.SetCursorPosition(c.PosX + i, c.PosY + 1 - y);
                    Console.Write("|   |");
                }
            }
        }
        public void Focus(int id_group)
        {
            int t = 0;
            foreach (Component c in CLIST)
            {
                if ((c.ID_group == id_group) && c.ID_object >= 0)
                {
                    c.Show = true;
                    Print(c);
                }
                else switch (c.Show)
                {
                    case true: Print(c, t * c.Height,false); break;
                    case false: t++; break;
                    case null: t=0; break;
                }
            }
        }
        public void Showability(int id_group, int id_object, bool show)
        {
            foreach (Component c in CLIST)
                if ((c.ID_group == id_group && id_object == 0 && c.ID_object > 0) || (c.ID_group == id_group && c.ID_object == id_object))
                    if(show == true) { c.Show = true; Print(c); } else { c.Show = false; Clear(c); }
        }
        private int AddComponent(string name, string[] view, bool show = true, int prop = 0, ConsoleColor color1 = static_base_color, ConsoleColor color2 = static_content_color)
        {
            int id_group = 0, id_object = 0, posY = 0, posX = 0, width = view[0].Length, height = view.Length;
            if(CLIST.Count > 0)
            {
                /// ustalanie id_group ///
                id_group = CLIST.Last().ID_group;
                int group = 1;
                if (CLIST.Last().ID_object == -2)
                    for (int i = CLIST.Count - 1; i > 0; i--)
                    {
                        if (CLIST[i].ID_object == -2) group++;
                        if (CLIST[i].ID_object == -1) group--;
                        if (group == 0 || CLIST[i].ID_group == 0) { id_group = CLIST[i].ID_group;  break; }
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
                            posX = CLIST[i].PosX + ((CLIST[i].Width - view[0].Length) / 2);
                            if (posX < 0) posX = 0;
                            else if (posX + view[0].Length > Console.WindowWidth) posX = Console.WindowWidth - posX - view[0].Length;
                        }
            }
            CLIST.Add(new Component(id_group, id_object, posX, posY, width, height, view, name, prop, show, color1, color2));
            return posY;
        }
        public void GroupStart(int selColumn)
        {
            int posX = Console.WindowWidth / column * (selColumn-1), id_group = 1;
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
            CLIST.Add(new Component(id_group, -1, posX, posY, Console.WindowWidth/column, 0, new string[]{ "" }, "GS", 0, null));
        }
        public void GroupEnd()
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

            CLIST.Add(new Component(id_group, -2, posX, posY, Console.WindowWidth / column, 0, new string[] { "" }, "GE", 0, null));
        }
        
        public void H1(string text)
        {
            AddComponent("H1", new string[]
            {
                ViewFragments.DoubleLine(),
                ViewFragments.CenteredText(Console.WindowWidth, text),
                ViewFragments.DoubleLine()
            });
        }
        public void H2(string text)
        {
            AddComponent("H2", new string[]
            {
                ViewFragments.CenteredText(Console.WindowWidth, text),
                ViewFragments.SingleLine(),
            });
        }
        public void Foot(string text)
        {
            string[] view = new string[]
            {
                ViewFragments.SingleLine(),
                ViewFragments.CenteredText(Console.WindowWidth, text),
                ViewFragments.SingleLine()
            };
            CLIST.Add(new Component(0, 0, 0, Console.WindowHeight-3, Console.WindowWidth, 3, view, "FT"));
        }
        public void Endl(int x)
        {
            string[] view = new string[x];
            for (int i = 0; i < x; i++) view[i] = "";
            AddComponent("EN", view);
        }
        public void Text(string content, bool show, ConsoleColor color = ConsoleColor.DarkGray)
        {
            AddComponent("TXT", new string[] { content }, show, 0, color);
        }

        public void TextBox(int L, string text, bool show = true, ConsoleColor color = ConsoleColor.DarkGray) 
        {
            string[] words = text.Split(' ');
            string line = "";
            List<string> content = new List<string>();
            foreach (string word in words)
            {
                if (line.Length + word.Length <= L - 8) line += string.IsNullOrEmpty(line) ? word : " " + word;
                else {
                    content.Add(("").PadRight((L - line.Length) / 2 - 1, ' ') + line + ("").PadRight(L - line.Length - (L - line.Length) / 2 - 1, ' '));
                    line = word;
                }
            }
            content.Add(("").PadRight((L - line.Length) / 2 - 1, ' ') + line + ("").PadRight(L - line.Length - (L - line.Length) / 2 - 1, ' '));
            string[] view1 = new string[] { ViewFragments.Top(L) };
            string[] view2 = ViewFragments.Sides(content);
            string[] view3 = new string[] { ViewFragments.Bot(L) };
            string[] view = view1.Concat(view2.Concat(view3).ToArray()).ToArray();
            AddComponent("TB", view, show, 0 ,color);
        }
        public void SingleButton(string text, int width = 0, bool show = true)
        {
            if (width == 0) width = 10 + text.Length;
            AddComponent("SB", new string[]
            {
                ViewFragments.Top(width),
                "|    " + text + "    |",
                ViewFragments.Bot(width)
            }, show, 0);
        }
        public void SingleButtonBot(string text, bool show = true)
        {
            AddComponent("-SB", new string[]
            {
                ViewFragments.Top(text.Length + 8),
                "|   " + text + "   |",
                ViewFragments.Bot(text.Length + 8)
            }, show);
        }
        public void DoubleButton(string text1, string text2, bool show = true, ConsoleColor color = ConsoleColor.DarkGray)
        {
            AddComponent("DB", new string[]
            {
                ViewFragments.Top(text1.Length + 6) + "   " + ViewFragments.Top(text2.Length + 6),
                "|  " + text1 + "  |   |  " + text2 + "  |",
                ViewFragments.Bot(text1.Length + 6) + "   " + ViewFragments.Bot(text2.Length + 6)
            }, show, 0, color);
        }
        public void DoubleButtonBot(int col, string text1, string text2)
        {
            string[] view = new string[]
            {
                ViewFragments.Top(text1.Length + 6) + "   " + ViewFragments.Top(text2.Length + 6),
                "|  " + text1 + "  |   |  " + text2 + "  |",
                ViewFragments.Bot(text1.Length + 6) + "   " + ViewFragments.Bot(text2.Length + 6)
            };

            int width = view[0].Length;
            int posX = Console.WindowWidth / column * (col - 1) + (Console.WindowWidth / column - width)/2;
            if (posX < 0) posX = 0;
            else if (posX + width > Console.WindowWidth) posX = Console.WindowWidth - width;

            CLIST.Add(new Component(0, 0, posX, Console.WindowHeight - 8, width, 3, view, "DB"));
        }
        public void RightBar(bool show = true)
        {
            string[] view1 = new string[] { ViewFragments.TopRight(Console.WindowWidth / column) };
            string[] view2 = ViewFragments.SideRight(Console.WindowWidth / column, Console.WindowHeight - 12);
            string[] view3 = new string[] { ViewFragments.BotRight(Console.WindowWidth / column) };
            string[] view = view1.Concat(view2.Concat(view3).ToArray()).ToArray();
            AddComponent("RB", view, show);
        }
        public void LeftBar(bool show = true)
        {
            string[] view1 = new string[] { ViewFragments.TopLeft(Console.WindowWidth / column) };
            string[] view2 = ViewFragments.SideLeft(Console.WindowWidth / column, Console.WindowHeight - 12);
            string[] view3 = new string[] { ViewFragments.BotLeft(Console.WindowWidth / column) };
            string[] view = view1.Concat(view2.Concat(view3).ToArray()).ToArray();
            AddComponent("LB", view, show);
        }
        public void Slider(int count, int value, bool show = true)
        {
            int width = 36, left = width / count * value, right = width - left;
            AddComponent("SL", new string[]
            {
                " " + ViewFragments.Fragment(left, '_') + "||" + ViewFragments.Fragment(right, '_') + " ",
                "|" + ViewFragments.Fragment(left, '.') + "||" + ViewFragments.Fragment(right, '.') + "|", ""
            }, show, 0, ConsoleColor.White);
        }
        
        public void ShowComponentList()
        {
            int i = 3, od = 150;
            Console.SetCursorPosition(od,0);
            Console.WriteLine("G | S |name| X | Y | W | H |prop|swow");
            foreach (Component c in CLIST)
            {
                i++;
                if (i + 2 > Console.WindowHeight) { Console.SetCursorPosition(150, i); Console.Write("> > > WIECEJ SIE NIE ZMIESCI < < <"); return; }
                Console.SetCursorPosition(od, i);
                Console.Write(c.ID_group + " | " + c.ID_object + " | " + c.Name + " | " + c.PosX + " | " + c.PosY+ " | " + c.Width + " | " + c.Height + " | " + c.Prop + " | " + c.Show + "             ");
            }
        }
    }
}
