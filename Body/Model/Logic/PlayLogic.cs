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
        public void Play()
        {
            GUI.Play();

            List<Product>[] items = save.GetInventory();
            List<int>[] activitiesID = new List<int>[]
            {
                new List<int>() { 0,6,7 },      // 0 pola nieuprawne
                new List<int>() { 0,1,8 },      // 1 pola uprawne
                new List<int>() { 0,1,9,10 },   // 2 pola posiane
                new List<int>() { 0,1,9,10 },   // 3 pola rosnące
                new List<int>() { 0,1,11 },     // 4 pola dojrzałe
                new List<int>() { 0,1,12 },     // 5 pola zgniłe
                new List<int>() { 0,1,2,4 },    // 6 budynki użytkowe
                new List<int>() { 0,1,2,3 },    // 7 dekoracje statyczne  
                new List<int>() { 0,2,3,5 },    // 8 maszyny rolne
            };
            List<string>[] activities = new List<string>[activitiesID.Length];
            for (int i = 0; i < activitiesID.Length; i++)
            {
                activities[i] = new List<string>();
                for (int j = 0; j < activitiesID[i].Count; j++)
                    activities[i].Add(XF.GetString(100 + activitiesID[i][j]));
            }

            int categoryFiled = 0;
            int categoryItems = 1;
            int LSize = activities[categoryFiled].Count;
            int RSize = items[categoryItems].Count;
            int CS = 1, PS = 1, LS = 1, RS = 1;     // CurrentSelect / PreviousSelect / LeftSelect / RightSelect //
            bool Q = false, E = false, OS = false;  // OppositeSelect / true - left / false - right //

            int FarmSize = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(save.GetMap().Length/3)));
            int X = FarmSize/2, Y = FarmSize/2;
            bool ShiftPressed;

            FieldsMenager fm = new FieldsMenager(save.GetMap());

            while (openScreen == "Play")
            {
                //QH.INFO(-3, "X: " + X, "Y: " + Y, "", "");
                var cki = Console.ReadKey(true);
                if (cki.Modifiers == ConsoleModifiers.Shift) ShiftPressed = true; else ShiftPressed = false;

                if (E||Q) switch (cki.Key)
                {
                    case ConsoleKey.Q: if (Q&&!E)    { Q = false; GUI.vt2.ClearList(); OS = true; fm.ShowPartFarm("left",E); }
                                  else if (Q&&E&&OS) { LS = 1;  CS = PS = RS; OS = false; GUI.vt2.ClearList(); GUI.vt3.UpdateList(CS, PS, RSize); Q = false; fm.ShowPartFarm("left",E); }
                                  else if (E)        { RS = PS; CS = PS = LS; OS = true;  if (!Q) GUI.LeftMenu(activities[categoryFiled]); GUI.vt2.UpdateList(CS, PS, LSize); Q = true; } break;
                    case ConsoleKey.E: if (!Q&&E)    { E = false; GUI.vt3.ClearList(); OS = false; fm.ShowPartFarm("right",Q); }
                                  else if (Q&&E&&!OS){ RS = 1;  CS = PS = LS; OS = true;  GUI.vt3.ClearList(); GUI.vt2.UpdateList(CS, PS, LSize); E = false; fm.ShowPartFarm("right",Q); }
                                  else if (Q)        { LS = PS; CS = PS = RS; OS = false; if (!E) GUI.RightMenu(items,categoryItems); GUI.vt3.UpdateList(CS, PS, RSize); E = true; } break;
                    case ConsoleKey.W: if (!OS && CS > 1) GUI.vt3.UpdateList(--CS, PS, RSize);
                                  else if (OS  && CS > 1) GUI.vt2.UpdateList(--CS, PS, LSize); PS = CS; break;
                    case ConsoleKey.S: if (!OS && CS < RSize) GUI.vt3.UpdateList(++CS, PS, RSize);
                                  else if (OS  && CS < LSize) GUI.vt2.UpdateList(++CS, PS, LSize); PS = CS; break;
                    case ConsoleKey.A: if (!OS && categoryItems > 1)
                            {
                                categoryItems--;
                                RSize = items[categoryItems].Count;
                                CS = PS = 1;
                                GUI.RightMenu(items, categoryItems);
                                GUI.vt3.UpdateList(CS, PS, RSize);
                            }
                            break;
                    case ConsoleKey.D: if (!OS && categoryItems < items.Length - 1)
                            {
                                categoryItems++;
                                RSize = items[categoryItems].Count;
                                CS = PS = 1;
                                GUI.RightMenu(items, categoryItems);
                                GUI.vt3.UpdateList(CS, PS, RSize);
                            }
                            break;
                    }
                else switch (cki.Key)
                {
                    case ConsoleKey.Escape: fm.ClearFarm(); openScreen = "Escape"; save.SetMap(fm.GetMap()); S.Play("K2"); break;
                    case ConsoleKey.Q: Q = true; CS = PS = 1; GUI.LeftMenu(activities[categoryFiled]); GUI.vt2.UpdateList(CS, PS, LSize); OS = true; break;
                    case ConsoleKey.E: E = true; CS = PS = 1; GUI.RightMenu(items,categoryItems); GUI.vt3.UpdateList(CS, PS, RSize); OS = false; break;

                    case ConsoleKey.W: if (X > 1) fm.MoveSelect(new Point(X, Y), new Point(--X, Y), ShiftPressed);
                            categoryFiled = fm.GetCategory(); LSize = activities[categoryFiled].Count; break;
                    case ConsoleKey.A: if (Y > 1) fm.MoveSelect(new Point(X, Y), new Point(X, --Y), ShiftPressed);
                            categoryFiled = fm.GetCategory(); LSize = activities[categoryFiled].Count; break;
                    case ConsoleKey.S: if (X < FarmSize) fm.MoveSelect(new Point(X, Y), new Point(++X, Y), ShiftPressed);
                            categoryFiled = fm.GetCategory(); LSize = activities[categoryFiled].Count; break;
                    case ConsoleKey.D: if (Y < FarmSize) fm.MoveSelect(new Point(X, Y), new Point(X, ++Y), ShiftPressed);
                            categoryFiled = fm.GetCategory(); LSize = activities[categoryFiled].Count; break;

                    case ConsoleKey.UpArrow: fm.MoveFarm(new Point(0, -1)); break;
                    case ConsoleKey.DownArrow: fm.MoveFarm(new Point(0, 1)); break;
                    case ConsoleKey.LeftArrow: fm.MoveFarm(new Point(-1, 0)); break;
                    case ConsoleKey.RightArrow: fm.MoveFarm(new Point(1, 0)); break;
                }
            }
            GUI.vt1.ClearList();
        }
    }
}
