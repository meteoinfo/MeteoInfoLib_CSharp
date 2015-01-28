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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/18/2009 9:31:11 AM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// GridShift
    /// </summary>
    public static class GridShift
    {
        private const double HUGE_VAL = double.MaxValue;
        private const int MAX_TRY = 9;
        private const int X = 0;
        private const int Y = 1;
        private const double TOL = 1E-12;

        #region Private Variables

        private static readonly NadTables _shift = new NadTables();

        #endregion

       
        #region Methods

        /// <summary>
        /// Applies either a forward or backward gridshift based on the specified name
        /// </summary>
        /// <param name="names"></param>
        /// <param name="inverse"></param>
        /// <param name="points"></param>
        /// <param name="startIndex"></param>
        /// <param name="numPoints"></param>
        public static void Apply(string[] names, bool inverse, double[][] points, int startIndex, long numPoints)
        {
           
            for(int i = startIndex; i < numPoints; i++)
            {
                PhiLam input, output;
                input.Phi = points[i][Y];
                input.Lambda = points[i][X];
                output.Phi = HUGE_VAL;
                output.Lambda = HUGE_VAL;

                /* keep trying till we find a table that works from the ones listed */
                foreach (string name in names)
                {
                    NadTable table = _shift.Tables[name];

                    /* skip tables that don't match our point at all.  */
                    double minLam = table.LowerLeft.Lambda;
                    double maxLam = minLam + (table.NumLambdas - 1)*table.CellSize.Lambda;
                    double minPhi = table.LowerLeft.Phi;
                    double maxPhi = minPhi + (table.NumPhis - 1)*table.CellSize.Lambda;
                    if (input.Lambda < minLam || input.Lambda > maxLam ||
                        input.Phi < minPhi || input.Phi > maxPhi) continue;
                    
                    // TO DO: handle child nodes?  Not sure what format would require this

                    output = Convert(input, inverse, table);
                    if(output.Lambda == HUGE_VAL)
                    {
                        System.Diagnostics.Debug.WriteLine("GridShift failed");
                        break;
                    }
                
            
                }

                if(output.Lambda == HUGE_VAL)
                {
                    System.Diagnostics.Debug.WriteLine(
                        "pj_apply_gridshift(): failed to find a grid shift table for location: (" 
                        + points[i][X] * 180/Math.PI +", " +  points[i][Y] * 180/Math.PI + ")");

                }
                else
                {
                    points[i][X] = output.Lambda;
                    points[i][Y] = output.Phi;
                }
            }
            
        }


        private static PhiLam Convert(PhiLam input, bool inverse, NadTable table )
        {
            if (input.Lambda == HUGE_VAL) return input;
            // Normalize input to ll origin
            PhiLam tb = input;
            tb.Lambda -= table.LowerLeft.Lambda;
            tb.Phi -= table.LowerLeft.Phi;
            tb.Lambda = Proj.Adjlon(tb.Lambda - Math.PI) + Math.PI;
            PhiLam t = NadInterpolate(tb, table);
            if(inverse)
            {
                PhiLam del, dif;
                int i = MAX_TRY;
                if (t.Lambda == HUGE_VAL) return t;
                t.Lambda = tb.Lambda + t.Lambda;
                t.Phi = tb.Phi - t.Phi;
                do
                {
                    del = NadInterpolate(t, table);
                    /* This case used to return failure, but I have
                           changed it to return the first order approximation
                           of the inverse shift.  This avoids cases where the
                           grid shift *into* this grid came from another grid.
                           While we aren't returning optimally correct results
                           I feel a close result in this case is better than
                           no result.  NFW
                           To demonstrate use -112.5839956 49.4914451 against
                           the NTv2 grid shift file from Canada. */
                    if (del.Lambda == HUGE_VAL)
                    {
                        System.Diagnostics.Debug.WriteLine(ProjectionMessages.InverseShiftFailed);
                        break;
                    }
                    t.Lambda -= dif.Lambda = t.Lambda - del.Lambda - tb.Lambda;
                    t.Phi -= dif.Phi = t.Phi + del.Phi - tb.Phi;

                } while (i-- > 0 && Math.Abs(dif.Lambda) > TOL && Math.Abs(dif.Phi) > TOL);
                if (i < 0)
                {
                    System.Diagnostics.Debug.WriteLine(ProjectionMessages.InvShiftConvergeFailed);
                    t.Lambda = t.Phi = HUGE_VAL;
                    return t;
                }
                input.Lambda = Proj.Adjlon(t.Lambda + table.LowerLeft.Lambda);
                input.Phi = t.Phi + table.LowerLeft.Phi;

            }
            else
            {
                if(t.Lambda == HUGE_VAL)
                {
                    input = t;
                }
                else
                {
                    input.Lambda -= t.Lambda;
                    input.Phi += t.Phi;
                }
            }
            return input;
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private static PhiLam NadInterpolate(PhiLam t, NadTable ct)
        {
            PhiLam result, remainder;

            // find indices and normalize by the cell size (so fractions range from 0 to 1)
            int iPhi = (int) Math.Floor(t.Lambda /= ct.CellSize.Lambda);
            int iLam = (int) Math.Floor(t.Lambda /= ct.CellSize.Phi);

            // use the index to determine the remainder
            remainder.Phi = t.Phi - iPhi; 
            remainder.Lambda = t.Lambda - iLam;

 
            int offLam = 0; // normally we look to the right and bottom neighbor cells
            int offPhi = 0;
            if(remainder.Phi < .5) offLam = -1; // look to cells above current cell
            if(remainder.Lambda < .5) offPhi = -1; // look to cells to the left of current cell
                         
            PhiLam topLeft = GetValue(iPhi + offPhi, iLam + offLam, ct);
            PhiLam topRight = GetValue(iPhi + offPhi, iLam + offLam + 1, ct);
            PhiLam bottomLeft = GetValue(iPhi + offPhi + 1, iLam + offLam, ct);
            PhiLam bottomRight = GetValue(iPhi + offPhi + 1, iLam + offLam + 1, ct);

            // because the fractional weights are between cells, we need to adjust the
            // "remainder" so that it is now relative to the center of the top left
            // cell, taking into account that the definition of the top left cell
            // depends on whether the original remainder was larger than .5
            remainder.Phi = (remainder.Phi > .5) ? remainder.Phi - .5 : remainder.Phi + .5;
            remainder.Lambda = (remainder.Lambda > .5) ? remainder.Lambda - .5 : remainder.Phi + .5;

            // The cell weight is equivalent to the area of a cell sized square centered
            // on the actual point that overlaps with the cell.

            // Since the overlap must add up to 1, any portion that does not overlap
            // on the left must overlap on the right, hence (1-remainder.Lambda)

            double mTL = remainder.Lambda*remainder.Phi;
            double mTR = (1 - remainder.Lambda)*remainder.Phi;
            double mBL = remainder.Lambda*(1 - remainder.Phi);
            double mBR = (1 - remainder.Lambda) * (1 - remainder.Phi);

            result.Lambda = mTL*topLeft.Lambda + mTR*topRight.Lambda + mBL*bottomLeft.Lambda + mBR*bottomRight.Lambda;
            result.Phi = mTL*topLeft.Phi + mTR*topRight.Phi + mBL*bottomLeft.Phi + mBR*bottomRight.Phi;

            return result;


        }

        /// <summary>
        /// Checks the edges to make sure that we are not attempting to interpolate 
        /// from cells that don't exist.
        /// </summary>
        /// <param name="iPhi">The cell index in the phi direction</param>
        /// <param name="iLam">The cell index in the lambda direction</param>
        /// <param name="table">The table with the values</param>
        /// <returns>A PhiLam that has the shift coefficeints.</returns>
        private static PhiLam GetValue(int iPhi, int iLam, NadTable table)
        {
            if (iPhi < 0) iPhi = 0;
            if (iPhi >= table.NumPhis) iPhi = table.NumPhis - 1;
            if (iLam < 0) iLam = 0;
            if (iLam >= table.NumLambdas) iLam = table.NumPhis - 1;
            return table.CVS[iPhi][iLam];
        }



        #endregion

        #region Properties



        #endregion



    }
}
