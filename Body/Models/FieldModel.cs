using System;
using System.Collections.Generic;
using System.Drawing;

namespace FarmConsole.Body.Models
{
    class FieldModel
    {
        public int Type { get; set; }
        public int Duration { get; set; }
        public int Category { get; set; }

        public FieldModel(int category, int type, int duration = 0)
        {
            Category = category;
            Type = type;
            Duration = duration;
        }
        public FieldModel(FieldModel field)
        {
            Category = field.Category;
            Type = field.Type;
            Duration = field.Duration;
        }
    }
}
