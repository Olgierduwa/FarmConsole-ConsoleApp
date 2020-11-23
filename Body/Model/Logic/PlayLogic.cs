using FarmConsole.Body.Model.Helpers;
using FarmConsole.Body.Sounds;
using FarmConsole.Body.View.Components;
using FarmConsole.Body.View.GUI;
using System;
using System.Drawing;

namespace FarmConsole.Body.Model.Logic
{
    public partial class Logic
    {
        public void Play()
        {
            GUI.Play();
            bool pressQ = false, pressE = false;
            string[] left_list = new string[]
            {
                "Jeden     ",
                "Dwa   .   ",
                "Trzy  ..  ",
                "Cztery... ",
                "piec  ....",
                "szesc  ...",
                "siedem  ..",
                "osiem    .",
                "dziewiec  "
            };
            string[] right_list = new string[]
            {
                "Jeden Jede",
                "Dwa   Dwa ",
                "Trzy  Trzy",
                "Cztery Elo",
                "Trzy  Trzy",
                "Cztery Elo"
            };
            int LSize = left_list.Length;
            int RSize = right_list.Length;
            int selected = 1, last_selected = 1, lest_selected = 1, right_selected = LSize + 1, lastPress = 0;

            int FarmSize = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(save.GetMap().Length)));
            int X = FarmSize/2, Y = FarmSize/2;
            bool ShiftPressed;

            FieldsMenager fm = new FieldsMenager(save.GetMap());

            while (openScreen == "Play")
            {
                QH.INFO(-3, "X: " + X, "Y: " + Y, "", "");
                var cki = Console.ReadKey(true);
                if (cki.Modifiers == ConsoleModifiers.Shift) ShiftPressed = true; else ShiftPressed = false;

                switch (cki.Key)
                {
                    case ConsoleKey.Escape: fm.ClearFarm(); openScreen = "Escape"; save.SetMap(fm.GetMap()); S.Play("K2"); break;
                    case ConsoleKey.Q:
                        {
                            if (!pressQ && !pressE) // pokazywanie Q //
                            {
                                pressQ = true;
                                selected = last_selected = lest_selected;
                                GUI.LeftMenu(left_list);
                                GUI.vt2.UpdateList(selected, last_selected, LSize);
                            }
                            else if (pressQ && !pressE) // chowanie Q //
                            {
                                pressQ = false;
                                lest_selected = 1;
                                selected = last_selected = 1;
                                GUI.vt2.ClearList();
                            }
                            else if (!pressQ && pressE) // przelaczanie sie na Q schowane //
                            {
                                pressQ = true;
                                right_selected = last_selected;
                                selected = last_selected = lest_selected;
                                GUI.LeftMenu(left_list);
                                GUI.vt2.UpdateList(selected, last_selected, LSize);
                            }
                            else if (pressQ && pressE && lastPress == -2) // chowanie Q i przelaczanie sie na E //
                            {
                                pressQ = false;
                                lest_selected = 1;
                                selected = last_selected = right_selected;
                                GUI.vt2.ClearList();
                                GUI.vt3.UpdateList(selected - LSize, last_selected - LSize, RSize);
                            }
                            else if (pressQ && pressE) // przelaczanie sie na Q pokazane //
                            {
                                right_selected = last_selected;
                                selected = last_selected = lest_selected;
                                GUI.vt2.UpdateList(selected, last_selected, LSize);
                            }
                            lastPress = -2;
                        }
                        break;
                    case ConsoleKey.E:
                        {
                            if (!pressQ && !pressE) // pokazywanie E //
                            {
                                pressE = true;
                                selected = last_selected = LSize + 1;
                                GUI.RightMenu(right_list);
                                GUI.vt3.UpdateList(selected - LSize, last_selected - LSize, RSize);
                            }
                            else if (!pressQ && pressE) // chowanie E //
                            {
                                pressE = false;
                                right_selected = LSize + 1;
                                selected = last_selected = 1;
                                GUI.vt3.ClearList();
                            }
                            else if (pressQ && !pressE) // przelaczanie sie na E schowane //
                            {
                                pressE = true;
                                lest_selected = last_selected;
                                selected = last_selected = LSize + 1;
                                GUI.RightMenu(right_list);
                                GUI.vt3.UpdateList(selected - LSize, last_selected - LSize, RSize);
                            }
                            else if (pressQ && pressE && lastPress == -1) // chowanie E i przelaczanie sie na Q //
                            {
                                pressE = false;
                                right_selected = LSize + 1;
                                selected = last_selected = lest_selected;
                                GUI.vt3.ClearList();
                                GUI.vt2.UpdateList(selected, last_selected, LSize);
                            }
                            else if (pressQ && pressE) // przelaczanie sie na E pokazane //
                            {
                                lest_selected = last_selected;
                                selected = last_selected = right_selected;
                                GUI.vt3.UpdateList(selected - LSize, last_selected - LSize, RSize);
                            }
                            lastPress = -1;
                        }
                        break;

                    case ConsoleKey.W: if (X > 1) fm.MoveSelect(new Point(X, Y), new Point(--X, Y), ShiftPressed); break;
                    case ConsoleKey.A: if (Y > 1) fm.MoveSelect(new Point(X, Y), new Point(X, --Y), ShiftPressed); break;
                    case ConsoleKey.S: if (X < FarmSize) fm.MoveSelect(new Point(X, Y), new Point(++X, Y), ShiftPressed); break;
                    case ConsoleKey.D: if (Y < FarmSize) fm.MoveSelect(new Point(X, Y), new Point(X, ++Y), ShiftPressed); break;

                    case ConsoleKey.UpArrow: fm.MoveFarm(new Point(0,-1)); break;
                    case ConsoleKey.DownArrow: fm.MoveFarm(new Point(0,1)); break;
                    case ConsoleKey.LeftArrow: fm.MoveFarm(new Point(-1,0)); break;
                    case ConsoleKey.RightArrow: fm.MoveFarm(new Point(1,0)); break;
                }














                /*selected = QH.Choice_side_menu(Console.ReadKey().KeyChar, selected, LSize, RSize);
                if (selected < 0) switch (selected)
                    {
                        case -5: GUI.vt2.ClearList(); GUI.vt3.ClearList(); openScreen = "Escape"; break;
                        case -4: break;
                        case -3: break;
                        case -2:
                            {
                                if (!pressQ && !pressE) // pokazywanie Q //
                                {
                                    pressQ = true;
                                    selected = last_selected = lest_selected;
                                    GUI.LeftMenu(left_list);
                                    GUI.vt2.UpdateList(selected, last_selected, LSize);
                                }
                                else if (pressQ && !pressE) // chowanie Q //
                                {
                                    pressQ = false;
                                    lest_selected = 1;
                                    selected = last_selected = 1;
                                    GUI.vt2.ClearList();
                                }
                                else if (!pressQ && pressE) // przelaczanie sie na Q schowane //
                                {
                                    pressQ = true;
                                    right_selected = last_selected;
                                    selected = last_selected = lest_selected;
                                    GUI.LeftMenu(left_list);
                                    GUI.vt2.UpdateList(selected, last_selected, LSize);
                                }
                                else if (pressQ && pressE && lastPress == -2) // chowanie Q i przelaczanie sie na E //
                                {
                                    pressQ = false;
                                    lest_selected = 1;
                                    selected = last_selected = right_selected;
                                    GUI.vt2.ClearList();
                                    GUI.vt3.UpdateList(selected - LSize, last_selected - LSize, RSize);
                                }
                                else if (pressQ && pressE) // przelaczanie sie na Q pokazane //
                                {
                                    right_selected = last_selected;
                                    selected = last_selected = lest_selected;
                                    GUI.vt2.UpdateList(selected, last_selected, LSize);
                                }
                                lastPress = -2;
                            }
                            break;
                        case -1:
                            {
                                if (!pressQ && !pressE) // pokazywanie E //
                                {
                                    pressE = true;
                                    selected = last_selected = LSize + 1;
                                    GUI.RightMenu(right_list);
                                    GUI.vt3.UpdateList(selected - LSize, last_selected - LSize, RSize);
                                }
                                else if (!pressQ && pressE) // chowanie E //
                                {
                                    pressE = false;
                                    right_selected = LSize + 1;
                                    selected = last_selected = 1;
                                    GUI.vt3.ClearList();
                                }
                                else if (pressQ && !pressE) // przelaczanie sie na E schowane //
                                {
                                    pressE = true;
                                    lest_selected = last_selected;
                                    selected = last_selected = LSize + 1;
                                    GUI.RightMenu(right_list);
                                    GUI.vt3.UpdateList(selected - LSize, last_selected - LSize, RSize);
                                }
                                else if (pressQ && pressE && lastPress == -1) // chowanie E i przelaczanie sie na Q //
                                {
                                    pressE = false;
                                    right_selected = LSize + 1;
                                    selected = last_selected = lest_selected;
                                    GUI.vt3.ClearList();
                                    GUI.vt2.UpdateList(selected, last_selected, LSize);
                                }
                                else if (pressQ && pressE) // przelaczanie sie na E pokazane //
                                {
                                    lest_selected = last_selected;
                                    selected = last_selected = right_selected;
                                    GUI.vt3.UpdateList(selected - LSize, last_selected - LSize, RSize);
                                }
                                lastPress = -1;
                            }
                            break;

                        case 1: break;
                        case 2: break;
                        case 3: break;
                        case 4: break;
                        case 5: break;

                        case 6: break;
                        case 7: break;
                        case 8: break;
                        case 9: break;
                    }
                else if (selected > 0 && selected < LSize + 1 && pressQ) { GUI.vt2.UpdateList(selected, last_selected, LSize); }
                else if (selected > LSize && selected < LSize + RSize + 1 && pressE) { GUI.vt3.UpdateList(selected - LSize, last_selected - LSize, RSize); }

                //QH.INFO(0, "c0: " + c0, "c1: " + c1, "Q: " + pressQ, "E: " + pressE);

                last_selected = selected;
                */
            }
            GUI.vt1.ClearList();
        }
    }
}
