using FarmConsole.Body.Controlers;
using FarmConsole.Body.Services;
using System;
using System.Xml.Schema;

namespace FarmConsole
{
    class Program
    {
        static void Main()
        {
            WindowService.PresetWindow();
            while (MenuControlerService.openScreen != "Close")
            {
                switch (MenuControlerService.openScreen)
                {
                    case "Menu": MenuControler.Open(); break;
                    case "Load": GameLoadControler.Open(); break;
                    case "NewGame": GameNewControler.Open(); break;
                    case "Options": OptionsControler.Open(); break;
                    case "Escape": EscapeControler.Open(); break;
                    case "Intro": IntroControler.Open(); break;
                    case "Save": GameSaveControler.Open(); break;
                    case "Help": HelpControler.Open(); break;
                    case "Farm": FarmControler.Open(); break;
                }
            }
        }
    }
}
