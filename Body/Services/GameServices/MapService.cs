using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace FarmConsole.Body.Services.GameServices
{
    public class MapService : MapEngine
    {
        public static void GlobalMapInit()
        {
            ColorService.SetColorPalette();
            ObjectModel.SetObjects();
            GameService.SetGrowCycle();
            SetMapScreenBorders();
        }

        public static void InitMap(MapModel Map)
        {
            SetMapScreenBorders();
            MapEngine.Map = Map;
            InitVisualMap();
            InitMapView();
        }

        public static void FadeOut()
        {
            int procent = 30;
            for (int i = 0; i < 5; i++)
            {
                BrighterMapView(procent);
                ShowMapView();
                Map.View.SetWords();
                procent += 10;
            }
        }

        public static void ShowMap(bool Smoothly = true)
        {
            int procent = 60;
            if (Smoothly)
            {
                for (int i = 0; i < 3; i++)
                {
                    DarkerMapView(procent);
                    ShowMapView();
                    procent -= 30;
                    Map.View.SetWords();
                }
            }
            else
            {
                DarkerMapView(procent);
                ShowMapView();
            }
        }

        public static void HideMap(bool CompletelyHide = true)
        {
            Map.View.SetWords();
            for (int i = 0; i < 3; i++)
            {
                DarkerMapView(30);
                ShowMapView();
            }
            if (CompletelyHide) ClearMapSreen();
        }


        public static string Build(ProductModel BuildProduct)
        {
            if (BuildProduct.Amount < 1) { ClearSelectedFields(); return LS.Action("no more objects"); }
            if (GetField().Category == 3) return LS.Action("overwriting");
            SetField(GetPos(), BuildProduct.ToField());
            ClearSelectedFields(1);
            return LS.Action("done");
        }

        public static string Destroy()
        {
            var DeletingField = GetField().ToField();
            if (DeletingField.Pocket != null && !DeletingField.Pocket.IsEmpty) return LS.Action("is no empty");
            SetField(GetPos(), null, "Base");
            ClearSelectedFields(1);
            return LS.Action("done");
        }

        public static void Dragg()
        {
            if (GetField("Stand").Category == 3) ClearSelectedFields();
            SetField(GetPos(), null, "Dragged");
            SetField(GetPos(), null, "Base");
            //Destroy();
            ClearSelectedFields();
            ShowField(GetPos());
        }

        public static void Drop(bool ConfirmDrop = true)
        {
            if (GetField("Dragged") != null)
            {
                if (ConfirmDrop)
                {
                    if (GetField().Category == 1 && GetField().State > 0) Destroy();
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
                if (Field.StateName[5] == '1') return LS.Action("cant edit locked");
                else Field.State++;
            }
            var NewField = Field.ToField();
            NewField.State++;
            NewField = NewField.ToField();

            if (NewField.ObjectName != "error" && NewField.StateName != "+") SetField(GetPos(), NewField);
            else
            {
                Field.State = 0;
                Field = Field.ToField();
                SetField(GetPos(), Field);
            }
            ClearSelectedFields(1);
            return LS.Action("done");
        }

        public static void DigPath()
        {
            SetField(GetPos(), ObjectModel.GetObject("path").ToField());
            ClearSelectedFields(1);
        }

        public static void Lock()
        {
            FieldModel Field = GetField();
            Field.State++;
            SetField(GetPos(), Field.ToField());
            ClearSelectedFields(1);
        }

        public static void Unlock()
        {
            FieldModel Field = GetField();
            Field.State--;
            SetField(GetPos(), Field.ToField());
            ClearSelectedFields(1);
        }
    }
}
