using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class BallCount : DataAPI
    {
        private List<Ball> balls;  //tutaj ustawiamy ilosc pilek
        public readonly object BallsLock = new object();


        public BallCount()
        {
            balls = new List<Ball>();
        }

        public override int GetBallCount()
        {
            return balls.Count;
        }

        public override void AddBall(Ball ball)
        {
            balls.Add(ball);
        }

        public override Ball GetBall(int index)
        {
            return balls[index];
        }


    }
}
