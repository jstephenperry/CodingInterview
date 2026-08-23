using System.Security.Cryptography;

namespace CodingInterviewImplementations.RandomUtilities
{
    /// <summary>
    /// Cryptographically secure random value generation.
    /// </summary>
    /// <remarks>
    /// Every range in this class follows the .NET convention used by
    /// <see cref="RandomNumberGenerator.GetInt32(int, int)"/> and <see cref="Random.Next(int, int)"/>:
    /// <paramref name="min"/> is <b>inclusive</b> and <paramref name="max"/> is <b>exclusive</b>.
    /// Callers that want an inclusive upper bound must pass <c>max + 1</c>.
    /// </remarks>
    public static class SecureRandomHelper
    {
        /// <summary>
        /// Generates a buffer of cryptographically secure random bytes.
        /// </summary>
        /// <param name="length">The number of bytes to generate. Must not be negative.</param>
        /// <returns>A new array of <paramref name="length"/> random bytes.</returns>
        public static byte[] GenerateRandomBytes(int length)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(length);
            return RandomNumberGenerator.GetBytes(length);
        }

        /// <summary>
        /// Generates a uniformly distributed <see cref="short"/> in [<paramref name="min"/>, <paramref name="max"/>).
        /// </summary>
        public static short GenerateRandomShort(short min, short max)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);
            return (short)RandomNumberGenerator.GetInt32(min, max);
        }

        /// <summary>
        /// Generates a uniformly distributed <see cref="int"/> in [<paramref name="min"/>, <paramref name="max"/>).
        /// </summary>
        public static int GenerateRandomInt(int min, int max)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);
            return RandomNumberGenerator.GetInt32(min, max);
        }

        /// <summary>
        /// Generates a uniformly distributed <see cref="long"/> in [<paramref name="min"/>, <paramref name="max"/>).
        /// </summary>
        /// <remarks>
        /// Uses rejection sampling so that every value in the range is equally likely. A plain
        /// modulus would over-represent the low end of the range whenever the range size does not
        /// divide 2^64 evenly.
        /// </remarks>
        public static long GenerateRandomLong(long min, long max)
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);

            // Unsigned subtraction so that a range spanning the whole Int64 domain does not overflow.
            ulong count = unchecked((ulong)max - (ulong)min);

            // Largest multiple of count that fits in a UInt64; anything at or above it is rejected.
            ulong limit = ulong.MaxValue - (ulong.MaxValue % count);

            ulong value;
            do
            {
                value = NextUInt64();
            }
            while (value >= limit);

            return unchecked((long)((ulong)min + (value % count)));
        }

        /// <summary>
        /// Generates a uniformly distributed <see cref="float"/> in [<paramref name="min"/>, <paramref name="max"/>).
        /// </summary>
        public static float GenerateRandomFloat(float min, float max)
        {
            ThrowIfNotAFiniteRange(min, max);

            // 24 random bits scaled into [0,1) -- one bit per bit of float significand.
            float unit = (NextUInt32() >> 8) * (1.0f / (1 << 24));
            float result = min + (unit * (max - min));

            // Rounding in the scale-and-shift above can land exactly on max; keep the bound exclusive.
            return result >= max ? MathF.BitDecrement(max) : result;
        }

        /// <summary>
        /// Generates a uniformly distributed <see cref="double"/> in [<paramref name="min"/>, <paramref name="max"/>).
        /// </summary>
        public static double GenerateRandomDouble(double min, double max)
        {
            ThrowIfNotAFiniteRange(min, max);

            // 53 random bits scaled into [0,1) -- one bit per bit of double significand.
            double unit = (NextUInt64() >> 11) * (1.0 / (1UL << 53));
            double result = min + (unit * (max - min));

            return result >= max ? Math.BitDecrement(max) : result;
        }

        /// <summary>
        /// Generates a <see cref="bool"/> that is true half the time.
        /// </summary>
        public static bool GenerateRandomBool()
        {
            return RandomNumberGenerator.GetInt32(0, 2) == 1;
        }

        private static uint NextUInt32()
        {
            Span<byte> buffer = stackalloc byte[sizeof(uint)];
            RandomNumberGenerator.Fill(buffer);
            return BitConverter.ToUInt32(buffer);
        }

        private static ulong NextUInt64()
        {
            Span<byte> buffer = stackalloc byte[sizeof(ulong)];
            RandomNumberGenerator.Fill(buffer);
            return BitConverter.ToUInt64(buffer);
        }

        private static void ThrowIfNotAFiniteRange(double min, double max)
        {
            if (!double.IsFinite(min) || !double.IsFinite(max))
            {
                throw new ArgumentOutOfRangeException(nameof(min), "The range bounds must be finite.");
            }

            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(min, max);

            if (!double.IsFinite(max - min))
            {
                throw new ArgumentOutOfRangeException(nameof(max), "The range is too wide to sample.");
            }
        }
    }
}
