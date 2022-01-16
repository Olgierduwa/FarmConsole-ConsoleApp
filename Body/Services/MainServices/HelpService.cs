using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Services.MainServices
{
    class HelpService
    {
        private static System.Diagnostics.Stopwatch timer_1;
        private static long sum_timer_1 = 0;
        private static int timer_1_times;
        private static long timer_1_ticks;
        private static long timer_1_ms;
        private static long tick_avg1;

        private static System.Diagnostics.Stopwatch timer_2;
        private static long sum_timer_2 = 0;
        private static int timer_2_times;
        private static long timer_2_ticks;
        private static long timer_2_ms;
        private static long tick_avg2;

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
            long tick = timer_1.ElapsedTicks;
            long ms = timer_1.ElapsedMilliseconds;
            timer_1_ticks = tick;
            timer_1_ms = ms;
            timer_1_times++;
            sum_timer_1 += tick;
            tick_avg1 = sum_timer_1 / timer_1_times;
        }
        public static void STOP_TIMER_2()
        {
            timer_2.Stop();
            var tick = timer_2.ElapsedTicks;
            var ms = timer_2.ElapsedMilliseconds;
            timer_2_ticks = tick;
            timer_2_ms = ms;
            timer_2_times++;
            sum_timer_2 += tick;
            tick_avg2 = sum_timer_2 / timer_2_times;
        }
        public static void CLEAR_TIMERS()
        {
            timer_1_times = timer_2_times = 0;
            sum_timer_1 = sum_timer_2 = 0;
        }
        public static void AVG()
        {
            INFO(0, "avg1: " + tick_avg1, "avg2: " + tick_avg2);
            INFO(1, "tms1: " + timer_1_times, "tms2: " + timer_2_times);
            INFO(2, "sum1: " + sum_timer_1, "sum2: " + sum_timer_2);
            INFO(3, "tic1: " + timer_1_ticks, "tic2: " + timer_2_ticks);
            INFO(4, "ms1: " + timer_1_ms, "ms2: " + timer_2_ms);
        }
        public static void INFO(int x, string a, string b = "", string c = "", string d = "")
        {
            Console.SetCursorPosition(12 + x * 20, 6); Console.Write(a + "   ");
            Console.SetCursorPosition(12 + x * 20, 7); Console.Write(b + "   ");
            Console.SetCursorPosition(12 + x * 20, 8); Console.Write(c + "   ");
            Console.SetCursorPosition(12 + x * 20, 9); Console.Write(d + "   ");
            Console.SetCursorPosition(0, 0);
        }
    }
}
