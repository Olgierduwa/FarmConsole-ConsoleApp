using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Views.GameViews
{
    class PlotExtendingView : ComponentService
    {
        public static void Display(int price, int count, string direction)
        {
            ClearList(false);

            int Height = Console.WindowHeight - 13;
            string c = LS.Navigation("currency");

            Endl(3);
            H2(LS.Navigation("plot extending label"));

            GroupStart(0);
            GroupStart(2);
            Endl(2);
            TextBox(LS.Navigation("direction of expansion"));
            Endl(1);
            TextBoxLines(LS.Text(direction).Split('\n'));
            
            Endl(1);
            TextBox(LS.Navigation("choose the direction", " [W] [A] [S] [D]"),
                foreground: ColorService.GetColorByName("cyan"));
            GroupEnd();

            GroupStart(4);
            Endl(2);
            TextBox(LS.Navigation("act of ownership"));
            Endl(1);
            TextBox(LS.Navigation("tile price", ": " + price + c));
            TextBox(LS.Navigation("tiles count", ": " + count + "x"));
            TextBox(LS.Navigation("amount to pay", ": " + (price * count) + c));
            Endl(3);
            TextBox(LS.Navigation("sign document and pay"," [E]"), foreground: ColorService.GetColorByName("limeD"));
            TextBox(LS.Navigation("do not sign document"," [Q]"), foreground: ColorService.GetColorByName("redL"));
            GroupEnd();
            GroupEnd();

            PrintList();
        }
    }
}
