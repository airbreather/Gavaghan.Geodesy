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
    public static class TestingUtils
    {
        /// <summary>
        /// Compare two floating point values for equality within a tolerance.
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <returns></returns>
        public static void AssertEqualityWithinExtremeTolerance(double expected, double actual)
        {
            if (Double.IsNaN(expected) || Double.IsNaN(actual))
            {
                Assert.Fail("NaN values are never equal to other values.  Expected: {0}, Actual: {1}", expected, actual);
            }

            long value1Bits = BitConverter.DoubleToInt64Bits(expected);
            long value2Bits = BitConverter.DoubleToInt64Bits(actual);

            // Math.Abs(value1Bits - value2Bits) could easily overflow where it needn't.
            long steps = value1Bits < value2Bits
                ? value2Bits - value1Bits
                : value1Bits - value2Bits;

            // TODO: 6 is higher than I expected to need.  I expected 2ish.  Ideally, it would be 0,
            // but that may be impossible without requiring additional storage space.
            const long MaxAbsoluteDifference = 6;
            Assert.LessOrEqual(steps, MaxAbsoluteDifference, "Expected: {0}, Actual: {1}", expected, actual);
        }
    }
}
