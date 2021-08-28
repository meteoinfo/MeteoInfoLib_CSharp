using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeteoInfoC.Global;

namespace MeteoInfoC.Projections
{
    /// <summary>
    /// Projection manage class
    /// </summary>
    public class ProjectionManage
    {
        /// <summary>
        /// Get global extent of a projection
        /// </summary>
        /// <param name="toProj">projection information</param>
        /// <returns>extent</returns>
        public static Extent GetProjectionGlobalExtent(ProjectionInfo toProj)
        {
            ProjectionInfo fromProj = KnownCoordinateSystems.Geographic.World.WGS1984;
            double x, y, minX = double.NaN, minY = double.NaN, maxX = double.NaN, maxY = double.NaN;
            int si = -90;
            int ei = 90;
            switch (toProj.Transform.ProjectionName)
            {
                case ProjectionNames.Lambert_Conformal:
                    si = -80;
                    break;
                case ProjectionNames.North_Polar_Stereographic:
                    si = 0;
                    break;
                case ProjectionNames.South_Polar_Stereographic:
                    ei = 0;
                    break;
            }
            for (int i = si; i <= ei; i++)
            {
                y = i;
                for (int j = -180; j <= 180; j++)
                {
                    x = i;
                    double[][] points = new double[1][];
                    points[0] = new double[] { x, y };
                    try
                    {
                        Reproject.ReprojectPoints(points, fromProj, toProj, 0, 1);
                        x = points[0][0];
                        y = points[0][1];
                        if (double.IsNaN(x) || double.IsNaN(y))
                        {
                            //j++;
                            continue;
                        }
                        if (double.IsNaN(minX))
                        {
                            minX = x;
                            minY = y;
                        }
                        else
                        {
                            if (x < minX)
                                minX = x;
                            if (y < minY)
                                minY = y;
                        }
                        if (double.IsNaN(maxX))
                        {
                            maxX = x;
                            maxY = y;
                        }
                        else
                        {
                            if (x > maxX)
                                maxX = x;
                            if (y > maxY)
                                maxY = y;
                        }
                    }
                    catch
                    {
                        //j++;
                        continue;
                    }
                }
            }

            Extent aExtent = new Extent();
            aExtent.minX = minX;
            aExtent.maxX = maxX;
            aExtent.minY = minY;
            aExtent.maxY = maxY;
            return aExtent;
        }

        /// <summary>
        /// Get projected extent
        /// </summary>
        /// <param name="fromProj">from projection</param>
        /// <param name="toProj">to projection</param>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        /// <returns>extent</returns>
        public static Extent GetProjectionExtent(ProjectionInfo fromProj, ProjectionInfo toProj, double[] X, double[] Y)
        {
            double x, y, minX = double.NaN, minY = double.NaN, maxX = double.NaN, maxY = double.NaN;
            int i;
            for (i = 0; i < Y.Length; i++)
            {
                switch (toProj.Transform.ProjectionName)
                {
                    case ProjectionNames.Lambert_Conformal:
                        if (Y[i] < -80)
                            continue;
                        break;
                    case ProjectionNames.North_Polar_Stereographic:
                        if (Y[i] < 0)
                            continue;
                        break;
                    case ProjectionNames.South_Polar_Stereographic:
                        if (Y[i] > 0)
                            continue;
                        break;
                }
                double[][] points = new double[1][];
                points[0] = new double[] { X[0], Y[i] };
                try
                {
                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, 1);
                    x = points[0][0];
                    y = points[0][1];
                    if (double.IsNaN(x) || double.IsNaN(y))
                        continue;

                    if (double.IsNaN(minX))
                    {
                        minX = x;
                        minY = y;
                    }
                    else
                    {
                        if (x < minX)
                            minX = x;
                        if (y < minY)
                            minY = y;
                    }
                }
                catch
                {                    
                    continue;
                }

                points[0] = new double[] { X[X.Length - 1], Y[i] };
                try
                {
                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, 1);
                    x = points[0][0];
                    y = points[0][1];
                    if (double.IsNaN(x) || double.IsNaN(y))
                        continue;

                    if (double.IsNaN(maxX))
                    {
                        maxY = y;
                        maxX = x;
                    }
                    else
                    {
                        if (x > maxX)
                            maxX = x;
                        if (y > maxY)
                            maxY = y;
                    }
                }
                catch
                {
                    continue;
                }
            }

            int yIdx = 0;
            int eyIdx = Y.Length - 1;
            switch (toProj.Transform.ProjectionName)
            {
                case ProjectionNames.Lambert_Conformal:
                    for (i = 0; i < Y.Length; i++)
                    {
                        if (Y[i] >= -80)
                        {
                            yIdx = i;
                            break;
                        }
                    }
                    break;
                case ProjectionNames.North_Polar_Stereographic:
                    for (i = 0; i < Y.Length; i++)
                    {
                        if (Y[i] >= 0)
                        {
                            yIdx = i;
                            break;
                        }
                    }
                    break;
                case ProjectionNames.South_Polar_Stereographic:
                    for (i = 0; i < Y.Length; i++)
                    {
                        if (Y[i] > 0)
                        {
                            eyIdx = i - 1;
                            break;
                        }
                    }
                    break;
            }
            if (eyIdx < 0)
                eyIdx = 0;

            for (i = 0; i < X.Length; i++)
            {
                double[][] points = new double[1][];
                points[0] = new double[] { X[i], Y[yIdx] };
                try
                {
                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, 1);
                    x = points[0][0];
                    y = points[0][1];
                    if (double.IsNaN(x) || double.IsNaN(y))
                        continue;

                    if (double.IsNaN(minX))
                    {
                        minX = x;
                        minY = y;
                    }
                    else
                    {
                        if (x < minX)
                            minX = x;
                        if (y < minY)
                            minY = y;
                    }
                }
                catch
                {
                    continue;
                }

                points[0] = new double[] { X[i], Y[eyIdx] };
                try
                {
                    Reproject.ReprojectPoints(points, fromProj, toProj, 0, 1);
                    x = points[0][0];
                    y = points[0][1];
                    if (double.IsNaN(x) || double.IsNaN(y))
                        continue;

                    if (double.IsNaN(maxX))
                    {
                        maxX = x;
                        maxY = y;
                    }
                    else
                    {
                        if (x > maxX)
                            maxX = x;
                        if (y > maxY)
                            maxY = y;
                    }
                }
                catch
                {
                    continue;
                }
            }

            Extent aExtent = new Extent();
            aExtent.minX = minX;
            aExtent.maxX = maxX;
            aExtent.minY = minY;
            aExtent.maxY = maxY;

            return aExtent;
        }
    }
}
