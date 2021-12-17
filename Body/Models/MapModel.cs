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
        public FieldModel BaseField;
        private FieldModel[,] Fields { get; set; }
        public FieldModel GetField(int x, int y) => Fields[x, y];
        public void SetField(int x, int y, FieldModel Field)
        {
            if(x < Size && y < Size && Field != null)
            {
                if (Field.Cutted == true)
                {
                    if (Fields[x, y] != null)
                        if (Fields[x, y].BaseID > 0)
                        {
                            Field.BaseView = Fields[x, y].BaseView;
                            Field.BaseID = Fields[x, y].BaseID;
                        }
                        else
                        {
                            Field.BaseView = Fields[x, y].View;
                            Field.BaseID = Fields[x, y].ID;
                        }

                    Fields[x, y] = Field;
                }
                else Fields[x, y] = Field;
            }
        }
        public void Reload()
        {
            int VisualMapSize = (WindowSize.Height - 3) / 6 + (WindowSize.Width + 23) / 24 + 3;
            Point VisualMapPosition = new Point(WindowSize.Width / 2 - 12, (WindowSize.Height - 8) / 2 + 4 - VisualMapSize / 2 * 6);
            Point PhisicalPos = GetMatchPoint(StandPosition, VisualMapPosition, MapPosition);

            WindowSize = new Size(Console.WindowWidth, Console.WindowHeight);
            MapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 - Size * 3 + 1);
            VisualMapSize = (Console.WindowHeight - 3) / 6 + (Console.WindowWidth + 23) / 24 + 3;
            VisualMapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 + 4 - VisualMapSize / 2 * 6);

            StandPosition = new Point(VisualMapSize / 2 + (PhisicalPos.X % 2), VisualMapSize / 2 + (PhisicalPos.Y % 2));
            Point CenterPos = GetMatchPoint(StandPosition, VisualMapPosition, MapPosition);
            MapPosition = GetCoordByPos(new Point(CenterPos.X - PhisicalPos.X, CenterPos.Y - PhisicalPos.Y), MapPosition);
        }

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
                BaseField = BaseField.ToField(),
                Fields = new FieldModel[Size, Size]
            };
            for (int x = 0; x < Size; x++)
                for (int y = 0; y < Size; y++)
                    map.SetField(x, y, GetField(x, y).ToField());
            return map;
        }
        public MapModel(string _MapName, string _BaseField, Point _StandPosition, int _Size)
        {
            Name = _MapName;
            Size = _Size + 1;
            BaseField = ObjectModel.GetObject(_BaseField).ToField();
            Scale = BaseField.Scale;
            Fields = new FieldModel[Size, Size];
            MapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 - Size * 3 + 1);
            WindowSize = new Size(Console.WindowWidth, Console.WindowHeight);
            Point ExpectedPos = new Point
            {
                X = (_StandPosition.X > 0 ? _StandPosition.X : Size + _StandPosition.X) - 1,
                Y = (_StandPosition.Y > 0 ? _StandPosition.Y : Size + _StandPosition.Y) - 1
            };

            int VisualMapSize = (Console.WindowHeight - 3) / 6 + (Console.WindowWidth + 23) / 24 + 3;
            Point VisualMapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 + 4 - VisualMapSize / 2 * 6);
            StandPosition = new Point(VisualMapSize / 2 - (_StandPosition.X % 2), VisualMapSize / 2 - (_StandPosition.Y % 2));
            Point CenterPos = GetMatchPoint(StandPosition, VisualMapPosition, MapPosition);
            MapPosition = GetCoordByPos(new Point(CenterPos.X - ExpectedPos.X, CenterPos.Y - ExpectedPos.Y), MapPosition);

            for (int x = 0; x < Size; x++) SetField(x, 0, ObjectModel.GetObject("eraser").ToField());
            for (int y = 0; y < Size; y++) SetField(0, y, ObjectModel.GetObject("eraser").ToField());
            for (int x = 0; x < Size - 1; x++)
                for (int y = 0; y < Size - 1; y++)
                    SetField(x + 1, y + 1, ObjectModel.GetObject(_BaseField).ToField());
        }
        public MapModel(string _MapString)
        {
            string[] _MapStringSplit = _MapString.Split(' ');
            Name = _MapStringSplit[0];
            Size = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(_MapStringSplit[^1].Length / 2))) + 1;
            BaseField = new ProductModel(_MapStringSplit[^3]).ToField();
            Scale = BaseField.Scale;
            Fields = new FieldModel[Size, Size];
            WindowSize = new Size(int.Parse(_MapStringSplit[1]), int.Parse(_MapStringSplit[2]));
            MapPosition = new Point(int.Parse(_MapStringSplit[3]), int.Parse(_MapStringSplit[4]));
            StandPosition = new Point(int.Parse(_MapStringSplit[5]), int.Parse(_MapStringSplit[6]));
            string[] BaseIDS = _MapStringSplit[7].Split('/');
            string[] BaseRefs = _MapStringSplit[8].Split('/');

            for (int x = 0; x < Size; x++) Fields[x, 0] = ObjectModel.GetObject("eraser").ToField();
            for (int y = 0; y < Size; y++) Fields[0, y] = ObjectModel.GetObject("eraser").ToField();
            for (int x = 0; x < Size - 1; x++)
                for (int y = 0; y < Size - 1; y++)
                    Fields[x + 1, y + 1] = new FieldModel(_MapStringSplit[^1].Substring(x * (Size - 1) * 2 + y * 2, 2), Scale).ToField();

            if(BaseIDS[0] != "-")
            for (int i = 0; i < BaseIDS.Length; i++)
            {
                int BaseID = int.Parse(BaseIDS[i]);
                string[] Refs = BaseRefs[i].Split(',');
                foreach(string RefCoordString in Refs)
                {
                    int RefCoord = int.Parse(RefCoordString);
                    int x = RefCoord / Size;
                    int y = RefCoord % Size;
                    Fields[x, y].BaseID = BaseID;
                    Fields[x, y].BaseView = ObjectModel.GetObject(BaseID).View.ViewClone();
                }
            }

        }
        public override string ToString()
        {
            string[] FieldStrings = new string[(Size - 1) * (Size - 1)];
            string BaseFieldString = BaseField.ToProduct().ToString() + " ";
            for (int x = 1; x < Size; x++)
                for (int y = 1; y < Size; y++)
                    FieldStrings[(x - 1) * (Size - 1) + (y - 1)] = GetField(x, y).ToString();

            List<int> BaseFieldID = new List<int>();
            List<string> BaseFieldRef = new List<string>();
            for (int x = 1; x < Size; x++)
                for (int y = 1; y < Size; y++)
                    if (GetField(x, y).BaseID > 0)
                    {
                        int BaseID = BaseFieldID.IndexOf(GetField(x, y).BaseID);
                        if (BaseID < 0)
                        {
                            BaseFieldID.Add(GetField(x, y).BaseID);
                            BaseFieldRef.Add("");
                            BaseID = BaseFieldID.Count - 1;
                        }
                        BaseFieldRef[BaseID] += (x * Size + y).ToString() + ",";
                    }
            string BaseIDS = "", BaseRef = "";
            foreach (int ID in BaseFieldID) BaseIDS += ID.ToString() + "/";
            foreach (string Ref in BaseFieldRef) BaseRef += Ref[0..^1] + "/";

            string WinSizeString =  WindowSize.Width + " " + WindowSize.Height + " ";
            string MapPosString =  MapPosition.X + " " + MapPosition.Y + " ";
            string StandPosString =  StandPosition.X + " " + StandPosition.Y + " ";
            BaseIDS = BaseIDS.Length == 0 ? "- " : BaseIDS[0..^1] + " ";
            BaseRef = BaseRef.Length == 0 ? "- " : BaseRef[0..^1] + " ";
            return Name + ";" + WinSizeString + MapPosString + StandPosString +
                BaseIDS + BaseRef + BaseFieldString + "\n " + String.Join("", FieldStrings);
        }
    }
}
