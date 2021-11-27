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
        public bool Fill { get; set; }

        public string FieldName { get; set; }
        public Color Color { get; set; }
        public string[] View { get; set; }
        public int[] ViewStartPos { get; set; }


        public void ReloadView(bool view = true)
        {
            var p = ProductModel.GetProduct(this);
            Color = p.Color;
            if (view)
            {
                View = p.View;
                ViewStartPos = p.ViewStartPos;
            }
        }
        public FieldModel(FieldModel field)
        {
            Category = field.Category;
            Scale = field.Scale;
            Type = field.Type;
            State = field.State;
            Duration = field.Duration;
            Fill = field.Fill;

            FieldName = field.FieldName.ToString();
            Color = Color.FromArgb(field.Color.ToArgb());
            View = new string[field.View.Length];
            field.View.CopyTo(View, 0);
            ViewStartPos = new int[field.ViewStartPos.Length];
            field.ViewStartPos.CopyTo(ViewStartPos, 0);
        }
        public FieldModel(ProductModel product)
        {
            Category = product.Category;
            Scale = product.Scale;
            Type = product.Type;
            State = product.State;
            Color = product.Color;
            FieldName = product.ProductName;
            View = product.View;
            ViewStartPos = product.ViewStartPos;
            Fill = product.Fill;
            Duration = 0;
        }
        public FieldModel(string ModelAsString, int scale)
        {
            int[] Values = ConvertService.ConvertStringToField(ModelAsString);
            Category = Values[0];
            State = Values[1];
            Type = Values[2];
            Duration = Values[3];
            Scale = scale;

            ProductModel P = ProductModel.GetProduct(Category, Scale, Type, State);
            FieldName = P.ProductName;
            Color = P.Color;
            View = P.View;
            ViewStartPos = P.ViewStartPos;
            Fill = P.Fill;
        }

        public override string ToString()
        {
            return ConvertService.ConvertFieldToString(Category, State, Type, Duration);
        }
    }
}
