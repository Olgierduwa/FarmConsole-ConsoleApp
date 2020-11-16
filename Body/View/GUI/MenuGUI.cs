using FarmConsole.Body.Model.Helpers;
using FarmConsole.Body.Model.Logic;
using FarmConsole.Body.Model.Objects;
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
            vt1.h1(title);
            vt1.endl(Console.WindowHeight / 5);

            vt1.groupStart(0);
            vt1.groupStart(2);
            vt1.endl(1);
            vt1.singleButton("Zacznij Rozgrywke ");
            vt1.singleButton("Kontynuuj Rozrywke");
            vt1.singleButton("Ustaw Wlasne Opcje");
            vt1.singleButton("Poznaj Zasady Gry ");
            vt1.groupEnd();
            vt1.groupStart(4);
            vt1.endl(1);
            vt1.textBox(32, "\"Swietny Tytul!!\"");
            vt1.textBox(32, "\"Oto najlpesza gra konsolowa, o jakiej slyszal swiat!\"");
            vt1.groupEnd();
            vt1.groupStart(3);
            vt1.endl(2);
            vt1.textBox(33, "!!  Ś W I A D O M I E  !! " +
                            "!!  O P U S Z C Z A M  !! " +
                            "!!  R O Z G R Y W K Ę  !!", false, ConsoleColor.Red);
            vt1.endl(1);
            vt1.doubleButton(" Q / NIE ", " E / TAK ", false, ConsoleColor.Red);
            vt1.groupEnd();
            vt1.groupEnd();

            vt1.foot(foot);
            //vt1.showComponentList();
            vt1.printList();
        }
        public static void Escape()
        {
            vt1.h1(title);
            vt1.endl(Console.WindowHeight / 5);

            vt1.groupStart(0);
            vt1.groupStart(3);
            vt1.endl(1);
            vt1.singleButton("  Kontynuuj ");
            vt1.singleButton(" Zapisz Gre ");
            vt1.singleButton("Wróc do Menu");
            vt1.singleButton(" Ustawienia ");
            vt1.singleButton("  Samouczek ");

            vt1.groupStart(3);
            vt1.endl(4);
            vt1.textBox(33, "!!  Ś W I A D O M I E  !! " +
                            "!!  O P U S Z C Z A M  !! " +
                            "!!  R O Z G R Y W K Ę  !!", false, ConsoleColor.Red);
            vt1.endl(1);
            vt1.doubleButton(" Q / NIE ", " E / TAK ", false, ConsoleColor.Red);
            vt1.groupEnd();

            vt1.groupEnd();
            vt1.groupEnd();

            vt1.foot(foot);
            vt1.printList();
            //vt1.showComponentList();
        }
        public static void NewGame()
        {
            vt1.h1(title);
            vt1.h2("Rozpocznij Nowa Giereczke");
            vt1.endl(Console.WindowHeight / 5);
            vt1.groupStart(3);
            vt1.endl(1);
            vt1.singleButton("Kobieta  ");
            vt1.singleButton("Mezczyzna");
            vt1.groupEnd();
            vt1.doubleButtonBot(3,"Q / Powrot","E / Enter");
            vt1.foot(foot);
            vt1.printList();
            //vt1.showComponentList();
        }
        public static void Load(Save[] saves)
        {
            vt1.h1(title);
            vt1.h2("Wczytaj Giereczke");
            vt1.endl(2);

            vt1.groupStart(3);
            vt1.singleButton("WYBIERZ ZAPIS");
            vt1.groupEnd();

            vt1.groupStart(0);

            vt1.groupStart(2);
            vt1.endl(1);
            vt1.textBox(44, "P U S T Y   Z A P I S");
            for (int i = 0; i < saves.Length; i++)
                if (i < (Console.WindowHeight - 20) / 3)
                    vt1.textBox(44, saves[i].name);
                else vt1.textBox(44, saves[i].name, false);
            vt1.groupEnd();

            vt1.groupStart(4);
            vt1.endl(1);
            vt1.textBox(44, "E / Rozpocznij Nową Grę");
            vt1.endl(11);
            vt1.doubleButton("D / Usuń Zapis", "E / Wczytaj Grę", false);
            vt1.groupEnd();

            vt1.groupStart(3);
            vt1.endl(5);
            vt1.textBox(33, "B E Z S P R Z E C Z N I E " +
                            "P R A G N Ę   U S U N Ą Ć " +
                            "T E N   Z A P I S   G R Y " +
                            "T R W A L E   T R A C Ą C " +
                            "O D T W A R Z A L N O Ś Ć", false ,ConsoleColor.Red);
            vt1.endl(1);
            vt1.doubleButton(" Q / NIE ", " E / TAK ", false, ConsoleColor.Red);
            vt1.groupEnd();

            vt1.groupEnd();
            vt1.foot(foot);
            vt1.printList();
            //vt1.showComponentList();
        }
        public static void Save(Save[] saves)
        {
            vt1.h1(title);
            vt1.h2("Zapisz Giereczke");
            vt1.endl(2);

            vt1.groupStart(3);
            vt1.singleButton("WYBIERZ ZAPIS");
            vt1.groupEnd();

            vt1.groupStart(0);

            vt1.groupStart(2);
            vt1.endl(1);
            vt1.textBox(44, "P U S T Y   Z A P I S");
            for (int i = 0; i < saves.Length; i++)
                if (i < (Console.WindowHeight - 20) / 3)
                    vt1.textBox(44, saves[i].name);
                else vt1.textBox(44, saves[i].name, false);
            vt1.groupEnd();

            vt1.groupStart(4);
            vt1.endl(1);
            vt1.textBox(44, "E / Utwórz Nowy Zapis");
            vt1.endl(11);
            vt1.singleButton("E / Nadpisz Gre", false);
            vt1.groupEnd();

            vt1.groupStart(3);
            vt1.endl(5);
            vt1.textBox(33, "B E Z S P R Z E C Z N I E " +
                            "C H C Ę   N A D P I S A Ć " +
                            "T E N   Z A P I S   G R Y " +
                            "T R W A L E   T R A C Ą C " +
                            "O D T W A R Z A L N O Ś Ć", false, ConsoleColor.Red);
            vt1.endl(1);
            vt1.doubleButton(" Q / NIE ", " E / TAK ", false, ConsoleColor.Red);
            vt1.groupEnd();

            vt1.groupEnd();
            vt1.foot(foot);
            vt1.printList();
            //vt1.showComponentList();
        }
        public static void Options() 
        {
            vt1.h1(title);
            vt1.h2("Ustaw Spersonalizowane Opcje");
            vt1.endl(Console.WindowHeight / 6);
            vt1.groupStart(0);
            vt1.groupStart(2);
            vt1.endl(1);
            vt1.textBox(40,"Szerokosc Ekranu");
            vt1.textBox(40,"Wysokosc Ekranu");
            vt1.textBox(40,"Wielkosc Czcionki");
            vt1.textBox(40,"Poziom trudnosc");
            vt1.textBox(40,"Przywróć Domyślne Ustawienia");
            //vt1.textBox(40,"Test Opcja");
            vt1.groupEnd();
            vt1.groupStart(4);
            vt1.endl(1);
            vt1.slider(6, OPTIONS.getOptionViewById(0));
            vt1.slider(6, OPTIONS.getOptionViewById(1));
            vt1.slider(6, OPTIONS.getOptionViewById(2));
            vt1.slider(6, OPTIONS.getOptionViewById(3));
            //vt1.slider(6, OPTIONS.getOptionViewById(4));
            vt1.groupEnd();
            vt1.groupEnd();
            vt1.doubleButtonBot(3, "Q / Powrot", "E / Enter");

            vt1.foot(foot);
            vt1.printList();
            //vt.showComponentList();
        }
        public static void Help()
        {
            vt1.h1(title);
            vt1.h2("Nie wiesz o co chodzi?");
            vt1.endl(Console.WindowHeight / 9);
            vt1.groupStart(0);
                vt1.groupStart(2);
                vt1.textBox(44,"Sterowanie");
                vt1.endl(1);
                vt1.textBox(44, XF.GetText(1));
                vt1.groupEnd();
                vt1.groupStart(4);
                vt1.textBox(44, "Od Autora");
                vt1.endl(1);
                vt1.textBox(44, XF.GetText(2));
                vt1.groupEnd();
            vt1.groupEnd();

            vt1.foot(foot);
            vt1.printList();
            //vt.showComponentList();
        }
    }
}
