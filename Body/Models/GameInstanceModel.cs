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
        public Dictionary<string, bool> Rules { get; set; }

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
            LVL = 0;
            UserName = name;
            Difficulty = difficulty;
            Gender = gender;
            Wallet = 420 * (5 - difficulty);
            GameDate = new DateTime(2020, 10, 10);
            Rules = XF.GetRules(Difficulty);

            Maps = new List<MapModel>();

            SetKitStart();
            BuildFarmMap();
            BuildHouseMap();

            Maps.Add(new MapModel("Shop", "Drewniana Podłoga", new Point(2, 8), 10));
            Maps.Add(new MapModel("Street", "Trawa", new Point(10, 10), 20));
        }

        private void BuildFarmMap()
        {
            int Size = 6;
            MapModel FarmMap = new MapModel("Farm", "Trawa", new Point(-2, -2), Size);

            // płot
            for (int x = 2; x < Size; x++) FarmMap.Fields[x, 1] = new FieldModel(ProductModel.GetProduct("Płot", 1));
            for (int y = 2; y < Size; y++) FarmMap.Fields[1, y] = new FieldModel(ProductModel.GetProduct("Płot", 0));
            for (int x = 2; x < Size; x++) FarmMap.Fields[x, Size] = new FieldModel(ProductModel.GetProduct("Płot", 1));
            for (int y = 2; y < Size; y++) FarmMap.Fields[Size, y] = new FieldModel(ProductModel.GetProduct("Płot", 0));
            FarmMap.Fields[1, 1] = new FieldModel(ProductModel.GetProduct("Płot", 5));
            FarmMap.Fields[Size, 1] = new FieldModel(ProductModel.GetProduct("Płot", 4));
            FarmMap.Fields[1, Size] = new FieldModel(ProductModel.GetProduct("Płot", 2));
            FarmMap.Fields[Size, Size] = new FieldModel(ProductModel.GetProduct("Płot", 3));

            // obiekty
            FarmMap.Fields[1, 2] = new FieldModel(ProductModel.GetProduct("Dom"));
            FarmMap.Fields[3, 1] = new FieldModel(ProductModel.GetProduct("Silos"));
            FarmMap.Fields[4, 1] = new FieldModel(ProductModel.GetProduct("Silos"));
            FarmMap.Fields[1, 4] = new FieldModel(ProductModel.GetProduct("Wieża Ciśnień"));
            FarmMap.Fields[Size, Size - 2] = new FieldModel(ProductModel.GetProduct("Brama"));

            Maps.Add(FarmMap);
        }
        private void BuildHouseMap()
        {
            int Size = 6;
            MapModel HouseMap = new MapModel("House", "Drewniana Podłoga", new Point(-2, -1), Size);

            // ściany zewnętrzne
            for (int x = 2; x < Size; x++) HouseMap.Fields[x, 1] = new FieldModel(ProductModel.GetProduct("Ściana", 0));
            for (int y = 2; y < Size; y++) HouseMap.Fields[1, y] = new FieldModel(ProductModel.GetProduct("Ściana", 1));
            for (int x = 2; x < Size; x++) HouseMap.Fields[x, Size] = new FieldModel(ProductModel.GetProduct("Ściana", 2));
            for (int y = 2; y < Size; y++) HouseMap.Fields[Size, y] = new FieldModel(ProductModel.GetProduct("Ściana", 3));
            HouseMap.Fields[Size, 1] = new FieldModel(ProductModel.GetProduct("Ściana", 4));
            HouseMap.Fields[1, 1] = new FieldModel(ProductModel.GetProduct("Ściana", 5));
            HouseMap.Fields[1, Size] = new FieldModel(ProductModel.GetProduct("Ściana", 6));
            HouseMap.Fields[Size, Size] = new FieldModel(ProductModel.GetProduct("Ściana", 7));

            // drzwi wejściowe, okna i meble
            HouseMap.Fields[4, 6] = new FieldModel(ProductModel.GetProduct("Drzwi wejściowe", 2));
            HouseMap.Fields[3, 1] = new FieldModel(ProductModel.GetProduct("Okno", 0));
            HouseMap.Fields[1, 3] = new FieldModel(ProductModel.GetProduct("Okno", 1));
            HouseMap.Fields[3, 6] = new FieldModel(ProductModel.GetProduct("Okno", 2));
            HouseMap.Fields[6, 3] = new FieldModel(ProductModel.GetProduct("Okno", 3));
            HouseMap.Fields[6, 4] = new FieldModel(ProductModel.GetProduct("Okno", 3));
            HouseMap.Fields[4, 4] = new FieldModel(ProductModel.GetProduct("Łóżko"));

            Maps.Add(HouseMap);
        } 
        private void SetKitStart()
        {
            Inventory = new List<ProductModel>
            {
                ProductModel.GetProduct("Portfel"),

                ProductModel.GetProduct("Droga Asfaltowa"),
                ProductModel.GetProduct("Płot"),
                ProductModel.GetProduct("Auto"),
                ProductModel.GetProduct("Brama"),

                ProductModel.GetProduct("Farma"),
                ProductModel.GetProduct("Dom"),
                ProductModel.GetProduct("Sklep Spożywczy"),
                ProductModel.GetProduct("Silos"),
                ProductModel.GetProduct("Wieża Ciśnień"),

                ProductModel.GetProduct("Nasiona Pszenicy"),
                ProductModel.GetProduct("Nasiona Marchwi"),
                ProductModel.GetProduct("Naturalny Nawóz"),
                ProductModel.GetProduct("Syntetyczny Nawóz"),

                ProductModel.GetProduct("Drzwi wejściowe"),
                ProductModel.GetProduct("Ściana"),
                ProductModel.GetProduct("Okno")
            };

            for (int i = 1; i < Inventory.Count; i++) Inventory[i].Amount = 40;
        }
        
        public void Update(int _id)
        {
            DateTime now = DateTime.Now;
            Lastplay = now.ToString();

            string inventoryString = "";
            for (int i = 0; i < Inventory.Count; i++)
                inventoryString += Inventory[i].ToString();

            string[] mapString = new string[Maps.Count];
            for (int i = 0; i < Maps.Count; i ++)
                mapString[i] = Maps[i].ToString();

            string rulesString = "";
            foreach(var rule in Rules)
                rulesString += Convert.ToInt32(rule.Value); 

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
                Inventory.Add(new ProductModel(inventoryString.Substring(index, 3)));

            string rulesString = save["rules"].InnerText;
            var SampleRules = XF.GetRules(0);
            Rules = new Dictionary<string, bool>();
            int indexx = 0;
            foreach (var samplerule in SampleRules)
                Rules.Add(samplerule.Key, Convert.ToBoolean(Convert.ToInt32(rulesString[indexx++]) - 48));

            ReloadMaps();
        }
        public void Delete() { XF.DeleteGameInstance(ID); }
    }
}
