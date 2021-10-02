using FarmConsole.Body.Models;
using FarmConsole.Body.Views.LocationViews;
using System.Collections.Generic;

namespace FarmConsole.Body.Services
{
    public static class SiteMenuService
    {
        public static List<ProductModel> Products { get; set; }
        public static decimal Wallet { get; set; }

        private static ProductModel P;
        private static void AddAmount(int Value)
        {
            int Index = Products.FindIndex(x => x.Category == P.Category && x.Type == P.Type && x.Scale == P.Scale);
            if (Index < 0) Products.Add(P); 
            else
            {
                Products[Index].Amount += Value;
                if (Products[Index].Amount == 0) Products.RemoveAt(Index);
            }
        }
        private static decimal GetPrice()
        {
            return Products[Products.IndexOf(P)].Price;
        }

        public static string DoInInventory(string ToDo, ProductModel Product = null)
        {
            P = Product;
            switch(ToDo)
            {
                // MapView Activities
                case "Zniszcz": AddAmount(-1); break;
                case "Postaw": if (MapView.Build(Product)) AddAmount(-1); else return "Error"; break;

                // FarmView Activities
                case "Posiej": if (FarmView.Sow(Product)) AddAmount(-1); else return "Error"; break;
                case "Nawieź": if (FarmView.Fertilize(Product)) AddAmount(-1); else return "Error"; break;

                // HouseView Activities
                case "Wypij": break;

                // ShopView Activities
                case "Kup": if (GetPrice() <= Wallet) { Wallet -= GetPrice(); AddAmount(1); } else return "Error"; break;
                case "Sprzedaj": { Wallet += GetPrice(); AddAmount(-1); } break;

                // Unknown Activities
                default: return "Nieznana Aktywność";
            }
            return "Zrobione";
        }
        public static string DoOnMap(string ToDo, ProductModel Product = null)
        {
            P = Product;
            switch (ToDo)
            {
                // MapView Activities
                case "Zniszcz": MapView.Destroy(); break;
                case "Przenieś": MapView.Dragg(); break;
                case "Schowaj": MapView.Destroy(); AddAmount(1); break;
                case "Wejdź": MapView.ComeIn(); break;
                case "Obróć": MapView.Rotate(); break;

                // FarmView Activities
                case "Zaoraj": if (!FarmView.Plow()) return "Error"; break;
                case "Podlej": if (!FarmView.WaterIt()) return "Error"; break;
                case "Skoś": if (!FarmView.MowGrass()) return "Error"; break;
                case "Zbierz": if (!FarmView.Collect()) return "Error"; AddAmount(1); break;
                case "Zrób nawóz": if (!FarmView.MakeFertilize()) return "Error"; AddAmount(1); break;

                // HouseView Activities
                case "Umyj": break;

                // Unknown Activities
                default: return "Nieznana Aktywność";
            }
            return "Zrobione";
        }
    }
}
