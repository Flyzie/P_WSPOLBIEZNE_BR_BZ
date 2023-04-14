using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using Model;

namespace ViewModel
{
    public class CommandSharer : ICommand
    {

        private readonly Action handler;
        private bool isEnabled;

        public CommandSharer(Action handler)
        {
            this.handler = handler;
            IsEnabled = true;
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (value != isEnabled)
                {
                    isEnabled = value;
                    if (CanExecuteChanged != null)
                    {
                        CanExecuteChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            handler();
        }
    }


    public class BallInstances : INotifyPropertyChanged
    {
        private Vector2 pos;
        
        public int r { get; set; }

        public BallInstances(float x, float y)
        {
            X = x;
            Y = y;
            r = 40;

        }
        public BallInstances(Vector2 position)
        {
            X = position.X;
            Y = position.Y;
            r = 40;
        }

        public float X
        {
            get { return pos.X; }
            set { pos.X = value; OnPropertyChanged(); }
        }
        public float Y
        {
            get { return pos.Y; }
            set { pos.Y = value; OnPropertyChanged(); }
        }

        public BallInstances()
        {
            X = 0;
            Y = 0;
            r = 40;
        }

        public void ChangePosition(Vector2 position)
        {
            this.X = position.X;
            this.Y = position.Y;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }

 
    
}

