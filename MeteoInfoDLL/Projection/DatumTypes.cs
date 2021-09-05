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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/28/2009 1:10:01 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
//using MapWindow;
//using MapWindow.Main;
//using MapWindow.Data;
//using MapWindow.Drawing;
//using MapWindow.Geometries;

namespace MeteoInfoC.Projections
{


    /// <summary>
    /// DatumTypes
    /// </summary>
    public enum DatumTypes
    {
        /// <summary>
        /// The datum type is not with a well defined ellips or grid-shift
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// The datum transform to WGS84 can be defined using 3 double parameters
        /// </summary>
        Param3 = 1,
        /// <summary>
        /// The datum transform to WGS84 can be defined using 7 double parameters
        /// </summary>
        Param7 = 2,
        /// <summary>
        /// The transform requires a special nad gridshift
        /// </summary>
        GridShift,
        /// <summary>
        /// The datum is already the WGS84 datum
        /// </summary>
        WGS84

    }
}
