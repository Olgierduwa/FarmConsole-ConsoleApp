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
            //FieldModel Field = GetField();
            GetField().State = 0;
            //SetField(GetPos(), Field);
            ShowField(GetPos());
            ClearSelectedFields(1);
        }

        public static string Plow()
        {
            SetField(GetPos(), ObjectModel.GetObject("Pole").ToField());
            ShowField(GetPos());
            ClearSelectedFields(1);
            return StringService.Get("done");
        }

        public static string Sow(ProductModel Product)
        {
            if (GetField().ObjectName != "Pole") { ClearSelectedFields(); return StringService.Get("no plowed field"); }
            SetField(GetPos(), ObjectModel.GetObject(Product.Property).ToField());
            ShowField(GetPos());
            ClearSelectedFields(1);
            return StringService.Get("done");
        }

        public static string Fertilize(ProductModel Product)
        {
            FieldModel Field = GetField();
            if (Field.Category != 1 || Field.Type < 1) return StringService.Get("cant fertilizating");
            if (Field.Duration / 10 > 1) return StringService.Get("already fertilized");

            Field.Duration += Convert.ToInt32(Product.Property);
            Field.ColorizeView("Bluer");
            SetField(GetPos(), Field);
            ShowField(GetPos());
            ClearSelectedFields(1);
            return StringService.Get("done");
        }

        public static string WaterIt()
        {
            FieldModel Field = GetField();
            if ((Field.Duration / 10) % 2 == 1) return StringService.Get("already watered");

            Field.Duration += 10;
            Field.ColorizeView("Darker");
            SetField(GetPos(), Field);
            ShowField(GetPos());
            ClearSelectedFields(1);
            return StringService.Get("done");
        }

        public static string Collect()
        {
            SetField(GetPos(), ObjectModel.GetObject("Pole").ToField());
            ShowField(GetPos());
            ClearSelectedFields(1);
            return StringService.Get("done");
        }

        public static void MakeFertilize()
        {
            SetField(GetPos(), ObjectModel.GetObject("Pole").ToField());
            ShowField(GetPos());
            ClearSelectedFields(1);
        }
    }
}
