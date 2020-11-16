using FarmConsole.Body.Model.Objects;
using System;

namespace FarmConsole.Body.View.GUI
{
    public static partial class GUI
    {
        public static void Play()
        {
            vt1.h1(title);
            vt1.h2("WITAJ W GRZE!");
            vt1.foot(foot);
            vt1.printList();
            //vt.showComponentList();
            //Console.SetCursorPosition(0, 5); for (int i = 0; i < 5; i++) Console.Write(("|").PadRight(Console.WindowWidth / 5-1, ' ') + "|");

            int size = 5;

            int h = size * 6 + 1, w = size * 6 + 1;
            char[,] tab = new char[h, w];
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


            /*for (int i = 0; i < h; i++)
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
            */


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
            vt2.doubleButtonBot(1, "A / Uzyj", "D / Odrzuc");
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
            vt3.doubleButtonBot(5, "A / Uzyj", "D / Wyrzuc");
            vt3.printList();
            //vt3.showComponentList();
        }
    }
}
