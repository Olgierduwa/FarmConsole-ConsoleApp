﻿using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Views.LocationViews
{
    public class MapView : MapEngine
    {
        public static void GlobalMapInit()
        {
            ColorService.SetColorPalette();
            ProductModel.SetProducts();
            GameService.SetGrowCycle();
            SetColors();
            SetMapBorders();
        }
        public static void InitMap(MapModel Map)
        {
            SetMapBorders();
            MapEngine.Map = Map;
            InitVisualMap();
            InitMapExtremePoints();
        }
        public static void ShowMap()
        {
            ShowVisualMap();
        }
        public static void HideMap()
        {
            ClearVisualMap();
        }

        public static string Build(ProductModel BuildProduct)
        {
            if (ProductModel.GetProduct(GetField()).Category == 3) return XF.GetString("overwriting");
            SetField(GetPos(), new FieldModel(BuildProduct));
            if (GetSelectedFieldCount() > 0)
            {
                CenterMapOnPos(GetPos());
                ClearSelected();
            }
            else ShowPhisicalField(GetPos());
            return XF.GetString("done");
        }
        public static void Dragg()
        {
            if(GetField("Stand").Category == 3) ClearSelected(); 
            SetField(GetPos(), new FieldModel(), "Dragged");
            Destroy(1);
            ClearSelected();
            ShowPhisicalField(GetPos());
        }
        public static void Drop(bool ConfirmDrop = true)
        {
            if(GetField("Dragged") != null)
            {
                if (ConfirmDrop) SetField(GetPos(), GetField("Dragged"));
                else
                {
                    Point PhisicalPos = GetPos(Dragged: true);
                    SetField(PhisicalPos, GetField("Dragged"));
                    ShowPhisicalField(PhisicalPos);
                }
                SetField(new Point(), new FieldModel(), "Dragged");
                ShowPhisicalField(GetPos());
            }
        }
        public static void Destroy(int CountToRemove = 0)
        {
            if (CountToRemove == 0) CountToRemove = GetSelectedFieldCount();
            do
            {
                CountToRemove--;
                Point PhisicalPos = GetPos();
                ClearSelected(1);
                SetField(PhisicalPos, new FieldModel(), "Base");
                ShowPhisicalField(PhisicalPos);
            }
            while (CountToRemove > 0 && GetSelectedFieldCount() > 0);
        }
        public static void Rotate()
        {
            FieldModel Field = GetField();
            Field.State++;
            if (ProductModel.GetProduct(Field).ProductName != "error") SetField(GetPos(), Field);
            else { Field.State = 0; SetField(GetPos(), Field); }
            ShowPhisicalField(GetPos());
        }
    }
}
