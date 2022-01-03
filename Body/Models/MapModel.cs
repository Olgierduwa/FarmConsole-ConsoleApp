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
        public Size WindowSize;
        public Point MapPosition;
        public Point StandPosition;
        public string Name { get; set; }
        public string EscapeScreen { get; set; }
        public int MapSize { get; set; }
        public int Scale { get; set; }
        public ViewModel View { get; set; }
        public FieldModel BaseField { get; set; }

        public DateTime LastVisitDate { get; set; }
        private SupplyModel Supply { get; set; }
        public void SetSupply(SupplyModel supply) => Supply = supply;
        public void SortContainers(List<ProductModel> Cart) { if (Supply != null) Supply.SortContainers(Fields, Cart); }
        public void Delivery(DateTime CurrentGameDate)
        {
            if (Supply != null) Supply.Delivery(Fields, CurrentGameDate, LastVisitDate);
        }

        private FieldModel[,] Fields { get; set; }
        public FieldModel GetField(int x, int y) => Fields[x, y];
        public void SetField(int x, int y, FieldModel Field)
        {
            if(x < MapSize && y < MapSize && Field != null)
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
            int VSize = (WindowSize.Width + 23) / 24 + (WindowSize.Height - 3) / 6 + 3;
            int correction = (VSize - VSize % 2) * 3 - 6;
            Point VisualMapPosition = new Point(WindowSize.Width / 2 - 12, (WindowSize.Height - 8) / 2 - correction);
            Point StandPos = GetMatchPoint(StandPosition, VisualMapPosition, MapPosition);

            WindowSize = new Size(Console.WindowWidth, Console.WindowHeight);
            MapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 - (MapSize + MapSize % 2) * 3);
            VSize =  (Console.WindowWidth + 23) / 24 + (Console.WindowHeight - 3) / 6 + 3;
            correction = (VSize - VSize % 2) * 3 - 6;
            VisualMapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 - correction);

            StandPosition = new Point(VSize / 2 + (StandPos.X % 2), VSize / 2 + (StandPos.Y % 2));
            Point CenterPos = GetMatchPoint(StandPosition, VisualMapPosition, MapPosition);
            MapPosition = GetCoordByPos(new Point(CenterPos.X - StandPos.X, CenterPos.Y - StandPos.Y), MapPosition);
        }
        public void SetDurationView()
        {
            for (int x = 1; x < MapSize; x++)
                for (int y = 1; y < MapSize; y++)
                {
                    FieldModel Field = Fields[x, y];
                    if (Field.Category == 1)
                    {
                        int Increase = Field.Duration / 10;
                        if (Increase % 2 == 1) Field.View.ColorizePixels("Darker");
                        if (Increase > 1) Field.View.ColorizePixels("Bluer");
                    }
                }
        }

        private MapModel() { }
        public MapModel Copy()
        {
            MapModel map = new MapModel
            {
                Name = Name.ToString(),
                EscapeScreen = EscapeScreen,
                MapSize = MapSize,
                Scale = Scale,
                Supply = Supply == null ? null : new SupplyModel(Supply.ToString(), MapSize),
                LastVisitDate = new DateTime(LastVisitDate.Year, LastVisitDate.Month, LastVisitDate.Day),
                WindowSize = new Size(WindowSize.Width, WindowSize.Height),
                MapPosition = new Point(MapPosition.X, MapPosition.Y),
                StandPosition = new Point(StandPosition.X, StandPosition.Y),
                BaseField = BaseField.ToField(),
                Fields = new FieldModel[MapSize, MapSize]
            };
            for (int x = 0; x < MapSize; x++)
                for (int y = 0; y < MapSize; y++)
                    map.SetField(x, y, GetField(x, y).ToField());

            map.View = new ViewModel(View.GetPixels, View.GetSize.Width, View.GetSize.Height, View.GetPosition);
            map.SetDurationView();

            return map;
        }
        public MapModel(string _MapName, string _BaseField, Point _StandPosition, int _Size)
        {
            Name = _MapName;
            MapSize = _Size + 1;
            BaseField = ObjectModel.GetObject(_BaseField).ToField();
            Scale = BaseField.Scale;
            Fields = new FieldModel[MapSize, MapSize];
            WindowSize = new Size(Console.WindowWidth, Console.WindowHeight);
            LastVisitDate = new DateTime();
            int W = Console.WindowWidth, H = Console.WindowHeight - 8;
            View = new ViewModel(new PixelModel[W, H], W, H, new Point(0, 5));
            Point ExpectedPos = new Point
            {
                X = (_StandPosition.X > 0 ? _StandPosition.X : MapSize + _StandPosition.X) - 1,
                Y = (_StandPosition.Y > 0 ? _StandPosition.Y : MapSize + _StandPosition.Y) - 1
            };

            MapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 - (MapSize + MapSize % 2) * 3);
            int VSize = (Console.WindowWidth + 23) / 24 + (Console.WindowHeight - 3) / 6 + 3;
            int correction = (VSize - VSize % 2) * 3 - 6;
            Point VisualMapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 - correction);
            StandPosition = new Point(VSize / 2 + (_StandPosition.X % 2), VSize / 2 + (_StandPosition.Y % 2));
            Point CenterPos = GetMatchPoint(StandPosition, VisualMapPosition, MapPosition);
            MapPosition = GetCoordByPos(new Point(CenterPos.X - ExpectedPos.X, CenterPos.Y - ExpectedPos.Y), MapPosition);
            
            for (int x = 0; x < MapSize; x++) SetField(x, 0, ObjectModel.GetObject("eraser").ToField());
            for (int y = 0; y < MapSize; y++) SetField(0, y, ObjectModel.GetObject("eraser").ToField());
            for (int x = 0; x < MapSize - 1; x++)
                for (int y = 0; y < MapSize - 1; y++)
                    SetField(x + 1, y + 1, ObjectModel.GetObject(_BaseField).ToField());

            SetDurationView();
        }
        public MapModel(string _MapString)
        {
            _MapString = _MapString.Replace("\n", "");
            string[] _MapStringSplit = _MapString.Split(' ');
            int W = Console.WindowWidth, H = Console.WindowHeight - 8;
            View = new ViewModel(new PixelModel[W, H], W, H, new Point(0, 5));
            Name = _MapStringSplit[0].Split(':')[0];
            EscapeScreen = _MapStringSplit[0].Split(':')[1];
            MapSize = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(_MapStringSplit[^1].Length / 2))) + 1;
            Fields = new FieldModel[MapSize, MapSize];
            WindowSize = new Size(int.Parse(_MapStringSplit[1]), int.Parse(_MapStringSplit[2]));
            MapPosition = new Point(int.Parse(_MapStringSplit[3]), int.Parse(_MapStringSplit[4]));
            StandPosition = new Point(int.Parse(_MapStringSplit[5]), int.Parse(_MapStringSplit[6]));
            LastVisitDate = DateTime.Parse(_MapStringSplit[7]);
            BaseField = new ProductModel(_MapStringSplit[8]).ToField();
            Scale = BaseField.Scale;
            string BaseFieldsString = _MapStringSplit[9];
            string PocketsString = _MapStringSplit[10];
            string SuplyString = _MapStringSplit[11];
            Supply = SuplyString == "-" ? null : new SupplyModel(SuplyString, MapSize);

            for (int x = 0; x < MapSize; x++) Fields[x, 0] = ObjectModel.GetObject("eraser").ToField();
            for (int y = 0; y < MapSize; y++) Fields[0, y] = ObjectModel.GetObject("eraser").ToField();
            for (int x = 0; x < MapSize - 1; x++)
                for (int y = 0; y < MapSize - 1; y++)
                    Fields[x + 1, y + 1] = new FieldModel(_MapStringSplit[^1].Substring(x * (MapSize - 1) * 2 + y * 2, 2), Scale).ToField();

            if (BaseFieldsString != "-")
            {
                string[] BaseIDS = BaseFieldsString.Split(':')[0].Split('/');
                string[] BaseRefs = BaseFieldsString.Split(':')[1].Split('/');
                for (int i = 0; i < BaseIDS.Length; i++)
                {
                    short BaseID = short.Parse(BaseIDS[i]);
                    string[] Refs = BaseRefs[i].Split(',');
                    foreach (string RefCoordString in Refs)
                    {
                        int RefCoord = int.Parse(RefCoordString);
                        int x = RefCoord / MapSize;
                        int y = RefCoord % MapSize;
                        Fields[x, y].BaseID = BaseID;
                        Fields[x, y].BaseView = ObjectModel.GetObject(BaseID).View.ViewClone();
                    }
                }
            }

            if (PocketsString != "-")
            {
                string[] PocketIDS = PocketsString.Split(':')[0].Split('/');
                string[] PocketContent = PocketsString.Split(':')[1].Split('/');
                for (int i = 0; i < PocketIDS.Length; i++)
                {
                    string[] PocketDefinition = PocketIDS[i].Split('@');
                    int PocketID = int.Parse(PocketDefinition[0]);
                    string PocketSufixName = PocketDefinition.Length > 1 ? PocketDefinition[1].Replace('_', ' ') : "";
                    int x = PocketID / MapSize;
                    int y = PocketID % MapSize;
                    Fields[x, y].Pocket = new ContainerModel(PocketContent[i], Fields[x, y].Slots, PocketSufixName);
                }
            }

            SetDurationView();
        }
        public override string ToString()
        {
            string WinSizeString =  WindowSize.Width + " " + WindowSize.Height + " ";
            string MapPosString =  MapPosition.X + " " + MapPosition.Y + " ";
            string StandPosString =  StandPosition.X + " " + StandPosition.Y + " ";
            string LastVisitString = LastVisitDate.ToString("d") + " ";
            string BaseFieldString = BaseField.ToProduct().ToString() + " ";
            string BaseFieldsString = "", PocketIDS = "", PocketContent = "", PocketsString = "";
            List<int> BaseFieldID = new List<int>();
            List<string> BaseFieldRef = new List<string>();
            string SupplyString = Supply == null ? "- " : Supply.ToString() + " ";
            string[] FieldStrings = new string[(MapSize - 1) * (MapSize - 1)];
            string MapNameString = Name + ";" + EscapeScreen + " ";

            for (int x = 1; x < MapSize; x++)
                for (int y = 1; y < MapSize; y++)
                {
                    var field = GetField(x, y);
                    FieldStrings[(x - 1) * (MapSize - 1) + (y - 1)] = field.ToString();

                    if (field.BaseID > 0) // finding base fields
                    {
                        int BaseID = BaseFieldID.IndexOf(field.BaseID);
                        if (BaseID < 0)
                        {
                            BaseFieldID.Add(field.BaseID);
                            BaseFieldRef.Add("");
                            BaseID = BaseFieldID.Count - 1;
                        }
                        BaseFieldRef[BaseID] += (x * MapSize + y).ToString() + ",";
                    }

                    if (field.Pocket != null) // finding pocket fields
                    {
                        var sufix = field.Pocket.SufixName;
                        PocketIDS += (x * MapSize + y).ToString();
                        PocketIDS += (sufix.Length > 0 ? "@" + sufix.Replace(' ', '_') : "") + "/";
                        PocketContent += field.Pocket.ToString() + "/";
                    }
                }

            foreach (int ID in BaseFieldID) BaseFieldsString += ID.ToString() + "/";
            BaseFieldsString = BaseFieldsString.Length > 0 ? BaseFieldsString[0..^1] + ":" : "";
            foreach (string Ref in BaseFieldRef) BaseFieldsString += Ref[0..^1] + "/";
            BaseFieldsString = BaseFieldsString.Length == 0 ? "- " : BaseFieldsString[0..^1] + " ";
            PocketsString = PocketIDS.Length == 0 ? "- " : PocketIDS[0..^1] + ":" + PocketContent[0..^1] + " ";

            return  MapNameString + WinSizeString + MapPosString + StandPosString + LastVisitString + BaseFieldString + "\n" +
                    BaseFieldsString + "\n" + PocketsString + "\n" + SupplyString + "\n" + String.Join("", FieldStrings);
        }
    }
}
