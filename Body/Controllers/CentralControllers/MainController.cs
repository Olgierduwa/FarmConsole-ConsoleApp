using FarmConsole.Body.Engines;
using FarmConsole.Body.Models;
using FarmConsole.Body.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Controlers
{
    public class MainController
    {
        protected static GameInstanceModel GameInstance;

        public static string POPUPTEXT = "";
        public static int POPUPSTAGE = -1;
        public static int POPUPID = 0;
        public static DateTime Now = DateTime.Now;
        public static string openScreen = "Menu";
        public static string lastScreen = "Menu";
        public static string escapeScreen = "Menu";

        public static void PopUp(int id, string text)
        {
            string popupText = text;
            if (id > 0) popupText = XF.GetString((400 + id).ToString());
            if (POPUPSTAGE >= 0)
            {
                AnimationController.Effect(GV: ComponentEngine.TextBoxView(popupText), S: POPUPSTAGE);
                POPUPSTAGE = AnimationController.PopUp();
            }
            if (POPUPSTAGE < 0) POPUPID = 0;
        }

        static MainController()
        {
            GameInstance = new GameInstanceModel();
        }
    }
}
