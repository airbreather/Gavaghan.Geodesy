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
    /// Encapsulation of an Angle.
    /// Angle comparisons are performed in absolute terms - no "wrapping" occurs.
    /// In other words, 360 degress != 0 degrees.
    /// </summary>
    [Serializable]
    public struct Angle : IComparable<Angle>, IComparable, IEquatable<Angle>, ISerializable
    {
        /// <summary>Zero Angle</summary>
        public static readonly Angle Zero = new Angle(0);

        /// <summary>180 degree Angle</summary>
        public static readonly Angle Angle180 = Angle.FromRadians(Math.PI);

        /// <summary>NaN-valued Angle</summary>
        public static readonly Angle NaN = Angle.FromRadians(Double.NaN);

        /// <summary>Degrees/Radians conversion constant.</summary>
        private const double PiOver180 = Math.PI / 180;

        /// <summary>
        /// Construct a new Angle from a measurement in radians.
        /// </summary>
        /// <param name="degrees">angle measurement</param>
        private Angle(double radians)
        {
            this.Radians = radians;
        }

        /// <summary>
        /// Get angle measured in degrees.
        /// </summary>
        public double Degrees => this.Radians / PiOver180;

        /// <summary>
        /// Get angle measured in radians.
        /// </summary>
        public double Radians { get; }

        public static Angle FromRadians(double radians) => new Angle(radians);
        public static Angle FromDegrees(double degrees) => new Angle(degrees * PiOver180);

        public static Angle FromDegreesAndMinutes(int degrees, double minutes)
        {
            double d = minutes / 60;
            d = degrees < 0 ? degrees - d : degrees + d;

            return new Angle(d * PiOver180);
        }

        public static Angle FromDegreesMinutesAndSeconds(int degrees, int minutes, double seconds)
        {
            double d = (seconds / 3600) + (minutes / 60.0);
            d = degrees < 0 ? degrees - d : degrees + d;

            return new Angle(d * PiOver180);
        }

        /// <summary>
        /// Get the absolute value of the angle.
        /// </summary>
        public static Angle Abs(Angle angle) => new Angle(Math.Abs(angle.Radians));

        public static bool IsNaN(Angle angle) => Double.IsNaN(angle.Radians);

        public static string ToString(Angle value) => $"Angle[Degrees={value.Degrees}, Radians={value.Radians}]";

        /// <summary>
        /// Compare this angle to another angle.
        /// </summary>
        /// <param name="obj">other angle to compare to.</param>
        /// <returns>result according to IComparable contract/></returns>
        public int CompareTo(object obj)
        {
            if (!(obj is Angle))
            {
                throw new ArgumentException("Can only compare Angles with other Angles.", nameof(obj));
            }

            return this.CompareTo((Angle)obj);
        }

        /// <summary>
        /// Compare this angle to another angle.
        /// </summary>
        /// <param name="other">other angle to compare to.</param>
        /// <returns>result according to IComparable contract/></returns>
        public int CompareTo(Angle other) => this.Radians.CompareTo(other.Radians);

        /// <summary>
        /// Calculate a hash code for the angle.
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode() => this.Radians.GetHashCode();

        /// <summary>
        /// Compare this Angle to another Angle for equality.  Angle comparisons
        /// are performed in absolute terms - no "wrapping" occurs.  In other
        /// words, 360 degress != 0 degrees.
        /// </summary>
        /// <param name="obj">object to compare to</param>
        /// <returns>'true' if angles are equal</returns>
        public override bool Equals(object obj) => obj is Angle && this.Equals((Angle)obj);

        /// <summary>
        /// Compare this Angle to another Angle for equality.
        /// Equality is defined in absolute terms.
        /// i.e., a zero-degree angle does not equal a 360-degree angle.
        /// Use Normalize for that.
        /// </summary>
        /// <param name="other">Angle to compare to</param>
        /// <returns>'true' if angles are equal</returns>
        public bool Equals(Angle other) => this.Radians == other.Radians;

        /// <summary>
        /// Get coordinates as a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => ToString(this);

        #region Serialization / Deserialization

        private Angle(SerializationInfo info, StreamingContext context)
        {
            this.Radians = info.GetDouble("radians");
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("radians", this.Radians);
        }

        #endregion

        #region Operators

        public static Angle operator +(Angle lhs, Angle rhs) => new Angle(lhs.Radians + rhs.Radians);
        public static Angle operator -(Angle lhs, Angle rhs) => new Angle(lhs.Radians - rhs.Radians);
        public static bool operator ==(Angle lhs, Angle rhs) => lhs.Radians == rhs.Radians;
        public static bool operator !=(Angle lhs, Angle rhs) => lhs.Radians != rhs.Radians;
        public static bool operator <(Angle lhs, Angle rhs) => lhs.Radians < rhs.Radians;
        public static bool operator <=(Angle lhs, Angle rhs) => lhs.Radians <= rhs.Radians;
        public static bool operator >(Angle lhs, Angle rhs) => lhs.Radians > rhs.Radians;
        public static bool operator >=(Angle lhs, Angle rhs) => lhs.Radians >= rhs.Radians;

        #endregion
    }
}
