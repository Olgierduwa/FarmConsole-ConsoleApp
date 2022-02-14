using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Models
{
    public class RecipeModel
    {
        public string RecipeName { get; set; }
        private List<ProductModel> Ingredients { get; set; }
        public List<ProductModel> GetIngredients(List<ProductModel> Inventory)
        {
            List<ProductModel> Products = new List<ProductModel>();
            foreach(var IP in Ingredients)
            {
                int Index = Inventory.FindIndex(x => x.ObjectName == IP.ObjectName && x.State == IP.State);
                var ingredient = ObjectModel.GetObject(IP.ObjectName, IP.State).ToProduct();
                ingredient.Scale = IP.Scale;
                ingredient.Price = IP.Price;
                if(Index < 0) ingredient.Amount = 0; 
                else ingredient.Amount = Inventory[Index].Amount; 
                Products.Add(ingredient);
            }
            return Products;
        }
        public RecipeModel(string _RecipeName, List<ProductModel> _IngredientsIDS)
        {
            RecipeName = _RecipeName;
            Ingredients = _IngredientsIDS;
        }
        public override string ToString() => RecipeName;
    }
}
