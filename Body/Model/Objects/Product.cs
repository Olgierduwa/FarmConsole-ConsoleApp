using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Model.Objects
{
    public class Product
    {
        public int category;
        public int type;
        public int amount;
        public string name;
        public decimal price;

        public Product()
        {
            this.category = 0;
            this.type = 0;
            this.amount = 0;
            this.name = "- BRAK PRODUKTU -";
            this.price = 0;
        }
        public Product(int cat, int type, int amount)
        {
            this.category = cat;
            this.type = type;
            this.amount = amount;
            this.name = "NAZWA PRODUKTU";
            this.price = 0;
        }
    }
}
