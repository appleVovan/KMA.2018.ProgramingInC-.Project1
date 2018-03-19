using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using KMA.Group2.Project1.Annotations;

namespace KMA.Group2.Project1
{
    class UserListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<User> _users;
        private Action<bool> _showLoaderAction;
        //private Thread _workingThread;
        //private CancellationToken _token;
        //private CancellationTokenSource _tokenSource;
        //private BackgroundWorker _backgroundWorker;
        

        public ObservableCollection<User> Users
        {
            get => _users;
            private set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        internal UserListViewModel(Action<bool> shwoLoderAction)
        {
            _users = new ObservableCollection<User>(DBAdapter.Users);
            _showLoaderAction = shwoLoderAction;
            //_tokenSource = new CancellationTokenSource();
            //_token = _tokenSource.Token;
            //_workingThread = new Thread(UpdateUserListWorker);
            //_workingThread.Start();

            //_backgroundWorker = new BackgroundWorker();
            //_backgroundWorker.WorkerSupportsCancellation = true;
            //_backgroundWorker.WorkerReportsProgress = true;
            //_backgroundWorker.DoWork += BackgroundWorkerOnDoWork;
            //_backgroundWorker.ProgressChanged += BackgroundWorkerOnProgressChanged;
            //_backgroundWorker.RunWorkerAsync();

            var progress = new Progress<List<User>>(users =>
            {
                _showLoaderAction.Invoke(true);
                Thread.Sleep(2000);
                Users = new ObservableCollection<User>(users);
                _showLoaderAction.Invoke(false);
            });
            Task.Factory.StartNew(() => UpdateUserListWorker(progress), TaskCreationOptions.LongRunning);
        }

        private void UpdateUserListWorker(IProgress<List<User>> progress)
        {
            int i = 0;
            while (true)
            {
                var users = _users.ToList();
                users.Add(new User("Login" + i, "Password" + i, "FirtsNAme" + i, "LastNAme" + i, "Email" + i, DateTime.Today));
                progress.Report(users);
                Thread.Sleep(5000);
            }
        }

        //private void BackgroundWorkerOnProgressChanged(object sender, ProgressChangedEventArgs progressChangedEventArgs)
        //{
        //    _showLoaderAction.Invoke(true);
        //    Users = new ObservableCollection<User>((List<User>)progressChangedEventArgs.UserState);
        //    Thread.Sleep(2000);
        //    _showLoaderAction.Invoke(false);
        //}

        private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var worker = (BackgroundWorker) sender;
            int i = 0;
            while (!worker.CancellationPending)
            {
                var users = _users.ToList();
                users.Add(new User("Login" + i, "Password" + i, "FirstNAme" + i, "LastNAme" + i, "Email" + i,
                    DateTime.Today));
                worker.ReportProgress(10, users);
                for (int j = 0; j < 10; j++)
                {
                    Thread.Sleep(500);
                    if (worker.CancellationPending)
                        break;
                }
                if (worker.CancellationPending)
                    break;
                i++;
            }
        }

        //private void UpdateUserListWorker()
        //{
        //    int i = 0;
        //    while (!_token.IsCancellationRequested)
        //    {
        //        var users = _users.ToList();
        //        users.Add(new User("Login" + i, "Password" + i, "FirstNAme" + i, "LastNAme" + i, "Email" + i,
        //            DateTime.Today));
        //        Application.Current.Dispatcher.Invoke(_showLoaderAction, true);
        //        Users = new ObservableCollection<User>(users);
        //        for (int j = 0; j < 5; j++)
        //        {
        //            Thread.Sleep(500);
        //            if (_token.IsCancellationRequested)
        //                break;
        //        }
        //        if (_token.IsCancellationRequested)
        //            break;
        //        Application.Current.Dispatcher.Invoke(_showLoaderAction, false);
        //        for (int j = 0; j < 10; j++)
        //        {
        //            Thread.Sleep(500);
        //            if (_token.IsCancellationRequested)
        //                break;
        //        }
        //        if (_token.IsCancellationRequested)
        //            break;
        //        i++;
        //    }
        //}

        internal void StopThread()
        {
            //_tokenSource.Cancel();
            //_workingThread.Join(2000);
            //_workingThread.Abort();
            //_workingThread = null;
            //_backgroundWorker.CancelAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
