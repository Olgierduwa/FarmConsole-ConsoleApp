using FarmConsole.Body.Controlers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Views.LocationViews;
using System.Collections.Generic;

namespace FarmConsole.Body.Services
{
    class SideMenuService : MainController
    {
        public static string DONE = StringService.Get("done");

        private static void AddAmount(int Value)
        {
            if (Value < 0 && SettingsService.GODMOD) return;
            int Index = GameInstance.Inventory.FindIndex(x => x.Category == Action.SelectedProduct.Category
            && x.Type == Action.SelectedProduct.Type && x.Scale == Action.SelectedProduct.Scale);
            if (Index < 0) GameInstance.Inventory.Add(Action.SelectedProduct); 
            else
            {
                GameInstance.Inventory[Index].Amount += Value;
                if (GameInstance.Inventory[Index].Amount == 0) GameInstance.Inventory.RemoveAt(Index);
            }
        }
        private static string DoInInventory()
        {
            string s;
            switch(Action.Name)
            {
                case "destroy": AddAmount(-1); break;
                case "build": s = MapManager.Build(Action.SelectedProduct); if (s == DONE) AddAmount(-1); return s;
                case "sow": s = FarmView.Sow(Action.SelectedProduct); if (s == DONE) AddAmount(-1); return s;
                case "fertilize": s = FarmView.Fertilize(Action.SelectedProduct); if (s == DONE) AddAmount(-1); return s;
                case "drink": break;
                default: return StringService.Get("unknown action");
            }
            return DONE;
        }
        private static string DoOnMap()
        {
            string s;
            Action.SelectedProduct = MapManager.GetField().ToProduct();
            switch (Action.Name)
            {
                // MapView Activities
                case "destroy": return MapManager.Destroy();
                case "move": MapManager.Dragg(); break;
                case "hide": s = MapManager.Destroy(); if(s == DONE) AddAmount(1); return s;
                case "rotate": return MapManager.Rotate();
                case "dig path": MapManager.DigPath(); break;
                case "sleep": GameService.Sleep(); break;
                case "take a look": OpenScreen = "Container"; break;
                case "come in":
                    {
                        string CurrentScreen = OpenScreen;
                        switch (Action.SelectedProduct.ObjectName)
                        {
                            case "Dom": OpenScreen = "House"; break;
                            case "Farma": OpenScreen = "Farm"; break;
                            case "Sklep Spożywczy": OpenScreen = "Shop"; break;
                            case "Drzwi wejściowe": OpenScreen = LastScreen; break;
                        }
                        GameInstance.GetMap(OpenScreen).EscapeScreen = CurrentScreen;
                        GameInstance.GetMap(OpenScreen).Delivery(GameInstance.GameDate);
                        GameInstance.Cart = OpenScreen switch
                        {
                            "House" => GameInstance.Inventory,
                            "Farm" => GameInstance.Inventory,
                            _ => new List<ProductModel>(),
                        };
                    } break;
                case "come out":
                    {
                        OpenScreen = MapEngine.Map.EscapeScreen;
                        MapEngine.Map.SortContainers(GameInstance.Cart);
                        GameInstance.Cart = OpenScreen switch
                        {
                            "House" => GameInstance.Inventory,
                            "Farm" => GameInstance.Inventory,
                            _ => new List<ProductModel>(),
                        };
                    } break;
                case "check": switch (Action.SelectedProduct.ObjectName)
                    {
                        case "Portfel": OpenScreen = "Portfel"; break;
                        case "Telefon": OpenScreen = "Telefon"; break;
                    } break;

                // ShopView Activities
                case "buy": OpenScreen = "CashRegister"; break;
                case "sell": OpenScreen = "CashRegister"; break;

                // FarmView Activities
                case "mow grass": FarmView.MowGrass(); break;
                case "plow": return FarmView.Plow(); 
                case "water it": return FarmView.WaterIt();
                case "collect": s = FarmView.Collect(); Action.SetPropertyProduct(); if(s == DONE) AddAmount(1); return s;
                case "make fertilizer": FarmView.MakeFertilize(); Action.SetProductByName("Naturalny Nawóz"); AddAmount(1); break;

                // HouseView Activities
                case "wash": break;
                case "lock": MapManager.Lock(); break;
                case "unlock": MapManager.Unlock(); break;

                // Unknown Activities
                default: return StringService.Get("unknown action");
            }
            return DONE;
        }

        public static string TakeAction()
        {
            if (MapEngine.GetSelectedFieldCount() > 1)
            {
                var Rule = RuleModel.Find(GameInstance.Rules, "multi " + Action.Name);
                if (Rule == null) return MakeAction();
                if (Rule.IsAllowed || SettingsService.GODMOD) Action.IsInProcess = true;
                else return StringService.Get(Rule.Name) + " (" + StringService.Get("lvl") + ":" + Rule.RequiredLevel + ")";
            }
            return MakeAction();
        }
        public static string MakeAction()
        {
            string result = "unknown action type";
            switch (Action.Type)
            {
                case "OnMap": result = DoOnMap(); break;
                case "InInventory": result = DoInInventory(); break;
            }
            if(MapEngine.GetSelectedFieldCount() == 0) Action.IsInProcess = false;
            return result;
        }
    }
}
