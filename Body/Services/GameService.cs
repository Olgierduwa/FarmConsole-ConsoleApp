using FarmConsole.Body.Controlers;
using FarmConsole.Body.Controlers.MenuControlers;
using FarmConsole.Body.Models;
using FarmConsole.Body.Views.LocationViews;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Services
{
    public class GameService : MainControllerService
    {
        public static void Sleep()
        {
            MapView.HideMap();
            TimeLapse();
            GrowingUp();
            MapView.ShowMap();
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
            int ChanceOfGrowing;
            int MapSize = (int)Math.Sqrt(Convert.ToDouble(GameInstance.FarmMap.Length));
            for (int x = 0; x < MapSize; x++)
                for (int y = 0; y < MapSize; y++)
                    if (GameInstance.FarmMap[x, y].Category == 2 && GameInstance.FarmMap[x, y].Type > 0 &&
                        ProductModel.GetProduct(GameInstance.FarmMap[x, y]).StateName != "Zgniłe") // state fields
                    {
                        FieldModel Field = GameInstance.FarmMap[x, y];
                        if (Field.Duration >= 50)
                        {
                            ChanceOfGrowing = 100;
                            //Field.Duration -= 50;
                        }
                        else ChanceOfGrowing = 100;
                        if (rnd.Next() % 100 < ChanceOfGrowing)
                        {
                            Field.Duration++;
                            if (Field.Duration % 1 == 0) Field.State++;
                            if (Field.State == ProductModel.GetProduct(new ProductModel(Field).ProductName, _StateName: "Zgniłe").State) Field.State = 0;
                        }
                        else Field.State = ProductModel.GetProduct(new ProductModel(Field).ProductName, _StateName: "Zgniłe").State;
                        Field.Color = ProductModel.GetProduct(Field).Color;
                    }
        }

    }
}
