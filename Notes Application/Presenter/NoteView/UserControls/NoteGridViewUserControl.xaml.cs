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
using Notes_Library.Models;
using System.ServiceModel.Channels;
using Notes_Library.DataManager;
using System.Threading.Tasks;
using Notes_Application.Dialogs;
using Notes_Application.Converters;
using Notes_Application.View;
using Windows.Storage;
using System.Reflection;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Transactions;
using Notes_Application.Utility_Classes;
using Notes_Library.Model.BussinessObjects;
using Windows.UI;
using Windows.UI.Popups;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Notes_Application.ViewModel.Models;
using Notes_Application.ViewModel;


// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Notes_Application.UserControls
{


    public sealed partial class NoteGridViewUserControl : UserControl, INotifyPropertyChanged
    {


        public NotesVobj Note
        {
            get
            {
                return this.DataContext as NotesVobj;
            }
        }



        public NoteGridViewUserControl()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }



        private bool _isSharedNote;
        public bool IsSharedNote
        {
            get => _isSharedNote;
            set
            {
                _isSharedNote = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSharedNote"));
            }

        }


        public event PropertyChangedEventHandler PropertyChanged;



        public event Action<NotesVobj> ToggleFavorite;
        
        public event Action<int> DeleteNote;

        public event Action<int> UnshareNote;

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleFavorite?.Invoke(Note);
        }

        private void StackPanel_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            
            OptionButton.Visibility = Visibility.Visible;

        }

        private void StackPanel_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var flyout = FlyoutBase.GetAttachedFlyout(OptionButton);
            if (flyout == null)
            {
                return;
            }

            if (flyout.IsOpen)
            {
                OptionButton.Visibility = Visibility.Visible;
            }
            else
            {
                OptionButton.Visibility = Visibility.Collapsed;
            }

        }

        private void DeleteItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            DeleteNote?.Invoke(Note.NoteId);
        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var flyout = FlyoutBase.GetAttachedFlyout(OptionButton);
            if (flyout != null)
            {
                flyout.Closing += Flyout_Closing;
            }
           
           
            

        }

        private void NoteGridViewUserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var columns = Math.Ceiling(ActualWidth / 300);
            NoteGrid.Width = Window.Current.Bounds.Width / columns;
        }

        private void Flyout_Closing(FlyoutBase sender, FlyoutBaseClosingEventArgs args)
        {
            OptionButton.Visibility = Visibility.Collapsed;
        }

        private void OptionButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            var flyout = FlyoutBase.GetAttachedFlyout(button);
            flyout.ShowAt(button);
        }

        private void UnShareNote_Click(object sender, RoutedEventArgs e)
        {
            UnshareNote?.Invoke(Note.NoteId);
        }

       

    }
}
