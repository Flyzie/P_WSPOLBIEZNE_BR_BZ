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

        protected virtual void onPositionChange(BallMovement b)
        {
            PositionChangedEvent?.Invoke(this, b);
        }
        

    }
}
