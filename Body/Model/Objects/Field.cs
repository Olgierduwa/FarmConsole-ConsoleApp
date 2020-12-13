using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Model.Objects
{
    class Field
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Type { get; set; }
        public int Duration { get; set; }
        public int Category { get; set; }

        public Field(int x, int y, int category, int type, int duration = 0)
        {
            this.X = x;
            this.Y = y;
            this.Category = category;
            this.Type = type;
            this.Duration = duration;
        }
        public void Move(Point p)
        {
            X += p.X;
            Y += p.Y;
        }
    }
}
