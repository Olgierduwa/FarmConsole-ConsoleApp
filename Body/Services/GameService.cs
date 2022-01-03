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
        private static short[,] GrowCycle;
        
        public static void SetGrowCycle()
        {
            int MaxStateCount = 0;
            List<ObjectModel> Objects = ObjectModel.GetObjects(_Category:1, _Scale:0, _State:0);
            List<string> DaysForStates = new List<string>();
            for (int i = 0; i < Objects.Count; i++) DaysForStates.Add(Objects[i].Price.ToString());
            foreach (var dfs in DaysForStates) if (dfs.Length > MaxStateCount) MaxStateCount = dfs.Length;
            GrowCycle = new short[Objects.Count, MaxStateCount + 1];
            for (int i = 0; i < DaysForStates.Count; i++)
                for (int j = 0; j < MaxStateCount + 1; j++)
                    if (j < DaysForStates[i].Length) GrowCycle[i, j] = short.Parse(DaysForStates[i][j].ToString());
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
            short Increase;
            for (int x = 0; x < GameInstance.GetMap("Farm").MapSize; x++)
                for (int y = 0; y < GameInstance.GetMap("Farm").MapSize; y++)
                {
                    FieldModel Field = GameInstance.GetMap("Farm").GetField(x, y);
                    if (Field.Category == 1 && Field.Type > 0 && Field.ToProduct().StateName != "Zgniłe") // state fields
                    {
                        // +5 synthetic fertilize + water
                        // +4 synthetic fertilize
                        // +3 natural fertilize + water
                        // +2 natural fertilize
                        // +1 water
                        // +0 no water

                        int RottenState = ObjectModel.GetObject(Field.ObjectName, _StateName: "Zgniłe").State;
                        Increase = (short)(Field.Duration / 10);
                        Field.Duration += Increase > 0 ? Increase : (short)1;
                        Field.Duration -= (short)(Increase * 10);

                        if (rnd.Next() % 100 >= (Increase % 2 == 1 ? 100 : 60) && Field.State + 1 != RottenState)
                            Field.State = RottenState;
                        else while (Field.Duration >= GrowCycle[Field.Type, Field.State])
                                Field.Duration -= GrowCycle[Field.Type, Field.State++];

                        Field = Field.ToField();
                        GameInstance.GetMap("Farm").SetField(x, y, Field);
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
