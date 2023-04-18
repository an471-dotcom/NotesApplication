using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Notes_Application.Dialogs;
using System.Threading.Tasks;
using Notes_Library.DataManager;
using static SQLite.SQLite3;
using Windows.UI.Popups;
using Notes_Application.Utility_Classes;
using Notes_Library.Domain.UseCase;
using Notes_Application.ViewModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Notes_Application.View
{
  
    public sealed partial class LoginView : Page,IView
    {
        private LoginPageViewModel _viewModel;
        public LoginView()
        {
            this.InitializeComponent();
            _viewModel = new LoginPageViewModel();
        }

        private async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            

            SignInDialog signInView = new SignInDialog();
            var theme = UtilityClass.FindParent<MainPage>(this).RequestedTheme;
            signInView.RequestedTheme = theme;
            await signInView.ShowAsync();
            if (signInView.ViewModel.LoginResult)
            {
                this.Frame.Navigate(typeof(MainView));

            }
            else
            {
     
                LoginPanel.Visibility = Visibility.Visible;
            }
        }

        private async void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            SignUpDialog signUpView = new SignUpDialog();
            var theme = UtilityClass.FindParent<MainPage>(this).RequestedTheme;
            signUpView.RequestedTheme = theme;
            await signUpView.ShowAsync();

            if (signUpView.SignUpVm.SignUpResult)
            {
                this.Frame?.Navigate(typeof(MainView));
            }
            else
            {
               
                LoginPanel.Visibility = Visibility.Visible;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoginPage = this;
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values["email"] == null)
            {
                LoginPanel.Visibility = Visibility.Visible;
            }
            else
            {
                _viewModel.GetUser(localSettings.Values["email"].ToString(), localSettings.Values["password"].ToString());
            }

        }

        void IView.OnUserFetched()
        {
            this.Frame.Navigate(typeof(MainView));
        }
    }

    public interface IView
    {
        void OnUserFetched();
    }
}
