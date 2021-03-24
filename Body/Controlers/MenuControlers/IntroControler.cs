
using FarmConsole.Body.Controlers.MenuControlers;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controlers
{
    public class IntroControler : MenuControlerService
    {
        public static void Open()
        {
            AnimationControler.SlideEffect(2, "purple");
            AnimationControler.SlideEffect(3, "red");
            AnimationControler.SlideEffect(0, "yellow");
            AnimationControler.SlideEffect(3, "green");
            AnimationControler.SlideEffect(1, "blue");
            AnimationControler.SlideEffect(0, "white");
            AnimationControler.CrossEffect(0, "red");
            AnimationControler.CrossEffect(1, "green");
            AnimationControler.CrossEffect(0, "blue");
            AnimationControler.CrossEffect(1, "purple");
            AnimationControler.CrossEffect(0, "white");
        }
    }
}
