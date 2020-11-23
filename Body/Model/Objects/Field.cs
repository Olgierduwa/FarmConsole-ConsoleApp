using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Model.Objects
{
    class Field
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Type { get; set; }
        public int Duration { get; set; }
        public bool Selected { get; set; }

        public Field(int x, int y, int type, int duration = 0)
        {
            this.X = x;
            this.Y = y;
            this.Type = type;
            this.Duration = duration;
            this.Selected = false;
        }
    }
}
