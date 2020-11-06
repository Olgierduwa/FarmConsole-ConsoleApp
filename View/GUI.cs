﻿using FarmConsole.Model;
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
            vt1.endl(1);
            vt1.singleButton("Zacznij Rozgrywke ");
            vt1.singleButton("Kontynuuj Rozrywke");
            vt1.singleButton("Ustaw Wlasne Opcje");
            vt1.singleButton("Poznaj Zasady Gry ");
            vt1.groupEnd();
            vt1.groupStart(4);
            vt1.endl(1);
            vt1.infoBlock(32, "\"Swietny Tytul!!\"");
            vt1.infoBlock(32, "\"Oto najlpesza gra konsolowa, o jakiej slyszal swiat!\"");
            vt1.groupEnd();
            vt1.groupEnd();

            vt1.foot(foot);
            vt1.printList();
            //vt1.showComponentList();
        }
        public static void Escape()
        {
            vt1.h1(title);
            vt1.endl(Console.WindowHeight / 5);

            vt1.groupStart(0);
            vt1.groupStart(3);
            vt1.endl(1);
            vt1.singleButton("  Kontynuuj ");
            vt1.singleButton(" Zapisz Gre ");
            vt1.singleButton("Wróc do Menu");
            vt1.singleButton(" Ustawienia ");
            vt1.singleButton("  Samouczek ");
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
            vt1.endl(1);
            vt1.singleButton("Kobieta  ");
            vt1.singleButton("Mezczyzna");
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
            vt1.endl(1);
            vt1.singleButton("Szerokosc Ekranu ");
            vt1.singleButton("Wysokosc Ekranu  ");
            vt1.singleButton("Wielkosc Czcionki");
            vt1.singleButton("Poziom trudnosc  ");
            vt1.groupEnd();
            vt1.groupStart(4);
            vt1.endl(1);
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
            vt1.endl(Console.WindowHeight / 9);
            vt1.groupStart(0);
                vt1.groupStart(2);
                vt1.infoBlock(45,"Sterowanie");
                vt1.endl(1);
                vt1.infoBlock(45, XF.findText(1));
                vt1.groupEnd();
                vt1.groupStart(4);
                vt1.infoBlock(45, "Od Autora");
                vt1.endl(1);
                vt1.infoBlock(45, XF.findText(2));
                vt1.groupEnd();
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
            //Console.SetCursorPosition(0, 5); for (int i = 0; i < 5; i++) Console.Write(("|").PadRight(Console.WindowWidth / 5-1, ' ') + "|");

            int size = 5;

            int h = size*6+1, w = size * 6 + 1;
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
        public static void LeftMenu(string[] component_list)
        {
            vt2.endl(6);
            vt2.groupStart(0);
            vt2.groupStart(1);
            vt2.leftBar();
            vt2.groupEnd();
            vt2.groupStart(1);
            vt2.endl(2);
            for (int i = 0; i < component_list.Length; i++)
                if (i < (Console.WindowHeight - 17) / 3)
                     vt2.singleButton(component_list[i]);
                else vt2.singleButton(component_list[i], false);
            vt2.groupEnd();
            vt2.groupEnd();
            vt2.doubleButtonBot(1,"A / Uzyj", "D / Odrzuc");
            vt2.printList();
            //vt2.showComponentList();
        }
        public static void RightMenu(string[] component_list)
        {
            vt3.endl(6);
            vt3.groupStart(0);
            vt3.groupStart(5);
            vt3.rightBar();
            vt3.groupEnd();
            vt3.groupStart(5);
            vt3.endl(2);
            for (int i = 0; i < component_list.Length; i++)
                if (i < (Console.WindowHeight - 17) / 3)
                    vt3.singleButton(component_list[i]);
                else vt3.singleButton(component_list[i], false);
            vt3.groupEnd();
            vt3.groupEnd();
            vt3.doubleButtonBot(5,"A / Uzyj", "D / Wyrzuc");
            vt3.printList();
            //vt3.showComponentList();
        }
    }
}
