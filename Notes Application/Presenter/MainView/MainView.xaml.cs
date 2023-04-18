using Notes_Application.Dialogs;
using Notes_Application.UserControls;
using Notes_Application.Utility_Classes;
using Notes_Application.ViewModel;
using Notes_Application.ViewModel.Models;
using Notes_Library.DataManager;
using Notes_Library.Domain.UseCase;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Notes_Application.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
   
    
    public sealed partial class MainView : Page
    {

        private MainPageViewModel _viewModel;

        public MainView()
        {
            this.InitializeComponent();
            _viewModel = new MainPageViewModel();
            NavView.SelectedItem = NoteItem;
            
            
        }
        
        

       

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["email"] = null;
            localSettings.Values["password"] = null;
            localSettings.Values["themeSetting"] = null;
            UserManager.CurrentUser = null;
            this.Frame.Navigate(typeof(LoginView));
        }

       
        private void UserDetailButton_Click(object sender, RoutedEventArgs e)
        {

            
            UserNameTextblock.Text = UserManager.CurrentUser.UserName;
            UserEmailTextBlock.Text = UserManager.CurrentUser.Email;

        }

        private  void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            
            var tag = ((NavigationViewItem)args.SelectedItemContainer).Tag.ToString();
        
            switch (tag)
            {
                case "Notebook":
                    contentFrame.Navigate(typeof(NoteBookView),null, new SuppressNavigationTransitionInfo());
                    break;
                case "Note":
                    contentFrame.Navigate(typeof(NotesView),NoteType.AllNotes, new SuppressNavigationTransitionInfo());
                    break;
                case "Favorites":
                    contentFrame.Navigate(typeof(NotesView), NoteType.FavoriteNotes, new SuppressNavigationTransitionInfo());
                    break;
                case "Shared":
                    contentFrame.Navigate(typeof(NotesView),NoteType.SharedNotes, new SuppressNavigationTransitionInfo());
                    break;
            }

        }

       

        

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

       

       
       
        private void SwitchThemeButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;

            var page = UtilityClass.FindParent<MainPage>(this);
            if (page != null)
            {
                if (page.RequestedTheme == ElementTheme.Dark || page.RequestedTheme == ElementTheme.Default)
                {

                    page.RequestedTheme = ElementTheme.Light;
                    titleBar.ButtonBackgroundColor = Colors.Transparent;
                    titleBar.ButtonForegroundColor = Colors.Black;
                    titleBar.ButtonHoverBackgroundColor = Color.FromArgb(20, 0, 0, 0);
                    titleBar.ButtonHoverForegroundColor = Colors.Black;
                    ApplicationData.Current.LocalSettings.Values["themeSetting"] = 1;
                }
                else if (page.RequestedTheme == ElementTheme.Light)
                {

                    page.RequestedTheme = ElementTheme.Dark;
                    titleBar.ButtonBackgroundColor = Colors.Transparent;
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.ButtonHoverBackgroundColor = Color.FromArgb(40, 255, 255, 255);
                    titleBar.ButtonHoverForegroundColor = Colors.White;
                    ApplicationData.Current.LocalSettings.Values["themeSetting"] = 2;
                }
            }
            var flyout = UserDetailButton.Flyout;
            flyout.Hide();
        }

        private void MyColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
           
            _viewModel.UpdateDefaultNoteColor(args.NewColor.ToString());
        }
    }
}



