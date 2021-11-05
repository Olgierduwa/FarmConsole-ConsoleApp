using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FarmConsole.Body.Models
{
    class ColorModel
    {
        public int ColorID;
        public string ColorName;
        public Color Color;

        public ColorModel(string ColorString)
        {
            string[] ColorValues = ColorString.Split(new char[] { ' ' });
            byte R = Convert.ToByte(ColorValues[2]);
            byte G = Convert.ToByte(ColorValues[3]);
            byte B = Convert.ToByte(ColorValues[4]);
            ColorID = Convert.ToInt16(ColorValues[0]);
            ColorName = ColorValues[1];
            Color = Color.FromArgb(R, G, B);
        }
    }
}
