using FarmConsole.Body.Models;
using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FarmConsole.Body.Views.GameViews
{
    class FoodComposingView : ComponentService
    {
        public static void DisplayCaptains(int SelectedList)
        {
            ClearList(false);

            Endl(3);
            H2(LS.Navigation("compose label"));
            Endl(2);
            GroupStart(0);

            GroupStart(1, 3);
            TextBox(LS.Navigation("recipes"));
            Endl(Console.WindowHeight - 21);
            TextBox(LS.Navigation("choose a recipe"," [E]"), Foreground: ColorService.GetColorByName("limed"));
            TextBox(LS.Navigation("disconnect the composition", " [Q]"), Foreground: ColorService.GetColorByName("redl"));
            GroupEnd();
            if (SelectedList != 0) { SetAvailability(2, 2, false); SetAvailability(2, 3, false); }

            GroupStart(3);
            TextBox(LS.Navigation("ingredients"));
            Endl(Console.WindowHeight - 21);
            TextBox(LS.Navigation("choose an ingredient", " [E]"), Foreground: ColorService.GetColorByName("limed"));
            TextBox(LS.Navigation("clean the composition", " [Q]"), Foreground: ColorService.GetColorByName("red"));
            GroupEnd();
            if (SelectedList != 1) { SetAvailability(3, 2, false); SetAvailability(3, 3, false);}

            GroupStart(3, 3);
            TextBox(LS.Navigation("plate"));
            Endl(Console.WindowHeight - 21);
            TextBox(LS.Navigation("compose and eat the dish", " [E]"), Foreground: ColorService.GetColorByName("limed"));
            TextBox(LS.Navigation("give up the ingredient", " [Q]"), Foreground: ColorService.GetColorByName("redl"));
            GroupEnd();
            if (SelectedList != 2) { SetAvailability(4, 2, false); SetAvailability(4, 3, false); }

            GroupEnd();
            if (SelectedList == 3) DisableView();
            else PrintList();
        }
        public static void DisplayRecipes(List<RecipeModel> recipes, int selected)
        {
            ClearList(false);

            Endl(10);
            GroupStart(0);

            GroupStart(1, 3);
            Endl(1);

            int Height = Console.WindowHeight - 23;
            for (int i = 0; i < recipes.Count; i++)
            {
                if (i < Height / 3) TextBox(LS.Object(recipes[i].RecipeName));
                else TextBox(LS.Object(recipes[i].RecipeName), Show: false);
            }

            GroupEnd();
            
            GroupEnd();
            PrintList();
            UpdateSelect(selected, selected, recipes.Count);
            GetRecipesList();
        }
        public static void DisplayIngredients(List<ProductModel> products, int selected)
        {
            if (IngredientsList != null)
            {
                SetIngredientsList();
                if(products.Count < GetGroupCount())
                RepairDamageRemoval(products.Count + 1);
            }
            ClearList(false);

            Endl(10);
            GroupStart(0);

            GroupStart(3);
            Endl(1);
            int Height = Console.WindowHeight - 23;
            for (int i = 0; i < products.Count; i++)
            {
                string amount = " [" + products[i].Amount + "/" + products[i].Price + "]";
                string state = products[i].StateName.Length > 0 &&
                    products[i].StateName[0] > '@' ? LS.Object(products[i].StateName) + " " : "";
                string product = state + LS.Object(products[i].ObjectName) + amount;
                if (i < Height / 3) TextBox(product, Margin: 0);
                else TextBox(product, Show: false, Margin: 0);
            }
            GroupEnd();

            GroupStart(3);
            Endl(Height + 1);
            GroupEnd();

            GroupEnd();
            PrintList();
            UpdateSelect(selected, selected, products.Count);
            GetIngredientsList();

            ComponentsDisplayed.Clear();
            ComponentsDisplayed.Add(GetComponentByName("GS",3));
        }
        public static void DisplayPlate(ProductModel[] products, int selected)
        {
            if (PlateList != null) SetPlateList();
            ClearList(false);

            Endl(10);
            GroupStart(0);

            GroupStart(3, 3);
            Endl(1);

            int Height = Console.WindowHeight - 23;
            for (int i = 0; i < products.Length; i++)
            {
                if (i < Height / 3)
                {
                    if (products[i] != null) TextBox(LS.Object(products[i].ObjectName));
                    else TextBox("...");
                }
                else
                {
                    if (products[i] != null) TextBox(LS.Object(products[i].ObjectName), Show: false);
                    else TextBox("...", Show:false);
                }
            }

            GroupEnd();

            GroupEnd();
            PrintList();
            UpdateSelect(selected, selected, products.Length);
            GetPlateList();
        }

        public static void Clean()
        {
            RecipesList = null;
            IngredientsList = null;
            PlateList = null;
            Clean(WithCleaning: true);
        }

        private static List<CM> RecipesList { get; set; }
        private static List<CM> IngredientsList { get; set; }
        private static List<CM> PlateList { get; set; }

        public static void GetRecipesList() => RecipesList = ComponentList.ToList();
        public static void GetIngredientsList() => IngredientsList = ComponentList.ToList();
        public static void GetPlateList() => PlateList = ComponentList.ToList();

        public static void SetRecipesList() => ComponentList = RecipesList.ToList();
        public static void SetIngredientsList() => ComponentList = IngredientsList.ToList();
        public static void SetPlateList() => ComponentList = PlateList.ToList();
    }
}
