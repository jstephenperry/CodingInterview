namespace CodingInterviewImplementations.Mathematical.Tests
{
    [TestFixture()]
    public class MovingAverageTests
    {
        [Test]
        public void CalculateMovingAverageTestWithArray()
        {
            Assert.That(MovingAverage.CalculateMovingAverage(new int[] { 1, 2, 3, 4, 5, 6 }), Is.EqualTo(new double[] { 2, 3, 4, 5 }));
            Assert.That(MovingAverage.CalculateMovingAverage(new int[] { 1, 2, 3, 4, 5, 6 }, 2), Is.EqualTo(new double[] { 1.5, 2.5, 3.5, 4.5, 5.5 }));
        }

        [Test]
        public void CalculateMovingAverageTestWithList()
        {
            Assert.That(MovingAverage.CalculateMovingAverage(new List<int> { 1, 2, 3, 4, 5, 6 }), Is.EqualTo(new List<double> { 2, 3, 4, 5 }));
            Assert.That(MovingAverage.CalculateMovingAverage(new List<int> { 1, 2, 3, 4, 5, 6 }, 2), Is.EqualTo(new List<double> { 1.5, 2.5, 3.5, 4.5, 5.5 }));
        }
    }
}