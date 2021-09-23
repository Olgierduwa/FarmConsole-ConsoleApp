using FarmConsole.Body.Models;
using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.LocationViews;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Controlers
{
    public class SideMenuController : MainControllerService
    {
        private static List<int>[] FACT_ID; // field activities ID
        private static List<int>[] IACT_ID; // item activities ID
        private static List<string>[] FACT; // field activities
        private static List<string>[] IACT; // item activities
        private static List<ProductModel>[] items;

        private static int catF, catI, LSize, RSize, CS, PS, LS, RS;      // CurrentSelect / PreviousSelect / LeftSelect / RightSelect //
        private static bool Q, C, E, OS, DRAGGED;                         // OppositeSelect / true - left (OS - Q) / false - right (!OS - E) //

        private static void Hide(string menu)
        {
            switch (menu)
            {
                case "Q": Q = false; LS = 1; MenuLeftView.Clean(); MapService.ShowMapFragment("left"); break;
                case "E": E = false; RS = 1; MenuRightView.Clean(); MapService.ShowMapFragment("right"); break;
                case "C": C = false; MenuCenterView.Clean(); MapService.ShowMapFragment("center"); break;
                case "2": Q = E = false; LS = RS = 1; MenuLeftView.Clean(); MenuRightView.Clean();
                    MapService.ShowMapFragment("right"); MapService.ShowMapFragment("left"); break;
                case "ALL": Q = E = C = false; LS = RS = 1; MenuLeftView.Clean(); MenuRightView.Clean(); MenuCenterView.Clean();
                    MapService.ShowMapFragment("all"); break;
            }
        }
        private static void Show(string type = "", string name = "", int duration = 0, string opis = "")
        {
            switch (type)
            {
                case "QI": Q = true; MenuLeftView.Show(IACT[catI], LS, true); break;
                case "QF": Q = true; MenuLeftView.Show(FACT[catF], LS, false); break;
                case "EI": E = true; MenuRightView.Show(items, catI, RS, false); break;
                case "EF": E = true; MenuRightView.Show(items, catI, RS, true); break;
                default: C = true; MenuCenterView.Show(type, name, duration, opis); break;
            }
        }
        private static void Refresh(string menu)
        {
            switch (menu)
            {
                case "QI": Q = true; MenuLeftView.Show(IACT[catI], LS, true, true); break;
                case "QF": Q = true; MenuLeftView.Show(FACT[catF], LS, false, true); break;
                case "EI": E = true; MenuRightView.Show(items, catI, RS, false, true); break;
                case "EF": E = true; MenuRightView.Show(items, catI, RS, true, true); break;
            }
        }
        private static void Update(string menu)
        {
            switch (menu)
            {
                case "Q": MenuLeftView.UpdateSelect(CS, PS, LSize); break;
                case "E": MenuRightView.UpdateSelect(CS, PS, RSize, 5); break;
            }
        }

        private static void FACTMenager() // wybierz akcje dla pola 
        {
            switch (FACT_ID[catF][CS - 1])
            {
                case 1: FarmView.Destroy(); Hide("Q"); break; // zniszcz
                case 2: FarmView.Dragg(); DRAGGED = true; Hide("Q"); break; // chwyc
                case 7: catI = 1; break; // postaw
                case 8: catI = 4; break; // posiej
                case 9: catI = 6; break; // nawiez
                case 6: C = FarmView.Plow(); Hide("Q"); if (C) { Show("warrning", XF.GetString(401)); } break; // zaoraj
                case 10: C = FarmView.WaterIt(); Hide("Q"); if (C) { Show("warrning", XF.GetString(402)); } break; // podlej
                case 11: C = FarmView.Collect(); Hide("Q"); if (C) { Show("warrning", XF.GetString(403)); } break; // zbierz
                case 12: C = FarmView.MakeFertilize(); Hide("Q"); if (C) { Show("warrning", XF.GetString(404)); } break; // zrob nawoz

                case 3: break; // sprzedaj
                case 4: break; // odwiedz
                case 5: break; // uzyj

                case 0: C = true; Show("field", XF.GetFieldName(FarmView.GetFieldProp("category"), FarmView.GetFieldProp("type")),
                        FarmView.GetFieldProp("duration"), XF.GetFieldDescription(FarmView.GetFieldProp("category"), FarmView.GetFieldProp("type"))); break; // info
            }
            if (FACT_ID[catF][CS - 1] > 6 && FACT_ID[catF][CS - 1] < 10)
            { OS = false; RSize = items[catI].Count; E = !OS; LS = PS; CS = PS = 1; Show("EF"); }
        }
        private static void FACTConfirm() // zatwierdz akcje dla pola 
        {
            if (items[catI].Count > 0)
            {
                ProductModel p = items[catI][CS - 1];
                switch (FACT_ID[catF][LS - 1])
                {
                    case 7: FarmView.Build(p.category, p.type); break; // postaw
                    case 8: C = FarmView.Sow(p); if (C) { Show("warrning", XF.GetString(405)); } break; // posiej
                    case 9: C = FarmView.Fertilize(p); if (C) { Show("warrning", XF.GetString(406)); } break; // uzyznij
                }
                if (!C) { items[catI][CS - 1].amount--; if (items[catI][CS - 1].amount == 0) items[catI].RemoveAt(CS - 1); }
                Hide("2");
                LS = PS = CS = RS = 1;
            }
        }
        private static void IACTMenager() // wybierz akcje dla przedmiotu 
        {
            LSize = IACT[catI].Count;
            if (items[catI].Count > 0)
            { Q = !Q; RS = PS; CS = PS = 1; Show("QI"); OS = true; }
        }
        private static void IACTConfirm() // zatwierdz akcje dla przedmiotu 
        {
            ProductModel p = items[catI][RS - 1];
            switch (IACT_ID[catI][CS - 1])
            {
                case 0: C = true; Show("item", items[catI][RS - 1].name, items[catI][RS - 1].amount, "XD produkcik"); break; // info
                case 1:
                    items[catI][RS - 1].amount--; if (items[catI][RS - 1].amount == 0)
                    {
                        items[catI].RemoveAt(RS - 1); OS = !OS; PS = CS = RS = 1;
                        Hide("Q"); RSize = items[catI].Count;
                    }
                    MenuRightView.UpdateItemList(items, catI); Update("E"); break; // zniszcz
                case 3:
                    if (FarmView.GetFieldProp("category") < 6)
                    {
                        Hide("2"); FarmView.Build(p.category, p.type); items[catI][RS - 1].amount--;
                        if (items[catI][RS - 1].amount == 0) items[catI].RemoveAt(RS - 1);
                    }
                    else { C = true; Show("warrning", XF.GetString(407)); } break; // postaw
                case 4:
                    switch (catI) // uzyj
                    {
                        case 4:
                            if (FarmView.GetFieldProp("category") == 1)
                            {
                                C = FarmView.Sow(p); if (C) { Show("warrning", XF.GetString(405)); }
                                else { Hide("2"); items[catI][RS - 1].amount--; if (items[catI][RS - 1].amount == 0) items[catI].RemoveAt(RS - 1); }
                            }
                            else { C = true; Show("warrning", XF.GetString(408)); } break; // posiej
                        case 6:
                            if (FarmView.GetFieldProp("category") == 3)
                            {
                                C = FarmView.Fertilize(p); if (C) { Show("warrning", XF.GetString(406)); }
                                else { Hide("2"); items[catI][RS - 1].amount--; if (items[catI][RS - 1].amount == 0) items[catI].RemoveAt(RS - 1); }
                            }
                            else { C = true; Show("warrning", XF.GetString(409)); } break; // nawieź
                    }
                    break;
                case 2: break; // sprzedaj
            }
        }

        public static void Initialize()
        {
            FACT_ID = new List<int>[]
            {
                new List<int>() { 6,7,0 },      // 0 pola nieuprawne
                new List<int>() { 8,1,0 },      // 1 pola uprawne
                new List<int>() { 10,1,0 },     // 2 pola posiane
                new List<int>() { 10,9,1,0 },   // 3 pola rosnące
                new List<int>() { 11,1,0 },     // 4 pola dojrzałe
                new List<int>() { 12,1,0 },     // 5 pola zgniłe
                new List<int>() { 4,2,1,0 },    // 6 budynki użytkowe
                new List<int>() { 3,2,1,0 },    // 7 dekoracje statyczne  
                new List<int>() { 5,2,3,0 },    // 8 maszyny rolne
            }; // field activities ID
            IACT_ID = new List<int>[]
            {
                new List<int>() { 0 },          // 0 nieuzytkowe
                new List<int>() { 3,2,1,0 },    // 1 budynki
                new List<int>() { 3,2,1,0 },    // 2 dekoracje
                new List<int>() { 3,2,1,0 },    // 3 maszyny
                new List<int>() { 4,2,1,0 },    // 4 nasiona
                new List<int>() { 2,1,0 },      // 5 plony
                new List<int>() { 4,2,1,0 },    // 6 nawozy
                new List<int>() { 2,1,0 },      // 7 karmy
                new List<int>() { 2,1,0 },      // 8 inne
            }; // item activities ID
            FACT = new List<string>[FACT_ID.Length];
            IACT = new List<string>[IACT_ID.Length];
            items = save.GetInventory();
            for (int i = 0; i < FACT_ID.Length; i++)
            {
                FACT[i] = new List<string>();
                for (int j = 0; j < FACT_ID[i].Count; j++)
                    FACT[i].Add(XF.GetString(100 + FACT_ID[i][j]));
            }
            for (int i = 0; i < IACT_ID.Length; i++)
            {
                IACT[i] = new List<string>();
                for (int j = 0; j < IACT_ID[i].Count; j++)
                    IACT[i].Add(XF.GetString(200 + IACT_ID[i][j]));
            }
            catI = CS = PS = LS = RS = 1;
            LSize = FACT[catF].Count;
            RSize = items[catI].Count;
            Q = E = OS = DRAGGED = false;
        }

        public static void Open(ConsoleKey cki)
        {
            switch (cki)
            {
                case ConsoleKey.Q:
                    if (DRAGGED) { FarmView.Drop(false); DRAGGED = false; LS = PS = CS = RS = 1; }
                    else { CS = PS = 1; catF = FarmView.GetFieldProp("category"); LSize = FACT[catF].Count; Show("QF"); OS = true; } break;

                case ConsoleKey.E:
                    if (DRAGGED) { FarmView.Drop(true); DRAGGED = false; LS = PS = CS = RS = 1; }
                    else { CS = PS = 1; LSize = IACT[catI].Count; Show("EI"); OS = false; } break;
            }
            while (Q || E || C)
            {
                cki = Console.ReadKey(true).Key;
                //HelpService.INFO(4, "OS:"+OS.ToString(), "Q:"+Q.ToString(), "E:"+E.ToString(), "C:"+C.ToString());
                if (C) { Hide("C"); if(Q || E) if (!OS) { RS = PS; Refresh("EI"); } else { LS = PS; if (E) Refresh("QI"); else Refresh("QF"); } }
                switch (cki)
                {
                    case ConsoleKey.Q:
                        if (!E) Hide("Q");                  //  [Q] !E  >> !Q !E  // wylacz menu czynnosci dla pola
                        else if (!Q) IACTMenager();         //  !Q  [E] >> [Q] E  // wlacz menu czynnosci dla przedmiotu
                        else if (!OS) { Hide("E"); Refresh("QF"); CS = PS = LS; LS = 1; OS = true; } //  Q [E] >> [Q] !E // powrot
                        else if (OS) IACTConfirm(); break;  //  [Q]  E  >> !Q !E  // zatwierdz czynnosc dla przedmiotu

                    case ConsoleKey.E:
                        if (!Q) Hide("E");                  //  !Q  [E] >> !Q !E  // wylacz menu przedmiotow
                        else if (!E) FACTMenager();         //  [Q] !E  >>  Q [E] // wlacz menu czynnosci dla pola
                        else if (OS) { Hide("Q"); Refresh("EI"); CS = PS = RS; RS = 1; OS = false; } // [Q] E  >> !Q  [E] // powrot
                        else if (!OS) FACTConfirm(); break; //   Q  [E] >> !Q !E  // zatwierdz czynnosc dla pola

                    case ConsoleKey.W:
                        if (CS > 1) { CS--; if(!OS) Update("E"); else Update("Q"); PS = CS; } break;

                    case ConsoleKey.S:
                        if (!OS && CS < RSize) { CS++; Update("E"); } else if (OS && CS < LSize) { CS++; Update("Q"); } PS = CS; break;

                    case ConsoleKey.A:
                        if (!OS && (Q && FACT_ID[catF][LS - 1] == 7 && catI > 1 || !Q && catI > 1)) {
                            catI--; CS = PS = 1; RSize = items[catI].Count;
                            MenuRightView.UpdateItemList(items, catI); Update("E"); } break;

                    case ConsoleKey.D:
                        if (!OS && (Q && FACT_ID[catF][LS - 1] == 7 && catI < 3 || !Q && catI < items.Length - 1)) {
                            catI++; RSize = items[catI].Count; CS = PS = 1;
                            MenuRightView.UpdateItemList(items, catI); Update("E"); } break;

                    case ConsoleKey.Escape: Q = E = C = false; Hide("ALL"); break;
                }
            }
        }
    }
}
