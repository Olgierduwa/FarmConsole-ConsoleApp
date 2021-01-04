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
		private List<List<int>>[] FieldStartPos;
		private List<List<string>>[] FieldView;

		private readonly int FieldHeigth = 7;
		private readonly int BorderTop = 4;
		private readonly int BorderBottom = Console.WindowHeight - 3;
		private readonly int BorderLeft = 0;
		private readonly int BorderRight = Console.WindowWidth;
		private readonly int FarmSize;

		private int StartX;
		private int StartY;
		private Point Position = new Point();
		private Field DraggedField = null;
		private int[,,] SimpleMap; // category / type / duration
		private Field[,] PhisicalMap;
		private List<Point> CheckList = new List<Point>() { };
		private List<Point> SelectedList = new List<Point>() { };

		public ConsoleColor Color(int category, int type)
		{
			switch(category)
			{
				case -1:
					switch(type) // kolory selekcyjne
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
		public FieldsMenager(int[,,] MapFromSave)
		{
			FarmSize = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(MapFromSave.Length/3))) + 3;
			StartX = Console.WindowWidth / 2 - 12;
			StartY = (Console.WindowHeight - 7 * FarmSize) / 2 + 2;

			this.SimpleMap = MapFromSave;
			GetFieldsData();
			InitializeMap();
			ShowFarm();
		}
		public int[,,] GetMap()
		{
			for (int i = 2; i < FarmSize-1; i++)
				for (int j = 2; j < FarmSize-1; j++)
				{
					SimpleMap[i-2, j-2, 0] = PhisicalMap[i, j].Category;
					SimpleMap[i-2, j-2, 1] = PhisicalMap[i, j].Type;
					SimpleMap[i-2, j-2, 2] = PhisicalMap[i, j].Duration;
				}
			return SimpleMap;
		}
		private void InitializeMap()
		{
			PhisicalMap = new Field[FarmSize, FarmSize];
			for (int i = 0; i < FarmSize; i++)
				for (int j = 0; j < FarmSize; j++)
					if (i < 2 || j < 2 || i > FarmSize - 2 || j > FarmSize - 2)
						PhisicalMap[i, j] = new Field(StartX + ((j - i) * 12), StartY + ((j + i) * 3), 0, 0);
					else PhisicalMap[i, j] = new Field(StartX + ((j - i) * 12), StartY + ((j + i) * 3), SimpleMap[i - 2, j - 2, 0], SimpleMap[i - 2, j - 2, 1], SimpleMap[i - 2, j - 2, 2]);
		}
		private void GetFieldsData()
		{
			char[] MyChar = { '´', '\r','\n' };
			List<string>[] singleLineViews = XF.GetFields();
			FieldView = new List<List<string>>[singleLineViews.Length];
			FieldStartPos = new List<List<int>>[singleLineViews.Length];
			for (int k = 0; k < singleLineViews.Length; k++) // kazda kategoria
			{
				FieldView[k] = new List<List<string>>();
				FieldStartPos[k] = new List<List<int>>();
				for (int i = 0; i < singleLineViews[k].Count; i++) // kazde pole w danej kategorii
				{

					List<string> _FieldView = new List<string>();
					List<int> _FieldStartPos = new List<int>();
					string[] singleFieldView = singleLineViews[k][i].Split('@');
					for (int j = 0; j < singleFieldView.Length-1; j++) // kazda grafika danego pola
					{
						int countChar = 0;
						string line = singleFieldView[j].Trim(MyChar).Substring(6);
						while (line[countChar] == '´') countChar++;
						_FieldStartPos.Add(countChar);
						_FieldView.Add(line.TrimStart(MyChar));
					}
					FieldView[k].Add(_FieldView);
					FieldStartPos[k].Add(_FieldStartPos);
				}
			}
		}
		public int GetCategory()
		{
			if (SelectedList == null || SelectedList.Count == 0)
				return PhisicalMap[Position.X + 1, Position.Y + 1].Category;
			else return PhisicalMap[SelectedList[0].X, SelectedList[0].Y].Category;
		}
		public Point GetSelectedPosition()
        {
			if (SelectedList.Count > 0) return new Point(SelectedList[0].X - 1, SelectedList[0].Y - 1);
			else return Position;
        }

		#region ACTIVIETIES
		public void Build(Product p)
		{
			var point = GetSelectedPosition();
			ClearSelected();
			var field = new Field(StartX + ((point.Y - point.X) * 12), StartY + ((point.Y + point.X + 2) * 3), p.category + 5, p.type, 0);
			PhisicalMap[point.X+1, point.Y+1] = field;
			MoveSelect(point, Position, false);
		}
		public void Destroy()
		{
			do
			{
				var point = GetSelectedPosition();
				if (SelectedList.Count > 0) SelectedList.RemoveAt(0);
				var field = new Field(StartX + ((point.Y - point.X) * 12), StartY + ((point.Y + point.X + 2) * 3), 0, 2, 0);
				PhisicalMap[point.X + 1, point.Y + 1] = field;

				ShowField(new Point(point.X, point.Y),false);
				ShowField(new Point(point.X, point.Y + 1),false);
				ShowField(new Point(point.X + 1, point.Y),false);
				ConsiderHeight(new Point(point.X, point.Y + 1), new Point(Position.X + 1, Position.Y + 1));
				ConsiderHeight(new Point(point.X + 1, point.Y), new Point(Position.X + 1, Position.Y + 1));
				ConsiderHeight(new Point(point.X + 1, point.Y + 1), new Point(Position.X + 1, Position.Y + 1));
			}
			while (SelectedList.Count > 0);
		}
		public Point Dragg()
		{
			if (SelectedList.Count > 0 && PhisicalMap[Position.X+1, Position.Y+1].Category > 5) ClearSelected();
			var point = GetSelectedPosition();
			DraggedField = new Field(PhisicalMap[point.X + 1, point.Y + 1]);
			PhisicalMap[point.X + 1, point.Y + 1].Category = 0;
			PhisicalMap[point.X + 1, point.Y + 1].Type = 2;
			PhisicalMap[point.X + 1, point.Y + 1].Duration = 0;
			Destroy();
			return point;
		}
		public void Drop(Point p)
		{
			PhisicalMap[p.X + 1, p.Y + 1].Category = DraggedField.Category;
			PhisicalMap[p.X + 1, p.Y + 1].Type = DraggedField.Type;
			PhisicalMap[p.X + 1, p.Y + 1].Duration = DraggedField.Duration;

			DraggedField = null;
			if (p.X != Position.X || p.Y != Position.Y)
			{
				ShowField(new Point(p.X, p.Y), false);
				ConsiderHeight(new Point(p.X, p.Y + 1), new Point(Position.X + 1, Position.Y + 1));
				ConsiderHeight(new Point(p.X + 1, p.Y), new Point(Position.X + 1, Position.Y + 1));

				ShowField(new Point(Position.X, Position.Y), false);
				ShowField(new Point(Position.X, Position.Y + 1), false);
				ShowField(new Point(Position.X + 1, Position.Y), false);
				ConsiderHeight(new Point(Position.X, Position.Y + 1), new Point(Position.X + 1, Position.Y + 1));
				ConsiderHeight(new Point(Position.X + 1, Position.Y), new Point(Position.X + 1, Position.Y + 1));
			}
			MoveSelect(Position, Position, false);
		}
		#endregion

		private void ShowFarm()
		{
			for (int i = 0; i < FarmSize; i++)
				for (int j = 0; j < FarmSize; j++)
					ShowField(new Point(i, j));
			MoveSelect(Position, Position, false);
		}
		public void ClearFarm()
		{
			string space = ("").PadRight(Console.WindowWidth, ' ');
			for (int i = BorderTop + 1; i < BorderBottom; i++)
			{
				Console.SetCursorPosition(0, i);
				Console.Write(space);
			}
		}
		public void MoveFarm(Point vector)
		{
			StartX += vector.X * 24;
			StartY += vector.Y * 6;

			for (int i = 0; i < FarmSize; i++)
				for (int j = 0; j < FarmSize; j++)
					PhisicalMap[i, j].Move(new Point(vector.X * 24, vector.Y * 6));

			ShowFarm();
		}
		public void ShowPartFarm(string type, bool oppositeMenu)
		{
			int CW = Console.WindowWidth;
			int leftBarrier;
			int rightBarrier;
			Point Pos;
			switch (type)
			{
				case "left":
					leftBarrier = BorderLeft;
					rightBarrier = CW / 5 + 1;
					for (int i = 2; i < FarmSize - 1; i++)
						for (int j = 2; j < FarmSize - 1; j++)
							ShowField(new Point(i, j), false, leftBarrier, rightBarrier);

					rightBarrier = BorderRight;
					if (oppositeMenu) rightBarrier = CW - CW / 5 - 2;
					Pos = new Point(Position.X + 1, Position.Y + 1);
					ConsiderHeight(Pos, Pos, leftBarrier, rightBarrier);

					break;

				case "right":
					rightBarrier = BorderRight;
					leftBarrier = CW - CW / 5 - 2;
					for (int i = 2; i < FarmSize - 1; i++)
						for (int j = 2; j < FarmSize - 1; j++)
							ShowField(new Point(i, j), false, leftBarrier, rightBarrier);

					leftBarrier = BorderLeft;
					if (oppositeMenu) leftBarrier = CW / 5 + 1;
					Pos = new Point(Position.X + 1, Position.Y + 1);
					ConsiderHeight(Pos, Pos, leftBarrier, rightBarrier);

					break;

				case "both":
					{
						leftBarrier = BorderLeft;
						rightBarrier = CW / 5 + 1;
						for (int i = 2; i < FarmSize - 1; i++)
							for (int j = 2; j < FarmSize - 1; j++)
								ShowField(new Point(i, j), false, leftBarrier, rightBarrier);

						rightBarrier = BorderRight;
						leftBarrier = CW - CW / 5 - 2;
						for (int i = 2; i < FarmSize - 1; i++)
							for (int j = 2; j < FarmSize - 1; j++)
								ShowField(new Point(i, j), false, leftBarrier, rightBarrier);

						Pos = new Point(Position.X + 1, Position.Y + 1);
						ConsiderHeight(Pos, Pos);
					}
					break;
			}
		}

		private void ClearSelected()
		{
			if (SelectedList.Count > 0)
			{
				while (SelectedList.Count > 0)
                {
					var point = SelectedList[0];
					SelectedList.RemoveAt(0);
					ConsiderHeight(new Point(point.X, point.Y), new Point(Position.X + 1, Position.Y + 1));
				}
			}
		}
		private void ShowField(Point p,bool currentSelected = false, int leftBarrier = 0, int rightBarrier = 0)
		{
			if (leftBarrier == 0) leftBarrier = BorderLeft;
			if (rightBarrier == 0) rightBarrier = BorderRight;

			int X = PhisicalMap[p.X, p.Y].X;
			int Y = PhisicalMap[p.X, p.Y].Y;
			int K = PhisicalMap[p.X, p.Y].Category;
			int T = PhisicalMap[p.X, p.Y].Type;
			int H = FieldStartPos[K][T].Count;
			int C = FieldHeigth - H;

			Console.ForegroundColor = Color(PhisicalMap[p.X, p.Y].Category, PhisicalMap[p.X, p.Y].Type);
			if (SelectedList.Contains(p)) Console.ForegroundColor = Color(-1,0);
			if (currentSelected == true)
			{
				Console.ForegroundColor = Color(-1,1);
				if (DraggedField != null)
				{
					Console.ForegroundColor = Color(-1, 0);
					K = DraggedField.Category;
					T = DraggedField.Type;
					H = FieldStartPos[K][T].Count;
					C = FieldHeigth - H;
				}
			}

			for (int i = 0; i < H; i++)
			{
				if (Y + i + C > BorderTop && BorderBottom > Y + i + C)
				{
					if (X + FieldStartPos[K][T][i] < leftBarrier)
					{
						if (X + FieldStartPos[K][T][i] + FieldView[K][T][i].Length > leftBarrier) {
							Console.SetCursorPosition(leftBarrier, Y + i + C);
							Console.Write(FieldView[K][T][i].Substring(leftBarrier - X - FieldStartPos[K][T][i])); }
					}
					else if (X + FieldStartPos[K][T][i] + FieldView[K][T][i].Length > rightBarrier)
					{
						if (X + FieldStartPos[K][T][i] < rightBarrier) {
							Console.SetCursorPosition(X + FieldStartPos[K][T][i], Y + i + C);
							Console.Write(FieldView[K][T][i].Substring(0, rightBarrier - X - FieldStartPos[K][T][i])); }
					}
					else {
						Console.SetCursorPosition(X + FieldStartPos[K][T][i], Y + i + C);
						Console.Write(FieldView[K][T][i]); }
				}
			}
			Console.ResetColor();
		}
		public bool MoveSelect(Point beforePos, Point currentPos, bool shift)
		{
			Position.X = currentPos.X;
			Position.Y = currentPos.Y;
			beforePos.X++; beforePos.Y++;
			currentPos.X++; currentPos.Y++;

			Console.SetCursorPosition(2, 6);
			Console.Write("[" + Position.X + "," + Position.Y + "]");
			//Modified = true;
			if (DraggedField != null)
			{
				if(PhisicalMap[currentPos.X,currentPos.Y].Category > 5)
				{
					Position.X = beforePos.X - 1;
					Position.Y = beforePos.Y - 1;
					return false;
                }
				ShowField(new Point(beforePos.X - 1, beforePos.Y - 1), false);
				ShowField(new Point(beforePos.X - 1, beforePos.Y), false);
				ShowField(new Point(beforePos.X, beforePos.Y - 1), false);
				ConsiderHeight(new Point(beforePos.X - 1, beforePos.Y), currentPos);
				ConsiderHeight(new Point(beforePos.X, beforePos.Y - 1), currentPos);
			}
			else if (shift == true)
			{
					 if (SelectedList.Contains(beforePos) == true) SelectedList.Remove(beforePos);
				else if (SelectedList.Count == 0) {SelectedList.Add(beforePos); }
				else if (PhisicalMap[SelectedList[0].X,SelectedList[0].Y].Category == PhisicalMap[beforePos.X, beforePos.Y].Category
					&& PhisicalMap[beforePos.X, beforePos.Y].Category < 6) SelectedList.Add(beforePos);
			}
			ConsiderHeight(beforePos, currentPos);
			return true;
		}
		private void ConsiderHeight(Point beforePos, Point currentPos, int leftBarrier = 0, int rightBarrier = 0)
		{
			if (beforePos.Y > currentPos.Y || beforePos.X > currentPos.X)
			{ FindFieldToOverride(currentPos); FindFieldToOverride(beforePos); }
			else { FindFieldToOverride(beforePos); FindFieldToOverride(currentPos); }

			ShowField(beforePos, false, leftBarrier, rightBarrier);
			CheckList.Remove(currentPos);
			ShowField(currentPos, true, leftBarrier, rightBarrier);
			for (int i = 0; i < CheckList.Count; i++) ShowField(CheckList[i], false, leftBarrier, rightBarrier);
			CheckList = new List<Point>() { };
		}
		private void FindFieldToOverride(Point p)
		{
			//Thread.Sleep(1000);
			//ShowCheckList();
			int i = 0;
			p.X += 1;
			if (FieldView[PhisicalMap[p.X, p.Y].Category][PhisicalMap[p.X, p.Y].Type].Count > FieldHeigth && CheckList.Contains(p) == false)
			{
				while (i < CheckList.Count && CheckList[i].Y < p.Y) i++;
				while (i < CheckList.Count && CheckList[i].X < p.X) i++;
				CheckList.Insert(i, p);
				FindFieldToOverride(p);
			}

			p.X -= 1;
			p.Y += 1;
			if (FieldView[PhisicalMap[p.X, p.Y].Category][PhisicalMap[p.X, p.Y].Type].Count > FieldHeigth && CheckList.Contains(p) == false)
			{
				while (i < CheckList.Count && CheckList[i].Y < p.Y) i++;
				while (i < CheckList.Count && CheckList[i].X < p.X) i++;
				CheckList.Insert(i, p);
				FindFieldToOverride(p);
			}

			p.X += 1;
			if (FieldView[PhisicalMap[p.X, p.Y].Category][PhisicalMap[p.X, p.Y].Type].Count > FieldHeigth && CheckList.Contains(p) == false)
			{
				while (i < CheckList.Count && CheckList[i].Y < p.Y) i++;
				while (i < CheckList.Count && CheckList[i].X < p.X) i++;
				CheckList.Insert(i, p);
				FindFieldToOverride(p);
			}
		}
		public void ShowFieldID()
		{
			for (int i = 0; i < FarmSize; i++)
				for (int j = 0; j < FarmSize; j++)
					if (StartX + 17 + ((j - i) * 12) < BorderRight && StartX + 10 + ((j - i) * 12) > BorderLeft
						&& StartY + 3 + ((j + i) * 3) < BorderBottom && StartY + 3 + ((j + i) * 3) > BorderTop)
					{
						Console.SetCursorPosition(StartX + 10 + ((j - i) * 12), StartY + 3 + ((j + i) * 3));
						Console.Write(" " + i + "," + j + " ");
					}
		}
		private void ShowCheckList()
		{
			Console.SetCursorPosition(14, 5);
			Console.Write("CheckList: ");
			foreach(Point p in CheckList)
				Console.Write("["+p.X+","+p.Y+"] ");
		}
	}
}
