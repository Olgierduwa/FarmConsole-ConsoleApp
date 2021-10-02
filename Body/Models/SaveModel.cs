using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Xml;

namespace FarmConsole.Body.Models
{
    public class SaveModel
    {
        public int ID { get; set; }
        public int LVL { get; set; }
        public string Name { get; set; }
        public string Lastplay { get; set; }
        public decimal Wallet { get; set; }

        private FieldModel [,] Map;
        public FieldModel[,] GetMap() { return Map; }
        public void SetMap(FieldModel[,] map) { Map = map; }

        private List<ProductModel> Inventory;
        public List<ProductModel> GetInventory() { return Inventory; }

        public SaveModel(string id, string lvl, string name, string lastplay, string wallet)
        {
            ID = Convert.ToInt32(id);
            LVL = Convert.ToInt32(lvl);
            Name = name;
            Lastplay = lastplay;
            Wallet = Convert.ToDecimal(wallet);
        }
        public SaveModel(string name)
        {
            ID = 0;
            LVL = 0;
            Name = name;
            Wallet = 420.0M;
            Map = new FieldModel[4, 4];
            Inventory = new List<ProductModel>();

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    Map[i, j] = new FieldModel(1,0,0);

            Map[0, 0] = new FieldModel(ProductModel.GetProduct("Dom"));
            Map[2, 0] = new FieldModel(ProductModel.GetProduct("Silos"));
            Map[3, 0] = new FieldModel(ProductModel.GetProduct("Silos"));
            Map[0, 2] = new FieldModel(ProductModel.GetProduct("Wieża Ciśnień"));

            Inventory.Add(ProductModel.GetProduct("Dom")); Inventory[0].Amount = 10;
            Inventory.Add(ProductModel.GetProduct("Silos")); Inventory[1].Amount = 10;
            Inventory.Add(ProductModel.GetProduct("Wieża Ciśnień")); Inventory[2].Amount = 10;
            Inventory.Add(ProductModel.GetProduct("Nasiona Przenicy")); Inventory[3].Amount = 10;
            Inventory.Add(ProductModel.GetProduct("Sadzonka Drzewa")); Inventory[4].Amount = 10;
            Inventory.Add(ProductModel.GetProduct("Nasiona Rzepaku")); Inventory[5].Amount = 10;
            Inventory.Add(ProductModel.GetProduct("Naturalny Nawóz")); Inventory[6].Amount = 10;
            Inventory.Add(ProductModel.GetProduct("Syntetyczny Nawóz")); Inventory[7].Amount = 10;
        }
        public SaveModel()
        {
            ID = 0;
            LVL = 0;
            Name = "- NOBODY -";
            Lastplay = "- NEVER -";
            Wallet = 0;
        }

        public void Update(int _id)
        {
            string mapString = "";
            string inventoryString = "";
            int mapLength = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(Map.Length)));

            for (int x = 0; x < mapLength; x++)
                for (int y = 0; y < mapLength; y++)
                    mapString += Map[x, y].ToString();

            for (int i = 0; i < Inventory.Count; i++)
                inventoryString += Inventory[i].ToString();

            string[] list = new string[]
            {
                "lvl", LVL.ToString(),
                "name", Name,
                "lastplay", Lastplay,
                "wallet", Wallet.ToString(),
                "map", mapString,
                "inventory", inventoryString
            };
            if (_id == 0) XF.AddSave(list);
            else XF.UpdateSave(list, _id);
        }
        public void Load(int _id)
        {
            XmlNode save = XF.GetSave(_id);
            Inventory = new List<ProductModel>();
            ID = int.Parse(save.Attributes["id"].Value);
            LVL = int.Parse(save["lvl"].InnerText);
            Name = save["name"].InnerText;
            Lastplay = save["lastplay"].InnerText;
            Wallet = decimal.Parse(save["wallet"].InnerText);

            string mapString = save["map"].InnerText;
            string inventoryString = save["inventory"].InnerText;
            int mapLength = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(mapString.Length/5)));

            Map = new FieldModel[mapLength, mapLength];
            for (int x = 0; x < mapLength; x++)
                for (int y = 0; y < mapLength; y++)
                    Map[x, y] = new FieldModel(mapString.Substring((x + y) * 5, 5), 0);

            for (int index = 0; index < inventoryString.Length; index += 5)
                Inventory.Add(new ProductModel(inventoryString.Substring(index, 5)));
        }
        public void Delete() { XF.DeleteSave(ID); }
    }
}
