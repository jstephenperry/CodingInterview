using System.Security.Cryptography;

namespace CodingInterviewImplementations.RandomUtilities
{
    /// <summary>
    /// Cryptographically-strong random number generation built on
    /// <see cref="RandomNumberGenerator"/>. All inclusive integer methods use rejection
    /// sampling so the distribution over the requested range is uniform (no modulo bias).
    /// </summary>
    public static class SecureRandomHelper
    {
        private static readonly RandomNumberGenerator Rng = RandomNumberGenerator.Create();

        /// <summary>Returns a new array of <paramref name="length"/> random bytes.</summary>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
        public static byte[] GenerateRandomBytes(int length)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(length);
            byte[] randomBytes = new byte[length];
            if (length > 0)
            {
                Rng.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        /// <summary>Returns a uniformly-distributed <see cref="short"/> in <c>[min, max]</c>.</summary>
        /// <exception cref="ArgumentException"><paramref name="min"/> &gt; <paramref name="max"/>.</exception>
        public static short GenerateRandomShort(short min, short max) =>
            (short)GenerateRandomInt(min, max);

        /// <summary>Returns a uniformly-distributed <see cref="int"/> in <c>[min, max]</c>.</summary>
        /// <exception cref="ArgumentException"><paramref name="min"/> &gt; <paramref name="max"/>.</exception>
        public static int GenerateRandomInt(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentException("min must be less than or equal to max.", nameof(min));
            }

            uint range = (uint)((long)max - min + 1);
            uint limit = uint.MaxValue - (uint.MaxValue % range);

            Span<byte> buffer = stackalloc byte[4];
            uint candidate;
            do
            {
                Rng.GetBytes(buffer);
                candidate = BitConverter.ToUInt32(buffer);
            } while (candidate >= limit);

            return (int)(min + candidate % range);
        }

        /// <summary>Returns a uniformly-distributed <see cref="long"/> in <c>[min, max]</c>.</summary>
        /// <exception cref="ArgumentException"><paramref name="min"/> &gt; <paramref name="max"/>.</exception>
        public static long GenerateRandomLong(long min, long max)
        {
            if (min > max)
            {
                throw new ArgumentException("min must be less than or equal to max.", nameof(min));
            }

            // range == 0 means the request spans the full ulong width (e.g. long.MinValue..long.MaxValue);
            // any 8 random bytes are already uniform.
            ulong range = (ulong)(max - min) + 1;
            ulong limit = range == 0 ? 0 : ulong.MaxValue - (ulong.MaxValue % range);

            Span<byte> buffer = stackalloc byte[8];
            ulong candidate;
            do
            {
                Rng.GetBytes(buffer);
                candidate = BitConverter.ToUInt64(buffer);
            } while (range != 0 && candidate >= limit);

            return range == 0 ? (long)candidate : min + (long)(candidate % range);
        }

        /// <summary>Returns a uniformly-distributed <see cref="float"/> in <c>[min, max]</c>.</summary>
        public static float GenerateRandomFloat(float min, float max) =>
            (float)GenerateRandomDouble(min, max);

        /// <summary>Returns a uniformly-distributed <see cref="double"/> in <c>[min, max]</c>.</summary>
        /// <exception cref="ArgumentException"><paramref name="min"/> &gt; <paramref name="max"/>.</exception>
        public static double GenerateRandomDouble(double min, double max)
        {
            if (min > max)
            {
                throw new ArgumentException("min must be less than or equal to max.", nameof(min));
            }

            Span<byte> buffer = stackalloc byte[8];
            Rng.GetBytes(buffer);
            ulong bits = BitConverter.ToUInt64(buffer);

            // 53 random bits divided by 2^53 gives a uniform value in [0, 1).
            double unit = (bits >> 11) * (1.0 / (1UL << 53));
            return min + unit * (max - min);
        }

        /// <summary>Returns a uniformly-distributed <see cref="bool"/>.</summary>
        public static bool GenerateRandomBool()
        {
            Span<byte> buffer = stackalloc byte[1];
            Rng.GetBytes(buffer);
            return (buffer[0] & 1) == 1;
        }
    }
}
