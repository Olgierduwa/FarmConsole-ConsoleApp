using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controlers
{
    class MainController
    {
        protected static GameInstanceModel GameInstance;
        protected static ActionModel Action;

        public static DateTime Previously = DateTime.Now;
        public static double FrameTime = 10;
        public static bool PlayerMovementAxis; 
        public static bool MapMovementAxis; 
        public static bool FieldNameVisibility; 

        public static string OpenScreen = "Menu";
        public static string LastScreen = "Menu";
        public static string EscapeScreen = "Menu";

        public static string POPUPTEXT = "";
        public static int POPUPSTAGE = -1;
        public static int POPUPID = 0;

        public static void PopUp(int id, string text)
        {
            string popupText = text;
            if (id > 0) popupText = StringService.Get((400 + id).ToString());
            if (POPUPSTAGE >= 0)
            {
                AnimationController.Effect(GV: /*ComponentEngine.TextBoxView(popupText)*/ null, S: POPUPSTAGE);
                POPUPSTAGE = AnimationController.PopUp();
            }
            if (POPUPSTAGE < 0) POPUPID = 0;
        }

        static MainController()
        {
            GameInstance = new GameInstanceModel();
            Action = new ActionModel();
        }
    }
}
