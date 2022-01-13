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
        private static CM LastDisplayedComponent;
        private static int MinimumWinWidth = 140;

        public static void DisplayCaptains(string[] item, bool side, int sliderValue)
        {
            ClearList(false);

            int Height = Console.WindowHeight - 15;
            int Multiplier = new int[] { 1, 5, 10, 25, 50 } [sliderValue];

            Endl(3);
            H2(LS.Navigation("container label"));

            GroupStart(0);

                GroupStart(3);
                Endl(2);
                TextBox(LS.Navigation("selected item"));
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
                    TextBox(LS.Navigation("take item", " " + Multiplier + "x [E]  >>>>", Before: ">>>>  "), foreground: ColorService.GetColorByName("limeD"), margin: 0);
                    TextBox(LS.Navigation("put items back", " [Q]  <<<<", Before: "<<<<  "), foreground: ColorService.GetColorByName("orangeL"), margin:0);
                }
                else
                {
                    TextBox(LS.Navigation("put item back", " " + Multiplier + "x [E]  <<<<", Before: "<<<<  "), foreground: ColorService.GetColorByName("orangeL"), margin: 0);
                    TextBox(LS.Navigation("take items", " [Q]  >>>>", Before: ">>>>  "), foreground: ColorService.GetColorByName("limeD"), margin: 0);
                }

                GroupEnd();

            GroupEnd();

            PrintList();
        }

        public static void DisplayLeftList(ProductModel[] products, string title, int selected, bool focus)
        {
            ClearList(false);

            int Width = Console.WindowWidth > MinimumWinWidth ? 40 : 30;

            Endl(7);
            GroupStart(1, 3);
            TextBox(LS.Object(title), Width);
            GroupEnd();

            GroupStart(1, 3);
            Endl(1);

            int Height = Console.WindowHeight - 15;
            for (int i = 0; i < products.Length; i++)
            {
                if (i < Height / 3)
                {
                    if (products[i] != null) TextBox(products[i].Amount + "x " + LS.Object(products[i].ObjectName), width: Width, margin: 0);
                    else TextBox("...", width: Width, margin: 0);
                }
                else
                {
                    if (products[i] != null) TextBox(products[i].Amount + "x " + LS.Object(products[i].ObjectName), width: Width, show: false, margin: 0);
                    else TextBox("...", width: Width, show: false, margin: 0);
                }
            }

            GroupEnd();
            GroupEnd();


            if (focus)
            {
                PrintList();
                UpdateSelect(selected, selected, products.Length, rangeProp: 15);
            }
            else DisableView();
            LeftList = ComponentList.ToList();
        }

        public static void DisplayRightList(List<ProductModel> products, string title, int selected, bool focus, bool cleaning = false)
        {
            if (cleaning && View != null && LastDisplayedComponent != null)
                View.DisplayPixels(LastDisplayedComponent.Pos, LastDisplayedComponent.Size);

            ClearList(false);

            int Width = Console.WindowWidth > MinimumWinWidth ? 40 : 30;

            Endl(7);
            GroupStart(3, 3);
            TextBox(LS.Object(title), Width);
            GroupEnd();

            GroupStart(3, 3);
            Endl(1);

            int Height = Console.WindowHeight - 15;

            if (products.Count == 0) TextBox("...", width: Width, margin: 0);
            else for (int i = 0; i < products.Count; i++)
                {
                    if (i < Height / 3) TextBox(products[i].Amount + "x " + LS.Object(products[i].ObjectName), width: Width, margin: 0);
                    else TextBox(products[i].Amount + "x " + LS.Object(products[i].ObjectName), width: Width, show: false, margin: 0);
                }

            GroupEnd();
            GroupEnd();

            if (focus)
            {
                PrintList();
                UpdateSelect(selected, selected, products.Count > 0 ? products.Count : 1, rangeProp: 15);
            }
            else DisableView();

            RightList = ComponentList.ToList();
            LastDisplayedComponent = GetComponentByName("TB", 2, products.Count > 0 ? products.Count : 1);
        }

        public static void GetLeftView() => LeftList = ComponentList.ToList();

        public static void GetRightView() => RightList = ComponentList.ToList();

        public static void SetLeftView() => ComponentList = LeftList.ToList();

        public static void SetRightView() => ComponentList = RightList.ToList();
    }
}
