﻿using FarmConsole.Body.Controlers;
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
            MapView.GlobalMapInitializate();
            while (MainControllerService.openScreen != "Close")
            {
                switch (MainControllerService.openScreen)
                {
                    case "Menu": MenuController.Open(); break;
                    case "Load": GameLoadController.Open(); break;
                    case "NewGame": GameNewController.Open(); break;
                    case "Options": OptionsController.Open(); break;
                    case "Escape": EscapeController.Open(); break;
                    case "Intro": IntroController.Open(); break;
                    case "Save": GameSaveController.Open(); break;
                    case "Help": HelpController.Open(); break;
                    case "Farm": FarmController.Open(); break;
                }
            }
        }
    }
}
