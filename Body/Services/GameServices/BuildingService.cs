using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Services.GameServices
{
    static class BuildingService
    {
        private static MapModel Map;
        private static void Set(int x, int y, string structure, int state)
            => Map.SetField(x, y, ObjectModel.GetObject(structure, state).ToField());

        public static void BuildParking(MapModel map, Point p, Size s, bool orientation = true)
        {
            Map = map;
            string structure = "parking";
            int c = orientation ? 0 : 6;
            for (int y = p.Y; y < p.Y + s.Height; y++)
                for (int x = p.X; x < p.X + s.Width; x++)
                {
                    if (y == p.Y && x == p.X + s.Width - 1) Set(x, y, structure, c + 2);
                    else if (y == p.Y && x == p.X) Set(x, y, structure, c + 3);
                    else if (y == p.Y + s.Height - 1 && x == p.X) Set(x, y, structure, c + 4);
                    else if (y == p.Y + s.Height - 1 && x == p.X + s.Width - 1) Set(x, y, structure, c + 5);

                    else if (x == p.X) Set(x, y, structure, 7);
                    else if (x == p.X + s.Width - 1) Set(x, y, structure, 6);
                    else if (y == p.Y) Set(x, y, structure, 0);
                    else if (y == p.Y + s.Height - 1) Set(x, y, structure, 1);

                    else if (orientation && y % 2 == 0) Set(x, y, structure, 0);
                    else if (orientation && y % 2 == 1) Set(x, y, structure, 1);
                    else if (!orientation && x % 2 == 0) Set(x, y, structure, 6);
                    else if (!orientation && x % 2 == 1) Set(x, y, structure, 7);
                }
            if (orientation)
            {
                var road = map.GetField(p.X + 1, p.Y - 1);
                if (road.ObjectName == "road with sidewalk")
                {
                    road.State = 8;
                    road = road.ToField();
                    Set(p.X + 1, p.Y, "parking exit", 0);
                }
                road = map.GetField(p.X + s.Width - 2, p.Y - 1);
                if (road.ObjectName == "road with sidewalk")
                {
                    road.State = 8;
                    road = road.ToField();
                    Set(p.X + s.Width - 2, p.Y, "parking exit", 0);
                }
            }
            else
            {
                var road = map.GetField(p.X - 1, p.Y + 1);
                if (road.ObjectName == "road with sidewalk")
                {
                    road.State = 8;
                    road = road.ToField();
                    Set(p.X, p.Y + 1, "parking exit", 0);
                }
                road = map.GetField(p.X - 1, p.Y + s.Height - 2);
                if (road.ObjectName == "road with sidewalk")
                {
                    road.State = 8;
                    road = road.ToField();
                    Set(p.X, p.Y + s.Height - 2, "parking exit", 0);
                }
            }
        }
        public static void BuildWalls(MapModel map, Point p, Size s)
        {
            Map = map;
            string structure = "wall";
            for (int y = p.Y; y < p.Y + s.Height; y++)
                for (int x = p.X; x < p.X + s.Width; x++)
                {
                    if (y == p.Y + s.Height - 1 && x == p.X) Set(x, y, structure, 8);
                    else if (y == p.Y + s.Height - 1 && x == p.X + s.Width - 1) Set(x, y, structure, 9);
                    else if (y == p.Y && x == p.X + s.Width - 1) Set(x, y, structure, 10);
                    else if (y == p.Y && x == p.X) Set(x, y, structure, 11);

                    else if (y == p.Y + s.Height - 1) Set(x, y, structure, 0);
                    else if (x == p.X + s.Width - 1) Set(x, y, structure, 1);
                    else if (y == p.Y) Set(x, y, structure, 2);
                    else if (x == p.X) Set(x, y, structure, 3);
                }
        }
        public static void BuildFence(MapModel map, Point p, Size s)
        {
            Map = map;
            string structure = "fence";
            for (int y = p.Y; y < p.Y + s.Height; y++)
                for (int x = p.X; x < p.X + s.Width; x++)
                {
                    if (y == p.Y + s.Height - 1 && x == p.X) Set(x, y, structure, 2);
                    else if (y == p.Y + s.Height - 1 && x == p.X + s.Width - 1) Set(x, y, structure, 3);
                    else if (y == p.Y && x == p.X + s.Width - 1) Set(x, y, structure, 4);
                    else if (y == p.Y && x == p.X) Set(x, y, structure, 5);

                    else if (x == p.X || x == p.X + s.Width - 1) Set(x, y, structure, 0);
                    else if (y == p.Y || y == p.Y + s.Height - 1) Set(x, y, structure, 1);
                }
        }
        public static void BuildRectangle(MapModel map, Point p, Size s, string material, bool fill = true)
        {
            Map = map;
            for (int y = p.Y; y < p.Y + s.Height; y++)
                for (int x = p.X; x < p.X + s.Width; x++)
                    if (fill || y == p.Y || x == p.X || x == p.X + s.Width - 1 || y == p.Y + s.Height - 1) Set(x, y, material, 0);
        }
    }
}
