using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controlers
{
    public class HelpControler : MenuControlerService
    {
        public static void Open()
        {
            HelpView.Show();
            openScreen = lastScreen;
            Console.ReadKey(true);
            S.Play("K3");
            HelpView.ClearList();
        }
    }
}
