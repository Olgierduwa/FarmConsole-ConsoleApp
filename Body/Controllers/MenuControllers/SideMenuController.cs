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
        private static List<ProductModel> SelectedProducts;

        private static int Scale, MenuSize, CS, PS, QS, ES;     // CurrentSelect / PreviousSelect / Q-Select / E-Select //
        private static bool Q, C, E, COM, DRAGGED;              // CurrentOpenMenu / true - Q / false - E //

        private static void Display(string menu)
        {
            Inventory = GameInstance.GetProductsByScale(Scale);
            switch (menu)
            {
                case "QF":  ProductModel Field = ProductModel.GetProduct(MapView.GetField()); CS = PS = 1;
                            FieldActions = Field.MapActions; MenuSize = FieldActions.Length; COM = Q = true;
                            MenuLeftView.Display(FieldActions, Field.ProductName, CS); break;

                case "QP":  ES = CS; ProductActions = SelectedProducts[ES - 2].MenuActions; MenuSize = ProductActions.Length; CS = PS = 1;
                            MenuLeftView.Display(ProductActions, SelectedProducts[ES - 2].ProductName, CS, true); COM = Q = true; break;

                case "EF":  QS = CS; SelectedProducts = ProductModel.GetProductsByAction(Inventory, Action); CS = PS = 1; COM = false; E = true;
                            MenuSize = SelectedProducts.Count; MenuRightView.Display(SelectedProducts, ES, extended: true); break;

                case "EP":  COM = false; E = true; CS = PS = 1; SelectedProducts = Inventory; MenuSize = Inventory.Count + 1;
                            MenuRightView.Display(SelectedProducts, CS, search: true); break;
            }
        }
        private static void Hide(string menu)
        {
            switch (menu)
            {
                case "Q":   MenuLeftView.Clean(); MapService.ShowMapFragment("left"); Q = false; QS = 1; break;

                case "E":   MenuRightView.Clean(); MapService.ShowMapFragment("right"); E = false; ES = 1; break;

                case "C":   MenuCenterView.Clean(); MapService.ShowMapFragment("center"); C = false; break;

                case "2":   MenuLeftView.Clean(); MapService.ShowMapFragment("left"); Q = E = false;
                            MenuRightView.Clean(); MapService.ShowMapFragment("right"); QS = ES = PS = CS = 1; break;

                case "ALL": MenuLeftView.Clean(); MenuRightView.Clean(); Q = E = C = false; QS = ES = 1;
                            MenuCenterView.Clean(); MapService.ShowMapFragment("all"); break;
            }
        }
        private static void GoBack(string menu)
        {
            switch (menu)
            {
                case "Q":   Hide("E"); COM = true; CS = PS = QS; MenuSize = FieldActions.Length;
                            ProductModel Product = ProductModel.GetProduct(MapView.GetField());
                            MenuLeftView.Display(FieldActions, Product.StateName + " " + Product.ProductName, QS, print: false); break;

                case "E":   Hide("Q"); COM = false; CS = PS = ES;  MenuSize = SelectedProducts.Count + 1;
                            MenuRightView.Display(SelectedProducts, ES, print: false, search: true); break;
            }
        }
        private static void Info(string content)
        {
            Hide("2");
            if (content != XF.GetString("done"))
            {
                C = true;
                MenuCenterView.Show("warrning", content);
            }
        }

        private static void SelectFieldAction()
        {
            if (FieldActions.Length > 0)
            {
                Action = FieldActions[CS - 1];
                if (Action == "Przenieś") DRAGGED = true;
                switch (Action)
                {
                    case "Postaw": case "Posiej": case "Nawieź": Display("EF"); break;
                    default: Info(SideMenuService.DoOnMap(Action, new ProductModel(MapView.GetField()))); break;
                }
            }
        }
        private static void SelectProduct()
        {
            if (SelectedProducts.Count > 0)
            {
                Info(SideMenuService.DoInInventory(Action, SelectedProducts[CS - 1]));
            }
        }
        private static void DisplayProductActions()
        {
            if (SelectedProducts.Count > 0)
            {
                if (CS == 1) FilterProducts();
                else Display("QP");
            }
        }
        private static void ConfirmProductAction()
        {
            Info(SideMenuService.DoInInventory(ProductActions[CS - 1], SelectedProducts[ES - 2]));
        }
        private static void FilterProducts()
        {
            ConsoleKey cki;
            string SearchedPhrase = "";
            string DefaultString = ":";
            SelectedProducts = new List<ProductModel>();
            MenuRightView.Search(Inventory, DefaultString);
            while (true)
            {
                cki = Console.ReadKey(true).Key;
                if (cki == ConsoleKey.Q)
                {
                    if (SelectedProducts.Count == 0) Display("EP");
                    else { MenuSize = SelectedProducts.Count + 1; CS = PS = 2;
                        MenuRightView.Display(SelectedProducts, CS, search: true); }
                    return;
                }
                if(cki == ConsoleKey.Backspace && SearchedPhrase.Length > 0)
                {
                    SearchedPhrase = SearchedPhrase.Substring(0, SearchedPhrase.Length - 1);
                    SelectedProducts = new List<ProductModel>();
                    foreach (var P in Inventory) if (P.ProductName.ToLower().Contains(SearchedPhrase.ToLower())) SelectedProducts.Add(P);
                    if(SearchedPhrase.Length > 0) MenuRightView.Search(SelectedProducts, SearchedPhrase);
                    else MenuRightView.Search(SelectedProducts, DefaultString);
                }
                if ((Convert.ToInt16(cki) > 64 && Convert.ToInt16(cki) < 91 || cki == ConsoleKey.Spacebar) && SearchedPhrase.Length < 15)
                {
                    SearchedPhrase += ((char)Convert.ToInt16(cki)).ToString();
                    SelectedProducts = new List<ProductModel>();
                    foreach(var P in Inventory) if (P.ProductName.ToLower().Contains(SearchedPhrase.ToLower())) SelectedProducts.Add(P);
                    MenuRightView.Search(SelectedProducts, SearchedPhrase);
                }
            }
        }

        public static void Initializate(int scale)
        {
            Scale = scale;
            CS = PS = QS = ES = 1;
            Q = E = COM = DRAGGED = false;
        }
        public static void Open(ConsoleKey cki)
        {
            switch (cki)
            {
                case ConsoleKey.Q: if (DRAGGED) { MapView.Drop(false); DRAGGED = false; } else Display("QF"); break;
                case ConsoleKey.E: if (DRAGGED) { MapView.Drop(true); DRAGGED = false; } else Display("EP"); break;
            }
            while (Q || E || C)
            {
                cki = Console.ReadKey(true).Key; 
                if (C) Hide("C"); 
                else switch (cki)
                {
                    case ConsoleKey.Q:
                        if (!E) Hide("Q");                              // [Q] !E  >> !Q  !E  // schowaj menu czynnosci dla pola
                        else if (!Q) DisplayProductActions();           // !Q  [E] >> [Q]  E  // wyswietl menu czynnosci dla przedmiotu
                        else if (!COM) GoBack("Q");                     //  Q  [E] >> [Q] !E  // powroc do menu czynnosci dla pola
                        else if (COM) ConfirmProductAction(); break;    // [Q]  E  >> !Q  !E  // zatwierdz czynnosc dla przedmiotu

                    case ConsoleKey.E:
                        if (!Q) Hide("E");                              // !Q  [E] >> !Q  !E  // schowaj menu przedmiotow
                        else if (!E) SelectFieldAction();               // [Q] !E  >>  Q  [E] // wybierz czynnosc dla pola
                        else if (COM) GoBack("E");                      // [Q]  E  >> !Q  [E] // powroc do menu przedmiotow
                        else if (!COM) SelectProduct(); break;          //  Q  [E] >> !Q  !E  // zatwierdz przedmiot dla czynnosci pola

                    case ConsoleKey.W: if (CS > 1)
                            { CS--; MainViewService.UpdateSelect(CS, PS, MenuSize); PS = CS; } break;
                    case ConsoleKey.S: if (CS < MenuSize)
                            { CS++; MainViewService.UpdateSelect(CS, PS, MenuSize); PS = CS; } break;
                    case ConsoleKey.Escape: Hide("ALL"); break;
                }
            }
        }
    }
}
