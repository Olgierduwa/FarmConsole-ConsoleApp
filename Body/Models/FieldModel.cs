using FarmConsole.Body.Services;
using System;

namespace FarmConsole.Body.Models
{
    public class FieldModel
    {
        public int Category { get; set; }
        public int Scale { get; set; }
        public int Type { get; set; }
        public int State { get; set; }
        public int Duration { get; set; }

        public FieldModel(int category, int scale, int type, int state = 0, int duration = 0)
        {
            Category = category;
            Scale = scale;
            Type = type;
            State = state;
            Duration = duration;
        }
        public FieldModel(FieldModel field)
        {
            Category = field.Category;
            Scale = field.Scale;
            Type = field.Type;
            State = field.State;
            Duration = field.Duration;
        }
        public FieldModel(ProductModel product)
        {
            Category = product.Category;
            Scale = product.Scale;
            Type = product.Type;
            State = product.State;
            Duration = 0;
        }
        public FieldModel(string ModelAsString, int scale)
        {
            int[] Values = ConvertService.ConvertToDecimalSystem(ModelAsString);
            Category = Values[0];
            State = Values[1];
            Type = Values[2];
            Duration = Values[3];
            Scale = scale;
        }
        public FieldModel() { }

        public override string ToString()
        {
            return ConvertService.ConvertToSixtyTripleSystem(Category, State, Type, Duration);
        }
    }
}
