using FarmConsole.Body.Controllers.CentralControllers;
using FarmConsole.Body.Controllers.GameControllers;
using FarmConsole.Body.Controllers.MenuControllers;
using FarmConsole.Body.Services.GameServices;
using FarmConsole.Body.Services.MainServices;
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
            MapService.GlobalMapInit();
            ComponentService.Captions();
            while (HeadController.OpenScreen != "Close")
            {
                switch (HeadController.OpenScreen)
                {
                    // menu controllers
                    case "Menu": MainMenuController.Open(); break;
                    case "Load": GameLoadController.Open(); break;
                    case "Save": GameSaveController.Open(); break;
                    case "Escape": EscapeController.Open(); break;
                    case "NewGame": GameNewController.Open(); break;
                    case "Settings": SettingsController.Open(); break;
                    case "Help": HelpController.Open(); break;
                    case "Intro": IntroController.Open(); break;

                    // operation interfaces
                    case "Container": ContainerController.Open(); break;
                    case "ProductOffer": ProductOfferController.Open(); break;
                    case "ProductBuying": ProductBuyingController.Open(); break;
                    case "PlotExtending": PlotExtendingController.Open(); break;
                    case "PlotSelling": PlotSellingController.Open(); break;
                    case "PlotBuying": PlotBuyingController.Open(); break;
                    case "CashMachine": CashMachineController.Open(); break;

                    // location controllers
                    default: LocationController.Open(HeadController.OpenScreen); break;
                }
            }
        }
    }
}
