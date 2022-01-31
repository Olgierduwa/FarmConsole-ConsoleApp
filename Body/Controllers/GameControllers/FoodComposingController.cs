using FarmConsole.Body.Controllers.CentralControllers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services.GameServices;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Views.GameViews;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Controllers.GameControllers
{
    class FoodComposingController : HeadController
    {
        private static int SelectedList;
        private static int[] Selected;
        private static int ItemCount;

        private static List<RecipeModel> Recipes { get; set; }
        private static List<ProductModel> Ingredients { get; set; }
        private static List<ProductModel> HiddenIngredients { get; set; }
        private static ProductModel[] PlateProducts { get; set; }

        private static void UpdateSelecetListElement(int value)
        {
            if (Selected[SelectedList] + value <= ItemCount && Selected[SelectedList] + value > 0)
            {
                SoundService.Play("K1");
                Selected[SelectedList] += value;
                ComponentService.UpdateMenuSelect(Selected[SelectedList], Selected[SelectedList] - value, ItemCount, Prop: 23);

                // update Ingredients by Recipes
                if (SelectedList == 0)
                {
                    FoodComposingView.GetRecipesList();
                    Ingredients = Recipes[Selected[0] - 1].GetIngredients(GameInstance.Inventory);
                    FoodComposingView.DisplayIngredients(Ingredients, 0);
                    FoodComposingView.SetRecipesList();
                }
            }
        }
        private static void UpdateSelectList(int value)
        {
            if (SelectedList + value >= 0 && SelectedList + value < Selected.Length)
            {
                SoundService.Play("K1");
                if(SelectedList == 0 && value > 0)
                {
                    SelectedList += value;
                    ItemCount = Ingredients.Count;
                    Selected[SelectedList] = 1;
                    FoodComposingView.GetRecipesList();
                    FoodComposingView.DisplayCaptains(SelectedList);
                    FoodComposingView.SetIngredientsList();
                    ComponentService.UpdateMenuSelect(1, 1, ItemCount);
                }
                else if(SelectedList == 1 && value < 0)
                {
                    FoodComposingView.DisplayIngredients(Ingredients, 0);
                    CancelAction(0);
                    Selected[SelectedList] = 0;
                    ItemCount = Recipes.Count;
                    SelectedList += value;
                    FoodComposingView.DisplayCaptains(SelectedList);
                    FoodComposingView.SetRecipesList();
                }
                else if(SelectedList == 1 && value > 0)
                {
                    SelectedList += value;
                    ItemCount = PlateProducts.Length;
                    Selected[SelectedList] = 1;
                    FoodComposingView.GetIngredientsList();
                    FoodComposingView.DisplayCaptains(SelectedList);
                    FoodComposingView.SetPlateList();
                    ComponentService.UpdateMenuSelect(1, 1, ItemCount);
                }
                else if(SelectedList == 2 && value < 0)
                {
                    ComponentService.UpdateMenuSelect(0, Selected[SelectedList], ItemCount);
                    FoodComposingView.GetPlateList();
                    Selected[SelectedList] = 0;
                    ItemCount = Ingredients.Count;
                    SelectedList += value;
                    FoodComposingView.DisplayCaptains(SelectedList);
                    FoodComposingView.SetIngredientsList();
                }
            }
        }
        private static void TakeAction()
        {
            if (SelectedList == 0) UpdateSelectList(+1);
            else if (SelectedList == 1)
            {
                if (Ingredients.Count > 0)
                {
                    for (int i = 0; i < PlateProducts.Length; i++)
                        if (PlateProducts[i] == null)
                        {
                            var ingredient = Ingredients[Selected[SelectedList] - 1];
                            var plateproduct = ObjectModel.GetObject(ingredient.ObjectName, ingredient.State).ToProduct();
                            plateproduct.Scale = ingredient.Scale;
                            plateproduct.Price = ingredient.Price;
                            PlateProducts[i] = plateproduct;
                            if (ingredient.Scale > 0)
                            {
                                int scale = ingredient.Scale;
                                for (int j = 0; j < Ingredients.Count; j++)
                                    if (Ingredients[j].Scale == scale)
                                    {
                                        HiddenIngredients.Add(Ingredients[j]);
                                        Ingredients.RemoveAt(j);
                                        j--;
                                    }
                                ItemCount = Ingredients.Count;
                                Selected[1] = 1;
                                FoodComposingView.DisplayIngredients(Ingredients, 1);
                            }
                            else
                            {
                                ingredient.AddAmount(-1 * ingredient.Price);
                                if (ingredient.Amount < 1)
                                {
                                    Ingredients.RemoveAt(Selected[SelectedList] - 1);
                                    Selected[SelectedList] = 1;
                                    FoodComposingView.DisplayIngredients(Ingredients, 1);
                                }
                                else
                                {
                                    string amount = " [" + ingredient.Amount + "/" + ingredient.Price + "]";
                                    string state = ingredient.StateName.Length > 0 &&
                                        ingredient.StateName[0] > '@' ? LS.Object(ingredient.StateName) + " " : "";
                                    string product = state + LS.Object(ingredient.ObjectName) + amount;
                                    FoodComposingView.UpdateMenuTextBox(Selected[SelectedList], product, Margin: 0);
                                    FoodComposingView.GetIngredientsList();
                                }
                            }
                            FoodComposingView.DisplayPlate(PlateProducts, 0);
                            FoodComposingView.SetIngredientsList();
                            break;
                        }
                }
            }
            else // zatwierdz kompozycje
            {
                int Hungry = 0;
                int Energy = 0;
                int Immunity = 0;
                for (int i = 0; i < PlateProducts.Length; i++)
                {
                    if (PlateProducts[i] != null)
                    {
                        var PP = PlateProducts[i];
                        if (PP.Property[0] == 'e')
                        {
                            string[] stats = PP.Property[1..].Split(':');
                            int amount = PP.Price;
                            Energy += int.Parse(stats[0]);
                            Hungry += int.Parse(stats[1]);
                            Immunity += (int.Parse(stats[2]) - 1);
                            int Index = GameInstance.Inventory.FindIndex(x => x.ObjectName == PP.ObjectName && x.State == PP.State);
                            GameInstance.Inventory[Index].AddAmount(-1 * amount);
                            if (GameInstance.Inventory[Index].Amount < 1) GameInstance.Inventory.RemoveAt(Index);
                        }
                    }
                }
                Action.Result = GameService.Eat(Energy, Hungry, Immunity);
                FoodComposingView.SetRecipesList();
                FoodComposingView.DisableView();
                FoodComposingView.SetIngredientsList();
                FoodComposingView.DisableView();
                FoodComposingView.SetPlateList();
                FoodComposingView.DisableView();
                FoodComposingView.DisplayCaptains(3);
                if (Action.ResultTitle == "") FoodComposingView.Warning(Action.Result);
                else FoodComposingView.GoodNews(Action.Result, Action.ResultTitle, "", false);
                Action.ResultTitle = "";
                OpenScreen = EscapeScreen;
            }
        }
        private static void CancelAction(int selected = 1)
        {
            if(SelectedList == 0)
            {
                //for (int i = 0; i < SelectedProducts.Length; i++) SelectedProducts[i] = null;
                OpenScreen = EscapeScreen;
            }
            else if(SelectedList == 1)
            {
                RestoreIngredients(selected: selected);
                ItemCount = Ingredients.Count;
                for (int i = 0; i < PlateProducts.Length; i++) PlateProducts[i] = null;
                FoodComposingView.DisplayPlate(PlateProducts, 0);
                FoodComposingView.SetIngredientsList();
            }
            else
            {
                var SP = PlateProducts[Selected[2] - 1];
                if (SP != null)
                {
                    ComponentService.UpdateMenuTextBox(Selected[2], "...");
                    FoodComposingView.GetPlateList();
                    RestoreIngredients(SP.Scale);
                    FoodComposingView.SetPlateList();
                    PlateProducts[Selected[2] - 1] = null;
                }
            }
        }
        private static void RestoreIngredients(int scale = 0, int selected = 1)
        {
            for (int i = 0; i < HiddenIngredients.Count; i++)
            {
                if ((SelectedList != 2 && (scale == 0 || HiddenIngredients[i].Scale == scale)) ||
                    (SelectedList == 2 && HiddenIngredients[i].Scale == scale))
                {
                    Ingredients.Add(HiddenIngredients[i]);
                    HiddenIngredients.RemoveAt(i);
                    i--;
                }
            }
            for (int i = 0; i < PlateProducts.Length; i++)
            {
                if(PlateProducts[i] != null && ((SelectedList == 2 && i == Selected[2] - 1) || (SelectedList < 2 && PlateProducts[i].Scale == 0)))
                {
                    var PP = PlateProducts[i];
                    int Index = Ingredients.FindIndex(x => x.ObjectName == PP.ObjectName && x.State == PP.State);
                    if (Index < 0)
                    {
                        var IP = ObjectModel.GetObject(PP.ObjectName, PP.State).ToProduct();
                        IP.Price = PP.Price;
                        IP.Scale = PP.Scale;
                        IP.Amount = PP.Price;
                        Ingredients.Add(IP);
                    }
                    else if(PP.Scale == 0) Ingredients[Index].AddAmount(PP.Price);
                }
            }
            Selected[1] = 1;
            FoodComposingView.DisplayIngredients(Ingredients, selected);
        }

        public static void Open()
        {
            SelectedList = 0;
            Selected = new int[3] { 1, 1, 1 };
            ComponentService.SetView = MapEngine.Map;

            Recipes = XF.GetFoodRecipes();
            Ingredients = Recipes[Selected[0] - 1].GetIngredients(GameInstance.Inventory);
            HiddenIngredients = new List<ProductModel>();
            PlateProducts = new ProductModel[4];
            ItemCount = Recipes.Count;

            FoodComposingView.DisplayCaptains(0);
            FoodComposingView.DisplayPlate(PlateProducts, 0);
            FoodComposingView.DisplayIngredients(Ingredients, 0);
            FoodComposingView.DisplayRecipes(Recipes, Selected[0]);
            
            while (OpenScreen.Contains("Food"))
            {
                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    switch (cki.Key)
                    {
                        case ConsoleKey.Tab:
                        case ConsoleKey.Escape: OpenScreen = EscapeScreen; SoundService.Play("K2"); break;

                        case ConsoleKey.W: UpdateSelecetListElement(-1); break;
                        case ConsoleKey.S: UpdateSelecetListElement(+1); break;

                        case ConsoleKey.A: UpdateSelectList(-1); break;
                        case ConsoleKey.D: UpdateSelectList(+1); break;

                        case ConsoleKey.Q: CancelAction(); break;
                        case ConsoleKey.E: TakeAction(); break;
                    }
                }
            }
            FoodComposingView.Clean();
        }
    }
}
