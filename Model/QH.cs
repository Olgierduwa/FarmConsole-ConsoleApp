using FarmConsole.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Model
{
    static class QH // Quick Help
    {
        public static long xxx = 0;
        public static long yyy = 0;
        public static int choice_selector(char c, int x, int r)
        {
            Console.Write("\b ");
            switch (c)
            {
                case 's': if (x < r) { S.play("K1"); return (x + 1); } break;
                case 'w': if (x > 1) { S.play("K1"); return (x - 1); } break;
                case 'e': S.play("K2"); return (-1);
                case 'q': S.play("K3"); return (-2);
            }
            return x;
        }
        public static int choice_side_menu(char c, int x, int r1, int r2)
        {
            Console.Write("\b ");
            if (x > r1 && x < r1 + r2 + 1) switch (c) // r1 - r2
            {
                case 's': if (x < r1 + r2) return (x + 1); break;
                case 'w': if (x > r1 + 1) return (x - 1); break;
            }
            if (x > 0 && x < r1 + 1) switch (c) // 1 - r1
            {
                case 's': if (x < r1) return (x + 1); break;
                case 'w': if (x > 1) return (x - 1); break;
            }
            switch (c) // q e // a d // ESC
            {
                case 'd': return (-4);
                case 'a': return (-3);
                case 'q': return (-2);
                case 'e': return (-1);
                case (char)27: return (-5);
            }
            return x;
        }
        public static void AVG()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            ////////////////////////////////////////////////////

            GUI.vt1.clearList();

            ////////////////////////////////////////////////////
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            yyy++;
            xxx = (xxx + elapsedMs);
            QH.INFO(0, elapsedMs + "  obecna", xxx + "  suma", yyy + "  ilosc", xxx / yyy + "  srednia");
        }
        public static void INFO(int x, string a, string b, string c, string d)
        {
            Console.SetCursorPosition(42 + x * 12, 6); Console.Write(a + "   ");
            Console.SetCursorPosition(42 + x * 12, 7); Console.Write(b + "   ");
            Console.SetCursorPosition(42 + x * 12, 8); Console.Write(c + "   ");
            Console.SetCursorPosition(42 + x * 12, 9); Console.Write(d + "   ");
            Console.SetCursorPosition(Console.WindowWidth-1, Console.WindowHeight-1);
        }
    }
}
