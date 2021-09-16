using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Services
{
    class HelpService
    {
        private static System.Diagnostics.Stopwatch timer_1;
        private static long sum_timer_1 = 0;
        private static int timer_1_times;
        private static long avg1;

        private static System.Diagnostics.Stopwatch timer_2;
        private static long sum_timer_2 = 0;
        private static int timer_2_times;
        private static long avg2;

        /* STUFF TO COPY
        
            HelpService.START_TIMER_1();
            HelpService.STOP_TIMER_1();

            HelpService.START_TIMER_2();
            HelpService.STOP_TIMER_2();

            HelpService.AVG();
        */


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
            avg1 = sum_timer_1 / timer_1_times;
        }
        public static void STOP_TIMER_2()
        {
            timer_2.Stop();
            var ms = timer_2.ElapsedTicks;
            timer_2_times++;
            sum_timer_2 += ms;
            avg2 = sum_timer_2 / timer_2_times;
        }
        public static void CLEAR_TIMERS()
        {
            timer_1_times = timer_2_times = 0;
            sum_timer_1 = sum_timer_2 = 0;
        }
        public static void AVG()
        {
            INFO(0, "avg1: " + avg1, "avg2: " + avg2);
            INFO(1, "times1: " + timer_1_times, "times2: " + timer_2_times);
            INFO(2, "sum1: " + sum_timer_1, "sum2: " + sum_timer_2);
            INFO(3, "ct1: " + timer_1.ElapsedTicks, "ct2: " + timer_2.ElapsedTicks);
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
