using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;

namespace FarmConsole.Body.Views.LocationViews
{
    public class FarmView : MapManager
    {
        public static void MowGrass()
        {
            FieldModel Field = GetField();
            Field.State = 0;
            Field = Field.ToField();
            SetField(GetPos(), Field);
            ClearSelectedFields(1);
        }

        public static string Plow()
        {
            SetField(GetPos(), ObjectModel.GetObject("field").ToField());
            ClearSelectedFields(1);
            return LS.Action("done");
        }

        public static string Sow(ProductModel Product)
        {
            if (GetField().ObjectName != "field") { ClearSelectedFields(); return LS.Action("no plowed field"); }
            SetField(GetPos(), ObjectModel.GetObject(Product.Property).ToField());
            ClearSelectedFields(1);
            return LS.Action("done");
        }

        public static string Fertilize(ProductModel Product)
        {
            FieldModel Field = GetField();
            if (Field.Category != 1 || Field.Type < 1) return LS.Action("cant fertilizating");
            if (Field.Duration / 10 > 1) return LS.Action("already fertilized");

            Field.Duration += (short)Convert.ToInt32(Product.Property);
            Field.View.ColorizePixels("Bluer");
            ClearSelectedFields(1);
            return LS.Action("done");
        }

        public static string WaterIt()
        {
            FieldModel Field = GetField();
            if ((Field.Duration / 10) % 2 == 1) return LS.Action("already watered");

            Field.Duration += 10;
            Field.View.ColorizePixels("Darker");
            ClearSelectedFields(1);
            return LS.Action("done");
        }

        public static string Collect()
        {
            SetField(GetPos(), ObjectModel.GetObject("field").ToField());
            ClearSelectedFields(1);
            return LS.Action("done");
        }

        public static void MakeFertilize()
        {
            SetField(GetPos(), ObjectModel.GetObject("field").ToField());
            ClearSelectedFields(1);
        }
    }
}
