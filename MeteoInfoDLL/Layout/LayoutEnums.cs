using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Layout
{
    /// <summary>
    /// An enumeration that defines the Mouses current behavior
    /// </summary>
    public enum MouseMode
    {
        /// <summary>
        /// The cursor is currently in default mode
        /// </summary>
        Default,        

        /// <summary>
        /// The cursor is currently in select mode
        /// </summary>
        Select,

        /// <summary>
        /// The cursor is currently being used to create a new selection
        /// </summary>
        CreateSelection,

        /// <summary>
        /// The cursor is currently is move selection mode
        /// </summary>
        MoveSelection,

        /// <summary>
        /// The cursor is in resize mode because its over the edge of a selected item
        /// </summary>
        ResizeSelected,

        /// <summary>
        /// When in this mode the user can click on the map select an area and an element is inserted at that spot
        /// </summary>
        InsertNewElement,

        /// <summary>
        /// In this mode a cross hair is shown letting the user create a new Insert rectangle
        /// </summary>
        StartInsertNewElement,

        /// <summary>
        /// Puts the mouse into a mode that allows map panning
        /// </summary>
        StartPanMap,

        /// <summary>
        /// The mouse is actually panning a map
        /// </summary>
        PanMap,
        /// <summary>
        /// New point
        /// </summary>
        New_Point,
        /// <summary>
        /// New label
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
        /// In editing vertices status
        /// </summary>
        InEditingVertices,
        /// <summary>
        /// Map zoom in
        /// </summary>
        Map_ZoomIn,
        /// <summary>
        /// Map zoom out
        /// </summary>
        Map_ZoomOut,
        /// <summary>
        /// Map pan
        /// </summary>
        Map_Pan,
        /// <summary>
        /// Map identifer
        /// </summary>
        Map_Identifer,
        /// <summary>
        /// Map select Features by rectangle
        /// </summary>
        Map_SelectFeatures_Rectangle,
        /// <summary>
        /// Map select features by polygon
        /// </summary>
        Map_SelectFeatures_Polygon,
        /// <summary>
        /// Map select features by lasso
        /// </summary>
        Map_SelectFeatures_Lasso,
        /// <summary>
        /// Map select features by circle
        /// </summary>
        Map_SelectFeatures_Circle,
        /// <summary>
        /// Map measurement
        /// </summary>
        Map_Measurement
    }

    /// <summary>
    /// Enumerates all the possible resize direction
    /// </summary>
    internal enum Edge
    {
        /// <summary>
        /// None
        /// </summary>
        None,

        /// <summary>
        /// Top left
        /// </summary>
        TopLeft,

        /// <summary>
        /// Top
        /// </summary>
        Top,

        /// <summary>
        /// Top right
        /// </summary>
        TopRight,

        /// <summary>
        /// Right
        /// </summary>
        Right,

        /// <summary>
        /// Bottom right
        /// </summary>
        BottomRight,

        /// <summary>
        /// Bottom
        /// </summary>
        Bottom,

        /// <summary>
        /// Bottom left
        /// </summary>
        BottomLeft,

        /// <summary>
        /// Left
        /// </summary>
        Left,
    }

    /// <summary>
    /// Layout element type
    /// </summary>
    public enum ElementType
    {
        /// <summary>
        /// Layout map element
        /// </summary>
        LayoutMap,
        /// <summary>
        /// Layout illustration element
        /// </summary>
        LayoutIllustration,
        /// <summary>
        /// Layout legend element
        /// </summary>
        LayoutLegend,
        /// <summary>
        /// Layout grahic element
        /// </summary>
        LayoutGraphic,
        /// <summary>
        /// Layout scale bar
        /// </summary>
        LayoutScaleBar,
        /// <summary>
        /// Layout north arraw
        /// </summary>
        LayoutNorthArraw
    }

    /// <summary>
    /// Resize ability enum
    /// </summary>
    public enum ResizeAbility
    {
        /// <summary>
        /// Can not be resized
        /// </summary>
        None,
        /// <summary>
        /// Keep same width and heigh during resizing
        /// </summary>
        SameWidthHeight,
        /// <summary>
        /// No limitation resize ability
        /// </summary>
        ResizeAll
    }

    /// <summary>
    /// Scale bar units
    /// </summary>
    public enum ScaleBarUnits
    {
        /// <summary>
        /// Kilometers
        /// </summary>
        Kilometers,
        /// <summary>
        /// Meters
        /// </summary>
        Meters,
        /// <summary>
        /// Centimeters
        /// </summary>
        Centimeters,
        /// <summary>
        /// Millimeters
        /// </summary>
        Millimeters,
        /// <summary>
        /// Miles
        /// </summary>
        Miles,
        /// <summary>
        /// Yards
        /// </summary>
        Yards,
        /// <summary>
        /// Feet
        /// </summary>
        Feet,
        /// <summary>
        /// Inches
        /// </summary>
        Inches
    }

    /// <summary>
    /// Scale bar type enum
    /// </summary>
    public enum ScaleBarTypes
    {
        /// <summary>
        /// Scale line1
        /// </summary>
        ScaleLine1,
        /// <summary>
        /// Scale line 2
        /// </summary>
        ScaleLine2,
        /// <summary>
        /// Alternating scale bar
        /// </summary>
        AlternatingBar
    }

    /// <summary>
    /// North arrow type enum
    /// </summary>
    public enum NorthArrowTypes
    {
        /// <summary>
        /// North arrow 1
        /// </summary>
        NorthArrow1,
        /// <summary>
        /// North arrow 2
        /// </summary>
        NorthArrow2
    }
}
