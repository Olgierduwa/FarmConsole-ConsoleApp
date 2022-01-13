using FarmConsole.Body.Controlers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.CentralViews;
using FarmConsole.Body.Views.LocationViews;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Controllers.CentralControllers
{
    class ContainerController : MainController
    {
        private static int Selected;
        private static int SliderValue = 2;
        private static int[] Values = new int[] { 1, 5, 10, 25, 50 };
        private static int Multiplier = Values[SliderValue];
        private static int CartCount;
        private static bool Side;
        private static bool ShiftPressed;
        private static string LeftTitle;
        private static string RightTitle;
        private static ProductModel SelectedProduct { get; set; }
        private static ContainerModel Pocket { get; set; }
        private static List<ProductModel> Cart { get; set; }


        private static void UpdateSelecet(int value)
        {
            if (Selected + value > 0 && ((!Side && Selected + value <= Pocket.GetSlotsCount) || (Side && Selected + value <= CartCount)))
            {
                S.Play("K1");
                Selected += value;
                ContainerView.UpdateMenuSelect(Selected, Selected - value, CartCount, Prop:15);
                GetProduct();
            }
        }
        private static void UpdateSlider(int value)
        {
            if ((value < 0 && SliderValue > 0) || (value > 0 && SliderValue < Values.Length - 1))
            {
                S.Play("K1");
                SliderValue += value;
                Multiplier = Values[SliderValue];
                Captains();
            }
        }
        private static void Transfer()
        {
            if (SelectedProduct != null && SelectedProduct.Amount >= Multiplier)
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
                        if (Pocket.GetSlot(index) != null) Pocket.GetSlot(index).Amount += Multiplier;
                        else
                        {
                            ProductModel NewProduct = SelectedProduct.ToProduct();
                            NewProduct.Amount = Multiplier;
                            Pocket.SetSlot(index, NewProduct);
                        }
                        ContainerView.DisplayLeftList(Pocket.GetSlots, LeftTitle, Selected, false);
                        SelectedProduct.Amount -= Multiplier;
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
                            ContainerView.UpdateMenuTextBox(Selected, SelectedProduct.Amount + "x " + LS.Object(SelectedProduct.ObjectName), margin:0);
                            ContainerView.GetRightView();
                        }
                        S.Play("K1");
                    }
                }
                else // from container to cart
                {
                    ProductModel NewProduct = SelectedProduct.ToProduct();
                    SelectedProduct.Amount -= Multiplier;
                    if (SelectedProduct.Amount == 0) Pocket.SetSlot(Selected - 1, null);
                    NewProduct.Amount = Multiplier;
                    int index = 0;
                    while (index < Cart.Count) if (Cart[index].ObjectName == NewProduct.ObjectName) break; else index++;
                    if (index == Cart.Count) Cart.Add(NewProduct);
                    else Cart[index].Amount += Multiplier;
                    ContainerView.GetLeftView();
                    ContainerView.DisplayRightList(Cart, RightTitle, Selected, false);
                    ContainerView.SetLeftView();
                    string content = SelectedProduct.Amount == 0 ? "..." : SelectedProduct.Amount + "x " + LS.Object(SelectedProduct.ObjectName);
                    ContainerView.UpdateMenuTextBox(Selected, content, margin: 0);
                    S.Play("K1");
                }
            }
            else S.Play("K2");
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
            string[] itemDetails = new string[] { "", "", "", "", "", "" };
            if (SelectedProduct != null)
            {
                itemDetails[2] = LS.Object(SelectedProduct.ObjectName);
                itemDetails[3] = LS.Navigation("price") + ": " + SelectedProduct.Price.ToString() + LS.Navigation("currency");
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
            MenuManager.SetView = MapEngine.Map;
            Pocket = Container ?? MapEngine.GetField().Pocket;
            SelectedProduct = Pocket.GetSlot(Selected);

            LeftTitle = Container == null ? Action.SelectedProduct.ObjectName : "inventory";
            RightTitle = Container == null ? Cart == GameInstance.Inventory ? "inventory" : "cart" : "offer";

            Switch();
            while (OpenScreen == "Container")
            {
                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    if (cki.Modifiers == ConsoleModifiers.Shift) ShiftPressed = true; else ShiftPressed = false;

                    switch (cki.Key)
                    {
                        case ConsoleKey.Tab:
                        case ConsoleKey.Escape: OpenScreen = EscapeScreen; S.Play("K2"); break;
                        case ConsoleKey.W: UpdateSelecet(-1); break;
                        case ConsoleKey.S: UpdateSelecet(+1); break;
                        case ConsoleKey.A: UpdateSlider(-1); break;
                        case ConsoleKey.D: UpdateSlider(+1); break;
                        case ConsoleKey.Q: Switch(); break;
                        case ConsoleKey.E: Transfer(); break;
                    }
                }
            }
            MenuManager.Clean(WithCleaning: true);
        }
    }
}
