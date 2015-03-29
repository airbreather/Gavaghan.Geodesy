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
  public class AngleTest
  {
    [Test]
    public void ConvertToRadians()
    {
      Angle angle = new Angle(10);
      Assert.IsTrue(TestingUtils.AreEqualWithinTolerance(angle.Degrees, 10));
      Assert.IsTrue(TestingUtils.AreEqualWithinTolerance(angle.Radians, 0.1745329251994));
    }

    [Test]
    public void ConvertDegreesMinutes()
    {
      Angle angle = new Angle(10, 30);
      Assert.IsTrue(TestingUtils.AreEqualWithinTolerance(angle.Degrees, 10.5));

      angle = new Angle(-10, 30);
      Assert.IsTrue(TestingUtils.AreEqualWithinTolerance(angle.Degrees, -10.5));
    }

    [Test]
    public void ConvertDegreesMinutesSeconds()
    {
      Angle angle = new Angle(10, 30, 20);
      Assert.IsTrue(TestingUtils.AreEqualWithinTolerance(angle.Degrees, 10.505555556));

      angle = new Angle(-10, 30, 20);
      Assert.IsTrue(TestingUtils.AreEqualWithinTolerance(angle.Degrees, 10.505555556));
    }

    [Test]
    public void Comparisons()
    {
      Angle angle1 = new Angle(10);
      Angle angle2 = new Angle(20);
      Angle angle3 = new Angle(20);

      Assert.IsTrue(angle1.CompareTo(angle2) < 0);
      Assert.IsTrue(angle2.CompareTo(angle1) > 0);
      Assert.IsTrue(angle2.CompareTo(angle3) == 0);

      Assert.IsTrue(angle1 < angle2);
      Assert.IsTrue(angle1 <= angle2);
      Assert.IsTrue(angle1 != angle2);

      Assert.IsTrue(angle2 > angle1);
      Assert.IsTrue(angle2 >= angle1);
      Assert.IsTrue(angle2 != angle1);

      Assert.IsTrue(angle2 <= angle3);
      Assert.IsTrue(angle2 >= angle3);
      Assert.IsTrue(angle2 == angle3);
    }

    [Test]
    public void Arithmetic()
    {
      Angle angle1 = new Angle(10);
      Angle angle2 = new Angle(12);
      Angle result;

      result = angle1 + angle2;
      Assert.IsTrue(TestingUtils.AreEqualWithinTolerance(result.Degrees, new Angle(22).Degrees));

      result = angle1 - angle2;
      Assert.IsTrue(TestingUtils.AreEqualWithinTolerance(result.Degrees, new Angle(-2).Degrees));
    }
  }
}
