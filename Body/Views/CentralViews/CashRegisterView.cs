using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.CentralViews
{
    class CashRegisterView : MenuManager
    {
        public static void Display(List<ProductModel> products, int selected, decimal amount, bool paybycard)
        {
            ClearList(false);

            string c = StringService.Get("currency");

            Endl(3);
            H2(StringService.Get("cashregister label"));
            GroupStart(0);
            
            GroupStart(2);
            Endl(2);
            TextBox(StringService.Get("cart"));
            GroupEnd();

            GroupStart(2);
            Endl(6);

            int Height = (Console.WindowHeight - 19) / 3;
            int Count = Height > products.Count ? Height : products.Count;
            for (int i = 0; i < Count; i++)
            {
                if (i < products.Count)
                {
                    if (i < Height) TextBox(products[i].Amount * products[i].Price + c + " - " + products[i].ObjectName);
                    else TextBox(products[i].Amount * products[i].Price + c + " - " + products[i].ObjectName, show: false);
                }
                else TextBox("...");
            }
            Endl(1);
            if (products.Count > 0)
                 TextBox(StringService.Get("reject product", " [D]"), foreground: ColorService.GetColorByName("redL"));
            else TextBox(StringService.Get("reject product", " [D]"), foreground: ColorService.GetColorByName("gray3"));

            GroupEnd();
            GroupStart(4);
            Endl(2);
            TextBox(StringService.Get("cashregister"));
            Endl(1);
            TextBox(StringService.Get("amount to pay") + ": " + amount.ToString() + StringService.Get("currency"));
            Endl(Height * 3 - 10);
            Endl(1);
            if(paybycard) TextBox(StringService.Get("pay by card", " [E]"), foreground: ColorService.GetColorByName("limeD"));
            else TextBox(StringService.Get("pay with cash", " [E]"), foreground: ColorService.GetColorByName("limeD"));
            TextBox(StringService.Get("change payment method", " [A]"), foreground: ColorService.GetColorByName("orangeL"));
            Endl(1);
            TextBox(StringService.Get("give up shopping", " [Q]"), foreground: ColorService.GetColorByName("redL"));

            GroupEnd();
            GroupEnd();

            PrintList();
            UpdateSelect(selected, selected, products.Count, 3, rangeProp:19);

            ComponentsDisplayed = new List<CM>();
            ComponentsDisplayed.Add(GetComponentByName("GS",2));
            ComponentsDisplayed.Add(GetComponentByName("GS",3));
        }
    }
}
