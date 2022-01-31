using FarmConsole.Body.Models;
using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FarmConsole.Body.Views.GameViews
{
    class StoveView : ComponentService
    {
        private static CM LastDisplayedComponent;

        public static void DisplayCaptains(string Action, List<ProductModel> Products, int Selected, bool Cleaning = false, bool IsSelected = true)
        {
            if (Cleaning && View != null && LastDisplayedComponent != null)
                View.DisplayPixels(LastDisplayedComponent.Pos, LastDisplayedComponent.Size);

            ClearList(false);

            Endl(3);
            H2(LS.Navigation(Action + " label"));
            Endl(2);
            GroupStart(0);

            GroupStart(1, 2);
            Endl(4);
            int Height = Console.WindowHeight - 23;
            for (int i = 0; i < Products.Count; i++)
            {
                if (Products[i] != null)
                {
                    string StateName = Products[i].StateName.Length > 0 && Products[i].StateName[0] > '@' ? LS.Object(Products[i].StateName) + " " : "";
                    if (i < Height / 3) 
                         TextBox(StateName + LS.Object(Products[i].ObjectName) + " : " + Products[i].Amount +
                         LS.Navigation(Products[i].Unit.Split('/')[0], " ", Before: " "), Margin: 0);
                    else TextBox(StateName + LS.Object(Products[i].ObjectName) + " : " + Products[i].Amount +
                         LS.Navigation(Products[i].Unit.Split('/')[0], " ", Before: " "), Margin: 0, Show: false);
                }
            }
            GroupEnd();

            GroupStart(1, 2);
            TextBox(LS.Object("products"));
            Endl(Height + 2);
            TextBox(LS.Action(Action, " [E]").ToUpper(), Foreground: ColorService.GetColorByName("limeD"));
            TextBox(LS.Navigation("manage dishes", " [Q]"), Foreground: ColorService.GetColorByName("orange"));
            GroupEnd();

            GroupStart(2, 2);
            TextBox(LS.Object("stove"));
            Height = Console.WindowHeight - 27;
            Endl(15 - (Height / 3 < 4 ? Height / 3 * 3 : 0));
            TextBox(LS.Object("oven"));
            GroupEnd();

            GroupEnd();
            PrintList();
            if(IsSelected) UpdateMenuSelect(Selected, Selected, Products.Count);

            LastDisplayedComponent = GetComponentByName("TB", 2, Products.Count > 0 ? Products.Count : 1);
        }
        public static void DisplayStove(ProductModel[] products, int selected)
        {
            ClearList(false);
            Endl(5);
            GroupStart(0);

            int Height = Console.WindowHeight - 27;

            GroupStart(2, 2);
            Endl(6);
            for (int i = 0; i < 4; i++)
                if (i < Height / 3)
                {
                    if (products[i] != null) TextBox(LS.Object(products[i].ObjectName));
                    else TextBox("...");
                }
                else
                {
                    if (products[i] != null) TextBox(LS.Object(products[i].ObjectName), Show: false);
                    else TextBox("...", Show: false);
                }
            GroupEnd();

            GroupStart(2, 2);
            Endl(24 - (Height / 3 < 4 ? Height / 3 * 3 : 0));
            for (int i = 4; i < 6; i++)
                if (products[i] != null) TextBox(LS.Object(products[i].ObjectName));
                else TextBox("...");
            GroupEnd();

            GroupEnd();

            PrintList();
            UpdateSelect(selected, selected, products.Length);

            for (int i = 0; i < products.Length; i++)
                if (products[i] != null)
                    SetProgressBar(2 + (i > 3 ? 1 : 0), i + 1 - (i > 3 ? 4 : 0), products[i].Amount);
        }

        private static List<CM> LeftList;
        public static void GetLeftList() => LeftList = ComponentList.ToList();
        public static void SetLeftList() => ComponentList = LeftList.ToList();

        private static List<CM> RightList;
        public static void GetRightList() => RightList = ComponentList.ToList();
        public static void SetRightList() => ComponentList = RightList.ToList();
    }
}
