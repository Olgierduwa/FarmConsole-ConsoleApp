using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Models
{
    class ActionModel
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Result { get; set; } 
        public string ResultTitle = "";
        public bool IsInProcess { get; set; }
        private ProductModel _SelectedProduct { get; set; }
        public ProductModel GetSelectedProduct { get => _SelectedProduct; }
        public void SetSelectedProduct(ProductModel product) => _SelectedProduct = product;
        public void SetPropertyProduct()
        {
            string[] _NameString = GetSelectedProduct.Property.Split(':');
            _SelectedProduct = ObjectModel.GetObject(_NameString[0]).ToProduct();

            for (int i = 1; i < _NameString.Length; i++)
                if (_NameString[i][0] == 'x') _SelectedProduct.Amount = int.Parse(_NameString[i][1..]);
        }
        public void SetProductByName(string Name) => _SelectedProduct = ObjectModel.GetObject(Name).ToProduct();
    }
}
