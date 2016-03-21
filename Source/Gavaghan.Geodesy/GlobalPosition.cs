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
    /// Encapsulates a three dimensional location on a globe (GlobalCoordinates combined with
    /// an elevation in meters above a reference ellipsoid).
    /// </summary>
    [Serializable]
    public struct GlobalPosition : IComparable<GlobalPosition>, IComparable, IEquatable<GlobalPosition>, ISerializable
    {
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
            this.Coordinates = coords;
            this.ElevationMeters = elevationMeters;
        }

        /// <summary>Get global coordinates.</summary>
        public GlobalCoordinates Coordinates { get; }

        /// <summary>Get latitude.</summary>
        public Angle Latitude => this.Coordinates.Latitude;

        /// <summary>Get longitude.</summary>
        public Angle Longitude => this.Coordinates.Longitude;

        /// <summary>
        /// Get elevation, in meters, above the surface of the reference ellipsoid.
        /// </summary>
        public double ElevationMeters { get; }

        public static int GetHashCode(GlobalPosition value) => HashCodeBuilder.Seed
                                                                              .HashWith(value.Coordinates)
                                                                              .HashWith(value.ElevationMeters);

        public static bool Equals(GlobalPosition first, GlobalPosition second) => GlobalCoordinates.Equals(first.Coordinates, second.Coordinates) &&
                                                                                  first.ElevationMeters == second.ElevationMeters;

        public static int Compare(GlobalPosition first, GlobalPosition second)
        {
            int a = GlobalCoordinates.Compare(first.Coordinates, second.Coordinates);

            return a == 0
                ? first.ElevationMeters.CompareTo(second.ElevationMeters)
                : a;
        }

        public static string ToString(GlobalPosition value) => $"GlobalPosition[Coordinates={value.Coordinates}, ElevationMeters={value.ElevationMeters}]";

        /// <summary>
        /// Calculate a hash code.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => GetHashCode(this);

        /// <summary>
        /// Compare this position to another object for equality.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) => obj is GlobalPosition &&
                                                   Equals(this, (GlobalPosition)obj);

        public bool Equals(GlobalPosition other) => Equals(this, other);

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
                throw new ArgumentException("Can only compare GlobalPositions with other GlobalPositions.", nameof(obj));
            }

            return Compare(this, (GlobalPosition)obj);
        }

        /// <summary>
        /// Compare this position to another.  Western longitudes are less than
        /// eastern logitudes.  If longitudes are equal, then southern latitudes are
        /// less than northern latitudes.  If coordinates are equal, lower elevations
        /// are less than higher elevations
        /// </summary>
        /// <param name="other">instance to compare to</param>
        /// <returns>-1, 0, or +1 as per IComparable contract</returns>
        public int CompareTo(GlobalPosition other) => Compare(this, other);

        /// <summary>
        /// Get position as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ToString(this);

        #region Serialization / Deserialization

        private GlobalPosition(SerializationInfo info, StreamingContext context)
        {
            this.ElevationMeters = info.GetDouble("elevationMeters");

            double longitudeRadians = info.GetDouble("longitudeRadians");
            double latitudeRadians = info.GetDouble("latitudeRadians");

            Angle longitude = Angle.FromRadians(longitudeRadians);
            Angle latitude = Angle.FromRadians(latitudeRadians);

            this.Coordinates = new GlobalCoordinates(longitude, latitude);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("elevationMeters", this.ElevationMeters);

            info.AddValue("longitudeRadians", this.Coordinates.Longitude.Radians);
            info.AddValue("latitudeRadians", this.Coordinates.Latitude.Radians);
        }

        #endregion

        #region Operators

        public static bool operator ==(GlobalPosition lhs, GlobalPosition rhs) => Equals(lhs, rhs);
        public static bool operator !=(GlobalPosition lhs, GlobalPosition rhs) => !Equals(lhs, rhs);
        public static bool operator <(GlobalPosition lhs, GlobalPosition rhs) => Compare(lhs, rhs) < 0;
        public static bool operator <=(GlobalPosition lhs, GlobalPosition rhs) => Compare(lhs, rhs) <= 0;
        public static bool operator >(GlobalPosition lhs, GlobalPosition rhs) => Compare(lhs, rhs) > 0;
        public static bool operator >=(GlobalPosition lhs, GlobalPosition rhs) => Compare(lhs, rhs) >= 0;

        #endregion
    }
}
