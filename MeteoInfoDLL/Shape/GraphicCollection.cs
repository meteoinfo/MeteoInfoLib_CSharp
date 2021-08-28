using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using MeteoInfoC.Global;

namespace MeteoInfoC.Shape
{
    /// <summary>
    /// Graphic collection
    /// </summary>
    public class GraphicCollection
    {
        #region Variables
        private List<Graphic> _graphicList = new List<Graphic>();        
        private Extent _extent;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public GraphicCollection()
        {

        }

        #endregion

        #region Properties
        /// <summary>
        /// Get graphic list
        /// </summary>
        public List<Graphic> GraphicList
        {
            get { return _graphicList; }           
        }

        /// <summary>
        /// Get graphic number
        /// </summary>
        public int Count
        {
            get { return _graphicList.Count; }
        }

        /// <summary>
        /// Get extent
        /// </summary>
        public Extent Extent
        {
            get { return _extent; }            
        }

        #endregion

        #region Methods
        /// <summary>
        /// Add a graphic
        /// </summary>
        /// <param name="aGraphic">graphic</param>
        public void Add(Graphic aGraphic)
        {
            _graphicList.Add(aGraphic);            

            //Update extent
            if (_graphicList.Count == 1)
                _extent = aGraphic.Shape.Extent;
            else
                _extent = MIMath.GetLagerExtent(_extent, aGraphic.Shape.Extent);
        }

        /// <summary>
        /// Insert a graphic
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="aGraphic">graphic</param>
        public void Insert(int index, Graphic aGraphic)
        {
            _graphicList.Insert(index, aGraphic);

            //Update extent
            if (_graphicList.Count == 1)
                _extent = aGraphic.Shape.Extent;
            else
                _extent = MIMath.GetLagerExtent(_extent, aGraphic.Shape.Extent);
        }

        /// <summary>
        /// Remove a grphic
        /// </summary>
        /// <param name="aGraphic">graphic</param>
        public void Remove(Graphic aGraphic)
        {
            if (_graphicList.Contains(aGraphic))
                _graphicList.Remove(aGraphic);
        }

        /// <summary>
        /// Remove at an index
        /// </summary>
        /// <param name="index">index</param>
        public void RemoveAt(int index)
        {
            _graphicList.RemoveAt(index);
        }

        /// <summary>
        /// Remove all graphics
        /// </summary>
        public void RemoveAll()
        {
            while (Count > 0)
            {               
                _graphicList.RemoveAt(0);
            }
        }

        /// <summary>
        /// Select graphics by an extent
        /// </summary>
        /// <param name="aExtent">extent</param>
        /// <param name="selectedGraphics">ref selected graphics</param>
        /// <returns>if selected</returns>
        public bool SelectGraphics(Extent aExtent, ref GraphicCollection selectedGraphics)
        {
            selectedGraphics.GraphicList.Clear();
            int i, j;
            PointD aPoint = new PointD();
            aPoint.X = (aExtent.minX + aExtent.maxX) / 2;
            aPoint.Y = (aExtent.minY + aExtent.maxY) / 2;

            foreach (Graphic aGraphic in _graphicList)
            {
                switch (aGraphic.Shape.ShapeType)
                {
                    case ShapeTypes.Point:
                        for (i = 0; i < Count; i++)
                        {
                            PointShape aPS = (PointShape)_graphicList[i].Shape;
                            if (MIMath.PointInExtent(aPS.Point, aExtent))
                            {
                                selectedGraphics.Add(aGraphic);
                            }
                        }
                        break;                    
                    case ShapeTypes.Polyline:
                    case ShapeTypes.PolylineZ:
                        for (i = 0; i < Count; i++)
                        {
                            PolylineShape aPLS = (PolylineShape)_graphicList[i].Shape;
                            if (MIMath.IsExtentCross(aExtent, aPLS.Extent))
                            {
                                for (j = 0; j < aPLS.Points.Count; j++)
                                {
                                    aPoint = aPLS.Points[j];
                                    if (MIMath.PointInExtent(aPoint, aExtent))
                                    {
                                        selectedGraphics.Add(aGraphic);
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    case ShapeTypes.Polygon:
                    case ShapeTypes.Rectangle:
                        for (i = Count - 1; i >= 0; i--)
                        {
                            PolygonShape aPGS = (PolygonShape)_graphicList[i].Shape;
                            if (!(aPGS.PartNum > 1))
                            {
                                if (MIMath.PointInPolygon(aPGS.Points, aPoint))
                                {
                                    selectedGraphics.Add(aGraphic);
                                }
                            }
                            else
                            {
                                for (int p = 0; p < aPGS.PartNum; p++)
                                {
                                    ArrayList pList = new ArrayList();
                                    if (p == aPGS.PartNum - 1)
                                    {
                                        for (int pp = aPGS.parts[p]; pp < aPGS.PointNum; pp++)
                                        {
                                            pList.Add(aPGS.Points[pp]);
                                        }
                                    }
                                    else
                                    {
                                        for (int pp = aPGS.parts[p]; pp < aPGS.parts[p + 1]; pp++)
                                        {
                                            pList.Add(aPGS.Points[pp]);
                                        }
                                    }
                                    if (MIMath.PointInPolygon(pList, aPoint))
                                    {
                                        selectedGraphics.Add(aGraphic);
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                }
            }

            if (selectedGraphics.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
