using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using FarmConsole.Body.Views.MenuViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FarmConsole.Body.Views.CentralViews
{
    class ContainerView : MenuManager
    {
        private static List<CM> RightList;
        private static List<CM> LeftList;

        public static void DisplayCaptains(string place, string[] item, bool side, int sliderValue)
        {
            ClearList(false);

            int Height = Console.WindowHeight - 15;
            int Multiplier = new int[] { 1, 5, 10, 25, 50 } [sliderValue];

            Endl(3);
            H2(StringService.Get("container label"));

            GroupStart(0);

                GroupStart(3);
                Endl(3);
                TextBox(place);
                Endl(1);
                TextBoxLines(item);
                GroupEnd();

                GroupStart(Console.WindowWidth / 2, Console.WindowWidth);
                Endl(Height + 8 - item.Length * 2);
                Slider(4, sliderValue);
                GroupEnd();

                GroupStart(3);
                Endl(Height);

                if(!side)
                {
                    TextBox(StringService.Get("take item", " " + Multiplier + "x [E]  >>>>", Before: ">>>>  "), foreground: ColorService.GetColorByName("limeD"), margin: 0);
                    TextBox(StringService.Get("put items back", " [Q]  <<<<", Before: "<<<<  "), foreground: ColorService.GetColorByName("orangeL"), margin:0);
                }
                else
                {
                    TextBox(StringService.Get("put item back", " " + Multiplier + "x [E]  <<<<", Before: "<<<<  "), foreground: ColorService.GetColorByName("orangeL"), margin: 0);
                    TextBox(StringService.Get("take items", " [Q]  >>>>", Before: ">>>>  "), foreground: ColorService.GetColorByName("limeD"), margin: 0);
                }

                GroupEnd();

            GroupEnd();

            PrintList();
        }

        public static void DisplayLeftList(ProductModel[] products, int selected, bool focus)
        {
            ClearList(false);

            Endl(5);
            GroupStart(0);
            GroupStart(1, 3);
            Endl(3);

            int Height = Console.WindowHeight - 5;
            for (int i = 0; i < products.Length; i++)
            {
                if (i * 3 < Height)
                {
                    if (products[i] != null) TextBox(products[i].Amount + "x " + products[i].ObjectName, width: 30);
                    else TextBox("...", width: 30);
                }
                else
                {
                    if (products[i] != null) TextBox(products[i].Amount + "x " + products[i].ObjectName, width: 30, show: false);
                    else TextBox("...", width: 30, show: false);
                }
            }

            GroupEnd();
            GroupEnd();


            if (focus)
            {
                PrintList();
                UpdateSelect(selected, selected, products.Length);
            }
            else DisableView();
            LeftList = ComponentList.ToList();
        }

        public static void DisplayRightList(List<ProductModel> products, int selected, bool focus, bool cleaning = false)
        {
            if (cleaning && View != null)
                foreach (var CD in ComponentsDisplayed)
                    View.DisplayPixels(CD.Pos, CD.Size);

            ClearList(false);

            Endl(5);
            GroupStart(0);
            GroupStart(3, 3);
            Endl(3);

            int Height = Console.WindowHeight - 14;

            if (products.Count == 0) TextBox("...", width: 30);
            else for (int i = 0; i < products.Count; i++)
                {
                    if (i * 3 < Height) TextBox(products[i].Amount + "x " + products[i].ObjectName, width: 30);
                    else TextBox(products[i].Amount + "x " + products[i].ObjectName, width: 30, show: false);
                }

            GroupEnd();
            GroupEnd();

            if (focus)
            {
                PrintList();
                UpdateSelect(selected, selected, products.Count > 0 ? products.Count : 1);
            }
            else DisableView();

            RightList = ComponentList.ToList();
            ComponentsDisplayed = new List<CM>() { GetComponentByName("TB", 2, products.Count > 0 ? products.Count : 1) };
        }

        public static void GetRightView() => RightList = ComponentList.ToList();

        public static void SetRightView() => ComponentList = RightList.ToList();

        public static void SetLeftView() => ComponentList = LeftList.ToList();
    }
}
