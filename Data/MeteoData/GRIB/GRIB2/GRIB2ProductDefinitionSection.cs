using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB edition 2 product definition section
    /// </summary>
    public class GRIB2ProductDefinitionSection
    {
        #region Variables
        /// <summary>
        /// Undefine data
        /// </summary>
        public double UNDEF = -9999.0;
        /// <summary>
        /// Length in bytes of this PDS.
        /// </summary>
        public int Length;
        /// <summary>
        /// Number of this section, should be 4.
        /// </summary>
        public int SectionNum;

        /**
         * Number of this coordinates.
         */
        private int coordinates;

        /**
         * productDefinition.
         */
        private int productDefinition;

        /**
         * parameterCategory.
         */
        private int parameterCategory;

        /**
         * parameterNumber.
         */
        private int parameterNumber;

        /**
         * typeGenProcess.
         */
        private int typeGenProcess;

        /**
         * backGenProcess.
         */
        private int backGenProcess;

        /**
         * analysisGenProcess.
         */
        private int analysisGenProcess;
        /// <summary>
        /// Hour after
        /// </summary>
        public int HoursAfter;

        /**
         * minutesAfter.
         */
        private int minutesAfter;

        /**
         * timeRangeUnit.
         */
        protected int timeRangeUnit;

        /**
         * forecastTime.
         */
        private int forecastTime;

        /**
         * typeFirstFixedSurface.
         */
        public int TypeFirstFixedSurface;

        /**
         * value of FirstFixedSurface.
         */
        public float FirstFixedSurfaceValue;

        /**
         * typeSecondFixedSurface.
         */
        private int typeSecondFixedSurface;

        /**
         * SecondFixedSurface Value.
         */
        private float SecondFixedSurfaceValue;

        /**
         * Type of Ensemble.
         */
        private int typeEnsemble;

        /**
         * Perturbation number.
         */
        private int perturbNumber;

        /**
         * number of Forecasts.
         */
        private int numberForecasts;

        /**
         * number of bands.
         */
        private int nb;

        /**
         * Model Run/Analysis/Reference time.
         */
        private DateTime endTI;
        private int timeRanges;
        private int[] timeIncrement;
        private float lowerLimit, upperLimit;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="br">binary reader</param>
        public GRIB2ProductDefinitionSection(BinaryReader br)
        {
            long sectionEnd = br.BaseStream.Position;

            // octets 1-4 (Length of PDS)
            Length = Bytes2Number.Int4(br);
            sectionEnd += Length;

            // octet 5
            SectionNum = br.ReadByte();
            //System.out.println( "PDS is 4, section=" + section );

            // octet 6-7
            coordinates = Bytes2Number.Int2(br);
            //System.out.println( "PDS coordinates=" + coordinates );

            // octet 8-9
            productDefinition = Bytes2Number.Int2(br);
            //System.out.println( "PDS productDefinition=" + productDefinition );

            switch (productDefinition)
            {

                // Analysis or forecast at a horizontal level or in a horizontal
                // layer at a point in time
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 11:


                    // octet 10
                    parameterCategory = br.ReadByte();                    

                    // octet 11
                    parameterNumber = br.ReadByte();                    

                    // octet 12
                    typeGenProcess = br.ReadByte();                    

                    // octet 13
                    backGenProcess = br.ReadByte();                    

                    // octet 14
                    analysisGenProcess = br.ReadByte();                    

                    // octet 15-16
                    HoursAfter = Bytes2Number.Int2(br);                    

                    // octet 17
                    minutesAfter = br.ReadByte();                    

                    // octet 18
                    timeRangeUnit = br.ReadByte();                    

                    // octet 19-22
                    forecastTime = Bytes2Number.Int4(br) * CalculateIncrement(timeRangeUnit, 1);                    
                    // octet 23
                    TypeFirstFixedSurface = br.ReadByte();                    

                    // octet 24
                    int scaleFirstFixedSurface = br.ReadByte();                    
                    // octet 25-28
                    int valueFirstFixedSurface = Bytes2Number.Int4(br);                    

                    FirstFixedSurfaceValue = (float)(((scaleFirstFixedSurface
                        == 0) || (valueFirstFixedSurface == 0))
                        ? valueFirstFixedSurface
                        : valueFirstFixedSurface
                        * Math.Pow(10, -scaleFirstFixedSurface));

                    switch (TypeFirstFixedSurface)
                    {
                        case 100:
                            FirstFixedSurfaceValue /= 100;
                            break;
                        case 109:
                            FirstFixedSurfaceValue *= 1000000;
                            break;
                    }                    

                    // octet 29
                    typeSecondFixedSurface = br.ReadByte();                    

                    // octet 30
                    int scaleSecondFixedSurface = br.ReadByte();                    

                    // octet 31-34
                    int valueSecondFixedSurface = Bytes2Number.Int4(br);                    

                    SecondFixedSurfaceValue = (float)(((scaleSecondFixedSurface
                        == 0) || (valueSecondFixedSurface == 0))
                        ? valueSecondFixedSurface
                        : valueSecondFixedSurface
                        * Math.Pow(10, -scaleSecondFixedSurface));

                    switch (typeSecondFixedSurface)
                    {
                        case 100:
                            SecondFixedSurfaceValue /= 100;
                            break;
                        case 109:
                            SecondFixedSurfaceValue *= 1000000;
                            break;
                    } 

                    try
                    {  // catches NotSupportedExceptions

                        // Individual ensemble forecast, control and perturbed, at a
                        // horizontal level or in a horizontal layer at a point in time
                        if ((productDefinition == 1) || (productDefinition == 11))
                        {
                            // octet 35
                            typeEnsemble = br.ReadByte();
                            // octet 36
                            perturbNumber = br.ReadByte();
                            // octet 37
                            numberForecasts = br.ReadByte();
                            /*
                            System.out.println( "Cat ="+ parameterCategory
                             +" parameter ="+ parameterNumber +" Time ="+ forecastTime
                             +" typeEns ="+ typeEnsemble + " perturbation ="+ perturbNumber
                             +" numberOfEns ="+ numberForecasts );
                            */
                            if (productDefinition == 11)
                            {  // skip 38-74-nn detail info                                   
                                br.BaseStream.Position = sectionEnd;
                            }
                            //            if (typeGenProcess == 4) { // ensemble var
                            //              // TODO: should perturbNumber be numberForecasts
                            //              typeGenProcess = 40000 + (1000 * typeEnsemble) + perturbNumber;
                            //            }
                            //System.out.println( "typeGenProcess ="+ typeGenProcess );
                            //Derived forecast based on all ensemble members at a horizontal
                            // level or in a horizontal layer at a point in time
                        }
                        else if (productDefinition == 2)
                        {
                            // octet 35
                            typeEnsemble = br.ReadByte();
                            // octet 36
                            numberForecasts = br.ReadByte();
                            //            if (typeGenProcess == 4) { // ensemble var
                            //              typeGenProcess = 40000 + (1000 * typeEnsemble) + numberForecasts;
                            //            }
                            //            System.out.println( "typeGenProcess ="+ typeGenProcess );
                            //
                            // Derived forecasts based on a cluster of ensemble members over
                            // a rectangular area at a horizontal level or in a horizontal layer
                            // at a point in time
                        }
                        else if (productDefinition == 3)
                        {
                            //System.out.println("PDS productDefinition == 3 not done");
                            throw new NotSupportedException("PDS productDefinition = 3 not implemented");

                            // Derived forecasts based on a cluster of ensemble members
                            // over a circular area at a horizontal level or in a horizontal
                            // layer at a point in time
                        }
                        else if (productDefinition == 4)
                        {
                            //System.out.println("PDS productDefinition == 4 not done");
                            throw new NotSupportedException("PDS productDefinition = 4 not implemented");

                            // Probability forecasts at a horizontal level or in a horizontal
                            //  layer at a point in time
                        }
                        else if (productDefinition == 5)
                        {
                            // 35
                            int probabilityNumber = br.ReadByte();
                            // 36
                            numberForecasts = br.ReadByte();
                            // 37
                            typeEnsemble = br.ReadByte();
                            // 38
                            int scaleFactorLL = br.ReadByte();
                            // 39-42
                            int scaleValueLL = Bytes2Number.Int4(br);
                            lowerLimit = (float)(((scaleFactorLL == 0) || (scaleValueLL == 0))
                                ? scaleValueLL
                                : scaleValueLL * Math.Pow(10, -scaleFactorLL));
                            // 43
                            int scaleFactorUL = br.ReadByte();
                            // 44-47
                            int scaleValueUL = Bytes2Number.Int4(br);
                            upperLimit = (float)(((scaleFactorUL == 0) || (scaleValueUL == 0))
                                ? scaleValueUL
                                : scaleValueUL * Math.Pow(10, -scaleFactorUL));
                            //            if (typeGenProcess == 5) { // Probability var
                            //              typeGenProcess = 50000 + (1000 * typeEnsemble) + totalProbabilities;
                            //            }
                            //System.out.print("PDS productDefinition == 5 PN="+probabilityNumber +" TP="+totalProbabilities +" PT="+probabilityType);
                            //System.out.println( " LL="+lowerLimit +" UL="+upperLimit);
                            //System.out.println( " typeGenProcess ="+ typeGenProcess );

                            // Percentile forecasts at a horizontal level or in a horizontal layer
                            // at a point in time
                        }
                        else if (productDefinition == 6)
                        {
                            //System.out.println("PDS productDefinition == 6 not done");
                            throw new NotSupportedException("PDS productDefinition = 6 not implemented");

                            // Analysis or forecast error at a horizontal level or in a horizontal
                            // layer at a point in time
                        }
                        else if (productDefinition == 7)
                        {
                            //System.out.println("PDS productDefinition == 7 not done");
                            throw new NotSupportedException("PDS productDefinition = 7 not implemented");

                            // Average, accumulation, and/or extreme values at a horizontal
                            // level or in a horizontal layer in a continuous or non-continuous
                            // time interval
                        }
                        else if (productDefinition == 8)
                        {
                            //System.out.println( "PDS productDefinition == 8 " );
                            //  35-41 bytes
                            int year = Bytes2Number.Int2(br);
                            int month = (br.ReadByte()) - 1;
                            if (month == 0)
                                month = 1;
                            int day = br.ReadByte();
                            int hour = br.ReadByte();
                            int minute = br.ReadByte();
                            int second = br.ReadByte();
                            //System.out.println( "PDS date:" + year +":" + month +
                            //":" + day + ":" + hour +":" + minute +":" + second );             
                            endTI = new DateTime(year, month, 1, hour, minute, second);
                            endTI = endTI.AddDays(day);
                            // 42
                            timeRanges = br.ReadByte();
                            //System.out.println( "PDS timeRanges=" + timeRanges ) ;
                            // 43 - 46
                            int missingDataValues = Bytes2Number.Int4(br);
                            //System.out.println( "PDS missingDataValues=" + missingDataValues ) ;

                            timeIncrement = new int[timeRanges * 6];
                            for (int t = 0; t < timeRanges; t += 6)
                            {
                                // 47 statProcess
                                timeIncrement[t] = br.ReadByte();
                                // 48  timeType
                                timeIncrement[t + 1] = br.ReadByte();
                                // 49   time Unit
                                timeIncrement[t + 2] = br.ReadByte();
                                // 50 - 53 lenTimeRange
                                timeIncrement[t + 3] = Bytes2Number.Int4(br);
                                // 54 indicatorTU
                                timeIncrement[t + 4] = br.ReadByte();
                                // 55 - 58 timeIncrement
                                timeIncrement[t + 5] = Bytes2Number.Int4(br);
                            }
                            // modify forecast time to reflect the end of the
                            // interval according to timeIncrement information.
                            // 1 accumulation
                            // 2 F.T. inc
                            // 1 Hour
                            // 3 number of hours to inc F.T.
                            // 255 missing
                            // 0 continuous processing
                            //if( timeRanges == 1 && timeIncrement[ 2 ] == 1) {
                            if (timeRanges == 1)
                            {
                                forecastTime += CalculateIncrement(timeIncrement[2], timeIncrement[3]);
                            }
                            else
                            { // throw flag
                                forecastTime = (int)UNDEF;
                            }
                            // Probability forecasts at a horizontal level or in a horizontal
                            // layer in a continuous or non-continuous time interval
                        }
                        else if (productDefinition == 9)
                        {
                            //  35-71 bytes
                            // 35
                            int probabilityNumber = br.ReadByte();
                            // 36
                            numberForecasts = br.ReadByte();
                            // 37
                            typeEnsemble = br.ReadByte();
                            //            if (typeGenProcess == 5) { // Probability var
                            //              typeGenProcess = 50000 + (1000 * typeEnsemble) + numberForecasts;
                            //            }
                            // 38
                            int scaleFactorLL = br.ReadByte();
                            // 39-42
                            int scaleValueLL = Bytes2Number.Int4(br);
                            lowerLimit = (float)(((scaleFactorLL == 0) || (scaleValueLL == 0))
                                ? scaleValueLL
                                : scaleValueLL * Math.Pow(10, -scaleFactorLL));

                            // 43
                            int scaleFactorUL = br.ReadByte();
                            // 44-47
                            int scaleValueUL = Bytes2Number.Int4(br);
                            upperLimit = (float)(((scaleFactorUL == 0) || (scaleValueUL == 0))
                                ? scaleValueUL
                                : scaleValueUL * Math.Pow(10, -scaleFactorUL));

                            // 48-49
                            int year = Bytes2Number.Int2(br);
                            // 50
                            int month = (br.ReadByte()) - 1;
                            // 51
                            int day = br.ReadByte();
                            // 52
                            int hour = br.ReadByte();
                            // 53
                            int minute = br.ReadByte();
                            // 54
                            int second = br.ReadByte();
                            //System.out.println( "PDS date:" + year +":" + month +
                            //":" + day + ":" + hour +":" + minute +":" + second );                                
                            endTI = new DateTime(year, month, day, hour, minute, second);
                            // 55
                            timeRanges = br.ReadByte();
                            //System.out.println( "PDS timeRanges=" + timeRanges ) ;
                            // 56-59
                            int missingDataValues = Bytes2Number.Int4(br);
                            //System.out.println( "PDS missingDataValues=" + missingDataValues ) ;

                            timeIncrement = new int[timeRanges * 6];
                            for (int t = 0; t < timeRanges; t += 6)
                            {
                                // 60 statProcess
                                timeIncrement[t] = br.ReadByte();
                                // 61 time type
                                timeIncrement[t + 1] = br.ReadByte();
                                // 62  time Unit
                                timeIncrement[t + 2] = br.ReadByte();
                                // 63 - 66  lenTimeRange
                                timeIncrement[t + 3] = Bytes2Number.Int4(br);
                                // 67  indicatorTU
                                timeIncrement[t + 4] = br.ReadByte();
                                // 68-71 time Inc
                                timeIncrement[t + 5] = Bytes2Number.Int4(br);
                            }
                            // modify forecast time to reflect the end of the
                            // interval according to timeIncrement information.
                            // 1 accumulation
                            // 2 F.T. inc
                            // 1 Hour
                            // 3 number of hours to inc F.T.
                            // 255 missing
                            // 0 continuous processing
                            if (timeRanges == 1)
                            {
                                forecastTime += CalculateIncrement(timeIncrement[2], timeIncrement[3]);
                            }
                            else
                            { // throw flag
                                forecastTime = (int)UNDEF;
                            }

                        }
                        break;
                    }
                    catch (NotSupportedException nse)
                    {
                        //nse.printStackTrace();
                    }

                    break;

                // Radar product
                case 20:
                    parameterCategory = br.ReadByte();
                    //System.out.println( "PDS parameterCategory=" +
                    //parameterCategory );

                    parameterNumber = br.ReadByte();
                    //System.out.println( "PDS parameterNumber=" + parameterNumber );

                    typeGenProcess = br.ReadByte();
                    //System.out.println( "PDS typeGenProcess=" + typeGenProcess );

                    break;

                // Satellite Product
                case 30:
                    parameterCategory = br.ReadByte();
                    //System.out.println( "PDS parameterCategory=" + parameterCategory );

                    parameterNumber = br.ReadByte();
                    //System.out.println( "PDS parameterNumber=" + parameterNumber );

                    typeGenProcess = br.ReadByte();
                    //System.out.println( "PDS typeGenProcess=" + typeGenProcess );

                    backGenProcess = br.ReadByte();
                    //System.out.println( "PDS backGenProcess=" + backGenProcess );

                    nb = br.ReadByte();
                    //System.out.println( "PDS nb =" + nb );
                    // nb sometime 0 based, other times 1 base
                    for (int j = 0; j < nb; j++)
                    {
                        br.BaseStream.Seek(10, SeekOrigin.Current);
                    }
                    break;

                // CCITTIA5 character string
                case 254:
                    parameterCategory = br.ReadByte();
                    //System.out.println( "PDS parameterCategory=" +
                    //parameterCategory );

                    parameterNumber = br.ReadByte();
                    //System.out.println( "PDS parameterNumber=" + parameterNumber );

                    //numberOfChars = Bytes2Number.int4( raf );
                    //System.out.println( "PDS numberOfChars=" +
                    //numberOfChars );
                    break;

                default:
                    break;

            }    // end switch

            br.BaseStream.Position = sectionEnd;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Get parameter
        /// </summary>
        /// <param name="aParTable">parameter table</param>
        /// <param name="disNum">discipline number</param>
        /// <returns>parameter</returns>
        public Variable GetParameter(GRIB2ParameterTable aParTable, int disNum)
        {
            Variable aPar = (Variable)aParTable.GetParameter(disNum, parameterCategory, parameterNumber).Clone();           
            return aPar;
        }

        /// <summary>
        /// Get product parameter
        /// </summary>
        /// <returns>parameter</returns>
        public Parameter GetProductParameter()
        {            
            //Get parameter
            switch (parameterCategory)
            {
                case 0:    //Temperature
                    return GetParameter_Category0(parameterNumber);   
                case 1:    //Moisture
                    return GetParameter_Category1(parameterNumber);
                default:
                    return null;
            }            
        }

        private Parameter GetParameter_Category0(int parameterNumber)
        {
            Parameter aPar = new Parameter();
            //Get parameter
            switch (parameterNumber)
            {
                case 0:
                    aPar.Name = "TMP";
                    aPar.Units = "K";
                    aPar.Description = "Temperature";
                    break;
                case 1:
                    aPar.Name = "VTMP";
                    aPar.Units = "K";
                    aPar.Description = "Virtual Temperature";
                    break;
                case 2:
                    aPar.Name = "POT";
                    aPar.Units = "K";
                    aPar.Description = "Potential Temperature";
                    break;
                case 3:
                    aPar.Name = "EPOT";
                    aPar.Units = "K";
                    aPar.Description = "Pseudo-Adiabatic Potential Temperature";
                    break;
                case 4:
                    aPar.Name = "TMAX";
                    aPar.Units = "K";
                    aPar.Description = "Maximum Temperature";
                    break;
                case 5:
                    aPar.Name = "TMIN";
                    aPar.Units = "K";
                    aPar.Description = "Minimum Temperature";
                    break;
                case 6:
                    aPar.Name = "DPT";
                    aPar.Units = "K";
                    aPar.Description = "Dew Point Temperature";
                    break;
                case 7:
                    aPar.Name = "DEPR";
                    aPar.Units = "K";
                    aPar.Description = "Dew Point Depression (or Deficit)";
                    break;
                case 8:
                    aPar.Name = "LAPR";
                    aPar.Units = "K m-1";
                    aPar.Description = "Lapse Rate";
                    break;
                case 9:
                    aPar.Name = "TMPA";
                    aPar.Units = "K";
                    aPar.Description = "Temperature Anomaly";
                    break; 
                case 10:
                    aPar.Name = "LHTFL";
                    aPar.Units = "W m-2";
                    aPar.Description = "Latent Heat Net Flux";
                    break;
                case 11:
                    aPar.Name = "SHTFL";
                    aPar.Units = "W m-2";
                    aPar.Description = "Sensible Heat Net Flux";
                    break;
                case 12:
                    aPar.Name = "HEATX";
                    aPar.Units = "K";
                    aPar.Description = "Heat Index";
                    break;
                case 13:
                    aPar.Name = "WCF";
                    aPar.Units = "K";
                    aPar.Description = "Wind Chill Factor";
                    break;
                case 14:
                    aPar.Name = "MINDPD";
                    aPar.Units = "K";
                    aPar.Description = "Minimum Dew Point Depression";
                    break;
                case 15:
                    aPar.Name = "VPTMP";
                    aPar.Units = "K";
                    aPar.Description = "Virtual Potential Temperature";
                    break;
                case 16:
                    aPar.Name = "SNOHF";
                    aPar.Units = "W m-2";
                    aPar.Description = "Snow Phase Change Heat Flux";
                    break; 
                case 17:
                    aPar.Name = "SKINT";
                    aPar.Units = "K";
                    aPar.Description = "Skin Temperature";
                    break;                 
            }

            return aPar;
        }

        private Parameter GetParameter_Category1(int parameterNumber)
        {
            Parameter aPar = new Parameter();
            //Get parameter
            switch (parameterNumber)
            {
                case 0:
                    aPar.Name = "SPFH";
                    aPar.Units = "kg kg-1";
                    aPar.Description = "Specific Humidity";
                    break;
                case 1:
                    aPar.Name = "RH";
                    aPar.Units = "%";
                    aPar.Description = "Relative Humidity";
                    break;
                case 2:
                    aPar.Name = "MIXR";
                    aPar.Units = "kg kg -1";
                    aPar.Description = "Humidity Mixing Ratio";
                    break;
                case 3:
                    aPar.Name = "PWAT";
                    aPar.Units = "kg m-2";
                    aPar.Description = "Precipitable Water";
                    break;
                case 4:
                    aPar.Name = "VAPP";
                    aPar.Units = "Pa";
                    aPar.Description = "Vapor Pressure";
                    break;
                case 5:
                    aPar.Name = "SATD";
                    aPar.Units = "Pa";
                    aPar.Description = "Saturation Deficit";
                    break;
                case 6:
                    aPar.Name = "EVP";
                    aPar.Units = "kg m-2";
                    aPar.Description = "Evaporation";
                    break;
                case 7:
                    aPar.Name = "PRATE";
                    aPar.Units = "kg m-2 s-1";
                    aPar.Description = "Precipitation Rate";
                    break;
                case 8:
                    aPar.Name = "APCP";
                    aPar.Units = "kg m-2";
                    aPar.Description = "Total Precipitation";
                    break;
                case 9:
                    aPar.Name = "NCPCP";
                    aPar.Units = "kg m-2";
                    aPar.Description = "Large-Scale Precipitation (non-convective)";
                    break;
                case 10:
                    aPar.Name = "ACPCP";
                    aPar.Units = "kg m-2";
                    aPar.Description = "Convective Precipitation";
                    break;
                case 11:
                    aPar.Name = "SNOD";
                    aPar.Units = "m";
                    aPar.Description = "Snow Depth";
                    break;
                case 12:
                    aPar.Name = "SRWEQ";
                    aPar.Units = "kg m-2 s-1";
                    aPar.Description = "Snowfall Rate Water Equivalent";
                    break;
                case 13:
                    aPar.Name = "WEASD";
                    aPar.Units = "kg m-2";
                    aPar.Description = "Water Equivalent of Accumulated Snow Depth";
                    break;
                case 14:
                    aPar.Name = "SNOC";
                    aPar.Units = "kg m-2";
                    aPar.Description = "Convect Snow";
                    break;
                case 15:
                    aPar.Name = "SNOL";
                    aPar.Units = "kg m-2";
                    aPar.Description = "Large-Scale Snow";
                    break;
                case 16:
                    aPar.Name = "SNOM";
                    aPar.Units = "kg m-2";
                    aPar.Description = "Snow Melt";
                    break;
                case 17:
                    aPar.Name = "SNOAG";
                    aPar.Units = "day";
                    aPar.Description = "Snow Age";
                    break;
                case 18:
                    aPar.Name = "ABSH";
                    aPar.Units = "kg m-3";
                    aPar.Description = "Absolute Humidity";
                    break;
                case 19:
                    aPar.Name = "PTYPE";
                    aPar.Units = "";
                    aPar.Description = "Precipitation Type";
                    break;
                case 20:
                    aPar.Name = "ILIQW";
                    aPar.Units = "kg m-2";
                    aPar.Description = "Integrated Liquid Water";
                    break;
                case 21:
                    aPar.Name = "TCOND";
                    aPar.Units = "kg kg-1";
                    aPar.Description = "Condensate";
                    break;
                case 22:
                    aPar.Name = "CLWMR";
                    aPar.Units = "kg kg-1";
                    aPar.Description = "Cloud Mixing Ratio";
                    break;
                case 23:
                    aPar.Name = "ICMR";
                    aPar.Units = "kg kg-1";
                    aPar.Description = "Ice Water Mixing Ratio";
                    break;
                case 24:
                    aPar.Name = "RWMR";
                    aPar.Units = "kg kg-1";
                    aPar.Description = "Rain Mixing Ratio";
                    break;
            }

            return aPar;
        }

        /// <summary>
        /// calculates the increment between time intervals 
        /// </summary>
        /// <param name="tui">tui time unit indicator</param>
        /// <param name="length">length of interval</param>
        /// <returns>increment</returns>
        private int CalculateIncrement(int tui, int length)
        {
            switch (tui)
            {

                case 1:
                    return length;
                case 10:
                    return 3 * length;
                case 11:
                    return 6 * length;
                case 12:
                    return 12 * length;
                case 0:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 13:
                    return length;
                default:
                    return (int)UNDEF;
            }

        }

        /// <summary>
        /// Get forecast time
        /// </summary>
        /// <param name="baseTime">base time</param>
        /// <returns>forecast time</returns>
        public DateTime GetForecastTime(DateTime baseTime)
        {
            DateTime fTime = baseTime;
            switch (timeRangeUnit)
            {
                case 0:    //Minute
                    fTime = baseTime.AddMinutes(forecastTime);
                    break;
                case 1:    //Hour
                case 10:
                case 11:
                case 12:
                    try
                    {
                        fTime = baseTime.AddHours(forecastTime);
                    }
                    catch
                    {
                        return fTime;
                    }
                    break;
                case 2:    //Day
                    fTime = baseTime.AddDays(forecastTime);
                    break;
                case 3:    //Month
                    fTime = baseTime.AddMonths(forecastTime);
                    break;
                case 4:    //Year
                case 5:
                case 6:
                case 7:
                    fTime = baseTime.AddYears(forecastTime);
                    break;
                case 13:
                    fTime = baseTime.AddSeconds(forecastTime);
                    break;
            }

            return fTime;
        }

        /// <summary>
        /// Get product definition name
        /// </summary>
        /// <param name="productDefinition">definition number</param>
        /// <returns>product definition name</returns>
        public static string GetProductDefinitionName(int productDefinition)
        {
            switch (productDefinition)
            {

                case 0:
                    return "Analysis/forecast at horizontal level/layer";

                case 1:
                    return "Individual ensemble forecast at a point in time";

                case 2:
                    return "Derived forecast on all ensemble members";

                case 3:
                    return "Derived forecasts on cluster of ensemble members over rectangular area";

                case 4:
                    return "Derived forecasts on cluster of ensemble members over circular area";

                case 5:
                    return "Probability forecasts at a horizontal level";

                case 6:
                    return "Percentile forecasts at a horizontal level";

                case 7:
                    return "Analysis or forecast error at a horizontal level";

                case 8:
                    return "Average, accumulation, extreme values or other statistically processed value at a horizontal level";

                case 11:
                    return "Individual ensemble forecast";

                case 20:
                    return "Radar product";

                case 30:
                    return "Satellite product";

                case 254:
                    return "CCITTIA5 character string";

                default:
                    return "Unknown";
            }
        }

        /// <summary>
        /// typeEnsemble number.
        /// </summary>
        /// <param name="tgp">tyeGenProcess</param>
        /// <returns>type ensemble</returns>        
        public static int GetTypeEnsemble(string tgp)
        {
            if (tgp.Contains("C_high"))
            {
                return 0;
            }
            else if (tgp.Contains("C_low"))
            {
                return 1;
            }
            else if (tgp.Contains("P_neg"))
            {
                return 2;
            }
            else if (tgp.Contains("P_pos"))
            {
                return 3;
            }
            return -9999; //didn't know what to put as illegal
        }

        /// <summary>
        /// type of Generating Process.
        /// </summary>
        /// <returns>type</returns>
        public String GetTypeGenProcess()
        {
            if (typeGenProcess == 4)
            {  //ensemble
                String type;
                if (typeEnsemble == 0)
                {
                    type = "C_high";
                }
                else if (typeEnsemble == 1)
                {
                    type = "C_low";
                }
                else if (typeEnsemble == 2)
                {
                    type = "P_neg";
                }
                else if (typeEnsemble == 3)
                {
                    type = "P_pos";
                }
                else
                {
                    type = "unknown";
                }

                return "4-" + type + "-" + perturbNumber.ToString();
            }
            return typeGenProcess.ToString();
        }

        /// <summary>
        /// typeGenProcess name. GRIB2 - TABLE 4.3  
        /// </summary>
        /// <param name="typeGenProcess">type of generating process</param>
        /// <returns>name</returns>
        public static String GetTypeGenProcessName(String typeGenProcess)
        {
            int tgp;
            if (typeGenProcess.StartsWith("4"))
            {
                tgp = 4;
            }
            else
            {
                tgp = int.Parse(typeGenProcess);
            }
            return GetTypeGenProcessName(tgp);
        }

        /// <summary>
        /// Get type of generating process name
        /// </summary>
        /// <param name="typeGenProcess">type of generating process</param>
        /// <returns>name</returns>
        public static String GetTypeGenProcessName(int typeGenProcess)
        {

            switch (typeGenProcess)
            {

                case 0:
                    return "Analysis";

                case 1:
                    return "Initialization";

                case 2:
                    return "Forecast";

                case 3:
                    return "Bias Corrected Forecast";

                case 4:
                    return "Ensemble Forecast";

                case 5:
                    return "Probability Forecast";

                case 6:
                    return "Forecast Error";

                case 7:
                    return "Analysis Error";

                case 8:
                    return "Observation";

                case 255:
                    return "Missing";

                default:
                    // ensemble will go from 4000 to 4399
                    if (typeGenProcess > 3999 && typeGenProcess < 4400)
                        return "Ensemble Forecast";

                    return "Unknown";
            }
        }

        /// <summary>
        /// return Time Range Unit Name from code table 4.4.
        /// </summary>
        /// <param name="timeRangeUnit">time range unit</param>
        /// <returns>time range unit name</returns>
         public static String GetTimeRangeUnitName(int timeRangeUnit)
        {
            switch (timeRangeUnit)
            {

                case 0:
                    return "minutes";

                case 1:
                    return "hours";

                case 2:
                    return "days";

                case 3:
                    return "months";

                case 4:
                    return "years";

                case 5:
                    return "decade";

                case 6:
                    return "normal";

                case 7:
                    return "century";

                case 10:
                    return "3hours";

                case 11:
                    return "6hours";

                case 12:
                    return "12hours";

                case 13:
                    return "secs";
            }
            return "unknown";
        }
  
        /// <summary>
         /// type of vertical coordinate: Name
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>name</returns>
        public static String GetTypeSurfaceName(int id)
        {

            switch (id)
            {

                case 0:
                    return "";

                case 1:
                    return "Ground or water surface";

                case 2:
                    return "Cloud base level";

                case 3:
                    return "Level of cloud tops";

                case 4:
                    return "Level of 0o C isotherm";

                case 5:
                    return "Level of adiabatic condensation lifted from the surface";

                case 6:
                    return "Maximum wind level";

                case 7:
                    return "Tropopause";

                case 8:
                    return "Nominal top of the atmosphere";

                case 9:
                    return "Sea bottom";

                case 20:
                    return "Isothermal level";

                case 100:
                    return "Isobaric surface";

                case 101:
                    return "Mean sea level";

                case 102:
                    return "Specific altitude above mean sea level";

                case 103:
                    return "Specified height level above ground";

                case 104:
                    return "Sigma level";

                case 105:
                    return "Hybrid level";

                case 106:
                    return "Depth below land surface";

                case 107:
                    return "Isentropic 'theta' level";

                case 108:
                    return "Level at specified pressure difference from ground to level";

                case 109:
                    return "Potential vorticity surface";

                case 111:
                    return "Eta level";

                case 117:
                    return "Mixed layer depth";

                case 160:
                    return "Depth below sea level";

                case 200:
                    return "Entire atmosphere layer";

                case 201:
                    return "Entire ocean layer";

                case 204:
                    return "Highest tropospheric freezing level";

                case 206:
                    return "Grid scale cloud bottom level";

                case 207:
                    return "Grid scale cloud top level";

                case 209:
                    return "Boundary layer cloud bottom level";

                case 210:
                    return "Boundary layer cloud top level";

                case 211:
                    return "Boundary layer cloud layer";

                case 212:
                    return "Low cloud bottom level";

                case 213:
                    return "Low cloud top level";

                case 214:
                    return "Low cloud layer";

                case 215:
                    return "Cloud ceiling";

                case 220:
                    return "Planetary Boundary Layer";

                case 221:
                    return "Layer Between Two Hybrid Levels";

                case 222:
                    return "Middle cloud bottom level";

                case 223:
                    return "Middle cloud top level";

                case 224:
                    return "Middle cloud layer";

                case 232:
                    return "High cloud bottom level";

                case 233:
                    return "High cloud top level";

                case 234:
                    return "High cloud layer";

                case 235:
                    return "Ocean isotherm level";

                case 236:
                    return "Layer between two depths below ocean surface";

                case 237:
                    return "Bottom of ocean mixed layer";

                case 238:
                    return "Bottom of ocean isothermal layer";

                case 239:
                    return "Layer Ocean Surface and 26C Ocean Isothermal Level";

                case 240:
                    return "Ocean Mixed Layer";

                case 241:
                    return "Ordered Sequence of Data";

                case 242:
                    return "Convective cloud bottom level";

                case 243:
                    return "Convective cloud top level";

                case 244:
                    return "Convective cloud layer";

                case 245:
                    return "Lowest level of the wet bulb zero";

                case 246:
                    return "Maximum equivalent potential temperature level";

                case 247:
                    return "Equilibrium level";

                case 248:
                    return "Shallow convective cloud bottom level";

                case 249:
                    return "Shallow convective cloud top level";

                case 251:
                    return "Deep convective cloud bottom level";

                case 252:
                    return "Deep convective cloud top level";

                case 253:
                    return "Lowest bottom level of supercooled liquid water layer";

                case 254:
                    return "Highest top level of supercooled liquid water layer";

                case 255:
                    return "Missing";

                default:
                    return "Unknown=" + id;
            }

        } 

        /// <summary>
        /// type of vertical coordinate: short Name
        /// </summary>
        /// <param name="id">Surface type id</param>
        /// <returns>short name</returns>
        public static String GetTypeSurfaceNameShort(int id)
        {

            switch (id)
            {

                case 0:
                    return "";

                case 1:
                    return "surface";

                case 2:
                    return "cloud_base";

                case 3:
                    return "cloud_tops";

                case 4:
                    return "zeroDegC_isotherm";

                case 5:
                    return "adiabatic_condensation_lifted";

                case 6:
                    return "maximum_wind";

                case 7:
                    return "tropopause";

                case 8:
                    return "atmosphere_top";

                case 9:
                    return "sea_bottom";

                case 20:
                    return "isotherm";

                case 100:
                    return "pressure";

                case 101:
                    return "msl";

                case 102:
                    return "altitude_above_msl";

                case 103:
                    return "height_above_ground";

                case 104:
                    return "sigma";

                case 105:
                    return "hybrid";

                case 106:
                    return "depth_below_surface";

                case 107:
                    return "isentrope";

                case 108:
                    return "pressure_difference";

                case 109:
                    return "potential_vorticity_surface";

                case 111:
                    return "eta";

                case 117:
                    return "mixed_layer_depth";

                case 160:
                    return "depth_below_sea";

                case 200:
                    return "entire_atmosphere";

                case 201:
                    return "entire_ocean";

                case 204:
                    return "highest_tropospheric_freezing";

                case 206:
                    return "grid_scale_cloud_bottom";

                case 207:
                    return "grid_scale_cloud_top";

                case 209:
                    return "boundary_layer_cloud_bottom";

                case 210:
                    return "boundary_layer_cloud_top";

                case 211:
                    return "boundary_layer_cloud";

                case 212:
                    return "low_cloud_bottom";

                case 213:
                    return "low_cloud_top";

                case 214:
                    return "low_cloud";

                case 215:
                    return "cloud_ceiling";

                case 220:
                    return "planetary_boundary";

                case 221:
                    return "between_two_hybrids";

                case 222:
                    return "middle_cloud_bottom";

                case 223:
                    return "middle_cloud_top";

                case 224:
                    return "middle_cloud";

                case 232:
                    return "high_cloud_bottom";

                case 233:
                    return "high_cloud_top";

                case 234:
                    return "high_cloud";

                case 235:
                    return "ocean_isotherm";

                case 236:
                    return "layer_between_two_depths_below_ocean";

                case 237:
                    return "bottom_of_ocean_mixed";

                case 238:
                    return "bottom_of_ocean_isothermal";

                case 239:
                    return "ocean_surface_and_26C_isothermal";

                case 240:
                    return "ocean_mixed";

                case 241:
                    return "ordered_sequence_of_data";

                case 242:
                    return "convective_cloud_bottom";

                case 243:
                    return "convective_cloud_top";

                case 244:
                    return "convective_cloud";

                case 245:
                    return "lowest_level_of_the_wet_bulb_zero";

                case 246:
                    return "maximum_equivalent_potential_temperature";

                case 247:
                    return "equilibrium";

                case 248:
                    return "shallow_convective_cloud_bottom";

                case 249:
                    return "shallow_convective_cloud_top";

                case 251:
                    return "deep_convective_cloud_bottom";

                case 252:
                    return "deep_convective_cloud_top";

                case 253:
                    return "lowest_level_water_layer";

                case 254:
                    return "highest_level_water_layer";

                case 255:
                    return "missing";

                default:
                    return "Unknown" + id;
            }

        } 

        /// <summary>
        /// type of vertical coordinate: Units.
        /// </summary>
        /// <param name="id">units id</param>
        /// <returns>units type</returns>
        public static String GetTypeSurfaceUnit(int id)
        {
            switch (id)
            {

                case 20:
                    return "K";

                case 100:
                    return "Pa";

                case 102:
                    return "m";

                case 103:
                    return "m";

                case 106:
                    return "m";

                case 107:
                    return "K";

                case 108:
                    return "Pa";

                case 109:
                    return "K m2 kg-1 s-1";

                case 117:
                    return "m";

                case 160:
                    return "m";

                case 235:
                    return "C 0.1";

                case 237:
                    return "m";

                case 238:
                    return "m";

                default:
                    return "";
            }
        }

        #endregion
    }
}
