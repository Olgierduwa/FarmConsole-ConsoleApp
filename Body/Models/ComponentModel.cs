using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Models
{
    public class CM
    {
        private const ConsoleColor static_base_color = ConsoleColor.DarkGray;
        private const ConsoleColor static_content_color = ConsoleColor.White;

        public int ID_group { get; }
        public int ID_object { get; }
        public int PosX { get; }
        public int PosY { get; }
        public int Width { get; }
        public int Height { get; set; }
        public string[] View { get; set; }
        public int Prop { get; set; }
        public string Name { get; set; } // jedynie do testów
        public bool? Show { get; set; }
        public ConsoleColor Base_color { get; set; }
        public ConsoleColor Content_color { get; set; }

        public CM(int id_group, int id_object, int posX, int posY, int width, int height, string[] view, string name, int prop = 0, bool? show = true,
                         ConsoleColor color1 = static_base_color, ConsoleColor color2 = static_content_color)
        {
            this.ID_group = id_group;
            this.ID_object = id_object;
            this.PosX = posX;
            this.PosY = posY;
            this.Width = width;
            this.Height = height;
            this.View = view;
            this.Prop = prop;
            this.Name = name;
            this.Show = show;
            this.Base_color = color1;
            this.Content_color = color2;
        }
    }
}
