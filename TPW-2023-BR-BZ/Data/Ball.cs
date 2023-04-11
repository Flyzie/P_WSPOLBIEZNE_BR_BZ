using System.Numerics;

namespace Data
{
    public class Ball
    {
        public double Speed { get; set; }  
        public double Position { get; set; }
        public double Radius { get; set; }

        public Ball(double p, double r, double S) //parametry piłek
        {
            Position = p;
            Radius = 40;
            Speed = S;
        }
    }
}
