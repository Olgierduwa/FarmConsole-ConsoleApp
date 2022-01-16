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
        public ProductModel SelectedProduct { get; set; }
        public void SetPropertyProduct() => SelectedProduct = ObjectModel.GetObject(SelectedProduct.Property).ToProduct();
        public void SetProductByName(string Name) => SelectedProduct = ObjectModel.GetObject(Name).ToProduct();
    }
}
