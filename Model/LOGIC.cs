using FarmConsole.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Model
{
    class LOGIC
    {
        public static long xxx = 0;
        public static long yyy = 0;
        private Save save = new Save();
        public static string openScreen = "Menu";

        public LOGIC()
        {
            while (openScreen != "Close")
            {
                switch (openScreen)
                {
                    case "Escape": Escape(); break;
                    case "Menu": Menu(); break;
                    case "NewGame": NewGame(); break;
                    case "Load": Load(); break;
                    case "Save": Save(); break;
                    case "Options": Options(); break;
                    case "Help": Help(); break;
                    case "Play": Play(); break;
                }
            }
        }
        public void Menu()
        {
            int c1 = 1, c0 = 1, optionsCount = 4; openScreen = "NewGame";
            GUI.Menu(); GUI.vt1.updateList(c1, c0, optionsCount, 2);
            while (c1 > 0)
            {
                c1 = QH.choice_selector(Console.ReadKey().KeyChar, c1, optionsCount);
                switch (c1)
                {
                    case -2: {
                            GUI.vt1.focus(4);
                        char c2 = 'x';
                        while (c2 != 'e' && c2 != 'q')
                        {
                            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
                            c2 = Console.ReadKey().KeyChar;
                            switch (c2) {
                                case 'e': S.play("K3"); GUI.vt1.clearList(); openScreen = "Close"; Console.SetCursorPosition(0,0); return;
                                case 'q': S.play("K2"); GUI.vt1.clearList(); openScreen = "Menu"; GUI.Menu(); break; }
                        }
                        } break;
                    case -1: break;
                    case 1: openScreen = "NewGame"; break;
                    case 2: openScreen = "Load"; break;
                    case 3: openScreen = "Options"; break;
                    case 4: openScreen = "Help"; break;
                }
                if (c1 > 0) { GUI.vt1.updateList(c1, c0, optionsCount, 2); }
                c0 = c1;
            }
            GUI.vt1.clearList();
        }
        public void Escape()
        {
            int selected = 1;
            int optionsCount = 5;
            openScreen = "Escape";
            ConsoleKeyInfo key;
            GUI.Escape(); GUI.vt1.updateList(selected, 1, optionsCount, 2);
            while (openScreen == "Escape")
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Escape: openScreen = "Play"; GUI.vt1.clearList(); break; // kontynuluj
                    case ConsoleKey.Q: openScreen = "Play"; break; // kontynuluj
                    case ConsoleKey.E:
                        switch (selected)
                        {
                            case 1: openScreen = "Play";break; // kontynuluj
                            case 2: openScreen = "Save"; break;
                            case 3: openScreen = "Menu"; save = new Save(); break;
                            case 4: openScreen = "Options"; break;
                            case 5: openScreen = "Help"; break;
                        }
                        break;
                    case ConsoleKey.W: if (selected > 1) { S.play("K1"); selected--; GUI.vt1.updateList(selected, selected+1, optionsCount, 2); } break;
                    case ConsoleKey.S: if (selected < 5) { S.play("K1"); selected++; GUI.vt1.updateList(selected, selected-1, optionsCount, 2); } break;
                }
            }
            GUI.vt1.clearList();
        }
        public void NewGame()
        {
            int c1 = 1, c0 = 1, optionsCount = 2;
            GUI.NewGame(); GUI.vt1.updateList(1, 1, optionsCount, 1);
            while (c1 > 0)
            {
                c1 = QH.choice_selector(Console.ReadKey().KeyChar, c1, optionsCount);
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
            GUI.vt1.clearList();
        }
        public void Load()
        {
            Save[] saves = XF.GetSaves();
            GUI.Load(saves);
            int  y = 0, oc = saves.Length + 1;
            GUI.vt1.updateList(1, 1, oc);
            char c1 = 'x';
            string opis = "E / Rozpocznij Nową Grę";
            while (c1 != 'q' && c1 != 'e')
            {
                c1 = Console.ReadKey().KeyChar;
                Console.Write("\b ");
                switch (c1)
                {
                    case 'q': S.play("K3"); openScreen = "Menu"; break;
                    case 'd': if (y != 0){
                            S.play("K2");
                            GUI.vt1.focus(6,7);
                            char c2 = 'x';
                            while(c2 != 'd' && c2 != 'q')
                            {
                                c2 = Console.ReadKey().KeyChar;
                                Console.Write("\b ");
                                switch (c2)
                                {
                                    case 'd':
                                        S.play("K2");
                                        GUI.vt1.clearList();
                                        saves[y-1].delete();
                                        saves = XF.GetSaves();
                                        GUI.Load(saves);
                                        y = 0; oc = saves.Length + 1;
                                        GUI.vt1.updateList(1, 1, oc);
                                        break;
                                    case 'q':
                                        S.play("K3");
                                        GUI.vt1.showability(0, 0, true);
                                        GUI.vt1.updateList(y + 1, y + 1, oc);
                                        break;
                                }
                            }
                            GUI.vt1.showability(6, 0, false);
                        } break; // usun save
                    case 'e': if (y == 0) { S.play("K2"); openScreen = "NewGame"; }
                            else { save.load(y); openScreen = "Play"; } break;
                    case 'w': if (y > 0) { S.play("K1"); GUI.vt1.updateList(y, y + 1, oc); y--; } break;
                    case 's': if (y < oc - 1) { S.play("K1"); GUI.vt1.updateList(y + 2, y + 1, oc); y++; } break;
                }
                //QH.INFO(-1, "","","","");
                if (y > 0)
                {
                    opis =
                    ". . ." +
                    " ------------------------------------" +
                    " -- Nazwa Gracza - " + saves[y - 1].name.ToString() + (" ").PadRight(18 - saves[y - 1].name.ToString().Length, '-') +
                    " ------------------------------------" +
                    " -- Osiągniety Poziom - " + saves[y - 1].lvl.ToString() + (" ").PadRight(13 - saves[y - 1].lvl.ToString().Length, '-') +
                    " ------------------------------------" +
                    " -- Posiadany Majątek - " + saves[y - 1].wallet.ToString() + (" ").PadRight(13 - saves[y - 1].wallet.ToString().Length, '-') +
                    " ------------------------------------" +
                    " -- Ostatni Zapis - " + saves[y - 1].lastplay.ToString() + " --" +
                    " ------------------------------------ . . .";
                    GUI.vt1.updateBox(4, 1, opis);
                    GUI.vt1.showability(4, 3, true);
                }
                else
                {
                    GUI.vt1.updateBox(4, 1, "E / Rozpocznij Nową Grę");
                    GUI.vt1.showability(4, 3, false);
                }
            }
            GUI.vt1.clearList();
        }
        public void Save()
        {
            Save[] saves = XF.GetSaves();
            GUI.Save(saves);
            int y = 0, oc = saves.Length + 1;
            GUI.vt1.updateList(1, 1, oc);
            char c1 = 'x';
            string opis = "E / Utwórz Nowy Zapis";
            while (c1 != 'q' && c1 != 'e')
            {
                c1 = Console.ReadKey().KeyChar;
                Console.Write("\b ");
                switch (c1)
                {
                    case 'q': S.play("K3"); openScreen = "Escape"; break;
                    case 'e': S.play("K2"); save.update(y); openScreen = "Escape"; break;
                    case 'w': if (y > 0) { S.play("K1"); GUI.vt1.updateList(y, y + 1, oc); y--; } break;
                    case 's': if (y < oc - 1) { S.play("K1"); GUI.vt1.updateList(y + 2, y + 1, oc); y++; } break;
                }
                if (y > 0)
                {
                    opis =
                    ". . ." +
                    " ------------------------------------" +
                    " -- Nazwa Gracza - " + saves[y - 1].name.ToString() + (" ").PadRight(18 - saves[y - 1].name.ToString().Length, '-') +
                    " ------------------------------------" +
                    " -- Osiągniety Poziom - " + saves[y - 1].lvl.ToString() + (" ").PadRight(13 - saves[y - 1].lvl.ToString().Length, '-') +
                    " ------------------------------------" +
                    " -- Posiadany Majątek - " + saves[y - 1].wallet.ToString() + (" ").PadRight(13 - saves[y - 1].wallet.ToString().Length, '-') +
                    " ------------------------------------" +
                    " -- Ostatni Zapis - " + saves[y - 1].lastplay.ToString() + " --" +
                    " ------------------------------------ . . .";
                    GUI.vt1.updateBox(4, 1, opis);
                    GUI.vt1.showability(4, 3, true);
                }
                else
                {
                    GUI.vt1.updateBox(4, 1, "E / Utwórz Nowy Zapis");
                    GUI.vt1.showability(4, 3, false);
                }
            }
            GUI.vt1.clearList();
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
                c1 = Console.ReadKey().KeyChar;
                Console.Write("\b ");
                switch (c1)
                {
                    case 'q': S.play("K3"); openScreen = "Menu"; break;
                    case 'e': S.play("K2"); GUI.vt1.clearList(); Console.Clear(); OPTIONS.saveOptions(opt);
                              ViewTools vt = new ViewTools(); vt.foot(XF.GetString(1)); break;
                    case 'w': if (y > 0 ) { S.play("K1"); GUI.vt1.updateList(y, y + 1, oc, 2); y--; } break;
                    case 's': if (y < oc-1) { S.play("K1"); GUI.vt1.updateList(y + 2, y + 1, oc, 2); y++; } break;
                    case 'a': if (opt[y] > 0) { S.play("K3"); opt[y]--; GUI.vt1.updateSlider(3, y+1, r, opt[y]); } break;
                    case 'd': if (opt[y] < r) { S.play("K2"); opt[y]++; GUI.vt1.updateSlider(3, y+1, r, opt[y]); } break;
                }
                //QH.INFO(0, "y: " + y, "options[y]: "+ opt[y], "", "");
            }GUI.vt1.clearList();
        }
        public void Help()
        {
            GUI.Help();
            char choice;
            choice = Console.ReadKey().KeyChar;
            GUI.vt1.clearList();
            openScreen = "Menu";
            S.play("K3");
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
                c1 = QH.choice_side_menu(Console.ReadKey().KeyChar, c1, LSize, RSize);
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
                                    GUI.vt2.clearList();
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
                                    GUI.vt2.clearList();
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
                                    GUI.vt3.clearList();
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
                                    GUI.vt3.clearList();
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
            GUI.vt1.clearList();
        }
    }
}
