using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;

namespace FarmConsole.Body.Views.LocationViews
{
    public class FarmView : MapService
    {
        public static void Show(int[,,] MapFromSave)
        {
            FarmSize = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(MapFromSave.Length / 3))) + 3;
            StandPosition = new Point();
            MapPosition = new Point();
            MapPosition.X = Console.WindowWidth / 2 - 12;
            MapPosition.Y = (Console.WindowHeight - 7 * FarmSize) / 2 + 2;

            SimpleMap = MapFromSave;
            GetFieldsData();
            InitializePhisicalMap();
            SetMapBorders();
            ShowMap();
            MoveStandPosition(StandPosition, StandPosition, false);
        }
        public static void Hide()
        {
            ClearMap();
        }
        public static void Build(ProductModel p)
        {
            var point = GetSelectedPosition();
            ClearSelected();
            var field = new FieldModel(MapPosition.X + (point.Y - point.X) * 12, MapPosition.Y + (point.Y + point.X) * 3, p.category + 5, p.type, 0);
            SetSelectedField(point, field);
            MoveStandPosition(point, StandPosition, false);
        }
        public static void Destroy()
        {
            do
            {
                var point = GetSelectedPosition();
                if (SelectedFields.Count > 0) SelectedFields.RemoveAt(0);
                var field = new FieldModel(MapPosition.X + (point.Y - point.X) * 12, MapPosition.Y + (point.Y + point.X) * 3, 0, 2, 0);
                SetSelectedField(point, field);

                ShowSingleField(new Point(point.X, point.Y));
                CompleteSurroundings(point);
            }
            while (SelectedFields.Count > 0);
        }
        public static void Dragg()
        {
            Point point;
            if (SelectedFields.Count > 0 && GetFieldCategory(true) > 5) { point = GetSelectedPosition(true); ClearSelected(); }
            else point = GetSelectedPosition();
            DraggPosition = new Point(point.X, point.Y);
            SetDraggedField(new FieldModel(GetSelectedField(point)));
            SetSelectedFieldProp(point, 0, 2, 0);
            Destroy();
        }
        public static void Drop(bool ConfirmDrop = true)
        {
            Point point = new Point(StandPosition.X, StandPosition.Y);
            if (!ConfirmDrop) point = new Point(DraggPosition.X, DraggPosition.Y);
            SetSelectedFieldProp(point, GetDraggedField().Category, GetDraggedField().Type, GetDraggedField().Duration);
            SetDraggedField(null);
            if (point.X != StandPosition.X || point.Y != StandPosition.Y)
            {
                ShowSingleField(new Point(point.X, point.Y));
                ConsiderHeight(new Point(point.X, point.Y - 1), StandPosition);
                ConsiderHeight(new Point(point.X - 1, point.Y), StandPosition);
                CompleteSurroundings(StandPosition);
            }
            MoveStandPosition(StandPosition, StandPosition, false);
        }

        

        public static bool Plow()
        {
            if (SelectedFields.Count > 1) { ClearSelected(); return true; }
            var point = GetSelectedPosition();
            SetSelectedFieldProp(point, 1, 0, 0);
            ClearSelected();
            CompleteSurroundings(StandPosition);
            return false;
        }
        public static bool Sow(ProductModel p)
        {
            if (SelectedFields.Count > 1) { ClearSelected(); return true; }
            var point = GetSelectedPosition();
            SetSelectedFieldProp(point, 2, p.type, 0);
            ClearSelected();
            CompleteSurroundings(StandPosition);
            return false;
        }
        public static bool WaterIt()
        {
            if (SelectedFields.Count > 1) { ClearSelected(); return true; }
            var point = GetSelectedPosition();
            //PhisicalMap[point.X, point.Y].Category = ?;
            //PhisicalMap[point.X, point.Y].Type = ?;
            ClearSelected();
            //CompleteSurroundings(StandPosition);
            return false;
        }
        public static bool Collect()
        {
            if (SelectedFields.Count > 1) { ClearSelected(); return true; }

            return false;
        }
        public static bool Fertilize(ProductModel p)
        {
            if (SelectedFields.Count > 1) { ClearSelected(); return true; }

            return false;
        }
        public static bool MakeFertilize()
        {
            if (SelectedFields.Count > 1) { ClearSelected(); return true; }

            return false;
        }
    }
}
