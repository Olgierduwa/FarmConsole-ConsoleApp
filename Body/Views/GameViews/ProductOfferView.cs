using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FarmConsole.Body.Views.CentralViews
{
    class ProductOfferView : MenuManager
    {
        public static void DisplayCaptains(int amount, List<ProductModel> products, bool transferpayment, int sliderValue, bool init = false)
        {
            ClearList(false);

            Endl(3);
            H2(LS.Navigation("product selling label"));
            GroupStart(0);

            GroupStart(4);
            Endl(2);
            TextBox(LS.Object("offer"));
            Endl(1);
            int Height = (Console.WindowHeight - 19) / 3;
            int Count = Height > products.Count ? Height : products.Count;
            for (int i = 0; i < Count; i++)
            {
                if (i < products.Count)
                {
                    if (i < Height) TextBox(products[i].Amount + "x " + LS.Object(products[i].ObjectName));
                    else TextBox(products[i].Amount + "x " + LS.Object(products[i].ObjectName), show: false);
                }
                else TextBox("...");
            }
            Endl(1);
            TextBox(LS.Navigation("manage offer", " [S]"), foreground: ColorService.GetColorByName("cyan"));
            GroupEnd();

            GroupStart(2);
            Endl(2);
            TextBox(LS.Object("cash register"));
            Endl(1);
            TextBox(LS.Navigation("amount for the offer") + ": " + amount.ToString() + LS.Navigation("currency"));

            Endl(Height * 3 - 12);
            int[] Values2 = new int[] { 1, 20, 40, 60, 80, 100, 120, 140, 160, 180, 200 };
            int[] Values = new int[] { 1, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
            TextBox(Values[sliderValue] + LS.Navigation("percent default price"));
            Slider(10, sliderValue);

            Endl(1);
            if(transferpayment) TextBox(LS.Navigation("accept payment by transfer", " [E]"), foreground: ColorService.GetColorByName("limeD"));
            else TextBox(LS.Navigation("accept cash payment", " [E]"), foreground: ColorService.GetColorByName("limeD"));
            TextBox(LS.Navigation("change payment method", " [Q]"), foreground: ColorService.GetColorByName("orangeL"));
            GroupEnd();

            GroupEnd();

            PrintList();

            ComponentsDisplayed = new List<CM>();
            ComponentsDisplayed.Add(GetComponentByName("GS", 2));
            ComponentsDisplayed.Add(GetComponentByName("GS", 3));
        }
    }
}
