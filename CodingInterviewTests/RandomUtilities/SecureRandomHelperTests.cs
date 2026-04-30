namespace CodingInterviewImplementations.RandomUtilities.Tests
{
    [TestFixture]
    public class SecureRandomHelperTests
    {
        private const int Iterations = 1000;

        [Test]
        public void GenerateRandomBytesTest()
        {
            byte[] bytes = SecureRandomHelper.GenerateRandomBytes(32);
            Assert.That(bytes, Has.Length.EqualTo(32));
            Assert.That(bytes, Is.Not.All.EqualTo((byte)0));
        }

        [Test]
        public void GenerateRandomShortTest()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < Iterations; i++)
                {
                    Assert.That(SecureRandomHelper.GenerateRandomShort(1, 4), Is.InRange((short)1, (short)4));
                    Assert.That(SecureRandomHelper.GenerateRandomShort(-100, 100), Is.InRange((short)-100, (short)100));
                }
            });
        }

        [Test]
        public void GenerateRandomIntTest()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < Iterations; i++)
                {
                    Assert.That(SecureRandomHelper.GenerateRandomInt(1, 4), Is.InRange(1, 4));
                    Assert.That(SecureRandomHelper.GenerateRandomInt(1, 6), Is.InRange(1, 6));
                    Assert.That(SecureRandomHelper.GenerateRandomInt(1, 20), Is.InRange(1, 20));
                    Assert.That(SecureRandomHelper.GenerateRandomInt(-50, 50), Is.InRange(-50, 50));
                    Assert.That(SecureRandomHelper.GenerateRandomInt(7, 7), Is.EqualTo(7));
                }
            });
        }

        [Test]
        public void GenerateRandomInt_CoversFullInclusiveRange()
        {
            // With 6 buckets and >>6 iterations the chance of missing one is vanishingly small,
            // and a buggy [min, max) implementation would never produce the upper bound.
            var seen = new HashSet<int>();
            for (int i = 0; i < 1000 && seen.Count < 6; i++)
            {
                seen.Add(SecureRandomHelper.GenerateRandomInt(1, 6));
            }
            Assert.That(seen, Is.EquivalentTo(new[] { 1, 2, 3, 4, 5, 6 }));
        }

        [Test]
        public void GenerateRandomInt_ThrowsWhenMinExceedsMax()
        {
            Assert.That(() => SecureRandomHelper.GenerateRandomInt(10, 1), Throws.ArgumentException);
        }

        [Test]
        public void GenerateRandomLongTest()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < Iterations; i++)
                {
                    Assert.That(SecureRandomHelper.GenerateRandomLong(1L, 1_000_000L), Is.InRange(1L, 1_000_000L));
                    Assert.That(SecureRandomHelper.GenerateRandomLong(-100L, 100L), Is.InRange(-100L, 100L));
                }
            });
        }

        [Test]
        public void GenerateRandomFloatTest()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < Iterations; i++)
                {
                    float v = SecureRandomHelper.GenerateRandomFloat(0f, 1f);
                    Assert.That(float.IsFinite(v), Is.True);
                    Assert.That(v, Is.InRange(0f, 1f));
                }
            });
        }

        [Test]
        public void GenerateRandomDoubleTest()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < Iterations; i++)
                {
                    double v = SecureRandomHelper.GenerateRandomDouble(1.0, 10.0);
                    Assert.That(double.IsFinite(v), Is.True);
                    Assert.That(v, Is.InRange(1.0, 10.0));
                }
            });
        }

        [Test]
        public void GenerateRandomBoolTest()
        {
            int trueCount = 0;
            for (int i = 0; i < Iterations; i++)
            {
                if (SecureRandomHelper.GenerateRandomBool())
                {
                    trueCount++;
                }
            }

            // With 1000 fair coin flips, the count should land well inside [400, 600] (~6 sigma).
            Assert.That(trueCount, Is.InRange(400, 600));
        }
    }
}
