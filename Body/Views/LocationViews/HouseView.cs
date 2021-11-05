using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.LocationViews
{
    class HouseView : MapView
    {
        public static void InitializeHouseView(FieldModel[,] Map)
        {
            InitializeMap(Map, new FieldModel(ProductModel.GetProduct("Drewniana Podłoga")));
        }
    }
}
