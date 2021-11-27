using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace FarmConsole.Body.Views.LocationViews
{
    public class MapManager : MapEngine
    {
        public static void GlobalMapInit()
        {
            ColorService.SetColorPalette();
            ProductModel.SetProducts();
            GameService.SetGrowCycle();
            SetMapBorders();
        }
        
        public static void InitMap(MapModel Map)
        {
            SetMapBorders();
            MapEngine.Map = Map;
            InitVisualMap();
            InitMapExtremePoints();
        }
        
        public static void ShowMap(bool Smoothly = true)
        {
            MapModel map = Map;
            int procent = 80;
            if (Smoothly)
            {
                for (int i = 0; i < 3; i++)
                {
                    DarkerMap(procent);
                    ShowVisualMap();
                    procent -= 40;
                    Map = map;
                }
            }
            else
            {
                DarkerMap(procent);
                ShowVisualMap();
            }
        }
        
        public static void HideMap()
        {
            MapModel map = Map;
            for (int i = 0; i < 2; i++)
            {
                DarkerMap(55);
                ShowVisualMap();
            }
            //Map = map;
        }


        public static string Build(ProductModel BuildProduct)
        {
            if (ProductModel.GetProduct(GetField()).Category == 3) return StringService.Get("overwriting");
            SetField(GetPos(), new FieldModel(BuildProduct));
            ShowPhisicalField(GetPos());
            ClearSelected(1);
            return StringService.Get("done");
        }
        
        public static string Destroy(int CountToRemove = 0)
        {
            if (CountToRemove == 0) CountToRemove = GetSelectedFieldCount();
            do
            {
                CountToRemove--;
                Point PhisicalPos = GetPos();
                ClearSelected(1);
                SetField(PhisicalPos, null, "Base");
                ShowPhisicalField(PhisicalPos);
            }
            while (CountToRemove > 0 && GetSelectedFieldCount() > 0);
            return StringService.Get("done");
        }
        
        public static void Dragg()
        {
            if(GetField("Stand").Category == 3) ClearSelected(); 
            SetField(GetPos(), null, "Dragged");
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
                SetField(new Point(), null, "Dragged");
                ShowPhisicalField(GetPos());
            }
        }
        
        public static void Rotate()
        {
            FieldModel Field = GetField();
            Field.State++;
            if (ProductModel.GetProduct(Field).ProductName != "error")
            {
                Field.ReloadView();
                SetField(GetPos(), Field);
            }
            else
            {
                Field.State = 0;
                Field.ReloadView();
                SetField(GetPos(), Field);
            }
            ShowPhisicalField(GetPos());
        }
        
        public static void MakePath()
        {
            SetField(GetPos(), new FieldModel(ProductModel.GetProduct("Ścieżka")));
            ShowPhisicalField(GetPos());
            ClearSelected(1);
        }
    }
}
