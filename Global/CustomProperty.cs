using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MeteoInfoC
{
    class CustomProperty : ICustomTypeDescriptor
    {
        #region Variables
        private object _CurrentSelectObject;
        private Dictionary<string, string> _ObjectAttribs = new Dictionary<string, string>();

        #endregion

        #region Constructor
        public CustomProperty(object pSelectObject, Dictionary<string, string> objectAttr)
        {
            _CurrentSelectObject = pSelectObject;
            _ObjectAttribs = objectAttr;
        }

        #endregion

        #region Properties
        public Dictionary<string, string> ObjectAttribs
        {
            get { return _ObjectAttribs; }
            set { _ObjectAttribs = value; }
        }

        /// <summary>
        /// Get current select object
        /// </summary>
        public object CurrentSelectObject
        {
            get { return _CurrentSelectObject; }
        }

        #endregion

        #region ICustomTypeDescriptor Members
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(_CurrentSelectObject);
        }
        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(_CurrentSelectObject);
        }
        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(_CurrentSelectObject);
        }
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(_CurrentSelectObject);
        }
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(_CurrentSelectObject);
        }
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(_CurrentSelectObject);
        }
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(_CurrentSelectObject, editorBaseType);
        }
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(_CurrentSelectObject, attributes);
        }
        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(_CurrentSelectObject);
        }
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            List<CustomPropertyDescriptor> tmpPDCLst = new List<CustomPropertyDescriptor>();
            PropertyDescriptorCollection tmpPDC = TypeDescriptor.GetProperties(_CurrentSelectObject, attributes);
            IEnumerator tmpIe = tmpPDC.GetEnumerator();
            CustomPropertyDescriptor tmpCPD;
            PropertyDescriptor tmpPD;
            while (tmpIe.MoveNext())
            {
                tmpPD = tmpIe.Current as PropertyDescriptor;
                if (_ObjectAttribs.ContainsKey(tmpPD.Name))
                {
                    tmpCPD = new CustomPropertyDescriptor(_CurrentSelectObject, tmpPD);
                    tmpCPD.SetDisplayName(_ObjectAttribs[tmpPD.Name]);
                    tmpCPD.SetCategory(tmpPD.Category);
                    tmpPDCLst.Add(tmpCPD);
                }
            }
            return new PropertyDescriptorCollection(tmpPDCLst.ToArray());
        }
        public PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(_CurrentSelectObject);
        }
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return _CurrentSelectObject;
        }
        #endregion

        /// <summary>
        /// CustomPropertyDescriptor class
        /// </summary>
        class CustomPropertyDescriptor : PropertyDescriptor
        {
            private PropertyDescriptor mProp;
            private object mComponent;

            public CustomPropertyDescriptor(object pComponent, PropertyDescriptor pPD)
                : base(pPD)
            {
                mCategory = base.Category;
                mDisplayName = base.DisplayName;
                mProp = pPD;
                mComponent = pComponent;
            }
            private string mCategory;
            public override string Category
            {
                get { return mCategory; }
            }
            private string mDisplayName;
            public override string DisplayName
            {
                get { return mDisplayName; }
            }
            public void SetDisplayName(string pDispalyName)
            {
                mDisplayName = pDispalyName;
            }
            public void SetCategory(string pCategory)
            {
                mCategory = pCategory;
            }
            public override bool CanResetValue(object component)
            {
                return mProp.CanResetValue(component);
            }

            public override Type ComponentType
            {
                get { return mProp.ComponentType; }
            }

            public override object GetValue(object component)
            {
                return mProp.GetValue(component);
            }

            public override bool IsReadOnly
            {
                get { return mProp.IsReadOnly; }
            }

            public override Type PropertyType
            {
                get { return mProp.PropertyType; }
            }
            public override void ResetValue(object component) { mProp.ResetValue(component); }
            public override void SetValue(object component, object value) { mProp.SetValue(component, value); }
            public override bool ShouldSerializeValue(object component)
            {
                return mProp.ShouldSerializeValue(component);
            }
        }
    }
}
