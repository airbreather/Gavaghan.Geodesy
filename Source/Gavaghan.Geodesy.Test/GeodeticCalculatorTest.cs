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
using NUnit.Framework;
using Gavaghan.Geodesy;

namespace Gavaghan.Geodesy.Test
{
  [TestFixture]
  public class GeodeticCalculatorTest
  {
    [Test]
    public void TestCalculateGeodeticCurve()
    {
      // instantiate the calculator
      GeodeticCalculator geoCalc = new GeodeticCalculator();

      // select a reference elllipsoid
      Ellipsoid reference = Ellipsoid.WGS84;

      // set Lincoln Memorial coordinates
      GlobalCoordinates lincolnMemorial;
      lincolnMemorial = new GlobalCoordinates(38.88922, -77.04978);

      // set Eiffel Tower coordinates
      GlobalCoordinates eiffelTower;
      eiffelTower = new GlobalCoordinates(48.85889, 2.29583);

      // calculate the geodetic curve
      GeodeticCurve geoCurve = geoCalc.CalculateGeodeticCurve(reference, lincolnMemorial, eiffelTower);

      Assert.AreEqual(6179016.136, geoCurve.EllipsoidalDistance, 0.001);
      Assert.AreEqual(51.76792142, geoCurve.Azimuth.Degrees, 0.0000001);
      Assert.AreEqual(291.75529334, geoCurve.ReverseAzimuth.Degrees, 0.0000001);
    }

    [Test]
    public void TestCalculateGeodeticMeasurement()
    {
      // instantiate the calculator
      GeodeticCalculator geoCalc = new GeodeticCalculator();

      // select a reference elllipsoid
      Ellipsoid reference = Ellipsoid.WGS84;

      // set Pike's Peak position
      GlobalPosition pikesPeak;
      pikesPeak = new GlobalPosition(new GlobalCoordinates(new Angle(38.840511), new Angle(-105.0445896)), 4301.0);

      // set Alcatraz Island coordinates
      GlobalPosition alcatrazIsland;
      alcatrazIsland = new GlobalPosition(new GlobalCoordinates(new Angle(37.826389), new Angle(-122.4225)), 0.0);

      // calculate the geodetic measurement
      GeodeticMeasurement geoMeasurement;

      geoMeasurement = geoCalc.CalculateGeodeticMeasurement(reference, pikesPeak, alcatrazIsland);

      Assert.AreEqual(-4301.0, geoMeasurement.ElevationChange, 0.001);
      Assert.AreEqual(1521788.826, geoMeasurement.PointToPointDistance, 0.001);
      Assert.AreEqual(1521782.748, geoMeasurement.EllipsoidalDistance, 0.001);
      Assert.AreEqual(271.21039153, geoMeasurement.Azimuth.Degrees, 0.0000001);
      Assert.AreEqual(80.38029386, geoMeasurement.ReverseAzimuth.Degrees, 0.0000001);
    }

    [Test]
    public void TestAntiPodal1()
    {
      // instantiate the calculator
      GeodeticCalculator geoCalc = new GeodeticCalculator();

      // select a reference elllipsoid
      Ellipsoid reference = Ellipsoid.WGS84;

      // set position 1
      GlobalCoordinates p1;
      p1 = new GlobalCoordinates(10, 80.6);

      // set position 2
      GlobalCoordinates p2;
      p2 = new GlobalCoordinates(-10, -100);

      // calculate the geodetic measurement
      GeodeticCurve geoCurve;

      geoCurve = geoCalc.CalculateGeodeticCurve(reference, p1, p2);

      Assert.AreEqual(19970718.422432076, geoCurve.EllipsoidalDistance, 0.001);
      Assert.AreEqual(90.0004877491174, geoCurve.Azimuth.Degrees, 0.0000001);
      Assert.AreEqual(270.0004877491174, geoCurve.ReverseAzimuth.Degrees, 0.0000001);
    }

    [Test]
    public void TestAntiPodal2()
    {
      // instantiate the calculator
      GeodeticCalculator geoCalc = new GeodeticCalculator();

      // select a reference elllipsoid
      Ellipsoid reference = Ellipsoid.WGS84;

      // set position 1
      GlobalCoordinates p1;
      p1 = new GlobalCoordinates(11, 80);

      // set position 2
      GlobalCoordinates p2;
      p2 = new GlobalCoordinates(-10, -100);

      // calculate the geodetic measurement
      GeodeticCurve geoCurve;

      geoCurve = geoCalc.CalculateGeodeticCurve(reference, p1, p2);

      Assert.AreEqual(19893320.272061437, geoCurve.EllipsoidalDistance, 0.001);
      Assert.AreEqual(360.0, geoCurve.Azimuth.Degrees, 0.0000001);
      Assert.AreEqual(0.0, geoCurve.ReverseAzimuth.Degrees, 0.0000001);
    }

    [Test]
    public void TestInverseWithDirect()
    {
      // instantiate the calculator
      GeodeticCalculator geoCalc = new GeodeticCalculator();

      // select a reference elllipsoid
      Ellipsoid reference = Ellipsoid.WGS84;

      // set Lincoln Memorial coordinates
      GlobalCoordinates lincolnMemorial;
      lincolnMemorial = new GlobalCoordinates( new Angle(38.88922), new Angle(-77.04978));

      // set Eiffel Tower coordinates
      GlobalCoordinates eiffelTower;
      eiffelTower = new GlobalCoordinates(48.85889, 2.29583);

      // calculate the geodetic curve
      GeodeticCurve geoCurve = geoCalc.CalculateGeodeticCurve(reference, lincolnMemorial, eiffelTower);

      // now, plug the result into to direct solution
      GlobalCoordinates dest;
      Angle endBearing = new Angle();

      dest = geoCalc.CalculateEndingGlobalCoordinates(reference, lincolnMemorial, geoCurve.Azimuth, geoCurve.EllipsoidalDistance, out endBearing);

      Assert.AreEqual(eiffelTower.Latitude.Degrees, dest.Latitude.Degrees, 0.0000001);
      Assert.AreEqual(eiffelTower.Longitude.Degrees, dest.Longitude.Degrees, 0.0000001);
    }

    [Test]
    public void TestPoleCrossing()
    {
      // instantiate the calculator
      GeodeticCalculator geoCalc = new GeodeticCalculator();

      // select a reference elllipsoid
      Ellipsoid reference = Ellipsoid.WGS84;

      // set Lincoln Memorial coordinates
      GlobalCoordinates lincolnMemorial;
      lincolnMemorial = new GlobalCoordinates(new Angle(38.88922), new Angle(-77.04978));

      // set a bearing of 1.0deg (almost straight up) and a distance
      Angle startBearing = new Angle(1.0);
      double distance = 6179016.13586;

      // set the expected destination
      GlobalCoordinates expected;
      expected = new GlobalCoordinates(new Angle(85.60006433), new Angle(92.17243943));

      // calculate the ending global coordinates
      Angle endBearing;
      GlobalCoordinates dest = geoCalc.CalculateEndingGlobalCoordinates(reference, lincolnMemorial, startBearing, distance, out endBearing);

      Assert.AreEqual(expected.Latitude.Degrees, dest.Latitude.Degrees, 0.0000001);
      Assert.AreEqual(expected.Longitude.Degrees, dest.Longitude.Degrees, 0.0000001);
    }
  }
}
