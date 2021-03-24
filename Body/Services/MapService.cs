using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Services
{
    public class MapService
    {
        protected static readonly int StandardFieldHeight = 7;
        protected static readonly int BorderTop = 4;
        protected static readonly int BorderBottom = Console.WindowHeight - 3;
        protected static readonly int BorderLeft = 0;
        protected static readonly int BorderRight = Console.WindowWidth;

        protected static List<List<int>>[] FieldStartPos;
        protected static List<List<string>>[] FieldViews;
        protected static List<Point> SelectedFields;
        protected static List<Point> CheckFields;
        protected static Point StandPosition;
        protected static Point MapPosition;
        protected static Point DraggPosition;
        private static FieldModel DraggedField;
        private static FieldModel[,] PhisicalMap;
        protected static int[,,] SimpleMap;
        protected static int FarmSize;
        
        #region FIELD MENEGEMENT
        public static bool MoveStandPosition(Point beforePos, Point currentPos, bool shift)
        {
            StandPosition.X = currentPos.X;
            StandPosition.Y = currentPos.Y;

            Console.SetCursorPosition(2, 6);
            Console.Write("[" + StandPosition.X + "," + StandPosition.Y + "]");
            //Modified = true;
            if (DraggedField != null)
            {
                if (PhisicalMap[currentPos.X, currentPos.Y].Category > 5)
                {
                    StandPosition.X = beforePos.X;
                    StandPosition.Y = beforePos.Y;
                    return false;
                }
                CompleteSurroundings(beforePos);
            }
            else if (shift == true)
            {
                if (SelectedFields.Contains(beforePos) == true) SelectedFields.Remove(beforePos);
                else if (SelectedFields.Count == 0) { SelectedFields.Add(beforePos); }
                else if (PhisicalMap[SelectedFields[0].X, SelectedFields[0].Y].Category == PhisicalMap[beforePos.X, beforePos.Y].Category
                    && PhisicalMap[beforePos.X, beforePos.Y].Category < 6) SelectedFields.Add(beforePos);
            }
            ConsiderHeight(beforePos, currentPos);
            return true;
        }
        public static int GetFieldCategory(bool stand = false)
        {
            if (stand || SelectedFields == null || SelectedFields.Count == 0)
                return PhisicalMap[StandPosition.X, StandPosition.Y].Category;
            else return PhisicalMap[SelectedFields[0].X, SelectedFields[0].Y].Category;
        }
        public static int GetFieldType(bool stand = false)
        {
            if (stand || SelectedFields == null || SelectedFields.Count == 0)
                return PhisicalMap[StandPosition.X, StandPosition.Y].Type;
            else return PhisicalMap[SelectedFields[0].X, SelectedFields[0].Y].Type;
        }
        public static int GetFieldDuration(bool stand = false)
        {
            if (stand || SelectedFields == null || SelectedFields.Count == 0)
                return PhisicalMap[StandPosition.X, StandPosition.Y].Duration;
            else return PhisicalMap[SelectedFields[0].X, SelectedFields[0].Y].Duration;
        }
        public static Point GetSelectedPosition(bool stand = false)
        {
            if (!stand && SelectedFields.Count > 0) return new Point(SelectedFields[0].X, SelectedFields[0].Y);
            else return StandPosition;
        }
        #endregion

        #region MAP MANAGEMENT
        public static int[,,] GetMap()
        {
            for (int i = 2; i < FarmSize - 1; i++)
                for (int j = 2; j < FarmSize - 1; j++)
                {
                    SimpleMap[i - 2, j - 2, 0] = PhisicalMap[i, j].Category;
                    SimpleMap[i - 2, j - 2, 1] = PhisicalMap[i, j].Type;
                    SimpleMap[i - 2, j - 2, 2] = PhisicalMap[i, j].Duration;
                }
            return SimpleMap;
        }
        public static void MoveMapPosition(Point vector)
        {
            MapPosition.X += vector.X * 24;
            MapPosition.Y += vector.Y * 6;

            for (int i = 0; i < FarmSize; i++)
                for (int j = 0; j < FarmSize; j++)
                    PhisicalMap[i, j].Move(new Point(vector.X * 24, vector.Y * 6));

            ShowMap();
        }
        public static void ShowMapFragment(string type, bool menu1 = false, bool menu2 = false)
        {
            int CW = Console.WindowWidth;
            int leftBarrier;
            int rightBarrier;
            switch (type)
            {
                case "left":
                    {
                        leftBarrier = BorderLeft;
                        rightBarrier = CW / 5 + 1;
                        for (int i = 2; i < FarmSize - 1; i++)
                            for (int j = 2; j < FarmSize - 1; j++)
                                ShowSingleField(new Point(i, j), false, leftBarrier, rightBarrier);

                        rightBarrier = BorderRight;
                        if (menu1) rightBarrier = CW - CW / 5 - 2;
                        ConsiderHeight(StandPosition, StandPosition, leftBarrier, rightBarrier);
                    }
                    break;

                case "right":
                    {
                        rightBarrier = BorderRight;
                        leftBarrier = CW - CW / 5 - 2;
                        for (int i = 2; i < FarmSize - 1; i++)
                            for (int j = 2; j < FarmSize - 1; j++)
                                ShowSingleField(new Point(i, j), false, leftBarrier, rightBarrier);

                        leftBarrier = BorderLeft;
                        if (menu1) leftBarrier = CW / 5 + 1;
                        ConsiderHeight(StandPosition, StandPosition, leftBarrier, rightBarrier);
                    }
                    break;

                case "center":
                    {
                        leftBarrier = Console.WindowWidth / 2 - 20;
                        rightBarrier = Console.WindowWidth / 2 + 20;
                        for (int i = 2; i < FarmSize - 1; i++)
                            for (int j = 2; j < FarmSize - 1; j++)
                                ShowSingleField(new Point(i, j), false, leftBarrier, rightBarrier);

                        rightBarrier = BorderRight;
                        leftBarrier = BorderLeft;
                        if (menu2) leftBarrier = CW / 5 + 1;
                        if (menu1) rightBarrier = CW - CW / 5 - 2;
                        ConsiderHeight(StandPosition, StandPosition, leftBarrier, rightBarrier);
                    }
                    break;

                case "all":
                    {
                        leftBarrier = BorderLeft;
                        rightBarrier = CW / 5 + 1;
                        for (int i = 2; i < FarmSize - 1; i++)
                            for (int j = 2; j < FarmSize - 1; j++)
                                ShowSingleField(new Point(i, j), false, leftBarrier, rightBarrier);

                        rightBarrier = BorderRight;
                        leftBarrier = CW - CW / 5 - 2;
                        for (int i = 2; i < FarmSize - 1; i++)
                            for (int j = 2; j < FarmSize - 1; j++)
                                ShowSingleField(new Point(i, j), false, leftBarrier, rightBarrier);

                        leftBarrier = Console.WindowWidth / 2 - 20;
                        rightBarrier = Console.WindowWidth / 2 + 20;
                        for (int i = 2; i < FarmSize - 1; i++)
                            for (int j = 2; j < FarmSize - 1; j++)
                                ShowSingleField(new Point(i, j), false, leftBarrier, rightBarrier);

                        ConsiderHeight(StandPosition, StandPosition);
                    }
                    break;
            }
        }
        protected static void ShowMap()
        {
            for (int i = 0; i < FarmSize; i++)
                for (int j = 0; j < FarmSize; j++)
                    ShowSingleField(new Point(i, j));
            MoveStandPosition(StandPosition, StandPosition, false);
        }
        public static void ClearMap()
        {
            string space = "".PadRight(Console.WindowWidth, ' ');
            for (int i = BorderTop + 1; i < BorderBottom; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(space);
            }
        }
        #endregion

        #region PRIVATE MANAGEMENT
        internal static void SetSelectedField(Point p, FieldModel f)
        {
            PhisicalMap[p.X, p.Y] = f;
        }
        internal static void SetSelectedFieldProp(Point p, int c = -1, int t = -1, int d = -1)
        {
            if (c>=0) PhisicalMap[p.X, p.Y].Category = c;
            if (t>=0) PhisicalMap[p.X, p.Y].Type = t;
            if (d>=0) PhisicalMap[p.X, p.Y].Duration = d;
        }
        internal static FieldModel GetSelectedField(Point p)
        {
            return PhisicalMap[p.X, p.Y];
        }
        internal static void SetDraggedField(FieldModel f)
        {
            DraggedField = f;
        }
        internal static FieldModel GetDraggedField()
        {
            return DraggedField;
        }
        protected static ConsoleColor Color(int category, int type)
        {
            switch (category)
            {
                case -1:
                    switch (type) // kolory selekcyjne
                    {
                        case 0: return ConsoleColor.Yellow; // SelectedColor
                        case 1: return ConsoleColor.White; // CurrentFieldColor
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 0: // pola nieużytkowe
                    switch (type)
                    {
                        case 0: return ConsoleColor.White; // air
                        case 1: return ConsoleColor.White; // empty field
                        case 2: return ConsoleColor.Green; // grass
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 1:  // pola uprawne
                    switch (type)
                    {
                        case 0: return ConsoleColor.DarkRed; // zaorane pole
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 2:  // pola posiane
                    switch (type)
                    {
                        case 0: return ConsoleColor.DarkGreen; // przenica
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 3:  // pola rosnące
                    switch (type)
                    {
                        case 0: return ConsoleColor.DarkYellow; // przenica
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 4:  // pola dojrzałe
                    switch (type)
                    {
                        case 0: return ConsoleColor.Yellow; // przenica
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 5:  // pola zgniłe
                    switch (type)
                    {
                        case 0: return ConsoleColor.DarkYellow; // przenica
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 6:  // budynki użytkowe
                    switch (type)
                    {
                        case 0: return ConsoleColor.Gray; // dom
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 7:  // dekoracje statyczne
                    switch (type)
                    {
                        case 0: return ConsoleColor.DarkGray; // silos
                        case 1: return ConsoleColor.DarkGray; // wieza
                        default: return ConsoleColor.Magenta; // undefined
                    }
                case 8:  // maszyny rolne
                    switch (type)
                    {
                        case 0: return ConsoleColor.DarkCyan; // traktor
                        default: return ConsoleColor.Magenta; // undefined
                    }
                default: return ConsoleColor.Magenta;
            }
        }
        protected static void InitializeMap()
        {
            DraggedField = null;
            CheckFields = new List<Point>() { };
            SelectedFields = new List<Point>() { };

            PhisicalMap = new FieldModel[FarmSize, FarmSize];
            for (int i = 0; i < FarmSize; i++)
                for (int j = 0; j < FarmSize; j++)
                    if (i < 2 || j < 2 || i > FarmSize - 2 || j > FarmSize - 2)
                        PhisicalMap[i, j] = new FieldModel(MapPosition.X + (j - i) * 12, MapPosition.Y + (j + i) * 3, 0, 0);
                    else PhisicalMap[i, j] = new FieldModel(MapPosition.X + (j - i) * 12, MapPosition.Y + (j + i) * 3, SimpleMap[i - 2, j - 2, 0], SimpleMap[i - 2, j - 2, 1], SimpleMap[i - 2, j - 2, 2]);
        }
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
                        string line = singleFieldView[j].Trim(MyChar).Substring(6);
                        while (line[countChar] == '´') countChar++;
                        _FieldStartPos.Add(countChar);
                        _FieldView.Add(line.TrimStart(MyChar));
                    }
                    FieldViews[k].Add(_FieldView);
                    FieldStartPos[k].Add(_FieldStartPos);
                }
            }
        }
        protected static void ShowSingleField(Point p, bool currentSelected = false, int leftBarrier = 0, int rightBarrier = 0)
        {
            if (leftBarrier == 0) leftBarrier = BorderLeft;
            if (rightBarrier == 0) rightBarrier = BorderRight;

            int X = PhisicalMap[p.X, p.Y].X;
            int Y = PhisicalMap[p.X, p.Y].Y;
            int K = PhisicalMap[p.X, p.Y].Category;
            int T = PhisicalMap[p.X, p.Y].Type;
            int H = FieldStartPos[K][T].Count;
            int C = StandardFieldHeight - H;

            Console.ForegroundColor = Color(PhisicalMap[p.X, p.Y].Category, PhisicalMap[p.X, p.Y].Type);
            if (SelectedFields.Contains(p)) Console.ForegroundColor = Color(-1, 0);
            if (currentSelected == true)
            {
                Console.ForegroundColor = Color(-1, 1);
                if (DraggedField != null)
                {
                    Console.ForegroundColor = Color(-1, 0);
                    K = DraggedField.Category;
                    T = DraggedField.Type;
                    H = FieldStartPos[K][T].Count;
                    C = StandardFieldHeight - H;
                }
            }

            for (int i = 0; i < H; i++)
            {
                if (Y + i + C > BorderTop && BorderBottom > Y + i + C)
                {
                    if (X + FieldStartPos[K][T][i] < leftBarrier)
                    {
                        if (X + FieldStartPos[K][T][i] + FieldViews[K][T][i].Length > leftBarrier)
                        {
                            Console.SetCursorPosition(leftBarrier, Y + i + C);
                            Console.Write(FieldViews[K][T][i].Substring(leftBarrier - X - FieldStartPos[K][T][i]));
                        }
                    }
                    else if (X + FieldStartPos[K][T][i] + FieldViews[K][T][i].Length > rightBarrier)
                    {
                        if (X + FieldStartPos[K][T][i] < rightBarrier)
                        {
                            Console.SetCursorPosition(X + FieldStartPos[K][T][i], Y + i + C);
                            Console.Write(FieldViews[K][T][i].Substring(0, rightBarrier - X - FieldStartPos[K][T][i]));
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
        protected static void CompleteSurroundings(Point point)
        {
            ShowSingleField(new Point(point.X - 1, point.Y - 1), false);
            ShowSingleField(new Point(point.X - 1, point.Y), false);
            ShowSingleField(new Point(point.X, point.Y - 1), false);
            ConsiderHeight(new Point(point.X - 1, point.Y - 1), StandPosition);
            ConsiderHeight(new Point(point.X - 1, point.Y), StandPosition);
            ConsiderHeight(new Point(point.X, point.Y - 1), StandPosition);
        }
        protected static void ConsiderHeight(Point beforePos, Point currentPos, int leftBarrier = 0, int rightBarrier = 0)
        {
            if (beforePos.Y > currentPos.Y || beforePos.X > currentPos.X)
            { FindFieldToOverride(currentPos); FindFieldToOverride(beforePos); }
            else { FindFieldToOverride(beforePos); FindFieldToOverride(currentPos); }

            ShowSingleField(beforePos, false, leftBarrier, rightBarrier);
            CheckFields.Remove(currentPos);
            ShowSingleField(currentPos, true, leftBarrier, rightBarrier);
            for (int i = 0; i < CheckFields.Count; i++) ShowSingleField(CheckFields[i], false, leftBarrier, rightBarrier);
            CheckFields = new List<Point>() { };
        }
        protected static void FindFieldToOverride(Point p)
        {
            //Thread.Sleep(1000);
            //ShowCheckList();
            int i = 0;
            p.X += 1;
            if (FieldViews[PhisicalMap[p.X, p.Y].Category][PhisicalMap[p.X, p.Y].Type].Count > StandardFieldHeight && CheckFields.Contains(p) == false)
            {
                while (i < CheckFields.Count && CheckFields[i].Y < p.Y) i++;
                while (i < CheckFields.Count && CheckFields[i].X < p.X) i++;
                CheckFields.Insert(i, p);
                FindFieldToOverride(p);
            }

            p.X -= 1;
            p.Y += 1;
            if (FieldViews[PhisicalMap[p.X, p.Y].Category][PhisicalMap[p.X, p.Y].Type].Count > StandardFieldHeight && CheckFields.Contains(p) == false)
            {
                while (i < CheckFields.Count && CheckFields[i].Y < p.Y) i++;
                while (i < CheckFields.Count && CheckFields[i].X < p.X) i++;
                CheckFields.Insert(i, p);
                FindFieldToOverride(p);
            }

            p.X += 1;
            if (FieldViews[PhisicalMap[p.X, p.Y].Category][PhisicalMap[p.X, p.Y].Type].Count > StandardFieldHeight && CheckFields.Contains(p) == false)
            {
                while (i < CheckFields.Count && CheckFields[i].Y < p.Y) i++;
                while (i < CheckFields.Count && CheckFields[i].X < p.X) i++;
                CheckFields.Insert(i, p);
                FindFieldToOverride(p);
            }
        }
        protected static void ClearSelected()
        {
            if (SelectedFields.Count > 0)
            {
                while (SelectedFields.Count > 0)
                {
                    var point = SelectedFields[0];
                    SelectedFields.RemoveAt(0);
                    ConsiderHeight(new Point(point.X, point.Y), StandPosition);
                }
            }
        }
        public static void ShowFieldID()
        {
            for (int i = 0; i < FarmSize; i++)
                for (int j = 0; j < FarmSize; j++)
                    if (MapPosition.X + 17 + (j - i) * 12 < BorderRight && MapPosition.X + 10 + (j - i) * 12 > BorderLeft
                        && MapPosition.Y + 3 + (j + i) * 3 < BorderBottom && MapPosition.Y + 3 + (j + i) * 3 > BorderTop)
                    {
                        Console.SetCursorPosition(MapPosition.X + 10 + (j - i) * 12, MapPosition.Y + 3 + (j + i) * 3);
                        Console.Write(" " + i + "," + j + " ");
                    }
        }
        protected static void ShowCheckList()
        {
            Console.SetCursorPosition(14, 5);
            Console.Write("CheckList: ");
            foreach (Point p in CheckFields)
                Console.Write("[" + p.X + "," + p.Y + "] ");
        }
        #endregion
    }
}
