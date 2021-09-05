﻿using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Global
{
    /// <summary>
    /// RGB HSL color conversion class
    /// </summary>
    public class ColorUtils
    {
        /// <summary>
        /// HSL class
        /// </summary>
        public class HSL
        {

            #region Variables
            private double _h;
            private double _s;
            private double _l;

            #endregion

            #region Constructor
            /// <summary>
            /// Constructor
            /// </summary>
            public HSL()
            {
                _h = 0;
                _s = 0;
                _l = 0;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="h">Hue</param>
            /// <param name="s">Saturation</param>
            /// <param name="l">Luminance</param>
            public HSL(double h, double s, double l)
            {
                _h = h;
                _s = s;
                _l = l;
            }

            #endregion

            #region Properties
            /// <summary>
            /// Get or set Hue
            /// </summary>
            public double H
            {
                get { return _h; }
                set
                {
                    _h = value;
                    _h = _h > 1 ? 1 : _h < 0 ? 0 : _h;
                }
            }

            /// <summary>
            /// Get or set Saturation
            /// </summary>
            public double S
            {
                get { return _s; }
                set
                {
                    _s = value;
                    _s = _s > 1 ? 1 : _s < 0 ? 0 : _s;
                }
            }

            /// <summary>
            /// Get or set Luminance
            /// </summary>
            public double L
            {
                get { return _l; }
                set
                {
                    _l = value;
                    _l = _l > 1 ? 1 : _l < 0 ? 0 : _l;
                }

            }

            #endregion

        }

        /// <summary>
        /// HSV class
        /// </summary>
        public class HSV
        {

            #region Variables
            private double _h;
            private double _s;
            private double _v;

            #endregion

            #region Constructor
            /// <summary>
            /// Constructor
            /// </summary>
            public HSV()
            {
                _h = 0;
                _s = 0;
                _v = 0;
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="h">Hue</param>
            /// <param name="s">Saturation</param>
            /// <param name="v">Value</param>
            public HSV(double h, double s, double v)
            {
                _h = h;
                _s = s;
                _v = v;
            }

            #endregion

            #region Properties
            /// <summary>
            /// Get or set Hue
            /// </summary>
            public double H
            {
                get { return _h; }
                set
                {
                    _h = value;
                    _h = _h > 1 ? 1 : _h < 0 ? 0 : _h;
                }
            }

            /// <summary>
            /// Get or set Saturation
            /// </summary>
            public double S
            {
                get { return _s; }
                set
                {
                    _s = value;
                    _s = _s > 1 ? 1 : _s < 0 ? 0 : _s;
                }
            }

            /// <summary>
            /// Get or set Value
            /// </summary>
            public double V
            {
                get { return _v; }
                set { _v = value; }

            }

            #endregion

        }

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public ColorUtils()
        {

        }

        #endregion

        #region Methods
        /// <summary>
        /// Sets the absolute brightness of a colour
        /// </summary>
        /// <param name="c">Original colour</param>
        /// <param name="brightness">The luminance level to impose</param>
        /// <returns>an adjusted colour</returns>
        public static Color SetBrightness(Color c, double brightness)
        {
            HSL hsl = RGBToHSL(c);
            hsl.L = brightness;
            return HSLToRGB(hsl);
        }

        /// <summary>
        /// Modifies an existing brightness level
        /// </summary>
        /// <remarks>
        /// To reduce brightness use a number smaller than 1. To increase brightness use a number larger tnan 1
        /// </remarks>
        /// <param name="c">The original colour</param>
        /// <param name="brightness">The luminance delta</param>
        /// <returns>An adjusted colour</returns>
        public static Color ModifyBrightness(Color c, double brightness)
        {
            HSL hsl = RGBToHSL(c);
            hsl.L *= brightness;
            return HSLToRGB(hsl);
        }

        /// <summary>
        /// Sets the absolute saturation level
        /// </summary>
        /// <remarks>Accepted values 0-1</remarks>
        /// <param name="c">An original colour</param>
        /// <param name="Saturation">The saturation value to impose</param>
        /// <returns>An adjusted colour</returns>
        public static Color SetSaturation(Color c, double Saturation)
        {
            HSL hsl = RGBToHSL(c);
            hsl.S = Saturation;
            return HSLToRGB(hsl);
        }

        /// <summary>
        /// Modifies an existing Saturation level
        /// </summary>
        /// <remarks>
        /// To reduce Saturation use a number smaller than 1. To increase Saturation use a number larger tnan 1
        /// </remarks>
        /// <param name="c">The original colour</param>
        /// <param name="Saturation">The saturation delta</param>
        /// <returns>An adjusted colour</returns>
        public static Color ModifySaturation(Color c, double Saturation)
        {
            HSL hsl = RGBToHSL(c);
            hsl.S *= Saturation;
            return HSLToRGB(hsl);
        }

        /// <summary>
        /// Sets the absolute Hue level
        /// </summary>
        /// <remarks>Accepted values 0-1</remarks>
        /// <param name="c">An original colour</param>
        /// <param name="Hue">The Hue value to impose</param>
        /// <returns>An adjusted colour</returns>
        public static Color SetHue(Color c, double Hue)
        {
            HSL hsl = RGBToHSL(c);
            hsl.H = Hue;
            return HSLToRGB(hsl);
        }

        /// <summary>
        /// Modifies an existing Hue level
        /// </summary>
        /// <remarks>
        /// To reduce Hue use a number smaller than 1. To increase Hue use a number larger tnan 1
        /// </remarks>
        /// <param name="c">The original colour</param>
        /// <param name="Hue">The Hue delta</param>
        /// <returns>An adjusted colour</returns>
        public static Color ModifyHue(Color c, double Hue)
        {
            HSL hsl = RGBToHSL(c);
            hsl.H *= Hue;
            return HSLToRGB(hsl);
        }

        /// <summary>
        /// Converts a colour from HSL to RGB
        /// </summary>
        /// <remarks>Adapted from the algoritm in Foley and Van-Dam</remarks>
        /// <param name="hsl">The HSL value</param>
        /// <returns>A Color structure containing the equivalent RGB values</returns>
        public static Color HSLToRGB(HSL hsl)
        {
            double r = 0, g = 0, b = 0;
            double temp1, temp2;

            if (hsl.L == 0)
            {
                r = g = b = 0;
            }
            else
            {
                if (hsl.S == 0)
                {
                    r = g = b = hsl.L;
                }
                else
                {
                    temp2 = ((hsl.L <= 0.5) ? hsl.L * (1.0 + hsl.S) : hsl.L + hsl.S - (hsl.L * hsl.S));
                    temp1 = 2.0 * hsl.L - temp2;

                    double[] t3 = new double[] { hsl.H + 1.0 / 3.0, hsl.H, hsl.H - 1.0 / 3.0 };
                    double[] clr = new double[] { 0, 0, 0 };
                    for (int i = 0; i < 3; i++)
                    {
                      if (t3[i] < 0)
                            t3[i] += 1.0;
                        if (t3[i] > 1)
                            t3[i] -= 1.0;

                        if (6.0 * t3[i] < 1.0)
                            clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0;
                        else if (2.0 * t3[i] < 1.0)
                            clr[i] = temp2;
                        else if (3.0 * t3[i] < 2.0)
                            clr[i] = (temp1 + (temp2 - temp1) * ((2.0 / 3.0) - t3[i]) * 6.0);
                        else
                            clr[i] = temp1;
                    }

                    r = clr[0];
                    g = clr[1];
                    b = clr[2];
                }
            }

            return Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
        }

        /// <summary>
        /// Converts RGB to HSL
        /// </summary>
        /// <remarks>Takes advantage of whats already built in to .NET by using the Color.GetHue, Color.GetSaturation and Color.GetBrightness methods</remarks>
        /// <param name="c">A Color to convert</param>
        /// <returns>An HSL value</returns>
        public static HSL RGBToHSL(Color c)
        {
            HSL hsl = new HSL();
            hsl.H = c.GetHue() / 360.0; // we store hue as 0-1 as opposed to 0-360
            hsl.L = c.GetBrightness();
            hsl.S = c.GetSaturation();

            return hsl;
        }

        /// <summary>
        /// Convert RGB color to HSV color
        /// </summary>
        /// <param name="color">RGB color</param>
        /// <returns>HSV color</returns>
        public static HSV RGBToHSV(Color color)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            double hue = color.GetHue();
            double saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            double value = max / 255d;

            return new HSV(hue, saturation, value);
        }

        /// <summary>
        /// Convert HSV color to RGB color
        /// </summary>
        /// <param name="hsv">HSV color</param>
        /// <returns>RGB color</returns>
        public static Color HSVToRGB(HSV hsv)
        {
            double hue = hsv.H;
            double saturation = hsv.S;
            double value = hsv.V;
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        /// <summary>
        /// Convert color to KML color string - AABBGGRR
        /// </summary>
        /// <param name="color">The color</param>
        /// <returns>KML color string</returns>
        public static string ToKMLColor(Color color)
        {
            int intA = (int)color.A;
            int intR = (int)color.R;
            int intG = (int)color.G;
            int intB = (int)color.B;

            string hexA = intA.ToString("X2");
            string hexR = intR.ToString("X2");
            string hexG = intG.ToString("X2");
            string hexB = intB.ToString("X2");

            return hexA + hexB + hexG + hexR;
        }

        #endregion
    }
}
