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
        public static void InitializeFarmView(int[,,] Map)
        {
            InitializeMap(Map, 1, 2, 0);
        }

        public static bool Plow()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return true; }
            var PhisicalPos = GetPos();
            SetFieldProp(PhisicalPos, 1, 0, 0);
            ClearSelected();
            ShowVisualField(GetPos(), true);
            return false;
        }

        public static bool Sow(ProductModel p)
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return true; }
            var PhisicalPos = GetPos();
            SetFieldProp(PhisicalPos, 2, p.type, 0);
            ClearSelected();
            ShowVisualField(GetPos(), true);
            return false;
        }

        public static bool WaterIt()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return true; }
            var PhisicalPos = GetPos();
            //PhisicalMap[point.X, point.Y].Category = ?;
            //PhisicalMap[point.X, point.Y].Type = ?;
            ClearSelected();
            //CompleteSurroundings(StandPosition);
            return false;
        }

        public static bool Collect()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return true; }

            return false;
        }

        public static bool Fertilize(ProductModel p)
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return true; }

            return false;
        }

        public static bool MakeFertilize()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return true; }

            return false;
        }
    }
}
