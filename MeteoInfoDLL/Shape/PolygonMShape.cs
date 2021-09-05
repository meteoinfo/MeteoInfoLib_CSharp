using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Global;
using MeteoInfoC.Geoprocess;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// PolygonMShape class
    /// </summary>
    public class PolygonMShape:PolygonShape
    {
        #region Variables


        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PolygonMShape():base()
        {
            ShapeType = ShapeTypes.PolygonM;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get Measurement Array
        /// </summary>
        public double[] MArray
        {
            get
            {
                double[] mArray = new double[Points.Count];
                for (int i = 0; i < Points.Count; i++)
                    mArray[i] = ((PointM)Points[i]).M;

                return mArray;
            }
        }

        /// <summary>
        /// Get M range array
        /// </summary>
        public double[] MRange
        {
            get
            {
                double[] mRange = new double[2];
                mRange[0] = MArray.Min();
                mRange[1] = MArray.Max();

                return mRange;
            }
        }

        #endregion

        #region Methods
       
 

        /// <summary>
        /// Clone PolygonShape
        /// </summary>
        /// <returns>PolygonShape</returns>
        public override object Clone()
        {
            PolygonMShape aPGS = new PolygonMShape();
            aPGS.Extent = Extent;
            aPGS.highValue = highValue;
            aPGS.lowValue = lowValue;
            aPGS.PartNum = PartNum;
            aPGS.parts = (int[])parts.Clone();
            aPGS.Points = new List<PointD>(Points);
            aPGS.Visible = Visible;
            aPGS.Selected = Selected;
            aPGS.LegendIndex = LegendIndex;

            return aPGS;
        }

        /// <summary>
        /// Clone PolygonShape with values
        /// </summary>
        /// <returns>new polygon shape</returns>
        public new PolygonMShape ValueClone()
        {
            PolygonMShape aPGS = new PolygonMShape();
            aPGS.highValue = highValue;
            aPGS.lowValue = lowValue;
            aPGS.Visible = Visible;
            aPGS.Selected = Selected;
            aPGS.LegendIndex = LegendIndex;

            return aPGS;
        }

        #endregion
    }
}
