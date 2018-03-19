using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using FontAwesome.WPF;

namespace KMA.Group2.Project1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SignInWindow _signInView;
        private SignUpView _signUpView;
        private ImageAwesome _loader;
        private UserListView _userListView;
        public MainWindow()
        {
            InitializeComponent();
            ShowSignInView();
        }

        protected override void OnClosed(EventArgs e)
        {
            (_userListView.DataContext as UserListViewModel).StopThread();
        }

        private void ShowSignInView()
        {
            HamburgerButtonGrid.Visibility = Visibility.Collapsed;

            if (_signInView == null)
            {
                _signInView = new SignInWindow(ShowMainView, ShowSignUpView, Close, ShowLoader);
            }

            ShowView(_signInView);
        }

        private void ShowSignUpView()
        {

            if (_signUpView == null)
            {
                _signUpView = new SignUpView(ShowMainView, ShowSignInView, ShowLoader);
            }

            ShowView(_signUpView);
        }

        private void ShowMainView()
        {
            HamburgerButtonGrid.Visibility = Visibility.Visible;

            if (_userListView == null)
            {
                _userListView = new UserListView(ShowLoader);
            }
            ShowView(_userListView);
        }

        private void ShowView(UIElement element)
        {
            MainGrid.Children.Clear();    
            MainGrid.Children.Add(element);
        }

        public void ShowLoader(bool isShow)
        {
            LoaderHelper.OnRequestLoader(MainGrid, ref _loader, isShow);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            DBAdapter.SaveData();
            base.OnClosing(e);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ExpandedGrid.Visibility = ExpandedGrid.Visibility == Visibility.Collapsed
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}
