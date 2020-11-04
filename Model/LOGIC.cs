using FarmConsole.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Model
{
    class LOGIC
    {
        public static string openScreen = "Play";

        public LOGIC()
        {
            while(openScreen != "Close")
            {
                switch (openScreen)
                {
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
            GUI.Menu();
            int c1 = 1, c0 = 1; openScreen = "NewGame";
            while (c1 > 0)
            {
                c1 = QH.choice_selector(Console.ReadKey().KeyChar, c1, 4);
                Console.Write("\b");
                switch (c1)
                {
                    case -2: openScreen = "Close"; break;
                    case -1: break;
                    case 1: GUI.vt1.setSO(2, c1 - 1, c0 - 1); openScreen = "NewGame"; break;
                    case 2: GUI.vt1.setSO(2, c1 - 1, c0 - 1); openScreen = "Load"; break;
                    case 3: GUI.vt1.setSO(2, c1 - 1, c0 - 1); openScreen = "Options"; break;
                    case 4: GUI.vt1.setSO(2, c1 - 1, c0 - 1); openScreen = "Help"; break;
                }
                c0 = c1;
            }
            GUI.Clear(GUI.vt1);
        }
        public void NewGame()
        {
            GUI.NewGame();
            int c1 = 1, c0 = 1;
            while (c1 > 0)
            {
                c1 = QH.choice_selector(Console.ReadKey().KeyChar, c1, 2);
                Console.Write("\b");
                switch (c1)
                {
                    case -2: openScreen = "Menu"; break;
                    case -1: openScreen = "Play"; break;
                    case 1: GUI.vt1.setSO(1, c1 - 1, c0 - 1); break;
                    case 2: GUI.vt1.setSO(1, c1 - 1, c0 - 1); break;
                }
                c0 = c1;
            }
            GUI.Clear(GUI.vt1);
        }
        public void Load()
        {
            GUI.Load();
            char choice;
            choice = Console.ReadKey().KeyChar;
            openScreen = "Menu";
            GUI.Clear(GUI.vt1);
        }
        public void Options()
        {
            int oc = OPTIONS.getOptionsCount();
            int[] opt = OPTIONS.getOptionsView();
            GUI.Options();
            char c1 = 'x';
            int y = 0, r = 6; // r: range of slides
            while (c1 != 'q' && c1 != 'e')
            {
                c1 = Console.ReadKey().KeyChar;
                Console.Write("\b");
                switch (c1)
                {
                    case 'q': openScreen = "Menu"; break;
                    case 'e': GUI.Clear(GUI.vt1); Console.Clear(); OPTIONS.saveOptions(opt);
                              ViewTools vt = new ViewTools(); vt.foot(XF.findString(1)); break;
                    case 'w': if (y > 0 )   { GUI.vt1.setSO(2, y - 1, y); y--; } break;
                    case 's': if (y < oc-1) { GUI.vt1.setSO(2, y + 1, y); y++; } break;
                    case 'a': if (opt[y] > 0) { opt[y]--; GUI.vt1.setSL(3, y, r, opt[y]); } break;
                    case 'd': if (opt[y] < r) { opt[y]++; GUI.vt1.setSL(3, y, r, opt[y]); } break;
                }
                //QH.INFO(0, "y: " + y, "options[y]: "+ options[y], "", "");
            }GUI.Clear(GUI.vt1);
        }
        public void Help()
        {
            GUI.Help();
            char choice;
            choice = Console.ReadKey().KeyChar;
            openScreen = "Menu";
            GUI.Clear(GUI.vt1);
        }
        public void Play()
        {
            GUI.Play();
            int c1 = 1, c0 = 1;
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
            while (c1 != -5)
            {
                Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
                c1 = QH.choice_side_menu(Console.ReadKey().KeyChar, c1, LSize, RSize);
                Console.Write("\b");
                if (c1 < 0) switch (c1)
                    {
                        case -5: openScreen = "Menu"; break;
                        case -4: break;
                        case -3: break;
                        case -2:
                            {
                                if (!pressQ)
                                {
                                    if (pressE) GUI.vt3.setSO(3, c0 - LSize);
                                    GUI.LeftMenu(left_list); pressQ = true; c1 = 1;
                                }
                                else if (pressE && (c0 > LSize && c0 < LSize + RSize + 1))
                                {
                                    GUI.vt3.setSO(3, c0 - LSize); c0 = c1 = 1; GUI.vt2.setSO(3, c1);
                                }
                                else
                                {
                                    GUI.Clear(GUI.vt2); pressQ = false;
                                    if (pressE) { c1 = LSize + 1; GUI.vt3.setSO(3, c1 - LSize); }
                                }
                            }
                            break;
                        case -1:
                            {
                                if (!pressE)
                                {
                                    if (pressQ) GUI.vt2.setSO(3, c0);
                                    GUI.RightMenu(right_list); pressE = true; c1 = LSize + 1;
                                }
                                else if (pressQ && (c0 > 0 && c0 < LSize + 1))
                                {
                                    GUI.vt2.setSO(3, c0); c0 = c1 = LSize + 1; GUI.vt3.setSO(3, c1 - LSize);
                                }
                                else
                                {
                                    GUI.Clear(GUI.vt3); pressE = false;
                                    if (pressQ) { c1 = 1; GUI.vt2.setSO(3, c1); }
                                }
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
