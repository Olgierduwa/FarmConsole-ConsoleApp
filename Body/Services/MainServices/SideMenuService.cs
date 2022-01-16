using FarmConsole.Body.Controllers.CentralControllers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services.GameServices;
using System.Collections.Generic;
using System.Linq;

namespace FarmConsole.Body.Services.MainServices
{
    class SideMenuService : HeadController
    {
        public static string DONE = LS.Action("done");
        public static string RESULT { get; set; }

        private static void AddAmount(int Value)
        {
            if (RESULT == DONE)
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
        }
        private static void SetCart()
        {
            GameInstance.Cart = OpenScreen switch
            {
                "House" => GameInstance.Inventory,
                "Farm" => GameInstance.Inventory,
                _ => new List<ProductModel>(),
            };
        }
        private static string GetWalletContent()
        {
            Action.ResultTitle = "wallet";
            string text = LS.Navigation("inside you will find:", "\n");
            string list = "";
            list += "\n* " + GameInstance.WalletFunds + LS.Navigation("currency", " ") + LS.Navigation("in cash");
            list += "\n* " + LS.Object("credit card");
            return text + list;
        }
        public static void GetTired(int Ration)
        {
            if (RESULT == DONE)
            {
                GameService.DecreaseInEnergy(Ration);
                GameService.IncreaseInHunger();
                GameService.IncreaseInExperience(Ration);
            }
        }

        private static void DoInInventory()
        {
            RESULT = DONE;
            switch (Action.Name)
            {
                case "destroy": AddAmount(-1); GetTired(40); break;
                case "build": RESULT = MapService.Build(Action.SelectedProduct); AddAmount(-1); GetTired(50); break;
                case "sow": RESULT = FarmService.Sow(Action.SelectedProduct); AddAmount(-1); GetTired(20); break;
                case "fertilize": RESULT = FarmService.Fertilize(Action.SelectedProduct); AddAmount(-1); GetTired(30); break;
                case "check":
                    {
                        switch (Action.SelectedProduct.ObjectName)
                        {
                            case "wallet": RESULT = GetWalletContent(); break;
                            case "phone": OpenScreen = "Phone"; break;
                        }
                    }
                    break;
                case "drink": break;
                default: RESULT = LS.Action("unknown action"); break;
            }
            Action.Result = RESULT;
        }
        private static void DoOnMap()
        {
            RESULT = DONE;
            Action.SelectedProduct = MapEngine.GetField().ToProduct();
            switch (Action.Name)
            {
                // MapView Activities
                case "destroy": RESULT = MapService.Destroy(); GetTired(40); break;
                case "move": MapService.Dragg(); break;
                case "hide": RESULT = MapService.Destroy(); AddAmount(1); break;
                case "rotate": RESULT = MapService.Rotate(); break;
                case "dig path": MapService.DigPath(); GetTired(20); break;
                case "sleep": RESULT = GameService.Sleep(); break;
                case "take a look": OpenScreen = "Container"; break;
                case "come in":
                    {
                        string CurrentScreen = OpenScreen;
                        OpenScreen = ConvertService.CamelCase(Action.SelectedProduct.ObjectName);
                        GameInstance.GetMap(OpenScreen).EscapeScreen = CurrentScreen;
                        GameInstance.GetMap(OpenScreen).Delivery(GameInstance.GameDate);
                        SetCart();
                    }
                    break;
                case "come out":
                    {
                        OpenScreen = MapEngine.Map.EscapeScreen;
                        MapEngine.Map.SortContainers(GameInstance.Cart);
                        SetCart();
                    }
                    break;

                // Interface Activities
                case "product buying":
                case "product offer":
                case "plot extending":
                case "plot selling":
                case "plot buying": OpenScreen = ConvertService.CamelCase(Action.Name); break;
                case "insert card":
                    if (GameInstance.Inventory.Find(x => x.ObjectName == "wallet") != null) OpenScreen = "CashMachine";
                    else RESULT = LS.Action("no wallet"); break;

                // FarmView Activities
                case "mow grass": FarmService.MowGrass(); GetTired(30); break;
                case "plow": RESULT = FarmService.Plow(); GetTired(30); break;
                case "water it": RESULT = FarmService.WaterIt(); GetTired(10); break;
                case "collect": RESULT = FarmService.Collect(); Action.SetPropertyProduct(); AddAmount(1); GetTired(40); break;
                case "make fertilizer": FarmService.MakeFertilize(); Action.SetProductByName("natural fertilizer"); AddAmount(1); GetTired(40); break;

                // HouseView Activities
                case "wash": break;
                case "lock": MapService.Lock(); break;
                case "unlock": MapService.Unlock(); break;

                // Unknown Activities
                default: RESULT = LS.Action("unknown action"); break;
            }
            Action.Result = RESULT;
        }

        public static void TakeAction()
        {
            if (MapEngine.GetSelectedFieldCount() > 1)
            {
                var Rule = RuleModel.Find(GameInstance.Rules, "multi " + Action.Name);
                if (Rule != null)
                {
                    if (Rule.IsAllowed || SettingsService.GODMOD)
                    {
                        Action.IsInProcess = true;
                        MakeAction();
                    }
                    else Action.Result = LS.Action(Rule.Name) + " (" + LS.Navigation("lvl") + ":" + Rule.RequiredLevel + ")";
                }
            }
            else MakeAction();
        }
        public static bool MakeAction()
        {
            if (GameInstance.Energy > 0)
            {
                switch (Action.Type)
                {
                    case "OnMap": DoOnMap(); break;
                    case "InInventory": DoInInventory(); break;
                }
                if (MapEngine.GetSelectedFieldCount() == 0) Action.IsInProcess = false;
                return true;
            }
            else
            {
                Action.Result = LS.Navigation("no energy");
                Action.IsInProcess = false;
                return false;
            }
        }
    }
}
