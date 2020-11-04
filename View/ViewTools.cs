using FarmConsole.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FarmConsole.View
{
    public class ViewTools
    {
        private int column = 5;
        private List<Component> CLIST = new List<Component>();

        private class Component
        {
            public int id_group { get; }
            public int id_object { get; }
            public int posX { get; }
            public int posY { get; }
            public int width { get; }
            public int height { get; }
            public string[] view { get; }
            public int prop { get; set; }
            public string name { get; set; } // jedynie do testów
            public bool show { get; set; }
            public ConsoleColor color { get; set; }
            
            public Component(int id_group, int id_object, int posX, int posY, int width, int height, string[] view,
                             string name, ConsoleColor color = ConsoleColor.DarkGray, int prop = 0, bool show = true)
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
                this.color = color;
            }
        }

        public void updateList(int c1, int c0, int count)
        {
            int id_group = 3;
            int startFrom = 6;
            Component first = CLIST[startFrom + 1];
            int range = (Console.WindowHeight - 17) / first.height;

            if (c0 < c1) {
                if (c0 < count + 1 - range) {
                    CLIST[startFrom + c0].show = false;
                    CLIST[startFrom + c0 + range].show = true;
                }
            } else {
                if (c1 < count + 1 - range) {
                    CLIST[startFrom + c1].show = true;
                    CLIST[startFrom + c1 + range].show = false;
                }
            }

            CLIST[startFrom + c0].prop = 0;
            CLIST[startFrom + c0].color = ConsoleColor.DarkGray;
            CLIST[startFrom + c1].prop = 1;
            CLIST[startFrom + c1].color = ConsoleColor.Yellow;

            int shownOnList = 0, firstShow = -1, currentOnList = 0;
            foreach (Component c in CLIST)
            {
                if (c.id_group == id_group && c.id_object > 0)
                {
                    if (c.show == true)
                    {
                        if (firstShow == -1) firstShow = startFrom - currentOnList;
                        print(c, first.posY + shownOnList * first.height);
                        shownOnList++;
                    }
                    currentOnList++;
                }
            }
            Console.SetCursorPosition(CLIST[firstShow + c0].posX + 4, CLIST[firstShow + c0].posY + 1);
            Console.Write("  ");

            Console.SetCursorPosition(CLIST[firstShow + c1].posX + 4, CLIST[firstShow + c1].posY + 1);
            Console.Write("><");
            
            //Console.Write(firstShow +"+"+ c1);
            showComponentList();
        }
        public void clearList()
        {
            foreach (Component c in CLIST)
            if(c.show == true)
            {
                for (int i = 0; i < c.height; i++)
                {
                    Console.SetCursorPosition(c.posX, c.posY + i);
                    Console.Write(("").PadRight(c.width, ' '));
                }
            }
            CLIST.Clear();
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
        }
        public void printList()
        {
            foreach(Component c in CLIST)
            if(c.show == true) print(c);
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
        }
        private void print(Component c, int posY = -10)
        {
            if(posY == -10) posY = c.posY;
            if (c.view[0].Length == 0) return;
            Console.ForegroundColor = c.color;
            Console.SetCursorPosition(c.posX, c.posY);
            for (int i = 0; i < c.height; i++)
            {
                Console.SetCursorPosition(c.posX, posY + i);
                Console.Write(c.view[i]);
            }
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 1; i < c.height - 1; i++)
            {
                Console.SetCursorPosition(c.posX + 1, posY + i);
                Console.Write(c.view[i].Substring(1,c.view[i].Length-2));
            }
        }
        private int addComponent(string name, string[] view, bool show = true, int prop = 0, ConsoleColor color = ConsoleColor.DarkGray)
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
                for (int i = CLIST.Count - 1; i > 0; i--)
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
            CLIST.Add(new Component(id_group, id_object, posX, posY, width, height, view, name, color, prop, show));
            return posY;
        }
        public void groupStart(int selColumn)
        {
            int posX = Console.WindowWidth / column * (selColumn-1), id_group = 1;
            int posY = CLIST[CLIST.Count-1].posY + CLIST[CLIST.Count - 1].height;
            for (int i = CLIST.Count - 1; i > 0; i--)
                if (CLIST[i].id_object == -1)
                { 
                    if (selColumn > 0) posY = CLIST[i].posY + CLIST[i].height;
                    id_group = CLIST[i].id_group + 1; break; 
                }
            CLIST.Add(new Component(id_group, -1, posX, posY, Console.WindowWidth/column, 0, new string[]{ "" }, "GS", ConsoleColor.DarkGray, 0, false));
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

            CLIST.Add(new Component(id_group, -2, posX, posY, Console.WindowWidth / column, 0, new string[] { "" }, "GE", ConsoleColor.DarkGray, 0, false));
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

        public void selOption(string text, bool sel, bool show = true)
        {
            string check = "><";
            if (sel == false) check = "  ";
            addComponent("SO", new string[]
            {
                ViewFragments.Top(12 + text.Length),
                "|  [" + check + "]  " + text + "  |",
                ViewFragments.Bot(12 + text.Length)
            }, show, Convert.ToInt32(sel));
        }
        public void infoBlock(int length, string text, bool show = true) 
        {
            List<string> content = new List<string>();
            int height = 3 + text.Length / (length - 6);
            int j = 0;
            while (j < text.Length)
            {
                if ((text[j] == ' ' || j + 1 == text.Length) && j >= length - 4)
                {
                    j--;
                    while (j > 0 && text[j] != ' ') j--;
                    string line = text.Substring(0, j);
                    content.Add(("").PadRight((length-2 - line.Length)/2,' ') + line + ("").PadRight(length / 2 - line.Length / 2 - 1, ' '));
                    text = text.Remove(0, j);
                    j = 0;
                }
                j++;
            }
            if (text.Length > 0) content.Add(("").PadRight((length - 2 - text.Length) / 2, ' ') + text + ("").PadRight(length / 2 - text.Length / 2 - 1, ' '));
            string[] view1 = new string[] { ViewFragments.Top(length) };
            string[] view2 = ViewFragments.Sides(content);
            string[] view3 = new string[] { ViewFragments.Bot(length) };
            string[] view = view1.Concat(view2.Concat(view3).ToArray()).ToArray();
            addComponent("IB", view, show);
        }
        public void singleButton(string text, bool show = true)
        {
            addComponent("SB", new string[]
            {
                ViewFragments.Top(text.Length + 8),
                "|   " + text + "   |",
                ViewFragments.Bot(text.Length + 8)
            }, show, 0, ConsoleColor.White);
        }
        public void singleButtonBot(string text, bool show = true)
        {
            addComponent("-SB", new string[]
            {
                ViewFragments.Top(text.Length + 8),
                "|   " + text + "   |",
                ViewFragments.Bot(text.Length + 8)
            }, show, 0, ConsoleColor.White);
        }
        public void doubleButton(string text1, string text2, bool show = true)
        {
            addComponent("DB", new string[]
            {
                ViewFragments.Top(text1.Length + 6) + " " + ViewFragments.Top(text2.Length + 6),
                "|  " + text1 + "  | |  " + text2 + "  |",
                ViewFragments.Bot(text1.Length + 6) + " " + ViewFragments.Bot(text2.Length + 6)
            }, show, 0, ConsoleColor.White);
        }
        public void doubleButtonBot(int col, string text1, string text2, bool show = true)
        {
            string[] view = new string[]
            {
                ViewFragments.Top(text1.Length + 6) + " " + ViewFragments.Top(text2.Length + 6),
                "|  " + text1 + "  | |  " + text2 + "  |",
                ViewFragments.Bot(text1.Length + 6) + " " + ViewFragments.Bot(text2.Length + 6)
            };

            int width = view[0].Length;
            int posX = Console.WindowWidth / column * (col - 1) + (Console.WindowWidth / column - width)/2;
            if (posX < 0) posX = 0;
            else if (posX + width > Console.WindowWidth) posX = Console.WindowWidth - width;

            CLIST.Add(new Component(0, 0, posX, Console.WindowHeight - 8, width, 3, view, "BTB", ConsoleColor.White));
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
        
        public void setSO(int idGroup, int idSelNow, int idSelBefore = -10)
        {
            foreach(Component c in CLIST)
            {
                if (c.id_group == idGroup && (c.id_object == idSelNow))
                {
                    if(c.prop == 1)
                    {
                        c.prop = 0;
                        Console.SetCursorPosition(c.posX + 4, c.posY + 1);
                        Console.Write("  ");
                    }
                    else
                    {
                        c.prop = 1;
                        Console.SetCursorPosition(c.posX + 4, c.posY + 1);
                        Console.Write("><");
                    }
                }
                if (c.id_group == idGroup && (c.id_object == idSelBefore))
                {
                    if (c.prop == 1)
                    {
                        c.prop = 0;
                        Console.SetCursorPosition(c.posX + 4, c.posY + 1);
                        Console.Write("  ");
                    }
                    else
                    {
                        c.prop = 1;
                        Console.SetCursorPosition(c.posX + 4, c.posY + 1);
                        Console.Write("><");
                    }
                }
            }
            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
        }
        public void setSL(int idGroup, int idSel, int count, int value)
        {
            for (int i = 0; i < CLIST.Count; i++)
                if (CLIST[i].id_group == idGroup && CLIST[i].id_object == idSel)
                {
                    int tick = CLIST[i].width / (count) * value + 1;
                    Console.SetCursorPosition(CLIST[i].posX, CLIST[i].posY);
                    Console.Write(" " + ViewFragments.Fragment(CLIST[i].width-2, '_'));
                    Console.SetCursorPosition(CLIST[i].posX + tick, CLIST[i].posY);
                    Console.Write("||");
                    Console.SetCursorPosition(CLIST[i].posX, CLIST[i].posY + 1);
                    Console.Write("|" + ViewFragments.Fragment(CLIST[i].width-2, '_') + "|");
                    Console.SetCursorPosition(CLIST[i].posX + tick, CLIST[i].posY + 1);
                    Console.Write("||");
                }
            Console.SetCursorPosition(Console.WindowWidth-1, Console.WindowHeight - 1);
        }

        public void showComponentList()
        {
            int i = 0;
            Console.SetCursorPosition(50,7);
            Console.WriteLine("G | S |name| X | Y | W | H |prop|swow");
            foreach (Component c in CLIST)
            {
                i++;
                Console.SetCursorPosition(50, 7+i);
                Console.Write(c.id_group + " | " + c.id_object + " | " + c.name + " | " + c.posX + " | " + c.posY+ " | " + c.width + " | " + c.height + " | " + c.prop + " | " + c.show + " ");
            }
        }
    }
}
