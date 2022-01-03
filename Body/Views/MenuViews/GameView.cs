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
            H2(StringService.Get("hello player") + ", " + name + "!");
            PrintList();
        }

        internal static void DisplayFieldName()
        {
            var f = MapEngine.GetField();
            string prefix = f.StateName[0] > '@' ? f.StateName + " " : "";
            string sufix = f.Pocket != null && f.Pocket.SufixName != null &&
                f.Pocket.SufixName.Length > 0 ? " " + f.Pocket.SufixName : "";
            ClearList(false);
            Endl(Console.WindowHeight - 5);
            GroupStart(3);
            BotBar(prefix + f.ObjectName + sufix, height:2, foreground: ColorService.GetColorByName("gray3"));
            GroupEnd();
            PrintList();
        }
    }
}
