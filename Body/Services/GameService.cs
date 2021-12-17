using FarmConsole.Body.Controlers;
using FarmConsole.Body.Models;
using FarmConsole.Body.Views.LocationViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Services
{
    class GameService : MainController
    {
        private static int[,] GrowCycle;
        
        public static void SetGrowCycle()
        {
            int MaxStateCount = 0;
            List<ObjectModel> Objects = ObjectModel.GetObjects(_Category:2, _Scale:0, _State:0);
            List<string> DaysForStates = new List<string>();
            for (int i = 0; i < Objects.Count; i++) DaysForStates.Add(Objects[i].Price.ToString());
            foreach (var dfs in DaysForStates) if (dfs.Length > MaxStateCount) MaxStateCount = dfs.Length;
            GrowCycle = new int[Objects.Count, MaxStateCount + 1];
            for (int i = 0; i < DaysForStates.Count; i++)
                for (int j = 0; j < MaxStateCount + 1; j++)
                    if (j < DaysForStates[i].Length) GrowCycle[i, j] = int.Parse(DaysForStates[i][j].ToString());
                    else GrowCycle[i, j] = 99;
        }
        public static void Sleep()
        {
            MapManager.HideMap();
            GrowingUp();
            TimeLapse();
            MapManager.ShowMap();
        }
        private static void TimeLapse()
        {
            DateTime today = GameInstance.GameDate;
            DateTime tomorrow = today.AddDays(1);
            AnimationController.DateChangeEffect(today.ToString("u").Split(' ')[0], tomorrow.ToString("u").Split(' ')[0]);
            GameInstance.GameDate = tomorrow;
        }
        public static void GrowingUp()
        {
            Random rnd = new Random();
            int Increase;
            for (int x = 0; x < GameInstance.GetMap("Farm").Size; x++)
                for (int y = 0; y < GameInstance.GetMap("Farm").Size; y++)
                {
                    FieldModel Field = GameInstance.GetMap("Farm").GetField(x, y);
                    if (Field.Category == 2 && Field.Type > 0 && Field.ToProduct().StateName != "Zgniłe") // state fields
                    {
                        // +5 synthetic fertilize + water
                        // +4 synthetic fertilize
                        // +3 natural fertilize + water
                        // +2 natural fertilize
                        // +1 water
                        // +0 no water

                        int RottenState = ObjectModel.GetObject(Field.ObjectName, _StateName: "Zgniłe").State;
                        Increase = Field.Duration / 10;
                        Field.Duration += Increase > 0 ? Increase : 1;
                        Field.Duration -= Increase * 10;

                        if (rnd.Next() % 100 >= (Increase % 2 == 1 ? 100 : 600) && Field.State + 1 != RottenState)
                            Field.State = RottenState;
                        else while (Field.Duration >= GrowCycle[Field.Type, Field.State])
                                Field.Duration -= GrowCycle[Field.Type, Field.State++];
                        Field = Field.ToField();
                    }
                }
        }
        public static void LvlUp()
        {
            GameInstance.LVL++;
            foreach (var Rule in GameInstance.Rules)
                Rule.Update(GameInstance.LVL);
        }
    }
}
