using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// Grid interpolation
    /// </summary>
    public class InterpolationSetting
    {
        #region Variables

        private GridDataSetting _GridDataPara = new GridDataSetting();
        private InterpolationMethods _GridInterMethod;
        private int _MinPointNum;
        private double _Radius;
        private double _UnDefData = -9999.0;
        private List<double> _RadList;    //For cressman analysis
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public InterpolationSetting()
        {
            _GridDataPara.XNum = 50;
            _GridDataPara.YNum = 50;
            _GridInterMethod = InterpolationMethods.IDW_Radius;
            _MinPointNum = 1;
            _Radius = 1;            
            _RadList = new List<double>();
            _RadList.AddRange(new double[] { 10, 7, 4, 2, 1 });
        }

        /// <summary>
        /// Set grid interpolation parameters
        /// </summary>
        /// <param name="minX">mininum x</param>
        /// <param name="maxX">maxinum x</param>
        /// <param name="minY">mininum y</param>
        /// <param name="maxY">maxinum y</param>
        /// <param name="xNum">x number</param>
        /// <param name="yNum">y nunmber</param>
        /// <param name="aInterMethod">interpolation method</param>
        /// <param name="radius">radius</param>
        /// <param name="minNum">mininum number</param>
        public InterpolationSetting(double minX, double maxX, double minY, double maxY, int xNum, int yNum,
            string aInterMethod, float radius, int minNum)
        {
            GridDataSetting aGDP = new GridDataSetting();
            aGDP.DataExtent.minX = minX;
            aGDP.DataExtent.maxX = maxX;
            aGDP.DataExtent.minY = minY;
            aGDP.DataExtent.maxY = maxY;
            aGDP.XNum = xNum;
            aGDP.YNum = yNum;
            _GridDataPara = aGDP;

            _GridInterMethod = (InterpolationMethods)Enum.Parse(typeof(InterpolationMethods), aInterMethod, true);
            _Radius = radius;
            _MinPointNum = minNum;

            _RadList = new List<double>();
            _RadList.AddRange(new double[] { 10, 7, 4, 2, 1 });
        }

        /// <summary>
        /// Set grid interpolation parameters
        /// </summary>
        /// <param name="minX">mininum x</param>
        /// <param name="maxX">maxinum x</param>
        /// <param name="minY">mininum y</param>
        /// <param name="maxY">maxinum y</param>
        /// <param name="xNum">x number</param>
        /// <param name="yNum">y nunmber</param>
        /// <param name="aInterMethod">interpolation method</param>
        /// <param name="radList">radius</param>        
        public InterpolationSetting(double minX, double maxX, double minY, double maxY, int xNum, int yNum,
            string aInterMethod, List<double> radList)
        {
            GridDataSetting aGDP = new GridDataSetting();
            aGDP.DataExtent.minX = minX;
            aGDP.DataExtent.maxX = maxX;
            aGDP.DataExtent.minY = minY;
            aGDP.DataExtent.maxY = maxY;
            aGDP.XNum = xNum;
            aGDP.YNum = yNum;
            _GridDataPara = aGDP;

            _GridInterMethod = (InterpolationMethods)Enum.Parse(typeof(InterpolationMethods), aInterMethod, true);
            _RadList = radList;
            _MinPointNum = 1;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Get or set grid data parameter
        /// </summary>
        public GridDataSetting GridDataSet
        {
            get { return _GridDataPara; }
            set { _GridDataPara = value; }
        }

        /// <summary>
        /// Get or set grid interpolation method
        /// </summary>
        public InterpolationMethods InterpolationMethod
        {
            get { return _GridInterMethod; }
            set { _GridInterMethod = value; }
        }

        /// <summary>
        /// Get or set minimum point number
        /// </summary>
        public int MinPointNum
        {
            get { return _MinPointNum; }
            set { _MinPointNum = value; }
        }

        /// <summary>
        /// Get or set search radius
        /// </summary>
        public double Radius
        {
            get { return _Radius; }
            set { _Radius = value; }
        }

        /// <summary>
        /// Get or set undefine data
        /// </summary>
        public double UnDefData
        {
            get { return _UnDefData; }
            set { _UnDefData = value; }
        }

        /// <summary>
        /// Get or set radius list
        /// </summary>
        public List<double> RadList
        {
            get { return _RadList; }
            set { _RadList = value; }
        }
        #endregion
    }
}
