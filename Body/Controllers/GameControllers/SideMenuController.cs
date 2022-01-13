using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views;
using FarmConsole.Body.Views.LocationViews;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace FarmConsole.Body.Controlers
{
    class SideMenuController : MainController
    {
        private static string InventoryLabel;
        private static string[] FieldActions;
        private static string[] BlockedActions;
        private static string[] ProductActions;
        private static List<ProductModel> Inventory;
        private static List<ProductModel> SelectedProducts;
        private static IDictionary<string, CM> MenuSize;
        private static ViewModel View;

        private static int Scale, MenuCount, CS, PS, QS, ES;     // CurrentSelect / PreviousSelect / Q-Select / E-Select //
        private static bool Q, C, E, COM, DRAGGED;              // CurrentOpenMenu / true - Q / false - E //

        private static void Display(string menu)
        {
            Inventory = GameInstance.GetProductsByScale(Scale);
            switch (menu)
            {
                case "QFT":
                    FieldActions = ConvertService.SelectAllowedActions(MapManager.GetField().ToProduct().MapActions, MapEngine.Map.AccessLevel);
                    MenuCount = FieldActions.Length; CS = PS = 1; SelectFieldAction(); break;

                case "QF":
                    ProductModel Field = MapManager.GetField().ToProduct(); CS = PS = 1; COM = Q = true;
                    FieldActions = ConvertService.SelectAllowedActions(Field.MapActions, MapEngine.Map.AccessLevel);
                    MenuCount = FieldActions.Length; string Title = Field.StateName.Length > 0 && Field.StateName[0] >= 'A' ?
                        LS.Object(Field.StateName) + " " + LS.Object(Field.ObjectName) : LS.Object(Field.ObjectName);
                    MenuSize["L"] = SideMenuView.DisplayLeftMenu(FieldActions, Title, CS); break;

                case "QP":
                    ES = CS; ProductActions = SelectedProducts[ES - 2].MenuActions; MenuCount = ProductActions.Length; CS = PS = 1;
                    MenuSize["L"] = SideMenuView.DisplayLeftMenu(ProductActions, LS.Object(SelectedProducts[ES - 2].ObjectName), CS, true); COM = Q = true; break;

                case "EF":
                    QS = CS; SelectedProducts = ProductModel.SelectProductsByAction(Inventory, Action.Name); CS = PS = 1; COM = false; E = true;
                    MenuCount = SelectedProducts.Count; MenuSize["R"] = SideMenuView.DisplayRightMenu(SelectedProducts, InventoryLabel, CS, extended: true); break;

                case "EP":
                    COM = false; E = true; CS = PS = 1; SelectedProducts = Inventory; MenuCount = Inventory.Count + 1;
                    MenuSize["R"] = SideMenuView.DisplayRightMenu(SelectedProducts, LS.Object("inventory"), CS, search: true); break;

                case "SE":
                    MenuCount = SelectedProducts.Count + 1; CS = PS = 1;
                    MenuSize["R"] = SideMenuView.DisplayRightMenu(SelectedProducts, LS.Object("inventory"), CS, search: true); break;
            }
        }
        private static void Hide(string menu)
        {
            switch (menu)
            {
                case "Q": FillMenuBg("L"); QS = 1; Q = false; break;
                case "E": FillMenuBg("R"); ES = 1; E = false; break;
                case "C": FillMenuBg("C"); C = false; break;
                case "S": FillMenuBg("S"); break;
                case "2": FillMenuBg("LR"); QS = ES = 1; Q = E = false; break;
                case "ALL": FillMenuBg("LRC"); QS = ES = 1; Q = E = C = false; break;
            }
            MenuManager.Clean(WithCleaning: false);
        }
        private static void FillMenuBg(string type)
        {
            CM cm;
            for (int i = 0; i < type.Length; i++)
            {
                cm = MenuSize[type[i].ToString()];
                if (cm != null)
                {
                    if (type[i] == 'S') View.DisplayPixels(new Point(cm.Pos.X, 4), new Size(cm.Size.Width, Console.WindowHeight - cm.Size.Height - 9));
                    else View.DisplayPixels(cm.Pos, new Size(cm.Size.Width, cm.Size.Height + 1));
                }
            }
        }
        private static void GoBack(string menu) 
        {
            switch (menu)
            {
                case "Q": 
                    Hide("E"); COM = true; CS = PS = QS; MenuCount = FieldActions.Length;
                    SideMenuView.RestoreLeftMenu(); break;

                case "E":
                    Hide("Q"); COM = false; CS = PS = ES;  MenuCount = SelectedProducts.Count + 1;
                    SideMenuView.RestoreRightMenu(); break;
            }
        }
        private static void Info(string content)
        {
            if(Q) Hide("Q");
            if(E) Hide("E");
            if (content != LS.Action("done"))
            {
                C = true;
                MenuSize["C"] = SideMenuView.DisplayCenterMenu(content);
            }
        }

        private static void SelectFieldAction()
        {
            if (FieldActions.Length > 0)
            {
                Action.Type = "OnMap";
                Action.Name = FieldActions[CS - 1];
                switch (Action.Name)
                {
                    case "build": InventoryLabel = LS.Object("objects"); Display("EF"); break;
                    case "sow": InventoryLabel = LS.Object("seeds"); Display("EF"); break;
                    case "fertilize": InventoryLabel = LS.Object("fertilizers"); Display("EF"); break;
                    case "move": DRAGGED = true; Info(SideMenuService.TakeAction()); break;
                    default: Info(SideMenuService.TakeAction()); break;
                }
            }
        }
        private static void SelectProduct()
        {
            if (SelectedProducts.Count > 0)
            {
                Action.Type = "InInventory";
                Action.SelectedProduct = SelectedProducts[CS - 1];
                Info(SideMenuService.TakeAction());
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
            {
                Action.Type = "InInventory";
                Action.Name = ProductActions[CS - 1];
                Action.SelectedProduct = SelectedProducts[ES - 2];
                Info(SideMenuService.TakeAction());
            }
        }
        private static void FilterProducts()
        {
            string Title = LS.Object("inventory");
            ConsoleKey cki;
            string SearchedPhrase = "", DefaultString = ":";
            SelectedProducts = new List<ProductModel>();
            MenuSize["S"] = SideMenuView.DisplaySearchRightMenu(Inventory, Title, DefaultString);
            while (true)
            {
                cki = Console.ReadKey(true).Key;
                if (cki == ConsoleKey.Q) { if (SelectedProducts.Count == 0) Display("EP"); else Display("SE"); return; }
                if(cki == ConsoleKey.Backspace && SearchedPhrase.Length > 0)
                {
                    SearchedPhrase = SearchedPhrase[0..^1];
                    SelectedProducts = new List<ProductModel>();
                    foreach (var P in Inventory) if (LS.Object(P.ObjectName).ToLower().Contains(SearchedPhrase.ToLower())) SelectedProducts.Add(P);
                    if (SearchedPhrase.Length > 0) MenuSize["S"] = SideMenuView.DisplaySearchRightMenu(SelectedProducts, Title, SearchedPhrase);
                    else MenuSize["S"] = SideMenuView.DisplaySearchRightMenu(SelectedProducts, Title, DefaultString);
                    Hide("S");
                }
                if ((Convert.ToInt16(cki) > 64 && Convert.ToInt16(cki) < 91 || cki == ConsoleKey.Spacebar) && SearchedPhrase.Length < 15)
                {
                    SearchedPhrase += ((char)Convert.ToInt16(cki)).ToString();
                    SelectedProducts = new List<ProductModel>();
                    foreach(var P in Inventory) if (LS.Object(P.ObjectName).ToLower().Contains(SearchedPhrase.ToLower())) SelectedProducts.Add(P);
                    MenuSize["S"] = SideMenuView.DisplaySearchRightMenu(SelectedProducts, Title, SearchedPhrase);
                    Hide("S");
                }
            }
        }

        public static void Init(int scale)
        {
            Scale = scale;
            CS = PS = QS = ES = 1;
            Q = E = COM = DRAGGED = false;
            MenuSize = new Dictionary<string, CM>
            {
                { "L", null },
                { "R", null },
                { "C", null },
                { "S", null }
            };
        }
        public static void Open(ConsoleKey cki)
        {
            BlockedActions = new string[] { };
            View = MapEngine.Map.View;
            switch (cki)
            {
                case ConsoleKey.Tab: if (DRAGGED) { MapManager.Drop(false); DRAGGED = false; } else Display("QFT"); break;
                case ConsoleKey.Q: if (DRAGGED) { MapManager.Drop(false); DRAGGED = false; } else Display("QF"); break;
                case ConsoleKey.E: if (DRAGGED) { MapManager.Drop(true); DRAGGED = false; } else Display("EP"); break;
            }
            while (Q || E || C)
            {
                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true).Key;
                    if (C) Hide("C");
                    else switch (cki)
                        {
                            case ConsoleKey.Q:
                                if (!E) Hide("Q");                              // [Q] !E  >> !Q  !E  // hide menu czynnosci dla pola
                                else if (!Q) DisplayProductActions();           // !Q  [E] >> [Q]  E  // wyswietl menu czynnosci dla przedmiotu
                                else if (!COM) GoBack("Q");                     //  Q  [E] >> [Q] !E  // powroc do menu czynnosci dla pola
                                else if (COM) ConfirmProductAction(); break;    // [Q]  E  >> !Q  !E  // zatwierdz czynnosc dla przedmiotu

                            case ConsoleKey.E:
                                if (!Q) Hide("E");                              // !Q  [E] >> !Q  !E  // hide menu przedmiotow
                                else if (!E) SelectFieldAction();               // [Q] !E  >>  Q  [E] // wybierz czynnosc dla pola
                                else if (COM) GoBack("E");                      // [Q]  E  >> !Q  [E] // powroc do menu przedmiotow
                                else if (!COM) SelectProduct(); break;          //  Q  [E] >> !Q  !E  // zatwierdz przedmiot dla czynnosci pola

                            case ConsoleKey.W:
                                if (CS > 1)
                                { CS--; SideMenuView.UpdateMenuSelect(CS, PS, MenuCount); PS = CS; }
                                break;

                            case ConsoleKey.S:
                                if (CS < MenuCount)
                                { CS++; SideMenuView.UpdateMenuSelect(CS, PS, MenuCount); PS = CS; }
                                break;

                            case ConsoleKey.Tab:
                            case ConsoleKey.Escape: Hide("ALL"); break;
                        }
                }
                else if ((DateTime.Now - Previously).TotalMilliseconds >= FrameTime)
                {
                    if (Action.IsInProcess) SideMenuService.MakeAction();
                    Previously = DateTime.Now;
                }
            }
        }
    }
}
