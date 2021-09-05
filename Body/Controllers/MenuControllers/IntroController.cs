
using FarmConsole.Body.Controlers.MenuControlers;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controlers
{
    public class IntroController : MainControllerService
    {
        public static void Open()
        {
            AnimationController.SlideEffect(2, "purple");
            AnimationController.SlideEffect(3, "red");
            AnimationController.SlideEffect(0, "yellow");
            AnimationController.SlideEffect(3, "green");
            AnimationController.SlideEffect(1, "blue");
            AnimationController.SlideEffect(0, "white");
            AnimationController.CrossEffect(0, "red");
            AnimationController.CrossEffect(1, "green");
            AnimationController.CrossEffect(0, "blue");
            AnimationController.CrossEffect(1, "purple");
            AnimationController.CrossEffect(0, "white");
        }
    }
}
