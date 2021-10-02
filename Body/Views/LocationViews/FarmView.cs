using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;

namespace FarmConsole.Body.Views.LocationViews
{
    public class FarmView : MapView
    {
        public static void InitializeFarmView(FieldModel[,] Map)
        {
            InitializeMap(Map, new FieldModel(ProductModel.GetProduct("Trawa")));
        }

        public static bool MowGrass()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return false; }
            FieldModel Field = GetField();
            Field.State = 0;
            SetField(GetPos(), Field);
            ShowPhisicalField(GetPos());
            return true;
        }

        public static bool Plow()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return false; }

            var PhisicalPos = GetPos();
            SetField(PhisicalPos, new FieldModel(ProductModel.GetProduct("Pole")));
            ClearSelected();
            ShowPhisicalField(GetPos());
            return true;
        }

        public static bool Sow(ProductModel Product)
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return false; }
            if (ProductModel.GetProduct(GetField()).ProductName != "Pole") return false;

            var PhisicalPos = GetPos();
            SetField(PhisicalPos, new FieldModel(ProductModel.GetRedirectProduct(Product)));
            ClearSelected();
            ShowPhisicalField(GetPos());
            return true;
        }

        public static bool Fertilize(ProductModel Product)
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return false; }

            return true;
        }

        public static bool WaterIt()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return false; }
            var PhisicalPos = GetPos();
            //PhisicalMap[point.X, point.Y].Category = ?;
            //PhisicalMap[point.X, point.Y].Type = ?;
            ClearSelected();
            ShowPhisicalField(GetPos());
            return true;
        }

        public static bool Collect()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return false; }

            return true;
        }

        public static bool MakeFertilize()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return false; }

            return true;
        }
    }
}
