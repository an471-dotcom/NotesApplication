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
using Notes_Library.DataManager;
using System.ServiceModel.Channels;
using Windows.UI.Popups;
using Notes_Application.ViewModel;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Notes_Application.Dialogs
{
    public sealed partial class SignUpDialog : ContentDialog,ISignUpView
    {
        public SignUpViewModel SignUpVm;
        public SignUpDialog()
        {
            this.InitializeComponent();
            SignUpVm = new SignUpViewModel();
           
        }



        private void ShowPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ShowPasswordCheckBox.IsChecked == true)
            {
                SignUpPasswordBox.PasswordRevealMode = PasswordRevealMode.Visible;
            }
            else
            {
                SignUpPasswordBox.PasswordRevealMode = PasswordRevealMode.Hidden;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        
        public void CloseDialog()
        {
            this.Hide();
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            SignUpVm.SignUpView = this;
        }
    }

    public interface ISignUpView
    {
        void CloseDialog();
    }


}
