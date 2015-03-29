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
    /// Encapsulates a three dimensional location on a globe (GlobalCoordinates combined with
    /// an elevation in meters above a reference ellipsoid).
    /// </summary>
    [Serializable]
    public struct GlobalPosition : IComparable<GlobalPosition>, IComparable, IEquatable<GlobalPosition>, ISerializable
    {
        // intentionally NOT readonly, for performance reasons.
        /// <summary>Global coordinates.</summary>
        private GlobalCoordinates coordinates;

        // intentionally NOT readonly, for performance reasons.
        /// <summary>Elevation, in meters, above the surface of the ellipsoid.</summary>
        private double elevationMeters;

        /// <summary>
        /// Creates a new instance of GlobalPosition for a position on the surface of
        /// the reference ellipsoid.
        /// </summary>
        /// <param name="coords"></param>
        public GlobalPosition(GlobalCoordinates coords)
            : this(coords, 0)
        {
        }

        /// <summary>
        /// Creates a new instance of GlobalPosition.
        /// </summary>
        /// <param name="coords">coordinates on the reference ellipsoid.</param>
        /// <param name="elevationMeters">elevation, in meters, above the reference ellipsoid.</param>
        public GlobalPosition(GlobalCoordinates coords, double elevationMeters)
        {
            this.coordinates = coords;
            this.elevationMeters = elevationMeters;
        }

        /// <summary>Get global coordinates.</summary>
        public GlobalCoordinates Coordinates
        {
            get { return this.coordinates; }
        }

        /// <summary>Get latitude.</summary>
        public Angle Latitude
        {
            get { return this.coordinates.Latitude; }
        }

        /// <summary>Get longitude.</summary>
        public Angle Longitude
        {
            get { return this.coordinates.Longitude; }
        }

        /// <summary>
        /// Get elevation, in meters, above the surface of the reference ellipsoid.
        /// </summary>
        public double ElevationMeters
        {
            get { return this.elevationMeters; }
        }

        /// <summary>
        /// Compare this position to another.  Western longitudes are less than
        /// eastern logitudes.  If longitudes are equal, then southern latitudes are
        /// less than northern latitudes.  If coordinates are equal, lower elevations
        /// are less than higher elevations
        /// </summary>
        /// <param name="other">instance to compare to</param>
        /// <returns>-1, 0, or +1 as per IComparable contract</returns>
        public int CompareTo(object obj)
        {
            if (!(obj is GlobalPosition))
            {
                throw new ArgumentException("Can only compare GlobalPositions with other GlobalPositions.", "obj");
            }

            return this.CompareTo((GlobalPosition)obj);
        }

        /// <summary>
        /// Compare this position to another.  Western longitudes are less than
        /// eastern logitudes.  If longitudes are equal, then southern latitudes are
        /// less than northern latitudes.  If coordinates are equal, lower elevations
        /// are less than higher elevations
        /// </summary>
        /// <param name="other">instance to compare to</param>
        /// <returns>-1, 0, or +1 as per IComparable contract</returns>
        public int CompareTo(GlobalPosition other)
        {
            int a = this.coordinates.CompareTo(other.coordinates);

            return a == 0
                ? this.elevationMeters.CompareTo(other.elevationMeters)
                : a;
        }

        /// <summary>
        /// Calculate a hash code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = 17;

            hashCode = hashCode * 31 + this.coordinates.GetHashCode();
            hashCode = hashCode * 31 + this.elevationMeters.GetHashCode();

            return hashCode;
        }

        /// <summary>
        /// Compare this position to another object for equality.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is GlobalPosition &&
                   this.Equals((GlobalPosition)obj);
        }

        public bool Equals(GlobalPosition other)
        {
            return this.coordinates.Equals(other.coordinates) &&
                   this.elevationMeters == other.elevationMeters;
        }

        /// <summary>
        /// Get position as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture,
                                 "GlobalPosition[Coordinates={0}, ElevationMeters={1}]",
                                 this.coordinates,
                                 this.elevationMeters);
        }

        #region Serialization / Deserialization

        private GlobalPosition(SerializationInfo info, StreamingContext context)
        {
            this.elevationMeters = info.GetDouble("elevationMeters");

            double longitudeRadians = info.GetDouble("longitudeRadians");
            double latitudeRadians = info.GetDouble("latitudeRadians");

            Angle longitude = Angle.FromRadians(longitudeRadians);
            Angle latitude = Angle.FromRadians(latitudeRadians);

            this.coordinates = new GlobalCoordinates(longitude, latitude);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("elevationMeters", this.elevationMeters);

            info.AddValue("longitudeRadians", this.coordinates.Longitude.Radians);
            info.AddValue("latitudeRadians", this.coordinates.Latitude.Radians);
        }

        #endregion

        #region Operators

        public static bool operator ==(GlobalPosition lhs, GlobalPosition rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(GlobalPosition lhs, GlobalPosition rhs)
        {
            return !lhs.Equals(rhs);
        }

        public static bool operator <(GlobalPosition lhs, GlobalPosition rhs)
        {
            return lhs.CompareTo(rhs) < 0;
        }

        public static bool operator <=(GlobalPosition lhs, GlobalPosition rhs)
        {
            return lhs.CompareTo(rhs) <= 0;
        }

        public static bool operator >(GlobalPosition lhs, GlobalPosition rhs)
        {
            return lhs.CompareTo(rhs) > 0;
        }

        public static bool operator >=(GlobalPosition lhs, GlobalPosition rhs)
        {
            return lhs.CompareTo(rhs) >= 0;
        }

        #endregion
    }
}
