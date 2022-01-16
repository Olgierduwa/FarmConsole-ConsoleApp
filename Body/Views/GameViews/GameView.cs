using FarmConsole.Body.Engines;
using FarmConsole.Body.Services.MainServices;
using Pastel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.GameViews
{
    class GameView : ComponentService
    {
        public static void Display(string name)
        {
            Endl(3);
            H2(LS.Navigation("hello player") + ", " + name + "!");
            PrintList();
        }

        public static void DisplayFieldName()
        {
            var f = MapEngine.GetField();
            string prefix = f.StateName[0] > '@' ? LS.Object(f.StateName) + " " : "";
            string sufix = f.Pocket != null && f.Pocket.SufixName.Length > 0 ? " " + LS.Object(f.Pocket.SufixName) :
                f.Property != "" && f.Property[0] == '*' ? " [" + LS.Object(f.Property[1..]) + "]" : "";
            ClearList(false);
            Endl(Console.WindowHeight - 5);
            GroupStart(3);
            BotBar(prefix + LS.Object(f.ObjectName) + sufix, height: 2, foreground: ColorService.GetColorByName("gray3"));
            GroupEnd();
            PrintList();
        }

        public static void DisplayStats(int[] stats)
        {
            string[] exp = new string[] { ("Poziom " + stats[0] + " ").PadLeft(10, ' ') + "".PadRight(stats[1] * 21 / stats[2], '▄').PadRight(20, '_') };
            string[] energy = new string[] { "Energia ".PadLeft(10, ' ') + "".PadRight(stats[3] / 500, '▄').PadRight(20, '_') };
            string[] hunger = new string[] { "Głód ".PadLeft(10, ' ') + "".PadRight(stats[4] / 50, '▄').PadRight(20, '_') };
            string[] immunity = new string[] { "Odporonść ".PadLeft(10, ' ') + "".PadRight(stats[5] / 50, '▄').PadRight(20, '_') };
            string[] health = new string[] { "Zdrowie ".PadLeft(10, ' ') + "".PadRight(stats[6] / 50, '▄').PadRight(20, '_') };

            ClearList(false);
            Endl(Console.WindowHeight - 8);
            GroupStart(1, 6);
            GraphicBox(exp, true, ColorService.GetColorByName("cyan"));
            GraphicBox(energy, true, ColorService.GetColorByName("limed"));
            GraphicBox(hunger, true, ColorService.GetColorByName("yellow"));
            GraphicBox(immunity, true, ColorService.GetColorByName("orange"));
            GraphicBox(health, true, ColorService.GetColorByName("redl"));
            GroupEnd();
            PrintList();
        }
    }
}
