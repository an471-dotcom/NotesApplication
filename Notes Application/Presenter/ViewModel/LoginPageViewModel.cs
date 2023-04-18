using Notes_Application.Utility_Classes;
using Notes_Application.View;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace Notes_Application.ViewModel
{
    public class LoginPageViewModel
    {

       public IView LoginPage { get; set; }


        public void GetUser(string email, string password)
        {
            new LoginUserUseCase(new LoginUserRequest
            {
                Password = password,
                Email = email,
            }, new LoginUserPresenterCallback(this)).Execute();
        }

        public void OnLoginSuccessfull(LoginUserResponse result)
        {
            UserManager.CurrentUser = result.User;
            LoginPage.OnUserFetched();
        }

        public class LoginUserPresenterCallback : IPresenterCallback<LoginUserResponse>
        {
            private LoginPageViewModel _viewModel;
            public LoginUserPresenterCallback(LoginPageViewModel viewModel)
            {
                _viewModel = viewModel;
            }
            public async void OnError(Exception e)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                async () =>
                {
                    await new MessageDialog(e.Message).ShowAsync();

                });
            }

            public async void OnSuccess(LoginUserResponse result)
            {
                await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     _viewModel.OnLoginSuccessfull(result);
                 });
            }
        }

    }

}
