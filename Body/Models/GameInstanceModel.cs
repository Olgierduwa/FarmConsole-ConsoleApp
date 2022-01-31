using FarmConsole.Body.Engines;
using FarmConsole.Body.Services.GameServices;
using FarmConsole.Body.Services.MainServices;
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
        public int Experience { get; set; }
        public int Energy { get; set; }
        public int Hunger { get; set; }
        public int Immunity { get; set; }
        public int Health { get; set; }
        public string UserName { get; set; }
        public string Lastplay { get; set; }
        public string Lastmap { get; set; }
        public int WalletFunds { get; set; }
        public int CardFunds { get; set; }
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
        public void SetMapPermissions(string Location, byte AccessLevel)
        {
            for (int i = 0; i < Maps.Count; i++)
                if (Location == "All" || Maps[i].Name == Location)
                    Maps[i].AccessLevel = AccessLevel;
        }
        public void ExpandMap(string Location, Point Vector)
        {
            MapModel map = GetMap(Location);
            map.Expand(Vector);
            SetMap(Location, map);
        }
        public bool IsLocation(string Screen)
        {
            int index = 0;
            while (index < Maps.Count) if (Maps[index].Name == Screen) break; else index++;
            if (index == Maps.Count) return false;
            return true;
        }
        public void ReloadMaps(string MapLoaded = "")
        {
            foreach (var map in Maps) map.Reload();
            if(MapLoaded != "")
            {
                MapService.InitMap(GetMap(MapLoaded));
                MapService.ShowMap(false);
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

        private void BuildFarmMap()
        {
            int Size = 6;
            MapModel FarmMap = new MapModel("Farm", "grass", new Point(-2, -2), Size, _AccessLevel: 3);
            FarmMap.EscapeScreen = "Street";

            BuildingService.BuildFence(FarmMap, new Point(1, 1), new Size(Size, Size));
            //FarmMap.SetField(Size - 2, Size - 3, ObjectModel.GetObject("town hall").ToField());
            FarmMap.SetField(1, 2, ObjectModel.GetObject("house").ToField());
            FarmMap.SetField(3, 1, ObjectModel.GetObject("silo").ToField());
            FarmMap.SetField(4, 1, ObjectModel.GetObject("silo").ToField());
            FarmMap.SetField(1, 4, ObjectModel.GetObject("water tower").ToField());
            FarmMap.SetField(Size, Size - 2, ObjectModel.GetObject("gate").ToField());

            Maps.Add(FarmMap);
        }
        private void BuildHouseMap()
        {
            int Size = 8;
            MapModel HouseMap = new MapModel("House", "spruce wooden floor", new Point(-4, Size), Size, _AccessLevel: 3);
            BuildingService.BuildRectangle(HouseMap, new Point(1, 1), new Size(Size, Size), "synthetic grass", false);
            BuildingService.BuildWalls(HouseMap, new Point(1, 1), new Size(Size, Size));

            // drzwi wejściowe, okna i meble
            HouseMap.SetField(4, Size, ObjectModel.GetObject("stone floor").ToField());
            HouseMap.SetField(4, Size, ObjectModel.GetObject("front door", 0).ToField());
            HouseMap.SetField(3, 1, ObjectModel.GetObject("window", 2).ToField());
            HouseMap.SetField(1, 3, ObjectModel.GetObject("window", 3).ToField());
            HouseMap.SetField(3, Size, ObjectModel.GetObject("window", 0).ToField());
            HouseMap.SetField(Size, 3, ObjectModel.GetObject("window", 1).ToField());
            HouseMap.SetField(Size, 4, ObjectModel.GetObject("window", 1).ToField());

            Maps.Add(HouseMap);
            SetMapSupply("House");
        }
        private void BuildStreetMap()
        {
            int Size = 10;
            MapModel StreetMap = new MapModel("Street", "grass", new Point(1, Size * 3 / 4), Size);

            Random rnd = new Random();
            for (int x = 2; x < Size; x++)
                for (int y = 2; y < Size; y++)
                    if (rnd.Next() % 100 < 80 && rnd.Next() % 100 > 40)
                        StreetMap.SetField(x, y, ObjectModel.GetObject("spruce", rnd.Next() % 4).ToField());

            for (int x = 1; x <= Size; x++) StreetMap.SetField(x, 1, ObjectModel.GetObject("spruce", rnd.Next() % 4).ToField());
            for (int y = 1; y <= Size; y++) StreetMap.SetField(1, y, ObjectModel.GetObject("spruce", rnd.Next() % 4).ToField());
            for (int x = 1; x <= Size; x++) StreetMap.SetField(x, Size, ObjectModel.GetObject("spruce", rnd.Next() % 4).ToField());
            for (int y = 1; y <= Size; y++) StreetMap.SetField(Size, y, ObjectModel.GetObject("spruce", rnd.Next() % 4).ToField());

            for (int y = 1; y <= Size; y++) StreetMap.SetField(Size - 3, y, ObjectModel.GetObject("road", 0).ToField());
            for (int y = 3; y <= Size * 2 / 3 + 1; y++) StreetMap.SetField(Size - 3, y, ObjectModel.GetObject("road with sidewalk", 0).ToField());
            for (int x = Size - 3; x <= Size; x++) StreetMap.SetField(x, Size * 3 / 4 - 1, ObjectModel.GetObject("road with sidewalk", 1).ToField());

            BuildingService.BuildParking(StreetMap, new Point(Size - 2, Size * 3 / 4), new Size(3, 2), true);

            StreetMap.SetField(Size - 3, Size * 3 / 4, ObjectModel.GetObject("road with sidewalk", 7).ToField());
            StreetMap.SetField(Size - 3, Size * 3 / 4 - 1, ObjectModel.GetObject("road with sidewalk", 9).ToField());
            for (int x = 1; x < Size - 3; x++) StreetMap.SetField(x, Size * 3 / 4, ObjectModel.GetObject("path", 1).ToField());
            for (int x = 1; x <= Size; x++) StreetMap.SetField(x, 2, ObjectModel.GetObject("rails", 1).ToField());
            StreetMap.SetField(Size - 3, 2, ObjectModel.GetObject("rail crossing", 1).ToField());
            StreetMap.SetField(1, Size * 3 / 4, ObjectModel.GetObject("farm", 0).ToField());

            StreetMap.SetField(Size - 4, Size * 3 / 4 - 1, ObjectModel.GetObject("agro shop", 0).ToField());
            StreetMap.SetField(Size - 4, Size * 3 / 4 - 2, ObjectModel.GetObject("supermarket", 0).ToField());
            StreetMap.SetField(Size - 4, Size * 3 / 4 - 3, ObjectModel.GetObject("construction shop", 0).ToField());
            StreetMap.SetField(Size - 4, Size * 3 / 4 - 4, ObjectModel.GetObject("town hall", 0).ToField());

            StreetMap.SetField(Size - 1, Size / 2 - 1, ObjectModel.GetObject("trading house", 0).ToField());
            StreetMap.SetField(Size - 2, Size / 2 - 1, ObjectModel.GetObject("trading house", 1).ToField());
            StreetMap.SetField(Size - 2, Size / 2, ObjectModel.GetObject("trading house", 2).ToField());
            StreetMap.SetField(Size - 1, Size / 2, ObjectModel.GetObject("trading house", 3).ToField());

            Maps.Add(StreetMap);
        }
        private void BuildTownHall()
        {
            int Size = 9;
            MapModel TownHallMap = new MapModel("TownHall", "stone floor", new Point(Size, 5), Size);
            BuildingService.BuildRectangle(TownHallMap, new Point(1, 1), new Size(Size, Size), "synthetic grass", false);
            BuildingService.BuildWalls(TownHallMap, new Point(1, 1), new Size(Size, Size));
            for (int y = 2; y < 4; y++) TownHallMap.SetField(6, y, ObjectModel.GetObject("wall", 3).ToField());
            for (int y = 7; y < Size; y++) TownHallMap.SetField(4, y, ObjectModel.GetObject("wall", 1).ToField());
            for (int x = 2; x < 7; x++) TownHallMap.SetField(x, 4, ObjectModel.GetObject("wall", 0).ToField());
            for (int x = 5; x < Size; x++) TownHallMap.SetField(x, 6, ObjectModel.GetObject("wall", 2).ToField());
            TownHallMap.SetField(4, 4, ObjectModel.GetObject("wall", 5).ToField());
            TownHallMap.SetField(4, 6, ObjectModel.GetObject("wall", 6).ToField());

            TownHallMap.SetField(5, 4, ObjectModel.GetObject("door", 0, sufix: "*notary office").ToField());
            TownHallMap.SetField(4, 5, ObjectModel.GetObject("door", 2, sufix: "*no one office").ToField());
            TownHallMap.SetField(5, 6, ObjectModel.GetObject("door", 4, sufix: "*no one office").ToField());
            TownHallMap.SetField(Size, 5, ObjectModel.GetObject("stone floor", 0).ToField());
            TownHallMap.SetField(Size, 5, ObjectModel.GetObject("front door", 2).ToField());

            Maps.Add(TownHallMap);
            SetMapSupply("TownHall");
        }
        private void BuildShopMap(string ShopName, Point Position, int Size = 7)
        {
            MapModel ShopMap = new MapModel(ShopName, "tiled floor", Position, Size);
            BuildingService.BuildRectangle(ShopMap, new Point(1, 1), new Size(Size, Size), "synthetic grass", false);
            BuildingService.BuildWalls(ShopMap, new Point(1, 1), new Size(Size, Size));

            ShopMap.SetField(3, Size, ObjectModel.GetObject("window", 0).ToField());
            ShopMap.SetField(4, Size, ObjectModel.GetObject("window", 0).ToField());
            ShopMap.SetField(5, Size, ObjectModel.GetObject("window", 0).ToField());
            ShopMap.SetField(3, 1, ObjectModel.GetObject("window", 2).ToField());
            ShopMap.SetField(4, 1, ObjectModel.GetObject("window", 2).ToField());
            ShopMap.SetField(5, 1, ObjectModel.GetObject("window", 2).ToField());
            ShopMap.SetField(Size, 3, ObjectModel.GetObject("window", 1).ToField());
            ShopMap.SetField(Size, 5, ObjectModel.GetObject("window", 1).ToField());
            ShopMap.SetField(1, 3, ObjectModel.GetObject("window", 3).ToField());
            ShopMap.SetField(1, 5, ObjectModel.GetObject("window", 3).ToField());

            // drzwi wejściowe, okna i meble
            int doorstate = Position.X == 1 ? 6 : Position.Y == 1 ? 4 : Position.X == 0 ? 2 : 0;
            if (Position.X < 1) Position.X = Size + Position.X;
            if (Position.Y < 1) Position.Y = Size + Position.Y;
            ShopMap.SetField(Position.X, Position.Y, ObjectModel.GetObject("stone floor").ToField());
            ShopMap.SetField(Position.X, Position.Y, ObjectModel.GetObject("front door", doorstate).ToField());

            Maps.Add(ShopMap);
            SetMapSupply(ShopName);
        }
        public void SetMapSupply(string MapName)
        {
            var Supply = XF.GetSupply(MapName);
            if (Supply != null && Supply.ChildNodes != null)
            {
                var ContainerList = Supply.ChildNodes;
                var ShopMap = GetMap(MapName);
                var Containers = new ContainerModel[ContainerList.Count];
                var ContainersPos = new Point[ContainerList.Count];
                var DeliveryDays = new List<int>();
                var DeliveryDaysString = Supply.Attributes["deliverydays"].Value.Split(',');
                foreach (var day in DeliveryDaysString) if (day.Length > 0) DeliveryDays.Add(int.Parse(day));
                for (int c = 0; c < ContainerList.Count; c++)
                {
                    var Attributes = new List<string>();
                    foreach (XmlAttribute a in ContainerList[c].Attributes) Attributes.Add(a.Name);
                    var Coords = ContainerList[c].Attributes["pos"].Value.Split(',');
                    var ContainerPos = new Point(int.Parse(Coords[0]), int.Parse(Coords[1]));
                    var ContainerState = Attributes.Contains("state") ? int.Parse(ContainerList[c].Attributes["state"].Value) : 0;
                    var ContainerSufix = Attributes.Contains("sufix") ? ContainerList[c].Attributes["sufix"].Value : "";
                    var ContainerName = ContainerList[c].Attributes["name"].Value;

                    var Field = ObjectModel.GetObject(ContainerName, ContainerState).ToField();
                    var Slots = new string[ContainerList[c].ChildNodes.Count];
                    for (int s = 0; s < ContainerList[c].ChildNodes.Count; s++)
                        Slots[s] = ContainerList[c].ChildNodes[s].InnerText;

                    foreach (XmlAttribute Attribute in ContainerList[c].Attributes)
                        if (Attribute.Name == "mapAct")
                            Field.MapActions = ConvertService.ConcatActionTables(Attribute.Value.Split(','), Field.MapActions);

                    var Container = new ContainerModel(Slots, Field.Slots, ContainerSufix);
                    ContainersPos[c] = ContainerPos;
                    Containers[c] = Container;
                    Field.Pocket = Container;
                    ShopMap.SetField(ContainerPos.X, ContainerPos.Y, Field);
                } 
                if (DeliveryDaysString[0].Length > 0)
                    ShopMap.SetSupply(new SupplyModel(Containers, ContainersPos, DeliveryDays, ShopMap.MapSize));
                SetMap(MapName, ShopMap);
            }
        }
        private void SetKitStart()
        {
            if (SettingsService.GODMOD)
            {
                Inventory = new List<ProductModel>();
                var objects = ObjectModel.GetObjects();
                foreach (var o in objects)
                    if (o.Category > 3) Inventory.Add(o.ToProduct(10));
                    else  if (o.Category > 2 && o.State == 0) Inventory.Add(o.ToProduct(10));
            }
            else Inventory = new List<ProductModel>
            {
                ObjectModel.GetObject("wallet").ToProduct(1)
            };
        }
        
        public GameInstanceModel(string id, string lvl, string name, string lastplay, string wallet, string card)
        {
            ID = Convert.ToInt32(id);
            LVL = Convert.ToInt32(lvl);
            UserName = name;
            Lastplay = lastplay;
            WalletFunds = int.Parse(wallet) + int.Parse(card);
        }
        public GameInstanceModel(string name, int difficulty, int gender)
        {
            ID = 0;
            LVL = SettingsService.GODMOD ? 1 : 1;
            Experience = 0;
            Energy = 10000;
            Hunger = 0;
            Immunity = 650;
            Health = 1000;

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
            BuildTownHall();
            BuildShopMap("TradingHouse", new Point(3, 0), 12);
            BuildShopMap("AgroShop", new Point(0, -3));
            BuildShopMap("Supermarket", new Point(0, -3));
            BuildShopMap("ConstructionShop", new Point(0, -3));
        }
        public GameInstanceModel(int id)
        {
            XmlNode save = XF.GetGameInstance(id);
            Inventory = new List<ProductModel>();
            Cart = new List<ProductModel>();
            ID = int.Parse(save.Attributes["id"].Value);
            LVL = int.Parse(save["lvl"].InnerText);
            Experience = int.Parse(save["experience"].InnerText);
            Energy = int.Parse(save["energy"].InnerText);
            Hunger = int.Parse(save["hunger"].InnerText);
            Immunity = int.Parse(save["immunity"].InnerText);
            Health = int.Parse(save["health"].InnerText);
            UserName = save["username"].InnerText;
            GameDate = DateTime.Parse(save["gamedate"].InnerText);
            Lastplay = save["lastplay"].InnerText;
            Lastmap = save["lastmap"].InnerText;
            WalletFunds = int.Parse(save["wallet"].InnerText);
            CardFunds = int.Parse(save["card"].InnerText);
            Difficulty = int.Parse(save["difficulty"].InnerText);
            Gender = int.Parse(save["gender"].InnerText);

            Maps = new List<MapModel>();
            string[] mapsString = XF.LoadMaps(id);
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
                "experience", Experience.ToString(),
                "energy", Energy.ToString(),
                "hunger", Hunger.ToString(),
                "immunity", Immunity.ToString(),
                "health", Health.ToString(),
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
        public void Delete() { XF.DeleteGameInstance(ID); }
    }
}
