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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/13/2009 11:22:02 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// Mollweide also acts as the base for Wag4 and Wag5
    /// </summary>
    public class Mollweide : Transform
    {
        #region Private Variables


        private double _cX;
        private double _cY;
        private double _cP;
        private const int MaxIter = 10;
        private const double LoopTOL = 1E-7;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Mollweide
        /// </summary>
        public Mollweide()
        {
            Proj4Name = "moll";
            Name = "Mollweide";
            ProjectionName = ProjectionNames.Mollweide;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The forward transform from geodetic units to linear units
        /// </summary>
        /// <param name="lp">The array of lambda, phi coordinates</param>
        /// <param name="xy">The array of x, y coordinates</param>
        protected override void OnForward(double[] lp, double[] xy)
        {
            int i;

            double k = _cP*Math.Sin(lp[Phi]);
            for (i = MaxIter; i > 0; --i)
            {
                double v;
                lp[Phi] -= v = (lp[Phi] + Math.Sin(lp[Phi]) - k)/
                               (1 + Math.Cos(lp[Phi]));
                if (Math.Abs(v) < LoopTOL)
                    break;
            }
            if (i == 0)
                lp[Phi] = (lp[Phi] < 0) ? -HalfPi : HalfPi;
            else
                lp[Phi] *= 0.5;
            xy[X] = _cX*lp[Lambda]*Math.Cos(lp[Phi]);
            xy[Y] = _cY*Math.Sin(lp[Phi]);
        }

        /// <summary>
        /// The inverse transform from linear units to geodetic units
        /// </summary>
        /// <param name="xy">The double values for the input x and y values stored in an array</param>
        /// <param name="lp">The double values for the output lambda and phi values stored in an array</param>
        protected override void OnInverse(double[] xy, double[] lp)
        {
            lp[Phi] = Proj.Aasin(xy[Y] / _cY);
            lp[Lambda] = xy[X] / (_cX * Math.Cos(lp[Phi]));
            lp[Phi] += lp[Phi];
            lp[Phi] = Proj.Aasin((lp[Phi] + Math.Sin(lp[Phi])) / _cP);
        }

        /// <summary>
        /// Initializes the transform using the parameters from the specified coordinate system information
        /// </summary>
        /// <param name="projInfo">A ProjectionInfo class contains all the standard and custom parameters needed to initialize this transform</param>
        protected override void OnInit(ProjectionInfo projInfo)
        {
            Setup(HalfPi);
        }

        /// <summary>
        /// Finalizes the setup based on the provided P paraemter
        /// </summary>
        /// <param name="p"></param>
        protected void Setup(double p)
        {
            double p2 = p + p;
            Es = 0;
            double sp = Math.Sin(p);
            double r = Math.Sqrt(TwoPi*sp/(p2 + Math.Sin(p2)));
            _cX = 2*r/Math.PI;
            _cY = r/sp;
            _cP = p2 + Math.Sin(p2);
        }

        /// <summary>
        /// Gets or sets the x coefficient
        /// </summary>
        protected double Cx
        {
            get { return _cX; }
            set { _cX = value; }
        }

        /// <summary>
        /// Gets or sets the y coefficient value
        /// </summary>
        protected double Cy
        {
            get { return _cY; }
            set { _cY = value; }
        }

        /// <summary>
        /// Gets or sets the P coefficient
        /// </summary>
        protected double Cp
        {
            get { return _cP;  }
            set { _cP = value; }
        }

        #endregion

       



    }
}
