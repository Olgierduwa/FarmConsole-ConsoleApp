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

        public static string MowGrass()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return XF.GetString("multi mowing"); }
            FieldModel Field = GetField();
            Field.State = 0;
            SetField(GetPos(), Field);
            ShowPhisicalField(GetPos());
            return XF.GetString("done");
        }

        public static string Plow()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return XF.GetString("multi_plowing"); }
            SetField(GetPos(), new FieldModel(ProductModel.GetProduct("Pole")));
            if (GetSelectedFieldCount() > 0)
            {
                CenterMapOnPos(GetPos());
                ClearSelected();
            }
            else ShowPhisicalField(GetPos());
            return XF.GetString("done");
        }

        public static string Sow(ProductModel Product)
        {
            if (ProductModel.GetProduct(GetField()).ProductName != "Pole") return XF.GetString("no plowed field");
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return XF.GetString("multi sowing"); }
            SetField(GetPos(), new FieldModel(ProductModel.GetRedirectProduct(Product)));
            if (GetSelectedFieldCount() > 0)
            {
                CenterMapOnPos(GetPos());
                ClearSelected();
            }
            else ShowPhisicalField(GetPos());
            return XF.GetString("done");
        }

        public static string Fertilize(ProductModel Product)
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return XF.GetString("multi fertilizating"); }
            if (ProductModel.GetProduct(GetField()).Category == 2 &&
                ProductModel.GetProduct(GetField()).Category > 0)
            {

            }
            else return XF.GetString("cant fertilizating");
            return XF.GetString("done");
        }

        public static string WaterIt()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return XF.GetString("multi watering"); }
            FieldModel Field = GetField();
            if(Field.Duration < 50)
            {
                Field.Duration += 50;
                Field.Color = ColorService.Darken(Field.Color);
                SetField(GetPos(), Field);
                ClearSelected();
                ShowPhisicalField(GetPos());
            }
            return XF.GetString("done");
        }

        public static string Collect()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return XF.GetString("multi collecting"); }

            return XF.GetString("done");
        }

        public static string MakeFertilize()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return XF.GetString("multi fertilize making"); }

            return XF.GetString("done");
        }
    }
}
