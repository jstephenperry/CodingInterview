namespace CodingInterviewImplementations.RandomUtilities.Tests
{
    [TestFixture()]
    public class SecureRandomHelperTests
    {
        [Test]
        public void GenerateRandomBytesTest()
        {

        }

        [Test]
        public void GenerateRandomShortTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(SecureRandomHelper.GenerateRandomDouble(1, 4), Is.InRange(1, 4));
                Assert.That(SecureRandomHelper.GenerateRandomDouble(1, 6), Is.InRange(1, 6));
                Assert.That(SecureRandomHelper.GenerateRandomDouble(1, 8), Is.InRange(1, 8));
                Assert.That(SecureRandomHelper.GenerateRandomDouble(1, 10), Is.InRange(1, 10));
                Assert.That(SecureRandomHelper.GenerateRandomDouble(1, 12), Is.InRange(1, 12));
                Assert.That(SecureRandomHelper.GenerateRandomDouble(1, 20), Is.InRange(1, 20));
            });
        }

        [Test]
        public void GenerateRandomIntTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(SecureRandomHelper.GenerateRandomInt(1, 4), Is.InRange(1, 4));
                Assert.That(SecureRandomHelper.GenerateRandomInt(1, 6), Is.InRange(1, 6));
                Assert.That(SecureRandomHelper.GenerateRandomInt(1, 8), Is.InRange(1, 8));
                Assert.That(SecureRandomHelper.GenerateRandomInt(1, 10), Is.InRange(1, 10));
                Assert.That(SecureRandomHelper.GenerateRandomInt(1, 12), Is.InRange(1, 12));
                Assert.That(SecureRandomHelper.GenerateRandomInt(1, 20), Is.InRange(1, 20));
            });
        }

        [Test]
        public void GenerateRandomLongTest()
        {

        }

        [Test]
        public void GenerateRandomFloatTest()
        {

        }

        [Test]
        public void GenerateRandomDoubleTest()
        {

        }

        [Test]
        public void GenerateRandomBoolTest()
        {

        }
    }
}