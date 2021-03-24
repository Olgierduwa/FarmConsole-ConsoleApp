using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FarmConsole.Body.Services
{
    public static class SettingsService
    {
        private static readonly int optionsCount = XF.GetOptionsCount();
        private static int[] currentOptions = LoadOptions();
        private static int[] optionsView = LoadOptionView();
        private static int[] LoadOptions()
        {
            return XF.GetOptions();
        }
        private static int[] LoadOptionView()
        {
            int[] opt = new int[optionsCount];

            opt[0] = (currentOptions[0] - 120) / 20;
            opt[1] = (currentOptions[1] - 35) / 5;
            opt[2] = currentOptions[2];
            opt[3] = currentOptions[3];
            //opt[4] = currentOptions[4];

            return opt;
        }
        public static void SaveOptions(int[] opt)
        {
            currentOptions[0] = opt[0] * 20 + 120;
            currentOptions[1] = opt[1] * 5 + 35;
            currentOptions[2] = opt[2];
            currentOptions[3] = opt[3];
            //currentOptions[4] = opt[4];

            XF.UpdateOptions(currentOptions);
            WindowService.SetWindow();
        }
        public static void ResetOptions()
        {
            XF.UpdateOptions(new int[] { });
            currentOptions = LoadOptions();
            optionsView = LoadOptionView();
            WindowService.SetWindow();
        }
        public static int GetOptionsCount()
        {
            return optionsCount;
        }
        public static int GetOptionById(int id)
        {
            return currentOptions[id];
        }
        public static int GetOptionViewById(int id)
        {
            return optionsView[id];
        }
        public static int[] GetOptionsView()
        {
            optionsView = LoadOptionView();
            return optionsView;
        }
    }
}
