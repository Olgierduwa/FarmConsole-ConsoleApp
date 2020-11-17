using FarmConsole.Body.Model.Helpers;
using FarmConsole.Body.Model.Logic;
using System;
using System.Xml.Schema;

namespace FarmConsole
{
    class Program
    {
        static void Main()
        {
            WindowMenager.PresetWindow();
            new Logic();
        }
    }
}
