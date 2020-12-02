using FarmConsole.Body.Model.Helpers;
using FarmConsole.Body.Model.Objects;
using FarmConsole.Body.Sounds;
using FarmConsole.Body.View.GUI;
using System;

namespace FarmConsole.Body.Model.Logic
{
    public partial class Logic
    {
        public static long xxx = 0;
        public static long yyy = 0;
        private Save save = new Save();
        public static string openScreen = "Menu";
        public static string lastScreen = "Menu";

        public bool? Warning(string type)
        {
            S.Play("K2");
            int focus = 0;
            switch (type)
            {
                case "exit": focus = 4; break;
                case "menu": focus = 3; break;
                case "delete": focus = 4; break;
                case "update": focus = 4; break;
            }
            GUI.vt1.Focus(focus);
            bool? choice = null;
            while (choice == null)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Q: S.Play("K3"); choice = false; break;
                    case ConsoleKey.E: S.Play("K2"); choice = true; break;
                }
            }
            GUI.vt1.Showability(focus, 0, false);
            GUI.vt1.PrintList();
            return choice;
        }

        public Logic()
        {
            while (openScreen != "Close")
            {
                switch (openScreen)
                {
                    case "NewGame": NewGame(); break;
                    case "Options": Options(); break;
                    case "Escape": Escape(); break;
                    case "Menu": Menu(); break;
                    case "Load": Load(); break;
                    case "Save": Save(); break;
                    case "Help": Help(); break;
                    case "Play": Play(); break;
                }
            }
        }
        public void Menu()
        {
            int selected = 1, selCount = 4, selStart = 1;
            GUI.Menu(); GUI.vt1.UpdateList(selected, selected, selCount, 2);
            while (openScreen == "Menu")
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GUI.vt1.UpdateList(selected, selected+1, selCount, 2); } break;
                    case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GUI.vt1.UpdateList(selected, selected-1, selCount, 2); } break;
                    case ConsoleKey.Escape:
                    case ConsoleKey.Q: if (Warning("exit") == true) openScreen = "Close"; break;
                    case ConsoleKey.E: S.Play("K2"); switch (selected)
                    {
                        case 1: openScreen = "NewGame"; break;
                        case 2: openScreen = "Load"; break;
                        case 3: openScreen = "Options"; break;
                        case 4: openScreen = "Help"; break;
                    }
                    break;
                }
            }
            GUI.vt1.ClearList();
        }
        public void Escape()
        {
            lastScreen = "Escape";
            int selected = 1, selCount = 5, selStart = 1;
            GUI.Escape(); GUI.vt1.UpdateList(selected, selected, selCount, 2);
            while (openScreen == "Escape")
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GUI.vt1.UpdateList(selected, selected + 1, selCount, 2); } break;
                    case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GUI.vt1.UpdateList(selected, selected - 1, selCount, 2); } break;
                    case ConsoleKey.Escape: S.Play("K3"); openScreen = "Play"; GUI.vt1.ClearList(); break;
                    case ConsoleKey.Q: S.Play("K3"); openScreen = "Play"; break;
                    case ConsoleKey.E: switch (selected)
                    {
                        case 1: S.Play("K3"); openScreen = "Play";break;
                        case 2: S.Play("K2"); openScreen = "Save"; break;
                        case 3: if(Warning("menu") == true) { openScreen = lastScreen = "Menu"; save = new Save(); } break;
                        case 4: S.Play("K2"); openScreen = "Options"; break;
                        case 5: S.Play("K2"); openScreen = "Help"; break;
                    }
                    break;
                }
            }
            GUI.vt1.ClearList();
        }
        public void NewGame()
        {
            int selected = 1, selCount = 2, selStart = 1;
            GUI.NewGame(); GUI.vt1.UpdateList(selected, selected, selCount, 1);
            while (openScreen == "NewGame")
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GUI.vt1.UpdateList(selected, selected + 1, selCount, 1); } break;
                    case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GUI.vt1.UpdateList(selected, selected - 1, selCount, 1); } break;
                    case ConsoleKey.Escape:
                    case ConsoleKey.Q: S.Play("K3"); openScreen = "Menu"; break;
                    case ConsoleKey.E: switch (selected)
                    {
                        case 1: S.Play("K2"); openScreen = "Play"; save = new Save("Locha"); break;
                        case 2: S.Play("K2"); openScreen = "Play"; save = new Save("Turbo Kox"); break;
                    }
                    break;
                }
            }
            GUI.vt1.ClearList();
        }
        public void Load()
        {
            Save[] saves = XF.GetSaves();
            int selected = 1, selCount = saves.Length + 1, selStart = 1;
            GUI.Load(saves); GUI.vt1.UpdateList(selected, selected, selCount, 2, 14);
            while (openScreen == "Load")
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GUI.vt1.UpdateList(selected, selected + 1, selCount, 2, 14); } break;
                    case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GUI.vt1.UpdateList(selected, selected - 1, selCount, 2, 14); } break;
                    case ConsoleKey.Escape:
                    case ConsoleKey.Q: S.Play("K3"); openScreen = "Menu"; break;
                    case ConsoleKey.E:  S.Play("K2"); switch (selected) { case 1: openScreen = "NewGame"; break; default: save.load(selected - 1); openScreen = "Play"; break; } break;
                    case ConsoleKey.D: if (selected > 1 && Warning("delete") == true)
                    {
                        saves[selected - 2].delete();
                        saves = XF.GetSaves();
                        selected = 1;
                        selCount = saves.Length + 1;
                        GUI.vt1.ClearList();
                        GUI.Load(saves);
                        GUI.vt1.UpdateList(1, 1, selCount, 2, 14);
                    }
                    break;
                }
                if (selected > 1)
                {
                    GUI.vt1.UpdateBox(3, 1, ". . ." +
                    " ---------------------------------- Nazwa Gracza - " + saves[selected - 2].name.ToString() +
                    " ---------------------------------- Osiągniety Poziom - " + saves[selected - 2].lvl.ToString() +
                    " ---------------------------------- Posiadany Majątek - " + saves[selected - 2].wallet.ToString() +
                    " ---------------------------------- Ostatni Zapis - " + saves[selected - 2].lastplay.ToString() + " ---------------------------------- . . .");
                    GUI.vt1.Showability(3, 3, true);
                }
                else { GUI.vt1.UpdateBox(3, 1, "E / Rozpocznij Nową Grę"); GUI.vt1.Showability(3, 3, false); }
            }
            GUI.vt1.ClearList();
        }
        public void Save()
        {
            Save[] saves = XF.GetSaves();
            int selected = 1, selCount = saves.Length + 1, selStart = 1;
            GUI.Save(saves); GUI.vt1.UpdateList(selected, selected, selCount, 2, 14);
            while (openScreen == "Save")
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GUI.vt1.UpdateList(selected, selected + 1, selCount, 2, 14); } break;
                    case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GUI.vt1.UpdateList(selected, selected - 1, selCount, 2, 14); } break;
                    case ConsoleKey.Escape:
                    case ConsoleKey.Q: S.Play("K3"); openScreen = "Escape"; break;
                    case ConsoleKey.E:  if (selected == 1 || Warning("update") == true)
                    {
                        save.update(selected - 1);
                        saves = XF.GetSaves();
                        selected = 1;
                        selCount = saves.Length + 1;
                        GUI.vt1.ClearList();
                        GUI.Save(saves);
                        GUI.vt1.UpdateList(1, 1, selCount, 2, 14);
                    }
                    break;
                }
                if (selected > 1)
                {
                    GUI.vt1.UpdateBox(3, 1, ". . ." +
                    " ---------------------------------- Nazwa Gracza - " + saves[selected - 2].name.ToString() +
                    " ---------------------------------- Osiągniety Poziom - " + saves[selected - 2].lvl.ToString() +
                    " ---------------------------------- Posiadany Majątek - " + saves[selected - 2].wallet.ToString()  +
                    " ---------------------------------- Ostatni Zapis - " + saves[selected - 2].lastplay.ToString() + " ---------------------------------- . . .");
                    GUI.vt1.Showability(3, 3, true);
                }
                else { GUI.vt1.UpdateBox(3, 1, "E / Utwórz Nowy Zapis"); GUI.vt1.Showability(3, 3, false); }
            }
            GUI.vt1.ClearList();
        }
        public void Options()
        {
            int[] opt = OPTIONS.GetOptionsView();
            int selected = 1, selCount = OPTIONS.GetOptionsCount() + 1, selStart = 1, R = 6;
            GUI.Options(); GUI.vt1.UpdateList(selected, selected, selCount, 2);
            while (openScreen == "Options")
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GUI.vt1.UpdateList(selected, selected + 1, selCount, 2); } break;
                    case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GUI.vt1.UpdateList(selected, selected - 1, selCount, 2); } break;

                    case ConsoleKey.A:
                        if (selected < selCount && opt[selected-1] > 0)
                        {
                            switch (selected) {
                                case 3: S.SetSoundVolume(opt[selected - 1]); break;
                                case 4: S.SetMusicVolume(opt[selected - 1]); break; }
                            GUI.vt1.UpdateSlider(3, selected, R, --opt[selected-1]);
                            S.Play("K3");
                        }
                        break;

                    case ConsoleKey.D:
                        if (selected < selCount && opt[selected-1] < R)
                        {
                            switch(selected){
                                case 3: S.SetSoundVolume(opt[selected - 1]); break;
                                case 4: S.SetMusicVolume(opt[selected - 1]); break;}
                            GUI.vt1.UpdateSlider(3, selected, R, ++opt[selected - 1]);
                            S.Play("K2");
                        }
                        break;

                    case ConsoleKey.Escape:
                    case ConsoleKey.Q: S.Play("K3"); S.SetSoundVolume(); S.SetMusicVolume(); openScreen = lastScreen; break;
                    case ConsoleKey.E: S.Play("K2"); GUI.vt1.ClearList(); if (selected == selCount) OPTIONS.ResetOptions(); else OPTIONS.SaveOptions(opt);
                        GUI.Options(); selected = 1; GUI.vt1.UpdateList(selected, selected, selCount, 2); opt = OPTIONS.GetOptionsView(); break;
                }
            }
            GUI.vt1.ClearList();
        }
        public void Help()
        {
            GUI.Help();
            openScreen = lastScreen;
            Console.ReadKey(true);
            S.Play("K3");
            GUI.vt1.ClearList();
        }
    }
}
