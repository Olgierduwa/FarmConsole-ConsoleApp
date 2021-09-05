using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace FarmConsole.Body.Services
{
    public static class XF
    {
        private static readonly string loc = "../../../Body/Resources/Database/";

        private static readonly string string_path = loc + "strings.xml";
        private static readonly string product_path = loc + "products.xml";
        private static readonly string long_path = loc + "longtexts.xml";
        private static readonly string options_path = loc + "options.xml";
        private static readonly string saves_path = loc + "saves.xml";
        private static readonly string fields_save = loc + "fields.xml";
        private static readonly string graphics_save = loc + "graphics.xml";

        public static string GetString(int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(string_path);
            XmlNodeList list = doc.SelectNodes("/StrigsCollection/String[@id=" + id.ToString() + "]");
            XmlNode node = list[0];
            string text = "- BRAK STRINGA -";
            if (list.Count < 1) { return text; }
            text = Regex.Replace(node.InnerText, @"\s+", " ", RegexOptions.Multiline);
            return text;
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
        public static string[] GetGraphic(int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(graphics_save);
            XmlNodeList list = doc.SelectNodes("/graphics/graphic[@id=" + id.ToString() + "]");
            XmlNode node = list[0];
            if (list.Count < 1) { return new string[] { "- BRAK GRAFIKI -" }; }

            char[] MyChar = { '\r', '\n' };
            string[] graphic = node.InnerText.Split('@');
            for (int i = 0; i < graphic.Length; i++)
                graphic[i] = graphic[i].Trim(MyChar).Substring(4);

            return graphic;
        }
        public static List<string>[] GetFields()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fields_save);
            XmlNodeList listOfCategory = doc.SelectNodes("/FieldsCollection/category");
            List<string>[] fields = new List<string>[listOfCategory.Count];
            for (int i = 0; i < listOfCategory.Count; i++)
            {
                fields[i] = new List<string>();
                XmlNode nodeOfCategory = listOfCategory[i];
                for (int j = 0; j < nodeOfCategory.ChildNodes.Count; j++)
                {
                    XmlNodeList nodeOfField = nodeOfCategory.SelectNodes("field[@id=" + j.ToString() + "]");
                    XmlNode node = nodeOfField[0];
                    fields[i].Add(node.InnerText);
                }
            }
            return fields;
        }
        public static string GetFieldName(int cat, int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fields_save);
            XmlNodeList category = doc.SelectNodes("/FieldsCollection/category[@cat=" + cat.ToString() + "]");
            XmlNode nodeCategory = category[0];
            XmlNodeList fields = nodeCategory.SelectNodes("field[@id=" + id.ToString() + "]");
            XmlNode nodeField = fields[0];
            return nodeField.Attributes["name"].Value;
        }
        public static string GetFieldDescription(int cat, int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fields_save);
            XmlNodeList category = doc.SelectNodes("/FieldsCollection/category[@cat=" + cat.ToString() + "]");
            XmlNode nodeCategory = category[0];
            XmlNodeList fields = nodeCategory.SelectNodes("field[@id=" + id.ToString() + "]");
            XmlNode nodeField = fields[0];
            return nodeField.Attributes["opis"].Value;
        }

        public static int GetOptionsCount()
        {
            return XElement.Load(options_path).Descendants("option").Select(x => int.Parse(x.Attribute("id").Value)).Last() + 1;
        }
        public static int[] GetOptions()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(options_path);
            XmlNodeList list = doc.SelectNodes("/OptionsCollection/option");
            int[] opt = new int[list.Count];
            for (int i = 0; i < list.Count; i++)
                opt[i] = int.Parse(list[i]["current"].InnerText);
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
            else for (int i = 0; i < list.Count; i++) list[i]["current"].InnerText = list[i]["default"].InnerText;
            doc.Save(options_path);
        }
        
        public static ProductModel GetProduct(int cat, int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(product_path);
            XmlNodeList category = doc.SelectNodes("/ProductsCollection/category[@cat=" + cat.ToString() + "]");
            XmlNode nodeCategory = category[0];
            XmlNodeList products = nodeCategory.SelectNodes("product[@id=" + id.ToString() + "]");
            XmlNode nodeProduct = products[0];
            ProductModel p = new ProductModel();
            if (products.Count < 1) { return p; }
            p.category = cat;
            p.type = id;
            p.name = nodeProduct["name"].InnerText;
            p.price = decimal.Parse(nodeProduct["price"].InnerText);
            return p;
        }
        public static string[] GetProductCategoryName()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(product_path);
            XmlNodeList listOfCategory = doc.SelectNodes("/ProductsCollection/category");
            string[] categoryList = new string[listOfCategory.Count];
            for (int i = 0; i < listOfCategory.Count; i++)
            {
                XmlNode node = listOfCategory[i];
                categoryList[i] = node.Attributes["name"].Value;
            }
            return categoryList;
        }
        public static void AddProduct(ProductModel _product)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(product_path);
            var lastId = XElement.Load(product_path).Descendants("product").Select(x => int.Parse(x.Attribute("id").Value)).Last();
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

        public static SaveModel[] GetSaves()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(saves_path);
            XmlNodeList list = doc.SelectNodes("/Saves/save");
            SaveModel[] saves = new SaveModel[list.Count];
            for (int i = 0; i < list.Count; i++)
                saves[i] = new SaveModel(
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
            for (int i = 0; i < list.Length; i += 2) node[list[i]].InnerText = list[i + 1];
            doc.Save(saves_path);
        }
        public static void AddSave(string[] list)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(saves_path);
            var lastId = XElement.Load(saves_path).Descendants("save").Select(x => int.Parse(x.Attribute("id").Value)).Last();
            XmlNode root = doc.DocumentElement;
            XmlElement save = doc.CreateElement("save");
            save.SetAttribute("id", (lastId + 1).ToString());
            int i = 0, j = 1;
            for (; i < list.Length; i += 2, j++)
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
            var lastID = XElement.Load(saves_path).Descendants("save").Select(x => int.Parse(x.Attribute("id").Value)).Last();
            XmlNode node = doc.SelectSingleNode("/Saves/save[@id=" + id.ToString() + "]");
            node.ParentNode.RemoveChild(node);
            for (int i = id + 1; i <= lastID; i++)
                doc.SelectSingleNode("/Saves/save[@id=" + i.ToString() + "]").Attributes["id"].Value = (i - 1).ToString();
            doc.Save(saves_path);
        }
    }
}

