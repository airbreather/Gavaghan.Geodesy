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
    /// Encapsulation of an ellipsoid, and declaration of common reference ellipsoids.
    /// </summary>
    [Serializable]
    public struct Ellipsoid : IEquatable<Ellipsoid>, ISerializable
    {
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
            this.SemiMajorAxisMeters = semiMajorAxisMeters;
            this.SemiMinorAxisMeters = semiMinorAxisMeters;
            this.Flattening = flattening;
            this.InverseFlattening = inverseFlattening;
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
        public double SemiMajorAxisMeters { get; }

        /// <summary>Get semi minor axis (meters).</summary>
        public double SemiMinorAxisMeters { get; }

        /// <summary>Get flattening.</summary>
        public double Flattening { get; }

        /// <summary>Get inverse flattening.</summary>
        public double InverseFlattening { get; }

        // others are derived from these values.
        public static int GetHashCode(Ellipsoid value) => HashCodeBuilder.Seed
                                                                         .HashWith(value.SemiMajorAxisMeters)
                                                                         .HashWith(value.Flattening);

        public static bool Equals(Ellipsoid first, Ellipsoid second) => first.SemiMajorAxisMeters == second.SemiMajorAxisMeters &&
                                                                        first.Flattening == second.Flattening;

        public static string ToString(Ellipsoid value) => $"Ellipsoid[SemiMajorAxisMeters={value.SemiMajorAxisMeters}, Flattening={value.Flattening}, SemiMinorAxisMeters={value.SemiMinorAxisMeters}, InverseFlattening={value.InverseFlattening}]";

        public override bool Equals(object obj) => obj is Ellipsoid && Equals(this, (Ellipsoid)obj);
        public bool Equals(Ellipsoid other) => Equals(this, other);
        public override int GetHashCode() => GetHashCode(this);
        public override string ToString() => ToString(this);

        #region Serialization / Deserialization

        private Ellipsoid(SerializationInfo info, StreamingContext context)
        {
            this.SemiMajorAxisMeters = info.GetDouble("semiMajorAxisMeters");
            this.Flattening = info.GetDouble("flattening");

            // Worth considering that these two can be MATHEMATICALLY derived from the other two,
            // but we could get different results if we tried it, depending on how we were created.
            this.SemiMinorAxisMeters = info.GetDouble("semiMinorAxisMeters");
            this.InverseFlattening = info.GetDouble("inverseFlattening");
        }

        private static void GetObjectData(Ellipsoid value, SerializationInfo info, StreamingContext context)
        {
            info.AddValue("semiMajorAxisMeters", value.SemiMajorAxisMeters);
            info.AddValue("flattening", value.Flattening);

            // Worth considering that these two can be MATHEMATICALLY derived from the other two,
            // but we could get different results if we tried it, depending on how we were created.
            info.AddValue("semiMinorAxisMeters", value.SemiMinorAxisMeters);
            info.AddValue("inverseFlattening", value.InverseFlattening);
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) => GetObjectData(this, info, context);

        #endregion

        #region Operators

        public static bool operator ==(Ellipsoid lhs, Ellipsoid rhs) => Equals(lhs, rhs);
        public static bool operator !=(Ellipsoid lhs, Ellipsoid rhs) => !Equals(lhs, rhs);

        #endregion
    }
}
