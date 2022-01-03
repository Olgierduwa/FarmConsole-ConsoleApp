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
        private static int Select;
        private static int SliderValue = 2;
        private static int Multiplier = new int[] { 1, 5, 10, 25, 50 } [SliderValue];
        private static int Count;
        private static bool Side;
        private static bool ShiftPressed;
        private static ProductModel SelectedProduct { get; set; }
        private static ContainerModel Pocket { get; set; }
        private static List<ProductModel> Cart { get; set; }


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
                        ContainerView.DisplayLeftList(Pocket.GetSlots, Select, false);
                        SelectedProduct.Amount -= Multiplier;
                        if (SelectedProduct.Amount == 0)
                        {
                            Select = 1;
                            Cart.Remove(SelectedProduct);
                            Count = Cart.Count;
                            if (Cart.Count > 0) SelectedProduct = Cart[Select - 1]; else SelectedProduct = null;
                            ContainerView.DisplayRightList(Cart, Select, true, cleaning: true);
                        }
                        else
                        {
                            ContainerView.SetRightView();
                            ContainerView.UpdateMenuTextBox(Select, SelectedProduct.Amount + "x " + SelectedProduct.ObjectName);
                            ContainerView.GetRightView();
                        }
                        S.Play("K1");
                    }
                }
                else // from container to cart
                {
                    ProductModel NewProduct = SelectedProduct.ToProduct();
                    SelectedProduct.Amount -= Multiplier;
                    if (SelectedProduct.Amount == 0) Pocket.SetSlot(Select - 1, null);
                    NewProduct.Amount = Multiplier;
                    int index = 0;
                    while (index < Cart.Count) if (Cart[index].ObjectName == NewProduct.ObjectName) break; else index++;
                    if (index == Cart.Count) Cart.Add(NewProduct);
                    else Cart[index].Amount += Multiplier;
                    ContainerView.DisplayRightList(Cart, Select, false);
                    ContainerView.DisplayLeftList(Pocket.GetSlots, Select, true);
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
                Select = 1;
                Count = Cart.Count;
                ContainerView.DisplayRightList(Cart, Select, true);
                ContainerView.DisplayLeftList(Pocket.GetSlots, Select, false);
                GetProduct();
            }
            else
            {
                Select = 1;
                Count = Pocket.GetSlotsCount;
                ContainerView.DisplayLeftList(Pocket.GetSlots, Select, true);
                ContainerView.DisplayRightList(Cart, Select, false);
                GetProduct();
            }
        }
        private static void GetProduct()
        {
            if (Side) if (Cart.Count > 0) SelectedProduct = Cart[Select - 1]; else SelectedProduct = null;
            else SelectedProduct = Pocket.GetSlot(Select - 1);
            Captains();
        }
        private static void Captains()
        {
            string[] itemDetails = new string[] { "", "", "", "", "", "" };
            if (SelectedProduct != null)
            {
                itemDetails[2] = SelectedProduct.ObjectName;
                itemDetails[3] = StringService.Get("price") + ": " + SelectedProduct.Price.ToString() + StringService.Get("currency");
            }
            else itemDetails[itemDetails.Length / 2] = "...";
            ContainerView.DisplayCaptains(Action.SelectedProduct.ObjectName, itemDetails, Side, SliderValue);
            if (Side) ContainerView.SetRightView();
            else ContainerView.SetLeftView();
        }
        public static void Open()
        {
            Cart = GameInstance.Cart;
            Select = 1;
            Side = true;
            MenuManager.SetView = MapEngine.Map;
            Pocket = MapEngine.GetField().Pocket;
            SelectedProduct = Pocket.GetSlot(Select);
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

                        case ConsoleKey.E: Transfer(); break;
                        case ConsoleKey.Q: Switch(); break;

                        case ConsoleKey.W: if (Select > 1) { S.Play("K1"); Select--; ContainerView.UpdateMenuSelect(Select, Select + 1, Count); GetProduct(); } break;
                        case ConsoleKey.S: if (Select < Count) { S.Play("K1"); Select++; ContainerView.UpdateMenuSelect(Select, Select - 1, Count); GetProduct(); } break;

                        case ConsoleKey.A: if (SliderValue > 0) { S.Play("K1"); SliderValue--; Multiplier = new int[] { 1, 5, 10, 25, 50 }[SliderValue]; Captains(); } break;
                        case ConsoleKey.D: if (SliderValue < 4) { S.Play("K1"); SliderValue++; Multiplier = new int[] { 1, 5, 10, 25, 50 }[SliderValue]; Captains(); } break;
                    }
                }
            }
            MenuManager.Clean();
        }
    }
}
