using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private static readonly string fields_path = loc + "fields.xml";
        private static readonly string graphics_path = loc + "graphics.xml";
        private static readonly string colors_path = loc + "colors.xml";

        public static string GetString(string Name)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(string_path);
            XmlNode node = doc.SelectSingleNode("/StrigsCollection/String[@name='"+Name+"']");
            string text = "- BRAK STRINGA -";
            if (node == null) { return text; }
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
        public static string[] GetGraphic(string GraphicName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(graphics_path);
            XmlNodeList list = doc.SelectNodes("/graphics/graphic[@id='" + GraphicName + "']");
            XmlNode node = list[0];
            if (list.Count < 1) { return new string[] { "- BRAK GRAFIKI -" }; }

            char[] MyChar = { '\r', '\n' };
            string[] oryginalgraphic = node.InnerText.Split('@');
            string[] graphic = new string[oryginalgraphic.Length - 1];
            for (int i = 0; i < graphic.Length; i++)
                graphic[i] = oryginalgraphic[i].Trim(MyChar)[4..];

            return graphic;
        }
        public static List<ProductModel> GetProducts()
        {
            XmlDocument doc = new XmlDocument();
            List<string> Attributes;
            doc.Load(fields_path);
            List<ProductModel> Products = new List<ProductModel>();
            XmlNodeList Categories = doc.SelectNodes("/FieldsCollection/category");
            for (int Category = 0; Category < Categories.Count; Category++)
            {
                XmlNodeList Scales = Categories[Category].ChildNodes;
                XmlNode CurrentCategory = Categories[Category];

                Attributes = new List<string>();
                foreach (XmlAttribute Attribute in CurrentCategory.Attributes) Attributes.Add(Attribute.Name);

                string[] MainMenuAct = Attributes.Contains("menuAct") ? CurrentCategory.Attributes["menuAct"].Value.Split(',') : null;
                string[] MainMapAct = Attributes.Contains("mapAct") ? CurrentCategory.Attributes["mapAct"].Value.Split(',') : null;
                for (int Scale = 0; Scale < Scales.Count; Scale++)
                {
                    XmlNodeList Fields = Scales[Scale].ChildNodes;
                    for (int Type = 0; Type < Fields.Count; Type++)
                    {
                        XmlNode Field = Fields[Type];

                        Attributes = new List<string>();
                        foreach (XmlAttribute Attribute in Field.Attributes) Attributes.Add(Attribute.Name);

                        string[] States = Attributes.Contains("state") ? Field.Attributes["state"].Value.Split('/') : new string[] { "" };
                        string[] Colors = Attributes.Contains("colors") ? Field.Attributes["colors"].Value.Split('/') : new string[] { "0" };
                        string[] MenuActs = Attributes.Contains("menuAct") ? Field.Attributes["menuAct"].Value.Split('/') : null;
                        string[] MapActs = Attributes.Contains("mapAct") ? Field.Attributes["mapAct"].Value.Split('/') : null;
                        string[] View = Field.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "").Split('@');
                        string Price = Attributes.Contains("price") ? Field.Attributes["price"].Value : "0";
                        string Property = Attributes.Contains("property") && Field.Attributes["property"].Value != "" ? Field.Attributes["property"].Value : "0003";

                        for (int State = 0; State < States.Length; State++)
                        {
                            ProductModel Product = new ProductModel()
                            {
                                Category = Category,
                                Scale = Scale,
                                Type = Type,
                                State = State,
                                StateName = States[State],
                                ProductName = Field.Attributes["name"].Value,
                                Price = Convert.ToDecimal(Price),
                                Property = Property,
                                MenuActions = MenuActs == null ? new string[] { } : MenuActs[State < MenuActs.Length ? State : 0].Split(','),
                                MapActions = MapActs == null ? new string[] { } : MapActs[State < MapActs.Length ? State : 0].Split(','),
                                Color = State < Colors.Length ?
                                    IsNumber(Colors[State]) ? ColorService.GetColorByName(Colors[State]) : ColorService.GetColorByID(Convert.ToInt16(Colors[State])) :
                                    IsNumber(Colors[0]) ? ColorService.GetColorByName(Colors[0]) : ColorService.GetColorByID(Convert.ToInt16(Colors[0]))
                            };

                            Product.MenuActions = MainMenuAct == null ? Product.MenuActions : Product.MenuActions.Concat(MainMenuAct).ToArray();
                            Product.MapActions = MainMapAct == null ? Product.MapActions : Product.MapActions.Concat(MainMapAct).ToArray();
                            Product.View = new string[(View.Length - 1) / States.Length];

                            int Line = State, Index = 0;
                            while (Line < View.Length - 1) { Product.View[Index++] = View[Line]; Line += States.Length; }

                            Products.Add(Product);
                        }
                    }
                }
            }
            return Products;
        }
        public static string[] GetColors()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(colors_path);
            XmlNodeList colors = doc.SelectNodes("/colors/color");
            int index = 0;
            string[] ColorsString = new string[colors.Count];
            foreach(XmlNode color in colors)
                ColorsString[index++] =
                    color.Attributes["id"].Value + " " +
                    color.Attributes["name"].Value + " " +
                    color.Attributes["value"].Value;
            return ColorsString;
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

        public static GameInstanceModel[] GetGameInstances()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(saves_path);
            XmlNodeList list = doc.SelectNodes("/Saves/save");
            GameInstanceModel[] saves = new GameInstanceModel[list.Count];
            for (int i = 0; i < list.Count; i++)
                saves[i] = new GameInstanceModel(
                    list[i].Attributes["id"].Value, list[i]["lvl"].InnerText, list[i]["username"].InnerText,
                    list[i]["lastplay"].InnerText, list[i]["wallet"].InnerText);
            return saves;
        }
        public static XmlNode GetGameInstance(int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(saves_path);
            return doc.SelectNodes("/Saves/save[@id=" + id.ToString() + "]")[0];
        }
        public static void UpdateGameInstance(string[] list, int id)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(saves_path);
            XmlNode node = doc.SelectSingleNode("/Saves/save[@id=" + id.ToString() + "]");
            for (int i = 0; i < list.Length; i += 2) node[list[i]].InnerText = list[i + 1];
            doc.Save(saves_path);
        }
        public static void AddGameInstance(string[] list)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(saves_path);
            int lastId = 0;
            if (doc.SelectSingleNode("/Saves").InnerText != "")
                lastId = XElement.Load(saves_path).Descendants("save").Select(x => int.Parse(x.Attribute("id").Value)).Last();
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
        public static void DeleteGameInstance(int id)
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
            name.InnerText = _product.ProductName;
            price.InnerText = _product.Price.ToString();
            product.AppendChild(name);
            product.AppendChild(price);
            root.AppendChild(product);
            doc.Save(product_path);
        }

        private static bool IsNumber(string Text)
        {
            Regex reg = new Regex("[^0-9]");
            return reg.IsMatch(Text);
        }
    }
}

