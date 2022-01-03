using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;

namespace FarmConsole.Body.Services
{
	public static class StringService
	{
		public static Dictionary<string, string> Strings;
		public static string Get(string Key, string After = "", string Before = "")
		{
			string Value = Strings.ContainsKey(Key) ? Strings[Key] : "UNKNOWN STRING";
			return Before + Value + After;
		}
		public static void SetStrings()
		{
			string language = SettingsService.LanguageKey;
			Strings = new Dictionary<string, string>();
			ResourceManager resmanager = new ResourceManager("FarmConsole.Body.Resources.Strings.Strings", typeof(StringService).Assembly);
			ResourceSet resourceSet = resmanager.GetResourceSet(new CultureInfo(language), true, true);
			foreach (DictionaryEntry entry in resourceSet)
				Strings.Add(entry.Key.ToString(), entry.Value.ToString());
		}
    }
}
