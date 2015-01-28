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
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/29/2009 3:49:49 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// AnalyticCodes
    /// </summary>
    [Flags]
    public enum AnalyticModes
    {
        /// <summary>
        /// Derivatives of lon analytic
        /// </summary>
        IsAnalXlYl = 0x1,
        /// <summary>
        /// Derivatives of lat analytic
        /// </summary>
        IsAnalXpYp = 0x2,
        /// <summary>
        /// h and k are analytic
        /// </summary>
        IsAnalHK = 0x4,
        /// <summary>
        /// convergence analytic
        /// </summary>
        IsAnalConv = 0x8,


    }
}
