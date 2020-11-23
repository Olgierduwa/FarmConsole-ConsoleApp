using FarmConsole.Body.Model.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace FarmConsole.Body.Model.Helpers
{
    public static class XF
    {
        private static readonly string loc = "../../../Body/Data/";

        private static readonly string string_path = loc + "strings.xml";
        private static readonly string product_path = loc + "products.xml";
        private static readonly string long_path = loc + "longtexts.xml";
        private static readonly string options_path = loc + "options.xml";
        private static readonly string saves_path = loc + "saves.xml";
        private static readonly string fields_save = loc + "fields.xml";

        public static string GetString(int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(string_path);
            XmlNodeList lista = doc.GetElementsByTagName("String");
            if (id > lista.Count) { return ("- BRAK ID W STRING.XML -"); }
            return lista[id].InnerText.Trim().ToString();
        }
        public static string GetText(int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(long_path);
            XmlNodeList list = doc.SelectNodes("/texts/text[@id=" + id.ToString() + "]");
            XmlNode node = list[0];
            string text = "- BRAK TEKSTU -";
            if (list.Count < 1) { return text; }
            text = Regex.Replace(node.InnerText, @"\s+", " ", RegexOptions.Multiline);
            return text;
        }
        public static string[] GetFields()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fields_save);
            XmlNodeList list = doc.SelectNodes("/FieldsCollection/field");
            string[] fields = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
                fields[i] = list[i].InnerText;
            return fields;
        }

        public static int GetOptionsCount()
        {
            return XElement.Load(options_path).Descendants("option").Select(x => Int32.Parse(x.Attribute("id").Value)).Last() + 1;
        }
        public static int[] GetOptions()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(options_path);
            XmlNodeList list = doc.SelectNodes("/OptionsCollection/option");
            int[] opt = new int[list.Count];
            for (int i = 0; i < list.Count; i++)
                opt[i] = Int32.Parse(list[i]["current"].InnerText);
            return opt;
        }
        public static string[] GetOptionsNames()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(options_path);
            XmlNodeList list = doc.SelectNodes("/OptionsCollection/option");
            string[] names = new string[list.Count];
            for (int i = 0; i < list.Count; i++) names[i] = list[i].Attributes["Name"].Value;
            return names;
        }
        public static void UpdateOptions(int[] opt)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(options_path);
            XmlNodeList list = doc.SelectNodes("/OptionsCollection/option");
            if (opt.Length > 0) for (int i = 0; i < list.Count; i++) list[i]["current"].InnerText = opt[i].ToString();
            else                for (int i = 0; i < list.Count; i++) list[i]["current"].InnerText = list[i]["default"].InnerText;
            doc.Save(options_path);
        }

        public static Product GetProduct(int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(product_path);
            XmlNodeList list = doc.SelectNodes("/ProductsCollection/product[@id=" + id.ToString() + "]");
            XmlNode node = list[0];
            Product p = new Product();
            if (list.Count < 1) {return p; }
            p.id = int.Parse(node.Attributes["id"].Value);
            p.name = node["name"].InnerText;
            p.price = decimal.Parse(node["price"].InnerText);
            return p;
        }
        public static void AddProduct(Product _product)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(product_path);
            var lastId = XElement.Load(product_path).Descendants("product").Select(x => Int32.Parse(x.Attribute("id").Value)).Last();
            XmlNode root = doc.DocumentElement;
            XmlElement product = doc.CreateElement("product");
            XmlElement name = doc.CreateElement("name");
            XmlElement price = doc.CreateElement("price");
            product.SetAttribute("id", (lastId + 1).ToString());
            name.InnerText = _product.name;
            price.InnerText = _product.price.ToString();
            product.AppendChild(name);
            product.AppendChild(price);
            root.AppendChild(product);
            doc.Save(product_path);
        }

        public static Save[] GetSaves()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(saves_path);
            XmlNodeList list = doc.SelectNodes("/Saves/save");
            Save[] saves = new Save[list.Count];
            for (int i = 0; i < list.Count; i++)
                saves[i] = new Save(
                    list[i].Attributes["id"].Value, list[i]["lvl"].InnerText, list[i]["name"].InnerText,
                    list[i]["lastplay"].InnerText, list[i]["wallet"].InnerText);
            return saves;
        }
        public static XmlNode GetSave(int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(saves_path);
            return doc.SelectNodes("/Saves/save[@id=" + id.ToString() + "]")[0];
        }
        public static void UpdateSave(string[] list, int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(saves_path);
            XmlNode node = doc.SelectSingleNode("/Saves/save[@id=" + id.ToString() + "]");
            for (int i = 0; i < list.Length; i += 2)  node[list[i]].InnerText = list[i + 1];
            doc.Save(saves_path);
        }
        public static void AddSave(string[] list)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(saves_path);
            var lastId = XElement.Load(saves_path).Descendants("save").Select(x => Int32.Parse(x.Attribute("id").Value)).Last();
            XmlNode root = doc.DocumentElement;
            XmlElement save = doc.CreateElement("save");
            save.SetAttribute("id", (lastId + 1).ToString());
            int i = 0, j = 1;
            for (; i < list.Length; i+=2, j++)
            {
                XmlElement node = doc.CreateElement(list[i]);
                node.InnerText = list[i + 1];
                save.AppendChild(node);
            }
            root.AppendChild(save);
            doc.Save(saves_path);
        }
        public static void DeleteSave(int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(saves_path);
            var lastID = XElement.Load(saves_path).Descendants("save").Select(x => Int32.Parse(x.Attribute("id").Value)).Last();
            XmlNode node = doc.SelectSingleNode("/Saves/save[@id=" + id.ToString() + "]");
            node.ParentNode.RemoveChild(node);
            for (int i = id + 1; i <= lastID; i++)
                doc.SelectSingleNode("/Saves/save[@id=" + i.ToString() + "]").Attributes["id"].Value = (i - 1).ToString();
            doc.Save(saves_path);
        }
    }
}

