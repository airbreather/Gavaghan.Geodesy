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
    /// This is the outcome of a geodetic calculation.  It represents the path and
    /// ellipsoidal distance between two GlobalCoordinates for a specified reference
    /// ellipsoid.
    /// </summary>
    [Serializable]
    public struct GeodeticCurve : IEquatable<GeodeticCurve>, ISerializable
    {
        // intentionally NOT readonly, for performance reasons.
        /// <summary>Ellipsoidal distance (in meters).</summary>
        private double ellipsoidalDistanceMeters;

        // intentionally NOT readonly, for performance reasons.
        /// <summary>Azimuth (degrees from north).</summary>
        private Angle azimuth;

        // intentionally NOT readonly, for performance reasons.
        /// <summary>Reverse azimuth (degrees from north).</summary>
        private Angle reverseAzimuth;

        /// <summary>
        /// Create a new GeodeticCurve.
        /// </summary>
        /// <param name="ellipsoidalDistanceMeters">ellipsoidal distance in meters</param>
        /// <param name="azimuth">azimuth in degrees</param>
        /// <param name="reverseAzimuth">reverse azimuth in degrees</param>
        public GeodeticCurve(double ellipsoidalDistanceMeters, Angle azimuth, Angle reverseAzimuth)
        {
            this.ellipsoidalDistanceMeters = ellipsoidalDistanceMeters;
            this.azimuth = azimuth;
            this.reverseAzimuth = reverseAzimuth;
        }

        /// <summary>Ellipsoidal distance (in meters).</summary>
        public double EllipsoidalDistanceMeters
        {
            get { return this.ellipsoidalDistanceMeters; }
        }

        /// <summary>
        /// Get the azimuth.  This is angle from north from start to end.
        /// </summary>
        public Angle Azimuth
        {
            get { return this.azimuth; }
        }

        /// <summary>
        /// Get the reverse azimuth.  This is angle from north from end to start.
        /// </summary>
        public Angle ReverseAzimuth
        {
            get { return this.reverseAzimuth; }
        }

        public override int GetHashCode()
        {
            // TODO: consider just leaving it at ellipsoidal distance...
            int hashCode = 17;

            hashCode = hashCode * 31 + this.ellipsoidalDistanceMeters.GetHashCode();
            hashCode = hashCode * 31 + this.azimuth.GetHashCode();
            hashCode = hashCode * 31 + this.reverseAzimuth.GetHashCode();

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is GeodeticCurve &&
                   this.Equals((GeodeticCurve)obj);
        }

        public bool Equals(GeodeticCurve other)
        {
            return this.ellipsoidalDistanceMeters == other.ellipsoidalDistanceMeters &&
                   this.azimuth == other.azimuth &&
                   this.reverseAzimuth == other.reverseAzimuth;
        }

        /// <summary>
        /// Get curve as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture,
                                 "GeodeticCurve[EllipsoidalDistanceMeters={0}, Azimuth={1}, ReverseAzimuth={2}]",
                                 this.ellipsoidalDistanceMeters,
                                 this.azimuth,
                                 this.reverseAzimuth);
        }

        #region Serialization / Deserialization

        private GeodeticCurve(SerializationInfo info, StreamingContext context)
        {
            this.ellipsoidalDistanceMeters = info.GetDouble("ellipsoidalDistanceMeters");

            double azimuthRadians = info.GetDouble("azimuthRadians");
            double reverseAzimuthRadians = info.GetDouble("reverseAzimuthRadians");

            this.azimuth = Angle.FromRadians(azimuthRadians);
            this.reverseAzimuth = Angle.FromRadians(reverseAzimuthRadians);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ellipsoidalDistanceMeters", this.ellipsoidalDistanceMeters);

            info.AddValue("azimuthRadians", this.azimuth.Radians);
            info.AddValue("reverseAzimuthRadians", this.reverseAzimuth.Radians);
        }

        #endregion

        #region Operators

        public static bool operator ==(GeodeticCurve lhs, GeodeticCurve rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(GeodeticCurve lhs, GeodeticCurve rhs)
        {
            return !lhs.Equals(rhs);
        }

        #endregion
    }
}
