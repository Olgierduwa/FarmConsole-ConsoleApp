using FarmConsole.Body.Controllers.CentralControllers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services.GameServices;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Views.GameViews;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FarmConsole.Body.Controllers.GameControllers
{
    class SideMenuController : HeadController
    {
        private static string ProductsLabel;
        private static string[] FieldActions;
        private static string[] ProductActions;
        private static List<ProductModel> FilteredProducts;
        private static List<ProductModel> ProductsGroup;
        private static IDictionary<string, CM> MenuSize;
        private static ViewModel View;

        private static int Scale, MenuCount, CS, PS, QS, ES;     // CurrentSelect / PreviousSelect / Q-Select / E-Select //
        private static bool Q, C, E, COM, TAB, DRAGGED;              // CurrentOpenMenu / true - Q / false - E //

        // [Q] LEFT MENU
        private static void DisplayFieldActions()
        {
            CS = PS = 1;
            COM = Q = true;

            var Field = MapEngine.GetField();
            string ActionsLabel = Field.StateName.Length > 0 && Field.StateName[0] >= 'A' ?
                LS.Object(Field.StateName) + " " + LS.Object(Field.ObjectName) : LS.Object(Field.ObjectName);

            FieldActions = ConvertService.SelectAllowedActions(Field.MapActions, MapEngine.Map.AccessLevel);
            MenuCount = FieldActions.Length;
            MenuSize["L"] = SideMenuView.DisplayLeftMenu(FieldActions, ActionsLabel, CS);
        }
        private static void DisplayFieldActionProducts(string productsGroup)
        {
            QS = CS;
            CS = PS = 1;
            COM = false;
            E = true;

            ProductsLabel = LS.Object(productsGroup);
            ProductsGroup = ProductModel.SelectProductsByAction(GameInstance.GetProductsByScale(Scale), Action.Name);
            FilteredProducts = ProductsGroup.ToList();
            MenuCount = ProductsGroup.Count + 1;
            MenuSize["R"] = SideMenuView.DisplayRightMenu(FilteredProducts, ProductsLabel, CS, extended: true);
        }
        private static void ConfirmFirstFieldAction()
        {
            CS = PS = 1;
            TAB = true;

            FieldActions = ConvertService.SelectAllowedActions(MapEngine.GetField().ToProduct().MapActions, MapEngine.Map.AccessLevel);
            MenuCount = FieldActions.Length;
            ConfirmFieldAction();
        }
        private static void ConfirmFieldAction()
        {
            if (FieldActions.Length > 0)
            {
                Action.Type = "OnMap";
                Action.Name = FieldActions[CS - 1];
                switch (Action.Name)
                {
                    case "build": DisplayFieldActionProducts("objects"); break;
                    case "sow": DisplayFieldActionProducts("seeds"); break;
                    case "fertilize": DisplayFieldActionProducts("fertilizers"); break;
                    case "move": DRAGGED = true; SideMenuService.TakeAction(); ShowResult(); break;
                    default: SideMenuService.TakeAction(); ShowResult(); break;
                }
            }
            else TAB = false;
        }
        private static void ConfirmFieldActionProduct()
        {
            if (FilteredProducts.Count > 0)
            {
                if (CS == 1) FilterProducts(false);
                else
                {
                    Action.Type = "InInventory";
                    Action.SelectedProduct = FilteredProducts[CS - 2];
                    SideMenuService.TakeAction();
                    ShowResult();
                }
            }
        }
        private static void GoBackFieldActions()
        {
            HideSideMenu("R");
            CS = PS = QS;
            COM = true;
            MenuCount = FieldActions.Length;
            SideMenuView.RestoreLeftMenu();
        }

        // [E] RIGHT MENU
        private static void DisplayProducts()
        {
            CS = PS = 1;
            COM = false;
            E = true;

            ProductsLabel = LS.Object("inventory");
            ProductsGroup = GameInstance.GetProductsByScale(Scale);
            FilteredProducts = ProductsGroup.ToList();
            MenuCount = ProductsGroup.Count + 1;
            MenuSize["R"] = SideMenuView.DisplayRightMenu(FilteredProducts, ProductsLabel, CS);
        }
        private static void DisplayProductActions()
        {
            if (FilteredProducts.Count > 0)
            {
                if (CS == 1) FilterProducts(true);
                else
                {
                    ES = CS;
                    CS = PS = 1;
                    COM = Q = true;

                    var Product = FilteredProducts[ES - 2];
                    ProductActions = Product.MenuActions;
                    MenuCount = ProductActions.Length;
                    MenuSize["L"] = SideMenuView.DisplayLeftMenu(ProductActions, LS.Object(Product.ObjectName), CS, true);
                }
            }
        }
        private static void ConfirmFirstProductAction()
        {
            if (CS == 1) HideSideMenu("R");
            else
            {
                ES = CS;
                CS = PS = 1;
                TAB = true;

                var Product = FilteredProducts[ES - 2];
                ProductActions = Product.MenuActions;
                MenuCount = ProductActions.Length;
                ConfirmProductAction();
            }
        }
        private static void ConfirmProductAction()
        {
            if (ProductActions.Length > 0)
            {
                Action.Type = "InInventory";
                Action.Name = ProductActions[CS - 1];
                Action.SelectedProduct = FilteredProducts[ES - 2];
                SideMenuService.TakeAction();
                ShowResult();
            }
        }
        private static void GoBackProducts()
        {
            HideSideMenu("L");
            CS = PS = ES;
            COM = false;
            MenuCount = FilteredProducts.Count + 1;
            SideMenuView.RestoreRightMenu();
        }

        // SERVICE
        private static void FilterProducts(bool inventory = true)
        {
            string Title = ProductsLabel, SearchedPhrase = "", DefaultString = ":";
            FilteredProducts = new List<ProductModel>();
            MenuSize["S"] = SideMenuView.DisplayRightMenu(ProductsGroup, Title, 1, filter: DefaultString);
            while (true)
            {
                ConsoleKey cki = Console.ReadKey(true).Key;

                if (cki == ConsoleKey.Tab || cki == ConsoleKey.Escape)
                {
                    if (FilteredProducts.Count == 0) FilteredProducts = ProductsGroup;
                    MenuCount = FilteredProducts.Count + 1;
                    MenuSize["R"] = SideMenuView.DisplayRightMenu(FilteredProducts, Title, CS, extended: !inventory);
                    return;
                }

                if (cki == ConsoleKey.Backspace && SearchedPhrase.Length > 0)
                {
                    SearchedPhrase = SearchedPhrase[0..^1];
                    FilteredProducts = new List<ProductModel>();
                    foreach (var P in ProductsGroup) if (LS.Object(P.ObjectName).ToLower().Contains(SearchedPhrase.ToLower())) FilteredProducts.Add(P);
                    if (SearchedPhrase.Length > 0) MenuSize["S"] = SideMenuView.DisplayRightMenu(FilteredProducts, Title, 1, filter: SearchedPhrase);
                    else MenuSize["S"] = SideMenuView.DisplayRightMenu(FilteredProducts, Title, 1, filter: DefaultString);
                    HideSideMenu("S");
                }

                if ((Convert.ToInt16(cki) > 64 && Convert.ToInt16(cki) < 91 || cki == ConsoleKey.Spacebar) && SearchedPhrase.Length < 15)
                {
                    SearchedPhrase += ((char)Convert.ToInt16(cki)).ToString();
                    FilteredProducts = new List<ProductModel>();
                    foreach (var P in ProductsGroup) if (LS.Object(P.ObjectName).ToLower().Contains(SearchedPhrase.ToLower())) FilteredProducts.Add(P);
                    MenuSize["S"] = SideMenuView.DisplayRightMenu(FilteredProducts, Title, 1, filter: SearchedPhrase);
                    HideSideMenu("S");
                }
            }
        }
        private static void HideSideMenu(string type, bool tab = false)
        {
            foreach (char symbol in type)
            {
                CM cm = MenuSize[symbol.ToString()];
                if (cm != null)
                {
                    if (symbol == 'S') View.DisplayPixels(new Point(cm.Pos.X, 4), new Size(cm.Size.Width, Console.WindowHeight - cm.Size.Height - 9));
                    else View.DisplayPixels(cm.Pos, new Size(cm.Size.Width, cm.Size.Height + 1));
                }

                switch (symbol.ToString())
                {
                    case "L": QS = 1; Q = false; break;
                    case "R": ES = 1; E = false; break;
                    case "C": C = false; break;
                }
            }
            if (tab) TAB = false;
            ComponentService.Clean(WithCleaning: false);
        }
        public static void ShowResult()
        {
            TAB = false;
            if (Q) HideSideMenu("L");
            if (E) HideSideMenu("R");
            if (Action.Result != LS.Action("done"))
            {
                C = true;
                MenuSize["C"] = SideMenuView.DisplayCenterMenu(Action.Result, Action.ResultTitle);
                Action.ResultTitle = "";
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
            View = MapEngine.Map.View;
            ProductsLabel = LS.Object("inventory");
            switch (cki)
            {
                case ConsoleKey.Tab: if (DRAGGED) { MapService.Drop(false); DRAGGED = false; } else ConfirmFirstFieldAction(); break;
                case ConsoleKey.Q: if (DRAGGED) { MapService.Drop(false); DRAGGED = false; } else DisplayFieldActions(); break;
                case ConsoleKey.E: if (DRAGGED) { MapService.Drop(true); DRAGGED = false; } else DisplayProducts(); break;
            }
            while (Q || E || C)
            {
                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true).Key;
                    if (C) HideSideMenu("C");
                    else switch (cki)
                        {
                            case ConsoleKey.Q:
                                if (!E) HideSideMenu("L");                     // [Q] !E  >> !Q  !E  // schowaj menu czynnosci dla pola
                                else if (TAB) HideSideMenu("R", tab: true);         // !Q  [E] >> !Q  !E  // anuluj wybór przedmiotu dla szybkiej czynności
                                else if (!Q) DisplayProductActions();               // !Q  [E] >> [Q]  E  // wyswietl menu czynnosci dla przedmiotu
                                else if (COM) ConfirmProductAction();               // [Q]  E  >> !Q  !E  // zatwierdz czynnosc dla przedmiotu
                                else if (!COM) GoBackFieldActions(); break;         //  Q  [E] >> [Q] !E  // powroc do menu czynnosci dla pola

                            case ConsoleKey.E:
                                if (TAB) ConfirmFieldActionProduct();          // !Q  [E] >> !Q  !E  // zatwierdz przedmiot dla szybkiej czynnosci
                                else if (!Q) HideSideMenu("R");                     // !Q  [E] >> !Q  !E  // schowaj menu przedmiotow
                                else if (!E) ConfirmFieldAction();                  // [Q] !E  >>  Q  [E] // zatwierdz czynnosc dla pola
                                else if (!COM) ConfirmFieldActionProduct();         //  Q  [E] >> !Q  !E  // zatwierdz przedmiot dla czynnosci pola
                                else if (COM) GoBackProducts(); break;              // [Q]  E  >> !Q  [E] // powroc do menu przedmiotow

                            case ConsoleKey.Tab:
                                if (!TAB && !Q && E) ConfirmFirstProductAction();   // !Q  [E] >> !Q  !E  // zatwierdz szybką czynnosc dla przedmiotu
                                else HideSideMenu("LRC", tab: true); break;

                            case ConsoleKey.W:
                                if (CS > 1)
                                { CS--; ComponentService.UpdateMenuSelect(CS, PS, MenuCount, Prop: 14); PS = CS; }
                                break;

                            case ConsoleKey.S:
                                if (CS < MenuCount)
                                { CS++; ComponentService.UpdateMenuSelect(CS, PS, MenuCount, Prop: 14); PS = CS; }
                                break;

                            case ConsoleKey.Escape: HideSideMenu("LRC", tab: true); break;
                        }
                }
                else if ((DateTime.Now - Previously).TotalMilliseconds >= FrameTime)
                {
                    if (Action.IsInProcess) if (!SideMenuService.MakeAction()) ShowResult();
                    Previously = DateTime.Now;
                }
            }
        }
    }
}
