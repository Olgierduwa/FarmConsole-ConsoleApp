using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Models
{
    public class MapModel
    {
        public string Name;
        public int Size;
        public int Scale;
        public Point Position;
        public FieldModel[,] Fields;
        public FieldModel BaseField;

        public MapModel(MapModel map)
        {
            Name = map.Name;
            Size = map.Size;
            Position = map.Position;
            Fields = map.Fields;
            BaseField = map.BaseField;
        }

        public MapModel(string _MapName, string _MapString, int _Size = 0)
        {
            Name = _MapName;
            Size = _Size > 0 ? _Size + 1 : Convert.ToInt32(Math.Sqrt(Convert.ToDouble(_MapString.Length / 5 - 1))) + 1;
            BaseField = _Size > 0 ? new FieldModel(ProductModel.GetProduct(_MapString)) : new FieldModel(new ProductModel(_MapString.Substring(0, 5)));
            Scale = BaseField.Scale;
            Position = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 - Size * 3 + 1);
            Fields = new FieldModel[Size, Size];

            for (int x = 0; x < Size; x++) Fields[x, 0] = new FieldModel(ProductModel.GetProduct("rubber"));
            for (int y = 0; y < Size; y++) Fields[0, y] = new FieldModel(ProductModel.GetProduct("rubber"));
            for (int x = 0; x < Size - 1; x++)
                for (int y = 0; y < Size - 1; y++)
                    if (_Size == 0) Fields[x + 1, y + 1] = new FieldModel(_MapString.Substring(5 + x * (Size-1) * 5 + y * 5, 5), Scale);
                    else Fields[x + 1, y + 1] = new FieldModel(ProductModel.GetProduct(_MapString));
        }

        public override string ToString()
        {
            string MapString = BaseField.ToString();
            for (int x = 1; x < Size; x++)
                for (int y = 1; y < Size; y++)
                    MapString += Fields[x, y].ToString();
            return MapString;
        }
    }
}
