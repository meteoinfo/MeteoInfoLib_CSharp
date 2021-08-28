using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MeteoInfoC.Shape;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Legend TreeNode
    /// </summary>
    public class LegendNode:ItemNode
    {
        #region Variables

        private ShapeTypes _ShapeType;
        private ColorBreak _LegendBreak;        

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public LegendNode():base()
        {
           
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
        /// Get or set legend break
        /// </summary>
        public ColorBreak LegendBreak
        {
            get { return _LegendBreak; }
            set { _LegendBreak = value; }
        }
        
        #endregion

        #region Methods
        /// <summary>
        /// Clone
        /// </summary>
        public override object Clone()
        {
            LegendNode aLN = new LegendNode();
            aLN.ShapeType = _ShapeType;
            aLN.LegendBreak = _LegendBreak;

            return aLN;
        }

        #endregion
    }
}
