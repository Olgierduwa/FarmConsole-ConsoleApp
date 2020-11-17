using FarmConsole.Body.Model.Helpers;
using FarmConsole.Body.Model.Objects;
using FarmConsole.Body.View.Components;
using System;

namespace FarmConsole.Body.View.GUI
{
    public static partial class GUI
    {
        public static string title = XF.GetString(0);
        public static string foot = XF.GetString(1);
        public static ViewTools vt1 = new ViewTools();
        public static ViewTools vt2 = new ViewTools();
        public static ViewTools vt3 = new ViewTools();

        public static void Menu()
        {
            vt1.H1(title);
            vt1.Endl(Console.WindowHeight / 5);

            vt1.GroupStart(0);
            vt1.GroupStart(2);
            vt1.Endl(1);
            vt1.TextBox(40,"Zacznij Rozgrywke");
            vt1.TextBox(40,"Kontynuuj Rozrywke");
            vt1.TextBox(40,"Ustaw Wlasne Opcje");
            vt1.TextBox(40,"Poznaj Zasady Gry");
            vt1.GroupEnd();
            vt1.GroupStart(4);
            vt1.Endl(1);
            vt1.TextBox(40, "\"Swietny Tytul!!\"");
            vt1.TextBox(40, "\"Oto najlpesza gra konsolowa, o jakiej slyszal swiat!\"");
            vt1.GroupEnd();
            vt1.GroupStart(3);
            vt1.Endl(2);
            vt1.TextBox(33, "!!  Ś W I A D O M I E  !! " +
                            "!!  O P U S Z C Z A M  !! " +
                            "!!  R O Z G R Y W K Ę  !!", false, ConsoleColor.Red);
            vt1.Endl(1);
            vt1.DoubleButton(" Q / NIE ", " E / TAK ", false, ConsoleColor.Red);
            vt1.GroupEnd();
            vt1.GroupEnd();

            vt1.Foot(foot);
            //vt1.showComponentList();
            vt1.PrintList();
        }
        public static void Escape()
        {
            vt1.H1(title);
            vt1.Endl(Console.WindowHeight / 5);

            vt1.GroupStart(0);
            vt1.GroupStart(3);
            vt1.Endl(1);
            vt1.TextBox(40,"Kontynuuj");
            vt1.TextBox(40,"Zapisz Gre");
            vt1.TextBox(40,"Wróc do Menu");
            vt1.TextBox(40,"Ustawienia");
            vt1.TextBox(40,"Samouczek");

            vt1.GroupStart(3);
            vt1.Endl(4);
            vt1.TextBox(33, "!!  Ś W I A D O M I E  !! " +
                            "!!  O P U S Z C Z A M  !! " +
                            "!!  R O Z G R Y W K Ę  !!", false, ConsoleColor.Red);
            vt1.Endl(1);
            vt1.DoubleButton(" Q / NIE ", " E / TAK ", false, ConsoleColor.Red);
            vt1.GroupEnd();

            vt1.GroupEnd();
            vt1.GroupEnd();

            vt1.Foot(foot);
            vt1.PrintList();
            //vt1.showComponentList();
        }
        public static void NewGame()
        {
            vt1.H1(title);
            vt1.H2("Rozpocznij Nowa Giereczke");
            vt1.Endl(Console.WindowHeight / 5);
            vt1.GroupStart(3);
            vt1.Endl(1);
            vt1.SingleButton("Kobieta  ");
            vt1.SingleButton("Mezczyzna");
            vt1.GroupEnd();
            vt1.DoubleButtonBot(3,"Q / Powrot","E / Enter");
            vt1.Foot(foot);
            vt1.PrintList();
            //vt1.showComponentList();
        }
        public static void Load(Save[] saves)
        {
            vt1.H1(title);
            vt1.H2("Wczytaj Giereczke");
            vt1.Endl(2);

            vt1.GroupStart(3);
            vt1.SingleButton("WYBIERZ ZAPIS");
            vt1.GroupEnd();

            vt1.GroupStart(0);

            vt1.GroupStart(2);
            vt1.Endl(1);
            vt1.TextBox(40, "P U S T Y   Z A P I S");
            for (int i = 0; i < saves.Length; i++)
                if (i < (Console.WindowHeight - 20) / 3)
                    vt1.TextBox(40, saves[i].name);
                else vt1.TextBox(40, saves[i].name, false);
            vt1.GroupEnd();

            vt1.GroupStart(4);
            vt1.Endl(1);
            vt1.TextBox(40, "E / Rozpocznij Nową Grę");
            vt1.Endl(11);
            vt1.DoubleButton("D / Usuń Zapis", "E / Wczytaj Grę", false);
            vt1.GroupEnd();

            vt1.GroupStart(3);
            vt1.Endl(5);
            vt1.TextBox(33, "B E Z S P R Z E C Z N I E " +
                            "P R A G N Ę   U S U N Ą Ć " +
                            "T E N   Z A P I S   G R Y " +
                            "T R W A L E   T R A C Ą C " +
                            "O D T W A R Z A L N O Ś Ć", false ,ConsoleColor.Red);
            vt1.Endl(1);
            vt1.DoubleButton(" Q / NIE ", " E / TAK ", false, ConsoleColor.Red);
            vt1.GroupEnd();

            vt1.GroupEnd();
            vt1.Foot(foot);
            vt1.PrintList();
            //vt1.showComponentList();
        }
        public static void Save(Save[] saves)
        {
            vt1.H1(title);
            vt1.H2("Zapisz Giereczke");
            vt1.Endl(2);

            vt1.GroupStart(3);
            vt1.SingleButton("WYBIERZ ZAPIS");
            vt1.GroupEnd();

            vt1.GroupStart(0);

            vt1.GroupStart(2);
            vt1.Endl(1);
            vt1.TextBox(40, "P U S T Y   Z A P I S");
            for (int i = 0; i < saves.Length; i++)
                if (i < (Console.WindowHeight - 20) / 3)
                    vt1.TextBox(40, saves[i].name);
                else vt1.TextBox(40, saves[i].name, false);
            vt1.GroupEnd();

            vt1.GroupStart(4);
            vt1.Endl(1);
            vt1.TextBox(40, "E / Utwórz Nowy Zapis");
            vt1.Endl(11);
            vt1.SingleButton("E / Nadpisz Gre", 0, false);
            vt1.GroupEnd();

            vt1.GroupStart(3);
            vt1.Endl(5);
            vt1.TextBox(33, "E K W I W A L E N T N I E " +
                            "N A D P I S U J Ę   T E N " +
                            "W Y B R A N Y   Z A P I S " +
                            "T Y M Ż E   Z A P I S E M " +
                            "A B Y   U Z U P E Ł N I Ć " +
                            "K R Z T A Ł T O W A N I E", false, ConsoleColor.Red);
            vt1.Endl(1);
            vt1.DoubleButton(" Q / NIE ", " E / TAK ", false, ConsoleColor.Red);
            vt1.GroupEnd();

            vt1.GroupEnd();
            vt1.Foot(foot);
            vt1.PrintList();
            //vt1.showComponentList();
        }
        public static void Options() 
        {
            vt1.H1(title);
            vt1.H2("Ustaw Spersonalizowane Opcje");
            vt1.Endl(Console.WindowHeight / 6);
            vt1.GroupStart(0);
            vt1.GroupStart(2);
            vt1.Endl(1);
            vt1.TextBox(40,"Szerokosc Ekranu");
            vt1.TextBox(40,"Wysokosc Ekranu");
            vt1.TextBox(40,"Głośność dźwięków");
            vt1.TextBox(40,". . .");
            vt1.TextBox(40,"Przywróć Domyślne Ustawienia");
            //vt1.textBox(40,"Test Opcja");
            vt1.GroupEnd();
            vt1.GroupStart(4);
            vt1.Endl(1);
            vt1.Slider(6, OPTIONS.GetOptionViewById(0));
            vt1.Slider(6, OPTIONS.GetOptionViewById(1));
            vt1.Slider(6, OPTIONS.GetOptionViewById(2));
            vt1.Slider(6, OPTIONS.GetOptionViewById(3));
            //vt1.slider(6, OPTIONS.getOptionViewById(4));
            vt1.GroupEnd();
            vt1.GroupEnd();
            vt1.DoubleButtonBot(3, "Q / Powrot", "E / Enter");

            vt1.Foot(foot);
            vt1.PrintList();
            //vt.showComponentList();
        }
        public static void Help()
        {
            vt1.H1(title);
            vt1.H2("Nie wiesz o co chodzi?");
            vt1.Endl(Console.WindowHeight / 9);
            vt1.GroupStart(0);
                vt1.GroupStart(2);
                vt1.TextBox(44,"Sterowanie");
                vt1.Endl(1);
                vt1.TextBox(44, XF.GetText(1));
                vt1.GroupEnd();
                vt1.GroupStart(4);
                vt1.TextBox(44, "Od Autora");
                vt1.Endl(1);
                vt1.TextBox(44, XF.GetText(2));
                vt1.GroupEnd();
            vt1.GroupEnd();

            vt1.Foot(foot);
            vt1.PrintList();
            //vt.showComponentList();
        }
    }
}
