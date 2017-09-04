using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PocketStrafe
{
    internal static class Drawing
    {
        public static Bitmap CreateBitmapImage(string sImageText, Color color)
        {
            Bitmap objBmpImage = new Bitmap(2, 2);

            int intWidth = 0;
            int intHeight = 0;

            // Create the Font object for the image text drawing.
            System.Drawing.Font objFont = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);

            // Create a graphics object to measure the text's width and height.
            Graphics objGraphics = Graphics.FromImage(objBmpImage);

            // This is where the bitmap size is determined.
            int stringWidth = (int)objGraphics.MeasureString(sImageText, objFont).Width;
            int stringHeight = (int)objGraphics.MeasureString(sImageText, objFont).Height;

            intWidth = Math.Max(stringWidth, stringHeight);
            intHeight = intWidth;

            // Create the bmpImage again with the correct size for the text and font.
            objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));

            // Add the colors to the new bitmap.
            objGraphics = Graphics.FromImage(objBmpImage);

            // Set Background color

            objGraphics.Clear(Color.Transparent);
            objGraphics.SmoothingMode = SmoothingMode.HighQuality;

            objGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            objGraphics.DrawString(sImageText, objFont, new SolidBrush(color), (intWidth - stringWidth) / 2, (intHeight - stringHeight) / 2, StringFormat.GenericDefault);

            objGraphics.Flush();

            return (objBmpImage);
        }
    }
}