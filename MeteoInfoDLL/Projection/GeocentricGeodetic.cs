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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/24/2009 10:11:03 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{
    
    /*
    * Reference...
    * ============
    * Wenzel, H.-G.(1985): Hochauflösende Kugelfunktionsmodelle für
    * das Gravitationspotential der Erde. Wiss. Arb. Univ. Hannover
    * Nr. 137, p. 130-131.

    * Programmed by GGA- Leibniz-Institue of Applied Geophysics
    *               Stilleweg 2
    *               D-30655 Hannover
    *               Federal Republic of Germany
    *               Internet: www.gga-hannover.de
    *
    *               Hannover, March 1999, April 2004.
    *               see also: comments in statements
    * remarks:
    * Mathematically exact and because of symmetry of rotation-ellipsoid,
    * each point (X,Y,Z) has at least two solutions (Latitude1,Longitude1,Height1) and
    * (Latitude2,Longitude2,Height2). Is point=(0.,0.,Z) (P=0.), so you get even
    * four solutions,	every two symmetrical to the semi-minor axis.
    * Here Height1 and Height2 have at least a difference in order of
    * radius of curvature (e.g. (0,0,b)=> (90.,0.,0.) or (-90.,0.,-2b);
    * (a+100.)*(sqrt(2.)/2.,sqrt(2.)/2.,0.) => (0.,45.,100.) or
    * (0.,225.,-(2a+100.))).
    * The algorithm always computes (Latitude,Longitude) with smallest |Height|.
    * For normal computations, that means |Height|<10000.m, algorithm normally
    * converges after to 2-3 steps!!!
    * But if |Height| has the amount of length of ellipsoid's axis
    * (e.g. -6300000.m),	algorithm needs about 15 steps.
    */
    /// <summary>
    /// Wenzel, H.-G.(1985): Hochauflösende Kugelfunktionsmodelle für
    /// das Gravitationspotential der Erde. Wiss. Arb. Univ. Hannover
    /// Nr. 137, p. 130-131.
    /// </summary>
    public class GeocentricGeodetic
    {
        /* local defintions and variables */
        /* end-criterium of loop, accuracy of sin(Latitude) */
        private const double genau = 1E-12;
        private const double genau2 = genau*genau;
        private const double PiOver2 = Math.PI/2;
        private const int maxiter = 30;

        private readonly double _a;
        private readonly double _b;
        private readonly double _a2;
        private readonly double _b2;
        private readonly double _e2;
        private readonly double _ep2;


        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of GeocentricGeodetic
        /// </summary>
        public GeocentricGeodetic(Spheroid gi)
        {
            _a = gi.EquatorialRadius;
            _b = gi.PolarRadius;
            _a2 = _a*_a;
            _b2 = _b*_b;
            _e2 = (_a2 - _b2)/_a2;
            _ep2 = (_a2 - _b2)/_b2;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts lon, lat, height to x, y, z where lon and lat are in radians and everything else is meters
        /// </summary>
        /// <param name="points"></param>
        /// <param name="startIndex"></param>
        /// <param name="numPoints"></param>
        public void GeodeticToGeocentric(double[][] points, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < numPoints; i++)
            {
                points[i] = GeodeticToGeocentric(points[i]);

            }
        }

        private double[] GeodeticToGeocentric(double[] point)
        {
            double lon = point[0];
            double lat = point[1];
            double height = point[2];

            /*
             * The function Convert_Geodetic_To_Geocentric converts geodetic coordinates
             * (latitude, longitude, and height) to geocentric coordinates (X, Y, Z),
             * according to the current ellipsoid parameters.
             *
             *    Latitude  : Geodetic latitude in radians                     (input)
             *    Longitude : Geodetic longitude in radians                    (input)
             *    Height    : Geodetic height, in meters                       (input)
             *    X         : Calculated Geocentric X coordinate, in meters    (output)
             *    Y         : Calculated Geocentric Y coordinate, in meters    (output)
             *    Z         : Calculated Geocentric Z coordinate, in meters    (output)
             *
             */

            /*
            ** Don't blow up if Latitude is just a little out of the value
            ** range as it may just be a rounding issue.  Also removed longitude
            ** test, it should be wrapped by cos() and sin().  NFW for PROJ.4, Sep/2001.
            */
            if (lat < -PiOver2 && lat > -1.001 * PiOver2)
                lat = -PiOver2;
            else if (lat > PiOver2 && lat < 1.001 * PiOver2)
                lat = PiOver2;
            else if ((lat < -PiOver2) || (lat > PiOver2))
            { /* lat out of range */
                throw new ProjectionException(11);
            }

            
            if (lon > Math.PI)
                lon -= (2 * Math.PI);
            double sinLat = Math.Sin(lat);  
            double cosLat = Math.Cos(lat);
            double sin2Lat = sinLat * sinLat;                     /*  Square of sin(Latitude)  */
            double rn = _a / (Math.Sqrt(1.0e0 - _e2 * sin2Lat)); /*  Earth radius at location  */
            double x = (rn + height) * cosLat * Math.Cos(lon);
            double y = (rn + height) * cosLat * Math.Sin(lon);
            double z = ((rn * (1 - _e2)) + height) * sinLat;

            return new[] {x, y, z};

        }

        /// <summary>
        /// Converts x, y, z to lon, lat, height
        /// </summary>
        /// <param name="points"></param>
        /// <param name="startIndex"></param>
        /// <param name="numPoints"></param>
        public void GeocentricToGeodetic(double[][] points, int startIndex, int numPoints)
        {
            for (int i = startIndex; i < numPoints; i++)
            {
                points[i] = GeocentricToGeodetic(points[i]);

            }
        }

        /// <summary>
        /// Converts geocentric x, y, z coords to geodetic lon, lat, h
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private double[] GeocentricToGeodetic(double[] point)
        {
            double lat;
            double lon;
            double height;

            double x = point[0];
            double y = point[1];
            double z = point[2];

            double cphi;     /* cos of searched geodetic latitude */
            double sphi;     /* sin of searched geodetic latitude */
            double sdphi;    /* end-criterium: addition-theorem of sin(Latitude(iter)-Latitude(iter-1)) */

            double p = Math.Sqrt(x*x+y*y);
            double rr = Math.Sqrt(x*x+y*y+z*z);

            /*	special cases for latitude and longitude */
            if (p/_a < genau) 
            {

               /*  special case, if P=0. (X=0., Y=0.) */
                lon = 0;

                /*  if (X,Y,Z)=(0.,0.,0.) then Height becomes semi-minor axis
                 *  of ellipsoid (=center of mass), Latitude becomes PI/2 */
                if (rr/_a < genau) 
                {
                    lat = Math.PI/2;
                    height = -_b;
                    return new[] {lon, lat, height};
                }
            }
            else 
            {
                /*  ellipsoidal (geodetic) longitude
                 *  interval: -PI < Longitude <= +PI */
                lon = Math.Atan2(y, x);
            }

            /* --------------------------------------------------------------
             * Following iterative algorithm was developped by
             * "Institut für Erdmessung", University of Hannover, July 1988.
             * Internet: www.ife.uni-hannover.de
             * Iterative computation of CPHI,SPHI and Height.
             * Iteration of CPHI and SPHI to 10**-12 radian resp.
             * 2*10**-7 arcsec.
             * --------------------------------------------------------------
             */
            double ct = z/rr;
            double st = p/rr;
            double rx = 1.0/Math.Sqrt(1.0-_e2*(2.0-_e2)*st*st);
            double cphi0 = st*(1.0-_e2)*rx;
            double sphi0 = ct * rx; /* sin of start or old geodetic latitude in iterations */
            int iter = 0;

            /* loop to find sin(Latitude) resp. Latitude
             * until |sin(Latitude(iter)-Latitude(iter-1))| < genau */
            do
            {
                iter++;
                double rn = _a/Math.Sqrt(1.0-_e2*sphi0*sphi0);       /* Earth radius at location */
                
                /*  ellipsoidal (geodetic) height */
                height = p*cphi0+z*sphi0-rn*(1.0-_e2*sphi0*sphi0);

                double rk = _e2*rn/(rn+height);
                rx = 1.0/Math.Sqrt(1.0-rk*(2.0-rk)*st*st);
                cphi = st*(1.0-rk)*rx;
                sphi = ct*rx;
                sdphi = sphi*cphi0-cphi*sphi0;
                cphi0 = cphi;
                sphi0 = sphi;
            }
            while (sdphi*sdphi > genau2 && iter < maxiter);

            /*	ellipsoidal (geodetic) latitude */
            lat=Math.Atan(sphi/Math.Abs(cphi));
            return new[]{lon, lat, height};
        }
        

        #endregion

        #region Properties



        #endregion



    }
}
