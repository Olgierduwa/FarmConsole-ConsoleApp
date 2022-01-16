using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FarmConsole.Body.Services.MainServices
{
    public static class ConvertService
    {
        // private static int MaxCategory = 5;
        private static readonly int MaxScale = 3;
        private static readonly int MaxFieldState = 12;
        private static readonly int MaxProductState = 3;
        private static readonly int MaxType = 170;
        private static readonly int MaxAmount = 100000;
        private static readonly int MaxDuration = 100;
        private static int SymbolsLength;

        private static int _lvl = 0;
        private static int _experience = 0;
        public static int RequiredExperience(int lvl)
        {
            if (lvl != _lvl)
            {
                _lvl = lvl;
                _experience = Convert.ToInt32(Math.Pow((1 + Math.Sqrt(5)) / 2, lvl)) * 100;
            }
            return _experience;
        }

        private static char[] Symbols;
        public static void SetSymbols()
        {
            Symbols = new char[800];
            SymbolsLength = Symbols.Length;
            for (int i = 0; i < 10; i++) Symbols[i] = (char)(48 + i);
            for (int i = 10; i < 37; i++) Symbols[i] = (char)(54 + i);
            for (int i = 37; i < 63; i++) Symbols[i] = (char)(60 + i);
            for (int i = 63; i < 790; i++) Symbols[i] = (char)(97 + i);
            for (int i = 790; i < 800; i++) Symbols[i] = (char)(120 + i);
        }
        private static int GetIndexOfSymbol(char Symbol)
        {
            int Index = 0;
            while (Symbols[Index] != Symbol) Index++;
            return Index;
        }

        public static string ConvertProductToString(int category, int scale, int state, int type, int amount)
        {
            int Value = category - 2;
            Value = Value * MaxScale + scale;
            Value = Value * MaxProductState + state;
            Value = Value * MaxType + type;
            Value = Value * MaxAmount + amount;

            string Result;
            Result = Symbols[Value / SymbolsLength / SymbolsLength % SymbolsLength].ToString();
            Result += Symbols[Value / SymbolsLength % SymbolsLength].ToString();
            Result += Symbols[Value % SymbolsLength].ToString();
            return Result;
        }
        public static string ConvertFieldToString(int category, int state, int type, int duration)
        {
            int Value = category - 1;
            Value = Value * MaxFieldState + state;
            Value = Value * MaxType + type;
            Value = Value * MaxDuration + duration;

            string Result;
            Result = Symbols[Value / SymbolsLength % SymbolsLength].ToString();
            Result += Symbols[Value % SymbolsLength].ToString();
            return Result;
        }

        public static int[] ConvertStringToProduct(string ProductString)
        {
            int Power = 1;
            int Value = 0;
            for (int i = ProductString.Length - 1; i >= 0; i--)
            {
                Value += GetIndexOfSymbol(ProductString[i]) * Power;
                Power *= SymbolsLength;
            }
            return new int[]
            {
                Value / MaxAmount / MaxType / MaxProductState / MaxScale  + 2,
                Value / MaxAmount / MaxType / MaxProductState  % MaxScale,
                Value / MaxAmount / MaxType  % MaxProductState,
                Value / MaxAmount % MaxType,
                Value % MaxAmount
            };
        }
        public static short[] ConvertStringToField(string FieldString)
        {
            int Power = 1;
            int Value = 0;
            for (int i = FieldString.Length - 1; i >= 0; i--)
            {
                Value += GetIndexOfSymbol(FieldString[i]) * Power;
                Power *= SymbolsLength;
            }
            return new short[4]
            {
                (short)(Value / MaxDuration / MaxType / MaxFieldState + 1),
                (short)(Value / MaxDuration / MaxType % MaxFieldState),
                (short)(Value / MaxDuration % MaxType),
                (short)(Value % MaxDuration)
            };
        }

        public static string[] ConcatActionTables(string[] stateActs, string[] mainActs)
        {
            if (stateActs.Length == 0 || stateActs.Length == 1 && stateActs[0] == "") return mainActs;

            List<string> ForbiddenActs = new List<string>();
            int ActsCount = stateActs.Length + mainActs.Length;
            foreach (string act in stateActs)
                if (act.Length > 0 && act[0] == '-')
                {
                    ActsCount -= 2;
                    ForbiddenActs.Add(act[1..]);
                }
            string[] ActionsTable = new string[ActsCount];
            int index = 0;
            for (int i = 0; i < stateActs.Length; i++)
                if (stateActs[i][0] != '-') ActionsTable[index++] = stateActs[i];

            for (int i = 0; i < mainActs.Length; i++)
                if (!ForbiddenActs.Contains(mainActs[i])) ActionsTable[index++] = mainActs[i];

            return ActionsTable;
        }
        public static string[] SelectAllowedActions(string[] mapActions, byte accesslevel)
        {
            if (SettingsService.GODMOD) return mapActions;
            if (accesslevel == 0) return new string[] { };
            List<string> AllowedActions = new List<string>();
            for (int i = 0; i < mapActions.Length; i++)
            {
                bool Permission = true;
                if (accesslevel < 3 && SettingsService.BLOCKEDACTIONS_LVL2.Contains(mapActions[i])) Permission = false;
                if (accesslevel < 2 && SettingsService.BLOCKEDACTIONS_LVL1.Contains(mapActions[i])) Permission = false;
                if (Permission) AllowedActions.Add(mapActions[i]);
            }
            return AllowedActions.ToArray();
        }
        public static string CamelCase(string text)
        {
            return string.Join("", text.Split().Select(i => char.ToUpper(i[0]) + i.Substring(1)));
        }
        public static string RemoveSpacesAfterEndline(string text)
        {
            for (int i = 0; i < text.Length;)
                if (text[i++] == '\n')
                    while (i < text.Length && text[i] == ' ')
                        text = text.Remove(i, 1);
            return text;
        }
    }
}
