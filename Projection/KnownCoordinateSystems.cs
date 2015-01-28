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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 11:58:44 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;

namespace MeteoInfoC.Projections
{


    /// <summary>
    /// KnownCoordinateSystems
    /// </summary>
    public static class KnownCoordinateSystems 
    {

        /// <summary>
        /// Geographic systems operate in angular units, but can use different
        /// spheroid definitions or angular offsets.
        /// </summary>
        public static GeographicSystems Geographic = new GeographicSystems();

        /// <summary>
        /// Projected systems are systems that use linear units like meters or feet
        /// rather than angular units like degrees or radians
        /// </summary>
        public static ProjectedSystems Projected = new ProjectedSystems();

        
    }
}
