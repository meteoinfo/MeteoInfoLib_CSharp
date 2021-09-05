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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/17/2009 1:47:05 PM
// 
// Contributor(s): (Open source contributors should list themselves and their modifications here). 
//
//********************************************************************************************************

using System;
using System.IO;


namespace MeteoInfoC.Projections
{


    /// <summary>
    /// NadRecord is a single entry from an lla file
    /// </summary>
    [Serializable]
    public class NadTable
    {
        /// <summary>
        /// The character based id for this record
        /// </summary>
        private string _name;

        /// <summary>
        /// The lower left coordinate
        /// </summary>
        private PhiLam _lowerLeft;

        /// <summary>
        /// The delta lambda and delta phi for a single cell
        /// </summary>
        private PhiLam _cellSize;

        /// <summary>
        /// The total count of coordinates in the lambda direction
        /// </summary>
        private int _numLambdas;

        /// <summary>
        /// The total count of coordinates in the phi direction
        /// </summary>
        private int _numPhis;

        /// <summary>
        /// The set of conversion matrix coefficients for lambda
        /// </summary>
        private PhiLam[][] _cvs;

        /// <summary>
        /// Converts degree values into radians 
        /// </summary>
        private const double DegToRad = Math.PI/180;

        // I think this converts micro-seconds of arc to radians
        private const double USecToRad = 4.848136811095359935899141023e-12;

        /// <summary>
        /// Creates a blank nad table
        /// </summary>
        public NadTable()
        {
            
        }

        /// <summary>
        /// Creates a new instance of a NadTable from the specified stream
        /// </summary>
        /// <param name="llaFileStream"></param>
        public NadTable(Stream llaFileStream)
        {
            ReadStream(llaFileStream);
        }


        #region Methods


        /// <summary>
        /// Reads a given text file with the *.lla extension
        /// </summary>
        /// <param name="filename">The string filename to parse</param>
        public void ReadLlaFile(string filename)
        {
            Stream str = new FileStream(filename, FileMode.Open, FileAccess.Read);
            ReadStream(str);
        }



        /// <summary>
        /// So that we can read from an embedded stream, this reads a NadTable
        /// from any stream, not just a filename.
        /// </summary>
        /// <param name="str">The stream to read</param>
        public void ReadStream(Stream str)
        {
            StreamReader sr = new StreamReader(str);
            _name = sr.ReadLine();
            string numText = sr.ReadToEnd();
            char[] separators = new[] {' ', ',',':', (char)10};
            string[] values = numText.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            _numLambdas = int.Parse(values[0]);
            _numPhis = int.Parse(values[1]);
            _lowerLeft.Lambda = double.Parse(values[3]) * DegToRad;
            _lowerLeft.Phi = double.Parse(values[4]) * DegToRad;
            _cellSize.Lambda = double.Parse(values[5]) * DegToRad;
            _cellSize.Phi = double.Parse(values[6]) * DegToRad;
            int p = 7;
            _cvs = new PhiLam[_numPhis][];
            for(int i = 0; i < _numPhis; i++)
            {
                _cvs[i] = new PhiLam[_numLambdas];
                int iCheck = int.Parse(values[p]);
                if (iCheck != i)
                {
                    throw new ProjectionException(ProjectionMessages.IndexMismatch);
                }
                p++;
                double lam = long.Parse(values[p]) * USecToRad;
                _cvs[i][0].Lambda = lam;
                p++;
                double phi = long.Parse(values[p]) * USecToRad;
                _cvs[i][0].Phi = phi;
                p++;
                for (int j = 1; j < _numLambdas; j++ )
                {
                    lam += long.Parse(values[p]) * USecToRad;
                    _cvs[i][j].Lambda = lam;
                    p++;
                    phi += long.Parse(values[p])*USecToRad;
                    _cvs[i][j].Phi = phi;
                    p++;
                }
            }

        }

      
       


        /// <summary>
        /// Gets or sets the string name for this record
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the lower left corner in radians
        /// </summary>
        public PhiLam LowerLeft
        {
            get { return _lowerLeft; }
            set { _lowerLeft = value; }
        }

        /// <summary>
        /// Gets or sets the angular cell size in radians
        /// </summary>
        public PhiLam CellSize
        {
            get { return _cellSize; }
            set { _cellSize = value; }
        }

        /// <summary>
        /// Gets or sets the integer count of phi coefficients
        /// </summary>
        public int NumPhis
        {
            get { return _numPhis; }
            set { _numPhis = value; }
        }

        /// <summary>
        /// Gets or sets the integer count of lambda coefficients
        /// </summary>
        public int NumLambdas
        {
            get { return _numLambdas; }
            set { _numLambdas = value; }
        }

        /// <summary>
        /// Gets or sets the array of lambda coefficients organized 
        /// in a spatial table (phi major)
        /// </summary>
        public PhiLam[][] CVS
        {
            get { return _cvs; }
            set { _cvs = value; }
        }

       
        
        #endregion

    }
}
