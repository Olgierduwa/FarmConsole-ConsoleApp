using FarmConsole.Body.Engines;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.LocationViews;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;

namespace FarmConsole.Body.Models
{
    public class GameInstanceModel
    {
        public int ID { get; set; }
        public int LVL { get; set; }
        public int Gender { get; set; }
        public int Difficulty { get; set; }
        public string UserName { get; set; }
        public string Lastplay { get; set; }
        public string Lastmap { get; set; }
        public decimal WalletFunds { get; set; }
        public decimal CardFunds { get; set; }
        public DateTime GameDate { get; set; }
        public List<RuleModel> Rules { get; set; }

        private List<MapModel> Maps;
        public MapModel GetMap(string Location)
        {
            int index = 0;
            while (index < Maps.Count) if (Maps[index].Name == Location) break; else index++;
            if (index == Maps.Count) return Maps[0];
            return Maps[index];
        }
        public void SetMap(string Location, MapModel Map)
        {
            int index = 0;
            while (index < Maps.Count) if (Maps[index].Name == Location) break; else index++;
            if (index != Maps.Count) Maps[index] = Map;
        }

        public void ReloadMaps(string MapLoaded = "")
        {
            foreach (var map in Maps) map.Reload();
            if(MapLoaded != "")
            {
                MapManager.InitMap(GetMap(MapLoaded));
                MapManager.ShowMap(false);
            }
        }

        public List<ProductModel> Cart { get; set; }
        public List<ProductModel> Inventory { get; set; }
        public List<ProductModel> GetProductsByScale(int scale)
        {
            List<ProductModel> Products = new List<ProductModel>();
            foreach (var P in Inventory)
                if (P.Scale == scale || P.Scale == 2) Products.Add(P);
            return Products;
        }

        public GameInstanceModel()
        {
            Gender = 2;
            Difficulty = 2;
            ID = 0;
            LVL = 0;
            UserName = "- NOBODY -";
            Lastplay = "- NEVER -";
            Lastmap = "- NOWHERE -";
            WalletFunds = 0;
            CardFunds = 0;
        }
        public GameInstanceModel(string id, string lvl, string name, string lastplay, string wallet, string card)
        {
            ID = Convert.ToInt32(id);
            LVL = Convert.ToInt32(lvl);
            UserName = name;
            Lastplay = lastplay;
            WalletFunds = Convert.ToDecimal(wallet);
            CardFunds = Convert.ToDecimal(card);
        }
        public GameInstanceModel(string name, int difficulty, int gender)
        {
            ID = 0;
            LVL = 100;
            UserName = name;
            Difficulty = difficulty;
            Gender = gender;
            WalletFunds = 105 * (5 - difficulty);
            CardFunds = 420 * (5 - difficulty);
            GameDate = new DateTime(2020, 10, 10);
            Rules = XF.GetRules();
            Lastmap = "Farm";
            foreach (var Rule in Rules) Rule.Update(LVL);

            SetKitStart();
            Cart = Inventory;

            Maps = new List<MapModel>();
            BuildFarmMap();
            BuildHouseMap();
            BuildStreetMap();
            BuildShopMap();
            SetMapSupply("Shop");
        }

        private void BuildFarmMap()
        {
            int Size = 6;
            MapModel FarmMap = new MapModel("Farm", "Trawa", new Point(-2, -2), Size);
            FarmMap.EscapeScreen = "Street";

            // płot
            for (int x = 2; x < Size; x++) FarmMap.SetField(x, 1, ObjectModel.GetObject("Płot", 1).ToField());
            for (int y = 2; y < Size; y++) FarmMap.SetField(1, y, ObjectModel.GetObject("Płot", 0).ToField());
            for (int x = 2; x < Size; x++) FarmMap.SetField(x, Size, ObjectModel.GetObject("Płot", 1).ToField());
            for (int y = 2; y < Size; y++) FarmMap.SetField(Size, y, ObjectModel.GetObject("Płot", 0).ToField());
            FarmMap.SetField(1, 1, ObjectModel.GetObject("Płot", 5).ToField());
            FarmMap.SetField(Size, 1, ObjectModel.GetObject("Płot", 4).ToField());
            FarmMap.SetField(1, Size, ObjectModel.GetObject("Płot", 2).ToField());
            FarmMap.SetField(Size, Size, ObjectModel.GetObject("Płot", 3).ToField());


            FarmMap.SetField(Size - 2, Size - 3, ObjectModel.GetObject("Sklep Spożywczy").ToField());

            // obiekty
            FarmMap.SetField(1, 2, ObjectModel.GetObject("Dom").ToField());
            FarmMap.SetField(3, 1, ObjectModel.GetObject("Silos").ToField());
            FarmMap.SetField(4, 1, ObjectModel.GetObject("Silos").ToField());
            FarmMap.SetField(1, 4, ObjectModel.GetObject("Wieża Ciśnień").ToField());
            FarmMap.SetField(Size, Size - 2, ObjectModel.GetObject("Brama").ToField());

            Maps.Add(FarmMap);
        }
        private void BuildHouseMap()
        {
            int Size = 8;
            MapModel HouseMap = new MapModel("House", "Drewniana Podłoga", new Point(-4, 0), Size);

            // ściany zewnętrzne
            for (int x = 2; x <= Size; x++) HouseMap.SetField(x, 1, ObjectModel.GetObject("Sztuczna Trawa").ToField());
            for (int y = 2; y <= Size; y++) HouseMap.SetField(1, y, ObjectModel.GetObject("Sztuczna Trawa").ToField());
            for (int x = 2; x <= Size; x++) HouseMap.SetField(x, Size, ObjectModel.GetObject("Sztuczna Trawa").ToField());
            for (int y = 2; y <= Size; y++) HouseMap.SetField(Size, y, ObjectModel.GetObject("Sztuczna Trawa").ToField());
            for (int x = 2; x < Size; x++) HouseMap.SetField(x, 1, ObjectModel.GetObject("Ściana", 2).ToField());
            for (int y = 2; y < Size; y++) HouseMap.SetField(1, y, ObjectModel.GetObject("Ściana", 3).ToField());
            for (int x = 2; x < Size; x++) HouseMap.SetField(x, Size, ObjectModel.GetObject("Ściana", 0).ToField());
            for (int y = 2; y < Size; y++) HouseMap.SetField(Size, y, ObjectModel.GetObject("Ściana", 1).ToField());
            HouseMap.SetField(1, Size, ObjectModel.GetObject("Ściana", 8).ToField());
            HouseMap.SetField(Size, Size, ObjectModel.GetObject("Ściana", 9).ToField());
            HouseMap.SetField(Size, 1, ObjectModel.GetObject("Ściana", 10).ToField());
            HouseMap.SetField(1, 1, ObjectModel.GetObject("Ściana", 11).ToField());

            // drzwi wejściowe, okna i meble
            HouseMap.SetField(4, Size, ObjectModel.GetObject("Kamienna Płyta").ToField());
            HouseMap.SetField(4, Size, ObjectModel.GetObject("Drzwi wejściowe", 0).ToField());
            HouseMap.SetField(3, 1, ObjectModel.GetObject("Okno", 2).ToField());
            HouseMap.SetField(1, 3, ObjectModel.GetObject("Okno", 3).ToField());
            HouseMap.SetField(3, Size, ObjectModel.GetObject("Okno", 0).ToField());
            HouseMap.SetField(Size, 3, ObjectModel.GetObject("Okno", 1).ToField());
            HouseMap.SetField(Size, 4, ObjectModel.GetObject("Okno", 1).ToField());
            HouseMap.SetField(4, 4, ObjectModel.GetObject("Łóżko").ToField());

            Maps.Add(HouseMap);
        }
        private void BuildShopMap()
        {
            int Size = 7;
            MapModel ShopMap = new MapModel("Shop", "Kafelkowa Podłoga", new Point(0, -3), Size);

            // ściany zewnętrzne
            for (int x = 2; x <= Size; x++) ShopMap.SetField(x, 1, ObjectModel.GetObject("Sztuczna Trawa").ToField());
            for (int y = 2; y <= Size; y++) ShopMap.SetField(1, y, ObjectModel.GetObject("Sztuczna Trawa").ToField());
            for (int x = 2; x <= Size; x++) ShopMap.SetField(x, Size, ObjectModel.GetObject("Sztuczna Trawa").ToField());
            for (int y = 2; y <= Size; y++) ShopMap.SetField(Size, y, ObjectModel.GetObject("Sztuczna Trawa").ToField());
            for (int x = 2; x < Size; x++) ShopMap.SetField(x, 1, ObjectModel.GetObject("Ściana", 2).ToField());
            for (int y = 2; y < Size; y++) ShopMap.SetField(1, y, ObjectModel.GetObject("Ściana", 3).ToField());
            for (int x = 2; x < Size; x++) ShopMap.SetField(x, Size, ObjectModel.GetObject("Ściana", 0).ToField());
            for (int y = 2; y < Size; y++) ShopMap.SetField(Size, y, ObjectModel.GetObject("Ściana", 1).ToField());
            ShopMap.SetField(1, Size, ObjectModel.GetObject("Ściana", 8).ToField());
            ShopMap.SetField(Size, Size, ObjectModel.GetObject("Ściana", 9).ToField());
            ShopMap.SetField(Size, 1, ObjectModel.GetObject("Ściana", 10).ToField());
            ShopMap.SetField(1, 1, ObjectModel.GetObject("Ściana", 11).ToField());

            // drzwi wejściowe, okna i meble
            ShopMap.SetField(Size, 4, ObjectModel.GetObject("Kamienna Płyta").ToField());
            ShopMap.SetField(Size, 4, ObjectModel.GetObject("Drzwi wejściowe", 2).ToField());
            ShopMap.SetField(2, Size/2+1, ObjectModel.GetObject("Kasa", 1).ToField());
            ShopMap.SetField(3, Size, ObjectModel.GetObject("Okno", 0).ToField());
            ShopMap.SetField(4, Size, ObjectModel.GetObject("Okno", 0).ToField());
            ShopMap.SetField(5, Size, ObjectModel.GetObject("Okno", 0).ToField());
            ShopMap.SetField(3, 1, ObjectModel.GetObject("Okno", 2).ToField());
            ShopMap.SetField(4, 1, ObjectModel.GetObject("Okno", 2).ToField());
            ShopMap.SetField(5, 1, ObjectModel.GetObject("Okno", 2).ToField());
            ShopMap.SetField(Size, 3, ObjectModel.GetObject("Okno", 1).ToField());
            ShopMap.SetField(Size, 5, ObjectModel.GetObject("Okno", 1).ToField());
            ShopMap.SetField(1, 3, ObjectModel.GetObject("Okno", 3).ToField());
            ShopMap.SetField(1, 5, ObjectModel.GetObject("Okno", 3).ToField());

            Maps.Add(ShopMap);
        }
        private void BuildStreetMap()
        {
            int Size = 8;
            MapModel StreetMap = new MapModel("Street", "Trawa", new Point(2, Size * 3 / 4 + 1), Size);

            Random rnd = new Random();
            for (int x = 2; x < Size; x++)
                for (int y = 2; y < Size; y++)
                    if (rnd.Next() % 100 < 80 && rnd.Next() % 100 > 40)
                        StreetMap.SetField(x, y, ObjectModel.GetObject("Świerk", rnd.Next() % 4).ToField());

            for (int x = 1; x <= Size; x++) StreetMap.SetField(x, 1, ObjectModel.GetObject("Świerk", rnd.Next() % 4).ToField());
            for (int y = 1; y <= Size; y++) StreetMap.SetField(1, y, ObjectModel.GetObject("Świerk", rnd.Next() % 4).ToField());
            for (int x = 1; x <= Size; x++) StreetMap.SetField(x, Size, ObjectModel.GetObject("Świerk", rnd.Next() % 4).ToField());
            for (int y = 1; y <= Size; y++) StreetMap.SetField(Size, y, ObjectModel.GetObject("Świerk", rnd.Next() % 4).ToField());

            for (int y = 1; y <= Size; y++) StreetMap.SetField(Size - 2, y, ObjectModel.GetObject("Droga", 0).ToField());
            for (int y = 3; y <= Size * 2 / 3 + 1; y++) StreetMap.SetField(Size - 2, y, ObjectModel.GetObject("Droga z Chodnikiem", 0).ToField());
            StreetMap.SetField(Size - 2, Size * 3 / 4, ObjectModel.GetObject("Droga z Chodnikiem", 7).ToField());
            for (int x = 1; x < Size - 2; x++) StreetMap.SetField(x, Size * 3 / 4, ObjectModel.GetObject("Ścieżka", 1).ToField());
            for (int x = 1; x <= Size; x++) StreetMap.SetField(x, 2, ObjectModel.GetObject("Tory", 1).ToField());
            StreetMap.SetField(Size - 2, 2, ObjectModel.GetObject("Przejazd Kolejowy", 1).ToField());
            StreetMap.SetField(1, Size * 3 / 4, ObjectModel.GetObject("Farma", 1).ToField());
            StreetMap.SetField(Size - 3, Size * 3 / 4 - 1, ObjectModel.GetObject("Sklep Spożywczy", 0).ToField());

            Maps.Add(StreetMap);
        }
        public void SetMapSupply(string MapName)
        {
            var ShopMap = GetMap(MapName);
            var Supply = XF.GetSupply(MapName);
            var ContainerList = Supply.ChildNodes;
            if (ContainerList != null)
            {
                var Containers = new ContainerModel[ContainerList.Count];
                var ContainersPos = new Point[ContainerList.Count];
                var DeliveryDays = new List<int>();
                var DeliveryDaysString = Supply.Attributes["deliverydays"].Value.Split(',');
                foreach (var day in DeliveryDaysString) DeliveryDays.Add(int.Parse(day));
                for (int c = 0; c < ContainerList.Count; c++)
                {
                    var Coords = ContainerList[c].Attributes["pos"].Value.Split(',');
                    var ContainerPos = new Point(int.Parse(Coords[0]), int.Parse(Coords[1]));
                    var ContainerState = int.Parse(ContainerList[c].Attributes["state"].Value);
                    var ContainerName = ContainerList[c].Attributes["name"].Value;
                    var ContainerSufix = ContainerList[c].Attributes["sufix"].Value;

                    var Field = ObjectModel.GetObject(ContainerName, ContainerState).ToField();
                    var Slots = new string[ContainerList[c].ChildNodes.Count];
                    for (int s = 0; s < ContainerList[c].ChildNodes.Count; s++)
                        Slots[s] = ContainerList[c].ChildNodes[s].InnerText;

                    var Container = new ContainerModel(Slots, Field.Slots, ContainerSufix);
                    ContainersPos[c] = ContainerPos;
                    Containers[c] = Container;
                    Field.Pocket = Container;
                    ShopMap.SetField(ContainerPos.X, ContainerPos.Y, Field);
                }
                ShopMap.SetSupply(new SupplyModel(Containers, ContainersPos, DeliveryDays, ShopMap.MapSize));
                SetMap(MapName, ShopMap);
            }
        }
        private void SetKitStart()
        {
            if (SettingsService.GODMOD)
            {
                Inventory = new List<ProductModel>();
                var objects = ObjectModel.GetObjects(_State: 0);
                foreach (var o in objects) if (o.Category > 1) Inventory.Add(o.ToProduct(99999));
            }
            else Inventory = new List<ProductModel>
            {
                ObjectModel.GetObject("Portfel").ToProduct(1)
            };
        }
        
        public void Update(int _id)
        {
            DateTime now = DateTime.Now;
            Lastplay = now.ToString();

            string[] mapString = new string[Maps.Count];
            for (int i = 0; i < Maps.Count; i ++)
                mapString[i] = Maps[i].ToString();

            string inventoryString = "";
            foreach (var product in Inventory)
                inventoryString += product.ToString();

            string cartString = "";
            foreach (var product in Cart)
                cartString += product.ToString();

            if (cartString == inventoryString) cartString = "inventory";

            string rulesString = "";
            foreach(var rule in Rules)
                rulesString += Convert.ToInt32(rule.IsAllowed); 

            string[] list = new string[]
            {
                "lvl", LVL.ToString(),
                "username", UserName,
                "difficulty", Difficulty.ToString(),
                "gender", Gender.ToString(),
                "gamedate", GameDate.ToString("d"),
                "lastplay", Lastplay,
                "lastmap", Lastmap,
                "wallet", WalletFunds.ToString(),
                "card", CardFunds.ToString(),
                "inventory", inventoryString,
                "cart", cartString,
                "rules", rulesString
            };

            if (_id == 0) _id = XF.AddGameInstance(list);
            else XF.UpdateGameInstance(list, _id);
            XF.UpdateMaps(mapString, _id);
        }
        public void Load(int _id)
        {
            XmlNode save = XF.GetGameInstance(_id);
            Inventory = new List<ProductModel>();
            Cart = new List<ProductModel>();
            ID = int.Parse(save.Attributes["id"].Value);
            LVL = int.Parse(save["lvl"].InnerText);
            UserName = save["username"].InnerText;
            GameDate = DateTime.Parse(save["gamedate"].InnerText);
            Lastplay = save["lastplay"].InnerText;
            Lastmap = save["lastmap"].InnerText;
            WalletFunds = decimal.Parse(save["wallet"].InnerText);
            CardFunds = decimal.Parse(save["card"].InnerText);
            Difficulty = int.Parse(save["difficulty"].InnerText);
            Gender = int.Parse(save["gender"].InnerText);

            Maps = new List<MapModel>();
            string[] mapsString = XF.LoadMaps(_id);
            foreach (string map in mapsString)
                Maps.Add(new MapModel(map));

            string inventoryString = save["inventory"].InnerText;
            for (int index = 0; index < inventoryString.Length; index += 3)
                Inventory.Add(new ProductModel(inventoryString.Substring(index, 3)).ToProduct());

            string cartString = save["cart"].InnerText;
            if (cartString == "inventory") Cart = Inventory;
            else for (int index = 0; index < cartString.Length; index += 3)
                    Cart.Add(new ProductModel(cartString.Substring(index, 3)).ToProduct());

            string rulesString = save["rules"].InnerText;
            Rules = XF.GetRules();
            foreach (var Rule in Rules) if (rulesString.Length > 0)
            {
                Rule.IsAllowed = Convert.ToBoolean(rulesString[0] - 48);
                rulesString = rulesString.Remove(0, 1);
            }

            ReloadMaps();
        }
        public void Delete() { XF.DeleteGameInstance(ID); }
    }
}
