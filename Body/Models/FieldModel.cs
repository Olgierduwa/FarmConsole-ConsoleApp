using System;
using System.Collections.Generic;
using System.Drawing;

namespace FarmConsole.Body.Models
{
    class FieldModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Type { get; set; }
        public int Duration { get; set; }
        public int Category { get; set; }

        public FieldModel(int x, int y, int category, int type, int duration = 0)
        {
            X = x;
            Y = y;
            Category = category;
            Type = type;
            Duration = duration;
        }
        public FieldModel(FieldModel field)
        {
            X = field.X;
            Y = field.Y;
            Category = field.Category;
            Type = field.Type;
            Duration = field.Duration;
        }

        public void Move(Point p)
        {
            X += p.X;
            Y += p.Y;
        }
    }
}
