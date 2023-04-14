using System;
using System.Collections.Generic;
using System.Text;
using Data;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;


namespace Logic
{
    public class BallMovement
    {
        private Ball ball;
        public int id;
        private BallLogic owner;

        public double Xend;
        public double Yend;

        public event EventHandler<BallMovement>? PositionChange;

        //Konstruktor
        public BallMovement(Ball b, int id, BallLogic owner, double Xend, double Yend)
        {
            this.ball = b;
            this.id = id;
            this.owner = owner;
            this.Xend = Xend;
            this.Yend = Yend;



        }

        public Ball GetBall()
        {
            return ball;
        }

        //Funkcja definiująca zachowanie kulki, aktualizuje jej pozycje (WYkrywa kolizje ze ścianą ????).
        public async void Move()
        {
            while (!owner.CancelSimulationSource.Token.IsCancellationRequested)
            {
                Vector2 featurePosition = this.ball.Position + this.ball.Speed;

                if (featurePosition.X < 0 || featurePosition.X + this.ball.Radius > this.Xend)
                {
                    this.ball.Speed = this.ball.Speed * new Vector2(-1, 1);
                }

                if (featurePosition.Y < 0 || featurePosition.Y + this.ball.Radius > this.Yend)
                {
                    this.ball.Speed = this.ball.Speed * new Vector2(1, -1);
                }

                this.ball.Position = this.ball.Position + this.ball.Speed;
                PositionChange?.Invoke(this, this);
                await Task.Delay(20, owner.CancelSimulationSource.Token).ContinueWith(_ => { });
            }
        }


    }


}

