using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public abstract class LogicAPI
    {

        public event EventHandler<BallMovement>? PositionChangedEvent;

        public abstract Ball getBall(int index);

        public abstract void addBall(int numberOfBalls);

        public abstract void removeBall(int numberOfBalls);

        public abstract int getBallCount();

        public abstract void ProgramStart();

        public abstract void ProgramStop();

        protected virtual void onPositionChange(BallMovement b)
        {
            PositionChangedEvent?.Invoke(this, b);
        }


    }
}
