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
using System.Globalization;
using System.Runtime.Serialization;

namespace Gavaghan.Geodesy
{
    /// <summary>
    /// This is the outcome of a three dimensional geodetic calculation.  It represents
    /// the path a between two GlobalPositions for a specified reference ellipsoid.
    /// </summary>
    [Serializable]
    public struct GeodeticMeasurement : IEquatable<GeodeticMeasurement>, ISerializable
    {
        // intentionally NOT readonly, for performance reasons.
        /// <summary>The average geodetic curve.</summary>
        private GeodeticCurve averageCurve;

        // intentionally NOT readonly, for performance reasons.
        /// <summary>The elevation change, in meters, going from the starting to the ending point.</summary>
        private double elevationChangeMeters;

        // intentionally NOT readonly, for performance reasons.
        /// <summary>The distance travelled, in meters, going from one point to the next.</summary>
        private double p2pMeters;

        /// <summary>
        /// Creates a new instance of GeodeticMeasurement.
        /// </summary>
        /// <param name="averageCurve">the geodetic curve as measured at the average elevation between two points</param>
        /// <param name="elevationChangeMeters">the change in elevation, in meters, going from the starting point to the ending point</param>
        public GeodeticMeasurement(GeodeticCurve averageCurve, double elevationChangeMeters)
        {
            double ellipsoidalDistanceMeters = averageCurve.EllipsoidalDistanceMeters;

            this.averageCurve = averageCurve;
            this.elevationChangeMeters = elevationChangeMeters;
            this.p2pMeters = Math.Sqrt((ellipsoidalDistanceMeters * ellipsoidalDistanceMeters) + (elevationChangeMeters * elevationChangeMeters));
        }

        /// <summary>
        /// Get the average geodetic curve.  This is the geodetic curve as measured
        /// at the average elevation between two points.
        /// </summary>
        public GeodeticCurve AverageCurve
        {
            get { return averageCurve; }
        }

        /// <summary>
        /// Get the ellipsoidal distance (in meters).  This is the length of the average geodetic
        /// curve.  For actual point-to-point distance, use PointToPointDistance property.
        /// </summary>
        public double EllipsoidalDistanceMeters
        {
            get { return averageCurve.EllipsoidalDistanceMeters; }
        }

        /// <summary>
        /// Get the azimuth.  This is angle from north from start to end.
        /// </summary>
        public Angle Azimuth
        {
            get { return averageCurve.Azimuth; }
        }

        /// <summary>
        /// Get the reverse azimuth.  This is angle from north from end to start.
        /// </summary>
        public Angle ReverseAzimuth
        {
            get { return averageCurve.ReverseAzimuth; }
        }

        /// <summary>
        /// Get the elevation change, in meters, going from the starting to the ending point.
        /// </summary>
        public double ElevationChangeMeters
        {
            get { return elevationChangeMeters; }
        }

        /// <summary>
        /// Get the distance travelled, in meters, going from one point to the next.
        /// </summary>
        public double PointToPointDistanceMeters
        {
            get { return p2pMeters; }
        }

        public override int GetHashCode()
        {
            // p2p is a derived metric, no need to test.
            int hashCode = 17;

            hashCode = hashCode * 31 + this.averageCurve.GetHashCode();
            hashCode = hashCode * 31 + this.elevationChangeMeters.GetHashCode();

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is GeodeticMeasurement &&
                   this.Equals((GeodeticMeasurement)obj);
        }

        public bool Equals(GeodeticMeasurement other)
        {
            // p2p is a derived metric, no need to test.
            return this.averageCurve == other.averageCurve &&
                   this.elevationChangeMeters == other.elevationChangeMeters;
        }

        /// <summary>
        /// Get the GeodeticMeasurement as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture,
                                 "GeodeticMeasurement[AverageCurve={0}, ElevationChangeMeters={1}, PointToPointDistanceMeters={2}]",
                                 this.averageCurve,
                                 this.elevationChangeMeters,
                                 this.p2pMeters);
        }

        #region Serialization / Deserialization

        private GeodeticMeasurement(SerializationInfo info, StreamingContext context)
        {
            this.elevationChangeMeters = info.GetDouble("elevationChangeMeters");

            double ellipsoidalDistanceMeters = info.GetDouble("averageCurveEllipsoidalDistanceMeters");
            double azimuthRadians = info.GetDouble("averageCurveAzimuthRadians");
            double reverseAzimuthRadians = info.GetDouble("averageCurveReverseAzimuthRadians");

            this.averageCurve = new GeodeticCurve(ellipsoidalDistanceMeters, Angle.FromRadians(azimuthRadians), Angle.FromRadians(reverseAzimuthRadians));
            this.p2pMeters = Math.Sqrt((ellipsoidalDistanceMeters * ellipsoidalDistanceMeters) + (elevationChangeMeters * elevationChangeMeters));
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("elevationChangeMeters", this.elevationChangeMeters);

            info.AddValue("averageCurveEllipsoidalDistanceMeters", this.averageCurve.EllipsoidalDistanceMeters);
            info.AddValue("averageCurveAzimuthRadians", this.averageCurve.Azimuth.Radians);
            info.AddValue("averageCurveReverseAzimuthRadians", this.averageCurve.ReverseAzimuth.Radians);
        }

        #endregion

        #region Operators

        public static bool operator ==(GeodeticMeasurement lhs, GeodeticMeasurement rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(GeodeticMeasurement lhs, GeodeticMeasurement rhs)
        {
            return !lhs.Equals(rhs);
        }

        #endregion
    }
}
