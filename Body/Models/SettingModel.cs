using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Models
{
    public class SettingModel
    {
        public SettingModel(string _key, int _default, int _value, int _max, int _multi, int _offset)
        {
            Key = _key;

            DefaultValue = _default;
            SliderValue = _value;
            MaxSliderValue = _max;
            Multiplier = _multi;
            Offset = _offset;
            RealValue = SliderValue * Multiplier + Offset;
        }
        public override string ToString()
        {
            return "<setting key='" + Key +
                "' default='" + DefaultValue +
                "' value='" + SliderValue +
                "' max='" + MaxSliderValue +
                "' multiplier='" + Multiplier +
                "' offset='" + Offset + "'/>";
        }

        public string Key { get; set; }
        public string Name { get; set; }

        private int Offset { get; set; }
        private int Multiplier { get; set; }
        private int MaxSliderValue { get; set; }
        private int DefaultValue { get; set; }
        private int RealValue { get; set; }
        private int SliderValue { get; set; }

        public int GetRealValue => RealValue;
        public int GetSliderValue => SliderValue;
        public int GetMaxSliderValue => MaxSliderValue;
        public bool SetSliderValue(int value)
        {
            if(SliderValue + value >= 0 && SliderValue + value <= MaxSliderValue)
            {
                SliderValue += value;
                RealValue = SliderValue * Multiplier + Offset;
                return true;
            }
            return false;
        }
        public void SetDefaultValue()
        {
            SliderValue = DefaultValue;
            RealValue = SliderValue * Multiplier + Offset;
        }
    }
}
