using FarmConsole.Body.Model.Helpers;
using FarmConsole.Body.Model.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace FarmConsole.Body.View.Components
{
    class FieldsMenager
    {
        private List<int>[] FieldStartPos;
        private List<string>[] FieldView;

        private readonly int FieldHeigth = 7;
        private readonly int BorderTop = 4;
        private readonly int BorderBottom = Console.WindowHeight - 3;
        private readonly int BorderLeft = 0;
        private readonly int BorderRight = Console.WindowWidth;

        private int StartX;
        private int StartY;
        private int FarmSize;
        private Point[,] SimpleMap;
        private Field[,] PhisicalMap;
        private List<Point> CheckList = new List<Point>() { };

        public ConsoleColor color(string type)
        {
            switch(type)
            {
                case "1":
                case "CurrentFieldColor": return ConsoleColor.White;

                case "2":
                case "SelectedColor": return ConsoleColor.Yellow;

                case "10":
                case "DefaultColor": return ConsoleColor.Green;

                case "11":
                case "House": return ConsoleColor.Gray;

                case "12":
                case "Silos": return ConsoleColor.DarkGray;

                default: return ConsoleColor.White;
            }
        }
        public FieldsMenager(Point[,] MapFromSave)
        {
            FarmSize = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(MapFromSave.Length))) + 3;
            StartX = Console.WindowWidth / 2 - 12;
            StartY = 5;

            this.SimpleMap = MapFromSave;
            GetFieldsData();
            InitializeMap();
            ShowFarm();
        }
        public Point[,] GetMap()
        {
            for (int i = 2; i < FarmSize-1; i++)
                for (int j = 2; j < FarmSize-1; j++)
                    SimpleMap[i-2, j-2] = new Point(PhisicalMap[i, j].Type, PhisicalMap[i, j].Duration);
            return SimpleMap;
        }
        private void InitializeMap()
        {
            PhisicalMap = new Field[FarmSize, FarmSize];
            for (int i = 0; i < FarmSize; i++)
                for (int j = 0; j < FarmSize; j++)
                    if (i < 2 || j < 2 || i > FarmSize - 2 || j > FarmSize - 2)
                         PhisicalMap[i, j] = new Field(StartX + ((j - i) * 12), StartY + ((j + i) * 3), 0);
                    else PhisicalMap[i, j] = new Field(StartX + ((j - i) * 12), StartY + ((j + i) * 3), SimpleMap[i-2, j-2].X, SimpleMap[i-2,j-2].Y);
        }
        private void GetFieldsData()
        {
            char[] MyChar = { '´', '\r','\n' };
            string[] fieldViewsInLines = XF.GetFields();
            FieldView = new List<string>[fieldViewsInLines.Length];
            FieldStartPos = new List<int>[fieldViewsInLines.Length];
            for (int i = 0; i < fieldViewsInLines.Length; i++)
            {
                string[] singleFieldView = fieldViewsInLines[i].Split('@');
                FieldView[i] = new List<string>();
                FieldStartPos[i] = new List<int>();
                for (int j = 0; j < singleFieldView.Length-1; j++)
                {
                    int countChar = 0;
                    string line = singleFieldView[j].Trim(MyChar).Substring(4);
                    while (line[countChar] == '´') countChar++;
                    FieldStartPos[i].Add(countChar);
                    FieldView[i].Add(line.TrimStart(MyChar));
                }
            }
        }

        private void ShowFarm()
        {
            for (int i = 0; i < FarmSize; i++)
                for (int j = 0; j < FarmSize; j++)
                    ShowField(new Point(i, j));
        }
        public void ClearFarm()
        {
            string space = ("").PadRight(Console.WindowWidth, ' ');
            Console.SetCursorPosition(0, BorderTop + 1);
            for (int i = BorderTop + 1; i < BorderBottom; i++) Console.WriteLine(space);
        }
        public void MoveFarm(Point vector)
        {
            StartX += vector.X * 24;
            StartY += vector.Y * 6;
            InitializeMap();
            ShowFarm();
        }

        private void ShowField(Point p,bool currentSelected = false)
        {
            int X = PhisicalMap[p.X, p.Y].X;
            int Y = PhisicalMap[p.X, p.Y].Y;
            int T = PhisicalMap[p.X, p.Y].Type;
            int H = FieldStartPos[T].Count;
            int C = FieldHeigth - H;

            Console.ForegroundColor = color(PhisicalMap[p.X, p.Y].Type.ToString());
            if (PhisicalMap[p.X, p.Y].Selected == true) Console.ForegroundColor = color("SelectedColor");
            if (currentSelected == true) Console.ForegroundColor = color("CurrentFieldColor");

            for (int i = 0; i < H; i++)
            {
                if (Y + i + C > BorderTop && BorderBottom > Y + i + C)
                {
                    if (X + FieldStartPos[T][i] < BorderLeft)
                    {
                        if (X + FieldStartPos[T][i] + FieldView[T][i].Length > BorderLeft)
                        {
                            Console.SetCursorPosition(BorderLeft, Y + i + C);
                            Console.Write(FieldView[T][i].Substring(BorderLeft - X - FieldStartPos[T][i]));
                        }
                    }
                    else if (X + FieldStartPos[T][i] + FieldView[T][i].Length > BorderRight)
                    {
                        if (X + FieldStartPos[T][i] < BorderRight)
                        {
                            Console.SetCursorPosition(X + FieldStartPos[T][i], Y + i + C);
                            Console.Write(FieldView[T][i].Substring(0, BorderRight - X - FieldStartPos[T][i]));
                        }
                    }
                    else
                    {
                        Console.SetCursorPosition(X + FieldStartPos[T][i], Y + i + C);
                        Console.Write(FieldView[T][i]);
                    }
                }
            }
            Console.ResetColor();
        }
        public void MoveSelect(Point beforePos, Point currentPos, bool shift)
        {
            beforePos.X++; beforePos.Y++;
            currentPos.X++; currentPos.Y++;
            //Modified = true;
            if (shift == true)
            {
                if (PhisicalMap[beforePos.X, beforePos.Y].Selected == true)
                    PhisicalMap[beforePos.X, beforePos.Y].Selected = false;
                else PhisicalMap[beforePos.X, beforePos.Y].Selected = true;
            }

            if (beforePos.Y > currentPos.Y || beforePos.X > currentPos.X)
                 { FindFieldToOverride(currentPos); FindFieldToOverride(beforePos); }
            else { FindFieldToOverride(beforePos); FindFieldToOverride(currentPos); }

            ShowField(beforePos);
            CheckList.Remove(currentPos);
            ShowField(currentPos, true);
            for(int i=0;i<CheckList.Count;i++) ShowField(CheckList[i]);
            CheckList = new List<Point>() { };
        }
        private void FindFieldToOverride(Point p)
        {
            int i = 0;
            p.X += 1;
            if (FieldView[PhisicalMap[p.X, p.Y].Type].Count > FieldHeigth && CheckList.Contains(p) == false)
            {
                while (i < CheckList.Count && CheckList[i].X < p.X) i++;
                while (i < CheckList.Count && CheckList[i].Y < p.Y) i++;
                CheckList.Insert(i, p);
                FindFieldToOverride(p);
            }

            p.X -= 1;
            p.Y += 1;
            if (FieldView[PhisicalMap[p.X, p.Y].Type].Count > FieldHeigth && CheckList.Contains(p) == false)
            {
                while (i < CheckList.Count && CheckList[i].X < p.X) i++;
                while (i < CheckList.Count && CheckList[i].Y < p.Y) i++;
                CheckList.Insert(i, p);
                FindFieldToOverride(p);
            }

            p.X += 1;
            if (FieldView[PhisicalMap[p.X, p.Y].Type].Count > FieldHeigth && CheckList.Contains(p) == false)
            {
                while (i < CheckList.Count && CheckList[i].X < p.X) i++;
                while (i < CheckList.Count && CheckList[i].Y < p.Y) i++;
                CheckList.Insert(i, p);
                FindFieldToOverride(p);
            }
        }
    }
}
