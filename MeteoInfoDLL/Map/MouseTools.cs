using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Map
{
    /// <summary>
    /// Mouse tools
    /// </summary>
    public enum MouseTools
    {
        /// <summary>
        /// Zoom in
        /// </summary>
        Zoom_In,
        /// <summary>
        /// Zoom out
        /// </summary>
        Zoom_Out,
        /// <summary>
        /// Pan
        /// </summary>
        Pan,
        /// <summary>
        /// Identifer
        /// </summary>
        Identifer,
        /// <summary>
        /// Select Elements
        /// </summary>
        SelectElements,
        /// <summary>
        /// Move selection
        /// </summary>
        MoveSelection,
        /// <summary>
        /// Resize selection
        /// </summary>
        ResizeSelection,
        /// <summary>
        /// Create selection
        /// </summary>
        CreateSelection,
        /// <summary>
        /// Select Features by rectangle
        /// </summary>
        SelectFeatures_Rectangle,
        /// <summary>
        /// Select Features by polygon
        /// </summary>
        SelectFeatures_Polygon,
        /// <summary>
        /// Select Features by lasso
        /// </summary>
        SelectFeatures_Lasso,
        /// <summary>
        /// Select Features by circle
        /// </summary>
        SelectFeatures_Circle,
        /// <summary>
        /// New point
        /// </summary>
        New_Point,
        /// <summary>
        /// New Lable
        /// </summary>
        New_Label,
        /// <summary>
        /// New polyline
        /// </summary>
        New_Polyline,
        /// <summary>
        /// New freehand
        /// </summary>
        New_Freehand,
        /// <summary>
        /// New curve
        /// </summary>
        New_Curve,
        /// <summary>
        /// New curve polygon
        /// </summary>
        New_CurvePolygon,
        /// <summary>
        /// New polygon
        /// </summary>
        New_Polygon,
        /// <summary>
        /// New rectangle
        /// </summary>
        New_Rectangle,
        /// <summary>
        /// New ellipse
        /// </summary>
        New_Ellipse,
        /// <summary>
        /// New circle
        /// </summary>
        New_Circle,
        /// <summary>
        /// Edit vertices of polyline or polygon
        /// </summary>
        EditVertices,
        /// <summary>
        /// Is editing vertices
        /// </summary>
        InEditingVertices,
        /// <summary>
        /// Measurement
        /// </summary>
        Measurement,
        /// <summary>
        /// None
        /// </summary>        
        None
    }
}
