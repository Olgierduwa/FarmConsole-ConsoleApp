using FarmConsole.Body.Controllers.CentralControllers;
using FarmConsole.Body.Services;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controllers.MenuControllers
{
    class HelpController : HeadController
    {
        public static void Open()
        {
            HelpView.Display();
            OpenScreen = LastScreen;
            Console.ReadKey(true);
            SoundService.Play("K3");
            ComponentService.Clean();
        }
    }
}
