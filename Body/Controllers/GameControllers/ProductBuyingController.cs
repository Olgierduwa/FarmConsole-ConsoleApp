using FarmConsole.Body.Controllers.CentralControllers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services.GameServices;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Views.GameViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controllers.GameControllers
{
    class ProductBuyingController : HeadController
    {
        private static int Selected;
        private static int amount;
        private static bool paybycard;
        private static List<ProductModel> Cart { get; set; }


        private static void UpdateSelecet(int value)
        {
            if (Selected + value > 0 && Selected + value <= Cart.Count)
            {
                ComponentService.UpdateMenuSelect(Selected + value, Selected, Cart.Count, 3, 19);
                Selected += value;
            }
        }
        private static void TryToPay()
        {
            if (Cart.Count > 0)
            {
                if (GameInstance.Inventory.Find(x => x.ObjectName == "wallet") != null)
                {
                    if (paybycard && GameInstance.CardFunds >= amount || GameInstance.WalletFunds >= amount)
                    {
                        GameInstance.CardFunds -= paybycard ? amount : 0;
                        GameInstance.WalletFunds -= paybycard ? 0 : amount;
                        foreach (var cartProduct in Cart)
                        {
                            var foundProduct = GameInstance.Inventory.Find(p => p.ObjectName == cartProduct.ObjectName && p.State == cartProduct.State);
                            cartProduct.Amount *= cartProduct.Slots < 0 ? cartProduct.Slots * -1 : 1;
                            if (foundProduct != null) foundProduct.AddAmount(cartProduct.Amount);
                            else GameInstance.Inventory.Add(cartProduct.ToProduct());
                            GameService.IncreaseInExperience(cartProduct.Amount);
                        }
                        Cart.Clear();
                        OpenScreen = EscapeScreen;
                        SoundService.Play("K3");
                        string message = LS.Navigation("purchases finalized");
                        ComponentService.GoodNews(message);
                    }
                    else
                    {
                        if (paybycard) ComponentService.Warning(LS.Action("no money on card"));
                        else ComponentService.Warning(LS.Action("no money in wallet"));
                    }
                }
                else ComponentService.Warning(LS.Action("no wallet"));
            }
            else ComponentService.Warning(LS.Action("no items in cart"));
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
                MapEngine.Map.SortContainers(new List<ProductModel>() { Cart[Selected - 1] });
                Cart.RemoveAt(Selected - 1);
                Selected = 1;
                SetAmount();
                DisplayView();
            }
        }
        private static void GiveUpShopping()
        {
            MapEngine.Map.SortContainers(Cart);
            OpenScreen = EscapeScreen;
            SoundService.Play("K2");
        }
        private static void DisplayView()
        {
            ProductBuyingView.Display(Cart, Selected, amount, paybycard);
        }
        private static void SetAmount()
        {
            amount = 0;
            foreach (var p in Cart) amount += p.Amount * p.Price;
        }

        public static void Open()
        {
            paybycard = false;
            Selected = 1;
            Cart = GameInstance.Cart;
            ComponentService.SetView = MapEngine.Map;
            SetAmount();
            DisplayView();

            while (OpenScreen == "ProductBuying")
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
                        case ConsoleKey.E: TryToPay(); break;
                        case ConsoleKey.A: ChangePaymentMethod(); break;
                        case ConsoleKey.D: RejectProduct(); break;
                        case ConsoleKey.Q: GiveUpShopping(); break;
                    }
                }
            }
            ComponentService.Clean();
        }

    }
}
