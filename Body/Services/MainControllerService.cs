using FarmConsole.Body.Controlers.MenuControlers;
using FarmConsole.Body.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmConsole.Body.Services
{
    public class MainControllerService
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
                AnimationController.Effect(GV: MainViewService.TextBoxView(popupText), S: POPUPSTAGE);
                POPUPSTAGE = AnimationController.PopUp();
            }
            if (POPUPSTAGE < 0) POPUPID = 0;
        }

        static MainControllerService()
        {
            GameInstance = new GameInstanceModel();
        }
    }
}
