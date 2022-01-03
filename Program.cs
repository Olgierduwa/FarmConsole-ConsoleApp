using FarmConsole.Body.Controlers;
using FarmConsole.Body.Controllers.CentralControllers;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.LocationViews;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.IO;
using System.Text;
using System.Xml.Schema;

namespace FarmConsole
{
    class Program
    {
        static void Main()
        {
            SettingsService.LoadSettings();
            WindowService.PresetWindow();
            MapManager.GlobalMapInit();
            MenuManager.Captions();
            while (MainController.OpenScreen != "Close")
            {
                switch (MainController.OpenScreen)
                {
                    case "Menu": MenuController.Open(); break;
                    case "Load": GameLoadController.Open(); break;
                    case "Save": GameSaveController.Open(); break;
                    case "Escape": EscapeController.Open(); break;
                    case "NewGame": GameNewController.Open(); break;
                    case "Settings": SettingsController.Open(); break;
                    case "Help": HelpController.Open(); break;
                    case "Intro": IntroController.Open(); break;

                    case "Farm": LocationController.Open("Farm"); break;
                    case "House": LocationController.Open("House"); break;
                    case "Street": LocationController.Open("Street"); break;
                    case "Shop": LocationController.Open("Shop"); break;

                    case "Container": ContainerController.Open(); break;
                    case "CashRegister": CashRegisterController.Open(); break;
                }
            }
        }
    }
}
