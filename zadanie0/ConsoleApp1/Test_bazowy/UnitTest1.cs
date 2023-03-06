using NUnit.Framework;
using ConsoleApp3;

namespace ConsoleApp3Tests
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void Add_WhenGivenTwoNumbers_ReturnsSum()
        {
            // Arrange
            int x = 3;
            int y = 4;
            int expected = 7;

            // Act
            int result = Program.Add(x, y);

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}