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
        /// <summary>
        /// Creates a new instance of GeodeticMeasurement.
        /// </summary>
        /// <param name="averageCurve">the geodetic curve as measured at the average elevation between two points</param>
        /// <param name="elevationChangeMeters">the change in elevation, in meters, going from the starting point to the ending point</param>
        public GeodeticMeasurement(GeodeticCurve averageCurve, double elevationChangeMeters)
        {
            double ellipsoidalDistanceMeters = averageCurve.EllipsoidalDistanceMeters;

            this.AverageCurve = averageCurve;
            this.ElevationChangeMeters = elevationChangeMeters;
            this.PointToPointDistanceMeters = Math.Sqrt((ellipsoidalDistanceMeters * ellipsoidalDistanceMeters) + (elevationChangeMeters * elevationChangeMeters));
        }

        /// <summary>
        /// Get the average geodetic curve.  This is the geodetic curve as measured
        /// at the average elevation between two points.
        /// </summary>
        public GeodeticCurve AverageCurve { get; }

        /// <summary>
        /// Get the ellipsoidal distance (in meters).  This is the length of the average geodetic
        /// curve.  For actual point-to-point distance, use PointToPointDistance property.
        /// </summary>
        public double EllipsoidalDistanceMeters => this.AverageCurve.EllipsoidalDistanceMeters;

        /// <summary>
        /// Get the azimuth.  This is angle from north from start to end.
        /// </summary>
        public Angle Azimuth => this.AverageCurve.Azimuth;

        /// <summary>
        /// Get the reverse azimuth.  This is angle from north from end to start.
        /// </summary>
        public Angle ReverseAzimuth => this.AverageCurve.ReverseAzimuth;

        /// <summary>
        /// Get the elevation change, in meters, going from the starting to the ending point.
        /// </summary>
        public double ElevationChangeMeters { get; }

        /// <summary>
        /// Get the distance travelled, in meters, going from one point to the next.
        /// </summary>
        public double PointToPointDistanceMeters { get; }

        // p2p is a derived metric, no need to test.
        public static int GetHashCode(GeodeticMeasurement value) => HashCodeBuilder.Seed
                                                                                   .HashWith(value.AverageCurve)
                                                                                   .HashWith(value.ElevationChangeMeters);

        public static bool Equals(GeodeticMeasurement first, GeodeticMeasurement second) => first.AverageCurve == second.AverageCurve &&
                                                                                            first.ElevationChangeMeters == second.ElevationChangeMeters;

        public static string ToString(GeodeticMeasurement value) => $"GeodeticMeasurement[AverageCurve={value.AverageCurve}, ElevationChangeMeters={value.ElevationChangeMeters}, PointToPointDistanceMeters={value.PointToPointDistanceMeters}]";

        public override int GetHashCode() => GetHashCode(this);
        public override bool Equals(object obj) => obj is GeodeticMeasurement && Equals(this, (GeodeticMeasurement)obj);
        public bool Equals(GeodeticMeasurement other) => Equals(this, other);

        /// <summary>
        /// Get the GeodeticMeasurement as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ToString(this);

        #region Serialization / Deserialization

        private GeodeticMeasurement(SerializationInfo info, StreamingContext context)
        {
            double elevationChangeMeters = this.ElevationChangeMeters = info.GetDouble("elevationChangeMeters");

            double ellipsoidalDistanceMeters = info.GetDouble("averageCurveEllipsoidalDistanceMeters");
            double azimuthRadians = info.GetDouble("averageCurveAzimuthRadians");
            double reverseAzimuthRadians = info.GetDouble("averageCurveReverseAzimuthRadians");

            this.AverageCurve = new GeodeticCurve(ellipsoidalDistanceMeters, Angle.FromRadians(azimuthRadians), Angle.FromRadians(reverseAzimuthRadians));
            this.PointToPointDistanceMeters = Math.Sqrt((ellipsoidalDistanceMeters * ellipsoidalDistanceMeters) + (elevationChangeMeters * elevationChangeMeters));
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("elevationChangeMeters", this.ElevationChangeMeters);

            info.AddValue("averageCurveEllipsoidalDistanceMeters", this.AverageCurve.EllipsoidalDistanceMeters);
            info.AddValue("averageCurveAzimuthRadians", this.AverageCurve.Azimuth.Radians);
            info.AddValue("averageCurveReverseAzimuthRadians", this.AverageCurve.ReverseAzimuth.Radians);
        }

        #endregion

        #region Operators

        public static bool operator ==(GeodeticMeasurement lhs, GeodeticMeasurement rhs) => Equals(lhs, rhs);
        public static bool operator !=(GeodeticMeasurement lhs, GeodeticMeasurement rhs) => !Equals(lhs, rhs);

        #endregion
    }
}
