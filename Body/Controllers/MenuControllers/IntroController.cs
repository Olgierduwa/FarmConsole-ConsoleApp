using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controlers
{
    class IntroController : MainController
    {
        public static void Open()
        {
            AnimationController.Effect("VerticalSlideEffect", GN: "blant", CID: ColorService.PurpleEffect);
            AnimationController.Effect("HorizontalSlideEffect", GN: "dreamworks", CID: ColorService.RedEffect);
            AnimationController.Effect("VerticalSlideEffect", GN: "farmconsole", CID: ColorService.YellowEffect);
            AnimationController.Effect("HorizontalSlideEffect", GN: "dreamworks", CID: ColorService.GreenEffect);
            AnimationController.Effect("VerticalSlideEffect", GN: "potion", CID: ColorService.BlueEffect);
            AnimationController.Effect("HorizontalSlideEffect", GN: "farmconsole");

            AnimationController.Effect("CrossEffect", GN: "farmconsole", CID: ColorService.RedEffect);
            AnimationController.Effect("CrossEffect", GN: "potion", CID: ColorService.GreenEffect);
            AnimationController.Effect("CrossEffect", GN: "farmconsole", CID: ColorService.BlueEffect);
            AnimationController.Effect("CrossEffect", GN: "potion", CID: ColorService.PurpleEffect);
            AnimationController.Effect("CrossEffect", GN: "farmconsole", CID: ColorService.RedEffect);
        }
    }
}
