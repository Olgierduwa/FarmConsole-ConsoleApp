using FarmConsole.Body.Controllers.CentralControllers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Views.GameViews;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Controllers.GameControllers
{
    class StoveController : HeadController
    {
        private static int Selected;
        private static int LastSelected;
        private static string ActionName;
        private static bool IsInProgress;
        private static bool Stove;
        private static FieldModel Field;

        private static List<ProductModel> IngredientProducts { get; set; }
        private static ProductModel[] StoveProducts { get; set; }

        private static void UpdateSelecet(int value)
        {
            if (!Stove && Selected + value <= IngredientProducts.Count && Selected + value > 0)
            {
                SoundService.Play("K1");
                Selected += value;
                ComponentService.UpdateMenuSelect(Selected, Selected - value, IngredientProducts.Count, Prop: 23);
            }
            else if (Stove && Selected + value > 0 && Selected + value <= StoveProducts.Length)
            {
                SoundService.Play("K1");
                if (Selected == 4 && value > 0)
                {
                    ComponentService.UpdateMenuSelect(5, 4, 4, 2, 27);
                    StoveView.GetRightList();
                    StoveView.SetLeftList();
                    ComponentService.UpdateMenuTextBox(2, LS.Navigation("remove from the oven", " [E]"), 3);
                    StoveView.SetRightList();
                }
                else if (Selected == 5 && value < 0)
                {
                    ComponentService.UpdateMenuSelect(0, 1, 2, 3);
                    StoveView.GetRightList();
                    StoveView.SetLeftList();
                    ComponentService.UpdateMenuTextBox(2, LS.Navigation("remove from the stove", " [E]"), 3);
                    StoveView.SetRightList();
                }
                Selected += value;
                if (Selected < 5) ComponentService.UpdateMenuSelect(Selected, Selected - value, 4, 2, 27);
                else ComponentService.UpdateMenuSelect(Selected - 4, Selected - 4 - value, 2, 3);
                ShowProgress();
            }
        }
        private static void SwitchStove()
        {
            if (!Stove)
            {
                var CurrentSelected = Selected;
                Selected = LastSelected;
                LastSelected = CurrentSelected;
                string WarningMessage = Selected > 4 ? "oven" : "stove";
                StoveView.GetLeftList();
                ComponentService.UpdateMenuTextBox(2, LS.Navigation("remove from the " + WarningMessage, " [E]"), 3);
                ComponentService.UpdateMenuTextBox(3, LS.Navigation("take items", " [A]"), 3);
                ComponentService.UpdateMenuSelect(IngredientProducts.Count + 1, LastSelected, 1, 2, 23);

                StoveView.SetRightList();
                ComponentService.UpdateMenuSelect(Selected - (Selected > 4 ? 4 : 0), Selected - (Selected > 4 ? 4 : 0), Selected > 4 ? 2 : 4, Selected > 4 ? 3 : 2, 27);
                StoveView.GetRightList();
                ShowProgress();
                Stove = !Stove;
            }
        }
        private static void SwitchItems()
        {
            if (Stove)
            {
                StoveView.GetRightList();
                ComponentService.UpdateMenuSelect(Selected > 4 ? 3 : 5, Selected - (Selected > 4 ? 4 : 0), Selected > 4 ? 2 : 4, Selected > 4 ? 3 : 2, 21);
                ShowProgress();

                var CurrentSelected = Selected;
                Selected = LastSelected;
                LastSelected = CurrentSelected;
                StoveView.SetLeftList();
                ComponentService.UpdateMenuTextBox(2, LS.Action(ActionName, " [E]").ToUpper(), 3);
                ComponentService.UpdateMenuTextBox(3, LS.Navigation("manage dishes", " [D]"), 3);
                ComponentService.UpdateMenuSelect(Selected, Selected, IngredientProducts.Count, 2, 23);
                Stove = !Stove;
            }
        }
        private static void Transfer()
        {
            if (Stove && StoveProducts[Selected - 1] != null) Drop();
            else if (!Stove && IngredientProducts.Count >= Selected) Put();
        }
        private static void Put()
        {
            int PlaceIndex = ActionName == "bake" ? 4 : 0;
            int MaxCount = ActionName == "bake" ? 6 : 4;
            string WarningMessage = ActionName == "bake" ? "oven" : "stove";
            while (PlaceIndex < MaxCount && StoveProducts[PlaceIndex] != null) PlaceIndex++;
            if (PlaceIndex < MaxCount)
            {
                var PlacedProduct = IngredientProducts[Selected - 1].ToProduct();
                string StateName = PlacedProduct.StateName.Split(' ')[0] + " " + ActionName;
                PlacedProduct.StateName = StateName;
                PlacedProduct.Amount = 1;
                StoveView.GetLeftList();
                Field.Pocket.SetSlot(PlaceIndex, PlacedProduct);
                StoveView.DisplayStove(StoveProducts, 0);
                LastSelected = 1;
                StoveView.GetRightList();

                IsInProgress = true;
                int PiecesAmount = IngredientProducts[Selected - 1].Slots < 0 ? IngredientProducts[Selected - 1].Slots : -1;
                int Index = GameInstance.Inventory.FindIndex(x => x.ObjectName == PlacedProduct.ObjectName && x.State == 0);
                if (!GameInstance.Inventory[Index].AddAmount(PiecesAmount))
                {
                    StoveView.SetLeftList();
                    StoveView.RepairDamageRemoval(IngredientProducts.Count);
                    GameInstance.Inventory.RemoveAt(Index);
                    IngredientProducts.RemoveAt(Selected - 1);
                    Selected = 1;
                    StoveView.DisplayCaptains(ActionName, IngredientProducts, 1);
                }
                else
                {
                    StoveView.SetLeftList();
                    string Content = (IngredientProducts[Selected - 1].StateName.Length > 0 && IngredientProducts[Selected - 1].StateName[0] > '@' ?
                                     LS.Object(IngredientProducts[Selected - 1].StateName) + " " : "") +  LS.Object(IngredientProducts[Selected - 1].ObjectName) +
                                     " : " + IngredientProducts[Selected - 1].Amount + LS.Navigation(IngredientProducts[Selected - 1].Unit.Split('/')[0], " ", Before: " ");
                    ComponentService.UpdateMenuTextBox(Selected, Content);
                }
            }
            else
            {
                StoveView.GetLeftList();
                StoveView.SetRightList();
                ComponentService.DisableView();
                StoveView.SetLeftList();
                ComponentService.Warning(LS.Action("no space on " + WarningMessage));
                StoveView.SetRightList();
                ComponentService.DisableView(true);
                StoveView.SetLeftList();
            }
        }
        private static void Drop()
        {
            var SP = StoveProducts[Selected - 1].ToProduct();
            StoveProducts[Selected - 1] = null;
            if (SP.Amount < 80) SP.State = 0;
            SP.Amount = SP.Slots < 0 ? SP.Slots * -1 : 1;
            int Index = GameInstance.Inventory.FindIndex(x => x.ObjectName == SP.ObjectName && x.State == SP.State);
            if (Index < 0) GameInstance.Inventory.Add(SP);
            else GameInstance.Inventory[Index].AddAmount(SP.Amount);
            SetProducts();
            LastSelected = Selected = 1;
            StoveView.DisplayCaptains(ActionName, IngredientProducts, LastSelected, IsSelected: false);
            ComponentService.UpdateMenuTextBox(2, LS.Navigation("remove from the stove", " [E]"), 3);
            ComponentService.UpdateMenuTextBox(3, LS.Navigation("take items", " [A]"), 3);
            StoveView.GetLeftList();
            StoveView.DisplayStove(StoveProducts, Selected);
        }
        private static void MakeProgress()
        {
            if(!Stove)
            {
                StoveView.GetLeftList();
                StoveView.SetRightList();
            }
            IsInProgress = false;
            for (int i = 0; i < StoveProducts.Length; i++)
            {
                var p = StoveProducts[i];
                if (p != null)
                {
                    IsInProgress = true;
                    ComponentService.SetProgressBar(2 + (i > 3 ? 1 : 0), i + 1 - (i > 3 ? 4 : 0), p.Amount);
                    if (p.Amount < 100 && p.State == 0) p.AddAmount(1);
                    else if (p.State == 0)
                    {
                        p.State = ObjectModel.GetObject(p.ObjectName,_StateName: p.StateName).State; // zmien stan na gotowe
                    }
                }
            }
            if (!Stove) StoveView.SetLeftList();
        }
        private static void ShowProgress()
        {
            for (int i = 0; i < StoveProducts.Length; i++)
                if (StoveProducts[i] != null)
                    ComponentService.SetProgressBar(2 + (i > 3 ? 1 : 0), i + 1 - (i > 3 ? 4 : 0), StoveProducts[i].Amount);
        }
        private static void SetProducts()
        {
            var InventoryProducts = ProductModel.SelectProductsByAction(GameInstance.Inventory, ActionName, Category: 4, _SearchMapActs: true);
            IngredientProducts = new List<ProductModel>();
            foreach (var ip in InventoryProducts)
            {
                var p = ip.ToProduct();
                p.Amount /= p.Slots < 0 ? (-1 * p.Slots) : 1;
                if(p.Amount > 0) IngredientProducts.Add(p);
            }
        }

        public static void Open(string _ActionName)
        {
            Stove = false;
            Selected = 1;
            LastSelected = 1;
            ActionName = _ActionName;
            ComponentService.SetView = MapEngine.Map;

            SetProducts();
            Field = MapEngine.GetField();
            StoveProducts = Field.Pocket.GetSlots;
            IsInProgress = false;
            foreach (var sp in StoveProducts) if (sp != null) IsInProgress = true;

            StoveView.DisplayStove(StoveProducts, 0);
            StoveView.GetRightList();
            StoveView.DisplayCaptains(ActionName, IngredientProducts, Selected);
            
            while (OpenScreen.Contains("Food"))
            {
                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    switch (cki.Key)
                    {
                        case ConsoleKey.Q:
                        case ConsoleKey.Tab:
                        case ConsoleKey.Escape: OpenScreen = EscapeScreen; SoundService.Play("K2"); break;
                        case ConsoleKey.W: UpdateSelecet(-1); break;
                        case ConsoleKey.S: UpdateSelecet(+1); break;
                        case ConsoleKey.D: SwitchStove(); break;
                        case ConsoleKey.A: SwitchItems(); break;
                        case ConsoleKey.E: Transfer(); break;
                    }
                }
                else if ((DateTime.Now - Previously).TotalMilliseconds >= BakeTime)
                {
                    if(IsInProgress) MakeProgress();
                    Previously = DateTime.Now;
                }
            }
            ComponentService.Clean(WithCleaning: true);
        }
    }
}
