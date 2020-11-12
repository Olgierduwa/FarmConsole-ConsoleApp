using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace FarmConsole.Model
{
    public class Save
    {
        public int id { get; set; }
        public int lvl { get; set; }
        public string name { get; set; }
        public string lastplay { get; set; }
        public decimal wallet { get; set; }

        public Save(string id, string lvl, string name, string lastplay, string wallet)
        {
            this.id = Convert.ToInt32(id);
            this.lvl = Convert.ToInt32(lvl);
            this.name = name;
            this.lastplay = lastplay;
            this.wallet = Convert.ToDecimal(wallet);
        } // gost constructor
        public Save() 
        {
            this.id = 0;
        } // empty constructor
        
        public void update(int _id)
        {
            string[] list = new string[]
            {
                "lvl", lvl.ToString(),
                "name", name,
                "lastplay", lastplay,
                "wallet", wallet.ToString()
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
        }
        public void delete() { XF.DeleteSave(id); }
    }
}
