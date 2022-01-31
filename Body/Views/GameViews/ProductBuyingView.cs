using FarmConsole.Body.Models;
using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.GameViews
{
    class ProductBuyingView : ComponentService
    {
        public static void Display(List<ProductModel> products, int Selected, decimal amount, bool paybycard)
        {
            ClearList(false);

            string c = LS.Navigation("currency");

            Endl(3);
            H2(LS.Navigation("product buying label"));
            GroupStart(0);

            GroupStart(2);
            Endl(2);
            TextBox(LS.Object("cart"));
            GroupEnd();

            GroupStart(2);
            Endl(6);

            int Height = (Console.WindowHeight - 19) / 3;
            int Count = Height > products.Count ? Height : products.Count;
            for (int i = 0; i < Count; i++)
            {
                if (i < products.Count)
                {
                    if (i < Height) TextBox(products[i].Amount * products[i].Price + c + " - " + LS.Object(products[i].ObjectName));
                    else TextBox(products[i].Amount * products[i].Price + c + " - " + LS.Object(products[i].ObjectName), Show: false);
                }
                else TextBox("...");
            }
            Endl(1);
            if (products.Count > 0)
                TextBox(LS.Navigation("reject product", " [D]"), Foreground: ColorService.GetColorByName("redL"));
            else TextBox(LS.Navigation("reject product", " [D]"), Foreground: ColorService.GetColorByName("gray3"));

            GroupEnd();
            GroupStart(4);
            Endl(2);
            TextBox(LS.Object("cash register"));
            Endl(1);
            TextBox(LS.Navigation("amount to pay") + ": " + amount.ToString() + LS.Navigation("currency"));
            Endl(Height * 3 - 10);
            Endl(1);
            if (paybycard) TextBox(LS.Navigation("pay by card", " [E]"), Foreground: ColorService.GetColorByName("limeD"));
            else TextBox(LS.Navigation("pay with cash", " [E]"), Foreground: ColorService.GetColorByName("limeD"));
            TextBox(LS.Navigation("change payment method", " [A]"), Foreground: ColorService.GetColorByName("orangeL"));
            Endl(1);
            TextBox(LS.Navigation("give up shopping", " [Q]"), Foreground: ColorService.GetColorByName("redL"));

            GroupEnd();
            GroupEnd();

            PrintList();
            UpdateSelect(Selected, Selected, products.Count, 3, rangeProp: 19);

            ComponentsDisplayed = new List<CM>();
            ComponentsDisplayed.Add(GetComponentByName("GS", 2));
            ComponentsDisplayed.Add(GetComponentByName("GS", 3));
        }

    }
}
