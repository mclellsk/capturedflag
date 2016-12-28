using UnityEngine;

namespace CapturedFlag.Engine
{
    /// <summary>
    /// Methods used to capture and store the contents of a screen in various formats.
    /// </summary>
    public static class ScreenCapture
    {
        /// <summary>
        /// Captures the pixels on the screen and stores them into a Texture2D the size of the screen.
        /// </summary>
        /// <returns>Screen capture as a Texture2D.</returns>
        public static Texture2D CaptureScreenAsTexture()
        {
            var width = Screen.width;
            var height = Screen.height;
            var texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            //NOTE: The error that occurs when ReadPixels() is used has no problem on function as per the unity3d wiki
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            return texture;
        }

        /// <summary>
        /// Captures the contents of the screen and stores them as a PNG at the specified path.
        /// </summary>
        /// <param name="filename">Name of file.</param>
        public static void CaptureScreenAsFile(string filename)
        {
            var path = DataSerializer.GetPersistentFile(filename);
            Application.CaptureScreenshot (path);
        }
    }
}
