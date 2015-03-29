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
  public class GlobalCoordinatesTest
  {
    [Test]
    public void CornerCaseForLatitude()
    {
      Angle latitude = new Angle(100);
      Angle longitude = new Angle(1000);
      GlobalCoordinates coords = new GlobalCoordinates(latitude, longitude);
      coords.Longitude = 0.0;
      coords.Latitude = -360.0;
      Assert.IsTrue(Math.Abs(coords.Latitude.Degrees - 0) < 1e-6);
      Assert.IsTrue(Math.Abs(coords.Longitude.Degrees - 0) < 1e-6);

      coords = new GlobalCoordinates(latitude, longitude);
      coords.Longitude = 180.0;
      coords.Latitude = -360.0;
      Assert.IsTrue(Math.Abs(coords.Latitude.Degrees - 0) < 1e-6);
      Assert.IsTrue(Math.Abs(coords.Longitude.Degrees - 180) < 1e-6);

      coords.Longitude = 0.0;
      coords.Latitude = 100.0;
      coords.Latitude = -360.0;
      Assert.IsTrue(Math.Abs(coords.Latitude.Degrees - 0) < 1e-6);
      Assert.IsTrue(Math.Abs(coords.Longitude.Degrees - 180) < 1e-6);
    }

    [Test]
    public void CanonicalizeLatitude()
    {
      Angle latitude = new Angle(30);
      Angle longitude = new Angle(20);
      GlobalCoordinates coords = new GlobalCoordinates(latitude, longitude);
      Assert.AreEqual(coords.Latitude, latitude);
      Assert.AreEqual(coords.Longitude, longitude);

      latitude = new Angle(100);
      coords = new GlobalCoordinates(latitude, longitude);
      Assert.AreEqual(coords.Latitude, new Angle(80));
      Assert.AreEqual(coords.Longitude, new Angle(-160));

      latitude = new Angle(-100);
      coords = new GlobalCoordinates(latitude, longitude);
      Assert.AreEqual(coords.Latitude, new Angle(-80));
      Assert.AreEqual(coords.Longitude, new Angle(-160));

      latitude = new Angle(200);
      coords = new GlobalCoordinates(latitude, longitude);
      Assert.AreEqual(coords.Latitude, new Angle(-20));
      Assert.AreEqual(coords.Longitude, new Angle(-160));

      latitude = new Angle(280);
      coords = new GlobalCoordinates(latitude, longitude);
      Assert.AreEqual(coords.Latitude, new Angle(-80));
      Assert.AreEqual(coords.Longitude, longitude);

      latitude = new Angle(-200);
      coords = new GlobalCoordinates(latitude, longitude);
      Assert.AreEqual(coords.Latitude, new Angle(20));
      Assert.AreEqual(coords.Longitude, new Angle(-160));

      latitude = new Angle(100);
      longitude = new Angle(1000);
      coords = new GlobalCoordinates(latitude, longitude);
      coords.Longitude = 0.0;
      coords.Latitude = -359.0;
      Assert.IsTrue(Math.Abs(coords.Latitude.Degrees - 1.0) < 1e-6);
    }

    [Test]
    public void CanonicalizeLongitude()
    {
      Angle latitude = new Angle(30);
      Angle longitude = new Angle(20);
      GlobalCoordinates coords = new GlobalCoordinates(latitude, longitude);
      Assert.AreEqual(coords.Latitude, latitude);
      Assert.AreEqual(coords.Longitude, longitude);

      longitude = new Angle(160);
      coords = new GlobalCoordinates(latitude, longitude);
      Assert.AreEqual(coords.Latitude, latitude);
      Assert.AreEqual(coords.Longitude, longitude);

      longitude = new Angle(200);
      coords = new GlobalCoordinates(latitude, longitude);
      Assert.AreEqual(coords.Latitude, latitude);
      Assert.AreEqual(coords.Longitude, new Angle(-160));

      longitude = new Angle(-200);
      coords = new GlobalCoordinates(latitude, longitude);
      Assert.AreEqual(coords.Latitude, latitude);
      Assert.AreEqual(coords.Longitude, new Angle(160));
    }
  }
}
