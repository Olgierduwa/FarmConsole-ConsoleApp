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
            //ProductModel.SetProducts();
            GameService.SetGrowCycle();
            SetMapScreenBorders();
        }
        
        public static void InitMap(MapModel Map)
        {
            SetMapScreenBorders();
            MapEngine.Map = Map;
            InitVisualMap();
        }
        
        public static void ShowMap(bool Smoothly = true)
        {
            MapModel map = Map;
            int procent = 60;
            if (Smoothly)
            {
                for (int i = 0; i < 3; i++)
                {
                    DarkerVisualMap(procent);
                    ShowVisualMap();
                    procent -= 30;
                    Map = map;
                }
            }
            else
            {
                DarkerVisualMap(procent);
                ShowVisualMap();
            }
        }
        
        public static void HideMap(bool CompletelyHide = true)
        {
            MapModel map = Map;
            for (int i = 0; i < 2; i++)
            {
                DarkerVisualMap(40);
                ShowVisualMap();
            }
            if (CompletelyHide)
            {
                ClearMapSreen();
                Map = map;
            }
        }


        public static string Build(ProductModel BuildProduct)
        {
            if (GetField().Category == 3) return StringService.Get("overwriting");
            SetField(GetPos(), BuildProduct.ToField());
            ShowField(GetPos());
            ClearSelectedFields(1);
            return StringService.Get("done");
        }
        
        public static void Destroy()
        {
            SetField(GetPos(), null, "Base");
            ShowField(GetPos());
            ClearSelectedFields(1);
        }
        
        public static void Dragg()
        {
            if(GetField("Stand").Category == 3) ClearSelectedFields(); 
            SetField(GetPos(), null, "Dragged");
            Destroy();
            ClearSelectedFields();
            ShowField(GetPos());
        }
        
        public static void Drop(bool ConfirmDrop = true)
        {
            if(GetField("Dragged") != null)
            {
                if (ConfirmDrop)
                {
                    SetField(GetPos(), GetField("Dragged"));
                    SetField(new Point(), null, "Dragged");
                }
                else
                {
                    Point DraggedPosition = GetPos(Dragged: true);
                    SetField(DraggedPosition, GetField("Dragged"));
                    SetField(new Point(), null, "Dragged");
                    ShowField(DraggedPosition);
                }
                ShowField(GetPos());
            }
        }
        
        public static string Rotate()
        {
            FieldModel Field = GetField();
            if (Field.StateName[0] == '-' && Field.StateName.Length == 6) // check if locked objects
            {
                if (Field.StateName[5] == '1') return StringService.Get("cant edit locked");
                else Field.State++;
            }
            var NewField = Field.ToField();
            NewField.State++;
            NewField.SetID();
            NewField = NewField.ToField();

            if (NewField.ObjectName != "error" && NewField.StateName != "+") SetField(GetPos(), NewField);
            else
            {
                Field.State = 0;
                Field.SetID();
                Field = Field.ToField();
                SetField(GetPos(), Field);
            }
            ClearSelectedFields(1);
            ShowField(GetPos());
            return StringService.Get("done");
        }
        
        public static void DigPath()
        {
            SetField(GetPos(), ObjectModel.GetObject("Ścieżka").ToField());
            ShowField(GetPos());
            ClearSelectedFields(1);
        }

        public static void Lock()
        {
            FieldModel Field = GetField();
            Field.State++;
            Field = Field.ToField();
            ClearSelectedFields(1);
            ShowField(GetPos());
        }

        public static void Unlock()
        {
            FieldModel Field = GetField();
            Field.State--;
            Field = Field.ToField();
            ClearSelectedFields(1);
            ShowField(GetPos());
        }
    }
}
