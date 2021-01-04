using FarmConsole.Body.View.GUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Model.Helpers
{
    static class QH
    {
        public static long xxx = 0;
        public static long yyy = 0;

        public static void AVG()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            ////////////////////////////////////////////////////

            GUI.vt1.ClearList();

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
