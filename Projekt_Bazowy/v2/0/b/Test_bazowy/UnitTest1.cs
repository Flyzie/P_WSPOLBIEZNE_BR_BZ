using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;

namespace Program0.Tests
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void TestAddition()
        {
            // Arrange
            Program program = new Program();
            int x = 3;
            int y = 5;
            long expectedSum = 8;

            // Act
            long actualSum = program.addition(x, y);

            // Assert
            Assert.AreEqual(expectedSum, actualSum);
        }
    }
}
