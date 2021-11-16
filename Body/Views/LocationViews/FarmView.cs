﻿using System;
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
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return XF.GetString("multi plowing"); }
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
            if (GetField().FieldName != "Pole") { ClearSelected(); return XF.GetString("no plowed field"); }
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return XF.GetString("multi sowing"); }
            SetField(GetPos(), new FieldModel(ProductModel.GetProduct(Product.Property)));
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
            FieldModel Field = GetField();
            if (Field.Category == 2 && Field.Type > 0)
            {
                Field.Duration += Convert.ToInt32(Product.Property);
                Field.Color = ColorService.Bluer(Field.Color);
                SetField(GetPos(), Field);
                ShowPhisicalField(GetPos());
                ClearSelected();
            }
            else return XF.GetString("cant fertilizating");
            return XF.GetString("done");
        }

        public static string WaterIt()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return XF.GetString("multi watering"); }
            FieldModel Field = GetField();
            if((Field.Duration / 50) % 2 == 0)
            {
                Field.Duration += 50;
                Field.Color = ColorService.Darker(Field.Color);
                SetField(GetPos(), Field);
                ShowPhisicalField(GetPos());
                ClearSelected();
            }
            return XF.GetString("done");
        }

        public static string Collect()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return XF.GetString("multi collecting"); }
            SetField(GetPos(), new FieldModel(ProductModel.GetProduct("Pole")));
            ShowPhisicalField(GetPos());
            return XF.GetString("done");
        }

        public static string MakeFertilize()
        {
            if (GetSelectedFieldCount() > 1) { ClearSelected(); return XF.GetString("multi fertilize making"); }
            SetField(GetPos(), new FieldModel(ProductModel.GetProduct("Pole")));
            ShowPhisicalField(GetPos());
            return XF.GetString("done");
        }
    }
}
