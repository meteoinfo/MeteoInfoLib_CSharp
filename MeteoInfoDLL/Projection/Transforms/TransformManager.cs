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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/24/2009 12:01:21 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************


using System.Collections.Generic;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// TransformManager
    /// </summary>
    public class TransformManager
    {
        #region Private Variables

        private readonly List<ITransform> _transforms;
        private static readonly TransformManager _defaultManager = new TransformManager();
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of TransformManager
        /// </summary>
        public TransformManager()
        {
            _transforms = new List<ITransform>();
            _transforms.Add(new Aitoff());
            _transforms.Add(new AlbersEqualArea());
            _transforms.Add(new AzimuthalEquidistant());
            _transforms.Add(new BipolarObliqueConformalConic());
            _transforms.Add(new Bonne());
            _transforms.Add(new Cassini());
            _transforms.Add(new CylindricalEqualArea());
            _transforms.Add(new CrasterParabolic());
            _transforms.Add(new Eckert1());
            _transforms.Add(new Eckert2());
            _transforms.Add(new Eckert3());
            _transforms.Add(new Eckert4());
            _transforms.Add(new Eckert5());
            _transforms.Add(new Eckert6());
            _transforms.Add(new EquidistantCylindrical());
            _transforms.Add(new EquidistantConic());
            _transforms.Add(new Foucaut());
            _transforms.Add(new GallStereographic());
            _transforms.Add(new GeneralSinusoidal());
            _transforms.Add(new GeostationarySatellite());
            _transforms.Add(new Gnomonic());
            _transforms.Add(new GoodeHomolosine());
            _transforms.Add(new Kavraisky5());
            _transforms.Add(new Kavraisky7());
            _transforms.Add(new Krovak());
            _transforms.Add(new LambertAzimuthalEqualArea());
            _transforms.Add(new LambertConformalConic());
            _transforms.Add(new LambertEqualAreaConic());
            _transforms.Add(new LongLat());
            _transforms.Add(new Loximuthal());
            _transforms.Add(new McBrydeThomasFlatPolarSine());
            _transforms.Add(new Mercator());
            _transforms.Add(new MillerCylindrical());
            _transforms.Add(new Mollweide());
            _transforms.Add(new NewZealandMapGrid());
            _transforms.Add(new ObliqueStereographicAlternative());
            _transforms.Add(new ObliqueCylindricalEqualArea());
            _transforms.Add(new ObliqueMercator());
            _transforms.Add(new Orthographic());
            _transforms.Add(new Polyconic());
            _transforms.Add(new PutinsP1());
            _transforms.Add(new QuarticAuthalic());
            _transforms.Add(new Robinson());
            _transforms.Add(new Sinusoidal());
            _transforms.Add(new Stereographic());
            _transforms.Add(new SwissObliqueMercator());
            _transforms.Add(new TransverseMercator());
            _transforms.Add(new TwoPointEquidistant());
            _transforms.Add(new UniversalPolarStereographic());
            _transforms.Add(new UniversalTransverseMercator());
            _transforms.Add(new VanderGrinten1());
            _transforms.Add(new Wagner4());
            _transforms.Add(new Wagner5());
            _transforms.Add(new Wagner6());
            _transforms.Add(new Winkel1());
            _transforms.Add(new Winkel2());
            _transforms.Add(new WinkelTripel());

   
        }

        #endregion

        #region Methods

        /// <summary>
        /// Given the proj4 string, returns the matching transform eg: tmerc.
        /// </summary>
        /// <param name="name">The string name</param>
        /// <returns>The ITransform that has the matching proj4 name</returns>
        public ITransform GetProj4(string name)
        {
            foreach (ITransform transform in _transforms)
            {
                //if (transform.Proj4Name == name) return transform.Copy();
                if (transform.Proj4Name == name) return (ITransform)transform.Clone();
                if (transform.Proj4Aliases == null) continue;
                foreach (string prj4name in transform.Proj4Aliases)
                {
                    //if (prj4name == name) return transform.Copy();
                    if (prj4name == name) return (ITransform)transform.Clone();
                }
            }
            throw new ProjectionException(37);
        }

        /// <summary>
        /// Given the .prj name (ESRI wkt), returns the matching transform
        /// </summary>
        /// <param name="name">The string name for the trnasform eg. Transverse_Mercator</param>
        /// <returns>The ITransform that has the matching ESRI wkt name</returns>
        public ITransform GetProjection(string name)
        {
            foreach (ITransform transform in _transforms)
            {
                string[] esriNames = transform.Name.Split(';');
                foreach (string esriName in esriNames)
                {
                    if (name == esriName) return transform;
                }
                
            }
            throw new ProjectionException(37);
        }

        

        #endregion

        #region Properties

        /// <summary>
        /// The entire list of transforms
        /// </summary>
        public List<ITransform> Transforms
        {
            get { return _transforms; }
        }

        /// <summary>
        /// Gets the default instance of the transform manager
        /// </summary>
        public static TransformManager DefaultTransformManager
        {
            get { return _defaultManager; }
        }


        #endregion



    }
}
