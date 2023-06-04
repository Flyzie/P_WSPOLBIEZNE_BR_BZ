using Logic;
using System.Numerics;

namespace TestBalls
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()  // test testuje, czy pi�ki w symulacji zmieniaj� swoj� pozycj� po uruchomieniu symulacji.
        {
            var interactionCount = 0;
            LogicAPI bl = new BallLogic(100, 100, false);
            bl.addBall(8);
            Assert.AreEqual(8, bl.getBallCount()); //sprawdza, czy po dodaniu 8 pi�ek do symulacji, ich liczba si� zgadza.

            var startPositionList = new List<Vector2>();
            for (int i = 0; i < bl.getBallCount(); i++)
            {
                startPositionList.Add(bl.getBall(i).Position);
            }

            bl.PositionChangedEvent += (_, _) =>
            {
                interactionCount++;   //zmienna przechowuj�ca liczb� interakcji w trakcie symulacji pi�ek
                Console.WriteLine("TPW");
                if (interactionCount >= 40)
                {
                    bl.ProgramStop();
                }
            };
            bl.ProgramStart();
            while (interactionCount < 40)
            { }
            Assert.IsTrue(interactionCount >= 39); //sprawdza, czy liczba interakcji wynosi co najmniej 39 po uruchomieniu symulacji.
            for (int i = 0; i < bl.getBallCount(); i++)
            {
                if (startPositionList[i] == bl.getBall(i).Position)
                {
                    Assert.Fail();
                }
            }





        }
    }
}