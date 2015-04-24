using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MeteoInfoC.Layout
{
    /// <summary>
    /// Layout element abstract class
    /// </summary>
    public abstract class LayoutElement
    {
        #region Events definition
        /// <summary>
        /// Occurs afate location changed
        /// </summary>
        public event EventHandler LocationChanged;
        /// <summary>
        /// Occurs afte size changed
        /// </summary>
        public event EventHandler SizeChanged;

        #endregion

        #region Variables
        private int _left;
        private int _top;
        private int _width;
        private int _height;
        private ElementType _elementType;
        private Color _foreColor;
        private Color _backColor;
        private bool _Selected;
        private ResizeAbility _resizeAbility;
        private bool _visible = true;
        private string _name = "";

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public LayoutElement()
        {
            _foreColor = Color.Black;
            _backColor = Color.Transparent;
            _Selected = false;
            _resizeAbility = ResizeAbility.None;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Get or set if visible
        /// </summary>
        public virtual bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        /// <summary>
        /// Get or set left position
        /// </summary>
        public virtual int Left
        {
            get { return _left; }
            set 
            { 
                _left = value;
                OnLocationChanged();
            }
        }

        /// <summary>
        /// Get or set top position
        /// </summary>
        public virtual int Top
        {
            get { return _top; }
            set 
            { 
                _top = value;
                OnLocationChanged();
            }
        }

        /// <summary>
        /// Get or set width
        /// </summary>
        public virtual int Width
        {
            get { return _width; }
            set 
            { 
                _width = value;
                OnSizeChanged();
            }            
        }

        /// <summary>
        /// Get or set height
        /// </summary>
        public virtual int Height
        {
            get { return _height; }
            set 
            {
                _height = value;
                OnSizeChanged();
            }
        }

        /// <summary>
        /// Get right
        /// </summary>
        public int Right
        {
            get { return Left + Width; }
        }

        /// <summary>
        /// Get bottom
        /// </summary>
        public int Bottom
        {
            get { return Top + Height; }
        }

        /// <summary>
        /// Get bounds rectangle
        /// </summary>
        public virtual Rectangle Bounds
        {
            get
            {
                Rectangle rect = new Rectangle(Left, Top, Width, Height);
                return rect;
            }
            set { }
        }

        /// <summary>
        /// Get element type
        /// </summary>
        public ElementType ElementType
        {
            get { return _elementType; }
            set { _elementType = value; }
        }

        /// <summary>
        /// Get or set fore color
        /// </summary>
        public Color ForeColor
        {
            get { return _foreColor; }
            set { _foreColor = value; }
        }

        /// <summary>
        /// Get or set back color
        /// </summary>
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        /// <summary>
        /// Get or set if selected
        /// </summary>
        public bool Selected
        {
            get { return _Selected; }
            set { _Selected = value; }
        }

        /// <summary>
        /// Get or set resize ability
        /// </summary>
        public ResizeAbility ResizeAbility
        {
            get { return _resizeAbility; }
            set { _resizeAbility = value; }
        }

        /// <summary>
        /// Get or set name
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Abstract method paint
        /// </summary>
        /// <param name="g">graphics</param>
        public abstract void Paint(Graphics g);

        /// <summary>
        /// Abstract method PaintOnLayout
        /// </summary>
        /// <param name="g">graphics</param>
        /// <param name="pageLocation">page location</param>
        /// <param name="zoom">zoom</param>
        public abstract void PaintOnLayout(Graphics g, PointF pageLocation, float zoom);

        /// <summary>
        /// Virtual move update method
        /// </summary>
        public virtual void MoveUpdate()
        {

        }

        /// <summary>
        /// Virtual resize update method
        /// </summary>
        public virtual void ResizeUpdate()
        {

        }

        /// <summary>
        /// Abstract get property object method
        /// </summary>
        /// <returns></returns>
        public abstract object GetPropertyObject();

        /// <summary>
        /// Page to screen
        /// </summary>
        /// <param name="pageX">page X</param>
        /// <param name="pageY">page Y</param>
        /// <param name="pageLocation">page location</param>
        /// <param name="zoom">zoom</param>
        /// <returns>screen point</returns>
        public PointF PageToScreen(float pageX, float pageY, PointF pageLocation, float zoom)
        {
            float x = pageX * zoom + pageLocation.X;
            float y = pageY * zoom + pageLocation.Y;
            return (new PointF(x, y));
        }        

        #endregion

        #region Events
        /// <summary>
        /// Fire the location changed event
        /// </summary>
        protected virtual void OnLocationChanged()
        {
            if (LocationChanged != null) LocationChanged(this, new EventArgs());
        }

        /// <summary>
        /// Fire the size changed event
        /// </summary>
        protected virtual void OnSizeChanged()
        {
            if (SizeChanged != null) SizeChanged(this, new EventArgs());
        }

        #endregion
    }
}
