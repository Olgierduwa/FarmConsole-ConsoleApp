using FarmConsole.Body.Models;
using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.GameViews
{
    class CashMachineView : ComponentService
    {
        public static void DisplayOperationList(int Selected, bool Enable = true)
        {
            ClearList(false);

            Endl(3);
            H2(LS.Navigation("cash machine label"));
            GroupStart(0);
            
            GroupStart(2);
            Endl(6);
            TextBox(LS.Navigation("account information"));
            TextBox(LS.Navigation("withdraw"));
            TextBox(LS.Navigation("deposit"));
            TextBox(LS.Navigation("transfer"));
            Endl(1);
            TextBox(LS.Navigation("remove card", " [Q]"), foreground: ColorService.GetColorByName("redl"));
            GroupEnd();
            
            GroupStart(2);
            Endl(2);
            TextBox(LS.Object("cash machine"));
            GroupEnd();

            GroupEnd();
            if (Enable)
            {
                PrintList();
                UpdateMenuSelect(Selected, Selected, 4);
            }
            else DisableView();
        }
        public static void DisplayDetails(int Amount,string Nick, string Date)
        {
            Clean();

            Endl(5);
            GroupStart(0);

            GroupStart(4);
            Endl(2);
            TextBox(LS.Navigation("account"));
            Endl(1);
            TextBox(LS.Navigation("opening date") + ": " + Date);
            TextBox(LS.Navigation("owner") + ": " + Nick);
            TextBox(LS.Navigation("account balance") + ": " + Amount + LS.Navigation("currency"));
            GroupEnd();

            GroupEnd();
            PrintList();
        }
        public static void DisplayWithdraw(int Card, int Wallet, int Amount, int SliderValue)
        {
            ClearList(false);

            Endl(5);
            GroupStart(0);

            GroupStart(4);
            Endl(2);
            TextBox(LS.Navigation("operation"));
            Endl(1);
            TextBox(LS.Navigation("account balance") + ": " + Card + LS.Navigation("currency"));
            TextBox(LS.Navigation("withdraw amount") + ": " + Amount + LS.Navigation("currency"));
            TextBox(LS.Navigation("wallet balance") + ": " + Wallet + LS.Navigation("currency"));
            Slider(36, SliderValue);
            Endl(1);
            TextBox(LS.Navigation("do operation", " [E]"), foreground: ColorService.GetColorByName("limed"));
            GroupEnd();

            GroupEnd();
            PrintList();
            ComponentsDisplayed.Clear();
            ComponentsDisplayed.Add(GetComponentByName("SL", 2));
            ComponentsDisplayed.Add(GetComponentByName("TB", 2, 8));
        }
        public static void DisplayDeposit(int Wallet, int Card, int Amount, int SliderValue)
        {
            ClearList(false);

            Endl(5);
            GroupStart(0);

            GroupStart(4);
            Endl(2);
            TextBox(LS.Navigation("operation"));
            Endl(1);
            TextBox(LS.Navigation("wallet balance") + ": " + Wallet + LS.Navigation("currency"));
            TextBox(LS.Navigation("deposit amount") + ": " + Amount + LS.Navigation("currency"));
            TextBox(LS.Navigation("account balance") + ": " + Card + LS.Navigation("currency"));
            Slider(36, SliderValue);
            Endl(1);
            TextBox(LS.Navigation("do operation", " [E]"), foreground: ColorService.GetColorByName("limed"));
            GroupEnd();

            GroupEnd();
            PrintList();
            ComponentsDisplayed.Clear();
            ComponentsDisplayed.Add(GetComponentByName("TB", 2, 4));
            ComponentsDisplayed.Add(GetComponentByName("TB", 2, 5));
            ComponentsDisplayed.Add(GetComponentByName("SL", 2));
        }
        public static void DisplayTransfer()
        {
            Clean();

            Endl(5);
            GroupStart(0);

            GroupStart(4);
            Endl(2);
            TextBox(LS.Navigation("operation"), foreground: ColorService.GetColorByName("gray3"));
            Endl(1);
            TextBox(LS.Navigation("operation temporarily unavailable"), foreground: ColorService.GetColorByName("gray3"));
            Endl(10);
            TextBox(LS.Navigation("do operation", " [E]"), foreground: ColorService.GetColorByName("gray3"));
            GroupEnd();

            GroupEnd();
            PrintList();
        }
    }
}
