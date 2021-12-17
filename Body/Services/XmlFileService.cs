﻿using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace FarmConsole.Body.Services
{
    public static class XF
    {
        private static readonly string loc = "../../../Body/Resources/Database/";
        private static readonly string locsaves = "../../../Body/Resources/Saves/";

        private static readonly string long_path = loc + "LongTexts.xml";
        private static readonly string options_path = loc + "Options.xml";
        private static readonly string objects_path = loc + "Objects.xml";
        private static readonly string graphics_path = loc + "Graphics.xml";
        private static readonly string colors_path = loc + "Colors.xml";
        private static readonly string rules_path = loc + "Rules.xml";

        private static readonly string saves_path = locsaves + "Saves.xml";

        public static List<ObjectModel> GetObjects()
        {
            XmlDocument doc = new XmlDocument();
            List<string> Attributes;
            doc.Load(objects_path);
            List<ObjectModel> Objects = new List<ObjectModel>();
            XmlNodeList Categories = doc.SelectNodes("/ObjectsCollection/category");
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
                        string[] States = Attributes.Contains("state") ? Field.Attributes["state"].Value.Split('/') : new string[] { "*" };
                        string[] Colors = Attributes.Contains("colors") ? Field.Attributes["colors"].Value.Split('/') : new string[] { "0" };
                        string[] MenuActs = Attributes.Contains("menuAct") ? Field.Attributes["menuAct"].Value.Split('/') : null;
                        string[] MapActs = Attributes.Contains("mapAct") ? Field.Attributes["mapAct"].Value.Split('/') : null;
                        string[] StateViewLines = Field.InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "").Split('@');
                        string Property = Attributes.Contains("property") && Field.Attributes["property"].Value != "" ? Field.Attributes["property"].Value : "0003";
                        string Price = Attributes.Contains("price") ? Field.Attributes["price"].Value : "0";
                        bool Cutted = Attributes.Contains("cut") ? true : false;
                        int ViewHeight = (StateViewLines.Length - 1) / States.Length;
                        int ViewWidth = StateViewLines[0].Length;
                        List<int>[] StateLayers = new List<int>[States.Length];
                        List<PixelModel[,]> StateViews = new List<PixelModel[,]>();
                        foreach (var s in States) if (s != "+") StateViews.Add(new PixelModel[ViewWidth, ViewHeight]);
                        for (int i = 0; i < States.Length; i++) StateLayers[i] = new List<int>();

                        // filling state view layers
                        if (Attributes.Contains("layers"))
                            if (Field.Attributes["layers"].Value != "")
                            {
                                string[] LayersForStates = Field.Attributes["layers"].Value.Split('/');
                                for (int Layer = 0; Layer < LayersForStates.Length; Layer++)
                                    foreach (var DirectState in LayersForStates[Layer].Split(','))
                                        StateLayers[Convert.ToInt32(DirectState)].Add(Layer);
                            }

                        // filling state views
                        for (int LineIndex = StateViewLines.Length - 2; LineIndex >= 0; LineIndex--)
                        {
                            int CurrentState = LineIndex % States.Length;
                            int CurrentHeight = LineIndex / States.Length;
                            Color Color = CurrentState < Colors.Length ?
                                IsNumber(Colors[CurrentState]) ? ColorService.GetColorByName(Colors[CurrentState]) :
                                ColorService.GetColorByID(Convert.ToInt16(Colors[CurrentState])) :
                                IsNumber(Colors[0]) ? ColorService.GetColorByName(Colors[0]) :
                                ColorService.GetColorByID(Convert.ToInt16(Colors[0]));
                            if (StateLayers[CurrentState].Count == 0) StateLayers[CurrentState].Add(CurrentState);

                            foreach (int DirectState in StateLayers[CurrentState])
                                for (int CurrentSymbol = 0; CurrentSymbol < ViewWidth; CurrentSymbol++)
                                    if (StateViewLines[LineIndex][CurrentSymbol] != '´')
                                        StateViews[DirectState][CurrentSymbol, CurrentHeight] = new PixelModel()
                                        { Content = StateViewLines[LineIndex][CurrentSymbol].ToString(), Color = Color };
                        }

                        // filling states
                        for (int State = 0; State < StateViews.Count; State++)
                        {
                            ObjectModel Object = new ObjectModel()
                            {
                                ID = Objects.Count(),
                                Category = Category,
                                Scale = Scale,
                                Type = Type,
                                State = State,
                                StateName = States[State],
                                ObjectName = Field.Attributes["name"].Value,
                                MenuActions = MenuActs == null ? new string[] { } : MenuActs[State < MenuActs.Length ? State : 0].Split(','),
                                MapActions = MapActs == null ? new string[] { } : MapActs[State < MapActs.Length ? State : 0].Split(','),
                                Price = Convert.ToDecimal(Price),
                                Property = Property,
                                Cutted = Cutted,
                                View = new ViewModel(StateViews[State], ViewWidth, ViewHeight)
                            };
                            Object.MenuActions = MainMenuAct == null ? Object.MenuActions : Object.MenuActions.Concat(MainMenuAct).ToArray();
                            Object.MapActions = MainMapAct == null ? Object.MapActions : Object.MapActions.Concat(MainMapAct).ToArray();
                            Objects.Add(Object);
                        }
                    }
                }
            }
            return Objects;
        }
        public static List<RuleModel> GetRules()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(rules_path);
            XmlNodeList NodeRules = doc.SelectNodes("/rules/rule");
            List<RuleModel> Rules = new List<RuleModel>();
            foreach (XmlNode NodeRule in NodeRules)
            {
                Rules.Add(new RuleModel {
                    Name = NodeRule.Attributes["name"].Value,
                    CaptureType = NodeRule.Attributes["type"].Value,
                    RequiredLevel = Convert.ToInt32(NodeRule.Attributes["lvl"].Value),
                    IsAllowed = true });
            }
            return Rules;
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
        private static bool IsNumber(string Text)
        {
            Regex reg = new Regex("[^0-9]");
            return reg.IsMatch(Text);
        }

        #region OPTIONS
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
        #endregion

        #region GAME INSTANCE
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
        public static int AddGameInstance(string[] list)
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
            return lastId + 1;
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

            string directory = locsaves + id.ToString().PadLeft(3, '0');
            var directores = Directory.EnumerateDirectories(locsaves);
            int count = directores.Count();
            bool deleted = false;
            foreach (string path in directores)
            {
                int currentID = int.Parse(path.Split('/')[^1]);
                if(currentID == id)
                {
                    Directory.Delete(path, true);
                    deleted = true;
                }
                else if(deleted)
                {
                    Directory.Move(path, locsaves + id.ToString().PadLeft(3, '0'));
                    id++;
                }
            }
        }
        public static void UpdateMaps(string[] MapList, int id)
        {
            string path, directory = locsaves + id.ToString().PadLeft(3, '0');
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            for (int i = 0; i < MapList.Length; i++)
            {
                string[] mapString = MapList[i].Split(';');
                path = directory + "/" + mapString[0] + ".txt";
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(mapString[1]);
                    fs.Write(info, 0, info.Length);
                    fs.Close();
                }
            }
        }
        public static string[] LoadMaps(int id)
        {
            string[] Maps = null;
            string directory = locsaves + id.ToString().PadLeft(3, '0');
            if (Directory.Exists(directory))
            {
                var files = Directory.EnumerateFiles(directory, "*.txt");
                Maps = new string[files.Count()];
                int index = 0;
                foreach (string path in files)
                    using (var file = File.OpenText(path))
                    {
                        Maps[index++] = path.Split('\\')[^1].Split('.')[0] + " " + file.ReadToEnd();
                        file.Close();
                    }
            }
            return Maps;
        }
        #endregion

    }
}

