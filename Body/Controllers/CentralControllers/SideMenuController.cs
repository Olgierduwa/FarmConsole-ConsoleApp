using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views;
using FarmConsole.Body.Views.LocationViews;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace FarmConsole.Body.Controlers
{
    public class SideMenuController : MainController
    {
        private static string InventoryLabel;
        private static string Action;
        private static string[] FieldActions;
        private static string[] ProductActions;
        private static List<ProductModel> Inventory;
        private static List<ProductModel> SelectedProducts;
        private static Point[] MenuDimensions = new Point[3];

        private static int Scale, MenuSize, CS, PS, QS, ES;     // CurrentSelect / PreviousSelect / Q-Select / E-Select //
        private static bool Q, C, E, COM, DRAGGED;              // CurrentOpenMenu / true - Q / false - E //

        private static void Display(string menu)
        {
            Inventory = GameInstance.GetProductsByScale(Scale);
            switch (menu)
            {
                case "QF":
                    ProductModel Field = ProductModel.GetProduct(MapView.GetField()); CS = PS = 1;
                    FieldActions = Field.MapActions; MenuSize = FieldActions.Length; COM = Q = true;
                    string Title = Field.StateName.Length == 0 || Field.StateName[0] == '-' ? Field.ProductName : Field.StateName + " " + Field.ProductName;
                    MenuDimensions[0] = SideMenuView.DisplayLeftMenu(FieldActions, Title, CS); break;

                case "QP":
                    ES = CS; ProductActions = SelectedProducts[ES - 2].MenuActions; MenuSize = ProductActions.Length; CS = PS = 1;
                    MenuDimensions[0] = SideMenuView.DisplayLeftMenu(ProductActions, SelectedProducts[ES - 2].ProductName, CS, true); COM = Q = true; break;

                case "EF":
                    QS = CS; SelectedProducts = ProductModel.GetProductsByAction(Inventory, Action); CS = PS = 1; COM = false; E = true;
                    MenuSize = SelectedProducts.Count; MenuDimensions[1] = SideMenuView.DisplayRightMenu(SelectedProducts, InventoryLabel, ES, extended: true); break;

                case "EP":
                    COM = false; E = true; CS = PS = 1; SelectedProducts = Inventory; MenuSize = Inventory.Count + 1;
                    MenuDimensions[1] = SideMenuView.DisplayRightMenu(SelectedProducts, "Ekwipunek", CS, search: true); break;

                case "SE":
                    MenuSize = SelectedProducts.Count + 1; CS = PS = 1;
                    MenuDimensions[1] = SideMenuView.DisplayRightMenu(SelectedProducts, "Ekwipunek", CS, search: true); break;
            }
        }
        private static void Hide(string menu)
        {
            switch (menu)
            {
                case "Q":   MapEngine.ShowMapFragment("L", MenuDimensions); QS = 1; Q = false; break;
                case "E":   MapEngine.ShowMapFragment("R", MenuDimensions); ES = 1; E = false; break;
                case "C":   MapEngine.ShowMapFragment("C", MenuDimensions); C = false; break;
                case "S":   MapEngine.ShowMapFragment("S", MenuDimensions); break;
                case "2":   MapEngine.ShowMapFragment("LR", MenuDimensions); QS = ES = 1; Q = E = false; break;
                case "ALL": MapEngine.ShowMapFragment("LRC", MenuDimensions); QS = ES = 1; Q = E = C = false; break;
            }
        }
        private static void GoBack(string menu) 
        {
            switch (menu)
            {
                case "Q": 
                    Hide("E"); COM = true; CS = PS = QS; MenuSize = FieldActions.Length;
                    ProductModel Product = ProductModel.GetProduct(MapView.GetField());
                    SideMenuView.RestoreLeftMenu(); break;

                case "E":
                    Hide("Q"); COM = false; CS = PS = ES;  MenuSize = SelectedProducts.Count + 1;
                    SideMenuView.RestoreRightMenu(); break;
            }
        }
        private static void Info(string content)
        {
            if(Q) Hide("Q");
            if(E) Hide("E");
            if (content != XF.GetString("done"))
            {
                C = true;
                MenuDimensions[2] = SideMenuView.DisplayCenterMenu(content);
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
                    case "Postaw": InventoryLabel = "Obiekty"; Display("EF"); break;
                    case "Posiej": InventoryLabel = "Nasiona"; Display("EF"); break;
                    case "Nawieź": InventoryLabel = "Nawozy"; Display("EF"); break;
                    default: Info(SideMenuService.DoOnMap(Action)); break;
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
            if(ProductActions.Length > 0)
                Info(SideMenuService.DoInInventory(ProductActions[CS - 1], SelectedProducts[ES - 2]));
        }
        private static void FilterProducts()
        {
            string Title = "Ekwpiunek";
            ConsoleKey cki;
            string SearchedPhrase = "", DefaultString = ":";
            SelectedProducts = new List<ProductModel>();
            SideMenuView.DisplaySearchRightMenu(Inventory, Title, DefaultString);
            while (true)
            {
                cki = Console.ReadKey(true).Key;
                if (cki == ConsoleKey.Q) { if (SelectedProducts.Count == 0) Display("EP"); else Display("SE"); return; }
                if(cki == ConsoleKey.Backspace && SearchedPhrase.Length > 0)
                {
                    SearchedPhrase = SearchedPhrase[0..^1];
                    SelectedProducts = new List<ProductModel>();
                    foreach (var P in Inventory) if (P.ProductName.ToLower().Contains(SearchedPhrase.ToLower())) SelectedProducts.Add(P);
                    if (SearchedPhrase.Length > 0) MenuDimensions[1] = SideMenuView.DisplaySearchRightMenu(SelectedProducts, Title, SearchedPhrase);
                    else MenuDimensions[1] = SideMenuView.DisplaySearchRightMenu(SelectedProducts, Title, DefaultString);
                    Hide("S");
                }
                if ((Convert.ToInt16(cki) > 64 && Convert.ToInt16(cki) < 91 || cki == ConsoleKey.Spacebar) && SearchedPhrase.Length < 15)
                {
                    SearchedPhrase += ((char)Convert.ToInt16(cki)).ToString();
                    SelectedProducts = new List<ProductModel>();
                    foreach(var P in Inventory) if (P.ProductName.ToLower().Contains(SearchedPhrase.ToLower())) SelectedProducts.Add(P);
                    MenuDimensions[1] = SideMenuView.DisplaySearchRightMenu(SelectedProducts, Title, SearchedPhrase);
                    Hide("S");
                }
            }
        }

        public static void Init(int scale)
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
                            { CS--; ComponentEngine.UpdateSelect(CS, PS, MenuSize); PS = CS; } break;
                    case ConsoleKey.S: if (CS < MenuSize)
                            { CS++; ComponentEngine.UpdateSelect(CS, PS, MenuSize); PS = CS; } break;
                    case ConsoleKey.Escape: Hide("ALL"); break;
                }
            }
        }
    }
}
