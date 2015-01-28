using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.IO;
using MeteoInfoC.Shape;
using MeteoInfoC.Legend;
using MeteoInfoC.Global;
using MeteoInfoC.Geoprocess;
using wContour;

namespace MeteoInfoC.Drawing
{
    /// <summary>
    /// Draw symbols
    /// </summary>
    public static class Draw
    {
        const double PI = 3.1415926535;

        #region Methods

        /// <summary>
        /// Draw wind arraw
        /// </summary>
        /// <param name="aColor"></param>
        /// <param name="sP"></param>
        /// <param name="aArraw"></param>
        /// <param name="g"></param>
        /// <param name="zoom"></param>
        public static void DrawArraw(Color aColor, PointF sP, WindArraw aArraw, Graphics g, double zoom)
        {
            PointF eP = new PointF(0, 0);
            PointF eP1 = new PointF(0, 0);
            double len = aArraw.length;
            Pen aPen = new Pen(aColor);            
            double angle = aArraw.angle + 180;
            if (angle >= 360)
                angle -= 360;

            len = len * zoom;

            eP.X = (int)(sP.X + len * Math.Sin(angle * PI / 180));
            eP.Y = (int)(sP.Y - len * Math.Cos(angle * PI / 180));            

            if (angle == 90)
            {
                eP.Y = sP.Y;
            }
            g.DrawLine(aPen, sP, eP);

            eP1.X = (int)(eP.X - aArraw.size * Math.Sin((angle + 20.0) * PI / 180));
            eP1.Y = (int)(eP.Y + aArraw.size * Math.Cos((angle + 20.0) * PI / 180));
            g.DrawLine(aPen, eP, eP1);

            eP1.X = (int)(eP.X - aArraw.size * Math.Sin((angle - 20.0) * PI / 180));
            eP1.Y = (int)(eP.Y + aArraw.size * Math.Cos((angle - 20.0) * PI / 180));
            g.DrawLine(aPen, eP, eP1);

            aPen.Dispose();
        }

        /// <summary>
        /// Calculate wind arraw
        /// </summary>
        /// <param name="U"></param>
        /// <param name="V"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <param name="sPoint"></param>
        /// <returns></returns>
        public static WindArraw CalArraw(double U, double V, double value, Single size, PointD sPoint)
        {
            WindArraw aArraw = new WindArraw();
            double aLen;

            aArraw.Point = sPoint;
            aArraw.Value = value;            
            aArraw.size = size;
            aLen = Math.Sqrt(U * U + V * V);
            aArraw.length = (Single)aLen;
            if (aLen == 0)
                aArraw.angle = 0;
            else
            {
                aArraw.angle = Math.Asin(U / aLen) * 180 / PI;
                if (U < 0 && V < 0)
                {
                    aArraw.angle = 180.0 - aArraw.angle;
                }
                else if (U > 0 && V < 0)
                {
                    aArraw.angle = 180.0 - aArraw.angle;
                }
                else if (U < 0 && V > 0)
                {
                    aArraw.angle = 360.0 + aArraw.angle;
                }
                aArraw.angle += 180;
                if (aArraw.angle >= 360)
                    aArraw.angle -= 360;
            }

            return aArraw;
        }

        /// <summary>
        /// Create wind barb from wind direction/speed
        /// </summary>
        /// <param name="windDir"></param>
        /// <param name="windSpeed"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <param name="sPoint"></param>
        /// <returns></returns>
        public static WindBarb CalWindBarb(Single windDir, Single windSpeed, double value,
            Single size, PointD sPoint)
        {
            WindBarb aWB = new WindBarb();

            windSpeed += 1;
            aWB.windSpeed = windSpeed;
            aWB.angle = windDir;
            aWB.Value = value;
            aWB.size = size;
            aWB.Point = sPoint;
            aWB.windSpeesLine.W20 = (int)(windSpeed / 20);
            aWB.windSpeesLine.W4 = (int)((windSpeed - aWB.windSpeesLine.W20 * 20) / 4);
            aWB.windSpeesLine.W2 = (int)((windSpeed - aWB.windSpeesLine.W20 * 20 -
                aWB.windSpeesLine.W4 * 4) / 2);

            return aWB;
        }

        /// <summary>
        /// Create wind barb from U/V
        /// </summary>
        /// <param name="U"></param>
        /// <param name="V"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <param name="sPoint"></param>
        /// <returns></returns>
        public static WindBarb CalWindBarbUV(double U, double V, double value,
            Single size, PointD sPoint)
        {
            Single windSpeed, windDir;

            windSpeed = (Single)Math.Sqrt(U * U + V * V);
            windDir = (Single)(Math.Asin(U / windSpeed) * 180 / PI);
            if (U < 0 && V < 0)
            {
                windDir = 180 - windDir;
            }
            else if (U > 0 && V < 0)
            {
                windDir = 180 - windDir;
            }
            else if (U < 0 && V > 0)
            {
                windDir = 360 + windDir;
            }
            windDir += 180;
            if (windDir >= 360)
                windDir -= 360;

            return CalWindBarb(windDir, windSpeed, value, size, sPoint);           
        }

        /// <summary>
        /// Draw wind barb
        /// </summary>
        /// <param name="aColor"></param>
        /// <param name="sP"></param>
        /// <param name="aWB"></param>
        /// <param name="g"></param>
        /// <param name="size"></param>
        public static void DrawWindBarb(Color aColor, PointF sP, WindBarb aWB, Graphics g, Single size)
        {
            PointF eP = new PointF(0, 0);
            PointF eP1 = new PointF(0, 0);
            double len = size * 2;
            Pen aPen = new Pen(aColor);
            int i;
            
            double aLen = len;

            eP.X = (Single)(sP.X + len * Math.Sin(aWB.angle * PI / 180));
            eP.Y = (Single)(sP.Y - len * Math.Cos(aWB.angle * PI / 180));
            g.DrawLine(aPen, sP, eP);

            len = len / 2;
            if (aWB.windSpeesLine.W20 > 0)
            {
                for (i = 0; i < aWB.windSpeesLine.W20; i++)
                {
                    eP1.X = (Single)(eP.X - len * Math.Sin((aWB.angle - 105) * PI / 180));
                    eP1.Y = (Single)(eP.Y + len * Math.Cos((aWB.angle - 105) * PI / 180));
                    g.DrawLine(aPen, eP, eP1);
                    //eP.X = (Single)(eP1.X - len * Math.Sin((aWB.angle + 45) * PI / 180));
                    //eP.Y = (Single)(eP1.Y + len * Math.Cos((aWB.angle + 45) * PI / 180));
                    eP.X = (Single)(eP.X - aLen / 8 * Math.Sin((aWB.angle) * PI / 180));
                    eP.Y = (Single)(eP.Y + aLen / 8 * Math.Cos((aWB.angle) * PI / 180));
                    g.DrawLine(aPen, eP, eP1);
                }
                eP.X = (Single)(eP.X - aLen / 8 * Math.Sin((aWB.angle) * PI / 180));
                eP.Y = (Single)(eP.Y + aLen / 8 * Math.Cos((aWB.angle) * PI / 180));
            }
            if (aWB.windSpeesLine.W4 > 0)
            {
                for (i = 0; i < aWB.windSpeesLine.W4; i++)
                {
                    eP1.X = (Single)(eP.X - len * Math.Sin((aWB.angle - 120) * PI / 180));
                    eP1.Y = (Single)(eP.Y + len * Math.Cos((aWB.angle - 120) * PI / 180));
                    g.DrawLine(aPen, eP, eP1);
                    eP.X = (Single)(eP.X - aLen / 8 * Math.Sin((aWB.angle) * PI / 180));
                    eP.Y = (Single)(eP.Y + aLen / 8 * Math.Cos((aWB.angle) * PI / 180));
                }
            }
            if (aWB.windSpeesLine.W2 > 0)
            {
                len = len / 2;
                eP1.X = (Single)(eP.X - len * Math.Sin((aWB.angle - 120) * PI / 180));
                eP1.Y = (Single)(eP.Y + len * Math.Cos((aWB.angle - 120) * PI / 180));
                g.DrawLine(aPen, eP, eP1);
            }

            aPen.Dispose();
        }

        /// <summary>
        /// Draw wind barb and cut the beginning
        /// </summary>
        /// <param name="aColor"></param>
        /// <param name="sP"></param>
        /// <param name="aWB"></param>
        /// <param name="g"></param>
        /// <param name="size"></param>
        /// <param name="cut"></param>
        public static void DrawWindBarb(Color aColor, PointF sP, WindBarb aWB, Graphics g, Single size, Single cut)
        {
            PointF eP = new PointF(0, 0);
            PointF eP1 = new PointF(0, 0);
            double len = size * 2;
            Pen aPen = new Pen(aColor);
            int i;

            double aLen = len;

            eP.X = (Single)(sP.X + len * Math.Sin(aWB.angle * PI / 180));
            eP.Y = (Single)(sP.Y - len * Math.Cos(aWB.angle * PI / 180));
            PointF cutSP = new PointF(0, 0);
            cutSP.X = (Single)(sP.X + cut * Math.Sin(aWB.angle * PI / 180));
            cutSP.Y = (Single)(sP.Y - cut * Math.Cos(aWB.angle * PI / 180));
            g.DrawLine(aPen, cutSP, eP);

            len = len / 2;
            if (aWB.windSpeesLine.W20 > 0)
            {
                for (i = 0; i < aWB.windSpeesLine.W20; i++)
                {
                    eP1.X = (Single)(eP.X - len * Math.Sin((aWB.angle - 105) * PI / 180));
                    eP1.Y = (Single)(eP.Y + len * Math.Cos((aWB.angle - 105) * PI / 180));
                    g.DrawLine(aPen, eP, eP1);
                    //eP.X = (Single)(eP1.X - len * Math.Sin((aWB.angle + 45) * PI / 180));
                    //eP.Y = (Single)(eP1.Y + len * Math.Cos((aWB.angle + 45) * PI / 180));
                    eP.X = (Single)(eP.X - aLen / 8 * Math.Sin((aWB.angle) * PI / 180));
                    eP.Y = (Single)(eP.Y + aLen / 8 * Math.Cos((aWB.angle) * PI / 180));
                    g.DrawLine(aPen, eP, eP1);
                }
                eP.X = (Single)(eP.X - aLen / 8 * Math.Sin((aWB.angle) * PI / 180));
                eP.Y = (Single)(eP.Y + aLen / 8 * Math.Cos((aWB.angle) * PI / 180));
            }
            if (aWB.windSpeesLine.W4 > 0)
            {
                for (i = 0; i < aWB.windSpeesLine.W4; i++)
                {
                    eP1.X = (Single)(eP.X - len * Math.Sin((aWB.angle - 120) * PI / 180));
                    eP1.Y = (Single)(eP.Y + len * Math.Cos((aWB.angle - 120) * PI / 180));
                    g.DrawLine(aPen, eP, eP1);
                    eP.X = (Single)(eP.X - aLen / 8 * Math.Sin((aWB.angle) * PI / 180));
                    eP.Y = (Single)(eP.Y + aLen / 8 * Math.Cos((aWB.angle) * PI / 180));
                }
            }
            if (aWB.windSpeesLine.W2 > 0)
            {
                len = len / 2;
                eP1.X = (Single)(eP.X - len * Math.Sin((aWB.angle - 120) * PI / 180));
                eP1.Y = (Single)(eP.Y + len * Math.Cos((aWB.angle - 120) * PI / 180));
                g.DrawLine(aPen, eP, eP1);
            }

            aPen.Dispose();
        }

        /// <summary>
        /// Create weather symbol
        /// </summary>
        /// <param name="value"></param>
        /// <param name="weather"></param>
        /// <param name="sP"></param>
        /// <returns></returns>
        public static WeatherSymbol CalWeatherSymbol(double value, int weather, PointD sP)
        {
            WeatherSymbol aWS = new WeatherSymbol();
            aWS.Point = sP;
            aWS.Value = value;
            aWS.weather = weather;

            return aWS;
        }

        /// <summary>
        /// Draw weather symbol
        /// </summary>
        /// <param name="aColor"></param>
        /// <param name="sP"></param>
        /// <param name="aWS"></param>
        /// <param name="g"></param>
        /// <param name="size"></param>
        public static void DrawWeatherSymbol(Color aColor, PointF sP, WeatherSymbol aWS, Graphics g, Single size)
        {
            SolidBrush aBrush = new SolidBrush(aColor);
            Font wFont = new Font("Weather", size);
            PointF sPoint = sP;
            SizeF sf = g.MeasureString(((char)(aWS.weather + 100)).ToString(), wFont);
            sPoint.X = sPoint.X - sf.Width / 2;
            sPoint.Y = sPoint.Y - sf.Height / 2;
            string text = ((char)(aWS.weather + 28)).ToString();
            if (aWS.weather == 99)
            {
                text = ((char)(aWS.weather + 97)).ToString();
            }
            //string text = ((char)(aWS.weather + 100)).ToString();
            g.DrawString(text, wFont, aBrush, sPoint);

            wFont.Dispose();
            aBrush.Dispose();           
        }

        /// <summary>
        /// Create statioin model symbol
        /// </summary>
        /// <param name="windDir"></param>
        /// <param name="windSpeed"></param>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <param name="sPoint"></param>
        /// <param name="weather"></param>
        /// <param name="temp"></param>
        /// <param name="dewPoint"></param>
        /// <param name="pressure"></param>
        /// <param name="cloudCover"></param>
        /// <returns></returns>
        public static StationModelShape CalStationModel(Single windDir, Single windSpeed, double value,
            Single size, PointD sPoint, int weather, int temp, int dewPoint, int pressure, int cloudCover)
        {
            StationModelShape aSM = new StationModelShape();
            aSM.Point = sPoint;
            aSM.Value = value;
            aSM.size = size;
            aSM.temperature = temp;
            aSM.dewPoint = dewPoint;
            aSM.pressure = pressure;
            aSM.windBarb = CalWindBarb(windDir, windSpeed, value, size, sPoint);            
            aSM.weatherSymbol.size = size / 4 * 3;
            //sPoint.X = sPoint.X - size / 2;
            PointD aPoint = new PointD(sPoint.X - size / 2, sPoint.Y);
            aSM.weatherSymbol.Point = aPoint;            
            aSM.weatherSymbol.weather = weather;
            aSM.cloudCoverage.cloudCover = cloudCover;
            aSM.cloudCoverage.size = size / 4 * 3;
            aSM.cloudCoverage.sPoint = aPoint;

            return aSM;
        }

        /// <summary>
        /// Draw station model symbol
        /// </summary>
        /// <param name="aColor"></param>
        /// <param name="foreColor"></param>
        /// <param name="sP"></param>
        /// <param name="aSM"></param>
        /// <param name="g"></param>
        /// <param name="size"></param>
        /// <param name="cut"></param>
        public static void DrawStationModel(Color aColor, Color foreColor, PointF sP, StationModelShape aSM, Graphics g,
            Single size, Single cut)
        {
            PointF sPoint = new PointF(0, 0);
            SolidBrush aBrush = new SolidBrush(foreColor);
            Font wFont = new Font("Weather", 6);              

            //Draw cloud coverage     
            if (aSM.cloudCoverage.cloudCover >= 0 && aSM.cloudCoverage.cloudCover <= 9)
            {
                //Draw wind barb
                DrawWindBarb(aColor, sP, aSM.windBarb, g, size, cut);

                wFont = new Font("Weather", size / 4 * 3);
                SizeF sf = g.MeasureString(((char)(aSM.cloudCoverage.cloudCover + 197)).ToString(), wFont);
                sPoint.X = sP.X - sf.Width / 2;
                sPoint.Y = sP.Y - sf.Height / 2;
                g.DrawString(((char)(aSM.cloudCoverage.cloudCover + 197)).ToString(), wFont, aBrush, sPoint);
            }
            else
            {
                //Draw wind barb
                DrawWindBarb(aColor, sP, aSM.windBarb, g, size);

                wFont = new Font("Arial", size / 2);
                SizeF sf = g.MeasureString("M", wFont);
                sPoint.X = sP.X - sf.Width / 2;
                sPoint.Y = sP.Y - sf.Height / 3 * 2;
                g.DrawString("M", wFont, aBrush, sPoint);
                wFont = new Font("Weather", size / 4 * 3);
                sf = g.MeasureString(((char)(197)).ToString(), wFont);
                sPoint.X = sP.X - sf.Width / 2;
                sPoint.Y = sP.Y - sf.Height / 2;
                g.DrawString(((char)(197)).ToString(), wFont, aBrush, sPoint);
                //g.DrawString(((char)(205)).ToString(), wFont, new SolidBrush(Color.LightGray), sPoint);
            }

            //Draw weather
            if (aSM.weatherSymbol.weather >= 4 && aSM.weatherSymbol.weather <= 99)
            {
                wFont = new Font("Weather", size / 4 * 3);
                SizeF sf = g.MeasureString(((char)(aSM.weatherSymbol.weather + 100)).ToString(), wFont);
                sPoint.X = sP.X - sf.Width - aSM.size / 2;
                sPoint.Y = sP.Y - sf.Height / 2;
                string text = ((char)(aSM.weatherSymbol.weather + 28)).ToString();
                if (aSM.weatherSymbol.weather == 99)
                {
                    text = ((char)(aSM.weatherSymbol.weather + 97)).ToString();
                }
                //string text = ((char)(aSM.weatherSymbol.weather + 100)).ToString();
                g.DrawString(text, wFont, aBrush, sPoint);
            }

            wFont = new Font("Arial", size / 2);
            //Draw temperature
            if (Math.Abs(aSM.temperature) < 1000)
            {
                aBrush.Color = Color.Red;                
                SizeF sf = g.MeasureString(aSM.temperature.ToString(), wFont);
                sPoint.X = sP.X - sf.Width - size / 3;
                sPoint.Y = sP.Y - sf.Height - size / 3;
                g.DrawString(aSM.temperature.ToString(), wFont, aBrush, sPoint);
            }

            //Draw dew point
            if (Math.Abs(aSM.dewPoint) < 1000)
            {
                aBrush.Color = Color.Green;                
                SizeF sf = g.MeasureString(aSM.dewPoint.ToString(), wFont);
                sPoint.X = sP.X - sf.Width - size / 3;
                sPoint.Y = sP.Y + size / 3;
                g.DrawString(aSM.dewPoint.ToString(), wFont, aBrush, sPoint);
            }

            //Draw pressure
            if (Math.Abs(aSM.pressure) < 1000)
            {
                aBrush.Color = foreColor;                
                SizeF sf = g.MeasureString(aSM.pressure.ToString("000"), wFont);
                sPoint.X = sP.X + size / 3;
                sPoint.Y = sP.Y - sf.Height - size / 3;
                g.DrawString(aSM.pressure.ToString("000"), wFont, aBrush, sPoint);
            }

            wFont.Dispose();
            aBrush.Dispose();
        }        

        /// <summary>
        /// Draw point
        /// </summary>
        /// <param name="aPS"></param>
        /// <param name="aP"></param>
        /// <param name="color"></param>
        /// <param name="outlineColor"></param>
        /// <param name="aSize"></param>
        /// <param name="drawOutline"></param>
        /// <param name="drawFill"></param>
        /// <param name="g"></param>
        public static void DrawPoint(PointStyle aPS, PointF aP, Color color, Color outlineColor,
            Single aSize, Boolean drawOutline, Boolean drawFill, Graphics g)
        {
            Pen aPen = new Pen(outlineColor);
            SolidBrush aBrush = new SolidBrush(color);
            PointF[] points;
            PointF bP = new PointF(0, 0);

            GraphicsPath path = new GraphicsPath();

            switch (aPS)
            {
                case PointStyle.Circle:
                    aP.X = aP.X - aSize / 2;
                    aP.Y = aP.Y - aSize / 2;
                    if (drawFill)
                    {
                        g.FillEllipse(aBrush, aP.X, aP.Y, aSize, aSize);
                    }
                    if (drawOutline)
                    {
                        g.DrawEllipse(aPen, aP.X, aP.Y, aSize, aSize);
                    }
                    break;
                case PointStyle.Square:
                    aP.X = aP.X - aSize / 2;
                    aP.Y = aP.Y - aSize / 2;
                    if (drawFill)
                    {
                        g.FillRectangle(aBrush, aP.X, aP.Y, aSize, aSize);
                    }
                    if (drawOutline)
                    {
                        g.DrawRectangle(aPen, aP.X, aP.Y, aSize, aSize);
                    }
                    break;
                case PointStyle.Diamond:
                    points = new PointF[4];
                    bP.X = aP.X - aSize / 2;
                    bP.Y = aP.Y;
                    points[0] = bP;
                    bP.X = aP.X;
                    bP.Y = aP.Y - aSize / 2;
                    points[1] = bP;
                    bP.X = aP.X + aSize / 2;
                    bP.Y = aP.Y;
                    points[2] = bP;
                    bP.X = aP.X;
                    bP.Y = aP.Y + aSize / 2;
                    points[3] = bP;
                    if (drawFill)
                    {
                        g.FillPolygon(aBrush, points);
                    }
                    if (drawOutline)
                    {
                        g.DrawPolygon(aPen, points);
                    }
                    break;
                case PointStyle.UpTriangle:
                    points = new PointF[3];
                    bP.X = aP.X;
                    bP.Y = aP.Y - aSize / 2;
                    points[0] = bP;
                    bP.X = (Single)(aP.X + aSize / 4 * Math.Sqrt(3));
                    bP.Y = aP.Y + aSize / 4;
                    points[1] = bP;
                    bP.X = (Single)(aP.X - aSize / 4 * Math.Sqrt(3));
                    bP.Y = aP.Y + aSize / 4;
                    points[2] = bP;
                    if (drawFill)
                    {
                        g.FillPolygon(aBrush, points);
                    }
                    if (drawOutline)
                    {
                        g.DrawPolygon(aPen, points);
                    }
                    break;
                case PointStyle.DownTriangle:
                    points = new PointF[3];
                    bP.X = aP.X;
                    bP.Y = aP.Y + aSize / 2;
                    points[0] = bP;
                    bP.X = (Single)(aP.X - aSize / 4 * Math.Sqrt(3));
                    bP.Y = aP.Y - aSize / 2;
                    points[1] = bP;
                    bP.X = (Single)(aP.X + aSize / 4 * Math.Sqrt(3));
                    bP.Y = aP.Y - aSize / 2;
                    points[2] = bP;
                    if (drawFill)
                    {
                        g.FillPolygon(aBrush, points);
                    }
                    if (drawOutline)
                    {
                        g.DrawPolygon(aPen, points);
                    }
                    break;
                case PointStyle.XCross:
                    aPen.Color = color;
                    path.AddLine(aP.X - aSize / 2, aP.Y - aSize / 2, aP.X + aSize / 2, aP.Y + aSize / 2);
                    path.StartFigure();
                    path.AddLine(aP.X - aSize / 2, aP.Y + aSize / 2, aP.X + aSize / 2, aP.Y - aSize / 2);
                    if (drawFill || drawOutline)
                        g.DrawPath(aPen, path);
                    break;
                case PointStyle.Plus:
                    aPen.Color = color;
                    path.AddLine(aP.X, aP.Y - aSize / 2, aP.X, aP.Y + aSize / 2);
                    path.StartFigure();
                    path.AddLine(aP.X - aSize / 2, aP.Y, aP.X + aSize / 2, aP.Y);
                    if (drawFill || drawOutline)
                        g.DrawPath(aPen, path);
                    break;
                case PointStyle.StarLines:
                    aPen.Color = color;
                    path.AddLine(aP.X - aSize / 2, aP.Y - aSize / 2, aP.X + aSize / 2, aP.Y + aSize / 2);
                    path.StartFigure();
                    path.AddLine(aP.X - aSize / 2, aP.Y + aSize / 2, aP.X + aSize / 2, aP.Y - aSize / 2);
                    path.StartFigure();
                    path.AddLine(aP.X, aP.Y - aSize / 2, aP.X, aP.Y + aSize / 2);
                    path.StartFigure();
                    path.AddLine(aP.X - aSize / 2, aP.Y, aP.X + aSize / 2, aP.Y);
                    if (drawFill || drawOutline)
                        g.DrawPath(aPen, path);                    
                    break;
                case PointStyle.Star:
                    float vRadius = aSize / 2;
                    //Calculate 5 end points
                    PointF[] vPoints = new PointF[5];
                    double vAngle = 2.0 * Math.PI / 4 + Math.PI;
                    for (int i = 0; i < vPoints.Length; i++)
                    {
                        vAngle += 2.0 * Math.PI / (double)vPoints.Length;
                        vPoints[i] = new PointF(
                            (float)(Math.Cos(vAngle) * vRadius) + aP.X,
                            (float)(Math.Sin(vAngle) * vRadius) + aP.Y);
                    }     
                    //Calculate 5 cross points
                    PointF[] cPoints = new PointF[5];
                    cPoints[0] = Contour.GetCrossPoint(vPoints[0], vPoints[2], vPoints[1], vPoints[4]);
                    cPoints[1] = Contour.GetCrossPoint(vPoints[1], vPoints[3], vPoints[0], vPoints[2]);
                    cPoints[2] = Contour.GetCrossPoint(vPoints[1], vPoints[3], vPoints[2], vPoints[4]);
                    cPoints[3] = Contour.GetCrossPoint(vPoints[0], vPoints[3], vPoints[2], vPoints[4]);
                    cPoints[4] = Contour.GetCrossPoint(vPoints[0], vPoints[3], vPoints[1], vPoints[4]);
                    //New points
                    PointF[] vTemps = new PointF[10];
                    for (int i = 0; i < 5; i++)
                    {
                        vTemps[i * 2] = vPoints[i];
                        vTemps[i * 2 + 1] = cPoints[i];
                    }
                    if (drawFill)
                        g.FillPolygon(aBrush, vTemps);
                    if (drawOutline)
                        g.DrawPolygon(aPen, vTemps);
                    break;
                case PointStyle.Pentagon:
                    vRadius = aSize / 2;
                    //Calculate 5 end points
                    vPoints = new PointF[5];
                    vAngle = 2.0 * Math.PI / 4 + Math.PI;
                    for (int i = 0; i < vPoints.Length; i++)
                    {
                        vAngle += 2.0 * Math.PI / (double)vPoints.Length;
                        vPoints[i] = new PointF(
                            (float)(Math.Cos(vAngle) * vRadius) + aP.X,
                            (float)(Math.Sin(vAngle) * vRadius) + aP.Y);
                    }
                    if (drawFill)
                        g.FillPolygon(aBrush, vPoints);
                    if (drawOutline)
                        g.DrawPolygon(aPen, vPoints);
                    break;
            }

            aPen.Dispose();
            aBrush.Dispose();
            path.Dispose();
        }
        
        /// <summary>
        /// Draw point
        /// </summary>       
        /// <param name="aP"></param>
        /// <param name="aPB"></param>
        /// <param name="g"></param>
        public static void DrawPoint(PointF aP, PointBreak aPB, Graphics g)
        {
            //Matrix myMatrix = new Matrix();
            //if (aPB.Angle != 0)
            //{                             
            //    myMatrix.RotateAt(aPB.Angle, aP, MatrixOrder.Append);
            //    g.Transform = myMatrix;
            //}

            switch (aPB.MarkerType)
            {
                case MarkerType.Simple:
                    DrawPoint_Simple(aP, aPB, g);
                    break;
                case MarkerType.Character:
                    DrawPoint_Character(aP, aPB, g);
                    break;
                case MarkerType.Image:
                    DrawPoint_Image(aP, aPB, g);
                    break;
            }

            //if (aPB.Angle != 0)
            //    g.Transform = new Matrix();
        }

        private static void DrawPoint_Simple_Up(PointF aP, PointBreak aPB, Graphics g)
        {
            Matrix oldMatrix = g.Transform;           
            if (aPB.Angle != 0)
            {
                Matrix myMatrix = new Matrix();
                myMatrix.RotateAt(aPB.Angle, aP);
                myMatrix.Translate(oldMatrix.OffsetX, oldMatrix.OffsetY, MatrixOrder.Append);
                g.Transform = myMatrix;
            }

            Pen aPen = new Pen(aPB.OutlineColor);
            SolidBrush aBrush = new SolidBrush(aPB.Color);
            PointF[] points;
            PointF bP = new PointF(0, 0);
            Single aSize = aPB.Size;
            bool drawFill = aPB.DrawFill;
            bool drawOutline = aPB.DrawOutline;
            Color color = aPB.Color;

            GraphicsPath path = new GraphicsPath();

            switch (aPB.Style)
            {
                case PointStyle.Circle:
                    aP.X = aP.X - aSize / 2;
                    aP.Y = aP.Y - aSize;
                    if (drawFill)
                    {
                        g.FillEllipse(aBrush, aP.X, aP.Y, aSize, aSize);
                    }
                    if (drawOutline)
                    {
                        g.DrawEllipse(aPen, aP.X, aP.Y, aSize, aSize);
                    }
                    break;
                case PointStyle.Square:
                    aP.X = aP.X - aSize / 2;
                    aP.Y = aP.Y - aSize;
                    if (drawFill)
                    {
                        g.FillRectangle(aBrush, aP.X, aP.Y, aSize, aSize);
                    }
                    if (drawOutline)
                    {
                        g.DrawRectangle(aPen, aP.X, aP.Y, aSize, aSize);
                    }
                    break;
                case PointStyle.Diamond:
                    points = new PointF[4];
                    bP.X = aP.X - aSize / 2;
                    bP.Y = aP.Y;
                    points[0] = bP;
                    bP.X = aP.X;
                    bP.Y = aP.Y - aSize / 2;
                    points[1] = bP;
                    bP.X = aP.X + aSize / 2;
                    bP.Y = aP.Y;
                    points[2] = bP;
                    bP.X = aP.X;
                    bP.Y = aP.Y + aSize / 2;
                    points[3] = bP;
                    if (drawFill)
                    {
                        g.FillPolygon(aBrush, points);
                    }
                    if (drawOutline)
                    {
                        g.DrawPolygon(aPen, points);
                    }
                    break;
                case PointStyle.UpTriangle:
                    points = new PointF[3];
                    bP.X = aP.X;
                    bP.Y = aP.Y - aSize * 3 / 4;
                    points[0] = bP;
                    bP.X = (Single)(aP.X + aSize / 4 * Math.Sqrt(3));
                    bP.Y = aP.Y;
                    points[1] = bP;
                    bP.X = (Single)(aP.X - aSize / 4 * Math.Sqrt(3));
                    bP.Y = aP.Y;
                    points[2] = bP;
                    if (drawFill)
                    {
                        g.FillPolygon(aBrush, points);
                    }
                    if (drawOutline)
                    {
                        g.DrawPolygon(aPen, points);
                    }
                    break;
                case PointStyle.DownTriangle:
                    points = new PointF[3];
                    bP.X = aP.X;
                    bP.Y = aP.Y + aSize / 2;
                    points[0] = bP;
                    bP.X = (Single)(aP.X - aSize / 4 * Math.Sqrt(3));
                    bP.Y = aP.Y - aSize / 2;
                    points[1] = bP;
                    bP.X = (Single)(aP.X + aSize / 4 * Math.Sqrt(3));
                    bP.Y = aP.Y - aSize / 2;
                    points[2] = bP;
                    if (drawFill)
                    {
                        g.FillPolygon(aBrush, points);
                    }
                    if (drawOutline)
                    {
                        g.DrawPolygon(aPen, points);
                    }
                    break;
                case PointStyle.XCross:
                    aPen.Color = color;
                    path.AddLine(aP.X - aSize / 2, aP.Y - aSize / 2, aP.X + aSize / 2, aP.Y + aSize / 2);
                    path.StartFigure();
                    path.AddLine(aP.X - aSize / 2, aP.Y + aSize / 2, aP.X + aSize / 2, aP.Y - aSize / 2);
                    if (drawFill || drawOutline)
                        g.DrawPath(aPen, path);
                    break;
                case PointStyle.Plus:
                    aPen.Color = color;
                    path.AddLine(aP.X, aP.Y - aSize / 2, aP.X, aP.Y + aSize / 2);
                    path.StartFigure();
                    path.AddLine(aP.X - aSize / 2, aP.Y, aP.X + aSize / 2, aP.Y);
                    if (drawFill || drawOutline)
                        g.DrawPath(aPen, path);
                    break;
                case PointStyle.StarLines:
                    aPen.Color = color;
                    path.AddLine(aP.X - aSize / 2, aP.Y - aSize / 2, aP.X + aSize / 2, aP.Y + aSize / 2);
                    path.StartFigure();
                    path.AddLine(aP.X - aSize / 2, aP.Y + aSize / 2, aP.X + aSize / 2, aP.Y - aSize / 2);
                    path.StartFigure();
                    path.AddLine(aP.X, aP.Y - aSize / 2, aP.X, aP.Y + aSize / 2);
                    path.StartFigure();
                    path.AddLine(aP.X - aSize / 2, aP.Y, aP.X + aSize / 2, aP.Y);
                    if (drawFill || drawOutline)
                        g.DrawPath(aPen, path);
                    break;
                case PointStyle.Star:
                    float vRadius = aSize / 2;
                    //Calculate 5 end points
                    PointF[] vPoints = new PointF[5];
                    double vAngle = 2.0 * Math.PI / 4 + Math.PI;
                    for (int i = 0; i < vPoints.Length; i++)
                    {
                        vAngle += 2.0 * Math.PI / (double)vPoints.Length;
                        vPoints[i] = new PointF(
                            (float)(Math.Cos(vAngle) * vRadius) + aP.X,
                            (float)(Math.Sin(vAngle) * vRadius) + aP.Y);
                    }
                    //Calculate 5 cross points
                    PointF[] cPoints = new PointF[5];
                    cPoints[0] = Contour.GetCrossPoint(vPoints[0], vPoints[2], vPoints[1], vPoints[4]);
                    cPoints[1] = Contour.GetCrossPoint(vPoints[1], vPoints[3], vPoints[0], vPoints[2]);
                    cPoints[2] = Contour.GetCrossPoint(vPoints[1], vPoints[3], vPoints[2], vPoints[4]);
                    cPoints[3] = Contour.GetCrossPoint(vPoints[0], vPoints[3], vPoints[2], vPoints[4]);
                    cPoints[4] = Contour.GetCrossPoint(vPoints[0], vPoints[3], vPoints[1], vPoints[4]);
                    //New points
                    PointF[] vTemps = new PointF[10];
                    for (int i = 0; i < 5; i++)
                    {
                        vTemps[i * 2] = vPoints[i];
                        vTemps[i * 2 + 1] = cPoints[i];
                    }
                    if (drawFill)
                        g.FillPolygon(aBrush, vTemps);
                    if (drawOutline)
                        g.DrawPolygon(aPen, vTemps);
                    break;
                case PointStyle.Pentagon:
                    vRadius = aSize / 2;
                    //Calculate 5 end points
                    vPoints = new PointF[5];
                    vAngle = 2.0 * Math.PI / 4 + Math.PI;
                    for (int i = 0; i < vPoints.Length; i++)
                    {
                        vAngle += 2.0 * Math.PI / (double)vPoints.Length;
                        vPoints[i] = new PointF(
                            (float)(Math.Cos(vAngle) * vRadius) + aP.X,
                            (float)(Math.Sin(vAngle) * vRadius) + aP.Y);
                    }
                    if (drawFill)
                        g.FillPolygon(aBrush, vPoints);
                    if (drawOutline)
                        g.DrawPolygon(aPen, vPoints);
                    break;
                case PointStyle.UpSemiCircle:
                    aP.X = aP.X - aSize / 2;
                    aP.Y = aP.Y - aSize / 2;
                    if (drawFill)
                    {
                        g.FillPie(aBrush, aP.X, aP.Y, aSize, aSize, 180, 180);
                    }
                    if (drawOutline)
                    {
                        g.DrawPie(aPen, aP.X, aP.Y, aSize, aSize, 180, 180);
                    }
                    break;
                case PointStyle.DownSemiCircle:
                    aP.X = aP.X - aSize / 2;
                    aP.Y = aP.Y - aSize / 2;
                    if (drawFill)
                    {
                        g.FillPie(aBrush, aP.X, aP.Y, aSize, aSize, 0, 180);
                    }
                    if (drawOutline)
                    {
                        g.DrawPie(aPen, aP.X, aP.Y, aSize, aSize, 0, 180);
                    }
                    break;
            }

            if (aPB.Angle != 0)
                g.Transform = oldMatrix;

            aPen.Dispose();
            aBrush.Dispose();
            path.Dispose();
            oldMatrix.Dispose();
        }

        /// <summary>
        /// Draw point
        /// </summary>       
        /// <param name="aP">Point position</param>
        /// <param name="aPB">Point break</param>
        /// <param name="g">graphics</param>
        private static void DrawPoint_Simple(PointF aP, PointBreak aPB, Graphics g)
        {
            Matrix oldMatrix = g.Transform;
            if (aPB.Angle != 0)
            {
                Matrix myMatrix = new Matrix();
                myMatrix.RotateAt(aPB.Angle, aP);
                myMatrix.Translate(oldMatrix.OffsetX, oldMatrix.OffsetY, MatrixOrder.Append);
                g.Transform = myMatrix;
                myMatrix.Dispose();
            }

            Pen aPen = new Pen(aPB.OutlineColor);
            SolidBrush aBrush = new SolidBrush(aPB.Color);
            PointF[] points;
            PointF bP = new PointF(0, 0);
            Single aSize = aPB.Size;
            bool drawFill = aPB.DrawFill;
            bool drawOutline = aPB.DrawOutline;
            Color color = aPB.Color;

            GraphicsPath path = new GraphicsPath();

            switch (aPB.Style)
            {
                case PointStyle.Circle:
                    aP.X = aP.X - aSize / 2;
                    aP.Y = aP.Y - aSize / 2;
                    if (drawFill)
                    {
                        g.FillEllipse(aBrush, aP.X, aP.Y, aSize, aSize);
                    }
                    if (drawOutline)
                    {
                        g.DrawEllipse(aPen, aP.X, aP.Y, aSize, aSize);
                    }
                    break;
                case PointStyle.Square:
                    aP.X = aP.X - aSize / 2;
                    aP.Y = aP.Y - aSize / 2;
                    if (drawFill)
                    {
                        g.FillRectangle(aBrush, aP.X, aP.Y, aSize, aSize);
                    }
                    if (drawOutline)
                    {
                        g.DrawRectangle(aPen, aP.X, aP.Y, aSize, aSize);
                    }
                    break;
                case PointStyle.Diamond:
                    points = new PointF[4];
                    bP.X = aP.X - aSize / 2;
                    bP.Y = aP.Y;
                    points[0] = bP;
                    bP.X = aP.X;
                    bP.Y = aP.Y - aSize / 2;
                    points[1] = bP;
                    bP.X = aP.X + aSize / 2;
                    bP.Y = aP.Y;
                    points[2] = bP;
                    bP.X = aP.X;
                    bP.Y = aP.Y + aSize / 2;
                    points[3] = bP;
                    if (drawFill)
                    {
                        g.FillPolygon(aBrush, points);
                    }
                    if (drawOutline)
                    {
                        g.DrawPolygon(aPen, points);
                    }
                    break;
                case PointStyle.UpTriangle:
                    points = new PointF[3];
                    bP.X = aP.X;
                    bP.Y = aP.Y - aSize / 2;
                    points[0] = bP;
                    bP.X = (Single)(aP.X + aSize / 4 * Math.Sqrt(3));
                    bP.Y = aP.Y + aSize / 4;
                    points[1] = bP;
                    bP.X = (Single)(aP.X - aSize / 4 * Math.Sqrt(3));
                    bP.Y = aP.Y + aSize / 4;
                    points[2] = bP;
                    if (drawFill)
                    {
                        g.FillPolygon(aBrush, points);
                    }
                    if (drawOutline)
                    {
                        g.DrawPolygon(aPen, points);
                    }
                    break;
                case PointStyle.DownTriangle:
                    points = new PointF[3];
                    bP.X = aP.X;
                    bP.Y = aP.Y + aSize / 2;
                    points[0] = bP;
                    bP.X = (Single)(aP.X - aSize / 4 * Math.Sqrt(3));
                    bP.Y = aP.Y - aSize / 2;
                    points[1] = bP;
                    bP.X = (Single)(aP.X + aSize / 4 * Math.Sqrt(3));
                    bP.Y = aP.Y - aSize / 2;
                    points[2] = bP;
                    if (drawFill)
                    {
                        g.FillPolygon(aBrush, points);
                    }
                    if (drawOutline)
                    {
                        g.DrawPolygon(aPen, points);
                    }
                    break;
                case PointStyle.XCross:
                    aPen.Color = color;
                    path.AddLine(aP.X - aSize / 2, aP.Y - aSize / 2, aP.X + aSize / 2, aP.Y + aSize / 2);
                    path.StartFigure();
                    path.AddLine(aP.X - aSize / 2, aP.Y + aSize / 2, aP.X + aSize / 2, aP.Y - aSize / 2);
                    if (drawFill || drawOutline)
                        g.DrawPath(aPen, path);
                    break;
                case PointStyle.Plus:
                    aPen.Color = color;
                    path.AddLine(aP.X, aP.Y - aSize / 2, aP.X, aP.Y + aSize / 2);
                    path.StartFigure();
                    path.AddLine(aP.X - aSize / 2, aP.Y, aP.X + aSize / 2, aP.Y);
                    if (drawFill || drawOutline)
                        g.DrawPath(aPen, path);
                    break;
                case PointStyle.StarLines:
                    aPen.Color = color;
                    path.AddLine(aP.X - aSize / 2, aP.Y - aSize / 2, aP.X + aSize / 2, aP.Y + aSize / 2);
                    path.StartFigure();
                    path.AddLine(aP.X - aSize / 2, aP.Y + aSize / 2, aP.X + aSize / 2, aP.Y - aSize / 2);
                    path.StartFigure();
                    path.AddLine(aP.X, aP.Y - aSize / 2, aP.X, aP.Y + aSize / 2);
                    path.StartFigure();
                    path.AddLine(aP.X - aSize / 2, aP.Y, aP.X + aSize / 2, aP.Y);
                    if (drawFill || drawOutline)
                        g.DrawPath(aPen, path);
                    break;
                case PointStyle.Star:
                    float vRadius = aSize / 2;
                    //Calculate 5 end points
                    PointF[] vPoints = new PointF[5];
                    double vAngle = 2.0 * Math.PI / 4 + Math.PI;
                    for (int i = 0; i < vPoints.Length; i++)
                    {
                        vAngle += 2.0 * Math.PI / (double)vPoints.Length;
                        vPoints[i] = new PointF(
                            (float)(Math.Cos(vAngle) * vRadius) + aP.X,
                            (float)(Math.Sin(vAngle) * vRadius) + aP.Y);
                    }
                    //Calculate 5 cross points
                    PointF[] cPoints = new PointF[5];
                    cPoints[0] = Contour.GetCrossPoint(vPoints[0], vPoints[2], vPoints[1], vPoints[4]);
                    cPoints[1] = Contour.GetCrossPoint(vPoints[1], vPoints[3], vPoints[0], vPoints[2]);
                    cPoints[2] = Contour.GetCrossPoint(vPoints[1], vPoints[3], vPoints[2], vPoints[4]);
                    cPoints[3] = Contour.GetCrossPoint(vPoints[0], vPoints[3], vPoints[2], vPoints[4]);
                    cPoints[4] = Contour.GetCrossPoint(vPoints[0], vPoints[3], vPoints[1], vPoints[4]);
                    //New points
                    PointF[] vTemps = new PointF[10];
                    for (int i = 0; i < 5; i++)
                    {
                        vTemps[i * 2] = vPoints[i];
                        vTemps[i * 2 + 1] = cPoints[i];
                    }
                    if (drawFill)
                        g.FillPolygon(aBrush, vTemps);
                    if (drawOutline)
                        g.DrawPolygon(aPen, vTemps);
                    break;
                case PointStyle.Pentagon:
                    vRadius = aSize / 2;
                    //Calculate 5 end points
                    vPoints = new PointF[5];
                    vAngle = 2.0 * Math.PI / 4 + Math.PI;
                    for (int i = 0; i < vPoints.Length; i++)
                    {
                        vAngle += 2.0 * Math.PI / (double)vPoints.Length;
                        vPoints[i] = new PointF(
                            (float)(Math.Cos(vAngle) * vRadius) + aP.X,
                            (float)(Math.Sin(vAngle) * vRadius) + aP.Y);
                    }
                    if (drawFill)
                        g.FillPolygon(aBrush, vPoints);
                    if (drawOutline)
                        g.DrawPolygon(aPen, vPoints);
                    break;
                case PointStyle.UpSemiCircle:
                    aP.X = aP.X - aSize / 2;
                    aP.Y = aP.Y - aSize / 2;
                    if (drawFill)
                    {
                        g.FillPie(aBrush, aP.X, aP.Y, aSize, aSize, 180, 180);
                    }
                    if (drawOutline)
                    {
                        g.DrawPie(aPen, aP.X, aP.Y, aSize, aSize, 180, 180);
                    }
                    break;
                case PointStyle.DownSemiCircle:
                    aP.X = aP.X - aSize / 2;
                    aP.Y = aP.Y - aSize / 2;
                    if (drawFill)
                    {
                        g.FillPie(aBrush, aP.X, aP.Y, aSize, aSize, 0, 180);
                    }
                    if (drawOutline)
                    {
                        g.DrawPie(aPen, aP.X, aP.Y, aSize, aSize, 0, 180);
                    }
                    break;
            }

            if (aPB.Angle != 0)
                g.Transform = oldMatrix;

            aPen.Dispose();
            aBrush.Dispose();
            path.Dispose();
            oldMatrix.Dispose();
        }

        private static void DrawPoint_Character(PointF aP, PointBreak aPB, Graphics g)
        {
            Matrix oldMatrix = g.Transform;
            if (aPB.Angle != 0)
            {
                Matrix myMatrix = new Matrix();
                myMatrix.RotateAt(aPB.Angle, aP);
                myMatrix.Translate(oldMatrix.OffsetX, oldMatrix.OffsetY, MatrixOrder.Append);
                g.Transform = myMatrix;
            }

            SolidBrush aBrush = new SolidBrush(aPB.Color);
            Font wFont = new Font(aPB.FontName, aPB.Size);
            PointF sPoint = aP;            
            sPoint.X = sPoint.X - aPB.Size / 2;
            sPoint.Y = sPoint.Y - aPB.Size / 2;
            string text = ((char)aPB.CharIndex).ToString();            
            
            g.DrawString(text, wFont, aBrush, sPoint);

            if (aPB.Angle != 0)
                g.Transform = oldMatrix;

            aBrush.Dispose();
            wFont.Dispose();
            oldMatrix.Dispose();
        }

        private static void DrawPoint_Image(PointF aP, PointBreak aPB, Graphics g)
        {
            Matrix oldMatrix = g.Transform;
            if (aPB.Angle != 0)
            {
                Matrix myMatrix = new Matrix();
                myMatrix.RotateAt(aPB.Angle, aP);
                myMatrix.Translate(oldMatrix.OffsetX, oldMatrix.OffsetY, MatrixOrder.Append);
                g.Transform = myMatrix;
                myMatrix.Dispose();
            }

            if (!File.Exists(aPB.ImagePath))
            {
                string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location.ToString());
                path = Path.Combine(path, "Image");
                aPB.ImagePath = Path.Combine(path, Path.GetFileName(aPB.ImagePath));
            }
            if (File.Exists(aPB.ImagePath))
            {
                Image image = null;
                if (Path.GetExtension(aPB.ImagePath).ToLower() == ".ico")
                    image = Bitmap.FromHicon(new Icon(aPB.ImagePath).Handle);
                else
                    image = new Bitmap(aPB.ImagePath);

                if (image != null)
                {
                    ((Bitmap)image).MakeTransparent(Color.White);
                    PointF sPoint = aP;
                    sPoint.X = sPoint.X - aPB.Size / 2;
                    sPoint.Y = sPoint.Y - aPB.Size / 2;
                    g.DrawImage(image, sPoint.X, sPoint.Y, aPB.Size, aPB.Size);
                }

                image.Dispose();
            }

            if (aPB.Angle != 0)
                g.Transform = oldMatrix;

            oldMatrix.Dispose();
        }

        /// <summary>
        /// Draw label point
        /// </summary>
        /// <param name="aPoint">screen point</param>
        /// <param name="aLB">label break</param>        
        /// <param name="g">graphics</param>
        /// <param name="rect">ref extent rectangle</param>
        public static void DrawLabelPoint(PointF aPoint, LabelBreak aLB, Graphics g, ref Rectangle rect)
        {
            Matrix oldMatrix = g.Transform;
            if (aLB.Angle != 0)
            {
                Matrix myMatrix = new Matrix();
                myMatrix.RotateAt(aLB.Angle, aPoint);
                myMatrix.Translate(oldMatrix.OffsetX, oldMatrix.OffsetY, MatrixOrder.Append);
                g.Transform = myMatrix;
                myMatrix.Dispose();
            }
            
            SizeF labSize = g.MeasureString(aLB.Text, aLB.Font);
            switch (aLB.AlignType)
            {
                case AlignType.Center:
                    aPoint.X = aPoint.X - labSize.Width / 2;
                    break;
                case AlignType.Left:
                    aPoint.X = aPoint.X - labSize.Width;
                    break;
            }
            aLB.YShift = labSize.Height / 2;
            aPoint.Y -= aLB.YShift;
            

            SolidBrush labelBrush = new SolidBrush(aLB.Color);
            g.DrawString(aLB.Text, aLB.Font, labelBrush, aPoint);

            rect.X = (int)aPoint.X;
            rect.Y = (int)aPoint.Y;
            rect.Width = (int)labSize.Width;
            rect.Height = (int)labSize.Height;

            if (aLB.Angle != 0)
                g.Transform = oldMatrix;

            labelBrush.Dispose();
            oldMatrix.Dispose();
        }


        ///// <summary>
        ///// Draw label point
        ///// </summary>
        ///// <param name="aPoint">screen point</param>
        ///// <param name="aLB">label break</param>        
        ///// <param name="g">graphics</param>
        ///// <param name="rect">ref extent rectangle</param>
        //public static void DrawLabelPoint(PointF aPoint, LabelBreak aLB, Graphics g, ref Rectangle rect)
        //{            
        //    SizeF labSize = g.MeasureString(aLB.Text, aLB.Font);
        //    switch (aLB.AlignType)
        //    {
        //        case AlignType.Center:
        //            aPoint.X = aPoint.X - labSize.Width / 2;
        //            break;
        //        case AlignType.Left:
        //            aPoint.X = aPoint.X - labSize.Width;
        //            break;
        //    }
        //    aLB.YShift = labSize.Height / 2;
        //    aPoint.Y -= aLB.YShift;

        //    Matrix oldMatrix = g.Transform;            
        //    if (aLB.Angle != 0)
        //    {
        //        Matrix myMatrix = new Matrix();
        //        myMatrix.RotateAt(aLB.Angle, aPoint);
        //        myMatrix.Translate(oldMatrix.OffsetX, oldMatrix.OffsetY, MatrixOrder.Append);
        //        g.Transform = myMatrix;
        //    }

        //    SolidBrush labelBrush = new SolidBrush(aLB.Color);
        //    g.DrawString(aLB.Text, aLB.Font, labelBrush, aPoint);

        //    ////Draw selected rectangle
        //    //if (isSelected)
        //    //{
        //    //    Pen aPen = new Pen(Color.Cyan);
        //    //    aPen.DashStyle = DashStyle.Dash;
        //    //    g.DrawRectangle(aPen, aPoint.X, aPoint.Y, labSize.Width, labSize.Height);
        //    //}

        //    rect.X = (int)aPoint.X;
        //    rect.Y = (int)aPoint.Y;
        //    rect.Width = (int)labSize.Width;
        //    rect.Height = (int)labSize.Height;

        //    if (aLB.Angle != 0)
        //        g.Transform = oldMatrix;
        //}

        /// <summary>
        /// Draw chart point
        /// </summary>
        /// <param name="aPoint">screen point</param>
        /// <param name="aCB">chart break</param>
        /// <param name="g">graphics</param>
        public static void DrawChartPoint(PointF aPoint, ChartBreak aCB, Graphics g)
        {                       
            switch (aCB.ChartType)
            {
                case ChartTypes.BarChart:
                    DrawBarChartSymbol(aPoint, aCB, g);
                    break;
                case ChartTypes.PieChart:
                    DrawPieChartSymbol(aPoint, aCB, g);
                    break;
            }

        }

        /// <summary>
        /// Draw bar chart symbol
        /// </summary>
        /// <param name="aPoint">start point</param>
        /// <param name="aCB">chart break</param>
        /// <param name="g">graphics</param>
        public static void DrawBarChartSymbol(PointF aPoint, ChartBreak aCB, Graphics g)
        {
            Font font = new Font("Arial", 8);
            DrawBarChartSymbol(aPoint, aCB, g, false, font);
            font.Dispose();
        }

        /// <summary>
        /// Draw bar chart symbol
        /// </summary>
        /// <param name="aPoint">start point</param>
        /// <param name="aCB">chart break</param>
        /// <param name="g">graphics</param>
        /// <param name="drawValue">If draw value</param>
        /// <param name="font">Value font</param>
        public static void DrawBarChartSymbol(PointF aPoint, ChartBreak aCB, Graphics g, bool drawValue, Font font)
        {
            List<int> heights = aCB.GetBarHeights();
            float y = aPoint.Y;
            Pen aPen = new Pen(Color.Black);
            Brush aBrush = new SolidBrush(Color.Black);
            for (int i = 0; i < heights.Count; i++)
            {
                if (heights[i] <= 0)
                {
                    aPoint.X += aCB.BarWidth;
                    continue;
                }

                aPoint.Y = y - heights[i];
                PolygonBreak aPGB = (PolygonBreak)aCB.LegendScheme.LegendBreaks[i];
                if (aCB.View3D)
                {
                    Color aColor = ColorUtils.ModifyBrightness(aPGB.Color, 0.5);
                    aPen.Color = aPGB.OutlineColor;
                    aBrush = new SolidBrush(aColor);
                    PointF[] points = new PointF[4];
                    points[0] = new PointF(aPoint.X, aPoint.Y);
                    points[1] = new PointF(aPoint.X + aCB.BarWidth, aPoint.Y);
                    points[2] = new PointF(points[1].X + aCB.Thickness, points[1].Y - aCB.Thickness);
                    points[3] = new PointF(points[0].X + aCB.Thickness, points[0].Y - aCB.Thickness);
                    g.FillPolygon(aBrush, points);
                    g.DrawPolygon(aPen, points);

                    points[0] = new PointF(aPoint.X + aCB.BarWidth, aPoint.Y);
                    points[1] = new PointF(aPoint.X + aCB.BarWidth, aPoint.Y + heights[i]);
                    points[2] = new PointF(points[1].X + aCB.Thickness, points[1].Y - aCB.Thickness);
                    points[3] = new PointF(points[0].X + aCB.Thickness, points[0].Y - aCB.Thickness);
                    g.FillPolygon(aBrush, points);
                    g.DrawPolygon(aPen, points);
                }
                DrawRectangle(aPoint, aCB.BarWidth, heights[i], aPGB, g);

                aPoint.X += aCB.BarWidth;

                if (i == heights.Count - 1)
                {
                    if (drawValue)
                    {
                        String vstr = aCB.ChartData[i].ToString("0");
                        SizeF aSF = g.MeasureString(vstr, font);
                        aPoint.X += 2;
                        aPoint.Y = y - heights[i] / 2 - aSF.Height / 2;
                        aBrush = new SolidBrush(Color.Black);
                        g.DrawString(vstr, font, aBrush, aPoint);
                    }
                }
            }

            aPen.Dispose();
            aBrush.Dispose();
        }

        /// <summary>
        /// Draw pie chart symbol
        /// </summary>
        /// <param name="aPoint">start point</param>
        /// <param name="aCB">chart break</param>
        /// <param name="g">graphics</param>
        public static void DrawPieChartSymbol(PointF aPoint, ChartBreak aCB, Graphics g)
        {
            int width = aCB.GetWidth();
            int height = aCB.GetHeight();
            if (width <= 0 || height <= 0)
                return;

            aPoint.Y -= height;
            List<List<float>> angles = aCB.GetPieAngles();
            float startAngle, sweepAngle;
            int i;
            Pen aPen = new Pen(Color.Black);
            Brush aBrush = new SolidBrush(Color.Black);

            if (aCB.View3D)
            {
                aPoint.Y = aPoint.Y + width / 6 - aCB.Thickness;
                for (i = 0; i < angles.Count; i++)
                {
                    startAngle = angles[i][0];
                    sweepAngle = angles[i][1];
                    PolygonBreak aPGB = (PolygonBreak)aCB.LegendScheme.LegendBreaks[i];
                    if (startAngle < 180)
                    {
                        PointF bPoint = new PointF(aPoint.X, aPoint.Y + aCB.Thickness);
                        Color aColor = ColorUtils.ModifyBrightness(aPGB.Color, 0.5);
                        aPen.Color = aPGB.OutlineColor;
                        aBrush = new SolidBrush(aColor);
                        g.FillPie(aBrush, bPoint.X, bPoint.Y, width, width * 2 / 3, startAngle, sweepAngle);
                        g.DrawPie(aPen, bPoint.X, bPoint.Y, width, width * 2 / 3, startAngle, sweepAngle);
                    }
                }
                float a = (float)width / 2;
                float b = (float)width / 3;
                float x0 = aPoint.X + a;
                float y0 = aPoint.Y + b;
                float sA, eA;
                for (i = 0; i < angles.Count; i++)
                {
                    startAngle = angles[i][0];
                    sweepAngle = angles[i][1];
                    if (startAngle < 180)
                    {
                        sA = (float)(startAngle / 180 * Math.PI);
                        eA = (float)((startAngle + sweepAngle) / 180 * Math.PI);
                        PolygonBreak aPGB = (PolygonBreak)aCB.LegendScheme.LegendBreaks[i];
                        PointF bPoint = MIMath.CalEllipseCoordByAngle(x0, y0, a, b, sA);
                        PointF cPoint = new PointF(x0 - a, y0);
                        if (eA < Math.PI)
                            cPoint = MIMath.CalEllipseCoordByAngle(x0, y0, a, b, eA);

                        Color aColor = ColorUtils.ModifyBrightness(aPGB.Color, 0.5);
                        PointF[] points = new PointF[4];
                        points[0] = cPoint;
                        points[1] = new PointF(cPoint.X, cPoint.Y + aCB.Thickness);
                        points[2] = new PointF(bPoint.X, bPoint.Y + aCB.Thickness);
                        points[3] = bPoint;
                        aPen.Color = aPGB.OutlineColor;
                        aBrush = new SolidBrush(aColor);
                        g.FillPolygon(aBrush, points);
                        g.DrawLine(aPen, points[0], points[1]);
                        g.DrawLine(aPen, points[2], points[3]);
                    }
                }
                for (i = 0; i < angles.Count; i++)
                {
                    startAngle = angles[i][0];
                    sweepAngle = angles[i][1];
                    PolygonBreak aPGB = (PolygonBreak)aCB.LegendScheme.LegendBreaks[i];
                    DrawPie(aPoint, width, width * 2 / 3, startAngle, sweepAngle, aPGB, g);
                }
            }
            else
            {
                for (i = 0; i < angles.Count; i++)
                {
                    startAngle = angles[i][0];
                    sweepAngle = angles[i][1];
                    PolygonBreak aPGB = (PolygonBreak)aCB.LegendScheme.LegendBreaks[i];
                    DrawPie(aPoint, width, width, startAngle, sweepAngle, aPGB, g);
                }
            }

            aPen.Dispose();
            aBrush.Dispose();
        }

        /// <summary>
        /// Draw polyline symbol
        /// </summary>
        /// <param name="aDS"></param>
        /// <param name="aP"></param>
        /// <param name="aColor"></param>
        /// <param name="aSize"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="g"></param>
        public static void DrawPolyLineSymbol(DashStyle aDS, PointF aP, Color aColor, Single aSize,
            Single width, Single height, Graphics g)
        {
            PointF[] points = new PointF[4];
            PointF aPoint = new PointF(0, 0);
            aPoint.X = aP.X - width / 2;
            aPoint.Y = aP.Y + height / 2;
            points[0] = aPoint;
            aPoint.X = aP.X - width / 6;
            aPoint.Y = aP.Y - height / 2;
            points[1] = aPoint;
            aPoint.X = aP.X + width / 6;
            aPoint.Y = aP.Y + height / 2;
            points[2] = aPoint;
            aPoint.X = aP.X + width / 2;
            aPoint.Y = aP.Y - height / 2;
            points[3] = aPoint;

            Pen aPen = new Pen(aColor);
            aPen.Width = aSize;
            aPen.DashStyle = aDS;

            g.DrawLines(aPen, points);

            aPen.Dispose();
        }

        /// <summary>
        /// Draw polyline symbol
        /// </summary>
        /// <param name="aP"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="aPLB"></param>
        /// <param name="g"></param>
        public static void DrawPolyLineSymbol(PointF aP, Single width, Single height, PolyLineBreak aPLB, Graphics g)
        {
            Pen aPen = new Pen(Color.Black);
            if (aPLB.IsUsingDashStyle)
            {
                PointF[] points = new PointF[4];
                PointF aPoint = new PointF(0, 0);
                aPoint.X = aP.X - width / 2;
                aPoint.Y = aP.Y + height / 2;
                points[0] = aPoint;
                aPoint.X = aP.X - width / 6;
                aPoint.Y = aP.Y - height / 2;
                points[1] = aPoint;
                aPoint.X = aP.X + width / 6;
                aPoint.Y = aP.Y + height / 2;
                points[2] = aPoint;
                aPoint.X = aP.X + width / 2;
                aPoint.Y = aP.Y - height / 2;
                points[3] = aPoint;

                aPen.Color = aPLB.Color;
                aPen.Width = aPLB.Size;
                aPen.DashStyle = aPLB.DashStyle;

                if (aPLB.DrawPolyline)
                    g.DrawLines(aPen, points);

                //Draw symbol
                if (aPLB.DrawSymbol)
                {
                    DrawPoint(aPLB.SymbolStyle, points[1], aPLB.SymbolColor, aPLB.SymbolColor, aPLB.SymbolSize, true, false, g);
                    DrawPoint(aPLB.SymbolStyle, points[2], aPLB.SymbolColor, aPLB.SymbolColor, aPLB.SymbolSize, true, false, g);
                }
            }
            else
            {
                PointF[] points = new PointF[2];
                PointF aPoint = new PointF(0, 0);
                aPoint.X = aP.X - width / 2;
                aPoint.Y = aP.Y;
                points[0] = aPoint;                
                aPoint.X = aP.X + width / 2;
                aPoint.Y = aP.Y;
                points[1] = aPoint;
                float lineWidth = 2.0f;
                switch (aPLB.Style)
                {
                    case LineStyles.ColdFront:                        
                        PointBreak aPB = new PointBreak();
                        aPB.Size = 14;
                        aPB.Color = Color.Blue;
                        aPB.Style = PointStyle.UpTriangle;
                        aPB.OutlineColor = Color.Blue;
                        aPB.DrawFill = true;
                        aPB.DrawOutline = true;
                        DrawPoint_Simple(new PointF(aP.X - aPB.Size * 2 / 3, aP.Y - aPB.Size / 4), aPB, g);
                        DrawPoint_Simple(new PointF(aP.X + aPB.Size * 2 / 3, aP.Y - aPB.Size / 4), aPB, g);

                        aPen.Color = Color.Blue;
                        aPen.Width = lineWidth;
                        g.DrawLines(aPen, points);
                        break;
                    case LineStyles.WarmFront:                        
                        aPB = new PointBreak();
                        aPB.Size = 14;
                        aPB.Color = Color.Red;
                        aPB.Style = PointStyle.UpSemiCircle;
                        aPB.OutlineColor = Color.Red;
                        aPB.DrawFill = true;
                        aPB.DrawOutline = true;
                        DrawPoint_Simple(new PointF(aP.X - aPB.Size * 2 / 3, aP.Y), aPB, g);
                        DrawPoint_Simple(new PointF(aP.X + aPB.Size * 2 / 3, aP.Y), aPB, g);

                        aPen.Color = Color.Red;
                        aPen.Width = lineWidth;
                        g.DrawLines(aPen, points);
                        break;
                    case LineStyles.OccludedFront:
                        Color aColor = Color.FromArgb(255, 0, 255);
                        aPB = new PointBreak();
                        aPB.Size = 14;
                        aPB.Color = aColor;
                        aPB.Style = PointStyle.UpTriangle;
                        aPB.OutlineColor = aColor;
                        aPB.DrawFill = true;
                        aPB.DrawOutline = true;
                        DrawPoint_Simple(new PointF(aP.X - aPB.Size * 2 / 3, aP.Y - aPB.Size / 4), aPB, g);
                        aPB = new PointBreak();
                        aPB.Size = 14;
                        aPB.Color = aColor;
                        aPB.Style = PointStyle.UpSemiCircle;
                        aPB.OutlineColor = aColor;
                        aPB.DrawFill = true;
                        aPB.DrawOutline = true;
                        DrawPoint_Simple(new PointF(aP.X + aPB.Size * 2 / 3, aP.Y), aPB, g);

                        aPen.Color = aColor;
                        aPen.Width = lineWidth;
                        g.DrawLines(aPen, points);
                        break;
                    case LineStyles.StationaryFront:
                        aPB = new PointBreak();
                        aPB.Size = 14;
                        aPB.Color = Color.Blue;
                        aPB.Style = PointStyle.UpTriangle;
                        aPB.OutlineColor = Color.Blue;
                        aPB.DrawFill = true;
                        aPB.DrawOutline = true;
                        DrawPoint_Simple(new PointF(aP.X - aPB.Size * 2 / 3, aP.Y - aPB.Size / 4), aPB, g);
                        aPB = new PointBreak();
                        aPB.Size = 14;
                        aPB.Color = Color.Red;
                        aPB.Style = PointStyle.DownSemiCircle;
                        aPB.OutlineColor = Color.Red;
                        aPB.DrawFill = true;
                        aPB.DrawOutline = true;
                        DrawPoint_Simple(new PointF(aP.X + aPB.Size * 2 / 3, aP.Y), aPB, g);

                        aPen.Color = Color.Blue;
                        aPen.Width = lineWidth;
                        g.DrawLines(aPen, points);
                        break;
                }
            }
            aPen.Dispose();
        }

        /// <summary>
        /// draw polygon symbol
        /// </summary>
        /// <param name="aP"></param>
        /// <param name="aColor"></param>
        /// <param name="outlineColor"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="drawFill"></param>
        /// <param name="drawOutline"></param>
        /// <param name="g"></param>
        public static void DrawPolygonSymbol(PointF aP, Color aColor, Color outlineColor,
            Single width, Single height, Boolean drawFill, Boolean drawOutline, Graphics g)
        {
            SolidBrush aBrush = new SolidBrush(aColor);
            Pen aPen = new Pen(outlineColor);
            aP.X = aP.X - width / 2;
            aP.Y = aP.Y - height / 2;
            if (drawFill)
            {
                g.FillRectangle(aBrush, aP.X, aP.Y, width, height);
            }
            if (drawOutline)
            {
                g.DrawRectangle(aPen, aP.X, aP.Y, width, height);
            }

            aPen.Dispose();
            aBrush.Dispose();
        }

        /// <summary>
        /// Draw polygon symbol
        /// </summary>
        /// <param name="aP">start point</param>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="aPGB">polygon break</param>
        /// <param name="transparencyPerc">transparency percent</param>
        /// <param name="g">graphics</param>
        public static void DrawPolygonSymbol(PointF aP, Single width, Single height, PolygonBreak aPGB,
            int transparencyPerc, Graphics g)
        {
            int alpha = (int)((1 - (double)transparencyPerc / 100.0) * 255);
            Color aColor = Color.FromArgb(alpha, aPGB.Color);
            Brush aBrush;
            if (aPGB.UsingHatchStyle)
                aBrush = new HatchBrush(aPGB.Style, aColor, aPGB.BackColor);
            else
                aBrush = new SolidBrush(aColor);

            Pen aPen = new Pen(aPGB.OutlineColor, aPGB.OutlineSize);
            aP.X = aP.X - width / 2;
            aP.Y = aP.Y - height / 2;
            if (aPGB.DrawFill)
            {
                g.FillRectangle(aBrush, aP.X, aP.Y, width, height);
            }
            if (aPGB.DrawOutline)
            {
                g.DrawRectangle(aPen, aP.X, aP.Y, width, height);
            }

            aPen.Dispose();
            aBrush.Dispose();
        }

        /// <summary>
        /// draw polygon
        /// </summary>
        /// <param name="points"></param>
        /// <param name="aColor"></param>
        /// <param name="outlineColor"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="drawFill"></param>
        /// <param name="drawOutline"></param>
        /// <param name="g"></param>
        public static void DrawPolygon(PointF[] points, Color aColor, Color outlineColor,
            Single width, Single height, Boolean drawFill, Boolean drawOutline, Graphics g)
        {
            SolidBrush aBrush = new SolidBrush(aColor);
            Pen aPen = new Pen(outlineColor);            
            if (drawFill)
            {                
                g.FillPolygon(aBrush, points);
            }
            if (drawOutline)
            {
                g.DrawPolygon(aPen, points);
            }

            aPen.Dispose();
            aBrush.Dispose();
        }

        /// <summary>
        /// Draw polygon
        /// </summary>
        /// <param name="points"></param>        
        /// <param name="aPGB"></param>        
        /// <param name="g"></param>
        public static void DrawPolygon(PointF[] points, PolygonBreak aPGB, Graphics g)
        {
            //int alpha = (int)((1 - (double)aPGB.TransparencyPercent / 100.0) * 255);
            //Color aColor = Color.FromArgb(alpha, aPGB.Color);
            Color aColor = aPGB.Color;
            Brush aBrush;
            if (aPGB.UsingHatchStyle)
                aBrush = new HatchBrush(aPGB.Style, aColor, aPGB.BackColor);
            else
                aBrush = new SolidBrush(aColor);

            Pen aPen = new Pen(aPGB.OutlineColor);
            aPen.Width = aPGB.OutlineSize;
            if (aPGB.DrawFill)
            {
                g.FillPolygon(aBrush, points);
            }
            if (aPGB.DrawOutline)
            {
                g.DrawPolygon(aPen, points);
            }

            aPen.Dispose();
            aBrush.Dispose();
        }

        /// <summary>
        /// Draw circle
        /// </summary>
        /// <param name="points">points</param>
        /// <param name="aPGB">polygon break</param>
        /// <param name="g">graphics</param>
        public static void DrawCircle(PointF[] points, PolygonBreak aPGB, Graphics g)
        {
            float radius = Math.Abs(points[1].X - points[0].X);

            //int alpha = (int)((1 - (double)aPGB.TransparencyPercent / 100.0) * 255);
            //Color aColor = Color.FromArgb(alpha, aPGB.Color);
            Color aColor = aPGB.Color;
            Brush aBrush;
            if (aPGB.UsingHatchStyle)
                aBrush = new HatchBrush(aPGB.Style, aColor, aPGB.BackColor);
            else
                aBrush = new SolidBrush(aColor);

            Pen aPen = new Pen(aPGB.OutlineColor);
            aPen.Width = aPGB.OutlineSize;
            if (aPGB.DrawFill)
                g.FillEllipse(aBrush, points[0].X, points[0].Y - radius, radius * 2, radius * 2);
            if (aPGB.DrawOutline)
                g.DrawEllipse(aPen, points[0].X, points[0].Y - radius, radius * 2, radius * 2);

            aPen.Dispose();
            aBrush.Dispose();
        }

        /// <summary>
        /// Draw ellipse
        /// </summary>
        /// <param name="points">points</param>
        /// <param name="aPGB">polygon break</param>
        /// <param name="g">graphics</param>
        public static void DrawEllipse(PointF[] points, PolygonBreak aPGB, Graphics g)
        {
            float sx = Math.Min(points[0].X, points[2].X);
            float sy = Math.Min(points[0].Y, points[2].Y);
            float width = Math.Abs(points[2].X - points[0].X);
            float height = Math.Abs(points[2].Y - points[0].Y);

            //int alpha = (int)((1 - (double)aPGB.TransparencyPercent / 100.0) * 255);
            //Color aColor = Color.FromArgb(alpha, aPGB.Color);
            Color aColor = aPGB.Color;
            Brush aBrush;
            if (aPGB.UsingHatchStyle)
                aBrush = new HatchBrush(aPGB.Style, aColor, aPGB.BackColor);
            else
                aBrush = new SolidBrush(aColor);

            Pen aPen = new Pen(aPGB.OutlineColor);
            aPen.Width = aPGB.OutlineSize;
            if (aPGB.DrawFill)
                g.FillEllipse(aBrush, sx, sy, width, height);
            if (aPGB.DrawOutline)
                g.DrawEllipse(aPen, sx, sy, width, height);

            aPen.Dispose();
            aBrush.Dispose();
        }

        /// <summary>
        /// Draw rectangle
        /// </summary>
        /// <param name="aPoint">start point</param>  
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="aPGB">polygon break</param>        
        /// <param name="g">graphics</param>
        public static void DrawRectangle(PointF aPoint, float width, float height, PolygonBreak aPGB, Graphics g)
        {
            //int alpha = (int)((1 - (double)aPGB.TransparencyPercent / 100.0) * 255);
            //Color aColor = Color.FromArgb(alpha, aPGB.Color);
            Color aColor = aPGB.Color;
            Brush aBrush;
            if (aPGB.UsingHatchStyle)
                aBrush = new HatchBrush(aPGB.Style, aColor, aPGB.BackColor);
            else
                aBrush = new SolidBrush(aColor);

            Pen aPen = new Pen(aPGB.OutlineColor);
            aPen.Width = aPGB.OutlineSize;
            if (aPGB.DrawFill)
            {
                g.FillRectangle(aBrush, aPoint.X, aPoint.Y, width, height);
            }
            if (aPGB.DrawOutline)
            {
                g.DrawRectangle(aPen, aPoint.X, aPoint.Y, width, height);
            }

            aPen.Dispose();
            aBrush.Dispose();
        }

        /// <summary>
        /// Draw pie
        /// </summary>
        /// <param name="aPoint">start point</param>  
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="startAngle">start angle</param>
        /// <param name="sweepAngle">sweep angle</param>
        /// <param name="aPGB">polygon break</param>        
        /// <param name="g">graphics</param>
        public static void DrawPie(PointF aPoint, float width, float height, float startAngle, float sweepAngle, PolygonBreak aPGB, Graphics g)
        {
            //int alpha = (int)((1 - (double)aPGB.TransparencyPercent / 100.0) * 255);
            //Color aColor = Color.FromArgb(alpha, aPGB.Color);
            Color aColor = aPGB.Color;
            Brush aBrush;
            if (aPGB.UsingHatchStyle)
                aBrush = new HatchBrush(aPGB.Style, aColor, aPGB.BackColor);
            else
                aBrush = new SolidBrush(aColor);

            Pen aPen = new Pen(aPGB.OutlineColor);
            aPen.Width = aPGB.OutlineSize;
            if (aPGB.DrawFill)
            {
                g.FillPie(aBrush, aPoint.X, aPoint.Y, width, height, startAngle, sweepAngle);
            }
            if (aPGB.DrawOutline)
            {
                g.DrawPie(aPen, aPoint.X, aPoint.Y, width, height, startAngle, sweepAngle);
            }

            aPen.Dispose();
            aBrush.Dispose();
        }

        /// <summary>
        /// Draw polyline
        /// </summary>
        /// <param name="points">points</param>
        /// <param name="aPLB">polyline break</param>
        /// <param name="g">graphics</param>
        public static void DrawPolyline(PointF[] points, PolyLineBreak aPLB, Graphics g)
        {
            if (aPLB.IsUsingDashStyle)
            {
                Pen aPen = new Pen(aPLB.Color);
                aPen.DashStyle = aPLB.DashStyle;
                aPen.Width = aPLB.Size;
                g.DrawLines(aPen, points);

                //Draw symbol            
                if (aPLB.DrawSymbol)
                {
                    SmoothingMode oldSMode = g.SmoothingMode;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    for (int i = 0; i < points.Length; i++)
                    {
                        if (i % aPLB.SymbolInterval == 0)
                            Draw.DrawPoint(aPLB.SymbolStyle, points[i], aPLB.SymbolColor, aPLB.SymbolColor, aPLB.SymbolSize, true, false, g);
                    }
                    g.SmoothingMode = oldSMode;
                }

                aPen.Dispose();
            }
            else
            {
                Shape.PolyLine aPLine = new Shape.PolyLine();
                aPLine.SetPoints(points);
                List<double[]> pos = aPLine.GetPositions(30);
                float aSize = 16;
                int i;
                Pen aPen = new Pen(Color.Black);
                switch (aPLB.Style)
                {
                    case LineStyles.ColdFront:
                        if (pos != null)
                        {
                            PointBreak aPB = new PointBreak();
                            aPB.Size = aSize;
                            aPB.Color = Color.Blue;
                            aPB.Style = PointStyle.UpTriangle;
                            aPB.OutlineColor = Color.Blue;
                            aPB.DrawFill = true;
                            aPB.DrawOutline = true;
                            for (i = 0; i < pos.Count; i++)
                            {
                                aPB.Angle = (float)pos[i][2];
                                DrawPoint_Simple_Up(new PointF((float)pos[i][0], (float)pos[i][1]), aPB, g);
                            }
                        }

                        aPen.Color = Color.Blue;
                        aPen.Width = aPLB.Size;
                        g.DrawLines(aPen, points);
                        break;
                    case LineStyles.WarmFront:
                        if (pos != null)
                        {
                            PointBreak aPB = new PointBreak();
                            aPB.Size = aSize;
                            aPB.Color = Color.Red;
                            aPB.Style = PointStyle.UpSemiCircle;
                            aPB.OutlineColor = Color.Red;
                            aPB.DrawFill = true;
                            aPB.DrawOutline = true;
                            for (i = 0; i < pos.Count; i++)
                            {
                                aPB.Angle = (float)pos[i][2];
                                DrawPoint_Simple(new PointF((float)pos[i][0], (float)pos[i][1]), aPB, g);
                            }
                        }

                        aPen.Color = Color.Red;
                        aPen.Width = aPLB.Size;
                        g.DrawLines(aPen, points);
                        break;
                    case LineStyles.OccludedFront:
                        Color aColor = Color.FromArgb(255, 0, 255);
                        if (pos != null)
                        {
                            PointBreak aPB = new PointBreak();
                            aPB.Size = aSize;
                            aPB.Color = aColor;
                            aPB.Style = PointStyle.UpTriangle;
                            aPB.OutlineColor = aColor;
                            aPB.DrawFill = true;
                            aPB.DrawOutline = true;
                            for (i = 0; i < pos.Count; i += 2)
                            {
                                aPB.Angle = (float)pos[i][2];
                                DrawPoint_Simple_Up(new PointF((float)pos[i][0], (float)pos[i][1]), aPB, g);
                            }

                            aPB = new PointBreak();
                            aPB.Size = aSize;
                            aPB.Color = aColor;
                            aPB.Style = PointStyle.UpSemiCircle;
                            aPB.OutlineColor = aColor;
                            aPB.DrawFill = true;
                            aPB.DrawOutline = true;
                            for (i = 1; i < pos.Count; i += 2)
                            {
                                aPB.Angle = (float)pos[i][2];
                                DrawPoint_Simple(new PointF((float)pos[i][0], (float)pos[i][1]), aPB, g);
                            }
                        }

                        aPen.Color = aColor;
                        aPen.Width = aPLB.Size;
                        g.DrawLines(aPen, points);
                        break;
                    case LineStyles.StationaryFront:
                        if (pos != null)
                        {
                            PointBreak aPB = new PointBreak();
                            aPB.Size = aSize;
                            aPB.Color = Color.Blue;
                            aPB.Style = PointStyle.UpTriangle;
                            aPB.OutlineColor = Color.Blue;
                            aPB.DrawFill = true;
                            aPB.DrawOutline = true;
                            for (i = 0; i < pos.Count; i += 2)
                            {
                                aPB.Angle = (float)pos[i][2];
                                DrawPoint_Simple_Up(new PointF((float)pos[i][0], (float)pos[i][1]), aPB, g);
                            }

                            aPB = new PointBreak();
                            aPB.Size = aSize;
                            aPB.Color = Color.Red;
                            aPB.Style = PointStyle.DownSemiCircle;
                            aPB.OutlineColor = Color.Red;
                            aPB.DrawFill = true;
                            aPB.DrawOutline = true;
                            for (i = 1; i < pos.Count; i += 2)
                            {
                                aPB.Angle = (float)pos[i][2];
                                DrawPoint_Simple(new PointF((float)pos[i][0], (float)pos[i][1]), aPB, g);
                            }
                        }

                        aPen.Color = Color.Blue;
                        aPen.Width = aPLB.Size;
                        g.DrawLines(aPen, points);
                        break;
                }

                aPen.Dispose();
            }
        }

        /// <summary>
        /// Draw Curve line
        /// </summary>
        /// <param name="points">points</param>
        /// <param name="aPLB">polyline break</param>
        /// <param name="g">graphics</param>
        public static void DrawCurveLine(PointF[] points, PolyLineBreak aPLB, Graphics g)
        {
            List<PointD> opoints = new List<PointD>();
            int i;
            for (i = 0; i < points.Length; i++)
                opoints.Add(new PointD(points[i].X, points[i].Y));

            //PointD[] rPoints = Spline.CardinalSpline(opoints, 0.5, false);
            PointD[] rPoints = Spline.CardinalSpline(opoints.ToArray(), 5);
            PointF[] dPoints = new PointF[rPoints.Length];
            for (i = 0; i < dPoints.Length; i++)
                dPoints[i] = new PointF((float)rPoints[i].X, (float)rPoints[i].Y);

            if (aPLB.IsUsingDashStyle)
            {
                Pen aPen = new Pen(aPLB.Color);
                aPen.DashStyle = aPLB.DashStyle;
                aPen.Width = aPLB.Size;                

                //g.DrawCurve(aPen, points);
                g.DrawLines(aPen, dPoints);

                //Draw symbol            
                if (aPLB.DrawSymbol)
                {
                    SmoothingMode oldSMode = g.SmoothingMode;
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    for (i = 0; i < points.Length; i++)
                    {
                        if (i % aPLB.SymbolInterval == 0)
                            Draw.DrawPoint(aPLB.SymbolStyle, points[i], aPLB.SymbolColor, aPLB.SymbolColor, aPLB.SymbolSize, true, false, g);
                    }
                    g.SmoothingMode = oldSMode;
                }
            }
            else
            {
                Shape.PolyLine aPLine = new Shape.PolyLine();
                aPLine.SetPoints(dPoints);
                List<double[]> pos = aPLine.GetPositions(30);
                float aSize = 16;
                Pen aPen = new Pen(Color.Black);
                switch (aPLB.Style)
                {
                    case LineStyles.ColdFront:
                        if (pos != null)
                        {
                            PointBreak aPB = new PointBreak();
                            aPB.Size = aSize;
                            aPB.Color = Color.Blue;
                            aPB.Style = PointStyle.UpTriangle;
                            aPB.OutlineColor = Color.Blue;
                            aPB.DrawFill = true;
                            aPB.DrawOutline = true;
                            for (i = 0; i < pos.Count; i++)
                            {
                                aPB.Angle = (float)pos[i][2];
                                DrawPoint_Simple_Up(new PointF((float)pos[i][0], (float)pos[i][1]), aPB, g);
                            }
                        }

                        aPen.Color = Color.Blue;
                        aPen.Width = aPLB.Size;
                        g.DrawLines(aPen, dPoints);
                        break;
                    case LineStyles.WarmFront:
                        if (pos != null)
                        {
                            PointBreak aPB = new PointBreak();
                            aPB.Size = aSize;
                            aPB.Color = Color.Red;
                            aPB.Style = PointStyle.UpSemiCircle;
                            aPB.OutlineColor = Color.Red;
                            aPB.DrawFill = true;
                            aPB.DrawOutline = true;
                            for (i = 0; i < pos.Count; i++)
                            {
                                aPB.Angle = (float)pos[i][2];
                                DrawPoint_Simple(new PointF((float)pos[i][0], (float)pos[i][1]), aPB, g);
                            }
                        }

                        aPen.Color = Color.Red;
                        aPen.Width = aPLB.Size;
                        g.DrawLines(aPen, dPoints);
                        break;
                    case LineStyles.OccludedFront:
                        Color aColor = Color.FromArgb(255, 0, 255);
                        if (pos != null)
                        {
                            PointBreak aPB = new PointBreak();
                            aPB.Size = aSize;
                            aPB.Color = aColor;
                            aPB.Style = PointStyle.UpTriangle;
                            aPB.OutlineColor = aColor;
                            aPB.DrawFill = true;
                            aPB.DrawOutline = true;
                            for (i = 0; i < pos.Count; i += 2)
                            {
                                aPB.Angle = (float)pos[i][2];
                                DrawPoint_Simple_Up(new PointF((float)pos[i][0], (float)pos[i][1]), aPB, g);
                            }

                            aPB = new PointBreak();
                            aPB.Size = aSize;
                            aPB.Color = aColor;
                            aPB.Style = PointStyle.UpSemiCircle;
                            aPB.OutlineColor = aColor;
                            aPB.DrawFill = true;
                            aPB.DrawOutline = true;
                            for (i = 1; i < pos.Count; i += 2)
                            {
                                aPB.Angle = (float)pos[i][2];
                                DrawPoint_Simple(new PointF((float)pos[i][0], (float)pos[i][1]), aPB, g);
                            }
                        }

                        aPen.Color = aColor;
                        aPen.Width = aPLB.Size;
                        g.DrawLines(aPen, dPoints);
                        break;
                    case LineStyles.StationaryFront:
                        if (pos != null)
                        {
                            PointBreak aPB = new PointBreak();
                            aPB.Size = aSize;
                            aPB.Color = Color.Blue;
                            aPB.Style = PointStyle.UpTriangle;
                            aPB.OutlineColor = Color.Blue;
                            aPB.DrawFill = true;
                            aPB.DrawOutline = true;
                            for (i = 0; i < pos.Count; i += 2)
                            {
                                aPB.Angle = (float)pos[i][2];
                                DrawPoint_Simple_Up(new PointF((float)pos[i][0], (float)pos[i][1]), aPB, g);
                            }

                            aPB = new PointBreak();
                            aPB.Size = aSize;
                            aPB.Color = Color.Red;
                            aPB.Style = PointStyle.DownSemiCircle;
                            aPB.OutlineColor = Color.Red;
                            aPB.DrawFill = true;
                            aPB.DrawOutline = true;
                            for (i = 1; i < pos.Count; i += 2)
                            {
                                aPB.Angle = (float)pos[i][2];
                                DrawPoint_Simple(new PointF((float)pos[i][0], (float)pos[i][1]), aPB, g);
                            }
                        }

                        aPen.Color = Color.Blue;
                        aPen.Width = aPLB.Size;
                        g.DrawLines(aPen, dPoints);
                        break;
                }

                aPen.Dispose();
            }
        }

        /// <summary>
        /// Draw Curve polygon
        /// </summary>
        /// <param name="points">points</param>
        /// <param name="aPGB">polygon break</param>
        /// <param name="g">graphics</param>
        public static void DrawCurvePolygon(PointF[] points, PolygonBreak aPGB, Graphics g)
        {
            //int alpha = (int)((1 - (double)aPGB.TransparencyPercent / 100.0) * 255);
            //Color aColor = Color.FromArgb(alpha, aPGB.Color);
            Color aColor = aPGB.Color;
            Brush aBrush;
            if (aPGB.UsingHatchStyle)
                aBrush = new HatchBrush(aPGB.Style, aColor, aPGB.BackColor);
            else
                aBrush = new SolidBrush(aColor);

            Pen aPen = new Pen(aPGB.OutlineColor);
            aPen.Width = aPGB.OutlineSize;
            if (aPGB.DrawFill)
            {
                g.FillClosedCurve(aBrush, points);
            }
            if (aPGB.DrawOutline)
            {
                g.DrawClosedCurve(aPen, points);
            }

            aPen.Dispose();
            aBrush.Dispose();
        }

        /// <summary>
        /// Get maximum and minimum wind speed from wind arraw list
        /// </summary>
        /// <param name="arrawList"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public static void GetMaxMinWindSpeed(List<WindArraw> arrawList, ref Single min, ref Single max)
        {
            int i;
            WindArraw aArraw = new WindArraw();
            for (i = 0; i < arrawList.Count - 1; i++)
            {
                aArraw = arrawList[i];
                if (i == 0)
                {
                    min = aArraw.length;
                    max = min;
                }
                else
                {
                    if (min > aArraw.length)
                    {
                        min = aArraw.length;
                    }
                    else if (max < aArraw.length)
                    {
                        max = aArraw.length;
                    }
                }
            }
        }

        #endregion

        #region Graphic
        /// <summary>
        /// Draw graphic
        /// </summary>
        /// <param name="points">points</param>
        /// <param name="aGraphic">graphic</param>
        /// <param name="g">graphics</param>
        /// <param name="isEditingVertice">is editing vertice</param>
        public static void DrawGrahpic(PointF[] points, Graphic aGraphic, Graphics g, bool isEditingVertice)
        {
            Rectangle rect = new Rectangle();
            MeteoInfoC.Global.Extent aExtent = MIMath.GetPointFsExtent(points);
            rect.X = (int)aExtent.minX;
            rect.Y = (int)aExtent.minY;
            rect.Width = (int)aExtent.Width;
            rect.Height = (int)aExtent.Height;            

            switch (aGraphic.Shape.ShapeType)
            {
                case ShapeTypes.Point:
                    switch (aGraphic.Legend.BreakType)
                    {
                        case BreakTypes.PointBreak:
                            DrawPoint(points[0], (PointBreak)aGraphic.Legend, g);
                            int aSize = (int)((PointBreak)aGraphic.Legend).Size / 2 + 2;
                            rect.X = (int)points[0].X - aSize;
                            rect.Y = (int)points[0].Y - aSize;
                            rect.Width = aSize * 2;
                            rect.Height = aSize * 2;
                            break;
                        case BreakTypes.LabelBreak:
                            DrawLabelPoint(points[0], (LabelBreak)aGraphic.Legend, g, ref rect);
                            break;
                    }   
                    break;
                case ShapeTypes.Polyline:
                    DrawPolyline(points, (PolyLineBreak)aGraphic.Legend, g);
                    break;
                case ShapeTypes.Polygon:
                case ShapeTypes.Rectangle:
                    DrawPolygon(points, (PolygonBreak)aGraphic.Legend, g);
                    break;
                case ShapeTypes.CurveLine:
                    DrawCurveLine(points, (PolyLineBreak)aGraphic.Legend, g);
                    break;
                case ShapeTypes.CurvePolygon:
                    DrawCurvePolygon(points, (PolygonBreak)aGraphic.Legend, g);
                    break;
                case ShapeTypes.Circle:
                    DrawCircle(points, (PolygonBreak)aGraphic.Legend, g);
                    break;
                case ShapeTypes.Ellipse:
                    DrawEllipse(points, (PolygonBreak)aGraphic.Legend, g);
                    break;
            }

            //Draw selected rectangle
            if (aGraphic.Shape.Selected)
            {
                if (isEditingVertice)
                    DrawSelectedVertices(g, points);
                else
                {
                    Pen aPen = new Pen(Color.Cyan);
                    aPen.DashStyle = DashStyle.Dash;
                    g.DrawRectangle(aPen, rect);
                    aPen.Dispose();
                    switch (aGraphic.Shape.ShapeType)
                    {
                        case ShapeTypes.Point:
                            if (aGraphic.Legend.BreakType == BreakTypes.PointBreak)
                                DrawSelectedCorners(g, rect);
                            break;
                        case ShapeTypes.Polyline:
                        case ShapeTypes.CurveLine:
                        case ShapeTypes.Polygon:
                        case ShapeTypes.Rectangle:                        
                        case ShapeTypes.Ellipse:
                        case ShapeTypes.CurvePolygon:
                            DrawSelectedCorners(g, rect);
                            DrawSelectedEdgeCenters(g, rect);
                            break;
                        case ShapeTypes.Circle:
                            DrawSelectedCorners(g, rect);
                            break;
                    }
                }
            }                        
        }

        /// <summary>
        /// Draw selected four corner rectangles
        /// </summary>
        /// <param name="g"></param>
        /// <param name="gRect"></param>
        public static void DrawSelectedCorners(Graphics g, Rectangle gRect)
        {
            int size = 6;
            SolidBrush sBrush = new SolidBrush(Color.Cyan);
            Pen aPen = new Pen(Color.Black);
            Rectangle rect = new Rectangle(gRect.Left - size / 2, gRect.Top - size / 2, size, size);
            g.FillRectangle(sBrush, rect);
            g.DrawRectangle(aPen, rect);
            rect.Y = gRect.Bottom - size / 2;
            g.FillRectangle(sBrush, rect);
            g.DrawRectangle(aPen, rect);
            rect.X = gRect.Right - size / 2;
            g.FillRectangle(sBrush, rect);
            g.DrawRectangle(aPen, rect);
            rect.Y = gRect.Top - size / 2;
            g.FillRectangle(sBrush, rect);
            g.DrawRectangle(aPen, rect);

            aPen.Dispose();
            sBrush.Dispose();
        }

        /// <summary>
        /// Draw selected four bouder edge center rectangles
        /// </summary>
        /// <param name="g"></param>
        /// <param name="gRect"></param>
        public static void DrawSelectedEdgeCenters(Graphics g, Rectangle gRect)
        {
            int size = 6;
            SolidBrush sBrush = new SolidBrush(Color.Cyan);
            Pen aPen = new Pen(Color.Black);
            Rectangle rect = new Rectangle(gRect.Left + gRect.Width / 2 - size / 2, gRect.Top - size / 2, size, size);
            g.FillRectangle(sBrush, rect);
            g.DrawRectangle(aPen, rect);
            rect.Y = gRect.Bottom - size / 2;
            g.FillRectangle(sBrush, rect);
            g.DrawRectangle(aPen, rect);
            rect.X = gRect.Left - size / 2;
            rect.Y = gRect.Top + gRect.Height / 2 - size / 2;
            g.FillRectangle(sBrush, rect);
            g.DrawRectangle(aPen, rect);
            rect.X = gRect.Right - size / 2;
            g.FillRectangle(sBrush, rect);
            g.DrawRectangle(aPen, rect);

            aPen.Dispose();
            sBrush.Dispose();
        }

        /// <summary>
        /// Draw selected vertices rectangles
        /// </summary>
        /// <param name="g"></param>
        /// <param name="points"></param>
        public static void DrawSelectedVertices(Graphics g, List<PointD> points)
        {
            int size = 6;
            SolidBrush sBrush = new SolidBrush(Color.Cyan);
            Pen aPen = new Pen(Color.Black);
            Rectangle rect = new Rectangle(0, 0, size, size);

            foreach (PointD aPoint in points)
            {
                rect.X = (int)aPoint.X - size / 2;
                rect.Y = (int)aPoint.Y - size / 2;
                g.FillRectangle(sBrush, rect);
                g.DrawRectangle(aPen, rect);
            }

            aPen.Dispose();
            sBrush.Dispose();
        }

        /// <summary>
        /// Draw selected vertices rectangles
        /// </summary>
        /// <param name="g"></param>
        /// <param name="points"></param>
        public static void DrawSelectedVertices(Graphics g, PointF[] points)
        {
            int size = 6;
            SolidBrush sBrush = new SolidBrush(Color.Cyan);
            Pen aPen = new Pen(Color.Black);
            Rectangle rect = new Rectangle(0, 0, size, size);

            foreach (PointF aPoint in points)
            {
                rect.X = (int)aPoint.X - size / 2;
                rect.Y = (int)aPoint.Y - size / 2;
                g.FillRectangle(sBrush, rect);
                g.DrawRectangle(aPen, rect);
            }

            aPen.Dispose();
            sBrush.Dispose();
        }

        #endregion
    }
}
