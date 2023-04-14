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

        public override int getBallCount()
        {
            return Balls.GetBallCount();
        }

        public void createBall(double r)
        {
            var rng = new Random();
            var x = ((double)rng.NextDouble() * (Scene.Length - (2 * r)) + r);
            var y = ((double)rng.NextDouble() * (Scene.Height - (2 * r)) + r);
            var vx = (rng.NextDouble() - 0.5) * 20;
            var vy = (rng.NextDouble() - 0.5) * 20;
            this.Balls.AddBall(new Ball(new Vector2((float)x, (float)y), r, new Vector2((float)vx, (float)vy)));
        }



        public override void addBall(int numberOfBalls)
        {
            var rng = new Random();
            for (int i = 0; i < numberOfBalls; i++)
            {

                var r = (rng.NextDouble() * 20) + 20;
                var x = (rng.NextDouble() * (Scene.Length - (2 * r)) + r);
                var y = (rng.NextDouble() * (Scene.Height - (2 * r)) + r);
                var vx = (rng.NextDouble() - 0.5) * 20;
                var vy = (rng.NextDouble() - 0.5) * 20;
                this.Balls.AddBall(new Ball(new Vector2((float)x, (float)y), r, new Vector2((float)vx, (float)vy)));
            }

        }

        public override Ball getBall(int index)
        {
            return Balls.GetBall(index);
        }

        public void MoveBall(Ball b)
        {
            Vector2 featurePosition = b.Position + b.Speed;
            if (featurePosition.X - b.Radius < 0 && featurePosition.X + b.Radius > Scene.Length)
            {
                b.Speed = b.Speed * new Vector2(1, -1);
            }

            if (featurePosition.Y - b.Radius < 0 && featurePosition.Y + b.Radius > Scene.Height)
            {
                b.Speed = b.Speed * new Vector2(-1, 1);
            }

            b.Position = b.Position + b.Speed;
        }


        public override void ProgramStart()
        {
            if (CancelSimulationSource.IsCancellationRequested) return;

            CancelSimulationSource = new CancellationTokenSource();

            for (var i = 0; i < Balls.GetBallCount(); i++)
            {
                var ball = new BallMovement(Balls.GetBall(i), i, this, Scene.Length, Scene.Height);
                ball.PositionChange += (_, args) => onPositionChange(ball);
                Task.Factory.StartNew(ball.Move, CancelSimulationSource.Token);
            }

        }

        public override void ProgramStop()
        {
            this.CancelSimulationSource.Cancel();
        }

        public void makeBall(double radius)
        {
            var random = new Random();
            var x = ((double)random.NextDouble() * (Scene.Length - (2 * radius)) + 1 + radius);
            var y = ((double)random.NextDouble() * (Scene.Height - (2 * radius)) + 1 + radius);
        }

        public override void removeBall(int numberOfBalls)
        {

        }

    }
}
