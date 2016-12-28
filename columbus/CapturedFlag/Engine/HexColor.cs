using UnityEngine;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Converts RGBA colors into Hexidecimal format. Useful for tk2d inline styling.
    /// </summary>
    public static class HexColor
    {
        public static string Red
        {
            get { return Color(new Vector4(1f, 0f, 0f, 1f)); }
        }
        public static string Green
        {
            get { return Color(new Vector4(0f, 255f / 255f, 23f / 255f, 1f)); }
        }
        public static string Purple
        {
            get { return Color(new Vector4(255f / 255f, 0f, 255f / 255f, 1f)); }
        }
        public static string Orange
        {
            get { return Color(new Vector4(255f / 255f, 128f / 255f, 0f, 1f)); }
        }
        public static string Yellow
        {
            get { return Color(new Vector4(255f / 255f, 222f / 255f, 0f, 1f)); }
        }
        public static string Blue
        {
            get { return Color(new Vector4(0f / 255f, 170f / 255f, 255f / 255f, 1f)); }
        }
        public static string White
        {
            get { return Color(new Vector4(1f, 1f, 1f, 1f)); }
        }

        /// <summary>
        /// Converts the RGBA vector into the double digit hexadecimal value.
        /// </summary>
        /// <param name="color">RGBA</param>
        /// <returns>Hexidecimal</returns>
        public static string Color(Vector4 color)
        {
            var rInt = (int)(color.x * 255);
            var gInt = (int)(color.y * 255);
            var bInt = (int)(color.z * 255);
            var aInt = (int)(color.w * 255);

            string rHex = rInt.ToString("X2");
            string gHex = gInt.ToString("X2");
            string bHex = bInt.ToString("X2");
            string aHex = aInt.ToString("X2");

            string inline = "^C" + rHex + gHex + bHex + aHex;
            return inline;
        }
    }
}