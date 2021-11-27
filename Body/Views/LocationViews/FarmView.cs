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
        public static string MowGrass()
        {
            FieldModel Field = GetField();
            Field.State = 0;
            SetField(GetPos(), Field);
            ShowPhisicalField(GetPos());
            ClearSelected(1);
            return StringService.Get("done");
        }

        public static string Plow()
        {
            SetField(GetPos(), new FieldModel(ProductModel.GetProduct("Pole")));
            ShowPhisicalField(GetPos());
            ClearSelected(1);
            return StringService.Get("done");
        }

        public static string Sow(ProductModel Product)
        {
            if (GetField().FieldName != "Pole") { ClearSelected(); return StringService.Get("no plowed field"); }
            SetField(GetPos(), new FieldModel(ProductModel.GetProduct(Product.Property)));
            ShowPhisicalField(GetPos());
            ClearSelected(1);
            return StringService.Get("done");
        }

        public static string Fertilize(ProductModel Product)
        {
            FieldModel Field = GetField();
            if (Field.Category != 2 || Field.Type < 1) return StringService.Get("cant fertilizating");
            if (Field.Duration / 10 > 1) return StringService.Get("already fertilized");

            Field.Duration += Convert.ToInt32(Product.Property);
            Field.Color = ColorService.Bluer(Field.Color);
            SetField(GetPos(), Field);
            ShowPhisicalField(GetPos());
            ClearSelected(1);
            return StringService.Get("done");
        }

        public static string WaterIt()
        {
            FieldModel Field = GetField();
            if ((Field.Duration / 10) % 2 == 1) return StringService.Get("already watered");

            Field.Duration += 10;
            Field.Color = ColorService.Darker(Field.Color);
            SetField(GetPos(), Field);
            ShowPhisicalField(GetPos());
            ClearSelected(1);
            return StringService.Get("done");
        }

        public static string Collect()
        {
            SetField(GetPos(), new FieldModel(ProductModel.GetProduct("Pole")));
            ShowPhisicalField(GetPos());
            ClearSelected(1);
            return StringService.Get("done");
        }

        public static string MakeFertilize()
        {
            SetField(GetPos(), new FieldModel(ProductModel.GetProduct("Pole")));
            ShowPhisicalField(GetPos());
            ClearSelected(1);
            return StringService.Get("done");
        }
    }
}
