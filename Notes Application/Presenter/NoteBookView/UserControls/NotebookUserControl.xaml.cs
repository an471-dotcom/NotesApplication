using Notes_Application.Presenter.ViewModel.Models;
using Notes_Application.Utility_Classes;
using Notes_Application.ViewModel;
using Notes_Application.ViewModel.Models;
using Notes_Library.DataManager;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;

using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static SQLite.SQLite3;


namespace Notes_Application.UserControls
{
    
    public sealed partial class NotebookUserControl : UserControl
    {

        private NoteBookUserControlViewModel _viewModel;
        public NoteBookVobj Notebook
        {
            get
            {
                return this.DataContext as NoteBookVobj;
            }

        }
        public NotebookUserControl()
        {
            this.InitializeComponent();
            _viewModel = new NoteBookUserControlViewModel();
            this.DataContextChanged += (s, e) => _viewModel.SetNotebookData(Notebook);
            Handler = new DoubleTappedEventHandler(RenameTextBox_DoubleTapped);
        }

        public DoubleTappedEventHandler Handler;

        public event Action<NoteBookVobj> ChangeNoteBookCover;
       
        public event Action<int> DeleteNotebook;


       

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            var flyout = OptionButton.Flyout;
            flyout.Closing += Flyout_Closing;
            RenameTextBox.AddHandler(DoubleTappedEvent, Handler, true);
        }

        private void Canvas_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            OptionButton.Visibility = Visibility.Visible;
        }

        private void Canvas_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var flyout = OptionButton.Flyout;
            if (flyout.IsOpen)
            {
                OptionButton.Visibility = Visibility.Visible;
            }
            else
            {
                OptionButton.Visibility = Visibility.Collapsed;
            }

        }

        private void Flyout_Closing(FlyoutBase sender, FlyoutBaseClosingEventArgs args)
        {
           OptionButton.Visibility = Visibility.Collapsed;
        }
       
        private void RenameTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {  
                _viewModel.RenameNotebook();
              
            }
        }

        private void RenameItem_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SetNoteBookName();
            RenameTextBox.Focus(FocusState.Programmatic);
            RenameTextBox.SelectAll();
        }

      
        private void RenameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.ResetNotebookName();

        }

        
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            DeleteNotebook?.Invoke(_viewModel.Notebook.NotebookId);
        }

        
        

       

        private void RenameTextBox_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            _viewModel.SetNoteBookName();
            RenameTextBox.Focus(FocusState.Programmatic);
            RenameTextBox.SelectAll();

        }

        private void ChangeNotebookCoverItemClick(object sender, RoutedEventArgs e)
        {
            ChangeNoteBookCover?.Invoke(_viewModel.Notebook);
        }
    }
}
