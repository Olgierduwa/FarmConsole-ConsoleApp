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
            SimpleMap = MapFromSave;
            GetFieldsData();
            SetMapBorders();
            InitializePhisicalMap();
            InitializeVisualMap();
            ShowMap();
        }
        public static void Hide()
        {
            ClearMap();
        }

        public static void Build(ProductModel p)
        {
            var PhisicalPos = GetSelectedPosition();

            // move visual map to: point (on center)

            ClearSelected();
            SetPhisicalField(PhisicalPos, new FieldModel(p.category + 5, p.type, 0));

            // move stand position to: point

            Point vector = new Point();
            MoveStandPosition(vector);
        }
        public static void Destroy()
        {
            do
            {
                Point PhisicalPos = GetSelectedPosition();
                if (SelectedFields.Count > 0) SelectedFields.RemoveAt(0);
                SetPhisicalField(PhisicalPos, new FieldModel(1, 2, 0));
                ShowPhisicalField(PhisicalPos);
            }
            while (SelectedFields.Count > 0);
        }
        public static void Dragg()
        {
            SetDraggedField(GetSelectedPosition());
            Destroy();
            ShowVisualField(CSP);
        }
        public static void Drop(bool ConfirmDrop = true)
        {
            if (ConfirmDrop) SetSelectedFieldProp(RealPos(CSP), GetDraggedField().Category, GetDraggedField().Type, GetDraggedField().Duration);
            else
            {
                Point PhisicalPos = new Point(DraggedPosition.X, DraggedPosition.Y);
                SetSelectedFieldProp(PhisicalPos, GetDraggedField().Category, GetDraggedField().Type, GetDraggedField().Duration);
                ShowPhisicalField(PhisicalPos);
            }
            SetDraggedField(new Point());
            ShowVisualField(CSP, true);
        }

        

        public static bool Plow()
        {
            if (SelectedFields.Count > 1) { ClearSelected(); return true; }
            var PhisicalPos = GetSelectedPosition();
            SetSelectedFieldProp(PhisicalPos, 1, 0, 0);
            ClearSelected();
            ShowVisualField(CSP, true);
            return false;
        }
        public static bool Sow(ProductModel p)
        {
            if (SelectedFields.Count > 1) { ClearSelected(); return true; }
            var PhisicalPos = GetSelectedPosition();
            SetSelectedFieldProp(PhisicalPos, 2, p.type, 0);
            ClearSelected();
            ShowVisualField(CSP, true);
            return false;
        }
        public static bool WaterIt()
        {
            if (SelectedFields.Count > 1) { ClearSelected(); return true; }
            var PhisicalPos = GetSelectedPosition();
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
