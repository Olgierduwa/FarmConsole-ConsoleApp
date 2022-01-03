using FarmConsole.Body.Controlers;
using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FarmConsole.Body.Services
{
    public static class SettingsService
    {
        public static bool GODMOD = true;

        public static string LanguageKey { get; set; }
        public static Dictionary<string, string> Languages { get; set; }
        public static string GetLanguage()
        {
            return Languages[LanguageKey];
        }
        public static void SetLanguage(int value)
        {
            List<string> Keys = new List<string>();
            foreach (var lan in Languages) Keys.Add(lan.Key);
            int index = 1;
            while (index < Keys.Count && Keys[index] != LanguageKey) index++;
            index += value;
            if (index == Keys.Count) index = 1;
            if (index == 0) index = Keys.Count - 1;
            LanguageKey = Keys[index];
        }
        public static void LoadLanguages()
        {
            Languages = XF.GetLanguages();
            LanguageKey = Languages["current"];
            StringService.SetStrings();
            foreach (var s in Settings) s.Name = StringService.Get(s.Key);
        }

        private static List<SettingModel> Settings { get; set; }
        public static List<SettingModel> GetSettings => Settings;
        public static SettingModel GetSetting(string key)
        {
            foreach (var s in Settings) if (s.Key == key) return s;
            return null;
        }
        public static void LoadSettings()
        {
            Settings = XF.GetSettings();
            ConvertService.SetSymbols();
            MainController.FieldNameVisibility = Convert.ToBoolean(GetSetting("set field name").GetRealValue);
            MainController.PlayerMovementAxis = Convert.ToBoolean(GetSetting("set player move axis").GetRealValue);
            MainController.MapMovementAxis = Convert.ToBoolean(GetSetting("set map move axis").GetRealValue);
            LoadLanguages();
        }
        public static void SaveSettings()
        {
            XF.UpdateLanguage(LanguageKey);
            XF.UpdateSettings(Settings);
        }
        public static void RestoreDefaultSettings()
        {
            foreach(var setting in Settings) setting.SetDefaultValue();
            SaveSettings();
        }
    }
}
