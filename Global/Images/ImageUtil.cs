using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging; 
using System.Linq;
using System.Text;

namespace MeteoInfoC.Global.Images
{
    /// <summary>
    /// Image util
    /// </summary>
    public class ImageUtil
    {
        /// <summary>
        /// Rotate image
        /// </summary>
        /// <param name="bmp">The original image</param>
        /// <param name="angle">Rotate angle</param>
        /// <param name="bkColor">Back color</param>
        /// <returns>Rotated image</returns>
        public static Bitmap RotateImage(Bitmap bmp, float angle, Color bkColor)
        {
            // Get width and height 
            int width = bmp.Width;
            int height = bmp.Height;
            // Set PixelFormat
            PixelFormat pixelFormat = default(PixelFormat);
            if (bkColor == Color.Transparent)
            {
                pixelFormat = PixelFormat.Format32bppArgb;
            }
            else
            {
                // Get original pixel format
                pixelFormat = bmp.PixelFormat;
            }

            Bitmap tempImg = new Bitmap(width, height, pixelFormat);
 
            // Create graphics
            Graphics g = Graphics.FromImage(tempImg);
            g.Clear(bkColor);

            // Draw original image
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();

            // Set graphics path 
            GraphicsPath path = new GraphicsPath();
            // Add a rectangle in the path 
            path.AddRectangle(new RectangleF(0f, 0f, width, height));
            // Create a matrix
            Matrix matrix = new Matrix();
            // Rotate the matrix
            matrix.Rotate(angle);

            RectangleF rct = path.GetBounds(matrix);
            Bitmap newImg = new Bitmap(Convert.ToInt32(rct.Width), Convert.ToInt32(rct.Height), pixelFormat);
            g = Graphics.FromImage(newImg);
            g.Clear(bkColor);
            // Translate transform
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tempImg, 0, 0);
            g.Dispose();
            tempImg.Dispose();

            return newImg;
        } 
    }
}
