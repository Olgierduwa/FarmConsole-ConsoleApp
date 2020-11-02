using FarmConsole.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace FarmConsole.View
{
    public static class GUI
    {
        public static string title = XF.findString(0);
        public static string foot = XF.findString(1);
        public static ViewTools vt1 = new ViewTools();
        public static ViewTools vt2 = new ViewTools();
        public static ViewTools vt3 = new ViewTools();
        public static void Clear(ViewTools vt0)
        {
            vt0.clearList();
        }
        public static void Menu()
        {
            vt1.h1(title);
            vt1.endl(Console.WindowHeight / 5);

            vt1.groupStart(0);
            vt1.groupStart(2);
            vt1.selOption("Zacznij Rozgrywke ", true);
            vt1.selOption("Kontynuuj Rozrywke", false);
            vt1.selOption("Ustaw Wlasne Opcje", false);
            vt1.selOption("Poznaj Zasady Gry ", false);
            vt1.groupEnd();
            vt1.groupStart(4);
            vt1.infoBlock(32, "\"Swietny Tytul!!\"");
            vt1.infoBlock(32, "\"Oto najlpesza gra konsolowa, o jakiej slyszal swiat!\"");
            vt1.groupEnd();
            vt1.groupEnd();

            vt1.foot(foot);
            vt1.printList();
            //vt1.showComponentList();
        }
        public static void NewGame()
        {
            vt1.h1(title);
            vt1.h2("Rozpocznij Nowa Giereczke");
            vt1.endl(Console.WindowHeight / 5);
            vt1.groupStart(3);
            vt1.selOption("Kobieta  ", true);
            vt1.selOption("Mezczyzna", false);
            vt1.groupEnd();
            vt1.doubleButtonBot(3,"Q / Powrot","E / Enter");
            vt1.foot(foot);
            vt1.printList();
            //vt1.showComponentList();
        }
        public static void Load()
        {
            vt1.h1(title);
            vt1.h2("Wczytaj Giereczke");
            vt1.endl(Console.WindowHeight / 5);
            vt1.groupStart(0);
            vt1.groupStart(2);

            vt1.groupEnd();
            vt1.groupStart(4);

            vt1.groupEnd();
            vt1.groupEnd();

            vt1.foot(foot);
            vt1.printList();
            //vt.showComponentList();
        }
        public static void Options() 
        {
            vt1.h1(title);
            vt1.h2("Ustaw Spersonalizowane Opcje");
            vt1.endl(Console.WindowHeight / 6);
            vt1.groupStart(0);
            vt1.groupStart(2);
            vt1.selOption("Szerokosc Ekranu ", true);
            vt1.selOption("Wysokosc Ekranu  ", false);
            vt1.selOption("Wielkosc Czcionki", false);
            vt1.selOption("Poziom trudnosc  ", false);
            vt1.groupEnd();
            vt1.groupStart(4);
            vt1.slider(6, OPTIONS.getOptionViewById(0));
            vt1.slider(6, OPTIONS.getOptionViewById(1));
            vt1.slider(6, OPTIONS.getOptionViewById(2));
            vt1.slider(6, OPTIONS.getOptionViewById(3));
            vt1.groupEnd();
            vt1.groupEnd();
            vt1.doubleButtonBot(3,"Q / Powrot","E / Enter");

            vt1.foot(foot);
            vt1.printList();
            //vt.showComponentList();
        }
        public static void Help()
        {
            vt1.h1(title);
            vt1.h2("Nie wiesz o co chodzi?");
            vt1.endl(Console.WindowHeight / 5);
            vt1.groupStart(3);
            vt1.infoBlock(40,XF.findText(0));
            vt1.groupEnd();

            vt1.foot(foot);
            vt1.printList();
            //vt.showComponentList();
        }
        public static void Play()
        {
            vt1.h1(title);
            vt1.h2("WITAJ W GRZE!");
            vt1.foot(foot);
            vt1.printList();
            //vt.showComponentList();

            Console.SetCursorPosition(0, 5);
            Console.Write(("|").PadRight(Console.WindowWidth / 5-1, ' ') + "|");
            Console.Write(("|").PadRight(Console.WindowWidth / 5-1, ' ') + "|");
            Console.Write(("|").PadRight(Console.WindowWidth / 5-1, ' ') + "|");
            Console.Write(("|").PadRight(Console.WindowWidth / 5-1, ' ') + "|");
            Console.Write(("|").PadRight(Console.WindowWidth / 5-1, ' ') + "|");

            int h = 43, w = 43;
            char[,] tab = new char[h,w];
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if (i % 6 == 0 && j % 2 == 0) tab[i, j] = '\'';
                    else if (i % 6 == 0 && j % 2 == 1) tab[i, j] = '.';
                    else if (j % 6 == 0 && i % 2 == 0) tab[i, j] = '\'';
                    else if (j % 6 == 0 && i % 2 == 1) tab[i, j] = '.';
                    else tab[i, j] = ' ';
                }
            }

            
            for (int i = 0; i < h; i++)
            {
                //Console.SetCursorPosition(20, 6 + i);
                for (int j = 0; j < w; j++)
                {
                    if(Console.WindowWidth / 2 + (j - i) * 2 < Console.WindowWidth && Console.WindowWidth / 2 + (j - i) * 2 > 0 && 5 + (j + i) / 2 < Console.WindowHeight-3)
                    {
                        Console.SetCursorPosition(Console.WindowWidth / 2 + (j - i) * 2, 5 + (j + i) / 2);
                        Console.Write(tab[i, j]);
                    }
                }
            }


        }
        public static void LeftMenu()
        {
            vt2.endl(6);
            vt2.groupStart(0);
            vt2.groupStart(1);
            vt2.leftBar();
            vt2.groupEnd();
            vt2.groupStart(1);
            vt2.endl(2);
            vt2.selOption("Jeden Jede", true);
            vt2.selOption("Dwa   Dwa ", false);
            vt2.selOption("Trzy  Trzy", false);
            vt2.selOption("Cztery Elo", false);
            vt2.selOption("piec piec ", false);
            vt2.selOption("szesc szes", false);
            vt2.selOption("siedem sie", false, false);
            vt2.selOption("osiem osie", false, false);
            vt2.selOption("dziewiec d", false, false);
            vt2.groupEnd();
            vt2.groupEnd();
            vt2.doubleButtonBot(1,"A / Uzyj", "D / Odrzuc");
            vt2.printList();
            //vt2.showComponentList();
        }
        public static void RightMenu()
        {
            vt3.endl(6);
            vt3.groupStart(0);
            vt3.groupStart(5);
            vt3.rightBar();
            vt3.groupEnd();
            vt3.groupStart(5);
            vt3.endl(2);
            vt3.selOption("Jeden Jede", true);
            vt3.selOption("Dwa   Dwa ", false);
            vt3.selOption("Trzy  Trzy", false);
            vt3.selOption("Cztery Elo", false);
            vt3.groupEnd();
            vt3.groupEnd();
            vt3.doubleButtonBot(5,"A / Uzyj", "D / Wyrzuc");
            vt3.printList();
            //vt3.showComponentList();
        }
    }
}
