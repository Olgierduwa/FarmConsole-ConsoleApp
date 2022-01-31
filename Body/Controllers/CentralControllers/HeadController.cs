using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services.MainServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controllers.CentralControllers
{
    class HeadController
    {
        protected static GameInstanceModel GameInstance { get; set; }
        protected static ActionModel Action { get; set; }

        public static DateTime Previously = DateTime.Now;
        public static double FrameTime = 10;
        public static double BakeTime = 100;
        public static bool PlayerMovementAxis;
        public static bool MapMovementAxis;
        public static bool FieldNameVisibility;
        public static bool StatsVisibility;

        public static string OpenScreen = "Menu";
        public static string LastScreen = "Menu";
        public static string EscapeScreen = "Menu";

        public static string POPUPTEXT = "";
        public static int POPUPSTAGE = -1;
        public static int POPUPID = 0;

        public static void PopUp(int id, string text)
        {
            string popupText = text;
            if (id > 0) popupText = LS.Navigation((400 + id).ToString());
            if (POPUPSTAGE >= 0)
            {
                AnimationController.Effect(GV: /*ComponentEngine.TextBoxView(popupText)*/ null, S: POPUPSTAGE);
                POPUPSTAGE = AnimationController.PopUp();
            }
            if (POPUPSTAGE < 0) POPUPID = 0;
        }

        static HeadController()
        {
            Action = new ActionModel();
        }
    }
}
