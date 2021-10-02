using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Views.LocationViews
{
    public class MapView : MapService
    {
        public static void GlobalMapInitializate()
        {
            ProductModel.SetProducts();
            SetMapBorders();
        }

        public static void InitializeMap(FieldModel[,] Map, FieldModel BaseField)
        {
            SetField(new Point(), BaseField, "Base");
            SetMapBorders();
            InitializePhisicalMap(Map);
            InitializeVisualMap();
            InitializeMapExtremePoints();
        }

        public static void ShowMap()
        {
            ShowVisualMap();
        }

        public static void HideMap()
        {
            ClearVisualMap();
        }

        public static bool Build(ProductModel BuildProduct)
        {
            if (ProductModel.GetProduct(GetField()).Category == 3) return false;
            if (GetSelectedFieldCount() > 0)
            {
                CenterMapOnPos(GetPos());
                ClearSelected();
            }
            SetField(GetPos(), new FieldModel(BuildProduct));
            ShowPhisicalField(GetPos());
            return true;
        }

        public static void Dragg()
        {
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

        public static void ComeIn()
        {
            ProductModel Product = new ProductModel(GetField());
            // wczytaj odpowiednią mapę na podstawie wybranego budynku (Product.ProductName)
        }

        public static void Rotate()
        {
            FieldModel Field = GetField();
            // obróć wybrane pole w zależności od stanu w którym się znajduje (Field.State)
        }
    }
}
