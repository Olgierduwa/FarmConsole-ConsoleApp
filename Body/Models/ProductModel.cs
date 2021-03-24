using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Models
{
    public class ProductModel
    {
        public int category;
        public int type;
        public int amount;
        public string name;
        public decimal price;

        public ProductModel()
        {
            category = 0;
            type = 0;
            amount = 0;
            name = "- BRAK PRODUKTU -";
            price = 0;
        }
        public ProductModel(int cat, int type, int amount)
        {
            category = cat;
            this.type = type;
            this.amount = amount;
            name = "NAZWA PRODUKTU";
            price = 0;
        }
    }
}
