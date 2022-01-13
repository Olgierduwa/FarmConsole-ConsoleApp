using FarmConsole.Body.Engines;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    class GameView : MenuManager
    {
        public static void Display(string name)
        {
            Endl(3);
            H2(LS.Navigation("hello player") + ", " + name + "!");
            PrintList();
        }

        internal static void DisplayFieldName()
        {
            var f = MapEngine.GetField();
            string prefix = f.StateName[0] > '@' ? LS.Object(f.StateName) + " " : "";
            string sufix = f.Pocket != null && f.Pocket.SufixName.Length > 0 ? " " + LS.Object(f.Pocket.SufixName) :
                f.Property != "" && f.Property[0] == '*' ? " [" +LS.Object(f.Property[1..]) + "]" : "";
            ClearList(false);
            Endl(Console.WindowHeight - 5);
            GroupStart(3);
            BotBar(prefix + LS.Object(f.ObjectName) + sufix, height:2, foreground: ColorService.GetColorByName("gray3"));
            GroupEnd();
            PrintList();
        }
    }
}
