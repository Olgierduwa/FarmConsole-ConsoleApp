using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace FarmConsole.Body.Models
{
    public class ProductModel : ObjectModel
    {
        public int Amount { get; set; }
        public bool AddAmount(int value)
        {
            Amount += value;
            if (Amount <= 0) return false;
            else return true;
        }

        public string ToString(bool BaseString = false)
        {
            if (BaseString) return base.ToString();
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
        public static List<ProductModel> SelectProductsByAction(List<ProductModel> _Products, string _Action, int? Category = null, bool _SearchMapActs = true)
        {
            List<ProductModel> SelectedProducts = new List<ProductModel>();
            foreach (var P in _Products)
                if (Category != null && P.Category == Category || Category == null)
                    if (_SearchMapActs) { foreach (var act in P.MapActions) if (act == _Action) SelectedProducts.Add(P); }
                    else { foreach (var act in P.MenuActions) if (act == _Action) SelectedProducts.Add(P); }
            return SelectedProducts;
        }
    }
}
