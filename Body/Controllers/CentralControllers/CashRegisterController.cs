using FarmConsole.Body.Controlers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.CentralViews;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controllers.CentralControllers
{
    class CashRegisterController : MainController
    {
        private static int selected;
        private static decimal amount;
        private static bool paybycard;
        private static List<ProductModel> Cart { get; set; }


        private static void UpdateSelecet(int value)
        {
            if ( selected + value > 0 && selected + value <= Cart.Count)
            {
                MenuManager.UpdateMenuSelect(selected + value, selected, Cart.Count, 3, 19);
                selected += value;
            }
        }
        private static void TryToPay()
        {
            if (Cart.Count > 0)
            {
                if (paybycard && GameInstance.CardFunds >= amount || GameInstance.WalletFunds >= amount)
                {
                    GameInstance.CardFunds -= paybycard ? amount : 0;
                    GameInstance.WalletFunds -= paybycard ? 0 : amount;
                    foreach (var product in Cart)
                    {
                        var foundProduct = GameInstance.Inventory.Find(p => p.ObjectName == product.ObjectName);
                        if (foundProduct != null) foundProduct.Amount += product.Amount;
                        else GameInstance.Inventory.Add(product.ToProduct());
                    }
                    Cart.Clear();
                    OpenScreen = EscapeScreen;
                    S.Play("K3");
                }
                else
                {
                    if (paybycard) MenuManager.Warning(StringService.Get("no money on card"));
                    else MenuManager.Warning(StringService.Get("no money in wallet"));
                }
            }
            else MenuManager.Warning(StringService.Get("no items in cart"));
        }
        private static void ChangePaymentMethod()
        {
            paybycard = !paybycard;
            DisplayView();
        }
        private static void RejectProduct()
        {
            if (Cart.Count > 0)
            {
                MapEngine.Map.SortContainers(new List<ProductModel>() { Cart[selected - 1] });
                Cart.RemoveAt(selected - 1);
                selected = 1;
                SetAmount();
                DisplayView();
            }
        }
        private static void GiveUpShopping()
        {
            MapEngine.Map.SortContainers(Cart);
            OpenScreen = EscapeScreen;
            S.Play("K2");
        }
        private static void DisplayView()
        {
            CashRegisterView.Display(Cart, selected, amount, paybycard);
        }
        private static void SetAmount()
        {
            amount = 0;
            foreach (var p in Cart) amount += p.Amount * p.Price;
        }

        public static void Open()
        {
            paybycard = false;
            selected = 1;
            Cart = GameInstance.Cart;
            MenuManager.SetView = MapEngine.Map;
            SetAmount();
            DisplayView();

            while (OpenScreen == "CashRegister")
            {
                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    switch (cki.Key)
                    {
                        case ConsoleKey.Tab:
                        case ConsoleKey.Escape: OpenScreen = EscapeScreen; S.Play("K2"); break;
                        case ConsoleKey.W: UpdateSelecet(-1); break;
                        case ConsoleKey.S: UpdateSelecet(+1); break;
                        case ConsoleKey.E: TryToPay(); break;
                        case ConsoleKey.A: ChangePaymentMethod(); break;
                        case ConsoleKey.D: RejectProduct(); break;
                        case ConsoleKey.Q: GiveUpShopping(); break;
                    }
                }
            }
            MenuManager.Clean();
        }

    }
}
