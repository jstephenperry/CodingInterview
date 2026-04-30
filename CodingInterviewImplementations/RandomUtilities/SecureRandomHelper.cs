using System.Security.Cryptography;

namespace CodingInterviewImplementations.RandomUtilities
{
    public static class SecureRandomHelper
    {
        private static readonly RandomNumberGenerator _randomNumberGenerator = RandomNumberGenerator.Create();

        public static byte[] GenerateRandomBytes(int length)
        {
            byte[] randomBytes = new byte[length];
            _randomNumberGenerator.GetBytes(randomBytes);
            return randomBytes;
        }

        public static short GenerateRandomShort(short min, short max)
        {
            return (short)GenerateRandomInt(min, max);
        }

        public static int GenerateRandomInt(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentException("min must be less than or equal to max");
            }

            // Inclusive range using rejection sampling (no modulo bias).
            uint range = (uint)((long)max - min + 1);
            uint limit = uint.MaxValue - (uint.MaxValue % range);

            byte[] buffer = new byte[4];
            uint candidate;
            do
            {
                _randomNumberGenerator.GetBytes(buffer);
                candidate = BitConverter.ToUInt32(buffer, 0);
            } while (candidate >= limit);

            return (int)(min + candidate % range);
        }

        public static long GenerateRandomLong(long min, long max)
        {
            if (min > max)
            {
                throw new ArgumentException("min must be less than or equal to max");
            }

            ulong range = (ulong)(max - min) + 1;
            ulong limit = range == 0 ? 0 : ulong.MaxValue - (ulong.MaxValue % range);

            byte[] buffer = new byte[8];
            ulong candidate;
            do
            {
                _randomNumberGenerator.GetBytes(buffer);
                candidate = BitConverter.ToUInt64(buffer, 0);
            } while (range != 0 && candidate >= limit);

            return range == 0 ? (long)candidate : min + (long)(candidate % range);
        }

        public static float GenerateRandomFloat(float min, float max)
        {
            return (float)GenerateRandomDouble(min, max);
        }

        public static double GenerateRandomDouble(double min, double max)
        {
            if (min > max)
            {
                throw new ArgumentException("min must be less than or equal to max");
            }

            byte[] buffer = new byte[8];
            _randomNumberGenerator.GetBytes(buffer);
            ulong bits = BitConverter.ToUInt64(buffer, 0);

            // Take 53 random bits and divide by 2^53 to produce a uniform value in [0, 1).
            double unit = (bits >> 11) * (1.0 / (1UL << 53));
            return min + unit * (max - min);
        }

        public static bool GenerateRandomBool()
        {
            byte[] buffer = new byte[1];
            _randomNumberGenerator.GetBytes(buffer);
            return (buffer[0] & 1) == 1;
        }
    }
}
