using FarmConsole.Body.Controlers;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.LocationViews;
using System;
using System.Xml.Schema;

namespace FarmConsole
{
    class Program
    {
        static void Main()
        {
            WindowService.PresetWindow();
            MapView.GlobalMapInit();
            while (MainController.openScreen != "Close")
            {
                switch (MainController.openScreen)
                {
                    case "Menu": MenuController.Open(); break;
                    case "Load": GameLoadController.Open(); break;
                    case "Save": GameSaveController.Open(); break;
                    case "Escape": EscapeController.Open(); break;
                    case "NewGame": GameNewController.Open(); break;
                    case "Options": OptionsController.Open(); break;
                    case "Help": HelpController.Open(); break;
                    case "Intro": IntroController.Open(); break;

                    case "Farm": LocationController.Open("Farm"); break;
                    case "House": LocationController.Open("House"); break;
                    case "Street": LocationController.Open("Street"); break;
                    case "Shop": LocationController.Open("Shop"); break;
                }
            }
        }
    }
}
