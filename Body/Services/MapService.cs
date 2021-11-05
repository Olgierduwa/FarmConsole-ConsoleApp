using FarmConsole.Body.Models;
using Pastel;
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
        private static Point VSP;                           // Visual Stand Position //
        private static Point DraggedPosition;               // Pozycja pochwyconego pola z Fizycznej Mapy
        private static FieldModel BaseField;                // Pole Podłoża
        private static FieldModel DraggedField;             // Pochwycone Pole
        private static int TopBorder;
        private static int BotBorder;
        private static int LeftBorder;
        private static int RightBorder;
        private static List<Point> PhisicalMapExtremePoints;
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
                if (PhisicalMap[RealPos(FSP).X, RealPos(FSP).Y].Category == 3) return;
                else FixAbove = true;
            else if (Shift)
            {
                Point StandPos = RealPos(VSP);
                if (SelectedFields.Contains(StandPos)) SelectedFields.Remove(StandPos);
                else if (SelectedFields.Count == 0 ||
                    GetField().Category == PhisicalMap[StandPos.X, StandPos.Y].Category &&
                    GetField().Type == PhisicalMap[StandPos.X, StandPos.Y].Type) SelectedFields.Add(StandPos);
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
            foreach (Point ExtremePoint in PhisicalMapExtremePoints)
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
            //ShowInfo_VisualMapContent();
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
        public static int GetSelectedFieldCount()
        {
            return SelectedFields.Count;
        }
        public static FieldModel[,] GetMap()
        {
            FieldModel[,] Map = new FieldModel[PhisicalMapSize - 1, PhisicalMapSize - 1];
            for (int i = 0; i < PhisicalMapSize - 1; i++)
                for (int j = 0; j < PhisicalMapSize - 1; j++)
                    Map[i, j] = PhisicalMap[i + 1, j + 1];
            return Map;
        }
        public static FieldModel GetField(string FieldType = "")
        {
            switch(FieldType)
            {
                case "Base": return BaseField;
                case "Dragged": return DraggedField;
                default:
                {
                    if (SelectedFields.Count == 0) return PhisicalMap[RealPos(VSP).X, RealPos(VSP).Y];
                    else return PhisicalMap[SelectedFields[0].X, SelectedFields[0].Y];
                }
            }
        }

        public static void SetColors()
        {
            for (int i = 1; i < PhisicalMapSize; i++)
                for (int j = 1; j < PhisicalMapSize; j++)
                    PhisicalMap[i, j].Color = ProductModel.GetProduct(PhisicalMap[i, j]).Color;
        }
        #endregion

        #region PROTECTED MANAGEMENT
        protected static void SetField(Point PhisicalPos, FieldModel NewField, string FieldType = "")
        {
            switch (FieldType)
            {
                case "Base":
                    if (PhisicalPos.X > 0 && PhisicalPos.Y > 0)
                        PhisicalMap[PhisicalPos.X, PhisicalPos.Y] = BaseField;
                    else BaseField = new FieldModel(NewField); break;
                case "Dragged":
                    if (PhisicalPos.X > 0 && PhisicalPos.Y > 0)
                    {
                        DraggedPosition = new Point(PhisicalPos.X, PhisicalPos.Y);
                        DraggedField = new FieldModel(PhisicalMap[PhisicalPos.X, PhisicalPos.Y]);
                    }
                    else DraggedField = null; break;
                default: if (NewField != null) PhisicalMap[PhisicalPos.X, PhisicalPos.Y] = NewField; break;
            }
        }
        protected static void SetMapBorders()
        {
            TopBorder = 4;
            BotBorder = Console.WindowHeight - 3;
            LeftBorder = 0;
            RightBorder = Console.WindowWidth - 1;
        }
        protected static void InitializePhisicalMap(FieldModel[,] Map)
        {
            PhisicalMapSize = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(Map.Length))) + 1;
            PhisicalMapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 - PhisicalMapSize * 3 + 1);
            PhisicalMap = new FieldModel[PhisicalMapSize, PhisicalMapSize];
            PhisicalMap[0, 0] = new FieldModel(ProductModel.GetProduct("emptiness"));
            PhisicalMap[0, 1] = new FieldModel(ProductModel.GetProduct("rubber"));
            for (int x = 0; x < PhisicalMapSize - 1; x++)
                for (int y = 0; y < PhisicalMapSize - 1; y++)
                    PhisicalMap[x + 1, y + 1] = new FieldModel(Map[x, y]);
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
            PhisicalMapExtremePoints = new List<Point>();
            for (int x = 0; x < VisualMapSize; x++) PhisicalMapExtremePoints.Add(new Point(x, 0));
            for (int x = 0; x < VisualMapSize; x++) PhisicalMapExtremePoints.Add(new Point(x, VisualMapSize - 1));
            for (int y = 1; y < VisualMapSize - 1; y++) PhisicalMapExtremePoints.Add(new Point(0, y));
            for (int y = 1; y < VisualMapSize - 1; y++) PhisicalMapExtremePoints.Add(new Point(VisualMapSize - 1, y));
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
                    case 1: WriteSingleField(VisualPoint, LB: Border.X + 11); VisualPoint.X--; VisualPoint.Y++; break;
                    case 2: WriteSingleField(VisualPoint, RB: Border.X + 11); VisualPoint.X++; break;
                    case 3: WriteSingleField(VisualPoint); VisualPoint.X++; break;
                    case 4: WriteSingleField(VisualPoint, LB: Border.X + 11, BB: Border.Y + 4); VisualPoint.X--; VisualPoint.Y++; break;
                    case 5: WriteSingleField(VisualPoint, RB: Border.X + 11, BB: Border.Y + 4); VisualPoint.X++; break;
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
            TB = TB < TopBorder ? TopBorder : TB > BotBorder ? BotBorder : TB;
            BB = BB < TopBorder ? TopBorder : BB > BotBorder ? BotBorder : BB;
            RB = RB > RightBorder ? RightBorder : RB < LeftBorder ? LeftBorder : RB;
            LB = LB > RightBorder ? RightBorder : LB < LeftBorder ? LeftBorder : LB;

            Point P = RealPos(VisualPoint);
            ProductModel Product = ProductModel.GetProduct(PhisicalMap[P.X, P.Y]);
            Color color;

            if (VSP == VisualPoint)
            {
                if (DraggedField != null)
                {
                    Product = ProductModel.GetProduct(DraggedField);
                    color = ColorService.GetColorByName("selected");
                }
                else color = ColorService.Brighten(PhisicalMap[P.X, P.Y].Color);
            }
            else if (SelectedFields.Contains(P)) color = ColorService.GetColorByName("selected");
            else color = PhisicalMap[P.X, P.Y].Color;

            int X = VisualMapPosition.X + (VisualPoint.Y - VisualPoint.X) * 12;
            int Y = VisualMapPosition.Y + (VisualPoint.Y + VisualPoint.X) * 3;
            int C = StandardFieldHeight - Product.View.Length;

            int startIndex, lenght, left;
            for (int Line = 0; Line < Product.View.Length; Line++)
                if (Y + Line + C > TB && BB > Y + Line + C &&
                    X + Product.ViewStartPos[Line] + Product.View[Line].Length > LB && RB >= X + Product.ViewStartPos[Line])
                {
                    startIndex = X + Product.ViewStartPos[Line] < LB ? LB - X - Product.ViewStartPos[Line] : 0;
                    left = X + startIndex + Product.ViewStartPos[Line];
                    lenght = Product.View[Line].Length - startIndex > RB - left + 1 ?
                        RB - left + 1 : Product.View[Line].Length - startIndex;
                    Console.SetCursorPosition(left, Y + Line + C);
                    Console.Write(Product.View[Line].Substring(startIndex, lenght).Pastel(color));
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
