using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace FarmConsole.Body.Services
{
    public class MapService
    {
        #region PRIVATE ATTRIBUTES
        private static readonly int StandardFieldHeight = 7;
        private static List<Point> SelectedFields;          // Trzyma X,Y zaznaczonych pól Fizycznej Mapy
        private static List<List<string>>[] FieldViews;
        private static List<List<int>>[] FSP;               // Field Start Positions
        private static Point VSP;                           // Visual Stand Position //
        private static Point DraggedPosition;               // Pozycja pochwyconego pola z Fizycznej Mapy
        private static FieldModel BaseField;                // Pole Podłoża
        private static FieldModel DraggedField;             // Pochwycone Pole
        private static int TopBorder;
        private static int BotBorder;
        private static int LeftBorder;
        private static int RightBorder;
        private static int[,,] SimpleMap;
        private static List<Point> MapExtremePoints;
        private static FieldModel[,] PhisicalMap;
        private static Point PhisicalMapPosition;
        private static int PhisicalMapSize;
        private static Point[,] VisualMap;
        private static Point VisualMapPosition;
        private static int VisualMapSize;
        #endregion

        #region PUBLIC MENEGEMENT
        public static void MoveStandPosition(Point Vector = new Point(), bool Shift = false)
        {
            if (!TryMoveCSP(Vector, false)) return;
            bool FixAbove = false;
            Point FSP = new Point(VSP.X + Vector.X, VSP.Y + Vector.Y); // future stand point
            if (FSP.X >= 0 && FSP.X < VisualMapSize && FSP.Y >= 0 && FSP.Y < VisualMapSize && VisualMap[FSP.X, FSP.Y].X == 0) return;
            if (DraggedField != null)
                if (PhisicalMap[RealPos(FSP).X, RealPos(FSP).Y].Category > 6) return;
                else FixAbove = true;
            else if (Shift)
            {
                Point StandPos = RealPos(VSP);
                if (SelectedFields.Contains(StandPos)) SelectedFields.Remove(StandPos);
                else if (SelectedFields.Count == 0 || GetFieldProp("category") == PhisicalMap[StandPos.X, StandPos.Y].Category) SelectedFields.Add(StandPos);
            }
            Point PSP = new Point(VSP.X, VSP.Y); // past stand point
            TryMoveCSP(Vector);
            ShowVisualField(PSP, FixAbove);
            ShowVisualField(VSP);

            // extra write:
            //ShowInfo_VisualMapContent();
            ShowInfo_Positions();
        }
        public static void MoveMapPosition(Point Vector)
        {
            if (!TryMoveCSP(Vector, false)) return;
            PhisicalMapPosition.X += (Vector.Y - Vector.X) * 12;
            PhisicalMapPosition.Y += (Vector.Y + Vector.X) * 3;
            int x_beg = 0, y_beg = 0, x_dif = 1, y_dif = 1, x_end = VisualMapSize - 1, y_end = VisualMapSize - 1;
            if (Vector.X > 0) { x_beg = x_end; x_end = 0; x_dif = -1; }
            if (Vector.Y > 0) { y_beg = y_end; y_end = 0; y_dif = -1; }

            for (int x = x_beg; x != x_end; x += x_dif)
                for (int y = y_beg; y != y_end; y += y_dif)
                    VisualMap[x, y] = VisualMap[x + x_dif, y + y_dif];

            int FrameSize = PhisicalMapSize + 1;
            Point FramePosition = new Point(PhisicalMapPosition.X, PhisicalMapPosition.Y - 3);
            foreach (Point ExtremePoint in MapExtremePoints)
            {
                Point VisualCoord = GetCoordByPos(new Point(ExtremePoint.X, ExtremePoint.Y), VisualMapPosition);
                Point PhisicalPos = GetPosByCoord(VisualCoord, PhisicalMapPosition);
                Point FramePos = GetPosByCoord(VisualCoord, FramePosition);

                if (IsOnMap(PhisicalPos, PhisicalMapSize, 1)) VisualMap[ExtremePoint.X, ExtremePoint.Y] = new Point(PhisicalPos.X, PhisicalPos.Y);
                else if (IsOnMap(FramePos, FrameSize)) VisualMap[ExtremePoint.X, ExtremePoint.Y] = new Point(0, 1);
                else VisualMap[ExtremePoint.X, ExtremePoint.Y] = new Point();
            }

            TryMoveCSP(Vector);
            ShowVisualMap();

            // extra write:
            ShowInfo_VisualMapContent();
            ShowInfo_Positions();
        }
        public static void ShowMapFragment(string type)
        {
            int LB = LeftBorder, RB = RightBorder, TB = TopBorder, BB = BotBorder;
            switch (type)
            {
                case "left": RB = RB / 5; break;
                case "right": LB = RB - RB / 5 - 1; break;
                case "center": LB = Console.BufferWidth / 2 - 20; RB = Console.BufferWidth / 2 + 19; break;
            }
            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                    WriteSingleField(new Point(x, y), LB, RB, TB, BB);
        }
        public static int GetFieldProp(string Type, bool Dragged = false, bool Base = false)
        {
            if (Base) switch (Type)
            {
                case "type": return BaseField.Type;
                case "category": return BaseField.Category;
                case "duration": return BaseField.Duration;
                default: return -100;
            }
            if (Dragged) switch (Type)
            {
                case "type": return DraggedField.Type;
                case "category": return DraggedField.Category;
                case "duration": return DraggedField.Duration;
                default: return -100;
            }
            if (SelectedFields.Count == 0) switch (Type)
            {
                case "type": return PhisicalMap[RealPos(VSP).X, RealPos(VSP).Y].Type;
                case "category": return PhisicalMap[RealPos(VSP).X, RealPos(VSP).Y].Category;
                case "duration": return PhisicalMap[RealPos(VSP).X, RealPos(VSP).Y].Duration;
                default: return -100;
            }
            switch (Type)
            {
                case "type": return PhisicalMap[SelectedFields[0].X, SelectedFields[0].Y].Type;
                case "category": return PhisicalMap[SelectedFields[0].X, SelectedFields[0].Y].Category;
                case "duration": return PhisicalMap[SelectedFields[0].X, SelectedFields[0].Y].Duration;
                default: return -100;
            }
        }
        public static void SetFieldProp(Point PhisicalPos, int c = -1, int t = -1, int d = -1, bool Dragged = false, bool Base = false)
        {
            if (Base)
            {
                if (PhisicalPos.X > 0 && PhisicalPos.Y > 0)
                {
                    PhisicalMap[PhisicalPos.X, PhisicalPos.Y].Category = BaseField.Category;
                    PhisicalMap[PhisicalPos.X, PhisicalPos.Y].Type = BaseField.Type;
                    PhisicalMap[PhisicalPos.X, PhisicalPos.Y].Duration = BaseField.Duration;
                }
                else BaseField = new FieldModel(c, t, d);
                return;
            }
            if (Dragged)
            {
                if (PhisicalPos.X > 0 && PhisicalPos.Y > 0)
                {
                    DraggedPosition = new Point(PhisicalPos.X, PhisicalPos.Y);
                    DraggedField = new FieldModel(PhisicalMap[PhisicalPos.X, PhisicalPos.Y]);
                }
                else DraggedField = null;
                return;
            }
            if (c >= 0) PhisicalMap[PhisicalPos.X, PhisicalPos.Y].Category = c;
            if (t >= 0) PhisicalMap[PhisicalPos.X, PhisicalPos.Y].Type = t;
            if (d >= 0) PhisicalMap[PhisicalPos.X, PhisicalPos.Y].Duration = d;
        }
        public static int[,,] GetMap()
        {
            for (int i = 1; i < PhisicalMapSize; i++)
                for (int j = 1; j < PhisicalMapSize; j++)
                {
                    SimpleMap[i - 1, j - 1, 0] = PhisicalMap[i, j].Category;
                    SimpleMap[i - 1, j - 1, 1] = PhisicalMap[i, j].Type;
                    SimpleMap[i - 1, j - 1, 2] = PhisicalMap[i, j].Duration;
                }
            return SimpleMap;
        }
        public static void SetMap(int[,,] Map)
        {
            SimpleMap = Map;
        }
        #endregion

        #region PROTECTED MANAGEMENT
        protected static void SetFieldsData()
        {
            char[] MyChar = { '´', '\r', '\n' };
            List<string>[] singleLineViews = XF.GetFields();
            FieldViews = new List<List<string>>[singleLineViews.Length];
            FSP = new List<List<int>>[singleLineViews.Length];
            for (int k = 0; k < singleLineViews.Length; k++) // kazda kategoria
            {
                FieldViews[k] = new List<List<string>>();
                FSP[k] = new List<List<int>>();
                for (int i = 0; i < singleLineViews[k].Count; i++) // kazde pole w danej kategorii
                {
                    List<string> _FieldView = new List<string>();
                    List<int> _FieldStartPos = new List<int>();
                    string[] singleFieldView = singleLineViews[k][i].Split('@');
                    for (int j = 0; j < singleFieldView.Length - 1; j++) // kazda grafika danego pola
                    {
                        int countChar = 0;
                        string line = singleFieldView[j].Trim(MyChar).Substring(4);
                        while (line[countChar] == '´') countChar++;
                        _FieldStartPos.Add(countChar);
                        _FieldView.Add(line.TrimStart(MyChar));
                    }
                    FieldViews[k].Add(_FieldView);
                    FSP[k].Add(_FieldStartPos);
                }
            }
        }
        protected static void SetMapBorders()
        {
            TopBorder = 4;
            BotBorder = Console.WindowHeight - 3;
            LeftBorder = 0;
            RightBorder = Console.WindowWidth - 1;
        }
        protected static void InitializePhisicalMap()
        {
            PhisicalMapSize = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(SimpleMap.Length / 3))) + 1;
            PhisicalMapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 - PhisicalMapSize * 3 + 1);
            PhisicalMap = new FieldModel[PhisicalMapSize, PhisicalMapSize];
            PhisicalMap[0, 0] = new FieldModel(0, 0);
            PhisicalMap[0, 1] = new FieldModel(0, 1);
            for (int x = 0; x < PhisicalMapSize - 1; x++)
                for (int y = 0; y < PhisicalMapSize - 1; y++)
                    PhisicalMap[x+1, y+1] = new FieldModel(SimpleMap[x, y, 0], SimpleMap[x, y, 1], SimpleMap[x, y, 2]);
        }
        protected static void InitializeVisualMap()
        {
            VisualMapSize = ((Console.WindowHeight + 1) / 12 + (Console.WindowWidth + 45) / 50) * 2 + 1;
            VisualMapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 + 4 - VisualMapSize / 2 * 6);
            VisualMap = new Point[VisualMapSize, VisualMapSize];
            VSP = new Point((VisualMapSize) / 2, (VisualMapSize) / 2);
            SelectedFields = new List<Point>() { };
            DraggedField = null;
            int FrameSize = PhisicalMapSize + 1;
            Point FramePosition = new Point(PhisicalMapPosition.X, PhisicalMapPosition.Y - 3);
            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                {
                    Point VisualCoord = GetCoordByPos(new Point(x, y), VisualMapPosition);
                    Point PhisicalPos = GetPosByCoord(VisualCoord, PhisicalMapPosition);
                    Point FramePos = GetPosByCoord(VisualCoord, FramePosition);

                    if (IsOnMap(PhisicalPos, PhisicalMapSize, 1))   VisualMap[x, y] = new Point(PhisicalPos.X, PhisicalPos.Y);
                    else if (IsOnMap(FramePos, FrameSize))          VisualMap[x, y] = new Point(0, 1);
                }
        }
        protected static void InitializeMapExtremePoints()
        {
            MapExtremePoints = new List<Point>();
            for (int x = 0; x < VisualMapSize; x++) MapExtremePoints.Add(new Point(x, 0));
            for (int x = 0; x < VisualMapSize; x++) MapExtremePoints.Add(new Point(x, VisualMapSize - 1));
            for (int y = 1; y < VisualMapSize - 1; y++) MapExtremePoints.Add(new Point(0, y));
            for (int y = 1; y < VisualMapSize - 1; y++) MapExtremePoints.Add(new Point(VisualMapSize - 1, y));
        }
        protected static void ShowVisualMap()
        {   
            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                    WriteSingleField(new Point(x,y));
        }
        protected static void ClearVisualMap()
        {
            string space = "".PadRight(Console.WindowWidth, ' ');
            for (int i = TopBorder + 1; i < BotBorder; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(space);
            }
        }
        protected static void ShowVisualField(Point VisualPoint, bool Above = false)
        {
            int start = 3;
            if (Above) start = 0;
            for (int i = start; i < 9; i++)
            {
                Point Border = new Point(VisualMapPosition.X + (VisualPoint.Y - VisualPoint.X) * 12, VisualMapPosition.Y + (VisualPoint.Y + VisualPoint.X) * 3);
                switch (i)
                {
                    case 0: VisualPoint.X--; VisualPoint.Y--; WriteSingleField(VisualPoint); VisualPoint.X++; break;
                    case 1: WriteSingleField(VisualPoint, LB: Border.X + 12); VisualPoint.X--; VisualPoint.Y++; break;
                    case 2: WriteSingleField(VisualPoint, RB: Border.X + 9); VisualPoint.X++; break;
                    case 3: WriteSingleField(VisualPoint); VisualPoint.X++; break;
                    case 4: WriteSingleField(VisualPoint, LB: Border.X + 12, BB: Border.Y + 4); VisualPoint.X--; VisualPoint.Y++; break;
                    case 5: WriteSingleField(VisualPoint, RB: Border.X + 9, BB: Border.Y + 4); VisualPoint.X++; break;
                    case 6: WriteSingleField(VisualPoint, BB: Border.Y + 1); VisualPoint.X++; break;
                    case 7: WriteSingleField(VisualPoint, BB: Border.Y - 2); VisualPoint.X--; VisualPoint.Y++; break;
                    case 8: WriteSingleField(VisualPoint, BB: Border.Y - 2); break;
                }
                if (!IsOnMap(VisualPoint, VisualMapSize)) return;
                //Thread.Sleep(1000);
            }
        }
        protected static void ShowPhisicalField(Point PhisicalPos)
        {
            var coord = GetCoordByPos(PhisicalPos, PhisicalMapPosition);
            Point VisualPos = GetPosByCoord(coord, VisualMapPosition);
            if (IsOnMap(VisualPos, VisualMapSize)) ShowVisualField(VisualPos, true);
        }
        protected static Point RealPos(Point VisualPos)
        {
            return VisualMap[VisualPos.X, VisualPos.Y];
        }
        protected static Point GetPos(bool Dragged = false)
        {
            if (Dragged) return DraggedPosition;
            if (SelectedFields.Count > 0) return new Point(SelectedFields[0].X, SelectedFields[0].Y);
            else return RealPos(VSP);
        }
        protected static int GetSelectedFieldCount()
        {
            return SelectedFields.Count;
        }
        protected static void ClearSelected(int CountToRemove = 0)
        {
            if (CountToRemove == 0) CountToRemove = SelectedFields.Count;
            while (SelectedFields.Count > 0 && CountToRemove > 0)
            {
                CountToRemove--;
                var PhisicalPos = SelectedFields[0];
                SelectedFields.RemoveAt(0);
                ShowPhisicalField(PhisicalPos);
            }
        }
        protected static void CenterMapOnPos(Point ExpectedPos)
        {
            ClearVisualMap();
            VisualMap = new Point[VisualMapSize, VisualMapSize];
            VSP = new Point((VisualMapSize) / 2, (VisualMapSize) / 2);
            Point CenterPos = GetPosByCoord(GetCoordByPos(VSP, VisualMapPosition), PhisicalMapPosition);
            PhisicalMapPosition = GetCoordByPos(new Point(CenterPos.X - ExpectedPos.X, CenterPos.Y - ExpectedPos.Y), PhisicalMapPosition);

            int FrameSize = PhisicalMapSize + 1;
            Point FramePosition = new Point(PhisicalMapPosition.X, PhisicalMapPosition.Y - 3);
            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                {
                    Point VisualCoord = GetCoordByPos(new Point(x, y), VisualMapPosition);
                    Point PhisicalPos = GetPosByCoord(VisualCoord, PhisicalMapPosition);
                    Point FramePos = GetPosByCoord(VisualCoord, FramePosition);

                    if (IsOnMap(PhisicalPos, PhisicalMapSize, 1)) VisualMap[x, y] = new Point(PhisicalPos.X, PhisicalPos.Y);
                    else if (IsOnMap(FramePos, FrameSize)) VisualMap[x, y] = new Point(0, 1);
                }
            ShowVisualMap();
        }
        #endregion

        #region PRIVATE MANAGEMENT
        private static bool TryMoveCSP(Point Vector, bool Change = true)
        {
            if (IsOnMap(new Point(VSP.X + Vector.X, VSP.Y + Vector.Y), VisualMapSize))
            {
                if (Change)
                {
                    VSP.X += Vector.X;
                    VSP.Y += Vector.Y;
                }
                return true;
            }
            return false;
        }
        private static void WriteSingleField(Point VisualPoint, int LB = -1, int RB = 1000, int TB = -1, int BB = 1000)
        {
            if (TB < TopBorder) TB = TopBorder;
            if (BB < TopBorder) BB = TopBorder;
            if (LB < LeftBorder) LB = LeftBorder;
            if (RB < LeftBorder) RB = LeftBorder;

            if (TB > BotBorder) TB = BotBorder;
            if (BB > BotBorder) BB = BotBorder;
            if (LB >= RightBorder) LB = RightBorder;
            if (RB > RightBorder) RB = RightBorder;

            Point P = RealPos(VisualPoint);

            int X = VisualMapPosition.X + (VisualPoint.Y - VisualPoint.X) * 12;
            int Y = VisualMapPosition.Y + (VisualPoint.Y + VisualPoint.X) * 3;
            int K = PhisicalMap[P.X, P.Y].Category;
            int T = PhisicalMap[P.X, P.Y].Type;
            int H = FSP[K][T].Count;
            int C = StandardFieldHeight - H;

            if (VSP == VisualPoint)
            {
                if (DraggedField != null)
                {
                    Console.ForegroundColor = GetFieldColor(-1, 0);
                    K = DraggedField.Category;
                    T = DraggedField.Type;
                    H = FSP[K][T].Count;
                    C = StandardFieldHeight - H;
                }
                else Console.ForegroundColor = GetFieldColor(-1, 1);
            }
            else if (SelectedFields.Contains(P)) Console.ForegroundColor = GetFieldColor(-1, 0);
            else Console.ForegroundColor = GetFieldColor(K, T);

            int startIndex, lenght, left;
            for (int i = 0; i < H; i++)
                if (Y + i + C > TB && BB > Y + i + C && X + FSP[K][T][i] + FieldViews[K][T][i].Length > LB && RB > X + FSP[K][T][i])
                {
                    startIndex = X + FSP[K][T][i] < LB ? LB - X - FSP[K][T][i] : 0;
                    left = X + startIndex + FSP[K][T][i];
                    lenght = FieldViews[K][T][i].Length - startIndex > RB - left + 1 ? RB - left + 1 : FieldViews[K][T][i].Length - startIndex;
                    Console.SetCursorPosition(left, Y + i + C);
                    Console.Write(FieldViews[K][T][i].Substring(startIndex, lenght));
                }
            Console.ResetColor();
        }
        private static bool IsOnMap(Point MapPos, int MapSize, int StartFrom = 0)
        {
            if (MapPos.X >= StartFrom && MapPos.Y >= StartFrom && MapPos.X < MapSize && MapPos.Y < MapSize) return true;
            else return false;
        }
        private static Point GetCoordByPos(Point MapPos, Point StartPos)
        {
            return new Point(StartPos.X + (MapPos.Y - MapPos.X) * 12, StartPos.Y + (MapPos.Y + MapPos.X) * 3);
        }
        private static Point GetPosByCoord(Point MapCoord, Point StartPos)
        {
            return new Point((4 * (MapCoord.Y - StartPos.Y) + StartPos.X - MapCoord.X) / 24, (4 * (MapCoord.Y - StartPos.Y) + MapCoord.X - StartPos.X) / 24);
        }
        private static ConsoleColor GetFieldColor(int Category, int Type)
        {
            switch (Category)
            {
                case -1:
                    switch (Type) // kolory selekcyjne
                    {
                        case 0: return ConsoleColor.Yellow; // SelectedFieldColor
                        case 1: return ConsoleColor.Magenta; // CurrentStandFieldColor
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 0: // pola nieużytkowe
                    switch (Type)
                    {
                        case 0: return ConsoleColor.White; // air field
                        case 1: return ConsoleColor.White; // black field
                        case 2: return ConsoleColor.White; // pointer field
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 1: // pola nieużytkowe
                    switch (Type)
                    {
                        case 0: return ConsoleColor.White; // air
                        case 1: return ConsoleColor.White; // empty field
                        case 2: return ConsoleColor.Green; // grass
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 2:  // pola uprawne
                    switch (Type)
                    {
                        case 0: return ConsoleColor.DarkRed; // zaorane pole
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 3:  // pola posiane
                    switch (Type)
                    {
                        case 0: return ConsoleColor.DarkGreen; // przenica
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 4:  // pola rosnące
                    switch (Type)
                    {
                        case 0: return ConsoleColor.DarkYellow; // przenica
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 5:  // pola dojrzałe
                    switch (Type)
                    {
                        case 0: return ConsoleColor.Yellow; // przenica
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 6:  // pola zgniłe
                    switch (Type)
                    {
                        case 0: return ConsoleColor.DarkYellow; // przenica
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 7:  // budynki użytkowe
                    switch (Type)
                    {
                        case 0: return ConsoleColor.Gray; // dom
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 8:  // dekoracje statyczne
                    switch (Type)
                    {
                        case 0: return ConsoleColor.DarkGray; // silos
                        case 1: return ConsoleColor.DarkGray; // wieza
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 9:  // maszyny rolne
                    switch (Type)
                    {
                        case 0: return ConsoleColor.DarkCyan; // traktor
                        default: return ConsoleColor.Magenta; // undefined
                    }
                default: return ConsoleColor.Magenta;
            }
        }
        private static void ShowInfo_FieldsID()
        {
            for (int i = 0; i < PhisicalMapSize; i++)
                for (int j = 0; j < PhisicalMapSize; j++)
                    if (PhisicalMapPosition.X + 17 + (j - i) * 12 < RightBorder && PhisicalMapPosition.X + 10 + (j - i) * 12 > LeftBorder
                        && PhisicalMapPosition.Y + 3 + (j + i) * 3 < BotBorder && PhisicalMapPosition.Y + 3 + (j + i) * 3 > TopBorder)
                    {
                        //Console.SetCursorPosition(MapPosition.X + 10 + (j - i) * 12, MapPosition.Y + 3 + (j + i) * 3);
                        //Console.Write(" " + i + "," + j + " ");

                        //Console.SetCursorPosition(PhisicalMap[i, j].X, PhisicalMap[i, j].Y);
                        //Console.Write(" " + PhisicalMap[i, j].X + "," + PhisicalMap[i, j].Y + " ");
                    }
        }
        private static void ShowInfo_VisualMapContent()
        {
            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                {
                    Console.SetCursorPosition(14 + 5 * x, 6 + 2 * y);
                    if (VisualMap[x, y].X == 0 && VisualMap[x, y].Y == 0) Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(VisualMap[x, y].X + "," + VisualMap[x, y].Y + "  ");
                    Console.ResetColor();
                }
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(14 + 5 * VSP.X, 6 + 2 * VSP.Y);
            Console.Write(VSP.X + "," + VSP.Y + " ");
            Console.ResetColor();
        }
        private static void ShowInfo_Positions()
        {
            Point vmp = GetPosByCoord(PhisicalMapPosition, VisualMapPosition);
            Console.SetCursorPosition(2, 6); Console.Write("PMP:[" + vmp.X + "," + vmp.Y + "]  ");
            Console.SetCursorPosition(2, 7); Console.Write("PSP:[" + RealPos(VSP).X + "," + RealPos(VSP).Y + "]  ");
            Console.SetCursorPosition(2, 8); Console.Write("VSP:[" + VSP.X + "," + VSP.Y + "]  ");
            Console.SetCursorPosition(2, 9);
            if (DraggedField != null) Console.Write("D:[" + DraggedPosition.X + "," + DraggedPosition.Y + "]  ");
            else Console.Write("           ");

        }
        #endregion
    }
}
