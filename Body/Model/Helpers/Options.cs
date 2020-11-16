using FarmConsole.Body.View.GUI;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FarmConsole.Body.Model.Helpers
{
    public static class OPTIONS
    {
        private static int optionsCount = 4;
        private static int[] currentOptions = loadOptions();
        private static int[] optionsView = loadOptionView();
        private static int[] loadOptions()
        {
            return XF.GetOptions();
        }
        private static int[] loadOptionView()
        {
            int[] opt = new int[optionsCount];

            opt[0] = (currentOptions[0] - 120) / 20;
            opt[1] = (currentOptions[1] - 35) / 5;
            opt[2] = currentOptions[2];
            opt[3] = currentOptions[3];
            //opt[4] = currentOptions[4];

            return opt;
        }
        public static void saveOptions(int[] opt)
        {
            currentOptions[0] = (opt[0] * 20) + 120;
            currentOptions[1] = (opt[1] * 5) + 35;
            currentOptions[2] = opt[2];
            currentOptions[3] = opt[3];
            //currentOptions[4] = opt[4];

            XF.UpdateOptions(currentOptions);
            WindowMenager.setWindow();
        }
        public static void resetOptions()
        {
            XF.UpdateOptions(new int[] { });
            currentOptions = loadOptions();
            optionsView = loadOptionView();
            WindowMenager.setWindow();
        }
        public static int getOptionsCount()
        {
            return optionsCount;
        }
        public static int getOptionById(int id)
        {
            return currentOptions[id];
        }
        public static int getOptionViewById(int id)
        {
            return optionsView[id];
        }
        public static int[] getOptionsView()
        {
            optionsView = loadOptionView();
            return optionsView;
        }
    }
}
