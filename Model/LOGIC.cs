using FarmConsole.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Model
{
    class LOGIC
    {
        public static string openScreen = "Menu";

        public LOGIC()
        {
            while(openScreen != "Close")
            {
                switch (openScreen)
                {
                    case "Escape": Escape(); break;
                    case "Menu": Menu(); break;
                    case "NewGame": NewGame(); break;
                    case "Load": Load(); break;
                    case "Options": Options(); break;
                    case "Help": Help(); break;
                    case "Play": Play(); break;
                }
            }
        }
        public void Menu()
        {
            int c1 = 1, c0 = 1, optionsCount = 4; openScreen = "NewGame";
            GUI.Menu(); GUI.vt1.updateList(1, 1, optionsCount, 2);
            while (c1 > 0)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
                c1 = QH.choice_selector(Console.ReadKey().KeyChar, c1, optionsCount);
                Console.Write("\b");
                switch (c1)
                {
                    case -2: GUI.Clear(GUI.vt1); openScreen = "Close"; break;
                    case -1: break;
                    case 1:openScreen = "NewGame"; break;
                    case 2:openScreen = "Load"; break;
                    case 3:openScreen = "Options"; break;
                    case 4:openScreen = "Help"; break;
                }
                if (c1 > 0) GUI.vt1.updateList(c1, c0, optionsCount, 2);
                c0 = c1;
            }
            GUI.Clear(GUI.vt1);
        }
        public void Escape()
        {
            int c1 = 1, c0 = 1, optionsCount = 5; openScreen = "NewGame";
            GUI.Escape(); GUI.vt1.updateList(1, 1, optionsCount, 2);
            while (c1 > 0)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
                c1 = QH.choice_selector(Console.ReadKey().KeyChar, c1, optionsCount);
                Console.Write("\b");
                switch (c1)
                {
                    case -2: break;
                    case -1: break;
                    case 1: break;
                    case 2: openScreen = "Save"; break;
                    case 3: openScreen = "Menu"; break;
                    case 4: openScreen = "Options"; break;
                    case 5: openScreen = "Help"; break;
                }
                if (c1 > 0) GUI.vt1.updateList(c1, c0, optionsCount, 2);
                c0 = c1;
            }
            GUI.Clear(GUI.vt1);
        }

        public void NewGame()
        {
            int c1 = 1, c0 = 1, optionsCount = 2;
            GUI.NewGame(); GUI.vt1.updateList(1, 1, optionsCount, 1);
            while (c1 > 0)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
                c1 = QH.choice_selector(Console.ReadKey().KeyChar, c1, optionsCount);
                Console.Write("\b");
                switch (c1)
                {
                    case -2: openScreen = "Menu"; break;
                    case -1: openScreen = "Play"; break;
                    case 1: break;
                    case 2: break;
                }
                if (c1 > 0) GUI.vt1.updateList(c1, c0, optionsCount, 1);
                c0 = c1;
            }
            GUI.Clear(GUI.vt1);
        }
        public void Load()
        {
            GUI.Load();
            char choice;
            choice = Console.ReadKey().KeyChar;
            GUI.Clear(GUI.vt1);
            openScreen = "Menu";
        }
        public void Options()
        {
            int oc = OPTIONS.getOptionsCount();
            int[] opt = OPTIONS.getOptionsView();
            GUI.Options();
            char c1 = 'x';
            int y = 0, r = 6; // r: range of slides
            GUI.vt1.updateList(1, 1, oc, 2);
            while (c1 != 'q' && c1 != 'e')
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
                c1 = Console.ReadKey().KeyChar;
                Console.Write("\b");
                switch (c1)
                {
                    case 'q': openScreen = "Menu"; break;
                    case 'e': GUI.Clear(GUI.vt1); Console.Clear(); OPTIONS.saveOptions(opt);
                              ViewTools vt = new ViewTools(); vt.foot(XF.findString(1)); break;
                    case 'w': if (y > 0 )   { GUI.vt1.updateList(y, y + 1, oc, 2); y--; } break;
                    case 's': if (y < oc-1) { GUI.vt1.updateList(y + 2, y + 1, oc, 2); y++; } break;
                    case 'a': if (opt[y] > 0) { opt[y]--; GUI.vt1.setSL(3, y+1, r, opt[y]); } break;
                    case 'd': if (opt[y] < r) { opt[y]++; GUI.vt1.setSL(3, y+1, r, opt[y]); } break;
                }
                //QH.INFO(0, "y: " + y, "options[y]: "+ opt[y], "", "");
            }GUI.Clear(GUI.vt1);
        }
        public void Help()
        {
            GUI.Help();
            char choice;
            choice = Console.ReadKey().KeyChar;
            GUI.Clear(GUI.vt1);
            openScreen = "Menu";
        }
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
            int c1 = 1, c0 = 1, cL = 1, cR = LSize + 1, lastPress = 0;
            while (c1 != -5)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
                c1 = QH.choice_side_menu(Console.ReadKey().KeyChar, c1, LSize, RSize);
                Console.Write("\b");
                if (c1 < 0) switch (c1)
                    {
                        case -5: openScreen = "Escape"; break;
                        case -4: break;
                        case -3: break;
                        case -2:
                            {
                                if (!pressQ && !pressE) // pokazywanie Q //
                                {
                                    pressQ = true;
                                    c1 = c0 = cL;
                                    GUI.LeftMenu(left_list);
                                    GUI.vt2.updateList(c1, c0, LSize);
                                }
                                else if (pressQ && !pressE) // chowanie Q //
                                {
                                    pressQ = false;
                                    cL = 1;
                                    c1 = c0 = 1;
                                    GUI.Clear(GUI.vt2);
                                }
                                else if (!pressQ && pressE) // przelaczanie sie na Q schowane //
                                {
                                    pressQ = true;
                                    cR = c0;
                                    c1 = c0 = cL;
                                    GUI.LeftMenu(left_list);
                                    GUI.vt2.updateList(c1, c0, LSize);
                                }
                                else if (pressQ && pressE && lastPress == -2) // chowanie Q i przelaczanie sie na E //
                                {
                                    pressQ = false;
                                    cL = 1;
                                    c1 = c0 = cR;
                                    GUI.Clear(GUI.vt2);
                                    GUI.vt3.updateList(c1 - LSize, c0 - LSize, RSize);
                                }
                                else if (pressQ && pressE) // przelaczanie sie na Q pokazane //
                                {
                                    cR = c0;
                                    c1 = c0 = cL;
                                    GUI.vt2.updateList(c1, c0, LSize);
                                }
                                lastPress = -2;
                            }
                            break;
                        case -1:
                            {
                                if (!pressQ && !pressE) // pokazywanie E //
                                {
                                    pressE = true;
                                    c1 = c0 = LSize + 1;
                                    GUI.RightMenu(right_list);
                                    GUI.vt3.updateList(c1 - LSize, c0 - LSize, RSize);
                                }
                                else if (!pressQ && pressE) // chowanie E //
                                {
                                    pressE = false;
                                    cR = LSize + 1;
                                    c1 = c0 = 1;
                                    GUI.Clear(GUI.vt3);
                                }
                                else if (pressQ && !pressE) // przelaczanie sie na E schowane //
                                {
                                    pressE = true;
                                    cL = c0;
                                    c1 = c0 = LSize + 1;
                                    GUI.RightMenu(right_list);
                                    GUI.vt3.updateList(c1 - LSize, c0 - LSize, RSize);
                                }
                                else if (pressQ && pressE && lastPress == -1) // chowanie E i przelaczanie sie na Q //
                                {
                                    pressE = false;
                                    cR = LSize + 1;
                                    c1 = c0 = cL;
                                    GUI.Clear(GUI.vt3);
                                    GUI.vt2.updateList(c1, c0, LSize);
                                }
                                else if (pressQ && pressE) // przelaczanie sie na E pokazane //
                                {
                                    cL = c0;
                                    c1 = c0 = cR;
                                    GUI.vt3.updateList(c1 - LSize, c0 - LSize, RSize);
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
                else if (c1 > 0 && c1 < LSize + 1 && pressQ) { GUI.vt2.updateList(c1, c0, LSize); }
                else if (c1 > LSize && c1 < LSize + RSize + 1 && pressE) { GUI.vt3.updateList(c1 - LSize, c0 - LSize, RSize); }

                //QH.INFO(0, "c0: " + c0, "c1: " + c1, "Q: " + pressQ, "E: " + pressE);

                c0 = c1;
            }
            GUI.Clear(GUI.vt1);
        }
    }
}
