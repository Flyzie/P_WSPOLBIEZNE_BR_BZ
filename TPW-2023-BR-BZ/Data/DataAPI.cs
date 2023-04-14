using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;


namespace Data
{
    public abstract class DataAPI

    {
        public abstract void AddBall(Ball b);
        public abstract Ball GetBall(int i);

        public abstract int GetBallCount();

        public static Ball CreateNewBall(Vector2 p, double r, Vector2 S)
        {
            return new Ball(p, r, S);
        }
        public static Scene CreateNewTable(double w, double h)
        {
            return new Scene(w, h);

        }
    }
}
