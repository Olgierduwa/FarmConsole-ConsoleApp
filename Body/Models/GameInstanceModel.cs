using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Xml;

namespace FarmConsole.Body.Models
{
    public class GameInstanceModel
    {
        public int ID { get; set; }
        public int LVL { get; set; }
        public string UserName { get; set; }
        public string Lastplay { get; set; }
        public decimal Wallet { get; set; }
        public DateTime GameDate { get; set; }

        public MapModel FarmMap { get; set; }
        public MapModel HouseMap { get; set; }
        public MapModel ShopMap { get; set; }
        public MapModel StreetMap { get; set; }

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
        public GameInstanceModel(string name)
        {
            ID = 0;
            LVL = 0;
            UserName = name;
            Wallet = 420.0M;
            GameDate = new DateTime(2020, 10, 10);

            Maps = new List<MapModel>();

            BuildFarmMap();
            BuildHouseMap();

            Maps.Add(new MapModel("Shop", "Drewniana Podłoga", 10));
            Maps.Add(new MapModel("Street", "Trawa", 20));

            SetKitStart();
        }

        private void BuildFarmMap()
        {
            int Size = 8;
            MapModel FarmMap = new MapModel("Farm", "Trawa", Size);

            FarmMap.Fields[1, 1] = new FieldModel(ProductModel.GetProduct("Dom"));
            FarmMap.Fields[Size, Size - 2] = new FieldModel(ProductModel.GetProduct("Brama"));

            for (int x = 3; x < 5; x++)
                FarmMap.Fields[x, 1] = new FieldModel(ProductModel.GetProduct("Silos"));

            for (int y = 3; y < 4; y++)
                FarmMap.Fields[1, y] = new FieldModel(ProductModel.GetProduct("Wieża Ciśnień"));

            Maps.Add(FarmMap);
        }
        private void BuildHouseMap()
        {
            int Size = 6;
            MapModel HouseMap = new MapModel("House", "Drewniana Podłoga", Size);

            // narożniki ścian
            HouseMap.Fields[1, 1] = new FieldModel(ProductModel.GetProduct("Narożnik", 1));
            HouseMap.Fields[Size, 1] = new FieldModel(ProductModel.GetProduct("Narożnik", 0));
            HouseMap.Fields[1, Size] = new FieldModel(ProductModel.GetProduct("Narożnik", 2));
            HouseMap.Fields[Size, Size] = new FieldModel(ProductModel.GetProduct("Narożnik", 3));

            // ściany zewnętrzne
            for (int x = 2; x < Size; x++)
                HouseMap.Fields[x, 1] = new FieldModel(ProductModel.GetProduct("Ściana", 0));
            for (int y = 2; y < Size; y++)
                HouseMap.Fields[1, y] = new FieldModel(ProductModel.GetProduct("Ściana", 1));
            for (int x = 2; x < Size; x++)
                HouseMap.Fields[x, Size] = new FieldModel(ProductModel.GetProduct("Ściana", 2));
            for (int y = 2; y < Size; y++)
                HouseMap.Fields[Size, y] = new FieldModel(ProductModel.GetProduct("Ściana", 3));

            // drzwi wejściowe
            HouseMap.Fields[4, 6] = new FieldModel(ProductModel.GetProduct("Drzwi wejściowe", 2));

            // okna
            HouseMap.Fields[3, 1] = new FieldModel(ProductModel.GetProduct("Okno", 0));
            HouseMap.Fields[1, 3] = new FieldModel(ProductModel.GetProduct("Okno", 1));
            HouseMap.Fields[3, 6] = new FieldModel(ProductModel.GetProduct("Okno", 2));
            HouseMap.Fields[6, 3] = new FieldModel(ProductModel.GetProduct("Okno", 3));
            HouseMap.Fields[6, 4] = new FieldModel(ProductModel.GetProduct("Okno", 3));

            // meble
            HouseMap.Fields[4, 4] = new FieldModel(ProductModel.GetProduct("Łóżko"));

            Maps.Add(HouseMap);
        }
        private void SetKitStart()
        {
            Inventory = new List<ProductModel>
            {
                ProductModel.GetProduct("Portfel"),

                ProductModel.GetProduct("Płot"),
                ProductModel.GetProduct("Dom"),
                ProductModel.GetProduct("Farma"),
                ProductModel.GetProduct("Sklep Spożywczy"),
                ProductModel.GetProduct("Silos"),
                ProductModel.GetProduct("Wieża Ciśnień"),

                ProductModel.GetProduct("Nasiona Przenicy"),
                ProductModel.GetProduct("Nasiona Marchwi"),
                ProductModel.GetProduct("Naturalny Nawóz"),
                ProductModel.GetProduct("Syntetyczny Nawóz"),

                ProductModel.GetProduct("Drzwi wejściowe"),
                ProductModel.GetProduct("Filar"),
                ProductModel.GetProduct("Ściana"),
                ProductModel.GetProduct("Narożnik")
            };

            for (int i = 1; i < Inventory.Count; i++) Inventory[i].Amount = 20;
        }
        
        public void Update(int _id)
        {
            DateTime now = DateTime.Now;
            Lastplay = now.ToString();

            string inventoryString = "";
            for (int i = 0; i < Inventory.Count; i++)
                inventoryString += Inventory[i].ToString();

            string[] list = new string[]
            {
                "lvl", LVL.ToString(),
                "username", UserName,
                "gamedate", GameDate.ToString("d"),
                "lastplay", Lastplay,
                "wallet", Wallet.ToString(),
                "farmmap", GetMap("Farm").ToString(),
                "housemap", GetMap("House").ToString(),
                "streetmap", GetMap("Street").ToString(),
                "shopmap", GetMap("Shop").ToString(),
                "inventory", inventoryString
            };
            if (_id == 0) XF.AddGameInstance(list);
            else XF.UpdateGameInstance(list, _id);
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

            Maps = new List<MapModel>
            {
                new MapModel("Farm", save["farmmap"].InnerText),
                new MapModel("House", save["housemap"].InnerText)
            };

            string inventoryString = save["inventory"].InnerText;
            for (int index = 0; index < inventoryString.Length; index += 5)
                Inventory.Add(new ProductModel(inventoryString.Substring(index, 5)));
        }
        public void Delete() { XF.DeleteGameInstance(ID); }
    }
}
