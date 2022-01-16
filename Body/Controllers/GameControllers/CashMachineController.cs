using FarmConsole.Body.Controllers.CentralControllers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Views.GameViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controllers.GameControllers
{
    class CashMachineController : HeadController
    {
        private static int Selected;
        private static int CardFunds;
        private static int WalletFunds;
        private static int DepositFunds;
        private static int DepositSliderValue;
        private static int WithdrawFunds;
        private static int WithdrawSliderValue;
        private static string Nick;
        private static string Date;

        public static void Open()
        {
            Selected = 1;
            Nick = GameInstance.UserName;
            Date = "10.10.2020";
            CardFunds = GameInstance.CardFunds;
            WalletFunds = GameInstance.WalletFunds;
            DepositFunds = 0;
            WithdrawFunds = 0;
            DepositSliderValue = 0;
            WithdrawSliderValue = 0;
            ComponentService.SetView = MapEngine.Map;
            CashMachineView.DisplayDetails(CardFunds, Nick, Date);
            CashMachineView.DisplayOperationList(Selected);
            while (OpenScreen == "CashMachine")
            {
                if (Console.KeyAvailable)
                {
                    var cki = Console.ReadKey(true);
                    switch (cki.Key)
                    {
                        case ConsoleKey.Q:
                        case ConsoleKey.Tab:
                        case ConsoleKey.Escape: OpenScreen = EscapeScreen; SoundService.Play("K2"); break;

                        case ConsoleKey.W: UpdateSelecet(-1); break;
                        case ConsoleKey.S: UpdateSelecet(+1); break;

                        case ConsoleKey.D: UpdateSlider(+1); break;
                        case ConsoleKey.A: UpdateSlider(-1); break;

                        case ConsoleKey.E: DoOperation(); break;
                    }
                }
            }
            ComponentService.Clean();
        }

        private static void Withdraw()
        {
            if (WithdrawFunds > 0)
            {
                GameInstance.WalletFunds += WithdrawFunds;
                WalletFunds += WithdrawFunds;
                GameInstance.CardFunds -= WithdrawFunds;
                CardFunds -= WithdrawFunds;
                WithdrawSliderValue = 0;
                CashMachineView.DisplayOperationList(Selected, false);
                UpdateSlider(0);
                ComponentService.GoodNews(LS.Navigation("amount paid out"));
                CashMachineView.DisplayOperationList(Selected);
            }
        }

        private static void Deposit()
        {
            if (DepositFunds > 0)
            {
                GameInstance.WalletFunds -= DepositFunds;
                WalletFunds -= DepositFunds;
                GameInstance.CardFunds += DepositFunds;
                CardFunds += DepositFunds;
                WithdrawSliderValue = 0;
                CashMachineView.DisplayOperationList(Selected, false);
                UpdateSlider(0);
                ComponentService.GoodNews(LS.Navigation("amount paid"));
                CashMachineView.DisplayOperationList(Selected);
            }
        }

        private static void DoOperation()
        {
            switch(Selected)
            {
                case 2: Withdraw(); break;
                case 3: Deposit(); break;
            }
        }

        private static void UpdateSlider(int value)
        {
            switch (Selected)
            {
                case 2:
                    if (WithdrawSliderValue + value >= 0 && WithdrawSliderValue + value <= 36)
                    {
                        WithdrawSliderValue += value;
                        WithdrawFunds = CardFunds * WithdrawSliderValue / 36;
                        CashMachineView.DisplayWithdraw(CardFunds, WalletFunds, WithdrawFunds, WithdrawSliderValue);
                    }
                    break;
                case 3:
                    if (DepositSliderValue + value >= 0 && DepositSliderValue + value <= 36)
                    {
                        DepositSliderValue += value;
                        DepositFunds = WalletFunds * DepositSliderValue / 36;
                        CashMachineView.DisplayDeposit(WalletFunds, CardFunds, DepositFunds, DepositSliderValue);
                    }
                    break;
            }
        }

        private static void UpdateSelecet(int value)
        {
            if (Selected + value > 0 && Selected + value < 5)
            {
                Selected += value;
                CashMachineView.DisplayOperationList(Selected);
                switch(Selected)
                {
                    case 1: CashMachineView.DisplayDetails(CardFunds, Nick, Date); break;
                    case 2: CashMachineView.DisplayWithdraw(CardFunds, WalletFunds, WithdrawFunds, WithdrawSliderValue); break;
                    case 3: CashMachineView.DisplayDeposit(WalletFunds, CardFunds, DepositFunds, DepositSliderValue); break;
                    case 4: CashMachineView.DisplayTransfer(); break;
                }
            }
        }
    }
}
