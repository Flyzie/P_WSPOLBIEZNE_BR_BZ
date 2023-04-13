using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class BallCount : DataAPI
    {
        private List<Ball> balls;  //tutaj ustawiamy ilosc pilek

        public BallCount()
        {
            balls = new List<Ball>();
        }

        public override Ball GetBall(int index)
        {
            return balls[index];
        }


    }
}
