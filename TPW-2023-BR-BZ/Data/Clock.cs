using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Data
{
    public class Clock
    {
        public delegate void CallMe();

        private CallMe Callback { get; }
        private int CycleLength { get; }

        public bool Running { get; private set; } = false;

        public Clock(CallMe callback, int msDelay)
        {
            Callback = callback;
            CycleLength = msDelay;
        }

        private void Timer()
        {
            while (Running)
            {
                var timer = Stopwatch.StartNew();
                Callback.Invoke();
                timer.Stop();
                int remaining = (CycleLength - (int)timer.ElapsedMilliseconds);
                if (remaining > 0)
                {
                    Thread.Sleep(remaining);
                }
            }
        }

        public void Start()
        {
            if (!Running)
            {
                Running = true;
                new Thread(Timer).Start();
            }
        }
    }
}
