using FarmConsole.Body.Services;
using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Models
{
    public class SettingModel
    {
        public SettingModel(string _key, int _default, int _value, int _max, int _offset)
        {
            Key = _key;
            DefaultValue = _default;
            SliderValue = _value;
            MaxSliderValue = _max;
            Offset = _offset;
            if (Key == "set screen width")
                RealValue = (SliderValue + Offset) * SettingsService.MaxWindowWidth / (MaxSliderValue + Offset);
            else if (Key == "set screen height")
                RealValue = (SliderValue + Offset) * SettingsService.MaxWindowHeihgt / (MaxSliderValue + Offset);
            else RealValue = SliderValue;
        }
        public override string ToString()
        {
            return "<setting key='" + Key +
                "' default='" + DefaultValue +
                "' value='" + SliderValue +
                "' max='" + MaxSliderValue +
                "' offset='" + Offset + "'/>";
        }

        public string Key { get; set; }
        public string Name { get; set; }

        private int Offset { get; set; }
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
                if (Key == "set screen width")
                    RealValue = (SliderValue + Offset) * SettingsService.MaxWindowWidth / (MaxSliderValue + Offset);
                else if(Key == "set screen height")
                    RealValue = (SliderValue + Offset) * SettingsService.MaxWindowHeihgt / (MaxSliderValue + Offset);
                else RealValue = SliderValue;
                return true;
            }
            return false;
        }
        public void SetDefaultValue()
        {
            SliderValue = DefaultValue;
            if (Key == "set screen width")
                RealValue = (SliderValue + Offset) * SettingsService.MaxWindowWidth / (MaxSliderValue + Offset);
            else if(Key == "set screen height")
                RealValue = (SliderValue + Offset) * SettingsService.MaxWindowHeihgt / (MaxSliderValue + Offset);
            else RealValue = SliderValue;
        }
    }
}
