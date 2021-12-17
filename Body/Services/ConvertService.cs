using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Services
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
            Result = Symbols[(Value / SymbolsLength / SymbolsLength) % SymbolsLength].ToString();
            Result += Symbols[(Value / SymbolsLength) % SymbolsLength].ToString();
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
            Result = Symbols[(Value / SymbolsLength) % SymbolsLength].ToString();
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
                (Value / MaxAmount / MaxType / MaxProductState / MaxScale ) + 2,
                (Value / MaxAmount / MaxType / MaxProductState ) % MaxScale,
                (Value / MaxAmount / MaxType ) % MaxProductState,
                (Value / MaxAmount) % MaxType,
                Value % MaxAmount
            };
        }
        public static int[] ConvertStringToField(string FieldString)
        {
            int Power = 1;
            int Value = 0;
            for (int i = FieldString.Length - 1; i >= 0; i--)
            {
                Value += GetIndexOfSymbol(FieldString[i]) * Power;
                Power *= SymbolsLength;
            }
            return new int[4]
            {
                (Value / MaxDuration / MaxType / MaxFieldState) + 1,
                (Value / MaxDuration / MaxType ) % MaxFieldState,
                (Value / MaxDuration) % MaxType,
                Value % MaxDuration
            };
        }


    }
}
