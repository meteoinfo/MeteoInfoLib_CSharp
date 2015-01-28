using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 2 parameter table
    /// </summary>
    public class GRIB2ParameterTable
    {
        #region Variables
        private static Hashtable _ParameterTable;
        //static GRIB2ParameterTable()
        //{
        //    Stream aFile = Assembly.GetExecutingAssembly().GetManifestResourceStream("MeteoInfoC.Data.MeteoData.grib2Parameters.xml");
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(aFile);
        //    XmlElement root = doc.DocumentElement;
        //    XmlNodeList disciplines = root.GetElementsByTagName("discipline");
        //    _ParameterTable = new Hashtable();
        //    foreach (XmlNode dis in disciplines)
        //    {
        //        Discipline aDis = new Discipline();
        //        aDis.Number = int.Parse(dis.Attributes["number"].InnerText);
        //        aDis.Name = dis.Attributes["id"].InnerText;
        //        foreach (XmlNode cat in dis.ChildNodes)
        //        {
        //            Category aCat = new Category();
        //            aCat.Number = int.Parse(cat.Attributes["number"].InnerText);
        //            aCat.Name = cat.Attributes["id"].InnerText;
        //            foreach (XmlNode par in cat.ChildNodes)
        //            {
        //                Parameter aPar = new Parameter();
        //                aPar.Number = int.Parse(par.Attributes["number"].InnerText);
        //                aPar.Name = par.Attributes["id"].InnerText;
        //                aPar.Units = par.Attributes["unit"].InnerText;
        //                aPar.Description = par.Attributes["description"].InnerText;
        //                aCat.SetParameter(aPar);
        //            }
        //            aDis.SetCategory(aCat);
        //        }
        //        _ParameterTable[aDis.Number] = aDis;
        //    }
        //}

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>        
        public GRIB2ParameterTable()
        {
            Stream aFile = Assembly.GetExecutingAssembly().GetManifestResourceStream
                ("MeteoInfoC.Data.MeteoData.GRIB.GRIB2.grib2Parameters.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(aFile);
            XmlElement root = doc.DocumentElement;
            XmlNodeList disciplines = root.GetElementsByTagName("discipline");
            _ParameterTable = new Hashtable();
            foreach (XmlNode dis in disciplines)
            {
                Discipline aDis = new Discipline();
                aDis.Number = int.Parse(dis.Attributes["number"].InnerText);
                aDis.Name = dis.Attributes["id"].InnerText;
                foreach (XmlNode cat in dis.ChildNodes)
                {
                    Category aCat = new Category();
                    aCat.Number = int.Parse(cat.Attributes["number"].InnerText);
                    aCat.Name = cat.Attributes["id"].InnerText;
                    foreach (XmlNode par in cat.ChildNodes)
                    {
                        Variable aPar = new Variable();
                        aPar.Number = int.Parse(par.Attributes["number"].InnerText);
                        aPar.Name = par.Attributes["id"].InnerText;
                        aPar.Units = par.Attributes["unit"].InnerText;
                        aPar.Description = par.Attributes["description"].InnerText;
                        aCat.SetParameter(aPar);
                    }
                    aDis.SetCategory(aCat);
                }
                _ParameterTable[aDis.Number] = aDis;
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// Parameter table
        /// </summary>
        public Hashtable ParameterTable
        {
            get { return _ParameterTable; }
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get a parameter
        /// </summary>
        /// <param name="disNum">discipline number</param>
        /// <param name="catNum">category number</param>
        /// <param name="parNum">parameter number</param>
        /// <returns>parameter</returns>
        public Variable GetParameter(int disNum, int catNum, int parNum)
        {
            Discipline dis = GetDiscipline(disNum);
            if (dis == null )
                return new Variable(parNum, "UnknownDiscipline_" + disNum.ToString(), 
                    "UnknownDiscipline_" + disNum.ToString(), "Unknown");
            Category cat = dis.GetCategory(catNum);
            if (cat == null)
                return new Variable(parNum, "UnknownCategory_" + catNum.ToString(),
                    "UnknownCategory_" + catNum.ToString(), "Unknown");
            Variable par = cat.GetParameter(parNum);
            if (par == null )
                return new Variable(parNum, "UnknownParameter_" + parNum.ToString(),
                    "UnknownParameter_" + parNum.ToString(), "Unknown");

            return par;
        }

        /// <summary>
        /// Get a discipline
        /// </summary>
        /// <param name="disNum">discipline number</param>
        /// <returns>discipline</returns>
        public Discipline GetDiscipline(int disNum)
        {            
            return (Discipline)_ParameterTable[disNum];
        }

        #endregion
    }
}
