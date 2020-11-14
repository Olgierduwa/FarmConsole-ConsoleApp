using FarmConsole.Model;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FarmConsole.View
{
    public class ViewTools
    {
        private const ConsoleColor static_base_color = ConsoleColor.DarkGray;
        private const ConsoleColor static_content_color = ConsoleColor.White;

        private int column = 5;
        private List<Component> CLIST = new List<Component>();

        private class Component
        {
            public int id_group { get; }
            public int id_object { get; }
            public int posX { get; }
            public int posY { get; }
            public int width { get; }
            public int height { get; set; }
            public string[] view { get; set; }
            public int prop { get; set; }
            public string name { get; set; } // jedynie do testów
            public bool? show { get; set; }
            public ConsoleColor base_color { get; set; }
            public ConsoleColor content_color { get; set; }

            public Component(int id_group, int id_object, int posX, int posY, int width, int height, string[] view, string name, int prop = 0, bool? show = true,
                             ConsoleColor color1 = static_base_color, ConsoleColor color2 = static_content_color)
            {
                this.id_group = id_group;
                this.id_object = id_object;
                this.posX = posX;
                this.posY = posY;
                this.width = width;
                this.height = height;
                this.view = view;
                this.prop = prop;
                this.name = name;
                this.show = show;
                this.base_color = color1;
                this.content_color = color2;
            }
        }

        public void updateList(int c1, int c0, int count, int id_group = 3)
        {
            int idStartFrom = 4;
            while (CLIST[idStartFrom-1].id_group != id_group) idStartFrom++;
            Component first = CLIST[idStartFrom + 1];
            int range = (Console.WindowHeight - 17) / first.height;
            if (c0 < c1) {
                if (c0 < count + 1 - range) {
                    CLIST[idStartFrom + c0].show = false;
                    CLIST[idStartFrom + c0 + range].show = true;
                }
            } else {
                if (c1 < count + 1 - range) {
                    CLIST[idStartFrom + c1].show = true;
                    CLIST[idStartFrom + c1 + range].show = false;
                }
            }

            CLIST[idStartFrom + c0].prop = 0;
            CLIST[idStartFrom + c0].base_color = static_base_color;
            CLIST[idStartFrom + c1].prop = 1;
            CLIST[idStartFrom + c1].base_color = ConsoleColor.Yellow;

            int shownOnList = 0, firstShow = -1, currentOnList = 0;
            foreach (Component c in CLIST)
            {
                if (c.id_group == id_group && c.id_object > 0)
                {
                    if (c.show == true)
                    {
                        if (firstShow == -1) firstShow = idStartFrom - currentOnList;
                        Console.ForegroundColor = c.base_color;
                        for (int i = 0; i < c.height; i++)
                        {
                            Console.SetCursorPosition(c.posX, first.posY + shownOnList * first.height + i);
                            Console.Write(c.view[i]);
                        }
                        Console.ForegroundColor = static_content_color;
                        for (int i = 1; i < c.height - 1; i++)
                        {
                            Console.SetCursorPosition(c.posX + 1, first.posY + shownOnList * first.height + i);
                            Console.Write(c.view[i].Substring(1, c.view[i].Length - 2));
                        }
                        shownOnList++;
                    }
                    currentOnList++;
                }
            }
            Console.SetCursorPosition(Console.WindowWidth-1, Console.WindowHeight-1);
            //showComponentList();
        }
        public void updateBox(int id_group, int id_object, string text)
        {
            int id = 0;
            for (int i = 0; i < CLIST.Count; i++)
                if (CLIST[i].id_group == id_group && CLIST[i].id_object == id_object) { id = i; break; }
            int L = CLIST[id].view[0].Length;
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
            if (CLIST[id].height != view.Length)
            for (int i = 0; i < CLIST[id].height; i++)
            {
                Console.SetCursorPosition(CLIST[id].posX, CLIST[id].posY + i);
                Console.Write(("").PadRight(CLIST[id].view[0].Length, ' '));
            }
            CLIST[id].view = view;
            CLIST[id].height = view.Length;
            print(CLIST[id]);
        }
        public void updateSlider(int idGroup, int idSel, int count, int value)
        {
            for (int i = 0; i < CLIST.Count; i++)
                if (CLIST[i].id_group == idGroup && CLIST[i].id_object == idSel)
                {
                    int tick = CLIST[i].width / (count) * value + 1;
                    Console.SetCursorPosition(CLIST[i].posX, CLIST[i].posY);
                    Console.Write(" " + ViewFragments.Fragment(CLIST[i].width - 2, '_'));
                    Console.SetCursorPosition(CLIST[i].posX + tick, CLIST[i].posY);
                    Console.Write("||");
                    Console.SetCursorPosition(CLIST[i].posX, CLIST[i].posY + 1);
                    Console.Write("|" + ViewFragments.Fragment(CLIST[i].width - 2, '_') + "|");
                    Console.SetCursorPosition(CLIST[i].posX + tick, CLIST[i].posY + 1);
                    Console.Write("||");
                }
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
        }

        public void clearList()
        {
            int t = 0;
            foreach (Component c in CLIST) switch(c.show)
            {
                case true: clear(c, t * c.height); break;
                case false: t++; break;
                case null: t=0; break;
            }
            CLIST.Clear();
        }
        public void printList()
        {
            int t = 0;
            foreach (Component c in CLIST) switch (c.show)
            {
                case true: print(c, t * c.height); break;
                case false: t++; break;
                case null: t=0; break;
            }
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
        }
        private void clear(Component c, int y = 0)
        {
            for (int i = 0; i < c.height; i++)
            {
                Console.SetCursorPosition(c.posX, c.posY + i - y);
                Console.Write(("").PadRight(c.width,' '));
            }
        }
        private void print(Component c, int y = 0, bool enable = true)
        {
            ConsoleColor base_color = c.base_color;
            ConsoleColor content_color = c.content_color;
            if (enable == false) base_color = content_color = static_base_color;

            Console.ForegroundColor = base_color;
            for (int i = 0; i < c.height; i++)
            {
                Console.SetCursorPosition(c.posX, c.posY + i - y);
                Console.Write(c.view[i]);
            }

            if (c.view[0].Length > 0)
            {
                Console.ForegroundColor = content_color;
                for (int i = 1; i < c.height - 1; i++)
                {
                    Console.SetCursorPosition(c.posX + 1, c.posY + i - y);
                    Console.Write(c.view[i].Substring(1,c.view[i].Length-2));
                }

                if (c.name == "DB")
                {
                    Console.ForegroundColor = base_color;
                    int i = 1; while (c.view[1][i] != '|') i++;
                    Console.SetCursorPosition(c.posX + i, c.posY + 1 - y);
                    Console.Write("|   |");
                }
            }
        }
        public void focus(int id_group_1, int id_group_2 = -10)
        {
            int t = 0;
            foreach (Component c in CLIST)
            {
                if ((c.id_group == id_group_1 || c.id_group == id_group_2) && c.id_object > 0)
                {
                    c.show = true;
                    print(c);
                }
                else switch (c.show)
                {
                    case true: print(c, t * c.height,false); break;
                    case false: t++; break;
                    case null: t=0; break;
                }
            }
        }
        public void showability(int id_group, int id_object, bool show)
        {
            foreach (Component c in CLIST)
                if ((c.id_group == id_group && id_object == 0 && c.id_object > 0) || (c.id_group == id_group && c.id_object == id_object))
                    if(show == true) { c.show = true; print(c); } else { c.show = false; clear(c); }
        }
        private int addComponent(string name, string[] view, bool show = true, int prop = 0, ConsoleColor color1 = static_base_color, ConsoleColor color2 = static_content_color)
        {
            int id_group = 0, id_object = 0, posY = 0, posX = 0, width = view[0].Length, height = view.Length;
            if(CLIST.Count > 0)
            {
                /// ustalanie id_group ///
                id_group = CLIST[CLIST.Count - 1].id_group;
                int group = 1;
                if (CLIST[CLIST.Count - 1].id_object == -2)
                    for (int i = CLIST.Count - 1; i > 0; i--)
                    {
                        if (CLIST[i].id_object == -2) group++;
                        if (CLIST[i].id_object == -1) group--;
                        if (group == 0 || CLIST[i].id_group == 0) { id_group = CLIST[i].id_group;  break; }
                    }
                /// ustalanie posY ///
                posY = CLIST[CLIST.Count - 1].posY + CLIST[CLIST.Count - 1].height;

                /// ustalanie id_object ///
                for (int i = CLIST.Count - 1; i >= 0; i--)
                    if (CLIST[i].id_group == id_group && CLIST[i].id_object >= 0)
                    {
                        id_object = CLIST[i].id_object + 1;
                        break;
                    }
                /// ustalanie posX ///
                if (id_group > 0)
                    for (int i = CLIST.Count - 1; i > 0; i--)
                        if (CLIST[i].id_group == id_group && CLIST[i].id_object == -1)
                        {
                            posX = CLIST[i].posX + ((CLIST[i].width - view[0].Length) / 2);
                            if (posX < 0) posX = 0;
                            else if (posX + view[0].Length > Console.WindowWidth) posX = Console.WindowWidth - posX - view[0].Length;
                        }
            }
            CLIST.Add(new Component(id_group, id_object, posX, posY, width, height, view, name, prop, show, color1, color2));
            return posY;
        }
        public void groupStart(int selColumn)
        {
            int posX = Console.WindowWidth / column * (selColumn-1), id_group = 1;
            int posY = CLIST[CLIST.Count-1].posY + CLIST[CLIST.Count - 1].height;
            int counter = 0;
            for (int i = 0; i < CLIST.Count; i++)
            {
                if (CLIST[i].id_object == -1) counter++;
                if (CLIST[i].id_object == -2) counter--;
            }
            for (int i = CLIST.Count - 1; i > 0; i--)
                if (CLIST[i].id_object == -1)
                { 
                    if (selColumn > 0 && counter > 0) posY = CLIST[i].posY + CLIST[i].height;
                    id_group = CLIST[i].id_group + 1; break; 
                }
            CLIST.Add(new Component(id_group, -1, posX, posY, Console.WindowWidth/column, 0, new string[]{ "" }, "GS", 0, null));
        }
        public void groupEnd()
        {
            int posX = CLIST[CLIST.Count - 1].posX;
            int posY = CLIST[CLIST.Count - 1].posY + CLIST[CLIST.Count - 1].height;
            int id_group = CLIST[CLIST.Count - 1].id_group;
            int group = 1;

            if (CLIST[CLIST.Count - 1].id_object == -2)
                for (int i = CLIST.Count - 1; i > 0; i--)
                {
                    if (CLIST[i].id_object == -1) group--;
                    if (CLIST[i].id_object == -2)
                    {
                        group++;
                        if (posY < CLIST[i].posY + CLIST[i].height) posY = CLIST[i].posY + CLIST[i].height;
                    }
                    if (group == 0 || CLIST[i].id_group == 0) { id_group = CLIST[i].id_group; break; }
                }

            CLIST.Add(new Component(id_group, -2, posX, posY, Console.WindowWidth / column, 0, new string[] { "" }, "GE", 0, null));
        }
        
        public void h1(string text)
        {
            addComponent("H1", new string[]
            {
                ViewFragments.doubleLine(),
                ViewFragments.centeredText(Console.WindowWidth, text),
                ViewFragments.doubleLine()
            });
        }
        public void h2(string text)
        {
            addComponent("H2", new string[]
            {
                ViewFragments.centeredText(Console.WindowWidth, text),
                ViewFragments.singleLine(),
            });
        }
        public void foot(string text)
        {
            string[] view = new string[]
            {
                ViewFragments.singleLine(),
                ViewFragments.centeredText(Console.WindowWidth, text),
                ViewFragments.singleLine()
            };
            CLIST.Add(new Component(0, 0, 0, Console.WindowHeight-3, Console.WindowWidth, 3, view, "FT"));
        }
        public void endl(int x)
        {
            string[] view = new string[x];
            for (int i = 0; i < x; i++) view[i] = "";
            addComponent("EN", view);
        }
        public void text(string content, bool show, ConsoleColor color = ConsoleColor.DarkGray)
        {
            addComponent("TXT", new string[] { content }, show, 0, color);
        }

        public void textBox(int L, string text, bool show = true, ConsoleColor color = ConsoleColor.DarkGray) 
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
            addComponent("TB", view, show, 0 ,color);
        }
        public void singleButton(string text, bool show = true)
        {
            addComponent("SB", new string[]
            {
                ViewFragments.Top(10 + text.Length),
                "|    " + text + "    |",
                ViewFragments.Bot(10 + text.Length)
            }, show, 0);
        }
        public void singleButtonBot(string text, bool show = true)
        {
            addComponent("-SB", new string[]
            {
                ViewFragments.Top(text.Length + 8),
                "|   " + text + "   |",
                ViewFragments.Bot(text.Length + 8)
            }, show);
        }
        public void doubleButton(string text1, string text2, bool show = true, ConsoleColor color = ConsoleColor.DarkGray)
        {
            addComponent("DB", new string[]
            {
                ViewFragments.Top(text1.Length + 6) + "   " + ViewFragments.Top(text2.Length + 6),
                "|  " + text1 + "  |   |  " + text2 + "  |",
                ViewFragments.Bot(text1.Length + 6) + "   " + ViewFragments.Bot(text2.Length + 6)
            }, show, 0, color);
        }
        public void doubleButtonBot(int col, string text1, string text2, bool show = true)
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
        public void rightBar(bool show = true)
        {
            string[] view1 = new string[] { ViewFragments.TopRight(Console.WindowWidth / column) };
            string[] view2 = ViewFragments.SideRight(Console.WindowWidth / column, Console.WindowHeight - 12);
            string[] view3 = new string[] { ViewFragments.BotRight(Console.WindowWidth / column) };
            string[] view = view1.Concat(view2.Concat(view3).ToArray()).ToArray();
            addComponent("RB", view, show);
        }
        public void leftBar(bool show = true)
        {
            string[] view1 = new string[] { ViewFragments.TopLeft(Console.WindowWidth / column) };
            string[] view2 = ViewFragments.SideLeft(Console.WindowWidth / column, Console.WindowHeight - 12);
            string[] view3 = new string[] { ViewFragments.BotLeft(Console.WindowWidth / column) };
            string[] view = view1.Concat(view2.Concat(view3).ToArray()).ToArray();
            addComponent("LB", view, show);
        }
        public void slider(int count, int value, bool show = true)
        {
            int width = 30, left = width / count * value, right = width - left;
            addComponent("SL", new string[]
            {
                " " + ViewFragments.Fragment(left, '_') + "||" + ViewFragments.Fragment(right, '_') + " ",
                "|" + ViewFragments.Fragment(left, '.') + "||" + ViewFragments.Fragment(right, '.') + "|", ""
            }, show, 0, ConsoleColor.White);
        }
        
        public void showComponentList()
        {
            int i = 3, od = 150;
            Console.SetCursorPosition(od,0);
            Console.WriteLine("G | S |name| X | Y | W | H |prop|swow");
            foreach (Component c in CLIST)
            {
                i++;
                if (i + 2 > Console.WindowHeight) { Console.SetCursorPosition(150, i); Console.Write("> > > WIECEJ SIE NIE ZMIESCI < < <"); return; }
                Console.SetCursorPosition(od, i);
                Console.Write(c.id_group + " | " + c.id_object + " | " + c.name + " | " + c.posX + " | " + c.posY+ " | " + c.width + " | " + c.height + " | " + c.prop + " | " + c.show + "             ");
            }
        }
    }
}
