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
        public int id { get; set; }
        public int lvl { get; set; }
        public string name { get; set; }
        public string lastplay { get; set; }
        public decimal wallet { get; set; }

        private Point[,] map;
        public Point[,] GetMap() { return map; }
        public void SetMap(Point[,] map) { this.map = map; }

        public Save(string id, string lvl, string name, string lastplay, string wallet)
        {
            this.id = Convert.ToInt32(id);
            this.lvl = Convert.ToInt32(lvl);
            this.name = name;
            this.lastplay = lastplay;
            this.wallet = Convert.ToDecimal(wallet);
        } // gost constructor
        public Save(string name) 
        {
            this.id = 0;
            this.lvl = 0;
            this.name = name;
            this.wallet = 420.0M;
            this.map = new Point[4, 4];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    map[i, j] = new Point(10,0);
            map[0, 0] = new Point(11, 0);
            map[2, 0] = new Point(12, 0);
            map[3, 0] = new Point(12, 0);
            map[0, 2] = new Point(14, 0);

        } // new constructor
        public Save() {} // empty constructor

        public void update(int _id)
        {
            string fieldType = "";
            string fieldDuration = "";

            int mapLength = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(map.Length)));
            for (int i = 0; i < mapLength; i++)
                for (int j = 0; j < mapLength; j++)
                {
                    fieldType = fieldType + map[i, j].X.ToString() + " ";
                    fieldDuration = fieldDuration + map[i, j].Y.ToString() + " ";
                }

            string[] list = new string[]
            {
                "lvl", lvl.ToString(),
                "name", name,
                "lastplay", lastplay,
                "wallet", wallet.ToString(),
                "field-type", fieldType,
                "field-duration", fieldDuration
            };
            if(_id == 0) XF.AddSave(list);
            else XF.UpdateSave(list, _id);
        }
        public void load(int _id)
        {
            XmlNode save = XF.GetSave(_id);
            id = int.Parse(save.Attributes["id"].Value);
            lvl = int.Parse(save["lvl"].InnerText);
            name = save["name"].InnerText;
            lastplay = save["lastplay"].InnerText;
            wallet = decimal.Parse(save["wallet"].InnerText);

            string[] type = Regex.Replace(save["field-type"].InnerText, @"\s+", " ", RegexOptions.Multiline).Split(' ');
            string[] duration = Regex.Replace(save["field-duration"].InnerText, @"\s+", " ", RegexOptions.Multiline).Split(' ');

            int mapLength = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(type.Length)));
            map = new Point[mapLength, mapLength];
            int k = 0;
            for (int i = 0; i < mapLength; i++)
                for (int j = 0; j < mapLength; j++,k++)
                    map[i, j] = new Point(int.Parse(type[k]), int.Parse(duration[k]));
        }
        public void delete() { XF.DeleteSave(id); }
    }
}
