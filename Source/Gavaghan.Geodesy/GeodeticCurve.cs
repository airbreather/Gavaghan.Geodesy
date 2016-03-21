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
    /// This is the outcome of a geodetic calculation.  It represents the path and
    /// ellipsoidal distance between two GlobalCoordinates for a specified reference
    /// ellipsoid.
    /// </summary>
    [Serializable]
    public struct GeodeticCurve : IEquatable<GeodeticCurve>, ISerializable
    {
        /// <summary>
        /// Create a new GeodeticCurve.
        /// </summary>
        /// <param name="ellipsoidalDistanceMeters">ellipsoidal distance in meters</param>
        /// <param name="azimuth">azimuth in degrees</param>
        /// <param name="reverseAzimuth">reverse azimuth in degrees</param>
        public GeodeticCurve(double ellipsoidalDistanceMeters, Angle azimuth, Angle reverseAzimuth)
        {
            this.EllipsoidalDistanceMeters = ellipsoidalDistanceMeters;
            this.Azimuth = azimuth;
            this.ReverseAzimuth = reverseAzimuth;
        }

        /// <summary>Ellipsoidal distance (in meters).</summary>
        public double EllipsoidalDistanceMeters { get; }

        /// <summary>
        /// Get the azimuth.  This is angle from north from start to end.
        /// </summary>
        public Angle Azimuth { get; }

        /// <summary>
        /// Get the reverse azimuth.  This is angle from north from end to start.
        /// </summary>
        public Angle ReverseAzimuth { get; }

        // TODO: consider just leaving it at ellipsoidal distance...
        public static int GetHashCode(GeodeticCurve value) => HashCodeBuilder.Seed
                                                                             .HashWith(value.EllipsoidalDistanceMeters)
                                                                             .HashWith(value.Azimuth)
                                                                             .HashWith(value.ReverseAzimuth);

        public static bool Equals(GeodeticCurve first, GeodeticCurve second) => first.EllipsoidalDistanceMeters == second.EllipsoidalDistanceMeters &&
                                                                                first.Azimuth == second.Azimuth &&
                                                                                first.ReverseAzimuth == second.ReverseAzimuth;

        public static string ToString(GeodeticCurve value) => $"GeodeticCurve[EllipsoidalDistanceMeters={value.EllipsoidalDistanceMeters}, Azimuth={value.Azimuth}, ReverseAzimuth={value.ReverseAzimuth}]";

        public override int GetHashCode() => GetHashCode(this);
        public override bool Equals(object obj) => obj is GeodeticCurve && Equals(this, (GeodeticCurve)obj);
        public bool Equals(GeodeticCurve other) => Equals(this, other);

        /// <summary>
        /// Get curve as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ToString(this);

        #region Serialization / Deserialization

        private GeodeticCurve(SerializationInfo info, StreamingContext context)
        {
            this.EllipsoidalDistanceMeters = info.GetDouble("ellipsoidalDistanceMeters");

            double azimuthRadians = info.GetDouble("azimuthRadians");
            double reverseAzimuthRadians = info.GetDouble("reverseAzimuthRadians");

            this.Azimuth = Angle.FromRadians(azimuthRadians);
            this.ReverseAzimuth = Angle.FromRadians(reverseAzimuthRadians);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ellipsoidalDistanceMeters", this.EllipsoidalDistanceMeters);

            info.AddValue("azimuthRadians", this.Azimuth.Radians);
            info.AddValue("reverseAzimuthRadians", this.ReverseAzimuth.Radians);
        }

        #endregion

        #region Operators

        public static bool operator ==(GeodeticCurve lhs, GeodeticCurve rhs) => Equals(lhs, rhs);
        public static bool operator !=(GeodeticCurve lhs, GeodeticCurve rhs) => !Equals(lhs, rhs);

        #endregion
    }
}
