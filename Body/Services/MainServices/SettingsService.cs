using FarmConsole.Body.Controllers.CentralControllers;
using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FarmConsole.Body.Services.MainServices
{
    public static class SettingsService
    {
        public static bool GODMOD = false;
        public static string[] BLOCKEDACTIONS_LVL2 = new string[] { "hide", "destroy", "build", "dig path", "plow" };
        public static string[] BLOCKEDACTIONS_LVL1 = new string[] { "rotate", "move", "lock", "unlock" };

        public static int MaxWindowWidth { get; set; }
        public static int MaxWindowHeihgt { get; set; }
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
            LS.SetStrings();
            foreach (var s in Settings) s.Name = LS.Navigation(s.Key);
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
            MaxWindowHeihgt = Console.LargestWindowHeight;
            MaxWindowWidth = Console.LargestWindowWidth;
            Settings = XF.GetSettings();
            ConvertService.SetSymbols();
            ColorService.SetColorPalette();
            HeadController.FieldNameVisibility = Convert.ToBoolean(GetSetting("set field name").GetRealValue);
            HeadController.StatsVisibility = Convert.ToBoolean(GetSetting("set stats").GetRealValue);
            HeadController.PlayerMovementAxis = Convert.ToBoolean(GetSetting("set player move axis").GetRealValue);
            HeadController.MapMovementAxis = Convert.ToBoolean(GetSetting("set map move axis").GetRealValue);
            LoadLanguages();
        }
        public static void SaveSettings()
        {
            XF.UpdateLanguage(LanguageKey);
            XF.UpdateSettings(Settings);
        }
        public static void RestoreDefaultSettings()
        {
            foreach (var setting in Settings) setting.SetDefaultValue();
            SaveSettings();
        }
    }
}
