//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
//********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
// you may not use this file except in compliance with the License. You may obtain a copy of the License at 
// http://www.mozilla.org/MPL/ 
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
// ANY KIND, either expressed or implied. See the License for the specificlanguage governing rights and 
// limitations under the License. 
//
// The original content was ported from the C language from the 4.6 version of Proj4 libraries. 
// Frank Warmerdam has released the full content of that version under the MIT license which is
// recognized as being approximately equivalent to public domain.  The original work was done 
// mostly by Gerald Evenden.  The latest versions of the C libraries can be obtained here:
// http://trac.osgeo.org/proj/
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/29/2009 3:23:29 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Factors
    /// </summary>
    public class Factors
    {
        /// <summary>
        /// derivatives of x for lambda
        /// </summary>
        public double x_l;
        /// <summary>
        /// derivatives of x for phi
        /// </summary>
        public double x_p;
        /// <summary>
        /// derivatives of y for lambda
        /// </summary>
        public double y_l;
        /// <summary>
        /// derivatives of y for phi
        /// </summary>
        public double y_p;
        /// <summary>
        /// Meridinal scale
        /// </summary>
        public double h;
        /// <summary>
        /// parallel scale
        /// </summary>
        public double k;
        /// <summary>
        /// Angular distortion
        /// </summary>
        public double omega;
        /// <summary>
        /// theta prime
        /// </summary>
        public double thetap;
        /// <summary>
        /// Convergence
        /// </summary>
        public double conv;
        /// <summary>
        /// Areal scale factor
        /// </summary>
        public double s;
        /// <summary>
        /// max scale error
        /// </summary>
        public double a;
        /// <summary>
        /// min scale error
        /// </summary>
        public double b;
        /// <summary>
        /// Info as to analytics 
        /// </summary>
        public AnalyticModes code;

        #region Private Variables


        #endregion

        #region Constructors


        #endregion

        #region Methods

        #endregion

        #region Properties



        #endregion



    }
}
