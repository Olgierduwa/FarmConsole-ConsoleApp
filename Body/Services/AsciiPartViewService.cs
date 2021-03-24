using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Services
{
    static class AsciiPartViewService
    {
        public static string SingleLine(int diff = 0)
        {
            return "".PadRight(Console.WindowWidth - diff, '-');
        }
        public static string DoubleLine()
        {
            return "".PadRight(Console.WindowWidth, '=');
        }
        public static string CenteredText(int length, string text)
        {
            return "".PadRight(length / 2 - text.Length / 2, ' ') + text + "".PadRight(length / 2 - text.Length / 2, ' ');
        }
        public static string Top(int lenght, bool left = true, bool right = true)
        {
            string view = "";
            if (left) view = "."; view += "".PadRight(lenght - 2, '─'); if (right) view += ".";
            return view;
        }
        public static string Bot(int lenght, bool left = true, bool right = true)
        {
            string view = "";
            if (left) view = "'"; view += "".PadRight(lenght - 2, '─'); if (right) view += "'";
            return view;
        }
        public static string[] SideLeft(int width, int lenght)
        {
            string[] text = new string[lenght];
            for (int i = 0; i < lenght; i++) text[i] = "".PadRight(width - 1, ' ') + "│";
            return text;
        }
        public static string[] SideRight(int width, int lenght)
        {
            string[] text = new string[lenght];
            for (int i = 0; i < lenght; i++) text[i] = "│" + "".PadRight(width - 1, ' ');
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
            return "".PadRight(lenght, c);
        }
    }
}
