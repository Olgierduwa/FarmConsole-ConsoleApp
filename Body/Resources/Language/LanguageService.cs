using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace FarmConsole.Body.Services.MainServices
{
    public static class LS // LanguageService
    {
        private static Dictionary<string, string> _navigation { get; set; }
        private static Dictionary<string, string> _actions { get; set; }
        private static Dictionary<string, string> _objects { get; set; }
        private static Dictionary<string, string> _texts { get; set; }
        private static string LanguageKey { get; set; }

        public static string Navigation(string Key, string After = "", string Before = "")
            => Before + (_navigation.ContainsKey(Key) ? _navigation[Key] : "UNKNOWN NAVIGATION") + After;
        public static string Action(string Key, string After = "", string Before = "")
            => Before + (_actions.ContainsKey(Key) ? _actions[Key] : "UNKNOWN ACTION") + After;
        public static string Object(string Key, string After = "", string Before = "")
            => Before + (_objects.ContainsKey(Key) ? _objects[Key] : "UNKNOWN OBJECT") + After;
        public static string Text(string Key, string After = "", string Before = "")
            => Before + (_texts.ContainsKey(Key) ? _texts[Key] : "UNKNOWN TEXT") + After;

        public static void SetStrings()
        {
            LanguageKey = SettingsService.LanguageKey;
            _navigation = new Dictionary<string, string>();
            _actions = new Dictionary<string, string>();
            _objects = new Dictionary<string, string>();

            Load(_navigation, "Navigation");
            Load(_actions, "Actions");
            Load(_objects, "Objects");

            _texts = new Dictionary<string, string>();
            XmlDocument doc = new XmlDocument();
            doc.Load("../../../Body/Resources/Language/" + LanguageKey + "/Texts.xml");
            foreach (XmlNode node in doc.SelectNodes("/texts/text"))
            {
                string text = ConvertService.RemoveSpacesAfterEndline(node.InnerText.Replace("\r", ""));
                _texts.Add(node.Attributes["key"].Value, text);
            }
        }
        private static void Load(Dictionary<string, string> dictionary, string name)
        {
            ResourceManager resmanager = new ResourceManager("FarmConsole.Body.Resources.Language." + LanguageKey + "." + name, typeof(LS).Assembly);
            ResourceSet resourceSet = resmanager.GetResourceSet(new CultureInfo(LanguageKey), true, true);
            foreach (DictionaryEntry entry in resourceSet)
                dictionary.Add(entry.Key.ToString(), entry.Value.ToString());
        }
    }
}
