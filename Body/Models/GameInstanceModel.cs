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

        public FieldModel[,] FarmMap { get; set; }
        public FieldModel [,] HouseMap { get; set; }
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

            BuildFarmMap();
            BuildHouseMap();
            SetKitStart();
        }

        private void BuildFarmMap()
        {
            int FarmMapSize = 8;
            FarmMap = new FieldModel[FarmMapSize, FarmMapSize];

            // trawa
            for (int x = 0; x < FarmMapSize; x++)
                for (int y = 0; y < FarmMapSize; y++)
                    FarmMap[x, y] = new FieldModel(ProductModel.GetProduct("Trawa"));

            FarmMap[0, 0] = new FieldModel(ProductModel.GetProduct("Dom"));

            for (int x = 2; x < 4; x++)
                FarmMap[x, 0] = new FieldModel(ProductModel.GetProduct("Silos"));

            for (int y = 2; y < 3; y++)
                FarmMap[0, y] = new FieldModel(ProductModel.GetProduct("Wieża Ciśnień"));
        }
        private void BuildHouseMap()
        {
            int HouseMapSize = 6;
            HouseMap = new FieldModel[HouseMapSize, HouseMapSize];

            // podłoga
            for (int x = 1; x < HouseMapSize; x++)
                for (int y = 1; y < HouseMapSize; y++)
                    HouseMap[x, y] = new FieldModel(ProductModel.GetProduct("Drewniana Podłoga"));

            // narożniki ścian
            HouseMap[0, 0] = new FieldModel(ProductModel.GetProduct("Narożnik", 1));
            HouseMap[HouseMapSize - 1, 0] = new FieldModel(ProductModel.GetProduct("Narożnik", 0));
            HouseMap[0, HouseMapSize - 1] = new FieldModel(ProductModel.GetProduct("Narożnik", 2));
            HouseMap[HouseMapSize - 1, HouseMapSize - 1] = new FieldModel(ProductModel.GetProduct("Narożnik", 3));

            // ściany zewnętrzne
            for (int x = 1; x < HouseMapSize - 1; x++)
                HouseMap[x, 0] = new FieldModel(ProductModel.GetProduct("Ściana", 0));
            for (int y = 1; y < HouseMapSize - 1; y++)
                HouseMap[0, y] = new FieldModel(ProductModel.GetProduct("Ściana", 1));
            for (int x = 1; x < HouseMapSize - 1; x++)
                HouseMap[x, HouseMapSize - 1] = new FieldModel(ProductModel.GetProduct("Ściana", 2));
            for (int y = 1; y < HouseMapSize - 1; y++)
                HouseMap[HouseMapSize - 1, y] = new FieldModel(ProductModel.GetProduct("Ściana", 3));

            // drzwi wejściowe
            HouseMap[3, 5] = new FieldModel(ProductModel.GetProduct("Drzwi wejściowe", 2));

            // okna
            HouseMap[2, 0] = new FieldModel(ProductModel.GetProduct("Okno", 0));
            HouseMap[0, 2] = new FieldModel(ProductModel.GetProduct("Okno", 1));
            HouseMap[2, 5] = new FieldModel(ProductModel.GetProduct("Okno", 2));
            HouseMap[5, 2] = new FieldModel(ProductModel.GetProduct("Okno", 3));
            HouseMap[5, 3] = new FieldModel(ProductModel.GetProduct("Okno", 3));

            // meble
            HouseMap[3, 3] = new FieldModel(ProductModel.GetProduct("Łóżko"));
        }
        private void SetKitStart()
        {
            Inventory = new List<ProductModel>();

            Inventory.Add(ProductModel.GetProduct("Portfel"));

            Inventory.Add(ProductModel.GetProduct("Dom"));
            Inventory.Add(ProductModel.GetProduct("Silos"));
            Inventory.Add(ProductModel.GetProduct("Wieża Ciśnień"));
            Inventory.Add(ProductModel.GetProduct("Nasiona Przenicy"));
            Inventory.Add(ProductModel.GetProduct("Sadzonka Drzewa"));
            Inventory.Add(ProductModel.GetProduct("Nasiona Rzepaku"));
            Inventory.Add(ProductModel.GetProduct("Naturalny Nawóz"));
            Inventory.Add(ProductModel.GetProduct("Syntetyczny Nawóz"));
            Inventory.Add(ProductModel.GetProduct("Ściana"));
            Inventory.Add(ProductModel.GetProduct("Narożnik"));
            Inventory.Add(ProductModel.GetProduct("Łóżko"));
            Inventory.Add(ProductModel.GetProduct("Drzwi wejściowe"));

            for (int i = 1; i < Inventory.Count; i++) Inventory[i].Amount = 100;
        }
        
        public void Update(int _id)
        {
            string farmMapString = "", houseMapString = "", inventoryString = "";
            int farmMapLength = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(FarmMap.Length)));
            int houseMapLength = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(HouseMap.Length)));

            for (int x = 0; x < farmMapLength; x++)
                for (int y = 0; y < farmMapLength; y++)
                    farmMapString += FarmMap[x, y].ToString();

            for (int x = 0; x < houseMapLength; x++)
                for (int y = 0; y < houseMapLength; y++)
                    houseMapString += HouseMap[x, y].ToString();

            for (int i = 0; i < Inventory.Count; i++)
                inventoryString += Inventory[i].ToString();

            DateTime now = DateTime.Now;
            Lastplay = now.ToString();

            string[] list = new string[]
            {
                "lvl", LVL.ToString(),
                "username", UserName,
                "gamedate", GameDate.ToString("d"),
                "lastplay", Lastplay,
                "wallet", Wallet.ToString(),
                "farmmap", farmMapString,
                "housemap", houseMapString,
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

            string farmMapString = save["farmmap"].InnerText;
            string houseMapString = save["housemap"].InnerText;
            string inventoryString = save["inventory"].InnerText;
            int farmMapLength = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(farmMapString.Length/5)));
            int houseMapLength = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(houseMapString.Length/5)));

            FarmMap = new FieldModel[farmMapLength, farmMapLength];
            for (int x = 0; x < farmMapLength; x++)
                for (int y = 0; y < farmMapLength; y++)
                    FarmMap[x, y] = new FieldModel(farmMapString.Substring(x * farmMapLength * 5 + y * 5, 5), 0);

            HouseMap = new FieldModel[houseMapLength, houseMapLength];
            for (int x = 0; x < houseMapLength; x++)
                for (int y = 0; y < houseMapLength; y++)
                    HouseMap[x, y] = new FieldModel(houseMapString.Substring(x * houseMapLength * 5 + y * 5, 5), 1);

            for (int index = 0; index < inventoryString.Length; index += 5)
                Inventory.Add(new ProductModel(inventoryString.Substring(index, 5)));
        }
        public void Delete() { XF.DeleteGameInstance(ID); }
    }
}
