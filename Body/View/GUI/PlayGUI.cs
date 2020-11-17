using FarmConsole.Body.Model.Objects;
using System;

namespace FarmConsole.Body.View.GUI
{
    public static partial class GUI
    {
        public static void Play()
        {
            vt1.H1(title);
            vt1.H2("WITAJ W GRZE!");
            vt1.Foot(foot);
            vt1.PrintList();
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
            vt2.Endl(6);
            vt2.GroupStart(0);
            vt2.GroupStart(1);
            vt2.LeftBar();
            vt2.GroupEnd();
            vt2.GroupStart(1);
            vt2.Endl(2);
            for (int i = 0; i < component_list.Length; i++)
                if (i < (Console.WindowHeight - 17) / 3)
                    vt2.SingleButton(component_list[i]);
                else vt2.SingleButton(component_list[i], 0, false);
            vt2.GroupEnd();
            vt2.GroupEnd();
            vt2.DoubleButtonBot(1, "A / Uzyj", "D / Odrzuc");
            vt2.PrintList();
            //vt2.showComponentList();
        }
        public static void RightMenu(string[] component_list)
        {
            vt3.Endl(6);
            vt3.GroupStart(0);
            vt3.GroupStart(5);
            vt3.RightBar();
            vt3.GroupEnd();
            vt3.GroupStart(5);
            vt3.Endl(2);
            for (int i = 0; i < component_list.Length; i++)
                if (i < (Console.WindowHeight - 17) / 3)
                    vt3.SingleButton(component_list[i]);
                else vt3.SingleButton(component_list[i], 0, false);
            vt3.GroupEnd();
            vt3.GroupEnd();
            vt3.DoubleButtonBot(5, "A / Uzyj", "D / Wyrzuc");
            vt3.PrintList();
            //vt3.showComponentList();
        }
    }
}
