using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.LocationViews;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;

namespace FarmConsole.Body.Controlers
{
    public class SideMenuController : MainControllerService
    {
        private static string Action;
        private static string[] FieldActions;
        private static string[] ProductActions;
        private static List<ProductModel> Inventory;
        private static List<ProductModel> FieldActionProducts;

        private static int MenuSize, CS, PS, QS, ES;            // CurrentSelect / PreviousSelect / Q-Select / E-Select //
        private static bool Q, C, E, COM, DRAGGED;              // CurrentOpenMenu / true - Q / false - E //

        private static void Hide(string menu)
        {
            switch (menu)
            {
                case "Q": MenuLeftView.Clean(); MapService.ShowMapFragment("left"); Q = false; QS = 1; break;
                case "E": MenuRightView.Clean(); MapService.ShowMapFragment("right"); E = false; ES = 1; break;
                case "C": MenuCenterView.Clean(); MapService.ShowMapFragment("center"); C = false; break;
                case "2": MenuLeftView.Clean(); MapService.ShowMapFragment("left"); Q = E = false;
                          MenuRightView.Clean(); MapService.ShowMapFragment("right"); QS = ES = PS = CS = 1; break;
                case "ALL": MenuLeftView.Clean(); MenuRightView.Clean(); Q = E = C = false; QS = ES = 1;
                            MenuCenterView.Clean(); MapService.ShowMapFragment("all"); break;
            }
        }
        private static void Display(string menu)
        {
            switch (menu)
            {
                case "QF":  ProductModel Field = ProductModel.GetProduct(MapView.GetField()); CS = PS = 1;
                            FieldActions = Field.MapActions; MenuSize = FieldActions.Length; COM = Q = true;
                            MenuLeftView.Show(FieldActions, Field.ProductName, CS, false); break;

                case "QP":  ProductActions = Inventory[CS - 1].MenuActions; ES = CS; MenuSize = ProductActions.Length; CS = PS = 1;
                            MenuLeftView.Show(ProductActions, Inventory[ES - 1].ProductName, CS, true); COM = Q = true; break;

                case "EF":  FieldActionProducts = ProductModel.GetProductsByAction(Inventory, Action); MenuSize = FieldActionProducts.Count;
                            COM = false; E = true; QS = CS; CS = PS = 1; MenuRightView.Show(FieldActionProducts, ES, true); break;

                case "EP":  COM = false; E = true; CS = PS = 1; MenuRightView.Show(Inventory, ES, false); MenuSize = Inventory.Count; break;
            }
        }
        private static void GoBack(string menu)
        {
            switch (menu)
            {
                case "Q": Hide("E"); COM = true; CS = PS = QS; MenuSize = FieldActions.Length;
                    MenuLeftView.Show(FieldActions, "", QS, false); break;
                case "E": Hide("Q"); COM = false; CS = PS = ES; MenuSize = Inventory.Count;
                    MenuRightView.Show(Inventory, ES, false); break;
            }
        }
        private static void Info(string content, int duration = 0, string opis = "")
        {
            Hide("2");
            if (content == "Zrobione")
            { 
                Inventory = SiteMenuService.Products;
                save.Wallet = SiteMenuService.Wallet;
            }
            else
            { 
                C = true;
                MenuCenterView.Show("warrning", content, duration, opis);
            }
        }
        private static void Update(string menu)
        {
            switch (menu)
            {
                case "Q": MenuLeftView.UpdateSelect(CS, PS, MenuSize); break;
                case "E": MenuRightView.UpdateSelect(CS, PS, MenuSize, 5); break;
            }
        }

        private static void ConfirmFieldAction()
        {
            SiteMenuService.Products = Inventory;
            Action = FieldActions[CS - 1];
            if (Action == "Przenieś") DRAGGED = true;
            switch (Action)
            {
                case "Postaw": case "Posiej": case "Nawieź": Display("EF"); break;
                default: Hide("2"); Info(SiteMenuService.DoOnMap(Action, new ProductModel(MapView.GetField()))); break;
            }
        }
        private static void ConfirmProduct()
        {
            if (FieldActionProducts.Count > 0)
            {
                Info(SiteMenuService.DoInInventory(Action, FieldActionProducts[CS - 1]));
            }
        }
        private static void DisplayProductActions()
        {
            // filtr controlled
            if (Inventory.Count > 0) Display("QP");
        }
        private static void ConfirmProductAction()
        {
            SiteMenuService.Products = Inventory;
            Info(SiteMenuService.DoInInventory(ProductActions[CS - 1], Inventory[ES - 1]));
        }

        public static void Initializate()
        {
            Inventory = save.GetInventory();
            CS = PS = QS = ES = 1;
            Q = E = COM = DRAGGED = false;
        }
        public static void Open(ConsoleKey cki)
        {
            switch (cki)
            {
                case ConsoleKey.Q:
                    if (DRAGGED) { MapView.Drop(false); DRAGGED = false; }
                    else Display("QF"); break;

                case ConsoleKey.E:
                    if (DRAGGED) { MapView.Drop(true); DRAGGED = false; }
                    else Display("EP"); break;
            }
            while (Q || E || C)
            {
                cki = Console.ReadKey(true).Key;
                //HelpService.INFO(4, "OS:"+OS.ToString(), "Q:"+Q.ToString(), "E:"+E.ToString(), "C:"+C.ToString());
                if (C) Hide("C"); 
                else switch (cki)
                {
                    case ConsoleKey.Q:
                        if (!E) Hide("Q");                              // [Q] !E  >> !Q  !E  // wylacz menu czynnosci dla pola
                        else if (!Q) DisplayProductActions();           // !Q  [E] >> [Q]  E  // wlacz menu czynnosci dla przedmiotu
                        else if (!COM) GoBack("Q");                     //  Q  [E] >> [Q] !E  // powrot do menu czynnosci dla pola
                        else if (COM) ConfirmProductAction(); break;    // [Q]  E  >> !Q  !E  // zatwierdz czynnosc dla przedmiotu

                    case ConsoleKey.E:
                        if (!Q) Hide("E");                              // !Q  [E] >> !Q  !E  // wylacz menu przedmiotow
                        else if (!E) ConfirmFieldAction();              // [Q] !E  >>  Q  [E] // wlacz menu czynnosci dla pola
                        else if (COM) GoBack("E");                      // [Q]  E  >> !Q  [E] // powrot do menu przedmiotow
                        else if (!COM) ConfirmProduct(); break;         //  Q  [E] >> !Q  !E  // zatwierdz czynnosc dla pola

                    case ConsoleKey.W:
                        if (CS > 1) { CS--; if(COM) Update("Q"); else Update("E"); PS = CS; } break;

                    case ConsoleKey.S:
                        if (CS < MenuSize) { CS++; if(COM) Update("Q"); else Update("E"); PS = CS; } break;

                    case ConsoleKey.Escape: Hide("ALL"); break;

                    {
                    //case ConsoleKey.A:
                    //    if (!COM && (Q && FACT_ID[catF][QS - 1] == 7 && catI > 1 || !Q && catI > 1)) {
                    //        catI--; CS = PS = 1; RSize = items[catI].Count;
                    //        MenuRightView.UpdateItemList(items, catI); Update("E"); } break;

                    //case ConsoleKey.D:
                    //    if (!COM && (Q && FACT_ID[catF][QS - 1] == 7 && catI < 3 || !Q && catI < items.Length - 1)) {
                    //        catI++; RSize = items[catI].Count; CS = PS = 1;
                    //        MenuRightView.UpdateItemList(items, catI); Update("E"); } break;
                    }
                }
            }
        }
    }
}
