using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Services
{
    class HelpService
    {
        public static System.Diagnostics.Stopwatch timer_1;
        public static long sum_timer_1 = 0;
        public static int timer_1_times;
        public static long avg1;

        public static System.Diagnostics.Stopwatch timer_2;
        public static long sum_timer_2 = 0;
        public static int timer_2_times;
        public static long avg2;

        public static void START_TIMER_1()
        {
            timer_1 = System.Diagnostics.Stopwatch.StartNew();
        }
        public static void START_TIMER_2()
        {
            timer_2 = System.Diagnostics.Stopwatch.StartNew();
        }
        public static void STOP_TIMER_1()
        {
            timer_1.Stop();
            long ms = timer_1.ElapsedTicks;
            timer_1_times++;
            sum_timer_1 += ms;
            avg1 = ms;
        }
        public static void STOP_TIMER_2()
        {
            timer_2.Stop();
            var ms = timer_2.ElapsedTicks;
            timer_2_times++;
            sum_timer_2 += ms;
            avg2 = ms;
        }
        public static void CLEAR_TIMERS()
        {
            timer_1_times = timer_2_times = 0;
            sum_timer_1 = sum_timer_2 = 0;
        }
        public static void AVG()
        {
            INFO(0, "timer1: " + avg1, "timer2: " + avg2);
            INFO(1, "times1: " + timer_1_times, "times2: " + timer_2_times);
            INFO(2, "sum1: " + sum_timer_1, "sum2: " + sum_timer_2);
        }
        public static void INFO(int x, string a, string b = "", string c = "", string d = "")
        {
            Console.SetCursorPosition(42 + x * 12, 6); Console.Write(a + "   ");
            Console.SetCursorPosition(42 + x * 12, 7); Console.Write(b + "   ");
            Console.SetCursorPosition(42 + x * 12, 8); Console.Write(c + "   ");
            Console.SetCursorPosition(42 + x * 12, 9); Console.Write(d + "   ");
            Console.SetCursorPosition(0, 0);
        }
    }
}
