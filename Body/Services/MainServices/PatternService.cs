using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Services.MainServices
{
    static class PatternService
    {
        public static string SingleLine(int diff = 0)
        {
            return "".PadRight(Console.WindowWidth - diff, '-');
        }
        public static string DoubleLine()
        {
            return "".PadRight(Console.WindowWidth, '=');
        }
        public static string CenteredText(string Text, int Width)
        {
            return "".PadRight((Width - Text.Length) / 2, ' ') + Text + "".PadRight(Width - Text.Length - (Width - Text.Length) / 2, ' ');
        }
        public static List<string> CenteredLines(string[] Lines, int Width)
        {
            List<string> Content = new List<string>();
            foreach (string Line in Lines) Content.Add(CenteredText(Line, Width));
            return Content;
        }
        public static List<string> SplitText(string Text, int Width, int Margin)
        {
            string Line = "";
            List<string> Content = new List<string>();
            foreach (string Word in Text.Split(' '))
            {
                if (Line.Length + Word.Length <= Width - (Margin * 2 + 2)) Line += string.IsNullOrEmpty(Line) ? Word : " " + Word;
                else if (Word != "") { Content.Add(CenteredText(Line, Width - 2)); Line = " " + Word; }
            }
            Content.Add(CenteredText(Line, Width - 2));
            return Content;
        }
        public static string Top(int Width, bool left = true, bool right = true)
        {
            string view = "";
            if (left) view = "."; view += "".PadRight(Width - 2, '─'); if (right) view += ".";
            return view;
        }
        public static string Bot(int Width, bool left = true, bool right = true)
        {
            string view = "";
            if (left) view = "'"; view += "".PadRight(Width - 2, '─'); if (right) view += "'";
            return view;
        }
        public static string[] SideLeft(int Width, int Height)
        {
            string[] text = new string[Height];
            for (int i = 0; i < Height; i++) text[i] = "".PadRight(Width - 1, ' ') + "│";
            return text;
        }
        public static string[] SideRight(int Width, int Height)
        {
            string[] text = new string[Height];
            for (int i = 0; i < Height; i++) text[i] = "│" + "".PadRight(Width - 1, ' ');
            return text;
        }
        public static string[] Sides(List<string> Content)
        {
            string[] text = new string[Content.Count];
            for (int i = 0; i < Content.Count; i++) text[i] = "│" + Content[i] + "│";
            return text;
        }
        public static string Fragment(int Width, char c)
        {
            return "".PadRight(Width, c);
        }
        public static string[] ExtendGraphis(string[] baseContent, string[] extendedContent)
        {
            string[] content = extendedContent.Length > baseContent.Length ? new string[extendedContent.Length] : new string[baseContent.Length];
            for (int i = 0; i < content.Length; i++)
            {
                content[i] = i < baseContent.Length ? baseContent[i] : "".PadLeft(baseContent[0].Length, ' ');
                content[i] += i < extendedContent.Length ? extendedContent[i] : "".PadLeft(extendedContent[0].Length);
            }
            return content;
        }
    }
}
