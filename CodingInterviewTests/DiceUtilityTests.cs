namespace CodingInterviewImplementations.Tests
{
    [TestFixture]
    public class DiceUtilityTests
    {
        [Test]
        public void RollDieTest()
        {
            Assert.Multiple(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    Assert.That(DiceUtility.RollDie(4), Is.InRange(1, 4));
                    Assert.That(DiceUtility.RollDie(6), Is.InRange(1, 6));
                    Assert.That(DiceUtility.RollDie(8), Is.InRange(1, 8));
                    Assert.That(DiceUtility.RollDie(10), Is.InRange(1, 10));
                    Assert.That(DiceUtility.RollDie(12), Is.InRange(1, 12));
                    Assert.That(DiceUtility.RollDie(20), Is.InRange(1, 20));
                    Assert.That(DiceUtility.RollDie(100), Is.InRange(1, 100));
                }
            });
        }

        [Test]
        public void RollDiceTest()
        {
            Assert.Multiple(() =>
            {
                Assert.That(DiceUtility.RollDice(4, 1), Is.InRange(1, 4));
                Assert.That(DiceUtility.RollDice(4, 1, 1), Is.InRange(2, 5));

                Assert.That(DiceUtility.RollDice(6, 1), Is.InRange(1, 6));
                Assert.That(DiceUtility.RollDice(6, 1, 1), Is.InRange(2, 7));

                Assert.That(DiceUtility.RollDice(8, 1), Is.InRange(1, 8));
                Assert.That(DiceUtility.RollDice(8, 1, 1), Is.InRange(2, 9));

                Assert.That(DiceUtility.RollDice(10, 1), Is.InRange(1, 10));
                Assert.That(DiceUtility.RollDice(10, 1, 1), Is.InRange(2, 11));

                Assert.That(DiceUtility.RollDice(12, 1), Is.InRange(1, 12));
                Assert.That(DiceUtility.RollDice(12, 1, 1), Is.InRange(2, 13));

                Assert.That(DiceUtility.RollDice(20, 1), Is.InRange(1, 20));
                Assert.That(DiceUtility.RollDice(20, 1, 1), Is.InRange(2, 21));

                Assert.That(DiceUtility.RollDice(100, 1), Is.InRange(1, 100));
                Assert.That(DiceUtility.RollDice(100, 1, 1), Is.InRange(2, 101));
            });
        }
    }
}