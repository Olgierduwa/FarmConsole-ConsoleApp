using FarmConsole.View;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace FarmConsole.Model
{
    public static class OPTIONS
    {
        private static int optionsCount = 4;
        private static int[] options = loadOptions();
        private static int[] optionsView = loadOptionView();
        private static int[] loadOptions()
        {
            return XF.getOption();
        }
        private static int[] loadOptionView()
        {
            int[] opt = new int[optionsCount];

            opt[0] = (options[0]-120)/20;
            opt[1] = (options[1]-35)/5;
            opt[2] = options[2];
            opt[3] = options[3];

            return opt;
        }
        public static void saveOptions(int[] opt)
        {
            options[0] = (opt[0]*20)+120;
            options[1] = (opt[1]*5)+35;
            options[2] = opt[2];
            options[3] = opt[3];

            XF.saveOptions(options);
            WindowMenager.setWindow();
        }
        public static int getOptionsCount()
        {
            return optionsCount;
        }
        public static int getOptionById(int id)
        {
            return options[id];
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
