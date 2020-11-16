using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.View.GUI
{
    static class ViewFragments
    {
        public static string singleLine()
        {
            return ("").PadRight(Console.WindowWidth, '-');
        }
        public static string doubleLine()
        {
            return ("").PadRight(Console.WindowWidth, '=');
        }
        public static string centeredText(int length, string text)
        {
            return ("").PadRight(length / 2 - text.Length / 2, ' ') + text + ("").PadRight(length / 2 - text.Length / 2, ' ');
        }
        public static string Top(int lenght)
        {
            return "." + ("").PadRight(lenght - 2, '─') + ".";
        }
        public static string TopLeft(int lenght)
        {
            return ("").PadRight(lenght - 1, '─') + ".";
        }
        public static string TopRight(int lenght)
        {
            return "." + ("").PadRight(lenght - 1, '─');
        }
        public static string Bot(int lenght)
        {
            return "'" + ("").PadRight(lenght - 2, '─') + "'";
        }
        public static string BotLeft(int lenght)
        {
            return ("").PadRight(lenght - 1, '─') + "'";
        }
        public static string BotRight(int lenght)
        {
            return "'" + ("").PadRight(lenght - 1, '─');
        }
        public static string[] SideLeft(int width, int lenght)
        {
            string[] text = new string[lenght];
            for (int i = 0; i < lenght; i++) text[i] = ("").PadRight(width - 1, ' ') + "│";
            return text;
        }
        public static string[] SideRight(int width, int lenght)
        {
            string[] text = new string[lenght];
            for (int i = 0; i < lenght; i++) text[i] = "│" + ("").PadRight(width - 1, ' ');
            return text;
        }
        public static string[] Sides(List<string> content)
        {
            string[] text = new string[content.Count];
            for (int i = 0; i < content.Count; i++) text[i] = "│" + content[i] + "│";
            return text;
        }
        public static string Fragment(int lenght, char c)
        {
            return ("").PadRight(lenght, c);
        }
    }
}
