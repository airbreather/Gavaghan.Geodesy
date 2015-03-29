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
    public class AngleTest
    {
        private const double StandardTolerance = 1e-6;

        [Test]
        public void ConvertToRadians()
        {
            Angle angle = Angle.FromDegrees(10);
            TestingUtils.AssertEqualityWithinExtremeTolerance(10, angle.Degrees);
            Assert.AreEqual(0.1745329251994, angle.Radians, StandardTolerance);
        }

        [Test]
        public void ConvertDegreesMinutes()
        {
            Angle angle = Angle.FromDegreesAndMinutes(10, 30);
            TestingUtils.AssertEqualityWithinExtremeTolerance(10.5, angle.Degrees);

            angle = Angle.FromDegreesAndMinutes(-10, 30);
            TestingUtils.AssertEqualityWithinExtremeTolerance(-10.5, angle.Degrees);
        }

        [Test]
        public void ConvertDegreesMinutesSeconds()
        {
            Angle angle = Angle.FromDegreesMinutesAndSeconds(10, 30, 20);
            Assert.AreEqual(10.505555556, angle.Degrees, StandardTolerance);

            angle = Angle.FromDegreesMinutesAndSeconds(-10, 30, 20);
            Assert.AreEqual(-10.505555556, angle.Degrees, StandardTolerance);
        }

        [Test]
        public void Comparisons()
        {
            Angle angle1 = Angle.FromDegrees(10);
            Angle angle2 = Angle.FromDegrees(20);
            Angle angle3 = Angle.FromDegrees(20);

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
            Angle angle1 = Angle.FromDegrees(10);
            Angle angle2 = Angle.FromDegrees(12);
            Angle result;

            result = angle1 + angle2;
            TestingUtils.AssertEqualityWithinExtremeTolerance(22, result.Degrees);

            result = angle1 - angle2;
            TestingUtils.AssertEqualityWithinExtremeTolerance(-2, result.Degrees);
        }
    }
}
