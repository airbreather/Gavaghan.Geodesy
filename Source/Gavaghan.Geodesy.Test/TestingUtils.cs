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
using System.Collections.Generic;
using System.Text;

namespace Gavaghan.Geodesy.Test
{
  public static class TestingUtils
  {
    /// <summary>
    /// Compare two floating point values for equality within a tolerance.
    /// </summary>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public static bool AreEqualWithinTolerance( double value1, double value2 )
    {
      double difference = Math.Abs(value1 - value2);
      double error = difference / value1;

      return error < 0.000001;
    }
  }
}
