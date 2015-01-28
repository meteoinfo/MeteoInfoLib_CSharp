using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MeteoInfoC.Layout
{
    /// <summary>
    /// Grid line
    /// </summary>
    public class GridLine
    {
        //#region Variables        
        ////private MapExtentSet m_MapExtentSet;
        //private Color m_GridLineColor;
        //private DashStyle m_GridLineStyle;
        //private int m_GridLineSize;
        //private Color m_LabelColor;
        //private bool m_DrawGridLine;        
        //private List<string> m_XGridStrs = new List<string>();
        //private List<string> m_YGridStrs = new List<string>();

        //#endregion

        //#region Constructor
        ///// <summary>
        ///// Constructor
        ///// </summary>
        //public GridLine(MapView aMapView)
        //{            
        //    //m_MapExtentSet = aMapView.MapExtentSetV;
        //    m_GridLineColor = Color.Gray;
        //    m_GridLineSize = 1;
        //    m_GridLineStyle = DashStyle.Dash;
        //    m_LabelColor = Color.Black;
        //    m_DrawGridLine = true;            
        //}

        //#endregion

        //#region Properties
        /////// <summary>
        /////// Get or set MapExtentSet
        /////// </summary>
        ////public MapExtentSet MapExtentSet
        ////{
        ////    get { return m_MapExtentSet; }
        ////    set { m_MapExtentSet = value; }
        ////}

        //#endregion

        //#region Methods
        ///// <summary>
        ///// Draw longitude and latitude
        ///// </summary>
        ///// <param name="g">graphics</param>
        //public void DrawLonLat(Graphics g)
        //{
        //    Single lonRan, lonDelta, latRan, latDelta, MinX, MinY;

        //    lonRan = (Single)(m_MapExtentSet.MaxX - m_MapExtentSet.MinX);
        //    latRan = (Single)(m_MapExtentSet.MaxY - m_MapExtentSet.MinY);
        //    lonDelta = lonRan / 10;
        //    latDelta = latRan / 10;

        //    string eStr;
        //    int aD, aE;
        //    //Longitude
        //    eStr = lonDelta.ToString("e1");
        //    aD = int.Parse(eStr.Substring(0, 1));
        //    aE = int.Parse(eStr.Substring(eStr.IndexOf("e") + 1, eStr.Length - eStr.IndexOf("e") - 1));
        //    if (aD > 4)
        //    {
        //        lonDelta = (Single)(Math.Pow(10, aE + 1));
        //        //MinX = Convert.ToInt32((m_XYScreenSet.MinX + lonDelta) / Math.Pow(10, aE + 1)) * Math.Pow(10, aE + 1);
        //        MinX = (Single)((int)(m_MapExtentSet.MinX / lonDelta + 1) * Math.Pow(10, aE + 1));
        //    }
        //    else if ((aD == 4) || aD == 2)
        //    {
        //        lonDelta = (Single)(5 * Math.Pow(10, aE));
        //        //MinX = Convert.ToInt32((m_XYScreenSet.MinX + lonDelta) / Math.Pow(10, aE)) * Math.Pow(10, aE);
        //        MinX = (Single)((int)(m_MapExtentSet.MinX / lonDelta + 1) * 5 * Math.Pow(10, aE));
        //    }
        //    else if (aD == 3)
        //    {
        //        lonDelta = (Single)(6 * Math.Pow(10, aE));
        //        //MinX = Convert.ToInt32((m_XYScreenSet.MinX + lonDelta) / Math.Pow(10, aE)) * Math.Pow(10, aE);
        //        MinX = (Single)((int)(m_MapExtentSet.MinX / lonDelta + 1) * 6 * Math.Pow(10, aE));
        //    }
        //    else
        //    {
        //        lonDelta = (Single)(2 * Math.Pow(10, aE));
        //        //MinX = Convert.ToInt32((m_XYScreenSet.MinX + lonDelta) / Math.Pow(10, aE)) * Math.Pow(10, aE);
        //        MinX = (Single)((int)(m_MapExtentSet.MinX / lonDelta + 1) * 2 * Math.Pow(10, aE));
        //    }
        //    if (MinX - lonDelta == m_MapExtentSet.MinX)
        //    {
        //        MinX = (Single)(m_MapExtentSet.MinX);
        //    }
        //    else if (MinX - lonDelta > m_MapExtentSet.MinX)
        //    {
        //        MinX = MinX - lonDelta;
        //    }
        //    string aStr = lonDelta.ToString();
        //    int lonValidNum;
        //    if (aStr.IndexOf(".") < 0)
        //    {
        //        lonValidNum = 0;
        //    }
        //    else
        //    {
        //        lonValidNum = aStr.Substring(aStr.IndexOf(".") + 1).TrimEnd('0').Length;
        //    }
        //    string lonVF = "f" + lonValidNum.ToString();

        //    //Latitude
        //    eStr = latDelta.ToString("e1");
        //    aD = int.Parse(eStr.Substring(0, 1));
        //    aE = int.Parse(eStr.Substring(eStr.IndexOf("e") + 1, eStr.Length - eStr.IndexOf("e") - 1));
        //    if (aD > 4)
        //    {
        //        latDelta = (Single)(Math.Pow(10, aE + 1));
        //        //MinY = Convert.ToInt32((m_XYScreenSet.MinY + latDelta) / Math.Pow(10, aE + 1)) * Math.Pow(10, aE + 1);
        //        MinY = (int)(m_MapExtentSet.MinY / latDelta + 1) * latDelta;
        //    }
        //    else if (aD <= 4 && aD >= 3)
        //    {
        //        latDelta = (Single)(5 * Math.Pow(10, aE));
        //        //MinY = Convert.ToInt32((m_XYScreenSet.MinY + latDelta) / Math.Pow(10, aE)) * Math.Pow(10, aE);
        //        MinY = (int)(m_MapExtentSet.MinY / latDelta + 1) * latDelta;
        //    }
        //    else if (aD <= 2 && aD >= 1)
        //    {
        //        latDelta = (Single)(3 * Math.Pow(10, aE));
        //        //MinY = Convert.ToInt32((m_XYScreenSet.MinY + latDelta) / Math.Pow(10, aE)) * Math.Pow(10, aE);
        //        MinY = (int)(m_MapExtentSet.MinY / latDelta + 1) * latDelta;
        //    }
        //    else
        //    {
        //        latDelta = (Single)(Math.Pow(10, aE));
        //        //MinY = Convert.ToInt32((m_XYScreenSet.MinY + latDelta) / Math.Pow(10, aE)) * Math.Pow(10, aE);
        //        MinY = (int)(m_MapExtentSet.MinY / latDelta + 1) * latDelta;
        //    }
        //    if (MinY - latDelta == m_MapExtentSet.MinY)
        //    {
        //        MinY = (Single)(m_MapExtentSet.MinY);
        //    }
        //    else if (MinY - latDelta > m_MapExtentSet.MinY)
        //    {
        //        MinY = MinY - latDelta;
        //    }
        //    aStr = latDelta.ToString();
        //    int latValidNum;
        //    if (aStr.IndexOf(".") < 0)
        //    {
        //        latValidNum = 0;
        //    }
        //    else
        //    {
        //        latValidNum = aStr.Substring(aStr.IndexOf(".") + 1).TrimEnd('0').Length;
        //    }
        //    string latVF = "f" + latValidNum.ToString();

        //    int i = 0;
        //    double lon = MinX;
        //    PointF sP, eP;
        //    sP = new PointF(0, 0);
        //    eP = new PointF(0, 0);
        //    Single X = 0, Y = 0;
        //    Pen aPen = new Pen(m_GridLineColor);
        //    aPen.DashStyle = m_GridLineStyle;
        //    aPen.Width = m_GridLineSize;
        //    Font drawFont = new Font("Arial", 7);
        //    string drawStr;
        //    SizeF aSF;
        //    SolidBrush aBrush = new SolidBrush(m_LabelColor);
        //    //Draw longitude
        //    while (true)
        //    {
        //        lon = MinX + i * lonDelta;
        //        if (lon > m_MapExtentSet.MaxX)
        //        {
        //            break;
        //        }
        //        m_MapExtentSet.ProjToScreen(lon, m_MapExtentSet.MinY, ref X, ref Y, 0, m_MapExtentSet);
        //        //sP.X = Convert.ToInt32((lon - m_MapExtentSet.MinX) * m_MapExtentSet.ScaleX) + m_XYScreenSet.XLBorderSpace;
        //        //sP.Y = this.Height - m_XYScreenSet.YBBorderSpace;
        //        sP.X = X;
        //        sP.Y = Y;
        //        if (m_DrawGridLine)
        //        {
        //            m_MapExtentSet.ProjToScreen(lon, m_MapExtentSet.MaxY, ref X, ref Y, 0, m_MapExtentSet);
        //            eP.X = X;
        //            eP.Y = Y;
        //        }
        //        else
        //        {
        //            eP.X = sP.X;
        //            eP.Y = sP.Y - 5;
        //            aPen.Color = m_GridLineColor;
        //            aPen.DashStyle = DashStyle.Solid;
        //            aPen.Width = 1;
        //        }
        //        if (lon > m_MapExtentSet.MinX || lon < m_MapExtentSet.MaxX)
        //        {
        //            g.DrawLine(aPen, sP, eP);
        //        }
        //        if (lon < 0)
        //        {
        //            lon = lon + 360;
        //        }
        //        if (lon > 0 && lon <= 180)
        //        {
        //            drawStr = lon.ToString(lonVF) + "E";
        //        }
        //        else if (lon == 0 || lon == 360)
        //        {
        //            drawStr = "0";
        //        }
        //        else
        //        {
        //            drawStr = (360 - lon).ToString(lonVF) + "W";
        //        }

        //        aSF = g.MeasureString(drawStr, drawFont);
        //        g.DrawString(drawStr, drawFont, aBrush, sP.X - aSF.Width / 2, sP.Y + 2);
        //        i++;
        //    }

        //    //Draw latitude
        //    i = 0;
        //    double lat = MinY;
        //    while (true)
        //    {
        //        lat = MinY + i * latDelta;
        //        if (lat > m_MapExtentSet.MaxY)
        //        {
        //            break;
        //        }
        //        m_MapExtentSet.ProjToScreen(m_MapExtentSet.MinX, lat, ref X, ref Y, 0, m_MapExtentSet);
        //        sP.X = X;
        //        sP.Y = Y;
        //        if (m_DrawGridLine)
        //        {
        //            m_MapExtentSet.ProjToScreen(m_MapExtentSet.MaxY, lat, ref X, ref Y, 0, m_MapExtentSet);
        //            eP.X = X;
        //            eP.Y = Y;
        //        }
        //        else
        //        {
        //            eP.X = sP.X + 5;
        //            eP.Y = sP.Y;
        //        }
        //        if (lat > m_MapExtentSet.MinY || lat < m_MapExtentSet.MaxY)
        //        {
        //            g.DrawLine(aPen, sP, eP);
        //        }

        //        if (lat > 0)
        //        {
        //            drawStr = lat.ToString(latVF) + "N";
        //        }
        //        else if (lat < 0)
        //        {
        //            drawStr = (-lat).ToString(latVF) + "S";
        //        }
        //        else
        //        {
        //            drawStr = "EQ";
        //        }

        //        aSF = g.MeasureString(drawStr, drawFont);
        //        g.DrawString(drawStr, drawFont, aBrush, sP.X - aSF.Width - 5, sP.Y - aSF.Height / 2);
        //        i++;
        //    }
        //}

        ///// <summary>
        ///// Draw X/Y grid
        ///// </summary>
        ///// <param name="g">graphics</param>        
        //public void DrawXYGrid(Graphics g)
        //{
        //    //if (Layers.LayerNum == 0)
        //    //{
        //    //    return;
        //    //}

        //    int XDelt, YDelt, vXNum, vYNum;
        //    vXNum = (int)(m_MapExtentSet.MaxX - m_MapExtentSet.MinX);
        //    vYNum = (int)(m_MapExtentSet.MaxY - m_MapExtentSet.MinY);
        //    XDelt = (int)vXNum / 10 + 1;
        //    YDelt = (int)vYNum / 10 + 1;

        //    int i;
        //    PointF sP, eP;
        //    sP = new PointF(0, 0);
        //    eP = new PointF(0, 0);
        //    Single X = 0, Y = 0;
        //    Pen aPen = new Pen(m_GridLineColor);
        //    aPen.DashStyle = m_GridLineStyle;
        //    aPen.Width = m_GridLineSize;
        //    Font drawFont = new Font("Arial", 7);
        //    if (!m_DrawGridLine)
        //    {
        //        aPen.Color = m_GridLineColor;
        //        aPen.DashStyle = DashStyle.Solid;
        //        aPen.Width = 1;
        //    }
        //    SolidBrush aBrush = new SolidBrush(m_GridLineColor);
        //    string drawStr;
        //    SizeF aSF;
        //    int XGridNum = m_XGridStrs.Count;
        //    int YGridNum = m_YGridStrs.Count;
        //    //Draw X grid
        //    for (i = 0; i < XGridNum; i += XDelt)
        //    {
        //        if (i >= m_MapExtentSet.MinX && i <= m_MapExtentSet.MaxX)
        //        {
        //            m_MapExtentSet.ProjToScreen(i, m_MapExtentSet.MinY, ref X, ref Y, 0, m_MapExtentSet);
        //            sP.X = X;
        //            sP.Y = Y;
        //            if (m_DrawGridLine)
        //            {
        //                m_MapExtentSet.ProjToScreen(i, m_MapExtentSet.MaxY, ref X, ref Y, 0, m_MapExtentSet);
        //                eP.X = X;
        //                eP.Y = Y;
        //            }
        //            else
        //            {
        //                eP.X = sP.X;
        //                eP.Y = sP.Y - 5;
        //            }
        //            if (i > m_MapExtentSet.MinX && i < m_MapExtentSet.MaxX)
        //            {
        //                g.DrawLine(aPen, sP, eP);
        //            }

        //            drawStr = m_XGridStrs[i];
        //            aSF = g.MeasureString(drawStr, drawFont);
        //            g.DrawString(drawStr, drawFont, aBrush, sP.X - aSF.Width / 2, sP.Y + 2);
        //        }
        //    }

        //    //Draw Y grid                        
        //    for (i = 0; i < YGridNum; i += YDelt)
        //    {
        //        if (i > m_MapExtentSet.MinY && i < m_MapExtentSet.MaxY)
        //        {
        //            m_MapExtentSet.ProjToScreen(m_MapExtentSet.MinX, i, ref X, ref Y, 0, m_MapExtentSet);
        //            sP.X = X;
        //            sP.Y = Y;
        //            if (m_DrawGridLine)
        //            {
        //                m_MapExtentSet.ProjToScreen(m_MapExtentSet.MaxX, i, ref X, ref Y, 0, m_MapExtentSet);
        //                eP.X = X;
        //                eP.Y = Y;
        //            }
        //            else
        //            {
        //                eP.X = sP.X + 5;
        //                eP.Y = sP.Y;
        //            }
        //            if (i > m_MapExtentSet.MinY && i < m_MapExtentSet.MaxY)
        //            {
        //                g.DrawLine(aPen, sP, eP);
        //            }

        //            drawStr = m_YGridStrs[i];
        //            aSF = g.MeasureString(drawStr, drawFont);
        //            g.DrawString(drawStr, drawFont, aBrush, sP.X - aSF.Width - 5, sP.Y - aSF.Height / 2);
        //        }
        //    }
        //}

        //#endregion

        //#region Events
        
        //#endregion
    }
}
