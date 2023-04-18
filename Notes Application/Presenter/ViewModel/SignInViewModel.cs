using Notes_Application.Utility_Classes;
using Notes_Library.DataManager;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SQLite.SQLite3;
using Windows.UI.Xaml;
using System.ComponentModel;
using System.Threading;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Notes_Application.Dialogs;
using Windows.ApplicationModel.Resources;
using Notes_Library.Exceptions;
using System.Text.RegularExpressions;

namespace Notes_Application.ViewModel
{
    public class SignInViewModel : INotifyPropertyChanged
    {
        private string _passwordText;
        public string PasswordText
        {
            get => _passwordText;
            set
            {
                _passwordText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PasswordText)));

            }
        }

        private string _emailText;
        public string EmailText
        {
            get => _emailText;
            set
            {
                _emailText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EmailText)));
            }
        }

        private string _errorText;

        public string ErrorText
        {
            get => _errorText;
            set
            {
                _errorText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ErrorText)));
            }
        }
        public bool LoginResult { set; get; } = false;

        public ISignInView SignInView { get; set; }

        Windows.Storage.ApplicationDataContainer localSettings;

        public event PropertyChangedEventHandler PropertyChanged;


        public SignInViewModel()
        {
           
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        }
        public void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            if (string.IsNullOrEmpty(EmailText) || string.IsNullOrEmpty(PasswordText))
            {
                ErrorText = resourceLoader.GetString("FillAllFields");
              
            }
            else if (!regex.Match(EmailText).Success)
            {
                ErrorText = resourceLoader.GetString("ValidEmailAddress");
       
            }
            else
            {
                ErrorText = "";
                var email = EmailText.ToLower();
                var password = PasswordText.ToLower();
                new LoginUserUseCase(new LoginUserRequest
                {
                    Email = email,
                    Password = password,
                }, new LoginUserPresenterCallback(this)).Execute();

            }

        }

        public class LoginUserPresenterCallback : IPresenterCallback<LoginUserResponse>
        {
            private SignInViewModel _viewModel;

            public LoginUserPresenterCallback(SignInViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public async void OnError(Exception e)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

                    if (e is UserNotFoundException)
                    {
                        _viewModel.ErrorText = resourceLoader.GetString("UserNotFound");
                    }
                    else if(e is WrongPasswordException)
                    {
                        _viewModel.ErrorText = resourceLoader.GetString("WrongPassword");
                    }
                    
                   

                });
            }

            public async void OnSuccess(LoginUserResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {
                   _viewModel.localSettings.Values["email"] = result.User.Email;
                   _viewModel.localSettings.Values["password"] = result.User.Password;
                   UserManager.CurrentUser = result.User;
                    _viewModel.SignInView.CloseDialog();
                   _viewModel.LoginResult = true;
               });
            }
        }

    }
}
