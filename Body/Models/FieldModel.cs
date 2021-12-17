using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace FarmConsole.Body.Models
{
    public class FieldModel : ObjectModel
    {
        public int Duration { get; set; }
        public int ArrivalDirection { get; set; }
        public int BaseID { get; set; }
        public ViewModel BaseView { get; set; }

        public override string ToString()
        {
            return ConvertService.ConvertFieldToString(Category, State, Type, Duration);
        }
        public FieldModel() { }
        public FieldModel(string _ModelAsString, int _Scale)
        {
            int[] Values = ConvertService.ConvertStringToField(_ModelAsString);
            Category = Values[0];
            State = Values[1];
            Type = Values[2];
            Duration = Values[3];
            Scale = _Scale;
            SetID();
        }
        public void ColorizeView(string _Type, int _Procent = 0)
        {
            var Views = new List<ViewModel>() { View };
            if (BaseView != null) Views.Add(BaseView);
            foreach (var view in Views)
                foreach (var word in view.GetWords())
                {
                    Color color = word.Color;
                    if (_Procent > 0)
                        switch (_Type)
                        {
                            case "Darker": color = ColorService.Darker(color); break;
                            case "Brighter": color = ColorService.Brighter(color); break;
                            case "Bluer": color = ColorService.Bluer(color); break;
                            case "Greener": color = ColorService.Greener(color); break;
                            case "Yellower": color = ColorService.Yellower(color); break;
                        }
                    else
                        switch (_Type)
                        {
                            case "Darker": color = ColorService.Darker(color, _Procent); break;
                            case "Brighter": color = ColorService.Brighter(color, _Procent); break;
                            case "Bluer": color = ColorService.Bluer(color, _Procent); break;
                            case "Greener": color = ColorService.Greener(color, _Procent); break;
                            case "Yellower": color = ColorService.Yellower(color, _Procent); break;
                        }
                    word.Color = color;
                }
        }
    }
}
