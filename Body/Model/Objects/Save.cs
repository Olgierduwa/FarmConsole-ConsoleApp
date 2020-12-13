using FarmConsole.Body.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace FarmConsole.Body.Model.Objects
{
    public class Save
    {
        public int ID { get; set; }
        public int LVL { get; set; }
        public string Name { get; set; }
        public string Lastplay { get; set; }
        public decimal Wallet { get; set; }

        private int[,,] Map;
        public int[,,] GetMap() { return Map; }
        public void SetMap(int[,,] map) { this.Map = map; }

        private List<Product>[] Inventory;
        public List<Product>[] GetInventory() { return Inventory; }

        public Save(string id, string lvl, string name, string lastplay, string wallet)
        {
            this.ID = Convert.ToInt32(id);
            this.LVL = Convert.ToInt32(lvl);
            this.Name = name;
            this.Lastplay = lastplay;
            this.Wallet = Convert.ToDecimal(wallet);
        } // gost constructor
        public Save(string name) 
        {
            this.ID = 0;
            this.LVL = 0;
            this.Name = name;
            this.Wallet = 420.0M;
            this.Map = new int[4, 4, 3];
            this.Inventory = new List<Product>[9];

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                {
                    Map[i, j, 0] = 0;
                    Map[i, j, 1] = 2;
                    Map[i, j, 2] = 0;
                }
            Map[0, 0, 0] = 6; Map[0, 0, 1] = 0;
            Map[2, 0, 0] = 7; Map[2, 0, 1] = 0;
            Map[3, 0, 0] = 7; Map[3, 0, 1] = 0;
            Map[0, 2, 0] = 7; Map[0, 2, 1] = 1;

            for (int i = 0; i < Inventory.Length; i++) Inventory[i] = new List<Product>();
            Inventory[1].Add(new Product(1, 0, 10));
            Inventory[2].Add(new Product(2, 0, 10));
            Inventory[2].Add(new Product(2, 1, 10));
            Inventory[3].Add(new Product(3, 0, 10));
            Inventory[4].Add(new Product(4, 0, 10));
            Inventory[5].Add(new Product(5, 0, 10));
            Inventory[6].Add(new Product(6, 0, 10));
            Inventory[7].Add(new Product(7, 0, 10));
            Inventory[8].Add(new Product(8, 0, 10));

        } // new constructor
        public Save() {} // empty constructor

        public void Update(int _id)
        {
            string fieldCategory = "";
            string fieldType = "";
            string fieldDuration = "";
            string inventory = "";

            int mapLength = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(Map.Length/3)));
            for (int i = 0; i < mapLength; i++)
                for (int j = 0; j < mapLength; j++)
                {
                    fieldCategory = fieldCategory + Map[i, j, 0].ToString() + " ";
                    fieldType = fieldType + Map[i, j, 1].ToString() + " ";
                    fieldDuration = fieldDuration + Map[i, j, 2].ToString() + " ";
                }

            for (int i = 0; i < Inventory.Length; i++)
                for (int j = 0; j < Inventory[i].Count; j++)
                {
                    inventory = inventory + Inventory[i][j].category + " ";
                    inventory = inventory + Inventory[i][j].type + " ";
                    inventory = inventory + Inventory[i][j].amount + " ";
                }

            string[] list = new string[]
            {
                "lvl", LVL.ToString(),
                "name", Name,
                "lastplay", Lastplay,
                "wallet", Wallet.ToString(),
                "field-category", fieldCategory,
                "field-type", fieldType,
                "field-duration", fieldDuration,
                "inventory", inventory
            };
            if(_id == 0) XF.AddSave(list);
            else XF.UpdateSave(list, _id);
        }
        public void Load(int _id)
        {
            XmlNode save = XF.GetSave(_id);
            ID = int.Parse(save.Attributes["id"].Value);
            LVL = int.Parse(save["lvl"].InnerText);
            Name = save["name"].InnerText;
            Lastplay = save["lastplay"].InnerText;
            Wallet = decimal.Parse(save["wallet"].InnerText);
            Inventory = new List<Product>[9];
            for (int i = 0; i < Inventory.Length; i++) Inventory[i] = new List<Product>();

            string[] fieldCategory = Regex.Replace(save["field-category"].InnerText, @"\s+", " ", RegexOptions.Multiline).Split(' ');
            string[] fieldType = Regex.Replace(save["field-type"].InnerText, @"\s+", " ", RegexOptions.Multiline).Split(' ');
            string[] fieldDuration = Regex.Replace(save["field-duration"].InnerText, @"\s+", " ", RegexOptions.Multiline).Split(' ');
            string[] inventory = Regex.Replace(save["inventory"].InnerText, @"\s+", " ", RegexOptions.Multiline).Split(' ');

            for (int i = 0; i < inventory.Length-1; i += 3)
            {
                int productCategory = int.Parse(inventory[i]);
                int productType = int.Parse(inventory[i + 1]);
                int productAmount = int.Parse(inventory[i + 2]);
                Product p = XF.GetProduct(productCategory, productType);
                p.amount = productAmount;
                Inventory[productCategory].Add(p);
            }

            int mapLength = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(fieldCategory.Length)));
            Map = new int[mapLength, mapLength, 3];
            int k = 0;
            for (int i = 0; i < mapLength; i++)
                for (int j = 0; j < mapLength; j++,k++)
                {
                    Map[i, j, 0] = int.Parse(fieldCategory[k]);
                    Map[i, j, 1] = int.Parse(fieldType[k]);
                    Map[i, j, 2] = int.Parse(fieldDuration[k]);
                }
        }
        public void Delete() { XF.DeleteSave(ID); }
    }
}
