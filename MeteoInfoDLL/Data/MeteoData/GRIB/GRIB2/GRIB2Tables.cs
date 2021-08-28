using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeteoInfoC.Data.MeteoData
{
    /// <summary>
    /// GRIB 2 tables
    /// </summary>
    public static class GRIB2Tables
    {
        /**
   * productDefinition  Name.
   * from code table 4.0.
   *
   * @param productDefinition productDefinition
   * @return ProductDefinitionName
   */
  static public String getProductDefinitionName(int productDefinition) {
    switch (productDefinition) {

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

      case 9:
        return
      "Probability forecasts at a horizontal level or in a horizontal layer in a continuous or non-continuous time interval";

      case 10:
        return
      "Percentile forecasts at a horizontal level or in a horizontal layer in a continuous or non-continuous time interval";

      case 11:
        return "Individual ensemble forecast";

      case 12:
        return "Derived forecast on all ensemble members in a continuous or non-continuous time interval";

      case 13:
        return "Derived forecasts on cluster of ensemble members over rectangular area"
           +" in a continuous or non-continuous time interval";
      case 14:
        return "Derived forecasts on cluster of ensemble members over circular area"
           +" in a continuous or non-continuous time interval";

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

  /**
   * typeGenProcess name.
   * GRIB2 - TABLE 4.3
   * TYPE OF GENERATING PROCESS
   * Section 4, Octet 12
   * Created 05/11/05
   *
   * @param typeGenProcess _more_
   * @return GenProcessName
   */
  public static String getTypeGenProcessName(String typeGenProcess) {
    int tgp;
    if (typeGenProcess.Substring(0, 1) == "4") {
      tgp = 4;
    } else {
      tgp = int.Parse(typeGenProcess);
    }
    return getTypeGenProcessName(tgp);
  }

  /// <summary>
  /// Get TypeGenProcessName
  /// </summary>
  /// <param name="typeGenProcess">typeGenProcess number</param>
  /// <returns>typeGenProcessName string</returns>
  public static String getTypeGenProcessName(int typeGenProcess) {

    switch (typeGenProcess) {

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

  /**
   * return Time Range Unit Name from code table 4.4.
   *
   * @param timeRangeUnit timeRangeUnit
   * @return TimeRangeUnitName
   */
  static public String getTimeRangeUnitName(int timeRangeUnit) {
    switch (timeRangeUnit) {

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
        //return "3hours";
        return "hours";

      case 11:
        //return "6hours";
        return "hours";

      case 12:
        //return "12hours";
        return "hours";

      case 13:
        return "seconds";

      default:
        //return "unknown";
        return "minutes"; // some grids don't set, so default is minutes, same default as old code
    }
  }

  /**
   * type of vertical coordinate: Name
   * code table 4.5.
   *
   * @param id surface type
   * @return SurfaceName
   */
  static public String getTypeSurfaceName(int id) {

    switch (id) {

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

  }  // end getTypeSurfaceName

  /**
   * type of vertical coordinate: short Name
   * derived from code table 4.5.
   *
   * @param id surfaceType
   * @return SurfaceNameShort
   */
  static public String getTypeSurfaceNameShort(int id) {

    switch (id) {

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

  }  // end getTypeSurfaceNameShort

  /**
   * type of vertical coordinate: Units.
   * code table 4.5.
   *
   * @param id units id as int
   * @return surfaceUnit
   */
  static public String getTypeSurfaceUnit(int id) {
    switch (id) {

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
  }  // end getTypeSurfaceUnit

  //**
  // * Makes a Ensemble, Derived, Probability or error Suffix
  // *
  // * @param ggr GribGridRecord
  // * @return suffix as String
  // */
  //public static String makeSuffix( GribGridRecord ggr ) {

  //  String interval = "";
  //  // check for accumulation variables
  //  if( ggr.productType > 8 && ggr.productType < 15 ) {
  //    int span = ggr.forecastTime - ggr.startOfInterval;
  //    interval = span.ToString() + getTimeRangeUnitName( ggr.timeUnit );
  //  }
    
  //  switch (ggr.productType) {
  //    case 0:
  //    case 7:
  //    case 40: {
  //      if (ggr.typeGenProcess == 6 || ggr.typeGenProcess == 7 ) {
  //        return "error";
  //      }
  //    }
  //    break;
  //    case 1:
  //    case 11:
  //    case 41:
  //    case 43: {
  //      // ensemble data
  //      /*
  //      if (typeGenProcess == 4) {
  //        if (type == 0) {
  //          return "Cntrl_high";
  //        } else if (type == 1) {
  //          return "Cntrl_low";
  //        } else if (type == 2) {
  //          return "Perturb_neg";
  //        } else if (type == 3) {
  //          return "Perturb_pos";
  //        } else {
  //          return "unknownEnsemble";
  //        }

  //      }
  //      */
  //      break;
  //    }

  //    case 2:
  //    case 3:
  //    case 4: {
  //      // Derived data
  //      if (ggr.typeGenProcess == 4) {
  //        if (ggr.type == 0) {
  //          return  "unweightedMean";
  //        } else if (ggr.type == 1) {
  //          return  "weightedMean";
  //        } else if (ggr.type == 2) {
  //          return  "stdDev";
  //        } else if (ggr.type == 3) {
  //          return  "stdDevNor";
  //        } else if (ggr.type == 4) {
  //          return  "spread";
  //        } else if (ggr.type == 5) {
  //          return  "anomaly";
  //        } else if (ggr.type == 6) {
  //          return  "unweightedMeanCluster";
  //        } else {
  //          return  "unknownEnsemble";
  //        }
  //      }
  //      break;
  //    }

  //    case 12:
  //    case 13:
  //    case 14: {
  //      // Derived data
  //      if (ggr.typeGenProcess == 4) {
  //        interval = interval +"_";
  //        if (ggr.type == 0) {
  //          return interval +"unweightedMean";
  //        } else if (ggr.type == 1) {
  //          return interval +"weightedMean";
  //        } else if (ggr.type == 2) {
  //          return interval +"stdDev";
  //        } else if (ggr.type == 3) {
  //          return interval +"stdDevNor";
  //        } else if (ggr.type == 4) {
  //          return interval +"spread";
  //        } else if (ggr.type == 5) {
  //          return interval +"anomaly";
  //        } else if (ggr.type == 6) {
  //          return interval +"unweightedMeanCluster";
  //        } else {
  //          return interval +"unknownEnsemble";
  //        }
  //      }
  //      break;
  //    }

  //    //case 5: {
  //    //  // probability data
  //    //  if (ggr.typeGenProcess == 5) {
  //    //    return getProbabilityVariableNameSuffix( ggr.lowerLimit, ggr.upperLimit, ggr.type );
  //    //  }
  //    //}
  //    //break;
  //    //case 9: {
  //    //  // probability data
  //    //  if (ggr.typeGenProcess == 5) {
  //    //    return interval +"_"+ getProbabilityVariableNameSuffix( ggr.lowerLimit, ggr.upperLimit, ggr.type );
  //    //  }
  //    //}
  //    //break;

  //    default:
  //      return interval;
  //  }
  //  return interval;
  //}

  //static String getProbabilityVariableNameSuffix( float lowerLimit, float upperLimit, int type )
  //{
  //  String ll = Float.toString( lowerLimit ).replace( '.', 'p' ).replaceFirst( "p0$", "" );
  //  String ul = Float.toString( upperLimit ).replace( '.', 'p' ).replaceFirst( "p0$", "" );
  //  if ( type == 0 )
  //  {
  //    //return "below_" + Float.toString(lowerLimit).replace('.', 'p');
  //    return "probability_below_" + ll;
  //  }
  //  else if ( type == 1 )
  //  {
  //    //return "above_" + Float.toString(upperLimit).replace('.', 'p');
  //    return "probability_above_" + ul;
  //  }
  //  else if ( type == 2 )
  //  {
  //    //return "between_" + Float.toString(lowerLimit).replace('.', 'p') + "_" +
  //    //    Float.toString(upperLimit).replace('.', 'p');
  //    return "probability_between_" + ll + "_" + ul;
  //  }
  //  else if ( type == 3 )
  //  {
  //    //return "above_" + Float.toString(lowerLimit).replace('.', 'p');
  //    return "probability_above_" + ll;
  //  }
  //  else if ( type == 4 )
  //  {
  //    //return "below_" + Float.toString(upperLimit).replace('.', 'p');
  //    return "probability_below_" + ul;
  //  }
  //  else
  //  {
  //    return "unknownProbability";
  //  }

  //}

  /**
   * Makes a Ensemble, Derived, Probability or error type
   *
   * @param productType,    productType
   * @param type            of ensemble, derived, probability
   * @return Ensemble type as String
   */
  public static String getEnsembleType( int productType, int type) {
    switch (productType) {

      case 1:
      case 11:
      case 41:
      case 43: {
        // ensemble data

        //if (typeGenProcess == 4) {
          if (type == 0) {
            return "Cntrl_high";
          } else if (type == 1) {
            return "Cntrl_low";
          } else if (type == 2) {
            return "Perturb_neg";
          } else if (type == 3) {
            return "Perturb_pos";
          } else {
            return "unknownEnsemble";
          }

        //}

        //break;
      }
      case 2:
      case 3:
      case 4:
      case 12:
      case 13:
      case 14: {
        // Derived data
        //if (typeGenProcess == 4) {
          if (type == 0) {
            return "unweightedMean";
          } else if (type == 1) {
            return "weightedMean";
          } else if (type == 2) {
            return "stdDev";
          } else if (type == 3) {
            return "stdDevNor";
          } else if (type == 4) {
            return "spread";
          } else if (type == 5) {
            return "anomaly";
          } else if (type == 6) {
            return "unweightedMeanCluster";
          } else {
            return "unknownEnsemble";
          }
        //}
        //break;
      }

      default:
        return "";
    }
    //return "";
  }

  // GDS static Tables

  /**
   * enum for componet_flag  for both Grib2 and Grib1
   *
   */
   public enum VectorComponentFlag
   { 
       /// <summary>
       /// Easterly northerly relative
       /// </summary>
       easterlyNortherlyRelative, 
       /// <summary>
       /// Grid relative
       /// </summary>
       gridRelative  
   };


  /**
   * .
   *
   * @param gdtn Grid definition template number same as type of grid
   * @return GridName as a String
   */
  public static String getGridName(int gdtn) {
    switch (gdtn) {  // code table 3.2

      case 0:
        return "Latitude/Longitude";

      case 1:
        return "Rotated Latitude/Longitude";

      case 2:
        return "Stretched Latitude/Longitude";

      case 3:
        return "iStretched and Rotated Latitude/Longitude";

      case 10:
        return "Mercator";

      case 20:
        return "Polar stereographic";

      case 30:
        return "Lambert Conformal";

      case 31:
        return "Albers Equal Area";

      case 40:
        return "Gaussian latitude/longitude";

      case 41:
        return "Rotated Gaussian Latitude/longitude";

      case 42:
        return "Stretched Gaussian Latitude/longitude";

      case 43:
        return "Stretched and Rotated Gaussian Latitude/longitude";

      case 50:
        return "Spherical harmonic coefficients";

      case 51:
        return "Rotated Spherical harmonic coefficients";

      case 52:
        return "Stretched Spherical harmonic coefficients";

      case 53:
        return "Stretched and Rotated Spherical harmonic coefficients";

      case 90:
        return "Space View Perspective or Orthographic";

      case 100:
        return "Triangular Grid Based on an Icosahedron";

      case 110:
        return "Equatorial Azimuthal Equidistant";

      case 120:
        return "Azimuth-Range";

      case 204:
        return "Curvilinear Orthogonal Grid";

      default:
        return "Unknown projection" + gdtn;
    }
  }                    // end getGridName

  //// code table 3.1
  //*
  // * gets the ProjectionType.
  // *
  // * @param gridType GridDefRecord
  // * @return ProjectionType
  // */
  //public static int getProjectionType(int gridType) {
  //  switch (gridType) {
  //    case 1:
  //      return GridTableLookup.RotatedLatLon;

  //    case 10:
  //      return GridTableLookup.Mercator;

  //    case 20:
  //      return GridTableLookup.PolarStereographic;

  //    case 30:
  //      return GridTableLookup.LambertConformal;

  //    case 31:
  //      return GridTableLookup.AlbersEqualArea;

  //    case 40:
  //      return GridTableLookup.GaussianLatLon;

  //    case 90:
  //      return GridTableLookup.Orthographic;

  //    case 204:
  //      return GridTableLookup.Curvilinear;

  //    default:
  //      return -1;
  //  }
  //}
  /**
   * .
   *
   * @param shape as an int
   * @return shapeName as a String
   */
  static public String getShapeName(int shape) {
    switch (shape) {  // code table 3.2

      case 0:
        return "Earth spherical with radius = 6367470 m";

      case 1:
        return "Earth spherical with radius specified by producer";

      case 2:
        return "Earth oblate spheroid with major axis = 6378160.0 m and minor axis = 6356775.0 m";

      case 3:
        return "Earth oblate spheroid with axes specified by producer";

      case 4:
        return "Earth oblate spheroid with major axis = 6378137.0 m and minor axis = 6356752.314 m";

      case 5:
        return "Earth represent by WGS84";

      case 6:
        return "Earth spherical with radius of 6371229.0 m";

      default:
        return "Unknown Earth Shape";
    }
  }
    }
}
