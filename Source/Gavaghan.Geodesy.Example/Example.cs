/* Gavaghan.Geodesy by Mike Gavaghan
 * 
 * http://www.gavaghan.org/blog/free-source-code/geodesy-library-vincentys-formula/
 * 
 * This code may be freely used and modified on any personal or professional
 * project.  It comes with no warranty.
 *
 * BitCoin tips graciously accepted at 1FB63FYQMy7hpC2ANVhZ5mSgAZEtY1aVLf
 */
using System;
using System.Text;
using Gavaghan.Geodesy;

namespace Gavaghan.Geodesy.Example
{
  public class Example
  {
    /// <summary>
    /// Calculate the destination if we start at:
    ///    Lincoln Memorial in Washington, D.C --> 38.8892N, 77.04978W
    ///         and travel at
    ///    51.7679 degrees for 6179.016136 kilometers
    /// 
    ///    WGS84 reference ellipsoid
    /// </summary>
    static void TwoDimensionalDirectCalculation()
    {
      // instantiate the calculator
      GeodeticCalculator geoCalc = new GeodeticCalculator();

      // select a reference elllipsoid
      Ellipsoid reference = Ellipsoid.WGS84;

      // set Lincoln Memorial coordinates
      GlobalCoordinates lincolnMemorial;
      lincolnMemorial = new GlobalCoordinates(
          new Angle(38.88922), new Angle(-77.04978)
      );

      // set the direction and distance
      Angle startBearing = new Angle(51.7679);
      double distance = 6179016.13586;

      // find the destination
      Angle endBearing;
      GlobalCoordinates dest = geoCalc.CalculateEndingGlobalCoordinates(reference, lincolnMemorial, startBearing, distance, out endBearing);

      Console.WriteLine("Travel from Lincoln Memorial at 51.767921 deg for 6179.016 km");
      Console.Write("   Destination: {0:0.0000}{1}", dest.Latitude.Degrees, (dest.Latitude > 0) ? "N" : "S" );
      Console.WriteLine(", {0:0.0000}{1}", dest.Longitude.Degrees, (dest.Longitude > 0) ? "E" : "W");
      Console.WriteLine("   End Bearing: {0:0.00} degrees", endBearing.Degrees);
    }

    /// <summary>
    /// Calculate the two-dimensional path from
    ///    Lincoln Memorial in Washington, D.C --> 38.8892N, 77.04978W
    ///         to
    ///    Eiffel Tower in Paris --> 48.85889N, 2.29583E
    ///         using
    ///    WGS84 reference ellipsoid
    /// </summary>
    static void TwoDimensionalInverseCalculation()
    {
      // instantiate the calculator
      GeodeticCalculator geoCalc = new GeodeticCalculator();

      // select a reference elllipsoid
      Ellipsoid reference = Ellipsoid.WGS84;

      // set Lincoln Memorial coordinates
      GlobalCoordinates lincolnMemorial;
      lincolnMemorial = new GlobalCoordinates(
          new Angle(38.88922), new Angle(-77.04978)
      );

      // set Eiffel Tower coordinates
      GlobalCoordinates eiffelTower;
      eiffelTower = new GlobalCoordinates(
          new Angle(48.85889), new Angle(2.29583)
      );

      // calculate the geodetic curve
      GeodeticCurve geoCurve = geoCalc.CalculateGeodeticCurve(reference, lincolnMemorial, eiffelTower);
      double ellipseKilometers = geoCurve.EllipsoidalDistance / 1000.0;
      double ellipseMiles = ellipseKilometers * 0.621371192;

      Console.WriteLine("2-D path from Lincoln Memorial to Eiffel Tower using WGS84");
      Console.WriteLine("   Ellipsoidal Distance: {0:0.00} kilometers ({1:0.00} miles)", ellipseKilometers, ellipseMiles);
      Console.WriteLine("   Azimuth:              {0:0.00} degrees", geoCurve.Azimuth.Degrees);
      Console.WriteLine("   Reverse Azimuth:      {0:0.00} degrees", geoCurve.ReverseAzimuth.Degrees);
    }

    /// <summary>
    /// Calculate the three-dimensional path from
    ///    Pike's Peak in Colorado --> 38.840511N, 105.0445896W, 4301 meters
    ///        to
    ///    Alcatraz Island --> 37.826389N, 122.4225W, sea level
    ///        using
    ///    WGS84 reference ellipsoid
    /// </summary>
    static void ThreeDimensionalInverseCalculation()
    {
      // instantiate the calculator
      GeodeticCalculator geoCalc = new GeodeticCalculator();

      // select a reference elllipsoid
      Ellipsoid reference = Ellipsoid.WGS84;

      // set Pike's Peak position
      GlobalPosition pikesPeak;
      pikesPeak = new GlobalPosition(
        new GlobalCoordinates(new Angle(38.840511), new Angle(-105.0445896)),
        4301.0
      );

      // set Alcatraz Island coordinates
      GlobalPosition alcatrazIsland;
      alcatrazIsland = new GlobalPosition(
        new GlobalCoordinates(new Angle(37.826389), new Angle(-122.4225)),
        0.0
      );

      // calculate the geodetic measurement
      GeodeticMeasurement geoMeasurement;
      double p2pKilometers;
      double p2pMiles;
      double elevChangeMeters;
      double elevChangeFeet;

      geoMeasurement = geoCalc.CalculateGeodeticMeasurement(reference, pikesPeak, alcatrazIsland);
      p2pKilometers = geoMeasurement.PointToPointDistance / 1000.0;
      p2pMiles = p2pKilometers * 0.621371192;
      elevChangeMeters = geoMeasurement.ElevationChange;
      elevChangeFeet = elevChangeMeters * 3.2808399;

      Console.WriteLine("3-D path from Pike's Peak to Alcatraz Island using WGS84");
      Console.WriteLine("   Point-to-Point Distance: {0:0.00} kilometers ({1:0.00} miles)", p2pKilometers, p2pMiles);
      Console.WriteLine("   Elevation change:        {0:0.0} meters ({1:0.0} feet)", elevChangeMeters, elevChangeFeet);
      Console.WriteLine("   Azimuth:                 {0:0.00} degrees", geoMeasurement.Azimuth.Degrees);
      Console.WriteLine("   Reverse Azimuth:         {0:0.00} degrees", geoMeasurement.ReverseAzimuth.Degrees);
    }

    static void Main()
    {
      TwoDimensionalDirectCalculation();

      Console.WriteLine();

      TwoDimensionalInverseCalculation();

      Console.WriteLine();

      ThreeDimensionalInverseCalculation();

      Console.ReadLine();
    }
  }
}
