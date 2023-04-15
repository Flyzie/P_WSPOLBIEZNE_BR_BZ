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
        private readonly Action handler; // pole do przechowywania referencji do metody obsługi, która zostanie wywołana w metodzie Execute()
        private bool isEnabled; // pole do przechowywania informacji o tym, czy polecenie jest aktywne

        public CommandSharer(Action handler) // konstruktor przyjmujący metodę obsługi jako parametr
        {
            this.handler = handler; // przypisanie przekazanej metody do pola handler
            IsEnabled = true; // domyślnie polecenie jest aktywne
        }

        public bool IsEnabled // właściwość służąca do ustawiania i pobierania informacji o tym, czy polecenie jest aktywne
        {
            get { return isEnabled; }
            set
            {
                if (value != isEnabled) // sprawdzenie, czy wartość się zmieniła
                {
                    isEnabled = value; // ustawienie nowej wartości
                    if (CanExecuteChanged != null) // sprawdzenie, czy istnieją subskrybenci zdarzenia CanExecuteChanged
                    {
                        CanExecuteChanged(this, EventArgs.Empty); // wywołanie zdarzenia CanExecuteChanged
                    }
                }
            }
        }

        public bool CanExecute(object parameter) // metoda sprawdzająca, czy można wykonać polecenie
        {
            return IsEnabled; // zwrócenie wartości właściwości IsEnabled
        }

        public event EventHandler CanExecuteChanged; // zdarzenie wywoływane, gdy zmienia się możliwość wykonania polecenia

        public void Execute(object parameter) // metoda wykonująca polecenie
        {
            handler(); // wywołanie metody obsługi przekazanej w konstruktorze
        }
    }



    public class BallInstances : INotifyPropertyChanged   //klasa reprezentująca pojedynczą piłkę, która może być wyświetlana na ekranie. 
    {
        private Vector2 pos;                              //Implementuje interfejs INotifyPropertyChanged, który umożliwia powiadomienie widoku o zmianach w obiektach tej klasy.

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
        public void ChangePosition(Vector2 position)
        {
            this.X = position.X;
            this.Y = position.Y;
        }

        public float Y
        {
            get { return pos.Y; }
            set { pos.Y = value; OnPropertyChanged(); }
        }

        public float X
        {
            get { return pos.X; }
            set { pos.X = value; OnPropertyChanged(); }
        }

        public BallInstances()
        {
            X = 0;
            Y = 0;
            r = 40;
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }

    public class ViewModelClass : INotifyPropertyChanged   //klasa odpowiadająca za pośrednictwo między modelem a view oraz za implementacje interfejsu
    {
        private ModelClass model;
        public AsyncObservableCollection<BallInstances> Circles { get; set; }

        public int BallsCount
        {
            get { return model.GetBallCount(); }
            set
            {
                if (value >= 0)
                {
                    model.SetBallNumber(value);
                    OnPropertyChanged();
                }
            }
        }

        public ICommand BeginSceneButton { get; }
        public ICommand StopSceneButton { get; }
        public ICommand AddBallNumberButton { get; }
        public ICommand RemoveBallNumberButton { get; }


        public ViewModelClass()
        {
            // Inicjalizuj kolekcję Circles jako nową kolekcję typu AsyncObservableCollection<BallInstances>
            Circles = new AsyncObservableCollection<BallInstances>();

            // Tworzy nowy obiekt klasy ModelClass i przypisuje go do pola model
            model = new ModelClass();

            // Ustawia domyślną liczbę piłek na 3
            BallsCount = 3;

            // Tworzy nowy obiekt klasy CommandSharer dla przycisku dodawania piłek
            // i przypisuje mu metodę anonimową, która zwiększa wartość BallsCount o 1
            AddBallNumberButton = new CommandSharer(() =>
            {
                BallsCount += 1;
            });

            // Tworzy nowy obiekt klasy CommandSharer dla przycisku usuwania piłek
            // i przypisuje mu metodę anonimową, która zmniejsza wartość BallsCount o 1
            RemoveBallNumberButton = new CommandSharer(() =>
            {
                BallsCount -= 1;
            });

            // Tworzy nowy obiekt klasy CommandSharer dla przycisku rozpoczęcia sceny
            // i przypisuje mu metodę anonimową, która ustawia liczbę piłek na wartość BallsCount,
            // tworzy piłki i zaczyna program ModelClass. Dodaje również obsługę zdarzenia BallPositionChange
            // i aktualizuje pozycję piłek w kolekcji Circles, gdy ModelClass zgłasza zdarzenie.
            BeginSceneButton = new CommandSharer(() =>
            {
                model.SetBallNumber(BallsCount);

                for (int i = 0; i < BallsCount; i++)
                {
                    Circles.Add(new BallInstances());
                }

                model.BallPositionChange += (sender, argv) =>
                {
                    if (Circles.Count > 0)
                        Circles[argv.Id].ChangePosition(argv.Position);
                };

                model.StartProgram();
            });

            // Tworzy nowy obiekt klasy CommandSharer dla przycisku zatrzymania sceny
            // i przypisuje mu metodę anonimową, która zatrzymuje program ModelClass,
            // usuwa piłki z kolekcji Circles i ustawia liczbę piłek na wartość BallsCount.
            StopSceneButton = new CommandSharer(() =>
            {
                model.StopProgram();
                Circles.Clear();
                model.SetBallNumber(BallsCount);
            });
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }

    public class AsyncObservableCollection<T> : ObservableCollection<T>    //klasa dziedzicząca po klasie ObservableCollection i umożliwiająca asynchroniczne powiadamianie o zmianach w kolekcji.
    {
        private SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        public AsyncObservableCollection()
        {
        }

        public AsyncObservableCollection(IEnumerable<T> list)
            : base(list)
        {
        }


        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            // Sprawdź, czy aktualny kontekst synchronizacji jest tym samym, co kontekst synchronizacji, który został przekazany w konstruktorze
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                // Wywołaj zdarzenie PropertyChanged w bieżącym wątku
                RaisePropertyChanged(e);
            }
            else
            {
                // Wywołaj zdarzenie PropertyChanged na wątku, na którym została utworzona kolekcja
                _synchronizationContext.Send(RaisePropertyChanged, e);
            }
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            // Sprawdź, czy aktualny kontekst synchronizacji jest tym samym, co kontekst synchronizacji, który został przekazany w konstruktorze
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                // Wywołaj zdarzenie CollectionChanged w bieżącym wątku
                RaiseCollectionChanged(e);
            }
            else
            {
                // Wywołaj zdarzenie CollectionChanged na wątku, na którym została utworzona kolekcja
                _synchronizationContext.Send(RaiseCollectionChanged, e);
            }
        }

        private void RaisePropertyChanged(object param)
        {
            // Jesteśmy na wątku tworzącym, więc wywołaj bezpośrednio implementację bazową
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }
        private void RaiseCollectionChanged(object param)
        {
            // Jesteśmy na wątku tworzącym, więc wywołaj bezpośrednio implementację bazową
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }




    }
    
}

