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
    /// Encapsulation of an ellipsoid, and declaration of common reference ellipsoids.
    /// </summary>
    [Serializable]
    public struct Ellipsoid : IEquatable<Ellipsoid>, ISerializable
    {
        // intentionally NOT readonly, for performance reasons.
        /// <summary>Semi major axis (meters).</summary>
        private double semiMajorAxisMeters;

        // intentionally NOT readonly, for performance reasons.
        /// <summary>Semi minor axis (meters).</summary>
        private double semiMinorAxisMeters;

        // intentionally NOT readonly, for performance reasons.
        /// <summary>Flattening.</summary>
        private double flattening;

        // intentionally NOT readonly, for performance reasons.
        /// <summary>Inverse flattening.</summary>
        private double inverseFlattening;

        /// <summary>
        /// Construct a new Ellipsoid.  This is private to ensure the values are
        /// consistent (flattening = 1.0 / inverseFlattening).  Use the methods 
        /// FromAAndInverseF() and FromAAndF() to create new instances.
        /// </summary>
        /// <param name="semiMajorAxisMeters"></param>
        /// <param name="semiMinorAxisMeters"></param>
        /// <param name="flattening"></param>
        /// <param name="inverseFlattening"></param>
        private Ellipsoid(double semiMajorAxisMeters, double semiMinorAxisMeters, double flattening, double inverseFlattening)
        {
            this.semiMajorAxisMeters = semiMajorAxisMeters;
            this.semiMinorAxisMeters = semiMinorAxisMeters;
            this.flattening = flattening;
            this.inverseFlattening = inverseFlattening;
        }

        #region Reference Ellipsoids

        /// <summary>The WGS84 ellipsoid.</summary>
        public static readonly Ellipsoid WGS84 = FromAAndInverseF(6378137.0, 298.257223563);

        /// <summary>The GRS80 ellipsoid.</summary>
        public static readonly Ellipsoid GRS80 = FromAAndInverseF(6378137.0, 298.257222101);

        /// <summary>The GRS67 ellipsoid.</summary>
        public static readonly Ellipsoid GRS67 = FromAAndInverseF(6378160.0, 298.25);

        /// <summary>The ANS ellipsoid.</summary>
        public static readonly Ellipsoid ANS = FromAAndInverseF(6378160.0, 298.25);

        /// <summary>The WGS72 ellipsoid.</summary>
        public static readonly Ellipsoid WGS72 = FromAAndInverseF(6378135.0, 298.26);

        /// <summary>The Clarke1858 ellipsoid.</summary>
        public static readonly Ellipsoid Clarke1858 = FromAAndInverseF(6378293.645, 294.26);

        /// <summary>The Clarke1880 ellipsoid.</summary>
        public static readonly Ellipsoid Clarke1880 = FromAAndInverseF(6378249.145, 293.465);

        /// <summary>A spherical "ellipsoid".</summary>
        public static readonly Ellipsoid Sphere = FromAAndF(6371000, 0.0);

        #endregion

        /// <summary>
        /// Build an Ellipsoid from the semi major axis measurement and the inverse flattening.
        /// </summary>
        /// <param name="semiMajorAxisMeters">semi major axis (meters)</param>
        /// <param name="inverseFlattening"></param>
        /// <returns></returns>
        public static Ellipsoid FromAAndInverseF(double semiMajorAxisMeters, double inverseFlattening)
        {
            double f = 1.0 / inverseFlattening;
            double b = (1.0 - f) * semiMajorAxisMeters;

            return new Ellipsoid(semiMajorAxisMeters, b, f, inverseFlattening);
        }

        /// <summary>
        /// Build an Ellipsoid from the semi major axis measurement and the flattening.
        /// </summary>
        /// <param name="semiMajorAxisMeters">semi major axis (meters)</param>
        /// <param name="flattening"></param>
        /// <returns></returns>
        public static Ellipsoid FromAAndF(double semiMajorAxisMeters, double flattening)
        {
            double inverseF = 1.0 / flattening;
            double b = (1.0 - flattening) * semiMajorAxisMeters;

            return new Ellipsoid(semiMajorAxisMeters, b, flattening, inverseF);
        }

        /// <summary>Get semi major axis (meters).</summary>
        public double SemiMajorAxisMeters
        {
            get { return semiMajorAxisMeters; }
        }

        /// <summary>Get semi minor axis (meters).</summary>
        public double SemiMinorAxisMeters
        {
            get { return semiMinorAxisMeters; }
        }

        /// <summary>Get flattening.</summary>
        public double Flattening
        {
            get { return flattening; }
        }

        /// <summary>Get inverse flattening.</summary>
        public double InverseFlattening
        {
            get { return inverseFlattening; }
        }

        public override int GetHashCode()
        {
            int hashCode = 17;

            // others are derived from these values.
            hashCode = hashCode * 31 + this.semiMajorAxisMeters.GetHashCode();
            hashCode = hashCode * 31 + this.flattening.GetHashCode();

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            return obj is Ellipsoid &&
                   this.Equals((Ellipsoid)obj);
        }

        public bool Equals(Ellipsoid other)
        {
            // others are derived from these values.
            return this.semiMajorAxisMeters == other.semiMajorAxisMeters &&
                   this.flattening == other.flattening;
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture,
                                 "Ellipsoid[SemiMajorAxisMeters={0}, Flattening={1}, SemiMinorAxisMeters={2}, InverseFlattening={3}]",
                                 this.semiMajorAxisMeters,
                                 this.flattening,
                                 this.semiMinorAxisMeters,
                                 this.inverseFlattening);
        }

        #region Serialization / Deserialization

        private Ellipsoid(SerializationInfo info, StreamingContext context)
        {
            this.semiMajorAxisMeters = info.GetDouble("semiMajorAxisMeters");
            this.flattening = info.GetDouble("flattening");

            // Worth considering that these two can be MATHEMATICALLY derived from the other two,
            // but we could get different results if we tried it, depending on how we were created.
            this.semiMinorAxisMeters = info.GetDouble("semiMinorAxisMeters");
            this.inverseFlattening = info.GetDouble("inverseFlattening");
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("semiMajorAxisMeters", this.semiMajorAxisMeters);
            info.AddValue("flattening", this.flattening);

            // Worth considering that these two can be MATHEMATICALLY derived from the other two,
            // but we could get different results if we tried it, depending on how we were created.
            info.AddValue("semiMinorAxisMeters", this.semiMinorAxisMeters);
            info.AddValue("inverseFlattening", this.inverseFlattening);
        }

        #endregion

        #region Operators

        public static bool operator ==(Ellipsoid lhs, Ellipsoid rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Ellipsoid lhs, Ellipsoid rhs)
        {
            return !lhs.Equals(rhs);
        }

        #endregion
    }
}
