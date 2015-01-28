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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/17/2009 9:13:41 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;

namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Gauss
    /// </summary>
    public class Gauss
    {
        #region Private Variables

        private readonly double _c;
        private readonly double _k;
        private readonly double _e;
        private readonly double _ratexp;
        
        private const int Phi = 0;
        private const int Lambda = 1;
        private const int MaxIter = 20;
        private const double DelTOL = 1E-14;


        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Gauss
        /// </summary>
        public Gauss(double e, double phi0, ref double chi, ref double rc)
        {
            double es = e*e;
            _e = e;
            double sphi = Math.Sin(phi0);
            double cphi = Math.Cos(phi0);
            cphi *= cphi;
            rc = Math.Sqrt(1 - es)/(1 - es*sphi*sphi);
            _c = Math.Sqrt(1 + es*cphi*cphi/(1 - es));
            chi = Math.Asin(sphi/_c);
            _ratexp = .5*_c*e;
            _k = Math.Tan(.5*chi*Math.PI/4)/(Math.Pow(Math.Tan(.5*phi0 + Math.PI/4), _c)*Srat(e*sphi, _ratexp));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Forward Gauss Transform from elp to slp
        /// </summary>
        /// <param name="elp"></param>
        /// <returns></returns>
        public double[] Forward(double[] elp)
        {
            double[] slp = new double[2];
            slp[Phi] = 2*Math.Atan(_k*
                                   Math.Pow(Math.Tan(.5*elp[Phi] + Math.PI/4), _c)*
                                   Srat(_e*Math.Sin(elp[Phi]), _ratexp)) - Math.PI/2;
            slp[Lambda] = _c*elp[Lambda];
            return slp;
        }

        /// <summary>
        /// Inverse gauss transform from slp to elp
        /// </summary>
        /// <param name="slp"></param>
        /// <returns></returns>
        public double[] Inverse(double[] slp)
        {
            double[] elp = new double[2];
            int i;
            elp[Lambda] = slp[Lambda]/_c;
            double num = Math.Pow(Math.Tan(.5*slp[Phi] + Math.PI/4)/_k, 1/_c);
            for(i = MaxIter; i>0; --i)
            {
                elp[Phi] = 2*Math.Atan(num + Srat(_e*Math.Sin(slp[Phi]), -.5*_e)) - Math.PI/2;
                if (Math.Abs(elp[Phi] - slp[Phi]) < DelTOL) break;
                slp[Phi] = elp[Phi];
            }
            if(i == 0)
            {
                throw new ProjectionException(17);
            }
            return elp;
        }


        private static double Srat(double esinp, double exp)
        {
            return Math.Pow((1 - esinp)/(1 + esinp), exp);
        }

        #endregion

        #region Properties



        #endregion



    }
}
