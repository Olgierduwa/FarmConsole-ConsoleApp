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
    class ContainerController : HeadController
    {
        private static int Selected;
        private static int SliderValue = 2;
        private static readonly int[] Values = new int[] { 1, 5, 10, 25, 50 };
        private static int Multiplier = Values[SliderValue];
        private static int CartCount;
        private static bool Side;
        //private static bool ShiftPressed;
        private static string LeftTitle;
        private static string RightTitle;
        private static ProductModel SelectedProduct { get; set; }
        private static ContainerModel Pocket { get; set; }
        private static List<ProductModel> Cart { get; set; }


        private static void UpdateSelecet(int value)
        {
            if (Selected + value > 0 && (!Side && Selected + value <= Pocket.GetSlotsCount || Side && Selected + value <= CartCount))
            {
                SoundService.Play("K1");
                Selected += value;
                ComponentService.UpdateMenuSelect(Selected, Selected - value, CartCount, Prop: 15);
                GetProduct();
            }
        }
        private static void UpdateSlider(int value)
        {
            if (value < 0 && SliderValue > 0 || value > 0 && SliderValue < Values.Length - 1)
            {
                SoundService.Play("K1");
                SliderValue += value;
                Multiplier = Values[SliderValue];
                Captains();
            }
        }
        private static void Transfer()
        {
            if (SelectedProduct != null)
            {
                if (SelectedProduct.Amount >= Multiplier)
                {
                    if (Side) // from cart to container
                    {
                        int index = 0;
                        while (index < Pocket.GetSlotsCount) if (Pocket.GetSlot(index) != null &&
                                Pocket.GetSlot(index).ObjectName == SelectedProduct.ObjectName) break;
                            else index++;
                        if (index == Pocket.GetSlotsCount)
                        {
                            index = 0;
                            while (index < Pocket.GetSlotsCount) if (Pocket.GetSlot(index) == null) break; else index++;
                        }
                        if (index != Pocket.GetSlotsCount)
                        {
                            if (Pocket.GetSlot(index) != null) Pocket.GetSlot(index).AddAmount(Multiplier);
                            else
                            {
                                ProductModel NewProduct = SelectedProduct.ToProduct();
                                NewProduct.AddAmount(Multiplier);
                                Pocket.SetSlot(index, NewProduct);
                            }
                            ContainerView.DisplayLeftList(Pocket.GetSlots, LeftTitle, Selected, false);
                            SelectedProduct.AddAmount(-1 * Multiplier);
                            if (SelectedProduct.Amount == 0)
                            {
                                Selected = 1;
                                Cart.Remove(SelectedProduct);
                                CartCount = Cart.Count;
                                if (Cart.Count > 0) SelectedProduct = Cart[Selected - 1]; else SelectedProduct = null;
                                ContainerView.DisplayRightList(Cart, RightTitle, Selected, true, cleaning: true);
                            }
                            else
                            {
                                ContainerView.SetRightView();
                                string content = LS.Object(SelectedProduct.ObjectName) + " : " + SelectedProduct.Amount +
                                                 LS.Navigation(SelectedProduct.Unit.Split('/')[0], " ", Before: " ");
                                ComponentService.UpdateMenuTextBox(Selected, content, Margin: 0);
                                ContainerView.GetRightView();
                            }
                            SoundService.Play("K1");
                        }
                    }
                    else if (RightTitle == "cart" && (SelectedProduct.RequiredLevel == 0 ||
                        SelectedProduct.RequiredLevel <= GameInstance.LVL) || RightTitle != "cart")           // from container to cart
                    {
                        ProductModel NewProduct = SelectedProduct.ToProduct();
                        if(!SelectedProduct.AddAmount(-1 * Multiplier)) Pocket.SetSlot(Selected - 1, null);
                        NewProduct.Amount = Multiplier;
                        int index = 0;
                        while (index < Cart.Count) if (Cart[index].ObjectName == NewProduct.ObjectName) break; else index++;
                        if (index == Cart.Count) Cart.Add(NewProduct);
                        else Cart[index].AddAmount(Multiplier);
                        ContainerView.GetLeftView();
                        ContainerView.DisplayRightList(Cart, RightTitle, Selected, false);
                        ContainerView.SetLeftView();
                        string content = SelectedProduct.Amount == 0 ? "..." :
                                        LS.Object(SelectedProduct.ObjectName) + " : " + SelectedProduct.Amount +
                                        LS.Navigation(SelectedProduct.Unit.Split('/')[0], " ", Before: " ");
                        ComponentService.UpdateMenuTextBox(Selected, content, Margin: 0);
                        SoundService.Play("K1");
                    }
                }
                else
                {
                    SoundService.Play("K2");
                    //ComponentService.Warning(LS.Action("dont have enough product"));
                }
            }
            else SoundService.Play("K2");
        }
        private static void Switch()
        {
            Side = !Side;
            if (Side)
            {
                Selected = 1;
                CartCount = Cart.Count;
                ContainerView.DisplayRightList(Cart, RightTitle, Selected, true);
                ContainerView.DisplayLeftList(Pocket.GetSlots, LeftTitle, Selected, false);
                GetProduct();
            }
            else
            {
                Selected = 1;
                CartCount = Pocket.GetSlotsCount;
                ContainerView.DisplayLeftList(Pocket.GetSlots, LeftTitle, Selected, true);
                ContainerView.DisplayRightList(Cart, RightTitle, Selected, false);
                GetProduct();
            }
        }
        private static void GetProduct()
        {
            if (Side) if (Cart.Count > 0) SelectedProduct = Cart[Selected - 1]; else SelectedProduct = null;
            else SelectedProduct = Pocket.GetSlot(Selected - 1);
            Captains();
        }
        private static void Captains()
        {
            string[] itemDetails = new string[] { "", "", "", "", "", "", "" };
            if (SelectedProduct != null)
            {
                itemDetails[2] = LS.Object(SelectedProduct.ObjectName); 
                if (RightTitle == "cart" || RightTitle == "offer")
                {
                    if(SelectedProduct.RequiredLevel > 0 && GameInstance.LVL < SelectedProduct.RequiredLevel)
                        itemDetails[1] = LS.Navigation("required", " ") + LS.Navigation("lvl",": ") + SelectedProduct.RequiredLevel;

                    itemDetails[3] = LS.Navigation("price") + ": " + SelectedProduct.Price.ToString() + LS.Navigation("currency");

                    var units = SelectedProduct.Unit.Split('/');
                    if (units.Length > 1) itemDetails[4] = LS.Navigation(units[1] + "s", Before: "[") + ": " +
                                          (SelectedProduct.Slots < 0 ? (-1 * SelectedProduct.Slots) : 1) + "]";
                }
            }
            else itemDetails[itemDetails.Length / 2] = "...";
            ContainerView.DisplayCaptains(itemDetails, Side, SliderValue);
            if (Side) ContainerView.SetRightView();
            else ContainerView.SetLeftView();
        }

        public static void Open(ContainerModel Container = null)
        {
            Selected = 1;
            Side = true;
            Cart = GameInstance.Cart;
            ComponentService.SetView = MapEngine.Map;
            Pocket = Container ?? MapEngine.GetField().Pocket;
            SelectedProduct = Pocket.GetSlot(Selected);

            LeftTitle = Container == null ? Action.GetSelectedProduct.ObjectName : Container.SufixName;
            RightTitle = Container == null ? Cart == GameInstance.Inventory ? "inventory" : "cart" : "offer";

            Switch();
            while (OpenScreen == "Container")
            {
                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    //if (cki.Modifiers == ConsoleModifiers.Shift) ShiftPressed = true; else ShiftPressed = false;

                    switch (cki.Key)
                    {
                        case ConsoleKey.Tab:
                        case ConsoleKey.Escape: OpenScreen = EscapeScreen; SoundService.Play("K2"); break;
                        case ConsoleKey.W: UpdateSelecet(-1); break;
                        case ConsoleKey.S: UpdateSelecet(+1); break;
                        case ConsoleKey.A: UpdateSlider(-1); break;
                        case ConsoleKey.D: UpdateSlider(+1); break;
                        case ConsoleKey.Q: Switch(); break;
                        case ConsoleKey.E: Transfer(); break;
                    }
                }
            }
            ComponentService.Clean(WithCleaning: true);
        }
    }
}
