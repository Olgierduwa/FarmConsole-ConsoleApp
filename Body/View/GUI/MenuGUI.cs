using FarmConsole.Body.Model.Helpers;
using FarmConsole.Body.Model.Objects;
using FarmConsole.Body.View.Components;
using System;

namespace FarmConsole.Body.View.GUI
{
    public static partial class GUI
    {
        public static readonly string title = XF.GetString(0);
        public static readonly string foot = XF.GetString(1);

        public static readonly string controlsText = XF.GetText(1);
        public static readonly string fromAuthorText = XF.GetText(2);
        public static readonly string exitQuestion = XF.GetText(100);
        public static readonly string deleteQuestion = XF.GetText(101);
        public static readonly string updateQuestion = XF.GetText(102);
        public static readonly string exitQuestion3 = XF.GetText(103);

        public static ViewTools vt1 = new ViewTools();
        public static ViewTools vt2 = new ViewTools();
        public static ViewTools vt3 = new ViewTools();

        public static void Menu()
        {
            vt1.H1(title);
            vt1.Endl((Console.WindowHeight-20)/2);

            vt1.GroupStart(0);
            vt1.GroupStart(2);
            vt1.Endl(1);
            vt1.TextBox("Zacznij Rozgrywke");
            vt1.TextBox("Kontynuuj Rozrywke");
            vt1.TextBox("Ustaw Wlasne Opcje");
            vt1.TextBox("Poznaj Zasady Gry");
            vt1.GroupEnd();
            vt1.GroupStart(4);
            vt1.Endl(1);
            vt1.TextBox("\"Swietny Tytul!!\"");
            vt1.TextBox("\"Oto najlpesza gra konsolowa, o jakiej slyszal swiat!\"");
            vt1.GroupEnd();
            vt1.GroupStart(3);
            vt1.Endl(2);
            vt1.TextBox(exitQuestion, 33, false, ConsoleColor.Red);
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
            vt1.TextBox("Kontynuuj");
            vt1.TextBox("Zapisz Gre");
            vt1.TextBox("Wróc do Menu");
            vt1.TextBox("Ustawienia");
            vt1.TextBox("Samouczek");

            vt1.GroupStart(3);
            vt1.Endl(4);
            vt1.TextBox(exitQuestion, 33, false, ConsoleColor.Red);
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
            vt1.TextBox("Kobieta");
            vt1.TextBox("Mezczyzna");
            vt1.GroupEnd();
            vt1.DoubleButtonBot(3,"Q / Powrot","E / Enter");
            vt1.Foot(foot);
            vt1.PrintList();
            //vt1.showComponentList();
        }
        public static void Load(Save[] saves)
        {

            int savesCount = saves.Length;
            int freeSpace = (Console.WindowHeight - 17);
            int showCount = freeSpace / 3;
            int endlCount = 2;
            if (showCount >= savesCount+1) endlCount += ((showCount - savesCount + 1) * 3 / 2);

            vt1.H1(title);
            vt1.H2("Kontynuuj Rozgrywkę");
            vt1.Endl(endlCount);

            vt1.GroupStart(0);
            vt1.GroupStart(2);
            vt1.Endl(1);
            vt1.TextBox("P U S T Y   Z A P I S");
            for (int i = 0; i < savesCount; i++)
                if (i < showCount) vt1.TextBox(saves[i].name);
                else vt1.TextBox(saves[i].name, 40, false);
            vt1.GroupEnd();

            vt1.GroupStart(4);
            vt1.Endl(1);
            vt1.TextBox("E / Rozpocznij Nową Grę");
            vt1.Endl(11);
            vt1.DoubleButton("D / Usun Gre", "E / Kontynuuj", false);
            vt1.GroupEnd();

            vt1.GroupStart(3);
            vt1.Endl(5);
            vt1.TextBox(deleteQuestion, 33, false ,ConsoleColor.Red);
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
            int savesCount = saves.Length;
            int freeSpace = (Console.WindowHeight - 17);
            int showCount = freeSpace / 3;
            int endlCount = 2;
            if (showCount >= savesCount + 1) endlCount += ((showCount - savesCount + 1) * 3 / 2);

            vt1.H1(title);
            vt1.H2("Zapisz Giereczke");
            vt1.Endl(endlCount);

            vt1.GroupStart(0);
            vt1.GroupStart(2);
            vt1.Endl(1);
            vt1.TextBox( "P U S T Y   Z A P I S");
            for (int i = 0; i < savesCount; i++)
                if (i < showCount)
                    vt1.TextBox(saves[i].name);
                else vt1.TextBox(saves[i].name, show: false);
            vt1.GroupEnd();

            vt1.GroupStart(4);
            vt1.Endl(1);
            vt1.TextBox("E / Utwórz Nowy Zapis");
            vt1.Endl(11);
            vt1.TextBox("E / Nadpisz Gre", show: false);
            vt1.GroupEnd();

            vt1.GroupStart(3);
            vt1.Endl(5);
            vt1.TextBox(updateQuestion, 33,  false, ConsoleColor.Red);
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
            string[] names = XF.GetOptionsNames();
            int optionsCount = OPTIONS.GetOptionsCount();
            int freeSpace = (Console.WindowHeight - 18);
            int showCount = freeSpace / 3;
            int endlCount = 2;
            if (showCount > optionsCount) endlCount += (showCount - optionsCount)*3/2;

            vt1.H1(title);
            vt1.H2("Ustaw Spersonalizowane Opcje");
            vt1.Endl(endlCount);
            vt1.GroupStart(0);

                vt1.GroupStart(2);
                vt1.Endl(1);
                for (int i = 0; i < optionsCount; i++)
                if (i <= showCount) vt1.TextBox(names[i]);
                else vt1.TextBox(names[i], show: false);

                if (optionsCount < showCount) vt1.TextBox("Przywróć Domyślne Ustawienia");
                else vt1.TextBox("Przywróć Domyślne Ustawienia", show:false);
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
            vt1.DoubleButtonBot(3, "Q / Odrzuć", "E / Zapisz");

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
                vt1.TextBox("Sterowanie",44);
                vt1.Endl(1);
                vt1.TextBox(controlsText,44);
                vt1.GroupEnd();
                vt1.GroupStart(4);
                vt1.TextBox("Od Autora",44);
                vt1.Endl(1);
                vt1.TextBox(fromAuthorText,44);
                vt1.GroupEnd();
            vt1.GroupEnd();

            vt1.Foot(foot);
            vt1.PrintList();
            //vt.showComponentList();
        }
    }
}
