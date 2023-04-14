using Logic;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Model
{
    public class OnPositionChangeUiAdapterEventArgs : EventArgs
    {
        public readonly Vector2 Position;
        public readonly int Id;

        public OnPositionChangeUiAdapterEventArgs(Vector2 position, int id)
        {
            this.Position = position;
            Id = id;
        }
    }

    public class ModelClass
    {
        public double SceneLength;
        public double SceneHeight;
        public int NumberOfBalls;
        public LogicAPI? logic;
        public event EventHandler<OnPositionChangeUiAdapterEventArgs>? BallPositionChange;


        public ModelClass()
        {
            SceneLength = 650;
            SceneHeight = 400;
            logic = new BallLogic(SceneLength, SceneHeight);
            NumberOfBalls = 0;

            logic.PositionChangedEvent += (sender, b) =>
            {
                BallPositionChange?.Invoke(this, new OnPositionChangeUiAdapterEventArgs(b.GetBall().Position, b.id));
            };
        }
        public void StartProgram()
        {
            logic.addBall(NumberOfBalls);
            logic.ProgramStart();
        }

        public void StopProgram()
        {
            logic.ProgramStop();
            logic = new BallLogic(SceneLength, SceneHeight);
            logic.PositionChangedEvent += (sender, b) =>
            {
                BallPositionChange?.Invoke(this, new OnPositionChangeUiAdapterEventArgs(b.GetBall().Position, b.id));
            };
        }

        public void SetBallNumber(int amount)
        {
            NumberOfBalls = amount;
        }

        public int GetBallCount()
        {
            return NumberOfBalls;
        }

        public void OnBallPositionChange(OnPositionChangeUiAdapterEventArgs args)
        {
            BallPositionChange?.Invoke(this, args);
        }
    }
}
