using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace FarmConsole.Body.Models
{
    public class FieldModel : ObjectModel
    {
        public short Duration { get; set; }
        public short ArrivalDirection { get; set; }
        public short BaseID { get; set; }
        public ViewModel BaseView { get; set; }
        public ContainerModel Pocket { get; set; }
        public bool New { get; set; }

        public override string ToString()
        {
            return ConvertService.ConvertFieldToString(Category, State, Type, Duration);
        }
        public FieldModel() { }
        public FieldModel(string _ModelAsString, int _Scale)
        {
            short[] Values = ConvertService.ConvertStringToField(_ModelAsString);
            Category = Values[0];
            State = Values[1];
            Type = Values[2];
            Duration = Values[3];
            Scale = _Scale;
            New = true;
        }
    }
}
