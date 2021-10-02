using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Services
{
    public static class ConvertService
    {
        private static char[] GetSymbols()
        {
            char[] Symbols = new char[63];
            for (int i = 0; i < 10; i++) Symbols[i] = (char)(48 + i);
            for (int i = 10; i < 37; i++) Symbols[i] = (char)(54 + i);
            for (int i = 37; i < 63; i++) Symbols[i] = (char)(60 + i);
            return Symbols;
        }
        private static int GetIndexOfSymbol(char Symbol)
        {
            int Index = 0;
            char[] Symbols = GetSymbols();
            while (Symbols[Index] != Symbol) Index++;
            return Index;
        }

        public static string ConvertToSixtyTripleSystem(int Value_1, int Value_2, int Value_3, int Value_4)
        {
            // V1: 0-8 V2: 0-9 V3: 000-999 V4: 0000-9999
            if (Value_1 > 8 || Value_2 > 9 || Value_3 > 999 || Value_4 > 9999) return "LARGE";
            string ValuesString = "";
            ValuesString += Convert.ToString(Value_1).PadLeft(1, '0');
            ValuesString += Convert.ToString(Value_2).PadLeft(1, '0');
            ValuesString += Convert.ToString(Value_3).PadLeft(3, '0');
            ValuesString += Convert.ToString(Value_4).PadLeft(4, '0');
            int Rest, Value = Convert.ToInt32(ValuesString);
            string Result = "";
            char[] Symbols = GetSymbols();
            while (Value > 0)
            {
                Rest = Value % 63;
                Result = Result.Insert(0, Symbols[Rest].ToString());
                Value /= 63;
            }
            return Result;
        }
        public static int[] ConvertToDecimalSystem(string Value)
        {
            char[] Symbols = GetSymbols();
            int Temp = 1;
            int Result = 0;
            for (int i = Value.Length - 1; i >= 0; i--)
            {
                Result += GetIndexOfSymbol(Value[i]) * Temp;
                Temp *= Symbols.Length;
            }
            string ResultString = Result.ToString().PadLeft(9, '0');
            return new int[4]
            {
                Convert.ToInt32(ResultString.Substring(0, 1)), // 0 
                Convert.ToInt32(ResultString.Substring(1, 1)), //  0
                Convert.ToInt32(ResultString.Substring(2, 3)), //   000
                Convert.ToInt32(ResultString.Substring(5, 4))  //      0000
            };
        }
    }
}
