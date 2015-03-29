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

namespace Gavaghan.Geodesy.Test
{
    [TestFixture]
    public class GeodeticCalculatorTest
    {
        private const double StandardTolerance = 1e-7;

        [Test]
        public void TestCalculateGeodeticCurve()
        {
            // instantiate the calculator
            GeodeticCalculator geoCalc = new GeodeticCalculator();

            // select a reference elllipsoid
            Ellipsoid reference = Ellipsoid.WGS84;

            // set Lincoln Memorial coordinates
            GlobalCoordinates lincolnMemorial = new GlobalCoordinates(Angle.FromDegrees(38.88922), Angle.FromDegrees(-77.04978));

            // set Eiffel Tower coordinates
            GlobalCoordinates eiffelTower = new GlobalCoordinates(Angle.FromDegrees(48.85889), Angle.FromDegrees(2.29583));

            // calculate the geodetic curve
            GeodeticCurve geoCurve = geoCalc.CalculateGeodeticCurve(reference, lincolnMemorial, eiffelTower);

            Assert.AreEqual(6179016.136, geoCurve.EllipsoidalDistanceMeters, 0.001);
            Assert.AreEqual(51.76792142, geoCurve.Azimuth.Degrees, StandardTolerance);
            Assert.AreEqual(291.75529334, geoCurve.ReverseAzimuth.Degrees, StandardTolerance);
        }

        [Test]
        public void TestCalculateGeodeticMeasurement()
        {
            // instantiate the calculator
            GeodeticCalculator geoCalc = new GeodeticCalculator();

            // select a reference elllipsoid
            Ellipsoid reference = Ellipsoid.WGS84;

            // set Pike's Peak position
            GlobalPosition pikesPeak = new GlobalPosition(new GlobalCoordinates(Angle.FromDegrees(38.840511), Angle.FromDegrees(-105.0445896)), 4301);

            // set Alcatraz Island coordinates
            GlobalPosition alcatrazIsland = new GlobalPosition(new GlobalCoordinates(Angle.FromDegrees(37.826389), Angle.FromDegrees(-122.4225)), 0);

            // calculate the geodetic measurement
            GeodeticMeasurement geoMeasurement = geoCalc.CalculateGeodeticMeasurement(reference, pikesPeak, alcatrazIsland);

            Assert.AreEqual(-4301, geoMeasurement.ElevationChangeMeters, 0.001);
            Assert.AreEqual(1521788.826, geoMeasurement.PointToPointDistanceMeters, 0.001);
            Assert.AreEqual(1521782.748, geoMeasurement.EllipsoidalDistanceMeters, 0.001);
            Assert.AreEqual(271.21039153, geoMeasurement.Azimuth.Degrees, StandardTolerance);
            Assert.AreEqual(80.38029386, geoMeasurement.ReverseAzimuth.Degrees, StandardTolerance);
        }

        [Test]
        public void TestAntiPodal1()
        {
            // instantiate the calculator
            GeodeticCalculator geoCalc = new GeodeticCalculator();

            // select a reference elllipsoid
            Ellipsoid reference = Ellipsoid.WGS84;

            // set position 1
            GlobalCoordinates p1 = new GlobalCoordinates(Angle.FromDegrees(10), Angle.FromDegrees(80.6));

            // set position 2
            GlobalCoordinates p2 = new GlobalCoordinates(Angle.FromDegrees(-10), Angle.FromDegrees(-100));

            // calculate the geodetic measurement
            GeodeticCurve geoCurve = geoCalc.CalculateGeodeticCurve(reference, p1, p2);

            Assert.AreEqual(19970718.422432076, geoCurve.EllipsoidalDistanceMeters, 0.001);
            Assert.AreEqual(90.0004877491174, geoCurve.Azimuth.Degrees, StandardTolerance);
            Assert.AreEqual(270.0004877491174, geoCurve.ReverseAzimuth.Degrees, StandardTolerance);
        }

        [Test]
        public void TestAntiPodal2()
        {
            // instantiate the calculator
            GeodeticCalculator geoCalc = new GeodeticCalculator();

            // select a reference elllipsoid
            Ellipsoid reference = Ellipsoid.WGS84;

            // set position 1
            GlobalCoordinates p1 = new GlobalCoordinates(Angle.FromDegrees(11), Angle.FromDegrees(80));

            // set position 2
            GlobalCoordinates p2 = new GlobalCoordinates(Angle.FromDegrees(-10), Angle.FromDegrees(-100));

            // calculate the geodetic measurement
            GeodeticCurve geoCurve = geoCalc.CalculateGeodeticCurve(reference, p1, p2);

            Assert.AreEqual(19893320.272061437, geoCurve.EllipsoidalDistanceMeters, 0.001);
            Assert.AreEqual(360, geoCurve.Azimuth.Degrees, StandardTolerance);
            Assert.AreEqual(0, geoCurve.ReverseAzimuth.Degrees, StandardTolerance);
        }

        [Test]
        public void TestInverseWithDirect()
        {
            // instantiate the calculator
            GeodeticCalculator geoCalc = new GeodeticCalculator();

            // select a reference elllipsoid
            Ellipsoid reference = Ellipsoid.WGS84;

            // set Lincoln Memorial coordinates
            GlobalCoordinates lincolnMemorial = new GlobalCoordinates(Angle.FromDegrees(38.88922), Angle.FromDegrees(-77.04978));

            // set Eiffel Tower coordinates
            GlobalCoordinates eiffelTower = new GlobalCoordinates(Angle.FromDegrees(48.85889), Angle.FromDegrees(2.29583));

            // calculate the geodetic curve
            GeodeticCurve geoCurve = geoCalc.CalculateGeodeticCurve(reference, lincolnMemorial, eiffelTower);

            // now, plug the result into to direct solution
            Angle endBearing;
            GlobalCoordinates dest = geoCalc.CalculateEndingGlobalCoordinates(reference, lincolnMemorial, geoCurve.Azimuth, geoCurve.EllipsoidalDistanceMeters, out endBearing);

            Assert.AreEqual(eiffelTower.Latitude.Degrees, dest.Latitude.Degrees, StandardTolerance);
            Assert.AreEqual(eiffelTower.Longitude.Degrees, dest.Longitude.Degrees, StandardTolerance);
        }

        [Test]
        public void TestPoleCrossing()
        {
            // instantiate the calculator
            GeodeticCalculator geoCalc = new GeodeticCalculator();

            // select a reference elllipsoid
            Ellipsoid reference = Ellipsoid.WGS84;

            // set Lincoln Memorial coordinates
            GlobalCoordinates lincolnMemorial = new GlobalCoordinates(Angle.FromDegrees(38.88922), Angle.FromDegrees(-77.04978));

            // set a bearing of 1.0deg (almost straight up) and a distance
            Angle startBearing = Angle.FromDegrees(1);
            double distance = 6179016.13586;

            // set the expected destination
            GlobalCoordinates expected = new GlobalCoordinates(Angle.FromDegrees(85.60006433), Angle.FromDegrees(92.17243943));

            // calculate the ending global coordinates
            Angle endBearing;
            GlobalCoordinates dest = geoCalc.CalculateEndingGlobalCoordinates(reference, lincolnMemorial, startBearing, distance, out endBearing);

            Assert.AreEqual(expected.Latitude.Degrees, dest.Latitude.Degrees, StandardTolerance);
            Assert.AreEqual(expected.Longitude.Degrees, dest.Longitude.Degrees, StandardTolerance);
        }
    }
}
