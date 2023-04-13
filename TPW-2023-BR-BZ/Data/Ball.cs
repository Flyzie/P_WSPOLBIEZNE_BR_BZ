using System.Numerics;

namespace Data
{
    public class Ball
    {
        public Vector2 Speed { get; set; }  
        public Vector2 Position { get; set; }
        public double Radius { get; set; }

        public Ball(Vector2 p, double r, Vector2 S) //parametry piłek
        {
            Position = p;
            Radius = 40;
            Speed = S;
        }
    }
}
