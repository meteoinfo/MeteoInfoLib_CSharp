//********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  This Interface defines how reprojection code should be called.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/11/2009 4:46:40 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Projections
{
    /// <summary>
    /// This interface defines how reprojection classes should be accessed
    /// </summary>
    public interface IReproject
    {
        /// <summary>
        /// Reprojects the specified points.  The first is the projection info to start from, while the destination
        /// is the projection to end with.
        /// </summary>
        /// <param name="points"></param>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="startIndex"></param>
        /// <param name="numPoints"></param>
        /// <returns></returns>
        void ReprojectPoints(double[][] points, ProjectionInfo source, ProjectionInfo dest, int startIndex,
                                   int numPoints);
    }
}
