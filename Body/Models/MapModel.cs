using FarmConsole.Body.Engines;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Models
{
    public class MapModel : MapEngine
    {
        public string Name;
        public int Size;
        public int Scale;
        public Point MapPosition;
        public Point StandPosition;
        public Size WindowSize;
        public FieldModel[,] Fields;
        public FieldModel BaseField;

        private MapModel() { }
        public MapModel Copy()
        {
            MapModel map = new MapModel
            {
                Name = Name.ToString(),
                Size = Size,
                Scale = Scale,
                WindowSize = new Size(WindowSize.Width, WindowSize.Height),
                MapPosition = new Point(MapPosition.X, MapPosition.Y),
                StandPosition = new Point(StandPosition.X, StandPosition.Y),
                BaseField = new FieldModel(BaseField),
                Fields = new FieldModel[Size, Size]
            };
            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++)
                    map.Fields[x, y] = new FieldModel(Fields[x, y]);
            return map;
        }
        public MapModel(string _MapString)
        {
            string[] _MapStringSplit = _MapString.Split(' ');
            Name = _MapStringSplit[0];
            Size = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(_MapStringSplit[^1].Length / 2))) + 1;
            BaseField = new FieldModel(new ProductModel(_MapStringSplit[^2]));
            Scale = BaseField.Scale;
            Fields = new FieldModel[Size, Size];
            WindowSize = new Size(int.Parse(_MapStringSplit[1]), int.Parse(_MapStringSplit[2]));
            MapPosition = new Point(int.Parse(_MapStringSplit[3]), int.Parse(_MapStringSplit[4]));
            StandPosition = new Point(int.Parse(_MapStringSplit[5]), int.Parse(_MapStringSplit[6]));

            for (int x = 0; x < Size; x++) Fields[x, 0] = new FieldModel(ProductModel.GetProduct("eraser"));
            for (int y = 0; y < Size; y++) Fields[0, y] = new FieldModel(ProductModel.GetProduct("eraser"));
            for (int x = 0; x < Size - 1; x++)
                for (int y = 0; y < Size - 1; y++)
                    Fields[x + 1, y + 1] = new FieldModel(_MapStringSplit[^1].Substring(x * (Size - 1) * 2 + y * 2, 2), Scale);
        }
        public MapModel(string _MapName, string _BaseField, Point _StandPosition, int _Size)
        {
            Name = _MapName;
            Size = _Size + 1;
            BaseField = new FieldModel(ProductModel.GetProduct(_BaseField));
            Scale = BaseField.Scale;
            Fields = new FieldModel[Size, Size];
            MapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 - Size * 3 + 1);
            WindowSize = new Size(Console.WindowWidth, Console.WindowHeight);
            Point ExpectedPos = new Point
            {
                X = (_StandPosition.X > 0 ? _StandPosition.X : Size + _StandPosition.X) - 1,
                Y = (_StandPosition.Y > 0 ? _StandPosition.Y : Size + _StandPosition.Y) - 1
            };

            int VisualMapSize = ((Console.WindowHeight + 1) / 12 + (Console.WindowWidth + 45) / 50) * 2 + 1;
            Point VisualMapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 + 4 - VisualMapSize / 2 * 6);
            StandPosition = new Point(VisualMapSize / 2 - (_StandPosition.X % 2), VisualMapSize / 2 - (_StandPosition.Y % 2));
            Point CenterPos = GetPosByCoord(GetCoordByPos(StandPosition, VisualMapPosition), MapPosition);
            MapPosition = GetCoordByPos(new Point(CenterPos.X - ExpectedPos.X, CenterPos.Y - ExpectedPos.Y), MapPosition);

            for (int x = 0; x < Size; x++) Fields[x, 0] = new FieldModel(ProductModel.GetProduct("eraser"));
            for (int y = 0; y < Size; y++) Fields[0, y] = new FieldModel(ProductModel.GetProduct("eraser"));
            for (int x = 0; x < Size - 1; x++)
                for (int y = 0; y < Size - 1; y++)
                    Fields[x + 1, y + 1] = new FieldModel(ProductModel.GetProduct(_BaseField));
        }
        public void Reload()
        {
            int VisualMapSize = ((WindowSize.Height + 1) / 12 + (WindowSize.Width + 45) / 50) * 2 + 1;
            Point VisualMapPosition = new Point(WindowSize.Width / 2 - 12, (WindowSize.Height - 8) / 2 + 4 - VisualMapSize / 2 * 6);
            Point ExpectedPos = GetPosByCoord(GetCoordByPos(StandPosition, VisualMapPosition), MapPosition);

            WindowSize = new Size(Console.WindowWidth, Console.WindowHeight);
            MapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 - Size * 3 + 1);

            VisualMapSize = ((Console.WindowHeight + 1) / 12 + (Console.WindowWidth + 45) / 50) * 2 + 1;
            VisualMapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 + 4 - VisualMapSize / 2 * 6);
            StandPosition = new Point(VisualMapSize / 2 + (ExpectedPos.X % 2), VisualMapSize / 2 + (ExpectedPos.Y % 2));
            Point CenterPos = GetPosByCoord(GetCoordByPos(StandPosition, VisualMapPosition), MapPosition);
            MapPosition = GetCoordByPos(new Point(CenterPos.X - ExpectedPos.X, CenterPos.Y - ExpectedPos.Y), MapPosition);
        }

        public override string ToString()
        {
            string[] FieldStrings = new string[(Size - 1) * (Size - 1) + 1];
            FieldStrings[0] = new ProductModel(BaseField).ToString() + " ";
            for (int x = 1; x < Size; x++)
                for (int y = 1; y < Size; y++)
                    FieldStrings[1 + (x - 1) * (Size - 1) + (y - 1)] = Fields[x, y].ToString();
            string WinSizeString =  WindowSize.Width + " " + WindowSize.Height + " ";
            string MapPosString =  MapPosition.X + " " + MapPosition.Y + " ";
            string StandPosString =  StandPosition.X + " " + StandPosition.Y + " ";
            return Name + ";" + WinSizeString + MapPosString + StandPosString + String.Join("", FieldStrings);
        }
    }
}
