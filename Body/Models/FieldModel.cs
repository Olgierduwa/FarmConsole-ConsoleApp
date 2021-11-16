using FarmConsole.Body.Services;
using System;
using System.Drawing;

namespace FarmConsole.Body.Models
{
    public class FieldModel
    {
        public int Category { get; set; }
        public int Scale { get; set; }
        public int Type { get; set; }
        public int State { get; set; }
        public int Duration { get; set; }

        public string FieldName { get; set; }
        public Color Color { get; set; }

        public FieldModel(FieldModel field)
        {
            Category = field.Category;
            Scale = field.Scale;
            Type = field.Type;
            State = field.State;
            Duration = field.Duration;
            FieldName = field.FieldName;
            Color = field.Color;
        }
        public FieldModel(ProductModel product)
        {
            Category = product.Category;
            Scale = product.Scale;
            Type = product.Type;
            State = product.State;
            Color = product.Color;
            FieldName = product.ProductName;
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
            FieldName = ProductModel.GetProduct(Category, Scale, Type, State).ProductName;
            Color = ProductModel.GetProduct(Category, Scale, Type, State).Color;
        }
        public FieldModel() { }

        public override string ToString()
        {
            return ConvertService.ConvertToSixtyTripleSystem(Category, State, Type, Duration);
        }
    }
}
