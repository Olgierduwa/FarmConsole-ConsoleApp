using FarmConsole.Body.Model.Helpers;
using FarmConsole.Body.Model.Objects;
using FarmConsole.Body.Sounds;
using FarmConsole.Body.View.Components;
using FarmConsole.Body.View.GUI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace FarmConsole.Body.Model.Logic
{
    public partial class Logic
    {
        private static class LogicPlay
        {
            private static List<int>[] FACT_ID = new List<int>[]
            {
                    new List<int>() { 0,7,6 },      // 0 pola nieuprawne
                    new List<int>() { 0,1,8 },      // 1 pola uprawne
                    new List<int>() { 0,1,10,9 },   // 2 pola posiane
                    new List<int>() { 0,1,10,9 },   // 3 pola rosnące
                    new List<int>() { 0,1,11 },     // 4 pola dojrzałe
                    new List<int>() { 0,1,12 },     // 5 pola zgniłe
                    new List<int>() { 0,1,2,4 },    // 6 budynki użytkowe
                    new List<int>() { 0,1,2,3 },    // 7 dekoracje statyczne  
                    new List<int>() { 0,2,3,5 },    // 8 maszyny rolne
            }; // field activities ID
            private static List<int>[] IACT_ID = new List<int>[]
            {
                    new List<int>() { 0 },          // 0 nieuzytkowe
                    new List<int>() { 0,1,2,3 },    // 1 budynki
                    new List<int>() { 0,1,2,3 },    // 2 dekoracje
                    new List<int>() { 0,1,2,3 },    // 3 maszyny
                    new List<int>() { 0,1,2,4 },    // 4 nasiona
                    new List<int>() { 0,1,2 },      // 5 plony
                    new List<int>() { 0,1,2,4 },    // 6 nawozy
                    new List<int>() { 0,1,2 },      // 7 karmy
                    new List<int>() { 0,1,2 },      // 8 inne
            }; // item activities ID
            private static List<string>[] FACT = new List<string>[FACT_ID.Length]; // field activities
            private static List<string>[] IACT = new List<string>[IACT_ID.Length]; // item activities
            private static List<Product>[] items;
            private static FieldsMenager fm;

            private static int X, Y, catF, catI, LSize, RSize, CS, PS, LS, RS, invert = 1;      // CurrentSelect / PreviousSelect / LeftSelect / RightSelect //
            private static bool Q, E, OS, DRAGGED;                                              // OppositeSelect / true - left / false - right //
            private static Point DPOINT;

            private static bool FACTMenager()
            {
                switch (FACT_ID[LogicPlay.catF][CS - 1])
                {
                    case 0: break;
                    case 1: fm.Destroy(); Q = !Q; GUI.vt2.ClearList(); fm.ShowPartFarm("left", E); break;
                    case 2: DPOINT = fm.Dragg(); DRAGGED = true; Q = !Q; GUI.vt2.ClearList(); fm.ShowPartFarm("left", E); break;
                    case 3: break;
                    case 4: break;
                    case 5: break;
                    case 6: break;

                    case 7: catI = 1; RSize = items[catI].Count; GUI.RightMenu(items, catI, true); GUI.vt3.UpdateSelect(1, 1, RSize, 5); return false;
                    case 8: catI = 4; RSize = items[catI].Count; GUI.RightMenu(items, catI, true); GUI.vt3.UpdateSelect(1, 1, RSize, 5); return false;
                    case 9: catI = 6; RSize = items[catI].Count; GUI.RightMenu(items, catI, true); GUI.vt3.UpdateSelect(1, 1, RSize, 5); return false;

                    case 10: break;
                    case 11: break;
                    case 12: break;
                }
                return true;
            }
            private static void FACTConfirm()
            {
                if (items[catI].Count > 0)
                {
                    E = Q = !E; GUI.vt3.ClearList(); GUI.vt2.ClearList(); fm.ShowPartFarm("both", E);
                    Product p = items[catI][CS - 1];
                    switch (FACT_ID[catF][LS - 1])
                    {
                        case 7: fm.Build(p); items[catI][CS - 1].amount--; if (items[catI][CS - 1].amount == 0) items[catI].RemoveAt(CS - 1); break; // postaw
                        case 8: break; // posiej
                        case 9: break; // uzyznij
                    }
                }
            }
            private static bool IACTMenager()
            {
                switch (FACT_ID[LogicPlay.catF][CS - 1])
                {
                    case 0: break;
                    case 1: break;
                    case 2: break;
                    case 3: break;
                    case 4: break;
                    case 5: break;
                    case 6: break;

                    case 7: catI = 1; RSize = items[catI].Count; GUI.RightMenu(items, catI); GUI.vt3.UpdateSelect(1, 1, RSize, 5); return false;
                    case 8: catI = 4; RSize = items[catI].Count; GUI.RightMenu(items, catI); GUI.vt3.UpdateSelect(1, 1, RSize, 5); return false;
                    case 9: catI = 6; RSize = items[catI].Count; GUI.RightMenu(items, catI); GUI.vt3.UpdateSelect(1, 1, RSize, 5); return false;

                    case 10: break;
                    case 11: break;
                    case 12: break;
                }
                return true;
            }
            
            public static void Play()
            {
                GUI.Play(save.Name);
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

                catI = CS = PS = LS =  RS = invert = 1;
                LSize = FACT[catF].Count;
                RSize = items[catI].Count;
                Q = E =  OS = false;

                int FarmSize = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(save.GetMap().Length/3)));
                X = Y = FarmSize / 2;
                bool ShiftPressed;
                bool ShowID = false;

                fm = new FieldsMenager(save.GetMap());

                while (openScreen == "Play")
                {
                    if (Console.KeyAvailable)
                    {
                        var cki = Console.ReadKey(true);
                        if (cki.Modifiers == ConsoleModifiers.Shift) ShiftPressed = true; else ShiftPressed = false;
                        if (ShowID) { fm.ShowFieldID();  }

                        if (E || Q) switch (cki.Key) // menu
                        {
                            case ConsoleKey.Escape: Q = E = false; GUI.vt2.ClearList(); GUI.vt3.ClearList(); fm.ShowPartFarm("both", E); break;

                            case ConsoleKey.Q:
                                if (!E) { Q = !Q; GUI.vt2.ClearList(); fm.ShowPartFarm("left", E); }
                                else if (!Q) { Q = !Q; RS = PS; CS = PS = 1; GUI.LeftMenu(IACT[catI],true); GUI.vt2.UpdateSelect(CS, PS, LSize); OS = true; } // czynnosci dla przedmiotu
                                else if (!OS) { E = !E; CS = PS = LS; GUI.vt3.ClearList(); GUI.vt2.UpdateSelect(CS, PS, LSize); OS = true; fm.ShowPartFarm("right", Q); }
                                else if (OS) { Q = E = !Q; GUI.vt2.ClearList(); GUI.vt3.ClearList(); fm.ShowPartFarm("both", E); IACTMenager(); } break; // zatwierdz czynnosc dla przedmiotu

                            case ConsoleKey.E:
                                if (!Q) { E = !E; GUI.vt3.ClearList(); fm.ShowPartFarm("right", Q); }
                                else if (!E) { OS = FACTMenager(); E = !OS; if (E) { LS = PS; CS = PS = 1; } }
                                else if (OS) { Q = !Q; CS = PS = RS; GUI.vt2.ClearList(); GUI.vt3.UpdateSelect(CS, PS, RSize, 5); OS = false; fm.ShowPartFarm("left", E); }
                                else if (!OS) FACTConfirm(); break;

                            case ConsoleKey.W:
                                if (!OS && CS > 1) GUI.vt3.UpdateSelect(--CS, PS, RSize, 5);
                                else if (OS && CS > 1) GUI.vt2.UpdateSelect(--CS, PS, LSize); PS = CS; break;

                            case ConsoleKey.S:
                                if (!OS && CS < RSize) GUI.vt3.UpdateSelect(++CS, PS, RSize, 5);
                                else if (OS && CS < LSize) GUI.vt2.UpdateSelect(++CS, PS, LSize); PS = CS; break;

                            case ConsoleKey.A:
                                if (!OS && ((Q && FACT_ID[catF][LS - 1] == 7 && catI > 1) || (!Q && catI > 1))) {
                                    catI--; CS = PS = 1; LSize = IACT[catI].Count; RSize = items[catI].Count;
                                    GUI.vt3.UpdateItemList(items, catI); GUI.vt3.UpdateSelect(CS, PS, RSize, 5); } break;

                            case ConsoleKey.D:
                                if (!OS && ((Q && FACT_ID[catF][LS - 1] == 7 && catI < 3) || (!Q && catI < items.Length - 1))) {
                                    catI++; RSize = items[catI].Count; LSize = IACT[catI].Count; CS = PS = 1;
                                    GUI.vt3.UpdateItemList(items, catI); GUI.vt3.UpdateSelect(CS, PS, RSize, 5); } break;
                        }
                        else switch (cki.Key) // mapa
                        {
                            case ConsoleKey.Escape:
                                    if (DRAGGED) { fm.Drop(DPOINT); DRAGGED = false; }
                                    fm.ClearFarm(); openScreen = "Escape"; save.SetMap(fm.GetMap()); S.Play("K2"); break;

                            case ConsoleKey.Q:
                                if (DRAGGED) { fm.Drop(DPOINT); DRAGGED = false; } else {
                                    Q = true; CS = PS = 1; catF = fm.GetCategory(); LSize = FACT[catF].Count;
                                    GUI.LeftMenu(FACT[catF]); GUI.vt2.UpdateSelect(CS, PS, LSize); OS = true; } break;

                            case ConsoleKey.E:
                                if (DRAGGED) { fm.Drop(new Point(X, Y)); DRAGGED = false; } else {
                                    E = true; CS = PS = 1; LSize = IACT[catI].Count;
                                    GUI.RightMenu(items, catI); GUI.vt3.UpdateSelect(CS, PS, RSize, 5); OS = false; } break;

                            case ConsoleKey.W: if (X > 1 && fm.MoveSelect(new Point(X, Y), new Point(X - 1, Y), ShiftPressed)) X--; break;
                            case ConsoleKey.A: if (Y > 1 && fm.MoveSelect(new Point(X, Y), new Point(X, Y - 1), ShiftPressed)) Y--; break;
                            case ConsoleKey.S: if (X < FarmSize && fm.MoveSelect(new Point(X, Y), new Point(X + 1, Y), ShiftPressed)) X++; break;
                            case ConsoleKey.D: if (Y < FarmSize && fm.MoveSelect(new Point(X, Y), new Point(X, Y + 1), ShiftPressed)) Y++; break;

                            case ConsoleKey.DownArrow: fm.MoveFarm(new Point(0, -1 * invert)); break;
                            case ConsoleKey.UpArrow: fm.MoveFarm(new Point(0, 1 * invert)); break;
                            case ConsoleKey.RightArrow: fm.MoveFarm(new Point(-1 * invert, 0)); break;
                            case ConsoleKey.LeftArrow: fm.MoveFarm(new Point(1 * invert, 0)); break;

                            case ConsoleKey.M: invert *= -1; break;
                            case ConsoleKey.I: ShowID = !ShowID; break;
                        }
                        //Console.SetCursorPosition(2,5); Console.Write("[" + X + "," + Y + "]");
                    }
                    else if((DateTime.Now - Now).TotalMilliseconds >= 50)
                    {
                        PopUp(POPUPID,POPUPTEXT);
                        Now = DateTime.Now;
                    }
                }
                GUI.vt1.ClearList();
            }
        }
    }
}