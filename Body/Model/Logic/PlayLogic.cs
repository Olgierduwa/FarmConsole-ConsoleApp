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
            int CS = 1, PS = 1, LS = 1, RS = 1;     // CurrentSelect / PreviousSelect / LeftSelect / RightSelect //
            bool Q = false, E = false, OS = false;  // OppositeSelect / true - left / false - right //

            int FarmSize = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(save.GetMap().Length)));
            int X = FarmSize/2, Y = FarmSize/2;
            bool ShiftPressed;

            FieldsMenager fm = new FieldsMenager(save.GetMap());

            while (openScreen == "Play")
            {
                //QH.INFO(-3, "X: " + X, "Y: " + Y, "", "");
                var cki = Console.ReadKey(true);
                if (cki.Modifiers == ConsoleModifiers.Shift) ShiftPressed = true; else ShiftPressed = false;

                if(E||Q) switch (cki.Key)
                {
                    case ConsoleKey.Q: if (Q&&!E)    { Q = false; GUI.vt2.ClearList(); OS = true; fm.ShowPartFarm("left"); }
                                  else if (Q&&E&&OS) { LS = 1;  CS = PS = RS; OS = false; GUI.vt2.ClearList(); GUI.vt3.UpdateList(CS, PS, RSize); Q = false; fm.ShowPartFarm("left"); }
                                  else if (E)        { RS = PS; CS = PS = LS; OS = true;  if (!Q) GUI.LeftMenu(left_list); GUI.vt2.UpdateList(CS, PS, LSize); Q = true; } break;
                    case ConsoleKey.E: if (!Q&&E)    { E = false; GUI.vt3.ClearList(); OS = false; fm.ShowPartFarm("right"); }
                                  else if (Q&&E&&!OS){ RS = 1;  CS = PS = LS; OS = true;  GUI.vt3.ClearList(); GUI.vt2.UpdateList(CS, PS, LSize); E = false; fm.ShowPartFarm("right"); }
                                  else if (Q)        { LS = PS; CS = PS = RS; OS = false; if (!E) GUI.RightMenu(right_list); GUI.vt3.UpdateList(CS, PS, RSize); E = true; } break;
                    case ConsoleKey.W: if (!OS && CS > 1) GUI.vt3.UpdateList(--CS, PS, RSize);
                                  else if (OS  && CS > 1) GUI.vt2.UpdateList(--CS, PS, LSize); PS = CS; break;
                    case ConsoleKey.S: if (!OS && CS < RSize) GUI.vt3.UpdateList(++CS, PS, RSize);
                                  else if (OS  && CS < LSize) GUI.vt2.UpdateList(++CS, PS, LSize); PS = CS; break;
                }
                else switch (cki.Key)
                {
                    case ConsoleKey.Escape: fm.ClearFarm(); openScreen = "Escape"; save.SetMap(fm.GetMap()); S.Play("K2"); break;
                    case ConsoleKey.Q: Q = true; CS = PS = 1; GUI.LeftMenu(left_list); GUI.vt2.UpdateList(CS, PS, LSize); OS = true; break;
                    case ConsoleKey.E: E = true; CS = PS = 1; GUI.RightMenu(right_list); GUI.vt3.UpdateList(CS, PS, RSize); OS = false; break;

                    case ConsoleKey.W: if (X > 1) fm.MoveSelect(new Point(X, Y), new Point(--X, Y), ShiftPressed); break;
                    case ConsoleKey.A: if (Y > 1) fm.MoveSelect(new Point(X, Y), new Point(X, --Y), ShiftPressed); break;
                    case ConsoleKey.S: if (X < FarmSize) fm.MoveSelect(new Point(X, Y), new Point(++X, Y), ShiftPressed); break;
                    case ConsoleKey.D: if (Y < FarmSize) fm.MoveSelect(new Point(X, Y), new Point(X, ++Y), ShiftPressed); break;

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
