using FarmConsole.Body.Controllers.CentralControllers;
using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services.MainServices;
using FarmConsole.Body.Views.GameViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Services.GameServices
{
    class GameService : HeadController
    {
        private static short[,] GrowCycle;

        public static void SetGrowCycle()
        {
            int MaxStateCount = 0;
            List<ObjectModel> Objects = ObjectModel.GetObjects(_Category: 1, _Scale: 0, _State: 0);
            List<string> DaysForStates = new List<string>();
            for (int i = 0; i < Objects.Count; i++) DaysForStates.Add(Objects[i].Price.ToString());
            foreach (var dfs in DaysForStates) if (dfs.Length > MaxStateCount) MaxStateCount = dfs.Length;
            GrowCycle = new short[Objects.Count, MaxStateCount + 1];
            for (int i = 0; i < DaysForStates.Count; i++)
                for (int j = 0; j < MaxStateCount + 1; j++)
                    if (j < DaysForStates[i].Length) GrowCycle[i, j] = short.Parse(DaysForStates[i][j].ToString());
                    else GrowCycle[i, j] = 99;
        }
        public static string Sleep()
        {
            if (GameInstance.Hunger > 750) return LS.Action("cant sleep cause hunger");
            if (GameInstance.Energy > 9000) return LS.Action("cant sleep cause too much energy");
            MapService.HideMap();
            GrowingUp();
            RegulateHealth();
            TimeLapse();
            MapService.ShowMap();
            return LS.Action("done");
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
                    if (Field.Category == 2 && Field.Type == 0 && Field.State == 0)
                    {
                        if (rnd.Next() % 100 > 95) Field.State++;
                        Field = Field.ToField();
                        GameInstance.GetMap("Farm").SetField(x, y, Field);
                    }
                    else if (Field.Category == 1 && Field.Type > 0 && Field.ToProduct().StateName != "Zgniłe") // state fields
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


        public static void IncreaseInExperience(int DoseOfEnergy)
        {
            int DoseOfExperience = DoseOfEnergy * (12 - GameInstance.Difficulty) / 100;
            GameInstance.Experience += DoseOfExperience;
            if (GameInstance.Experience >= ConvertService.RequiredExperience(GameInstance.LVL))
            {
                GameInstance.LVL++;
                GameInstance.Experience = 0;
                foreach (var Rule in GameInstance.Rules)
                    Rule.Update(GameInstance.LVL);
            }
        }
        public static void IncreaseInEnergy(int DoseOfEnergy)
        {
            GameInstance.Energy += DoseOfEnergy * (12 - GameInstance.Gender) / 10;
            if (GameInstance.Energy > 10000)
            {
                GameInstance.Energy = 10000;
                GameInstance.Immunity--;
            }
        }
        public static void DecreaseInEnergy(int DoseOfEnergy)
        {
            GameInstance.Energy -= DoseOfEnergy * (12 - GameInstance.Gender) / 10;
            if (GameInstance.Energy < 0)
            {
                GameInstance.Health += GameInstance.Energy / 10;
                GameInstance.Energy = 0;
                if (GameInstance.Health <= 0) { Die(); return; }
            }
        }
        public static void IncreaseInHunger()
        {
            double DoseOfHunger = Math.Pow(1.1, (10000 - GameInstance.Energy) / 200) + 2;
            DoseOfHunger = DoseOfHunger * (8 + GameInstance.Gender) / 10;
            GameInstance.Hunger += Convert.ToInt32(DoseOfHunger);
            if (GameInstance.Hunger > 1000) GameInstance.Hunger = 1000;
            if (GameInstance.Hunger > 600) DecreaseInEnergy((GameInstance.Hunger - 600) / 2);
        }
        public static void DecreaseInHunger(int DoseOfHungry)
        {
            GameInstance.Hunger -= DoseOfHungry * (12 - GameInstance.Gender) / 10;
            if (GameInstance.Hunger < 1) GameInstance.Hunger = 0;
        }
        public static void RegulateImmunity(int DoseOfImmunity)
        {
            GameInstance.Immunity += DoseOfImmunity * (12 - GameInstance.Gender) / 10;
            if (GameInstance.Immunity < 1) GameInstance.Immunity = 0;
            if (GameInstance.Immunity > 1000) GameInstance.Immunity = 1000;
        }
        public static void RegulateHealth()
        {
            int Change = (GameInstance.Immunity - (350 + GameInstance.Difficulty * 75)) / (6 + GameInstance.Difficulty);
            if (Change < 0) GameInstance.Health += GameInstance.Health * Change / 100;
            else GameInstance.Health += (1000 - GameInstance.Health) * Change / 100;
            if (GameInstance.Health > 1000) GameInstance.Health = 1000;
            if (GameInstance.Health <= 0) Die();
        }
        public static void Die()
        {
            GameInstance.Health = 0;
            GameInstance.Inventory.Clear();
            GameInstance.SetMap(OpenScreen, MapEngine.Map);
            GameInstance.SetMapPermissions("All", 0);
            GameInstance.ReloadMaps(OpenScreen);
            ColorService.ColorVisibility = false;
            MapService.FadeOut();
        }
        public static string Drink()
        {
            if (GameInstance.Hunger < 10) return LS.Action("you don't feel like drinking");

            Action.ResultTitle = "cheers";
            string[] stats = Action.GetSelectedProduct.Property[1..].Split(':');
            IncreaseInEnergy(Convert.ToInt32(stats[0]));
            DecreaseInHunger(Convert.ToInt32(stats[1]));
            RegulateImmunity(Convert.ToInt32(stats[2]));
            if (!Action.GetSelectedProduct.AddAmount(-1))
            {
                int Index = GameInstance.Inventory.FindIndex(x => x.Amount == 0);
                GameInstance.Inventory.RemoveAt(Index);
            }
            return "";
        }
        public static string Eat(int energy, int hungry, int immunity)
        {
            if (GameInstance.Hunger < 10) return LS.Action("you don't feel like eating");

            Action.ResultTitle = LS.Navigation("cheers");
            IncreaseInEnergy(energy);
            DecreaseInHunger(hungry);
            RegulateImmunity(immunity);
            return "";
        }


        public static void DisplayFieldName()
        {
            if (FieldNameVisibility)
            GameView.DisplayFieldName();
        }
        public static void DisplayStats()
        {
            if (StatsVisibility)
            {
                int[] stats = new int[]
                {
                GameInstance.LVL,
                GameInstance.Experience,
                ConvertService.RequiredExperience(GameInstance.LVL),
                GameInstance.Energy,
                GameInstance.Hunger,
                GameInstance.Immunity,
                GameInstance.Health
                };
                GameView.DisplayStats(stats);
            }
        }
    }
}
