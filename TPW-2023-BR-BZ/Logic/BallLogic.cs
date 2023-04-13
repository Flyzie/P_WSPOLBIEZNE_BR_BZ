using System;
using System.Collections.Generic;
using System.Text;
using Data;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Logic
{
    public class BallLogic : LogicAPI
    {
        public BallCount Balls { get; set; }
        public Scene Scene { get; set; }

        public CancellationTokenSource CancelSimulationSource { get; private set; }

        private bool _started = false;

        public BallLogic(double w, double h)
        {
            Scene = new Scene(w, h);
            Balls = new BallCount();
            CancelSimulationSource = new CancellationTokenSource();
        }

        protected override void onPositionChange(BallMovement args)
        {
            base.onPositionChange(args);
        }


        public void MoveBall(Ball b)
        {
            Vector2 featurePosition = b.Position + b.Speed;
            if (featurePosition.X - b.Radius < 0 && featurePosition.X + b.Radius > Scene.Width)
            {
                b.Speed = b.Speed * new Vector2(1, -1);
            }

            if (featurePosition.Y - b.Radius < 0 && featurePosition.Y + b.Radius > Scene.Height)
            {
                b.Speed = b.Speed * new Vector2(-1, 1);
            }

            b.Position = b.Position + b.Speed;
        }

        public void makeBall(double r)
        {
            var rng = new Random();
            var x = ((double)rng.NextDouble() * (Scene.Width - (2 * r)) + r);
            var y = ((double)rng.NextDouble() * (Scene.Height - (2 * r)) + r);
        }
        

       
}
}
