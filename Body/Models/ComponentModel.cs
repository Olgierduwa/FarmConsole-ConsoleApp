using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Models
{
    public class CM
    {
        private static readonly Color static_base_color = ColorService.GetColorByName("gray3");
        private static readonly Color static_content_color = ColorService.GetColorByName("White");

        public int ID_group { get; }
        public int ID_object { get; }
        public string Name { get; set; }
        public bool? Show { get; set; }
        public string[] View { get; set; }
        public int Prop { get; set; }
        public Point Pos { get; set; }
        public Size Size { get; set; }
        public Color Base_color { get; set; }
        public Color Content_color { get; set; }

        public CM Copy()
        {
            CM cm = (CM)this.MemberwiseClone();
            cm.Size = new Size(this.Size.Width, this.Size.Height);
            cm.Pos = new Point(this.Pos.X, this.Pos.Y);
            cm.Base_color = Color.FromArgb(Base_color.ToArgb());
            cm.Content_color = Color.FromArgb(Content_color.ToArgb());
            return cm;
        }

        public override string ToString()
        {
            return "".PadLeft(ID_group,' ') + Name + "   " + Pos.X + "X," + Pos.Y + "Y, " + Size.Width + "W, " + Size.Height + "H";
        }

        public CM(int id_group, int id_object, Point pos, Size size, string[] view, string name, int prop = 0, bool? show = true,
                         Color color1 = new Color(), Color color2 = new Color())
        {
            color1 = color1 == new Color() ? static_base_color : color1;
            color2 = color2 == new Color() ? static_content_color : color2;

            this.ID_group = id_group;
            this.ID_object = id_object;
            this.Pos = pos;
            this.Size = size;
            this.View = view;
            this.Prop = prop;
            this.Name = name;
            this.Show = show;
            this.Base_color = color1;
            this.Content_color = color2;
        }
    }
}
