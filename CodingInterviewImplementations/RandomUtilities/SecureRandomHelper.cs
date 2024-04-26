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
            byte[] randomBytes = GenerateRandomBytes(2);
            short randomShort = BitConverter.ToInt16(randomBytes, 0);
            return (short)(Math.Abs(randomShort % (max - min)) + min);
        }

        public static int GenerateRandomInt(int min, int max)
        {
            byte[] randomBytes = GenerateRandomBytes(4);
            int randomInt = BitConverter.ToInt32(randomBytes, 0);
            return Math.Abs(randomInt % (max - min)) + min;
        }

        public static long GenerateRandomLong(long min, long max)
        {
            byte[] randomBytes = GenerateRandomBytes(8);
            long randomLong = BitConverter.ToInt64(randomBytes, 0);
            return Math.Abs(randomLong % (max - min)) + min;
        }

        public static float GenerateRandomFloat(float min, float max)
        {
            byte[] randomBytes = GenerateRandomBytes(4);
            float randomFloat = BitConverter.ToSingle(randomBytes, 0);
            return Math.Abs(randomFloat % (max - min)) + min;
        }

        public static double GenerateRandomDouble(double min, double max)
        {
            byte[] randomBytes = GenerateRandomBytes(8);
            double randomDouble = BitConverter.ToDouble(randomBytes, 0);
            return Math.Abs(randomDouble % (max - min)) + min;
        }

        public static bool GenerateRandomBool()
        {
            byte[] randomBytes = GenerateRandomBytes(1);
            return randomBytes[0] % 2 == 0;
        }
    }
}
