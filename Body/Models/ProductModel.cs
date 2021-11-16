using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace FarmConsole.Body.Models
{
    public class ProductModel
    {
        public int Category { get; set; }
        public int Scale { get; set; }
        public int Type { get; set; }
        public string StateName { get; set; }
        public string ProductName { get; set; }
        public string Property { get; set; }
        public decimal Price { get; set; }
        public Color Color { get; set; }
        public string[] MenuActions { get; set; }
        public string[] MapActions { get; set; }
        public string[] View { get; set; }
        public int[] ViewStartPos { get; set; }
        public int State { get; set; }
        public int Duration { get; set; }
        public int Amount { get; set; }

        private static List<ProductModel> Products;

        public ProductModel()
        {
            Category = 0;
            Scale = 0;
            Type = 0;
            State = 0;
            StateName = "- STAN NIEZNANY -";
            ProductName = "- NAZWA NIEZNANA -";
            Property = "";
            Price = 0;
            Color = Color.FromName("Magenta");
            Duration = 0;
            Amount = 1;
            MenuActions = new string[0];
            MapActions = new string[0];
        }
        public ProductModel(FieldModel field)
        {
            Category = field.Category;
            Scale = field.Scale;
            Type = field.Type;
            State = field.State;
            ProductModel PM = GetProduct(Category, Scale, Type, State);
            StateName = PM.StateName;
            ProductName = PM.ProductName;
            Property = PM.Property;
            Price = PM.Price;
            Color = PM.Color;
            MenuActions = PM.MenuActions;
            MapActions = PM.MapActions;
            View = PM.View;
            Amount = 1;
            Duration = 0;
        }
        public ProductModel(string ModelAsString)
        {
            int[] Values = ConvertService.ConvertToDecimalSystem(ModelAsString);
            Category = Values[0];
            Scale = Values[1];
            Type = Values[2];
            Amount = Values[3];
            State = 0;
            ProductModel PM = GetProduct(Category, Scale, Type, State);
            StateName = PM.StateName;
            ProductName = PM.ProductName;
            Property = PM.Property;
            Price = PM.Price;
            Color = PM.Color;
            MenuActions = PM.MenuActions;
            MapActions = PM.MapActions;
            View = PM.View;
            Duration = PM.Duration;
        }
        
        public override string ToString()
        {
            return ConvertService.ConvertToSixtyTripleSystem(Category, Scale, Type, Amount);
        }
        public static void SetProducts()
        {
            Products = Services.XF.GetProducts();
            foreach (var Product in Products)
            {
                string[] View = Product.View;
                int[] StartViewPos = new int[View.Length];
                for (int Line = 0; Line < View.Length; Line++)
                {
                    int countChar = 0;
                    while (countChar < View[Line].Length && View[Line][countChar] == '´') countChar++;
                    View[Line] = View[Line].TrimStart('´');
                    View[Line] = View[Line].TrimEnd('´');
                    StartViewPos[Line] = countChar;
                }
                Product.View = View;
                Product.ViewStartPos = StartViewPos;
            }
        }
        public static ProductModel GetProduct(string _ProductName, int _State = 0, string _StateName = "")
        {
            int Index = 0;
            while (Index < Products.Count)
            {
                if (Products[Index].ProductName == _ProductName &&
                  ((_StateName != "" && Products[Index].StateName == _StateName) ||
                   (_StateName == "" && Products[Index].State == _State))) break;
                Index++;
            }
            if (Index == Products.Count) return Products[3]; // error
            return Products[Index];
        }
        public static ProductModel GetProduct(FieldModel Field)
        {
            int Index = 0;
            while (Index < Products.Count)
            {
                if (Products[Index].Category == Field.Category &&
                    Products[Index].Scale == Field.Scale &&
                    Products[Index].Type == Field.Type &&
                    Products[Index].State == Field.State) break;
                Index++;
            }
            if (Index == Products.Count) return Products[3]; // error
            return Products[Index];
        }
        public static ProductModel GetProduct(int Category, int Scale, int Type, int State)
        {
            int Index = 0;
            while (Index < Products.Count)
            {
                if (Products[Index].Category == Category &&
                    Products[Index].Scale == Scale &&
                    Products[Index].Type == Type &&
                    Products[Index].State == State) break;
                Index++;
            }
            if (Index == Products.Count) return Products[3]; // error
            return Products[Index];
        }
        public static List<ProductModel> GetProducts(int? Category = null, int? Scale = null, int? Type = null, int? State = null)
        {
            List<ProductModel> products = new List<ProductModel>();
            if (Category != null || Scale != null || Type != null || State != null)
                foreach (var p in Products)
                {
                    if ((Category == null || p.Category == Category) &&
                        (Scale == null || p.Scale == Scale) &&
                        (Type == null || p.Type == Type) &&
                        (State == null || p.State == State))
                        products.Add(p);
                }
            return products;
        }
        public static ProductModel GetRedirectProduct(ProductModel Product)
        {
            string RedirectString = Product.Property;
            // redirect: cat:0-8 scale: 0-9 state: 0-9 type: 0-999
            int category = Convert.ToInt32(RedirectString.Substring(0, 1));
            int scale = Convert.ToInt32(RedirectString.Substring(1, 1));
            int state = Convert.ToInt32(RedirectString.Substring(2, 1));
            int type = Convert.ToInt32(RedirectString[3..]);
            return GetProduct(category, scale, type, state);
        }
        public static List<ProductModel> GetProductsByAction(List<ProductModel> List, string Action)
        {
            List<ProductModel> SelectedProducts = new List<ProductModel>();
            foreach (var P in List)
                if (P.MenuActions.Length > 0 && P.MenuActions[0] == Action)
                    SelectedProducts.Add(P);
            return SelectedProducts;
        }
    }
}
