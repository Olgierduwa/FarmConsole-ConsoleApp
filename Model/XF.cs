using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace FarmConsole.Model
{
    public static class XF
    {
        private static string string_path = "../../../Data/strings.xml";
        private static string product_path = "../../../Data/products.xml";
        private static string long_path = "../../../Data/longtexts.xml";
        private static string options_path = "../../../Data/options.xml";

        public static string findString(int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(string_path);
            XmlNodeList lista = doc.GetElementsByTagName("String");
            if (id > lista.Count) { return ("- BRAK ID W STRING.XML -"); }
            return lista[id].InnerText.Trim().ToString();
        }
        public static int[] getOption()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(options_path);
            XmlNodeList lista = doc.GetElementsByTagName("Option");
            int[] opt = new int[lista.Count];
            for (int i = 0; i < lista.Count; i++) opt[i] = Int32.Parse(lista[i].InnerText.ToString());
            return opt;
        }
        public static void saveOptions(int[] opt)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode lista = doc.CreateElement("OptionsCollection");
            doc.AppendChild(lista);
            XmlNode opcja;
            for (int i = 0; i < opt.Length; i++)
            {
                opcja = doc.CreateElement("Option");
                opcja.InnerText = opt[i].ToString();
                lista.AppendChild(opcja);
            }
            doc.Save(options_path);

        }
        public static Product findProduct(int id)
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
        public static string findText(int id)
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
        public static void addProduct(Product _product)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(product_path);
            XElement store = XElement.Load(product_path);
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
    }
}

