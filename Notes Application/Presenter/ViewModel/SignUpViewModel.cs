using Notes_Application.Dialogs;
using Notes_Application.Utility_Classes;
using Notes_Library.DataManager;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase;
using Notes_Library.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Notes_Application.ViewModel
{
    public class SignUpViewModel : INotifyPropertyChanged
    {

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
        private string _usernameText;
        public string UsernameText
        {
            get => _usernameText;
            set
            {
                _usernameText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UsernameText)));
            }
        }

        public bool SignUpResult { get; set; } = false;

        public ISignUpView SignUpView { get; set; }

        Windows.Storage.ApplicationDataContainer localSettings;

        public event PropertyChangedEventHandler PropertyChanged;
        public SignUpViewModel()
        {
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        }
       


      
        public void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

            if (string.IsNullOrEmpty(EmailText) || string.IsNullOrEmpty(PasswordText))
            {
                ErrorText = resourceLoader.GetString("FillAllFields");

            }
            else if(!regex.Match(EmailText).Success)
            {
               
                ErrorText = resourceLoader.GetString("ValidEmailAddress");
            }
            else
            {
                ErrorText = "";
                var email = EmailText.ToLower();
                var userName = UsernameText;
                var password = PasswordText;

                new AddUserUseCase(new AddUserRequest
                {
                    Email = email,
                    UserName = userName,
                    Password = password,
                    DefaultNoteColor = "#FFF7FF2B",

                }, new AddUserPresenterCallback(this)).Execute();


            }
        }

        private class AddUserPresenterCallback : IPresenterCallback<AddUserResponse>
        {
            private SignUpViewModel _viewModel;

            public AddUserPresenterCallback(SignUpViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public async void OnSuccess(AddUserResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
               () =>
               {


                   _viewModel.SignUpResult = true;
                   UserManager.CurrentUser = result.NewUser;
                   _viewModel.localSettings.Values["email"] = result.NewUser.Email;
                   _viewModel.localSettings.Values["password"] = result.NewUser.Password;
                   _viewModel.SignUpView.CloseDialog();



               });
            }

            public async void OnError(Exception e)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    if (e is DuplicateUserException)
                    { 
                        var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                        _viewModel.ErrorText = resourceLoader.GetString("EmailAlreadyExists");
                       
                    }

                });
            }
        }

    }
}
