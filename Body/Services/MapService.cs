using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace FarmConsole.Body.Services
{
    public class MapService
    {
        protected static readonly int StandardFieldHeight = 7;

        private static List<List<string>>[] FieldViews;
        private static List<List<int>>[] FieldStartPos;
        protected static List<Point> SelectedFields;        // Trzyma X,Y zaznaczonych pól Fizycznej Mapy
        protected static Point CSP;                         // Pozycja stania na Wizualnej Mapie // Current Stand Position //
        protected static Point DraggedPosition;             // Pozycja pochwyconego pola z Fizycznej Mapy
        private static FieldModel DraggedField;             // Pochwycone Pole

        // prosta mapa pozwalająca na zapis i odczyt
        protected static int[,,] SimpleMap;

        // Pełna Mapa składająca się z pełnych pól
        private static FieldModel[,] PhisicalMap;
        protected static Point PhisicalMapPosition;
        protected static int PhisicalMapSize;

        // Praktyczna Mapa składająca się ze wskazników do pełnej mapy
        private static Point[,] VisualMap;
        protected static Point VisualMapPosition;
        protected static int VisualMapSize;
        
        private static int TopBorder;
        private static int BotBorder;
        private static int LeftBorder;
        private static int RightBorder;

        #region PUBLIC MENEGEMENT
        public static void MoveStandPosition(Point Vector = new Point(), bool Shift = false)
        {
            bool FixAbove = false;
            Point FSP = new Point(CSP.X + Vector.X, CSP.Y + Vector.Y); // future stand point
            if (FSP.X > 0 && FSP.X < VisualMapSize && FSP.Y >= 0 && FSP.Y < VisualMapSize && VisualMap[FSP.X, FSP.Y].X == 0) return;
            if (DraggedField != null)
                if (PhisicalMap[RealPos(FSP).X, RealPos(FSP).Y].Category > 6) return;
                else FixAbove = true;
            else if (Shift)
            {
                Point StandPos = RealPos(CSP);
                if (SelectedFields.Contains(StandPos)) SelectedFields.Remove(StandPos);
                else if (SelectedFields.Count == 0 || GetFieldCategory() == PhisicalMap[StandPos.X, StandPos.Y].Category) SelectedFields.Add(StandPos);
            }
            CSP.X += Vector.X;
            CSP.Y += Vector.Y;
            Point PSP = new Point(CSP.X - Vector.X, CSP.Y - Vector.Y); // past stand point
            ShowVisualField(PSP, FixAbove);
            ShowVisualField(CSP);

            // extra write:
            Console.SetCursorPosition(2, 6); Console.Write("[" + RealPos(CSP).X + "," + RealPos(CSP).Y + "]");
        }
        public static void MoveMapPosition(Point vector)
        {
            VisualMapPosition.X += vector.X * 24;
            VisualMapPosition.Y += vector.Y * 6;
            ShowMap();

            //for (int i = 0; i < VisualMapPosition; i++)
            //    for (int j = 0; j < VisualMapPosition; j++) ;
            //        PhisicalMap[i, j].Move(new Point(vector.X * 24, vector.Y * 6));

            //MoveStandPosition();
            ShowMapTable();
        }
        #endregion

        #region MANAGEMENT OF SIDE MENU
          public static void ShowMapFragment(string type, bool menu1 = false, bool menu2 = false)
        {
            int CW = Console.WindowWidth;
            int leftBarrier;
            int rightBarrier;
            switch (type)
            {
                case "left":
                    {
                        leftBarrier = LeftBorder;
                        rightBarrier = CW / 5 + 1;
                        for (int i = 2; i < PhisicalMapSize - 1; i++)
                            for (int j = 2; j < PhisicalMapSize - 1; j++)
                                WriteSingleField(new Point(i, j), leftBarrier, rightBarrier);
                    }
                    break;

                case "right":
                    {
                        rightBarrier = RightBorder;
                        leftBarrier = CW - CW / 5 - 2;
                        for (int i = 2; i < PhisicalMapSize - 1; i++)
                            for (int j = 2; j < PhisicalMapSize - 1; j++)
                                WriteSingleField(new Point(i, j), leftBarrier, rightBarrier);
                    }
                    break;

                case "center":
                    {
                        leftBarrier = Console.WindowWidth / 2 - 20;
                        rightBarrier = Console.WindowWidth / 2 + 20;
                        for (int i = 2; i < PhisicalMapSize - 1; i++)
                            for (int j = 2; j < PhisicalMapSize - 1; j++)
                                WriteSingleField(new Point(i, j), leftBarrier, rightBarrier);
                    }
                    break;

                case "all":
                    {
                        leftBarrier = LeftBorder;
                        rightBarrier = CW / 5 + 1;
                        for (int i = 2; i < PhisicalMapSize - 1; i++)
                            for (int j = 2; j < PhisicalMapSize - 1; j++)
                                WriteSingleField(new Point(i, j), leftBarrier, rightBarrier);

                        rightBarrier = RightBorder;
                        leftBarrier = CW - CW / 5 - 2;
                        for (int i = 2; i < PhisicalMapSize - 1; i++)
                            for (int j = 2; j < PhisicalMapSize - 1; j++)
                                WriteSingleField(new Point(i, j), leftBarrier, rightBarrier);

                        leftBarrier = Console.WindowWidth / 2 - 20;
                        rightBarrier = Console.WindowWidth / 2 + 20;
                        for (int i = 2; i < PhisicalMapSize - 1; i++)
                            for (int j = 2; j < PhisicalMapSize - 1; j++)
                                WriteSingleField(new Point(i, j), leftBarrier, rightBarrier);
                    }
                    break;
            }
        }
        public static int GetFieldCategory(bool stand = false)
        {
            if (stand || SelectedFields == null || SelectedFields.Count == 0)
                return PhisicalMap[RealPos(CSP).X, RealPos(CSP).Y].Category;
            else return PhisicalMap[SelectedFields[0].X, SelectedFields[0].Y].Category;
        }
        public static int GetFieldType(bool stand = false)
        {
            if (stand || SelectedFields == null || SelectedFields.Count == 0)
                return PhisicalMap[RealPos(CSP).X, RealPos(CSP).Y].Type;
            else return PhisicalMap[SelectedFields[0].X, SelectedFields[0].Y].Type;
        }
        public static int GetFieldDuration(bool stand = false)
        {
            if (stand || SelectedFields == null || SelectedFields.Count == 0)
                return PhisicalMap[RealPos(CSP).X, RealPos(CSP).Y].Duration;
            else return PhisicalMap[SelectedFields[0].X, SelectedFields[0].Y].Duration;
        }
        #endregion

        #region MANAGEMENT OF FIELD PROPERTY
        internal static void SetPhisicalField(Point PhisicalPos, FieldModel f)
        {
            PhisicalMap[PhisicalPos.X, PhisicalPos.Y] = f;
        }
        protected static void SetSelectedFieldProp(Point PhisicalPos, int c = -1, int t = -1, int d = -1)
        {
            if (c >= 0) PhisicalMap[PhisicalPos.X, PhisicalPos.Y].Category = c;
            if (t >= 0) PhisicalMap[PhisicalPos.X, PhisicalPos.Y].Type = t;
            if (d >= 0) PhisicalMap[PhisicalPos.X, PhisicalPos.Y].Duration = d;
        }
        internal static void SetDraggedField(Point PhisicalPos)
        {
            if (PhisicalPos.X > 0 && PhisicalPos.Y > 0)
            {
                DraggedPosition = new Point(PhisicalPos.X, PhisicalPos.Y);
                DraggedField = new FieldModel(GetSelectedField(PhisicalPos));
            }
            else DraggedField = null;
        }
        internal static FieldModel GetDraggedField()
        {
            return DraggedField;
        }
        internal static FieldModel GetSelectedField(Point PhisicalPos)
        {
            return PhisicalMap[PhisicalPos.X, PhisicalPos.Y];
        }
        protected static Point GetSelectedPosition(bool stand = false)
        {
            if (!stand && SelectedFields.Count > 0) return new Point(SelectedFields[0].X, SelectedFields[0].Y);
            else return RealPos(CSP);
        }
        protected static void ClearSelected()
        {
            while (SelectedFields.Count > 0)
            {
                var PhisicalPos = SelectedFields[0];
                SelectedFields.RemoveAt(0);
                ShowPhisicalField(PhisicalPos);
            }
        }
        #endregion

        #region MANAGEMENT OF MAP EXISTENCE
        protected static void GetFieldsData()
        {
            char[] MyChar = { '´', '\r', '\n' };
            List<string>[] singleLineViews = XF.GetFields();
            FieldViews = new List<List<string>>[singleLineViews.Length];
            FieldStartPos = new List<List<int>>[singleLineViews.Length];
            for (int k = 0; k < singleLineViews.Length; k++) // kazda kategoria
            {
                FieldViews[k] = new List<List<string>>();
                FieldStartPos[k] = new List<List<int>>();
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
                    FieldStartPos[k].Add(_FieldStartPos);
                }
            }
        }
        protected static void SetMapBorders()
        {
            TopBorder = 4;
            BotBorder = Console.WindowHeight - 3;
            LeftBorder = 0;
            RightBorder = Console.WindowWidth;
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
            CSP = new Point((VisualMapSize) / 2, (VisualMapSize) / 2);
            SelectedFields = new List<Point>() { };
            DraggedField = null;
            int FrameSize = PhisicalMapSize + 1;
            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                {
                    Point VisualCoord = GetCoord(new Point(x, y), VisualMapPosition);
                    Point FramePos = GetMapPos(VisualCoord, new Point(Console.WindowWidth / 2 - 12, (Console.WindowHeight - 8) / 2 - FrameSize * 3 + 1));
                    Point PhisicalPos = GetMapPos(VisualCoord, PhisicalMapPosition);

                    if (IsOnMap(PhisicalPos, PhisicalMapSize, 1)) VisualMap[x, y] = new Point(PhisicalPos.X, PhisicalPos.Y);
                    else if (IsOnMap(FramePos, FrameSize))
                        VisualMap[x, y] = new Point(0, 1);
                }
        }
        protected static void ShowMap()
        {   
            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                    WriteSingleField(new Point(x,y));
        }
        protected static void ClearMap()
        {
            string space = "".PadRight(Console.WindowWidth, ' ');
            for (int i = TopBorder + 1; i < BotBorder; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(space);
            }
        }
        public static int[,,] GetMap()
        {
            for (int i = 2; i < PhisicalMapSize - 1; i++)
                for (int j = 2; j < PhisicalMapSize - 1; j++)
                {
                    SimpleMap[i - 2, j - 2, 0] = PhisicalMap[i, j].Category;
                    SimpleMap[i - 2, j - 2, 1] = PhisicalMap[i, j].Type;
                    SimpleMap[i - 2, j - 2, 2] = PhisicalMap[i, j].Duration;
                }
            return SimpleMap;
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
                //Thread.Sleep(1000);
            }
        }
        protected static void ShowPhisicalField(Point PhisicalPos)
        {
            Point VisualPos = GetMapPos(GetCoord(PhisicalPos, PhisicalMapPosition), VisualMapPosition);
            if (IsOnMap(VisualPos, VisualMapSize))
                ShowVisualField(VisualPos, true);
        }
        protected static Point RealPos(Point VisualPos)
        {
            return VisualMap[VisualPos.X, VisualPos.Y];
        }
        #endregion

        #region PRIVATE MANAGEMENT
        private static void WriteSingleField(Point VisualPoint, int LB = -1, int RB = 1000, int TB = -1, int BB = 1000)
        {
            if (TB < 0) TB = TopBorder;
            if (LB < 0) LB = LeftBorder;
            if (BB > BotBorder) BB = BotBorder;
            if (RB > RightBorder) RB = RightBorder;

            Point P = RealPos(VisualPoint);

            int X = VisualMapPosition.X + (VisualPoint.Y - VisualPoint.X) * 12;
            int Y = VisualMapPosition.Y + (VisualPoint.Y + VisualPoint.X) * 3;
            int K = PhisicalMap[P.X, P.Y].Category;
            int T = PhisicalMap[P.X, P.Y].Type;
            int H = FieldStartPos[K][T].Count;
            int C = StandardFieldHeight - H;

            if (CSP == VisualPoint)
            {
                if (DraggedField != null)
                {
                    Console.ForegroundColor = GetFieldColor(-1, 0);
                    K = DraggedField.Category;
                    T = DraggedField.Type;
                    H = FieldStartPos[K][T].Count;
                    C = StandardFieldHeight - H;
                }
                else Console.ForegroundColor = GetFieldColor(-1, 1);
            }
            else if (SelectedFields.Contains(P)) Console.ForegroundColor = GetFieldColor(-1, 0);
            else Console.ForegroundColor = GetFieldColor(K, T);

            for (int i = 0; i < H; i++)
            {
                if (Y + i + C > TB && BB > Y + i + C)
                {
                    if (X + FieldStartPos[K][T][i] < LB)
                    {
                        if (X + FieldStartPos[K][T][i] + FieldViews[K][T][i].Length > LB)
                        {
                            Console.SetCursorPosition(LB, Y + i + C);
                            Console.Write(FieldViews[K][T][i].Substring(LB - X - FieldStartPos[K][T][i]));
                        }
                    }
                    else if (X + FieldStartPos[K][T][i] + FieldViews[K][T][i].Length > RB)
                    {
                        if (X + FieldStartPos[K][T][i] < RB)
                        {
                            Console.SetCursorPosition(X + FieldStartPos[K][T][i], Y + i + C);
                            Console.Write(FieldViews[K][T][i].Substring(0, RB - X - FieldStartPos[K][T][i]));
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(X + FieldStartPos[K][T][i], Y + i + C);
                        Console.Write(FieldViews[K][T][i]);
                    }
                }
            }
            Console.ResetColor();
        }
        private static ConsoleColor GetFieldColor(int category, int type)
        {
            switch (category)
            {
                case -1:
                    switch (type) // kolory selekcyjne
                    {
                        case 0: return ConsoleColor.Yellow; // SelectedFieldColor
                        case 1: return ConsoleColor.Magenta; // CurrentStandFieldColor
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 0: // pola nieużytkowe
                    switch (type)
                    {
                        case 0: return ConsoleColor.White; // air field
                        case 1: return ConsoleColor.White; // black field
                        case 2: return ConsoleColor.White; // pointer field
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 1: // pola nieużytkowe
                    switch (type)
                    {
                        case 0: return ConsoleColor.White; // air
                        case 1: return ConsoleColor.White; // empty field
                        case 2: return ConsoleColor.Green; // grass
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 2:  // pola uprawne
                    switch (type)
                    {
                        case 0: return ConsoleColor.DarkRed; // zaorane pole
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 3:  // pola posiane
                    switch (type)
                    {
                        case 0: return ConsoleColor.DarkGreen; // przenica
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 4:  // pola rosnące
                    switch (type)
                    {
                        case 0: return ConsoleColor.DarkYellow; // przenica
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 5:  // pola dojrzałe
                    switch (type)
                    {
                        case 0: return ConsoleColor.Yellow; // przenica
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 6:  // pola zgniłe
                    switch (type)
                    {
                        case 0: return ConsoleColor.DarkYellow; // przenica
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 7:  // budynki użytkowe
                    switch (type)
                    {
                        case 0: return ConsoleColor.Gray; // dom
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 8:  // dekoracje statyczne
                    switch (type)
                    {
                        case 0: return ConsoleColor.DarkGray; // silos
                        case 1: return ConsoleColor.DarkGray; // wieza
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 9:  // maszyny rolne
                    switch (type)
                    {
                        case 0: return ConsoleColor.DarkCyan; // traktor
                        default: return ConsoleColor.Magenta; // undefined
                    }
                default: return ConsoleColor.Magenta;
            }
        }
        private static bool IsOnMap(Point MapPos, int MapSize, int StartFrom = 0)
        {
            if (MapPos.X >= StartFrom && MapPos.Y >= StartFrom && MapPos.X < MapSize && MapPos.Y < MapSize) return true;
            else return false;
        }
        private static Point GetCoord(Point MapPos, Point StartPos)
        {
            return new Point(StartPos.X + (MapPos.Y - MapPos.X) * 12, StartPos.Y + (MapPos.Y + MapPos.X) * 3);
        }
        private static Point GetMapPos(Point Coord, Point StartPos)
        {
            return new Point((4 * (Coord.Y - StartPos.Y) + StartPos.X - Coord.X) / 24, (4 * (Coord.Y - StartPos.Y) + Coord.X - StartPos.X) / 24);
        }
        private static void ShowMapTable()
        {
            for (int x = 0; x < VisualMapSize; x++)
                for (int y = 0; y < VisualMapSize; y++)
                {
                    Console.SetCursorPosition(2 + 6 * x, 3 + y * 2);
                    if (VisualMap[x, y].X == 0 && VisualMap[x, y].Y == 0) Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(VisualMap[x, y].X + "," + VisualMap[x, y].Y + "  ");
                    Console.ResetColor();
                }
        }
        public static void ShowFieldID()
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
        #endregion
    }
}
