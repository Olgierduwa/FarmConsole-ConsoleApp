using FarmConsole.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.View
{
    public static class WindowMenager
    {
        private static int windowWidth;
        private static int windowHeight;
        public static void setWindow()
        {
            windowWidth = OPTIONS.getOptionById(0);
            windowHeight = OPTIONS.getOptionById(1);
            Console.SetWindowSize(windowWidth, windowHeight);
            Console.SetBufferSize(windowWidth, windowHeight);
            Console.SetWindowPosition(0, 0);
        }
    }
}
