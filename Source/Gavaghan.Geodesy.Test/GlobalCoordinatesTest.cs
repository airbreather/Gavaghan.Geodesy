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
    public class GlobalCoordinatesTest
    {
        [Test]
        public void CanonicalizeLatitude()
        {
            Angle latitude = Angle.FromDegrees(30);
            Angle longitude = Angle.FromDegrees(20);
            GlobalCoordinates coords = new GlobalCoordinates(latitude, longitude);
            TestingUtils.AssertEqualityWithinExtremeTolerance(latitude.Radians, coords.Latitude.Radians);
            TestingUtils.AssertEqualityWithinExtremeTolerance(longitude.Radians, coords.Longitude.Radians);

            latitude = Angle.FromDegrees(100);
            coords = new GlobalCoordinates(latitude, longitude);
            TestingUtils.AssertEqualityWithinExtremeTolerance(80, coords.Latitude.Degrees);
            TestingUtils.AssertEqualityWithinExtremeTolerance(-160, coords.Longitude.Degrees);

            latitude = Angle.FromDegrees(-100);
            coords = new GlobalCoordinates(latitude, longitude);
            TestingUtils.AssertEqualityWithinExtremeTolerance(-80, coords.Latitude.Degrees);
            TestingUtils.AssertEqualityWithinExtremeTolerance(-160, coords.Longitude.Degrees);

            latitude = Angle.FromDegrees(200);
            coords = new GlobalCoordinates(latitude, longitude);
            TestingUtils.AssertEqualityWithinExtremeTolerance(-20, coords.Latitude.Degrees);
            TestingUtils.AssertEqualityWithinExtremeTolerance(-160, coords.Longitude.Degrees);

            latitude = Angle.FromDegrees(280);
            coords = new GlobalCoordinates(latitude, longitude);
            TestingUtils.AssertEqualityWithinExtremeTolerance(-80, coords.Latitude.Degrees);
            TestingUtils.AssertEqualityWithinExtremeTolerance(longitude.Radians, coords.Longitude.Radians);

            latitude = Angle.FromDegrees(-200);
            coords = new GlobalCoordinates(latitude, longitude);
            TestingUtils.AssertEqualityWithinExtremeTolerance(20, coords.Latitude.Degrees);
            TestingUtils.AssertEqualityWithinExtremeTolerance(-160, coords.Longitude.Degrees);
        }

        [Test]
        public void CanonicalizeLongitude()
        {
            Angle latitude = Angle.FromDegrees(30);
            Angle longitude = Angle.FromDegrees(20);
            GlobalCoordinates coords = new GlobalCoordinates(latitude, longitude);
            TestingUtils.AssertEqualityWithinExtremeTolerance(latitude.Degrees, coords.Latitude.Degrees);
            TestingUtils.AssertEqualityWithinExtremeTolerance(longitude.Degrees, coords.Longitude.Degrees);
            TestingUtils.AssertEqualityWithinExtremeTolerance(latitude.Radians, coords.Latitude.Radians);
            TestingUtils.AssertEqualityWithinExtremeTolerance(longitude.Radians, coords.Longitude.Radians);

            longitude = Angle.FromDegrees(160);
            coords = new GlobalCoordinates(latitude, longitude);
            TestingUtils.AssertEqualityWithinExtremeTolerance(latitude.Degrees, coords.Latitude.Degrees);
            TestingUtils.AssertEqualityWithinExtremeTolerance(longitude.Degrees, coords.Longitude.Degrees);
            TestingUtils.AssertEqualityWithinExtremeTolerance(latitude.Radians, coords.Latitude.Radians);
            TestingUtils.AssertEqualityWithinExtremeTolerance(longitude.Radians, coords.Longitude.Radians);

            longitude = Angle.FromDegrees(200);
            coords = new GlobalCoordinates(latitude, longitude);
            TestingUtils.AssertEqualityWithinExtremeTolerance(latitude.Degrees, coords.Latitude.Degrees);
            TestingUtils.AssertEqualityWithinExtremeTolerance(-160, coords.Longitude.Degrees);
            TestingUtils.AssertEqualityWithinExtremeTolerance(latitude.Radians, coords.Latitude.Radians);
            TestingUtils.AssertEqualityWithinExtremeTolerance(Angle.FromDegrees(-160).Radians, coords.Longitude.Radians);

            longitude = Angle.FromDegrees(-200);
            coords = new GlobalCoordinates(latitude, longitude);
            TestingUtils.AssertEqualityWithinExtremeTolerance(latitude.Degrees, coords.Latitude.Degrees);
            TestingUtils.AssertEqualityWithinExtremeTolerance(160, coords.Longitude.Degrees);
            TestingUtils.AssertEqualityWithinExtremeTolerance(latitude.Radians, coords.Latitude.Radians);
            TestingUtils.AssertEqualityWithinExtremeTolerance(Angle.FromDegrees(160).Radians, coords.Longitude.Radians);
        }
    }
}
