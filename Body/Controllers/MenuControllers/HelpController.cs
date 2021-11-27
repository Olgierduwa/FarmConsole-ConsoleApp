using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controlers
{
    class HelpController : MainController
    {
        public static void Open()
        {
            HelpView.Display();
            OpenScreen = LastScreen;
            Console.ReadKey(true);
            S.Play("K3");
            HelpView.Clean();
        }
    }
}
