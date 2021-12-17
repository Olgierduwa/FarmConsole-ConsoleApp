using FarmConsole.Body.Engines;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.LocationViews;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public decimal Wallet { get; set; }
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
            Wallet = 0;
        }
        public GameInstanceModel(string id, string lvl, string name, string lastplay, string wallet)
        {
            ID = Convert.ToInt32(id);
            LVL = Convert.ToInt32(lvl);
            UserName = name;
            Lastplay = lastplay;
            Wallet = Convert.ToDecimal(wallet);
        }
        public GameInstanceModel(string name, int difficulty, int gender)
        {
            ID = 0;
            LVL = 100;
            UserName = name;
            Difficulty = difficulty;
            Gender = gender;
            Wallet = 420 * (5 - difficulty);
            GameDate = new DateTime(2020, 10, 10);
            Rules = XF.GetRules();
            foreach (var Rule in Rules) Rule.Update(LVL);

            Maps = new List<MapModel>();

            SetKitStart();
            BuildFarmMap();
            BuildHouseMap();

            Maps.Add(new MapModel("Shop", "Kafelkowa Podłoga", new Point(2, 8), 10));
            Maps.Add(new MapModel("Street", "Trawa", new Point(10, 10), 20));
        }

        private void BuildFarmMap()
        {
            int Size = 6;
            MapModel FarmMap = new MapModel("Farm", "Trawa", new Point(-2, -2), Size);

            // płot
            for (int x = 2; x < Size; x++) FarmMap.SetField(x, 1, ObjectModel.GetObject("Płot", 1).ToField());
            for (int y = 2; y < Size; y++) FarmMap.SetField(1, y, ObjectModel.GetObject("Płot", 0).ToField());
            for (int x = 2; x < Size; x++) FarmMap.SetField(x, Size, ObjectModel.GetObject("Płot", 1).ToField());
            for (int y = 2; y < Size; y++) FarmMap.SetField(Size, y, ObjectModel.GetObject("Płot", 0).ToField());
            FarmMap.SetField(1, 1, ObjectModel.GetObject("Płot", 5).ToField());
            FarmMap.SetField(Size, 1, ObjectModel.GetObject("Płot", 4).ToField());
            FarmMap.SetField(1, Size, ObjectModel.GetObject("Płot", 2).ToField());
            FarmMap.SetField(Size, Size, ObjectModel.GetObject("Płot", 3).ToField());

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
            int Size = 6;
            MapModel HouseMap = new MapModel("House", "Drewniana Podłoga", new Point(-2, -1), Size);

            // ściany zewnętrzne
            for (int x = 2; x < Size; x++) HouseMap.SetField(x, 1, ObjectModel.GetObject("Ściana", 0).ToField());
            for (int y = 2; y < Size; y++) HouseMap.SetField(1, y, ObjectModel.GetObject("Ściana", 1).ToField());
            for (int x = 2; x < Size; x++) HouseMap.SetField(x, Size, ObjectModel.GetObject("Ściana", 2).ToField());
            for (int y = 2; y < Size; y++) HouseMap.SetField(Size, y, ObjectModel.GetObject("Ściana", 3).ToField());
            HouseMap.SetField(Size, 1, ObjectModel.GetObject("Ściana", 4).ToField());
            HouseMap.SetField(1, 1, ObjectModel.GetObject("Ściana", 5).ToField());
            HouseMap.SetField(1, Size, ObjectModel.GetObject("Ściana", 6).ToField());
            HouseMap.SetField(Size, Size, ObjectModel.GetObject("Ściana", 7).ToField());

            // drzwi wejściowe, okna i meble
            HouseMap.SetField(4, 6, ObjectModel.GetObject("Drzwi wejściowe", 2).ToField());
            HouseMap.SetField(3, 1, ObjectModel.GetObject("Okno", 0).ToField());
            HouseMap.SetField(1, 3, ObjectModel.GetObject("Okno", 1).ToField());
            HouseMap.SetField(3, 6, ObjectModel.GetObject("Okno", 2).ToField());
            HouseMap.SetField(6, 3, ObjectModel.GetObject("Okno", 3).ToField());
            HouseMap.SetField(6, 4, ObjectModel.GetObject("Okno", 3).ToField());
            HouseMap.SetField(4, 4, ObjectModel.GetObject("Łóżko").ToField());

            Maps.Add(HouseMap);
        } 
        private void SetKitStart()
        {
            Inventory = new List<ProductModel>
            {
                ObjectModel.GetObject("Portfel").ToProduct(),

                ObjectModel.GetObject("Lada").ToProduct(),
                ObjectModel.GetObject("Lodówka").ToProduct(),
                ObjectModel.GetObject("Zamrażarka").ToProduct(),
                ObjectModel.GetObject("Duży Regał").ToProduct(),
                ObjectModel.GetObject("Mały Regał").ToProduct(),
                ObjectModel.GetObject("Stragan").ToProduct(),
                ObjectModel.GetObject("Drzwi").ToProduct(),
                ObjectModel.GetObject("Drzwi wejściowe").ToProduct(),
                ObjectModel.GetObject("Ściana").ToProduct(),
                ObjectModel.GetObject("Okno").ToProduct(),

                ObjectModel.GetObject("Świerk").ToProduct(),
                ObjectModel.GetObject("Droga Asfaltowa").ToProduct(),
                ObjectModel.GetObject("Płot").ToProduct(),
                ObjectModel.GetObject("Auto").ToProduct(),
                ObjectModel.GetObject("Brama").ToProduct(),

                ObjectModel.GetObject("Farma").ToProduct(),
                ObjectModel.GetObject("Dom").ToProduct(),
                ObjectModel.GetObject("Sklep Spożywczy").ToProduct(),
                ObjectModel.GetObject("Silos").ToProduct(),
                ObjectModel.GetObject("Wieża Ciśnień").ToProduct(),

                ObjectModel.GetObject("Nasiona Pszenicy").ToProduct(),
                ObjectModel.GetObject("Nasiona Pietruszki").ToProduct(),
                ObjectModel.GetObject("Nasiona Ziemniaka").ToProduct(),
                ObjectModel.GetObject("Nasiona Marchwi").ToProduct(),
                ObjectModel.GetObject("Nasiona Rzodkiewki").ToProduct(),
                ObjectModel.GetObject("Nasiona Buraka").ToProduct(),

                ObjectModel.GetObject("Naturalny Nawóz").ToProduct(),
                ObjectModel.GetObject("Syntetyczny Nawóz").ToProduct()

            };

            for (int i = 1; i < Inventory.Count; i++) Inventory[i].Amount = 40;
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
                "wallet", Wallet.ToString(),
                "inventory", inventoryString,
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
            ID = int.Parse(save.Attributes["id"].Value);
            LVL = int.Parse(save["lvl"].InnerText);
            UserName = save["username"].InnerText;
            GameDate = DateTime.Parse(save["gamedate"].InnerText);
            Lastplay = save["lastplay"].InnerText;
            Wallet = decimal.Parse(save["wallet"].InnerText);
            Difficulty = int.Parse(save["difficulty"].InnerText);
            Gender = int.Parse(save["gender"].InnerText);

            Maps = new List<MapModel>();
            string[] mapsString = XF.LoadMaps(_id);
            foreach(string map in mapsString)
                Maps.Add(new MapModel(map));

            string inventoryString = save["inventory"].InnerText;
            for (int index = 0; index < inventoryString.Length; index += 3)
                Inventory.Add(new ProductModel(inventoryString.Substring(index, 3)).ToProduct());

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
