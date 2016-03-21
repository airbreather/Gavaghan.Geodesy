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
    /// Encapsulation of latitude and longitude coordinates on a globe.  Negative
    /// latitude is southern hemisphere.  Negative longitude is western hemisphere.
    /// 
    /// Any angle may be specified for longtiude and latitude, but all angles will
    /// be canonicalized such that:
    /// 
    ///      -90 &lt;= latitude &lt;= +90
    ///     -180 &lt;  longitude &lt;= +180
    /// </summary>
    [Serializable]
    public struct GlobalCoordinates : IComparable<GlobalCoordinates>, IComparable, IEquatable<GlobalCoordinates>, ISerializable
    {
        private const double PiOver2 = Math.PI / 2;
        private const double TwoPi = Math.PI + Math.PI;
        private const double NegativePiOver2 = -PiOver2;
        private const double NegativeTwoPi = -TwoPi;

        /// <summary>
        /// Construct a new GlobalCoordinates.  Angles will be canonicalized.
        /// </summary>
        /// <param name="latitude">latitude</param>
        /// <param name="longitude">longitude</param>
        public GlobalCoordinates(Angle latitude, Angle longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Canonicalize();
        }

        /// <summary>
        /// Get latitude.  The latitude value will be canonicalized (which might
        /// result in a change to the longitude). Negative latitude is southern hemisphere.
        /// </summary>
        public Angle Latitude { get; private set; }

        /// <summary>
        /// Get longitude.  The longitude value will be canonicalized. Negative
        /// longitude is western hemisphere.
        /// </summary>
        public Angle Longitude { get; private set; }

        /// <summary>
        /// Canonicalize the current latitude and longitude values such that:
        /// 
        ///      -90 &lt;= latitude  &lt;=  +90
        ///     -180 &lt;  longitude &lt;= +180
        /// </summary>
        private void Canonicalize()
        {
            // To understand why this works the way it does, imagine walking along a meridian,
            // starting at the South Pole, heading north.  As you keep going north, your latitude
            // gets bigger and bigger, until you reach the North Pole.  You've now walked from -90
            // to 90, and mathematically, that's as high as latitude can go.  However, you're
            // completely capable of continuing to walk in that same straight line.  A little
            // farther (relatively speaking), and you've walked 181 degrees of latitude, but now
            // you're walking on the opposite meridian.
            double latitudeRadians = this.Latitude.Radians;
            double longitudeRadians = this.Longitude.Radians;

            latitudeRadians = (latitudeRadians + Math.PI) % TwoPi;
            if (latitudeRadians < 0) latitudeRadians += TwoPi;
            latitudeRadians -= Math.PI;

            if (latitudeRadians > PiOver2)
            {
                latitudeRadians = Math.PI - latitudeRadians;
                longitudeRadians += Math.PI;
            }
            else if (latitudeRadians < NegativePiOver2)
            {
                latitudeRadians = -Math.PI - latitudeRadians;
                longitudeRadians += Math.PI;
            }

            longitudeRadians = ((longitudeRadians + Math.PI) % TwoPi);
            if (longitudeRadians <= 0) longitudeRadians += TwoPi;
            longitudeRadians -= Math.PI;

            this.Latitude = Angle.FromRadians(latitudeRadians);
            this.Longitude = Angle.FromRadians(longitudeRadians);
        }

        public static int GetHashCode(GlobalCoordinates value) => HashCodeBuilder.Seed
                                                                                 .HashWith(value.Longitude)
                                                                                 .HashWith(value.Latitude);

        public static bool Equals(GlobalCoordinates first, GlobalCoordinates second) => first.Latitude == second.Latitude &&
                                                                                        first.Longitude == second.Longitude;

        public static int Compare(GlobalCoordinates first, GlobalCoordinates second)
        {
            int a = first.Longitude.CompareTo(second.Longitude);

            return a == 0
                ? first.Latitude.CompareTo(second.Latitude)
                : a;
        }

        public static string ToString(GlobalCoordinates value) => $"GlobalCoordinates[Longitude={value.Longitude}, Latitude={value.Latitude}]";

        /// <summary>
        /// Get a hash code for these coordinates.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => GetHashCode(this);

        /// <summary>
        /// Compare these coordinates to another object for equality.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) => obj is GlobalCoordinates &&
                                                   Equals(this, (GlobalCoordinates)obj);

        /// <summary>
        /// Compare these coordinates to another object for equality.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(GlobalCoordinates other) => Equals(this, other);

        /// <summary>
        /// Compare these coordinates to another set of coordiates.  Western
        /// longitudes are less than eastern logitudes.  If longitudes are equal,
        /// then southern latitudes are less than northern latitudes.
        /// </summary>
        /// <param name="other">instance to compare to</param>
        /// <returns>-1, 0, or +1 as per IComparable contract</returns>
        public int CompareTo(object obj)
        {
            if (!(obj is GlobalCoordinates))
            {
                throw new ArgumentException("Can only compare GlobalCoordinates with other GlobalCoordinates.", nameof(obj));
            }

            return Compare(this, (GlobalCoordinates)obj);
        }

        /// <summary>
        /// Compare these coordinates to another set of coordiates.  Western
        /// longitudes are less than eastern logitudes.  If longitudes are equal,
        /// then southern latitudes are less than northern latitudes.
        /// </summary>
        /// <param name="other">instance to compare to</param>
        /// <returns>-1, 0, or +1 as per IComparable contract</returns>
        public int CompareTo(GlobalCoordinates other) => Compare(this, other);

        /// <summary>
        /// Get coordinates as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ToString(this);

        #region Serialization / Deserialization

        private GlobalCoordinates(SerializationInfo info, StreamingContext context)
        {
            double longitudeRadians = info.GetDouble("longitudeRadians");
            double latitudeRadians = info.GetDouble("latitudeRadians");

            this.Longitude = Angle.FromRadians(longitudeRadians);
            this.Latitude = Angle.FromRadians(latitudeRadians);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("longitudeRadians", this.Longitude.Radians);
            info.AddValue("latitudeRadians", this.Latitude.Radians);
        }

        #endregion

        #region Operators

        public static bool operator ==(GlobalCoordinates lhs, GlobalCoordinates rhs) => Equals(lhs, rhs);
        public static bool operator !=(GlobalCoordinates lhs, GlobalCoordinates rhs) => !Equals(lhs, rhs);
        public static bool operator <(GlobalCoordinates lhs, GlobalCoordinates rhs) => Compare(lhs, rhs) < 0;
        public static bool operator <=(GlobalCoordinates lhs, GlobalCoordinates rhs) => Compare(lhs, rhs) <= 0;
        public static bool operator >(GlobalCoordinates lhs, GlobalCoordinates rhs) => Compare(lhs, rhs) > 0;
        public static bool operator >=(GlobalCoordinates lhs, GlobalCoordinates rhs) => Compare(lhs, rhs) >= 0;

        #endregion
    }
}
