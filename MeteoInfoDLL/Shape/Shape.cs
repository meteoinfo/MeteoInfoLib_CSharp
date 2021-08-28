using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Global;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Shape
    /// </summary>
    public class Shape
    {
        #region Variables
        private ShapeTypes _ShapeType;
        private bool _Visible;
        private bool _Selected;
        private Extent _extent = new Extent();
        private int _legendIndex = 0;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Shape()
        {
            _ShapeType = ShapeTypes.Point;
            _Visible = true;
            _Selected = false;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set shape type
        /// </summary>
        public ShapeTypes ShapeType
        {
            get { return _ShapeType; }
            set { _ShapeType = value; }
        }

        /// <summary>
        /// Get or set visible
        /// </summary>
        public bool Visible
        {
            get { return _Visible; }
            set { _Visible = value; }
        }

        /// <summary>
        /// Get or set selected
        /// </summary>
        public bool Selected
        {
            get { return _Selected; }
            set { _Selected = value; }
        }

        /// <summary>
        /// Get or set extent
        /// </summary>
        public Extent Extent
        {
            get { return _extent; }
            set { _extent = value; }
        }

        /// <summary>
        /// Get or set legend break index
        /// </summary>
        public int LegendIndex
        {
            get { return _legendIndex; }
            set { _legendIndex = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get points
        /// </summary>
        /// <returns>point list</returns>
        public virtual List<PointD> GetPoints()
        {
            return null;
        }
        
        /// <summary>
        /// Set points
        /// </summary>
        public virtual void SetPoints(List<PointD> points)
        {

        }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>object</returns>
        public virtual object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
