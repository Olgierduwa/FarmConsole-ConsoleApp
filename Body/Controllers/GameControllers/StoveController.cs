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

        private static List<ProductModel> Products { get; set; }
        private static List<ProductModel> InventoryProducts { get; set; }
        private static ProductModel[] StoveProducts { get; set; }

        private static void UpdateSelecet(int value)
        {
            if (!Stove && Selected + value <= Products.Count && Selected + value > 0)
            {
                SoundService.Play("K1");
                Selected += value;
                ComponentService.UpdateMenuSelect(Selected, Selected - value, Products.Count, Prop: 23);
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
        private static void Switch()
        {
            if(Stove)
            {
                StoveView.GetRightList();
                ComponentService.UpdateMenuSelect(Selected > 4 ? 3 : 5, Selected - (Selected > 4 ? 4 : 0), Selected > 4 ? 2 : 4, Selected > 4 ? 3 : 2, 21);
                ShowProgress();

                var CurrentSelected = Selected;
                Selected = LastSelected;
                LastSelected = CurrentSelected;
                StoveView.SetLeftList();
                ComponentService.UpdateMenuTextBox(2, LS.Action(ActionName, " [E]").ToUpper(), 3);
                ComponentService.UpdateMenuTextBox(3, LS.Navigation("manage dishes", " [Q]"), 3);
                ComponentService.UpdateMenuSelect(Selected, Selected, Products.Count, 2, 23);
            }
            else
            {
                var CurrentSelected = Selected;
                Selected = LastSelected;
                LastSelected = CurrentSelected;
                string WarningMessage = Selected > 4 ? "oven" : "stove";
                StoveView.GetLeftList();
                ComponentService.UpdateMenuTextBox(2, LS.Navigation("remove from the " + WarningMessage, " [E]"), 3);
                ComponentService.UpdateMenuTextBox(3, LS.Navigation("take items", " [Q]"), 3);
                ComponentService.UpdateMenuSelect(Products.Count + 1, LastSelected, 1, 2, 23);

                StoveView.SetRightList();
                ComponentService.UpdateMenuSelect(Selected - (Selected > 4 ? 4 : 0), Selected - (Selected > 4 ? 4 : 0), Selected > 4 ? 2 : 4, Selected > 4 ? 3 : 2, 27);
                StoveView.GetRightList();
                ShowProgress();
            }
            Stove = !Stove;
        }
        private static void Transfer()
        {
            if (Stove && StoveProducts[Selected - 1] != null) Drop();
            else if (!Stove && Products.Count >= Selected) Put();
        }
        private static void Put()
        {
            int PlaceIndex = ActionName == "bake" ? 4 : 0;
            int MaxCount = ActionName == "bake" ? 6 : 4;
            string WarningMessage = ActionName == "bake" ? "oven" : "stove";
            while (PlaceIndex < MaxCount && StoveProducts[PlaceIndex] != null) PlaceIndex++;
            if (PlaceIndex < MaxCount)
            {
                var PlacedProduct = Products[Selected - 1].ToProduct();
                string StateName = PlacedProduct.StateName.Split(' ')[0] + " " + ActionName;
                PlacedProduct.State = ObjectModel.GetObject(Field.ObjectName, _StateName: StateName).State;
                PlacedProduct.Amount = 1;
                StoveView.GetLeftList();
                Field.Pocket.SetSlot(PlaceIndex, PlacedProduct);
                StoveView.DisplayStove(StoveProducts, 0);
                LastSelected = 1;
                StoveView.GetRightList();

                IsInProgress = true;
                int PiecesAmount = Products[Selected - 1].Slots < 0 ? Products[Selected - 1].Slots : -1;
                int Index = GameInstance.Inventory.FindIndex(x => x.ObjectName == PlacedProduct.ObjectName && x.State == PlacedProduct.State);
                if (!GameInstance.Inventory[Index].AddAmount(PiecesAmount)) GameInstance.Inventory.RemoveAt(Index);
                if (!InventoryProducts[Selected - 1].AddAmount(PiecesAmount)) InventoryProducts.RemoveAt(Selected - 1);
                if (!Products[Selected - 1].AddAmount(-1)) 
                {
                    StoveView.SetLeftList();
                    StoveView.RepairDamageRemoval(Products.Count);
                    Products.RemoveAt(Selected - 1);
                    Selected = 1;
                    StoveView.DisplayCaptains(ActionName, Products, 1);
                }
                else
                {
                    StoveView.SetLeftList();
                    string Content = (Products[Selected - 1].StateName.Length > 0 && Products[Selected - 1].StateName[0] > '@' ?
                                     LS.Object(Products[Selected - 1].StateName) + " " : "") +  LS.Object(Products[Selected - 1].ObjectName) +
                                     " : " + Products[Selected - 1].Amount + LS.Navigation(Products[Selected - 1].Unit.Split('/')[0], " ", Before: " ");
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
            StoveView.DisplayCaptains(ActionName, Products, LastSelected, IsSelected: false);
            ComponentService.UpdateMenuTextBox(2, LS.Navigation("remove from the stove", " [E]"), 3);
            ComponentService.UpdateMenuTextBox(3, LS.Navigation("take items", " [Q]"), 3);
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
                        p.State++; // zmien stan na gotowe
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
            InventoryProducts = ProductModel.SelectProductsByAction(GameInstance.Inventory, ActionName, Category: 4, _SearchMapActs: true);
            Products = new List<ProductModel>();
            foreach (var ip in InventoryProducts)
            {
                var p = ip.ToProduct();
                p.Amount /= p.Slots < 0 ? (-1 * p.Slots) : 1;
                if(p.Amount > 0) Products.Add(p);
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
            StoveView.DisplayCaptains(ActionName, Products, Selected);
            
            while (OpenScreen.Contains("Food"))
            {
                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    switch (cki.Key)
                    {
                        case ConsoleKey.Tab:
                        case ConsoleKey.Escape: OpenScreen = EscapeScreen; SoundService.Play("K2"); break;
                        case ConsoleKey.W: UpdateSelecet(-1); break;
                        case ConsoleKey.S: UpdateSelecet(+1); break;
                        case ConsoleKey.Q: Switch(); break;
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
