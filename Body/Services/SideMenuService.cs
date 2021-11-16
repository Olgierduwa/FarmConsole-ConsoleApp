﻿using FarmConsole.Body.Controlers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Views.LocationViews;

namespace FarmConsole.Body.Services
{
    public class SideMenuService : MainController
    {
        public static int TradedQuantity { get; set; }
        public static string DONE = XF.GetString("done");

        private static ProductModel P;
        private static void AddAmount(int Value)
        {
            int Index = GameInstance.Inventory.FindIndex(x => x.Category == P.Category && x.Type == P.Type && x.Scale == P.Scale);
            if (Index < 0) GameInstance.Inventory.Add(P); 
            else
            {
                GameInstance.Inventory[Index].Amount += Value;
                if (GameInstance.Inventory[Index].Amount == 0) GameInstance.Inventory.RemoveAt(Index);
            }
        }
        private static decimal GetPrice()
        {
            return GameInstance.Inventory[GameInstance.Inventory.IndexOf(P)].Price;
        }

        public static string DoInInventory(string ToDo, ProductModel Product = null)
        {
            string s;
            SideMenuService.P = Product;
            switch(ToDo)
            {
                // MapView Activities
                case "Zniszcz": AddAmount(-1); break;
                case "Postaw": s = MapView.Build(Product); if (s == DONE) AddAmount(-1); return s;

                // FarmView Activities
                case "Posiej": s = FarmView.Sow(Product); if (s == DONE) AddAmount(-1); return s;
                case "Nawieź": s = FarmView.Fertilize(Product); if (s == DONE) AddAmount(-1); return s;

                // HouseView Activities
                case "Wypij": break;

                // ShopView Activities
                case "Kup": if (GetPrice() * TradedQuantity > GameInstance.Wallet) return XF.GetString("no money");
                            GameInstance.Wallet -= TradedQuantity * GetPrice(); AddAmount(TradedQuantity); break;

                case "Sprzedaj": GameInstance.Wallet += TradedQuantity * GetPrice(); AddAmount(-1 * TradedQuantity); break;

                // Unknown Activities
                default: return XF.GetString("unknown action");
            }
            return DONE;
        }
        public static string DoOnMap(string ToDo)
        {
            string s;
            P = new ProductModel(MapView.GetField());
            switch (ToDo)
            {
                // MapView Activities
                case "Zniszcz": MapView.Destroy(); break;
                case "Przenieś": MapView.Dragg(); break;
                case "Schowaj": int c = MapEngine.GetSelectedFieldCount(); AddAmount(c == 0 ? 1 : c); MapView.Destroy(); break;
                case "Obróć": MapView.Rotate(); break;
                case "Śpij":  GameService.Sleep(); break;
                case "Wejdź": switch (P.ProductName)
                    {
                        case "Dom": openScreen = "House"; break;
                        case "Farma": openScreen = "Farm"; break;
                        case "Sklep Spożywczy": openScreen = "Shop"; break;
                        case "Drzwi wejściowe": openScreen = lastScreen; break;
                    } break;
                case "Wyjdź": switch (escapeScreen)
                    {
                        case "House": openScreen = "Farm"; break;
                        case "Shop": openScreen = "Street"; break;
                        case "Farm": openScreen = "Street"; break;
                    } break;

                // FarmView Activities
                case "Zaoraj": return FarmView.Plow(); 
                case "Podlej": return FarmView.WaterIt();
                case "Skoś": return FarmView.MowGrass();
                case "Zbierz": s = FarmView.Collect(); P = ProductModel.GetProduct(P.Property); if (s == DONE) AddAmount(1); return s;
                case "Zrób nawóz": s = FarmView.MakeFertilize(); P = ProductModel.GetProduct("Naturalny Nawóz"); if(s == DONE) AddAmount(1); return s;

                // HouseView Activities
                case "Umyj": break;

                // Unknown Activities
                default: return XF.GetString("unknown action");
            }
            return DONE;
        }
    }
}
