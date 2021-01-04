using FarmConsole.Body.Model.Helpers;
using FarmConsole.Body.Model.Objects;
using FarmConsole.Body.Sounds;
using FarmConsole.Body.View.Components;
using FarmConsole.Body.View.GUI;
using System;

namespace FarmConsole.Body.Model.Logic
{
    public partial class Logic
    {
        public static string POPUPTEXT = "";
        public static int POPUPSTAGE = -1;
        public static int POPUPID = 0;
        public static long xxx = 0;
        public static long yyy = 0;
        private static Save save = new Save();
        public static string openScreen = "Load";
        public static string lastScreen = "Menu";
        public static DateTime Now = DateTime.Now;

        public bool? Warning(string type)
        {
            S.Play("K2");
            int focus = 0;
            switch (type)
            {
                case "exit": focus = 4; break;
                case "menu": focus = 3; break;
                case "delete": focus = 6; break;
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
            GUI.vt1.Showability(focus + 1, 0, false);
            GUI.vt1.Showability(focus + 2, 0, false);
            GUI.vt1.PrintList();
            return choice;
        }
        public static void PopUp(int id, string text)
        {
            string popupText = text;
            if (id > 0) popupText = XF.GetString(300 + id);
            if (POPUPSTAGE >= 0) POPUPSTAGE = Animations.PopUp(new ViewTools().TextBoxView(popupText), POPUPSTAGE);
            if (POPUPSTAGE < 0) POPUPID = 0;
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
                    case "Intro": Intro(); break;
                    case "Menu": Menu(); break;
                    case "Load": Load(); break;
                    case "Save": Save(); break;
                    case "Help": Help(); break;
                    case "Play": LogicPlay.Play(); break;
                }
            }
        }

        public void Intro()
        {
            Animations.SlideEffect(2, "purple");
            Animations.SlideEffect(3, "red");
            Animations.SlideEffect(0, "yellow");
            Animations.SlideEffect(3, "green");
            Animations.SlideEffect(1, "blue");
            Animations.SlideEffect(0, "white");
            Animations.CrossEffect(0, "red");
            Animations.CrossEffect(1, "green");
            Animations.CrossEffect(0, "blue");
            Animations.CrossEffect(1, "purple");
            Animations.CrossEffect(0, "white");
        }
        public void Menu()
        {
            int selected = 1, selCount = 4, selStart = 1, sleep = 1000;
            GUI.Menu(); GUI.vt1.UpdateSelect(selected, selected, selCount);
            while (openScreen == "Menu")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.R: POPUPID = 1; POPUPSTAGE = 0; break;
                        case ConsoleKey.T: openScreen = "Intro"; break;
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GUI.vt1.UpdateSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GUI.vt1.UpdateSelect(selected, selected - 1, selCount); } break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: if (Warning("exit") == true) openScreen = "Close"; break;
                        case ConsoleKey.E:
                            S.Play("K2"); switch (selected)
                            {
                                case 1: openScreen = "NewGame"; break;
                                case 2: openScreen = "Load"; break;
                                case 3: openScreen = "Options"; break;
                                case 4: openScreen = "Help"; break;
                            }
                            break;
                    }
                }
                else if ((DateTime.Now - Now).TotalMilliseconds >= sleep)
                {
                    sleep = 50;
                    PopUp(POPUPID, POPUPTEXT);
                    Now = DateTime.Now;
                }
            }
            GUI.vt1.ClearList();
        }
        public void Escape()
        {
            lastScreen = "Escape";
            int selected = 1, selCount = 5, selStart = 1;
            GUI.Escape(); GUI.vt1.UpdateSelect(selected, selected, selCount);
            while (openScreen == "Escape")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GUI.vt1.UpdateSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GUI.vt1.UpdateSelect(selected, selected - 1, selCount); } break;
                        case ConsoleKey.Escape: S.Play("K3"); openScreen = "Play"; GUI.vt1.ClearList(); break;
                        case ConsoleKey.Q: S.Play("K3"); openScreen = "Play"; break;
                        case ConsoleKey.E:
                            switch (selected)
                            {
                                case 1: S.Play("K3"); openScreen = "Play"; break;
                                case 2: S.Play("K2"); openScreen = "Save"; break;
                                case 3: if (Warning("menu") == true) { openScreen = lastScreen = "Menu"; save = new Save(); } break;
                                case 4: S.Play("K2"); openScreen = "Options"; break;
                                case 5: S.Play("K2"); openScreen = "Help"; break;
                            }
                            break;
                    }
                }
                else if ((DateTime.Now - Now).TotalMilliseconds >= 50)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Now = DateTime.Now;
                }
            }
            GUI.vt1.ClearList();
        }
        public void NewGame()
        {
            int selected = 1, selCount = 2, selStart = 1;
            GUI.NewGame(); GUI.vt1.UpdateSelect(selected, selected, selCount);
            while (openScreen == "NewGame")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GUI.vt1.UpdateSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GUI.vt1.UpdateSelect(selected, selected - 1, selCount); } break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: S.Play("K3"); openScreen = "Menu"; break;
                        case ConsoleKey.E:
                            switch (selected)
                            {
                                case 1: S.Play("K2"); openScreen = "Play"; save = new Save("Locha"); break;
                                case 2: S.Play("K2"); openScreen = "Play"; save = new Save("Turbo Kox"); break;
                            }
                            break;
                    }
                }
                else if ((DateTime.Now - Now).TotalMilliseconds >= 50)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Now = DateTime.Now;
                }
            }
            GUI.vt1.ClearList();
        }
        public void Load()
        {
            Save[] saves = XF.GetSaves();
            int selected = 1, selCount = saves.Length + 1, selStart = 1;
            GUI.Load(saves); GUI.vt1.UpdateSelect(selected, selected, selCount);
            while (openScreen == "Load")
            {

                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GUI.vt1.UpdateSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GUI.vt1.UpdateSelect(selected, selected - 1, selCount); } break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: S.Play("K3"); openScreen = "Menu"; break;
                        case ConsoleKey.E: S.Play("K2"); switch (selected) { case 1: openScreen = "NewGame"; break; default: save.Load(selected - 1); openScreen = "Play"; break; } break;
                        case ConsoleKey.D:
                            if (selected > 1 && Warning("delete") == true)
                            {
                                saves[selected - 2].Delete();
                                saves = XF.GetSaves();
                                selected = 1;
                                selCount = saves.Length + 1;
                                GUI.vt1.ClearList();
                                GUI.Load(saves);
                                GUI.vt1.UpdateSelect(1, 1, selCount);
                            }
                            break;
                    }
                    if (selected > 1)
                    {
                        GUI.vt1.UpdateBox(3, 1, ". . ." + " ---------------------------------- " +
                        "Nazwa Gracza - " + saves[selected - 2].Name.ToString() + " ---------------------------------- " +
                        "Osiągniety Poziom - " + saves[selected - 2].LVL.ToString() + " ---------------------------------- " +
                        "Posiadany Majątek - " + saves[selected - 2].Wallet.ToString() + " ---------------------------------- " +
                        "Ostatni Zapis - " + saves[selected - 2].Lastplay.ToString() + " ---------------------------------- . . .");
                        GUI.vt1.Showability(4, 1, true);
                        GUI.vt1.Showability(5, 1, true);
                    }
                    else
                    {
                        GUI.vt1.UpdateBox(3, 1, "E / Rozpocznij Nową Grę");
                        GUI.vt1.Showability(3, 3, false);
                        GUI.vt1.Showability(4, 1, false);
                        GUI.vt1.Showability(5, 1, false);
                    }
                }
                else if ((DateTime.Now - Now).TotalMilliseconds >= 50)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Now = DateTime.Now;
                }
            }
            GUI.vt1.ClearList();
        }
        public void Save()
        {
            Save[] saves = XF.GetSaves();
            int selected = 1, selCount = saves.Length + 1, selStart = 1;
            GUI.Save(saves); GUI.vt1.UpdateSelect(selected, selected, selCount);
            while (openScreen == "Save")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GUI.vt1.UpdateSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GUI.vt1.UpdateSelect(selected, selected - 1, selCount); } break;
                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: S.Play("K3"); openScreen = "Escape"; break;
                        case ConsoleKey.E:
                            if (selected == 1 || Warning("update") == true)
                            {
                                save.Update(selected - 1);
                                saves = XF.GetSaves();
                                selected = 1;
                                selCount = saves.Length + 1;
                                GUI.vt1.ClearList();
                                GUI.Save(saves);
                                GUI.vt1.UpdateSelect(1, 1, selCount);
                            }
                            break;
                    }
                    if (selected > 1)
                    {
                        GUI.vt1.UpdateBox(3, 1, ". . ." + " ---------------------------------- " +
                        "Nazwa Gracza - " + saves[selected - 2].Name.ToString() + " ---------------------------------- " +
                        "Osiągniety Poziom - " + saves[selected - 2].LVL.ToString() + " ---------------------------------- " +
                        "Posiadany Majątek - " + saves[selected - 2].Wallet.ToString() + " ---------------------------------- " +
                        "Ostatni Zapis - " + saves[selected - 2].Lastplay.ToString() + " ---------------------------------- . . .");
                        GUI.vt1.Showability(3, 0, true);
                    }
                    else
                    {
                        GUI.vt1.UpdateBox(3, 1, "E / Rozpocznij Nową Grę");
                        GUI.vt1.Showability(3, 3, false);
                    }
                }
                else if ((DateTime.Now - Now).TotalMilliseconds >= 50)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Now = DateTime.Now;
                }
            }
            GUI.vt1.ClearList();
        }
        public void Options()
        {
            int[] opt = OPTIONS.GetOptionsView();
            int selected = 1, selCount = OPTIONS.GetOptionsCount() + 1, selStart = 1, R = 6;
            GUI.Options(); GUI.vt1.UpdateSelect(selected, selected, selCount);
            while (openScreen == "Options")
            {
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.W: if (selected > selStart) { S.Play("K1"); selected--; GUI.vt1.UpdateSelect(selected, selected + 1, selCount); } break;
                        case ConsoleKey.S: if (selected < selCount) { S.Play("K1"); selected++; GUI.vt1.UpdateSelect(selected, selected - 1, selCount); } break;

                        case ConsoleKey.A:
                            if (selected < selCount && opt[selected - 1] > 0)
                            {
                                switch (selected)
                                {
                                    case 3: S.SetSoundVolume(opt[selected - 1]); break;
                                    case 4: S.SetMusicVolume(opt[selected - 1]); break;
                                }
                                GUI.vt1.UpdateSlider(3, selected, R, --opt[selected - 1]);
                                S.Play("K3");
                            }
                            break;

                        case ConsoleKey.D:
                            if (selected < selCount && opt[selected - 1] < R)
                            {
                                switch (selected)
                                {
                                    case 3: S.SetSoundVolume(opt[selected - 1]); break;
                                    case 4: S.SetMusicVolume(opt[selected - 1]); break;
                                }
                                GUI.vt1.UpdateSlider(3, selected, R, ++opt[selected - 1]);
                                S.Play("K2");
                            }
                            break;

                        case ConsoleKey.Escape:
                        case ConsoleKey.Q: S.Play("K3"); S.SetSoundVolume(); S.SetMusicVolume(); openScreen = lastScreen; break;
                        case ConsoleKey.E:
                            S.Play("K2"); GUI.vt1.ClearList(); if (selected == selCount) OPTIONS.ResetOptions(); else OPTIONS.SaveOptions(opt);
                            GUI.Options(); selected = 1; GUI.vt1.UpdateSelect(selected, selected, selCount); opt = OPTIONS.GetOptionsView(); break;
                    }
                }
                else if ((DateTime.Now - Now).TotalMilliseconds >= 50)
                {
                    PopUp(POPUPID, POPUPTEXT);
                    Now = DateTime.Now;
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
