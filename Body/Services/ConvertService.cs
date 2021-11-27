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
        private static readonly int MaxState = 12;
        private static readonly int MaxProductType = 340;
        private static readonly int MaxFieldType = 100;
        private static readonly int MaxAmmount = 100000;
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

        public static string ConvertProductToString(int category, int scale, int producttype, int ammount)
        {
            int Rest, Value = ((category * MaxScale + scale) * MaxProductType + producttype) * MaxAmmount + ammount;
            string Result = "";
            while (Value > 0)
            {
                Rest = Value % SymbolsLength;
                Result = Result.Insert(0, Symbols[Rest].ToString());
                Value /= SymbolsLength;
            }
            return Result;
        }
        public static string ConvertFieldToString(int category, int state, int fieldtype, int duration)
        {
            int Value = ((category * MaxState + state) * MaxFieldType + fieldtype) * MaxDuration + duration;
            return Symbols[(Value / SymbolsLength) % SymbolsLength].ToString() + Symbols[Value % SymbolsLength].ToString();
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
                (Value / MaxAmmount / MaxProductType / MaxScale),
                (Value / MaxAmmount / MaxProductType ) % MaxScale,
                (Value / MaxAmmount) % MaxProductType,
                Value % MaxAmmount
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
                (Value / MaxDuration / MaxFieldType / MaxState),
                (Value / MaxDuration / MaxFieldType ) % MaxState,
                (Value / MaxDuration) % MaxFieldType,
                Value % MaxDuration
            };
        }

        public static bool DirectionalAccess(Point point)
        {
            switch(point.X)
            {
                case 1:return true;
            }
            return true;
        }

    }
}
