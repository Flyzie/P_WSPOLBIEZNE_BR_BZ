using System;
using System.Collections.Generic;
using System.Text;
using Data;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Logic
{

    /// <summary>
    /// Ball logic zajmuje się tworzeniem kulek i ich zarządzaniem w danej scenie
    /// </summary>
    public class BallLogic : LogicAPI
    {
        // Zmienna przechowująca liczbe kul istniejących w scenie
        public BallCount Balls { get; set; }

        //Zmienna przechowująca parametry sceny
        public Scene Scene { get; set; }

        //Funkcja pozwalająca zatrzymać symulacje
        public CancellationTokenSource CancelSimulationSource { get; private set; }

        private bool _started = false;

        //Konstruktor inicjalizujący kulki, scene i warunek stopu
        public BallLogic(double w, double h)
        {
            Scene = new Scene(w, h);
            Balls = new BallCount();
            CancelSimulationSource = new CancellationTokenSource();
        }

        //Funkcja wywoływana gdy kulka zmieni swoją pozycje
        protected override void onPositionChange(BallMovement args)
        {
            base.onPositionChange(args);
        }

        //Funkcja zwraca ilość kulek w scenie
        public override int getBallCount()
        {
            return Balls.GetBallCount();
        }

        //funkcja tworząca kule o ustalonym promieniu (w sumie nie potrzebna bo zastępuje ją addBall)
        public void createBall(double r)
        {
            var rng = new Random();
            var x = ((double)rng.NextDouble() * (Scene.Length - (2 * r)) + r);
            var y = ((double)rng.NextDouble() * (Scene.Height - (2 * r)) + r);
            var vx = (rng.NextDouble() - 0.5) * 20;
            var vy = (rng.NextDouble() - 0.5) * 20;
            this.Balls.AddBall(new Ball(new Vector2((float)x, (float)y), r, new Vector2((float)vx, (float)vy)));
        }


        //Funkcja dodająca do sceny określoną ilość kuli w losowej pozycji, o ustalonej wielkości i losowej prędkości.
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

        //Funkcja zwracająca Kulke o danym indeksie
        public override Ball getBall(int index)
        {
            return Balls.GetBall(index);
        }

        //Funkcja poruszająca piłką na podstawie jej prędkości i sprawdza kolizje ze ścianą
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

        //Funkcja uruchamiająca symulacje i umorzliwiająca ruszanie się kulek
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

        //Funkcja zatrzymująca symulacje
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
