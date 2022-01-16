using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace FarmConsole.Body.Models
{
    public class ProductModel : ObjectModel
    {
        public int Amount { get; set; }

        public override string ToString()
        {
            return ConvertService.ConvertProductToString(Category, Scale, State, Type, Amount);
        }
        public ProductModel() { }
        public ProductModel(string ModelAsString)
        {
            int[] Values = ConvertService.ConvertStringToProduct(ModelAsString);
            Category = Values[0];
            Scale = Values[1];
            State = Values[2];
            Type = Values[3];
            Amount = Values[4];
        }
        public static List<ProductModel> SelectProductsByAction(List<ProductModel> _Products, string _Action)
        {
            List<ProductModel> SelectedProducts = new List<ProductModel>();
            foreach (var P in _Products)
                if (P.MenuActions.Length > 0 && P.MenuActions[0] == _Action)
                    SelectedProducts.Add(P);
            return SelectedProducts;
        }
    }
}
