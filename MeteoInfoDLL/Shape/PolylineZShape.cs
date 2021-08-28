using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using MeteoInfoC.Global;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// PolylineZ shape
    /// </summary>
    public class PolylineZShape : PolylineShape
    {
        #region Variables   
        

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public PolylineZShape():base()
        {
            ShapeType = ShapeTypes.PolylineZ;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Get Z Array
        /// </summary>
        public double[] ZArray
        {
            get
            {
                double[] zArray = new double[Points.Count];
                for (int i = 0; i < Points.Count; i++)
                    zArray[i] = ((PointZ)Points[i]).Z;

                return zArray;
            }
        }

        /// <summary>
        /// Get Measurement Array
        /// </summary>
        public double[] MArray
        {
            get
            {
                double[] mArray = new double[Points.Count];
                for (int i = 0; i < Points.Count; i++)
                    mArray[i] = ((PointZ)Points[i]).M;

                return mArray;
            }
        }

        /// <summary>
        /// Get Z range array
        /// </summary>
        public double[] ZRange
        {
            get
            {
                double[] zRange = new double[2];
                zRange[0] = ZArray.Min();
                zRange[1] = ZArray.Max();

                return zRange;
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
        /// Clone polylineZ shape
        /// </summary>
        /// <returns>PolylineZShape</returns>
        public override object Clone()
        {
            PolylineZShape aPLS = new PolylineZShape();
            aPLS.value = value;
            aPLS.Extent = Extent;
            aPLS.PartNum = PartNum;
            aPLS.parts = (int[])parts.Clone();
            aPLS.Points = new List<PointD>(Points);
            aPLS.Visible = Visible;
            aPLS.Selected = Selected;
            aPLS.LegendIndex = LegendIndex;
            //aPLS.ZRange = (double[])ZRange.Clone();
            //aPLS.MRange = (double[])MRange.Clone();
            //aPLS.ZArray = (double[])ZArray.Clone();
            //aPLS.MArray = (double[])MArray.Clone();

            return aPLS;
        }

        /// <summary>
        /// Value clone
        /// </summary>
        /// <returns>new polyline Z shape</returns>
        public new PolylineZShape ValueClone()
        {
            PolylineZShape aPLS = new PolylineZShape();
            aPLS.value = value;
            aPLS.Visible = Visible;
            aPLS.Selected = Selected;
            aPLS.LegendIndex = LegendIndex;

            return aPLS;
        }

        #endregion
    }
}
