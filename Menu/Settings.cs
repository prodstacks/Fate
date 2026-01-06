using Fate.Classes;
using UnityEngine;

namespace Fate
{
    public class Settings
    {
        /*
         * These are the settings for the menu.
         * 
         * To change the colors, you need to modify the ExtGradient variables.
         * Here are some examples on how to use ExtGradient:
         * 
         * Solid Color:
         *  new ExtGradient { colors = ExtGradient.GetSolidGradient(Color.black) }
         *  
         * Simple Gradient:
         *  new ExtGradient { colors = ExtGradient.GetSimpleGradient(Color.black, Color.white) }
         * 
         * Rainbow Color:
         *   new ExtGradient { rainbow = true }
         *   
         * Epileptic Color (random color every frame):
         *   new ExtGradient { epileptic = true }
         *   
         * Self Color:
         *   new ExtGradient { copyRigColor = true }
         *   
         * To change the font, you may use the following code:
         *   Font.CreateDynamicFontFromOSFont("Comic Sans MS", 24)
         */

        private static Color32 outlinecolor1 = new Color32(255, 5, 5, 1);
        private static Color32 outlinecolor2 = new Color32(55, 5, 5, 1);
        private static Color32 bgcolor = new Color32(15, 15, 15, 1);
        private static Color32 dbuttoncolor = new Color32(45, 45, 45, 1);
        private static Color32 ebuttoncolor = new Color32(15, 15, 15, 1);

        public static ExtGradient backgroundColor = new ExtGradient { colors = ExtGradient.GetSolidGradient(bgcolor) };
        public static ExtGradient outlineColor = new ExtGradient { colors = ExtGradient.GetSimpleGradient(outlinecolor1, outlinecolor2) };
        public static ExtGradient[] buttonColors = new ExtGradient[]
        {
            new ExtGradient { colors = ExtGradient.GetSolidGradient(dbuttoncolor) }, // Disabled
            new ExtGradient { colors = ExtGradient.GetSolidGradient(ebuttoncolor) } // Enabled
        };
        public static Color[] textColors = new Color[]
        {
            Color.white, // Disabled
            Color.white // Enabled
        };

        public static Font currentFont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;

        public static bool fpsCounter = true;
        public static bool disconnectButton = true;
        public static bool rightHanded;
        public static bool disableNotifications;

        public static KeyCode keyboardButton = KeyCode.Q;

        public static Vector3 menuSize = new Vector3(0.1f, 1f, 1f); // Depth, width, height
        public static int buttonsPerPage = 7;

        public static float gradientSpeed = 0.5f; // Speed of colors
    }
}
