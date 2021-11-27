using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using Pastel;
using System;
using System.Collections.Generic;
using System.Drawing;

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
        private static List<Point> PhisicalMapExtremePoints;
        private static Point[,] VisualMap;
        private static Point VisualMapPosition;
        private static int VisualMapSize;
        private static MapModel PhisicalMap;
        #endregion


        #region PUBLIC MENEGEMENT
        public static void MoveStandPosition(Point Vector = new Point(), bool Shift = false)
        {
            if (!TryMoveCSP(Vector, false)) return;
            bool FixAbove = false;
            Point FSP = new Point(PhisicalMap.StandPosition.X + Vector.X, PhisicalMap.StandPosition.Y + Vector.Y); // future stand point
            if (FSP.X >= 0 && FSP.X < VisualMapSize && FSP.Y >= 0 && FSP.Y < VisualMapSize && VisualMap[FSP.X, FSP.Y].X == 0) return;
            if (DraggedField != null)
                if (PhisicalMap.Fields[RealPos(FSP).X, RealPos(FSP).Y].Category == 3) return;
                else FixAbove = true;
            else if (Shift)
            {
                Point StandPos = RealPos(PhisicalMap.StandPosition);
                if (SelectedFields.Contains(StandPos)) SelectedFields.Remove(StandPos);
                else if (SelectedFields.Count == 0 ||
                    GetField().Category == PhisicalMap.Fields[StandPos.X, StandPos.Y].Category &&
                    GetField().Type == PhisicalMap.Fields[StandPos.X, StandPos.Y].Type) SelectedFields.Add(StandPos);
            }
            Point PSP = new Point(PhisicalMap.StandPosition.X, PhisicalMap.StandPosition.Y); // past stand point
            TryMoveCSP(Vector);
            ShowVisualField(PSP, FixAbove);
            ShowVisualField(PhisicalMap.StandPosition);

            // extra write:
            //ShowInfo_VisualMapContent();
            ShowInfo_Positions();
        }
        public static void MoveMapPosition(Point Vector)
        {
            if (!TryMoveCSP(Vector, false)) return;
            PhisicalMap.MapPosition.X += (Vector.Y - Vector.X) * 12;
            PhisicalMap.MapPosition.Y += (Vector.Y + Vector.X) * 3;
            int x_beg = 0, y_beg = 0, x_dif = 1, y_dif = 1, x_end = VisualMapSize - 1, y_end = VisualMapSize - 1;
            if (Vector.X > 0) { x_beg = x_end; x_end = 0; x_dif = -1; }
            if (Vector.Y > 0) { y_beg = y_end; y_end = 0; y_dif = -1; }

            for (int x = x_beg; x != x_end; x += x_dif)
                for (int y = y_beg; y != y_end; y += y_dif)
                    VisualMap[x, y] = VisualMap[x + x_dif, y + y_dif];

            foreach (Point ExtremePoint in PhisicalMapExtremePoints)
            {
                Point VisualCoord = GetCoordByPos(new Point(ExtremePoint.X, ExtremePoint.Y), VisualMapPosition);
                Point PhisicalPos = GetPosByCoord(VisualCoord, PhisicalMap.MapPosition);

                if (IsOnMap(PhisicalPos, PhisicalMap.Size, 1)) VisualMap[ExtremePoint.X, ExtremePoint.Y] = new Point(PhisicalPos.X, PhisicalPos.Y);
                else VisualMap[ExtremePoint.X, ExtremePoint.Y] = new Point();
            }

            TryMoveCSP(Vector);
            ShowVisualMap();

            // extra write:
            //ShowInfo_VisualMapContent();
            //ShowInfo_Positions();
        }
        public static void ShowMapFragment(Point Position, Size Size)
        {
            if (Size.Height > 0 && Size.Width > 0)
                for (int x = 0; x < VisualMapSize; x++)
                    for (int y = 0; y < VisualMapSize; y++)
                        WriteSingleField(new Point(x, y), Position.X, Position.X + Size.Width, Position.Y, Position.Y + Size.Height);
        }
        public static int GetSelectedFieldCount()
        {
            return SelectedFields.Count;
        }
        public static FieldModel GetField(string FieldType = "")
        {
            switch(FieldType)
            {
                case "Base": return PhisicalMap.BaseField;
                case "Dragged": return DraggedField;
                case "Stand": return PhisicalMap.Fields[RealPos(PhisicalMap.StandPosition).X, RealPos(PhisicalMap.StandPosition).Y];
                default:
                {
                    if (SelectedFields.Count == 0) return PhisicalMap.Fields[RealPos(PhisicalMap.StandPosition).X, RealPos(PhisicalMap.StandPosition).Y];
                    else return PhisicalMap.Fields[SelectedFields[0].X, SelectedFields[0].Y];
                }
            }
        }
        public static MapModel Map
        {
            set
            {
                if (value == null) PhisicalMap = null;
                else PhisicalMap = value.Copy();
            }
            get
            {
                if (PhisicalMap != null) return PhisicalMap.Copy();
                else return null;
            }
        }
        public static void ReloadMap()
        {
            ProductModel P = ProductModel.GetProduct(PhisicalMap.BaseField);
            PhisicalMap.BaseField.Color = P.Color;
            PhisicalMap.BaseField.View = P.View;
            PhisicalMap.BaseField.ViewStartPos = P.ViewStartPos;

            for (int x = 0; x < PhisicalMap.Size; x++)
                for (int y = 0; y < PhisicalMap.Size; y++)
                {
                    P = ProductModel.GetProduct(PhisicalMap.Fields[x, y]);
                    PhisicalMap.Fields[x, y].Color = P.Color;
                    PhisicalMap.Fields[x, y].View = P.View;
                    PhisicalMap.Fields[x, y].ViewStartPos = P.ViewStartPos;
                }

            ShowVisualMap();
        }
        #endregion


        #region PROTECTED MANAGEMENT
        protected static void SetField(Point PhisicalPos, FieldModel NewField, string FieldType = "")
        {
            switch (FieldType)
            {
                case "Base": PhisicalMap.Fields[PhisicalPos.X, PhisicalPos.Y] = PhisicalMap.BaseField; break;
                case "Dragged":
                    if (PhisicalPos.X > 0 && PhisicalPos.Y > 0)
                    {
                        DraggedPosition = new Point(PhisicalPos.X, PhisicalPos.Y);
                        DraggedField = new FieldModel(PhisicalMap.Fields[PhisicalPos.X, PhisicalPos.Y]);
                    }
                    else DraggedField = null; break;
                default: if (NewField != null) PhisicalMap.Fields[PhisicalPos.X, PhisicalPos.Y] = NewField; break;
            }
        }
        protected static void SetMapBorders()
        {
            TopBorder = 5;
            BotBorder = Console.WindowHeight - 4;
            LeftBorder = 0;
            RightBorder = Console.WindowWidth - 1;
        }
        protected static void InitVisualMap()
        {
            VisualMapSize = ((Console.WindowHeight + 1) / 12 + (Console.WindowWidth + 45) / 50) * 2 + 1;
            VisualMapPosition = new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 + 4 - VisualMapSize / 2 * 6);
            VisualMap = new Point[VisualMapSize, VisualMapSize];
            SelectedFields = new List<Point>() { };
            DraggedField = null;

            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                {
                    Point PhisicalPos = GetPosByCoord(GetCoordByPos(new Point(x, y), VisualMapPosition), PhisicalMap.MapPosition);
                    if (IsOnMap(PhisicalPos, PhisicalMap.Size, 1)) VisualMap[x, y] = new Point(PhisicalPos.X, PhisicalPos.Y);
                }
        }
        protected static void InitMapExtremePoints()
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
        protected static void DarkerMap(int procent)
        {
            PhisicalMap.BaseField.Color = ColorService.Darker(PhisicalMap.BaseField.Color, procent);
            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                {
                    Point p = RealPos(new Point(x, y));
                    PhisicalMap.Fields[p.X, p.Y].Color = ColorService.Darker(PhisicalMap.Fields[p.X, p.Y].Color, procent);
                }
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
            int start = 0;
            if (Above == false || VisualPoint.X == 0 || VisualPoint.Y == 0) start = 3;
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
            var coord = GetCoordByPos(PhisicalPos, PhisicalMap.MapPosition);
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
            else return RealPos(PhisicalMap.StandPosition);
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
            PhisicalMap.StandPosition = new Point((VisualMapSize) / 2, (VisualMapSize) / 2);
            Point CenterPos = GetPosByCoord(GetCoordByPos(PhisicalMap.StandPosition, VisualMapPosition), PhisicalMap.MapPosition);
            PhisicalMap.MapPosition = GetCoordByPos(new Point(CenterPos.X - ExpectedPos.X, CenterPos.Y - ExpectedPos.Y), PhisicalMap.MapPosition);

            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                {
                    Point VisualCoord = GetCoordByPos(new Point(x, y), VisualMapPosition);
                    Point PhisicalPos = GetPosByCoord(VisualCoord, PhisicalMap.MapPosition);
                    if (IsOnMap(PhisicalPos, PhisicalMap.Size, 1)) VisualMap[x, y] = new Point(PhisicalPos.X, PhisicalPos.Y);
                }

            ShowVisualMap();
        }
        protected static Point GetCoordByPos(Point MapPos, Point StartPos)
        {
            return new Point(StartPos.X + (MapPos.Y - MapPos.X) * 12, StartPos.Y + (MapPos.Y + MapPos.X) * 3);
        }
        protected static Point GetPosByCoord(Point MapCoord, Point StartPos)
        {
            return new Point((4 * (MapCoord.Y - StartPos.Y) + StartPos.X - MapCoord.X) / 24, (4 * (MapCoord.Y - StartPos.Y) + MapCoord.X - StartPos.X) / 24);
        }
        #endregion


        #region PRIVATE MANAGEMENT
        private static bool TryMoveCSP(Point Vector, bool Change = true)
        {
            if (IsOnMap(new Point(PhisicalMap.StandPosition.X + Vector.X, PhisicalMap.StandPosition.Y + Vector.Y), VisualMapSize))
            {
                string State = ProductModel.GetProduct(GetField()).StateName;
                if (State.Length > 1 && State[0] == '-')
                {
                    if (Change)
                    {
                        PhisicalMap.StandPosition.X += Vector.X;
                        PhisicalMap.StandPosition.Y += Vector.Y;
                    }


                    return true;
                }

                if (Change)
                {
                    PhisicalMap.StandPosition.X += Vector.X;
                    PhisicalMap.StandPosition.Y += Vector.Y;
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

            Color color;
            Point P = RealPos(VisualPoint);
            List<FieldModel> Fields = new List<FieldModel>() { PhisicalMap.Fields[P.X, P.Y] };
            if (!Fields[0].Fill) Fields.Insert(0, PhisicalMap.BaseField);
            foreach (FieldModel Field in Fields)
            {
                FieldModel F = Field;
                if (PhisicalMap.StandPosition == VisualPoint)
                {
                    if (DraggedField != null) { F = DraggedField; color = ColorService.GetColorByName("selected"); }
                    else color = ColorService.Brighter(F.Color);
                }
                else if (SelectedFields.Contains(P)) color = ColorService.GetColorByName("selected");
                else color = F.Color;

                int X = VisualMapPosition.X + (VisualPoint.Y - VisualPoint.X) * 12;
                int Y = VisualMapPosition.Y + (VisualPoint.Y + VisualPoint.X) * 3;
                int C = StandardFieldHeight - F.View.Length;

                int startIndex, lenght, left;
                for (int Line = 0; Line < F.View.Length; Line++)
                    if (Y + Line + C >= TB && BB >= Y + Line + C &&
                        X + F.ViewStartPos[Line] + F.View[Line].Length > LB && RB >= X + F.ViewStartPos[Line])
                    {
                        startIndex = X + F.ViewStartPos[Line] < LB ? LB - X - F.ViewStartPos[Line] : 0;
                        left = X + startIndex + F.ViewStartPos[Line];
                        lenght = F.View[Line].Length - startIndex > RB - left + 1 ?
                            RB - left + 1 : F.View[Line].Length - startIndex;

                        string LineString = F.View[Line].Substring(startIndex, lenght);
                        if (!LineString.Contains('´'))
                        {
                            Console.SetCursorPosition(left, Y + Line + C);
                            Console.Write(LineString.Pastel(color));
                        }
                        else if(LineString.Split('´', 1).Length < 3)
                        {
                            Console.SetCursorPosition(left, Y + Line + C);
                            Console.Write(LineString.Split('´')[0].Pastel(color));
                            if(LineString.Split('´', 1).Length == 2)
                            {
                                int padding = LineString.Split('´')[0].Length + LineString.Split('´').Length - 1;
                                Console.SetCursorPosition(left + padding, Y + Line + C);
                                Console.Write(LineString.Split('´')[^1].Pastel(color));
                            }
                        }
                    }
                Console.ResetColor();
            }
        }
        private static bool IsOnMap(Point MapPos, int MapSize, int StartFrom = 0)
        {
            if (MapPos.X >= StartFrom && MapPos.Y >= StartFrom && MapPos.X < MapSize && MapPos.Y < MapSize) return true;
            else return false;
        }
        private static void ShowInfo_FieldsID()
        {
            for (int i = 0; i < PhisicalMap.Size; i++)
                for (int j = 0; j < PhisicalMap.Size; j++)
                    if (PhisicalMap.MapPosition.X + 17 + (j - i) * 12 < RightBorder && PhisicalMap.MapPosition.X + 10 + (j - i) * 12 > LeftBorder
                        && PhisicalMap.MapPosition.Y + 3 + (j + i) * 3 < BotBorder && PhisicalMap.MapPosition.Y + 3 + (j + i) * 3 > TopBorder)
                    {
                        //Console.SetCursorPosition(MapPosition.X + 10 + (j - i) * 12, MapPosition.Y + 3 + (j + i) * 3);
                        //Console.Write(" " + i + "," + j + " ");

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
            Point vmp = GetPosByCoord(PhisicalMap.MapPosition, VisualMapPosition);
            Console.SetCursorPosition(2, 6); Console.Write("PMP:[" + vmp.X + "," + vmp.Y + "]  ");
            Console.SetCursorPosition(2, 7); Console.Write("PSP:[" + RealPos(PhisicalMap.StandPosition).X + "," + RealPos(PhisicalMap.StandPosition).Y + "]  ");
            Console.SetCursorPosition(2, 8); Console.Write("VSP:[" + PhisicalMap.StandPosition.X + "," + PhisicalMap.StandPosition.Y + "]  ");
            Console.SetCursorPosition(2, 9);
            if (DraggedField != null) Console.Write("D:[" + DraggedPosition.X + "," + DraggedPosition.Y + "]  ");
            else Console.Write("           ");

        }
        #endregion
    }
}
