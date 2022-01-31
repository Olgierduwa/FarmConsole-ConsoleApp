using FarmConsole.Body.Controllers.CentralControllers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services.GameServices;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Views.GameViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FarmConsole.Body.Controllers.GameControllers
{
    class ProductOfferController : HeadController
    {
        private static int SliderValue = 4;
        private static int[] Values2 = new int[] { 1, 20, 40, 60, 80, 100, 120, 140, 160, 180, 200 };
        private static int[] Values = new int[] { 1, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
        private static int Multiplier = Values[SliderValue];
        private static int amount;
        private static bool transferpayment;
        private static List<ProductModel> OfferCart { get; set; }
        private static ContainerModel AvailableProductsContainer;


        private static void EditOffer()
        {
            string lasEscapeScreen = EscapeScreen;
            EscapeScreen = "ProductOffer";
            OpenScreen = "Container";
            ComponentService.Clean();
            ContainerController.Open(AvailableProductsContainer);
            EscapeScreen = lasEscapeScreen;
            SetAmount();
            DisplayCaptains(true);
        }
        private static void UpdateSlider(int value)
        {
            if (value < 0 && SliderValue > 0 || value > 0 && SliderValue < Values.Length - 1)
            {
                SoundService.Play("K1");
                SliderValue += value;
                Multiplier = Values[SliderValue];
                SetAmount();
                DisplayCaptains();
            }
        }
        private static void TryToSell()
        {
            if (OfferCart.Count > 0)
            {
                bool acceptOffer = false;

                if (Multiplier < 80) acceptOffer = true;

                if (acceptOffer)
                {
                    foreach (var offerProduct in OfferCart)
                    {
                        var inventoryProduct = GameInstance.Inventory.Find(p => p.ObjectName == offerProduct.ObjectName);
                        offerProduct.Amount *= offerProduct.Slots < 0 ? offerProduct.Slots * -1 : 1;
                        inventoryProduct.AddAmount(-1 * offerProduct.Amount);
                        GameService.IncreaseInExperience(offerProduct.Amount);
                        if (inventoryProduct.Amount == 0) GameInstance.Inventory.Remove(inventoryProduct);
                    }
                    MapEngine.Map.SortContainers(OfferCart);
                    GameInstance.CardFunds += transferpayment ? amount : 0;
                    GameInstance.WalletFunds += transferpayment ? 0 : amount;
                    OpenScreen = EscapeScreen;
                    SoundService.Play("K3");
                    string message = LS.Navigation("sale finalized");
                    ComponentService.GoodNews(message);
                }
                else
                {
                    string seller = LS.Object("seller");
                    string message = LS.Navigation("i have to reject offer, because") + LS.Navigation("price is to hight");
                    ComponentService.Warning(message, seller);
                }
            }
            else ComponentService.Warning(LS.Action("no items in cart"));
            DisplayCaptains();
        }
        private static void ChangePaymentMethod()
        {
            transferpayment = !transferpayment;
            DisplayCaptains();
        }
        private static void SetAmount()
        {
            amount = 0;
            foreach (var p in OfferCart) amount += p.Amount * p.Price * Multiplier / 100;
        }
        private static void DisplayCaptains(bool init = false) => ProductOfferView.DisplayCaptains(amount, OfferCart, transferpayment, SliderValue, init);

        public static void Open()
        {
            List<ProductModel> BufferCart = GameInstance.Cart.ToList();
            GameInstance.Cart.Clear();
            List<ProductModel> SupplyProducts = MapEngine.Map.GetSupplyProducts();
            List<ProductModel> MatchProducts = new List<ProductModel>();
            foreach (var inventoryProduct in GameInstance.Inventory)
            {
                var founded = SupplyProducts.Find(x => x.ObjectName == inventoryProduct.ObjectName);
                if (founded != null)
                {
                    var splitProduct = founded.ToProduct();
                    splitProduct.Amount /= splitProduct.Slots < 0 ? (splitProduct.Slots * -1) : 1;
                    MatchProducts.Add(splitProduct);
                }
            }
                AvailableProductsContainer = new ContainerModel(MatchProducts, (short)MatchProducts.Count, "inventory");
            OfferCart = GameInstance.Cart;
            transferpayment = false;
            SetAmount();
            ComponentService.SetView = MapEngine.Map;
            DisplayCaptains(true);

            while (OpenScreen == "ProductOffer")
            {
                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    switch (cki.Key)
                    {
                        case ConsoleKey.Tab:
                        case ConsoleKey.Escape: OpenScreen = EscapeScreen; SoundService.Play("K2"); break;
                        case ConsoleKey.S: EditOffer(); break;
                        case ConsoleKey.A: UpdateSlider(-1); break;
                        case ConsoleKey.D: UpdateSlider(+1); break;
                        case ConsoleKey.Q: ChangePaymentMethod(); break;
                        case ConsoleKey.E: TryToSell(); break;
                    }
                }
            }

            GameInstance.Cart = BufferCart;
            ComponentService.Clean();
        }

    }
}
