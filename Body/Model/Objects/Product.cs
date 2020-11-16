using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Model.Objects
{
    public class Product
    {
        public int id;
        public string name;
        public decimal price;

        public Product(string id, string name, string price)
        {
            this.id = Int32.Parse(id);
            this.name = name;
            this.price = Decimal.Parse(price);
        }
        public Product(string name, string price)
        {
            this.id = 0;
            this.name = name;
            this.price = Decimal.Parse(price);
        }
        public Product()
        {
            this.id = 0;
            this.name = "- BRAK PRODUKTU -";
            this.price = 0;
        }
    }
}
