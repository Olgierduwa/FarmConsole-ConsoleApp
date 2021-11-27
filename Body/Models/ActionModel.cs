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
        public bool IsInProcess { get; set; }
        public ProductModel SelectedProduct { get; set; }
        public void SetPropertyProduct() => SelectedProduct = ProductModel.GetProduct(SelectedProduct.Property);
    }
}
