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
            SetFieldsData();
            SetMapBorders();
        }

        public static void InitializeMap(int[,,] Map, int C, int T, int D)
        {
            SetMap(Map);
            SetFieldProp(new Point(), C, T, D, Base: true);
            SetMapBorders();
            InitializePhisicalMap();
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

        public static void Build(int Category, int Type)
        {
            if (GetSelectedFieldCount() > 0)
            {
                CenterMapOnPos(GetPos());
                ClearSelected();
            }
            SetFieldProp(GetPos(), Category, Type, 0);
        }

        public static void Dragg()
        {
            SetFieldProp(GetPos(), Dragged: true);
            Destroy();
            ShowVisualField(GetPos());
        }

        public static void Drop(bool ConfirmDrop = true)
        {
            if (ConfirmDrop) SetFieldProp(GetPos(), GetFieldProp("category", true), GetFieldProp("type", true), GetFieldProp("duration", true));
            else
            {
                Point PhisicalPos = new Point(GetPos(Dragged: true).X, GetPos(Dragged: true).Y);
                SetFieldProp(PhisicalPos, GetFieldProp("category", true), GetFieldProp("type", true), GetFieldProp("duration", true));
                ShowPhisicalField(PhisicalPos);
            }
            SetFieldProp(new Point(), Dragged: true);
            ShowVisualField(GetPos(), true);
        }

        public static void Destroy()
        {
            do
            {
                Point PhisicalPos = GetPos();
                ClearSelected(1);
                SetFieldProp(PhisicalPos, Base: true);
                ShowPhisicalField(PhisicalPos);
            }
            while (GetSelectedFieldCount() > 0);
        }
    }
}
