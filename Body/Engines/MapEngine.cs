using FarmConsole.Body.Controlers;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.MenuViews;
using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace FarmConsole.Body.Engines
{
    public class MapEngine
    {
        #region PRIVATE ATTRIBUTES
        private static readonly int StandardFieldHeight = 7;
        private static List<Point> SelectedFields;          // Trzyma X,Y zaznaczonych pól Fizycznej Mapys
        private static Point DraggedPosition;               // Pozycja pochwyconego pola z Fizycznej Mapy
        private static FieldModel DraggedField;             // Pochwycone Pole
        private static int TopBorder;
        private static int BotBorder;
        private static int LeftBorder;
        private static int RightBorder;
        private static List<Point> OutermostMapFields;
        private static List<Point>[] OutermostScreenFields;
        private static Point[,] VisualMap;
        private static Point VisualMapPosition;
        private static int VisualMapSize;
        private static MapModel PhisicalMap;

        public static MapModel Map
        {
            set
            {
                if (value == null) PhisicalMap = null;
                else PhisicalMap = value.Copy();
            }
            get
            {
                if (PhisicalMap != null) return PhisicalMap;
                else return null;
            }
        }
        public static int FPM = 2;
        #endregion


        #region PUBLIC MENEGEMENT
        public static void MoveStandPosition(Point Vector = new Point(), bool Shift = false)
        {
            if (!TryMoveCSP(Vector, false)) return;
            Point FSP = new Point(PhisicalMap.StandPosition.X + Vector.X, PhisicalMap.StandPosition.Y + Vector.Y); // future stand point
            if (FSP.X >= 0 && FSP.X < VisualMapSize && FSP.Y >= 0 && FSP.Y < VisualMapSize && VisualMap[FSP.X, FSP.Y].X == 0) return;
            if (DraggedField != null && PhisicalMap.GetField(RealPos(FSP).X, RealPos(FSP).Y).Category == 3) return;
            else if (Shift)
            {
                Point StandPos = RealPos(PhisicalMap.StandPosition);
                if (SelectedFields.Contains(StandPos)) SelectedFields.Remove(StandPos);
                else if (SelectedFields.Count == 0 ||
                    GetField().Category == PhisicalMap.GetField(StandPos.X, StandPos.Y).Category &&
                    GetField().Type == PhisicalMap.GetField(StandPos.X, StandPos.Y).Type) SelectedFields.Add(StandPos);
            }
            Point PSP = new Point(PhisicalMap.StandPosition.X, PhisicalMap.StandPosition.Y); // past stand point
            TryMoveCSP(Vector);
            ShowVisualField(PSP);
            ShowVisualField(PhisicalMap.StandPosition);

            // extra write:
            //ShowInfo_VisualMapContent();
            ShowInfo_Positions();
        }
        public static void MoveMapPosition(Point Vector)
        {
            if (!TryMoveCSP(Vector, false, false)) return;

            Point ViewPos = PhisicalMap.View.GetPosition;
            Point MOVE = new Point(0, 0), ITER = new Point(1, 1);
            Point START = new Point(LeftBorder, TopBorder);
            Point END = new Point(RightBorder, BotBorder);
            Point VisualPos = new Point(VisualMapPosition.X, VisualMapPosition.Y);
            int LB = LeftBorder, RB = RightBorder, TB = TopBorder, BB = BotBorder, EDGE = 0;

            if (Vector.X == -1 && Vector.Y == 1) { EDGE = 0; MOVE.X = 24 / FPM;  RB = LB + MOVE.X; START.X = END.X; END.X = MOVE.X; ITER.X = -1; }
            if (Vector.X == 1 && Vector.Y == -1) { EDGE = 1; MOVE.X = -24 / FPM; LB = RB + MOVE.X; END.X += MOVE.X; }
            if (Vector.X == 1 && Vector.Y == 1)  { EDGE = 2; MOVE.Y = 6 / FPM;   BB = TB + MOVE.Y; START.Y = END.Y; END.Y = MOVE.Y; ITER.Y = -1; }
            if (Vector.X == -1 && Vector.Y == -1){ EDGE = 3; MOVE.Y = -6 / FPM;  TB = BB + MOVE.Y; END.Y += MOVE.Y; }

            for (int i = 0; i < FPM; i++)
            {
                for (int Y = START.Y; Y != END.Y + ITER.Y; Y += ITER.Y)
                    for (int X = START.X; X != END.X + ITER.X; X += ITER.X)
                        PhisicalMap.View.SetPixel(X - ViewPos.X, Y - ViewPos.Y, PhisicalMap.View.GetPixel(X - MOVE.X - ViewPos.X, Y - MOVE.Y - ViewPos.Y));

                VisualMapPosition.X += MOVE.X;
                VisualMapPosition.Y += MOVE.Y;
                foreach (var OSField in OutermostScreenFields[EDGE]) SetFieldWords(OSField, LB, RB, TB, BB);
                ShowMapView();

                if (MenuController.FieldNameVisibility) GameView.DisplayFieldName();
            }

            VisualMapPosition.X = VisualPos.X;
            VisualMapPosition.Y = VisualPos.Y;
            PhisicalMap.MapPosition.X += (Vector.Y - Vector.X) * 12;
            PhisicalMap.MapPosition.Y += (Vector.Y + Vector.X) * 3;

            START = new Point(0, 0);
            ITER = new Point(1, 1);
            END = new Point(VisualMapSize - 1, VisualMapSize - 1);
            if (Vector.X > 0) { START.X = END.X; END.X = 0; ITER.X = -1; }
            if (Vector.Y > 0) { START.Y = END.Y; END.Y = 0; ITER.Y = -1; }

            TryMoveCSP(Vector, true, false);

            for (int X = START.X; X != END.X; X += ITER.X)
                for (int Y = START.Y; Y != END.Y; Y += ITER.Y)
                    VisualMap[X, Y] = VisualMap[X + ITER.X, Y + ITER.Y];

            foreach (Point OMField in OutermostMapFields)
            {
                Point PhisicalPos = GetMatchPoint(OMField, true);
                if (IsOnMap(PhisicalPos, PhisicalMap.MapSize, 1)) VisualMap[OMField.X, OMField.Y] = new Point(PhisicalPos.X, PhisicalPos.Y);
                else VisualMap[OMField.X, OMField.Y] = new Point();
            }

            //extra write:
            //ShowInfo_VisualMapContent();
            //ShowInfo_Positions();
        }
        public static int GetSelectedFieldCount()
        {
            return SelectedFields.Count;
        }
        public static FieldModel GetField(string FieldType = "")
        {
            Point P = RealPos(PhisicalMap.StandPosition);
            switch(FieldType)
            {
                case "Base": return PhisicalMap.BaseField;
                case "Dragged": return DraggedField;
                case "Stand": return PhisicalMap.GetField(P.X, P.Y);
                default:
                {
                    if (SelectedFields.Count == 0) return PhisicalMap.GetField(P.X, P.Y);
                    else return PhisicalMap.GetField(SelectedFields[0].X, SelectedFields[0].Y);
                }
            }
        }
        public static void ReloadMapView()
        {
            PhisicalMap.BaseField.View = PhisicalMap.BaseField.View.ViewClone();
            for (int x = 0; x < PhisicalMap.MapSize; x++)
                for (int y = 0; y < PhisicalMap.MapSize; y++)
                    PhisicalMap.GetField(x, y).View = PhisicalMap.GetField(x, y).ToField().View.ViewClone();

            InitMapView();
            ShowMapView();
        }
        #endregion


        #region PROTECTED MANAGEMENT
        protected static void SetField(Point PhisicalPos, FieldModel NewField, string FieldType = "")
        {
            switch (FieldType)
            {
                case "Base":
                    {
                        FieldModel _ParentField = PhisicalMap.GetField(PhisicalPos.X, PhisicalPos.Y);
                        int BaseID = _ParentField.BaseID;
                        if (BaseID > 0)
                        {
                            FieldModel _ChildField = ObjectModel.GetObject(BaseID).ToField();
                            _ChildField.View = _ParentField.BaseView.ViewClone();
                            PhisicalMap.SetField(PhisicalPos.X, PhisicalPos.Y, _ChildField);
                        }
                        else PhisicalMap.SetField(PhisicalPos.X, PhisicalPos.Y, PhisicalMap.BaseField);
                    }
                    break;
                case "Dragged":
                    if (PhisicalPos.X > 0 && PhisicalPos.Y > 0)
                    {
                        DraggedPosition = new Point(PhisicalPos.X, PhisicalPos.Y);
                        DraggedField = PhisicalMap.GetField(PhisicalPos.X, PhisicalPos.Y).ToField();
                    }
                    else
                    {
                        DraggedField = null;
                        DraggedPosition = new Point(-1, -1);
                    }
                    break;
                default: if (NewField != null) PhisicalMap.SetField(PhisicalPos.X, PhisicalPos.Y, NewField); break;
            }
        }
        protected static void ShowField(Point PhisicalPos)
        {
            Point VisualPos = GetMatchPoint(PhisicalPos);
            if (IsOnMap(VisualPos, VisualMapSize)) ShowVisualField(VisualPos);
        }
        protected static void ClearSelectedFields(int CountToRemove = 0)
        {
            if (CountToRemove == 0) CountToRemove = SelectedFields.Count;
            if (SelectedFields.Count == 0) ShowField(GetPos());
            while (SelectedFields.Count > 0 && CountToRemove > 0)
            {
                CountToRemove--;
                var PhisicalPos = SelectedFields[0];
                SelectedFields.RemoveAt(0);
                ShowField(PhisicalPos);
            }
        }
        protected static void SetMapScreenBorders()
        {
            TopBorder = 5;
            BotBorder = Console.WindowHeight - 4;
            LeftBorder = 0;
            RightBorder = Console.WindowWidth - 1;
        }
        protected static void InitVisualMap()
        {
            VisualMapSize = (Console.WindowWidth + 23) / 24 + (Console.WindowHeight - 3) / 6 + 3;
            int correction = (VisualMapSize - VisualMapSize % 2) * 3 - 6;
            VisualMapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 - correction);
            VisualMap = new Point[VisualMapSize, VisualMapSize];
            SelectedFields = new List<Point>() { };
            DraggedField = null;
            DraggedPosition = new Point(-1, -1);

            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                {
                    Point PhisicalPos = GetMatchPoint(new Point(x, y), true);
                    if (IsOnMap(PhisicalPos, PhisicalMap.MapSize, 1)) VisualMap[x, y] = new Point(PhisicalPos.X, PhisicalPos.Y);
                }

            InitOutermosMapFields();
            InitOutermostScreenFields();
        }
        protected static void InitMapView()
        {
            int W = Console.WindowWidth, H = Console.WindowHeight - 8;
            PhisicalMap.View = new ViewModel(new PixelModel[W, H], W, H, new Point(0, 5));
            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                    SetFieldWords(new Point(x, y));
        }
        protected static void ShowMapView()
        {
            Point ViewPos = PhisicalMap.View.GetPosition;
            foreach (var w in PhisicalMap.View.GetWords)
                WindowService.Write(ViewPos.X + w.Position.X, ViewPos.Y + w.Position.Y, w.Content, w.Color);
        }
        protected static void DarkerMapView(int procent) => PhisicalMap.View.ColorizeWords("Darker", procent);
        protected static void CenterVisualMapOnPos(Point PhisicalPos)
        {
            ClearMapSreen();

            VisualMap = new Point[VisualMapSize, VisualMapSize];
            PhisicalMap.StandPosition = new Point(VisualMapSize / 2 + (PhisicalPos.X % 2), VisualMapSize / 2 + (PhisicalPos.Y % 2));
            Point CenterPos = GetMatchPoint(PhisicalMap.StandPosition, true);
            PhisicalMap.MapPosition = GetCoordByPos(new Point(CenterPos.X - PhisicalPos.X, CenterPos.Y - PhisicalPos.Y), PhisicalMap.MapPosition);

            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                {
                    Point Positon = GetMatchPoint(new Point(x, y), true);
                    if (IsOnMap(Positon, PhisicalMap.MapSize, 1)) VisualMap[x, y] = new Point(Positon.X, Positon.Y);
                }

            InitMapView();
            ShowMapView();
        }
        protected static void ClearMapSreen()
        {
            string space = "".PadRight(Console.WindowWidth, ' ');
            for (int i = TopBorder; i <= BotBorder; i++)
                WindowService.Write(0, i, space, ColorService.BackgroundColor);
        }
        protected static Point GetPos(bool Dragged = false)
        {
            if (Dragged) return DraggedPosition;
            if (SelectedFields.Count > 0) return new Point(SelectedFields[0].X, SelectedFields[0].Y);
            else return RealPos(PhisicalMap.StandPosition);
        }
        protected static Point GetCoordByPos(Point MapPos, Point StartPos)
        {
            return new Point(StartPos.X + (MapPos.Y - MapPos.X) * 12, StartPos.Y + (MapPos.Y + MapPos.X) * 3);
        }
        protected static Point GetMatchPoint(Point Position, bool GetRealPos = false)
        {
            if (GetRealPos) return GetPosByCoord(GetCoordByPos(Position, VisualMapPosition), PhisicalMap.MapPosition);
            else return GetPosByCoord(GetCoordByPos(Position, PhisicalMap.MapPosition), VisualMapPosition);
        }
        protected static Point GetMatchPoint(Point Position, Point SourceMapMapPosition, Point DestinationMapPosition)
        {
            return GetPosByCoord(GetCoordByPos(Position, SourceMapMapPosition), DestinationMapPosition);
        }
        #endregion


        #region PRIVATE MANAGEMENT
        private static void InitOutermosMapFields()
        {
            OutermostMapFields = new List<Point>();
            for (int x = 0; x < VisualMapSize; x++) OutermostMapFields.Add(new Point(x, 0));
            for (int x = 0; x < VisualMapSize; x++) OutermostMapFields.Add(new Point(x, VisualMapSize - 1));
            for (int y = 1; y < VisualMapSize - 1; y++) OutermostMapFields.Add(new Point(0, y));
            for (int y = 1; y < VisualMapSize - 1; y++) OutermostMapFields.Add(new Point(VisualMapSize - 1, y));
        }
        private static void InitOutermostScreenFields()
        {
            OutermostScreenFields = new List<Point>[] { new List<Point>(), new List<Point>(), new List<Point>(), new List<Point>()};

            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                {
                    Point P = GetCoordByPos(new Point(x,y), VisualMapPosition);
                    if (IsBetweenBorder(P, LeftBorder - 22, LeftBorder + 22, TopBorder - 6, BotBorder + 5)) OutermostScreenFields[0].Add(new Point(x, y));
                    if (IsBetweenBorder(P, RightBorder - 22, RightBorder + 22, TopBorder - 6, BotBorder + 5)) OutermostScreenFields[1].Add(new Point(x, y));
                    if (IsBetweenBorder(P, LeftBorder - 22, RightBorder + 22, TopBorder - 6, TopBorder + 5)) OutermostScreenFields[2].Add(new Point(x, y));
                    if (IsBetweenBorder(P, LeftBorder - 22, RightBorder + 22, BotBorder - 6, BotBorder + 5)) OutermostScreenFields[3].Add(new Point(x, y));
                }
        }
        private static bool TryMoveCSP(Point Vector, bool Change = true, bool IncludeHitbox = true)
        {
            Point TargetVisualPos = new Point(PhisicalMap.StandPosition.X + Vector.X, PhisicalMap.StandPosition.Y + Vector.Y);
            if (!IsOnMap(TargetVisualPos, VisualMapSize)) return false;

            short Escape = (short)(Math.Abs(Vector.X) * Math.Abs(Vector.X - 1) + Math.Abs(Vector.Y) * Math.Abs(Vector.Y - 2));
            short Arrival = (short)(Math.Abs(Vector.X) * (Vector.X + 1) + Math.Abs(Vector.Y) * (Vector.Y + 2));
            if (IncludeHitbox)
            {
                var Field = GetField("Stand");
                if (Field.StateName[0] == '-') // Escape Hitbox
                {
                    if (Field.StateName.Contains('2')) Arrival = AccessEscape(Field.ArrivalDirection, Escape, Field.StateName);
                    else if (!CrossingPermission(Escape, Field.StateName)) return false;
                    if (Arrival < 0) return false;
                }
                Field = PhisicalMap.GetField(RealPos(TargetVisualPos).X, RealPos(TargetVisualPos).Y);
                if (Field.StateName[0] == '-' && !Field.StateName.Contains('2')) // Enter Hitbox
                    if (!CrossingPermission(Arrival, Field.StateName)) return false;
            }
            else Arrival = GetField("Stand").ArrivalDirection;

            if (Change) // Confirm Move
            {
                PhisicalMap.StandPosition.X += Vector.X;
                PhisicalMap.StandPosition.Y += Vector.Y;
                GetField("Stand").ArrivalDirection = Arrival;
            }
            return true;
        }
        private static void ShowVisualField(Point VisualPoint)
        {
            Point Coord = GetCoordByPos(VisualPoint, VisualMapPosition);
            int[] Borders = new int[] { Coord.X, Coord.X + 22, Coord.Y - 5, Coord.Y + 6 };
            int[] FieldsAround = new int[] { -1,-1,  0,-1,  -1,0,  0,0,  1,0,  0,1,  1,1,  2,1,  1,2 };
            for (int i = 0; i < FieldsAround.Length; i += 2)
            {
                var Position = new Point(VisualPoint.X + FieldsAround[i], VisualPoint.Y + FieldsAround[i + 1]);
                if (IsOnMap(Position, VisualMapSize))
                    SetFieldWords(Position, Borders[0], Borders[1], Borders[2], Borders[3]);
            }
            PhisicalMap.View.DisplayPixels(new Point(Borders[0], Borders[2]), new Size(23, 12));
        }
        private static void SetFieldWords(Point VisualPoint, int LB = -1, int RB = 10000, int TB = -1, int BB = 10000)
        {
            if (RB < LeftBorder || LB > RightBorder || BB < TopBorder || TB > BotBorder) return;
            LB = LB < LeftBorder ? LeftBorder : LB;
            RB = RB > RightBorder ? RightBorder : RB;
            TB = TB < TopBorder ? TopBorder : TB;
            BB = BB > BotBorder ? BotBorder : BB;

            Point ViewPos = PhisicalMap.View.GetPosition;
            Point P = RealPos(VisualPoint);

            FieldModel Field = PhisicalMap.GetField(P.X, P.Y);
            List<ViewModel> views = new List<ViewModel>() { PhisicalMap.GetField(P.X, P.Y).View };
            if (PhisicalMap.StandPosition == VisualPoint && DraggedField != null) views.Add(DraggedField.View);
            if (Field.BaseID > 0) views.Insert(0, Field.BaseView);

            int StartIndex, Lenght, LeftPad;
            int X = VisualMapPosition.X + (VisualPoint.Y - VisualPoint.X) * 12;
            int Y = VisualMapPosition.Y + (VisualPoint.Y + VisualPoint.X) * 3;

            foreach (var view in views)
                foreach (var word in view.GetWords)
                {
                    int C = StandardFieldHeight - view.GetSize.Height;
                    if (Y + word.Position.Y + C >= TB && BB >= Y + word.Position.Y + C &&
                        X + word.Position.X + word.Content.Length > LB && RB >= X + word.Position.X)
                    {
                        Color Color = SelectedFields.Contains(P) || Field == DraggedField ?
                              Color = ColorService.Yellower(word.Color) : word.Color;
                        Color = PhisicalMap.StandPosition == VisualPoint ? Color = ColorService.Brighter(word.Color) : Color;
                        StartIndex = X + word.Position.X < LB ? LB - X - word.Position.X : 0;
                        LeftPad = X + StartIndex + word.Position.X;
                        Lenght = word.Content.Length - StartIndex > RB - LeftPad + 1 ? RB - LeftPad + 1 : word.Content.Length - StartIndex;
                        string WordContent = word.Content.Substring(StartIndex, Lenght);
                        PhisicalMap.View.SetWord(LeftPad - ViewPos.X, Y + word.Position.Y + C - ViewPos.Y, WordContent, Color);
                    }
                }
        }
        private static bool IsOnMap(Point CheckPos, int MapSize, int StartFrom = 0)
        {
            if (CheckPos.X >= StartFrom && CheckPos.Y >= StartFrom && CheckPos.X < MapSize && CheckPos.Y < MapSize) return true;
            else return false;
        }
        private static bool IsBetweenBorder(Point P, int LB, int RB, int TB, int BB)
        {
            if (P.X + 22 >= LB && P.X <= RB && P.Y + 6 >= TB && P.Y - 6 <= BB) return true;
            else return false;
        }
        private static Point RealPos(Point VisualPos)
        {
            return VisualMap[VisualPos.X, VisualPos.Y];
        }
        private static Point GetPosByCoord(Point MapCoord, Point StartPos)
        {
            return new Point((4 * (MapCoord.Y - StartPos.Y) + StartPos.X - MapCoord.X) / 24,
                             (4 * (MapCoord.Y - StartPos.Y) + MapCoord.X - StartPos.X) / 24);
        }
        private static short AccessEscape(int ArrivalDirection, int DepartureDirection, string StateName)
        {
            StateName = StateName.Trim('-');
            int[] P0 = new int[] { 1, 3, 5, 7, 0, 2, 4, 6 };
            short[] P1 = new short[] { 4, 0, 5, 1, 6, 2, 7, 3 };
            short[] P2 = new short[] { 6, 7, 4, 5, 2, 3, 0, 1 };
            bool[] Blokada = new bool[8];
            bool[] Dostep = new bool[8];
            for (int i = 0; i < 4; i++) Blokada[i * 2 + 1] = Convert.ToBoolean(StateName[i] - 48);

            int it = P0[Convert.ToInt32(ArrivalDirection)];
            Dostep[P1[it]] = true;
            do { it = (it + 1) % 8; if (!Blokada[it]) Dostep[P1[it]] = true; }
            while (Dostep[P1[it]]);

            it = P0[Convert.ToInt32(ArrivalDirection)];
            do { it = (it + 7) % 8; if (!Blokada[(it + 1) % 8]) Dostep[P1[it]] = true; }
            while (Dostep[P1[it]]);

            if (Dostep[DepartureDirection] == true) return P2[DepartureDirection];
            if (Dostep[DepartureDirection + 4] == true) return P2[DepartureDirection + 4];
            return -1;
        }
        private static bool CrossingPermission(int Direction, string StateName)
        {
            if (StateName.Trim('-')[Direction % 4] > 48) return false;
            return true;
        }
        private static void ShowInfo_FieldsID()
        {
            WindowService.Write(0, 0, VisualMapSize.ToString(), ColorService.ForegroundColor);
            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                    if (VisualMapPosition.X + 17 + (y - x) * 12 < RightBorder && VisualMapPosition.X + 10 + (y - x) * 12 > LeftBorder
                        && VisualMapPosition.Y + 3 + (y + x) * 3 < Console.WindowHeight - 1 && VisualMapPosition.Y + 3 + (y + x) * 3 >= 0)
                    {
                        Console.SetCursorPosition(VisualMapPosition.X + 10 + (y - x) * 12, VisualMapPosition.Y + 3 + (y + x) * 3);
                        Console.Write(" " + x + "," + y + " ");

                        //Console.SetCursorPosition(PMap.Fields[i, j].X, PMap.Fields[i, j].Y);
                        //Console.Write(" " + PMap.Fields[i, j].X + "," + PMap.Fields[i, j].Y + " ");
                    }
        }
        private static void ShowInfo_VisualMapContent()
        {
            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                {
                    Console.SetCursorPosition(14 + 5 * x, 6 + 2 * y);
                    if (VisualMap[x, y].X == 0 && VisualMap[x, y].Y == 0)
                    Console.Write((VisualMap[x, y].X + "," + VisualMap[x, y].Y + "  ").Pastel(ColorService.GetColorByName("Red")));
                    else 
                    Console.Write(VisualMap[x, y].X + "," + VisualMap[x, y].Y + "  ");
                }
            Console.SetCursorPosition(14 + 5 * PhisicalMap.StandPosition.X, 6 + 2 * PhisicalMap.StandPosition.Y);
            Console.Write((PhisicalMap.StandPosition.X + "," + PhisicalMap.StandPosition.Y + " ").ToString().Pastel(ColorService.GetColorByName("Magenta")));
        }
        private static void ShowInfo_Positions()
        {
            Color color = ColorService.GetColorByName("gray5");
            Point vmp = GetPosByCoord(PhisicalMap.MapPosition, VisualMapPosition);
            //Console.SetCursorPosition(2, 7); Console.Write("PMP:[" + vmp.X + "," + vmp.Y + "]  ");
            WindowService.Write(2, 6, "PSP:[" + RealPos(PhisicalMap.StandPosition).X + "," + RealPos(PhisicalMap.StandPosition).Y + "]  ", color);
            WindowService.Write(2, 7, "VSP:[" + PhisicalMap.StandPosition.X + "," + PhisicalMap.StandPosition.Y + "]  ", color);
            WindowService.Write(2, 8, "DRG:[" + DraggedPosition.X + "," + DraggedPosition.Y + "]  ", color);
        }
        #endregion
    }
}
