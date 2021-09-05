using FarmConsole.Body.Resources.Sounds;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FarmConsole.Body.Views.MenuViews
{
    public class MenuCenterView : MainViewService
    {
        public static void Show(string type, string name, int duration = 0, string opis = "")
        {
            ClearList(false);
            Endl(10);
            GroupStart(3);

            string[] text, text2;

            switch (type)
            {
                case "warrning":
                    text = new string[] { " ", "OSTRZEZENIE!", " " };
                    text2 = TextBoxView(name, 30);
                    text = text.Concat(text2.Concat(new string[] { " " }).ToArray()).ToArray();
                    TextBoxLines(text, color1: ConsoleColor.Red); break;
                case "field":
                    text = new string[] {
                        " ", "Nazwa Pola: " + name,
                        " ", "Czas Trwania: " + duration.ToString() + " dni",
                        " ", "Drobny Opis: ", " "
                    };
                    text2 = TextBoxView(opis, 30);
                    text = text.Concat(text2.Concat(new string[] { " " }).ToArray()).ToArray();
                    TextBoxLines(text);
                    break;
                case "item":
                    text = new string[] {
                        " ", "Nazwa Przedmiotu: " + name,
                        " ", "Posiadana Ilosc: " + duration.ToString() + "x",
                        " ", "Drobny Opis: ", " "
                    };
                    text2 = TextBoxView(opis, 30);
                    text = text.Concat(text2.Concat(new string[] { " " }).ToArray()).ToArray();
                    TextBoxLines(text);
                    break;
                default: TextBox("Text Box Info"); break;
            }

            GroupEnd();
            PrintList();
        }

        public static void Clean()
        {
            ClearList(false);
            Endl(5);
            GroupStart(3);
            CenterBar();
            GroupEnd();
            ClearList();
        }
    }
}
