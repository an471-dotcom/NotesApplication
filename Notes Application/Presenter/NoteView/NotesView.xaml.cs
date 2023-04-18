using Notes_Application.Dialogs;
using Notes_Application.UserControls;
using Notes_Application.Utility_Classes;
using Notes_Library.DataManager;
using Notes_Library.Domain;
using Notes_Library.Domain.UseCase;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Notes_Application.ViewModel;
using Windows.Devices.Bluetooth.Advertisement;
using Notes_Library.Model.BussinessObjects;
using Notes_Application.ViewModel.Models;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Notes_Application.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>


    public sealed partial class NotesView : Page
    {
        public NotesPageViewModel _viewModel { get; set; }


        private string _currentView;

        public NotesView()
        {
            this.InitializeComponent();
            _viewModel = new NotesPageViewModel();
        }



        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            _viewModel.OnNavigatedTo(e);
            base.OnNavigatedTo(e);
        }

        private void NoteGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Window.Current.Bounds.Width > 800)
            {

                VisualStateManager.GoToState(this, "NoteDetailViewWide", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "NoteDetailViewNarrow", false);
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {


            var viewtype = (string)localSettings.Values["View"];

            if (viewtype == "List")
            {
                _currentView = "List";
                Viewicon.Glyph = "\uF0E2";

                ShowListView();
            }
            else
            {
                _currentView = "Grid";
                Viewicon.Glyph = "\uE14C";
                ShowGridView();
            }


            this.SizeChanged += NotesView_SizeChanged;
            SetToggleButtonIcon();

        }

        private void NotesView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var width = Window.Current.Bounds.Width;
            if (width > 1100)
            {
                if (_currentView == "Grid")
                {
                    if (LayoutStates.CurrentState.Name == "GridViewNarrow")
                    {
                        VisualStateManager.GoToState(this, "GridViewWide", false);
                    }
                    else if(LayoutStates.CurrentState.Name == "NoteDetailViewNarrow")
                    {
                        VisualStateManager.GoToState(this, "NoteDetailViewWide", false);
                    }


                }
                else if (_currentView == "List")
                {
                    VisualStateManager.GoToState(this, "ListViewWide", false);
                }
            }
            else if (width > 600)
            {
                if (_currentView == "List")
                {
                    VisualStateManager.GoToState(this, "ListViewNarrow", false);
                }
                else if (_currentView == "Grid")
                {
                    if (LayoutStates.CurrentState.Name == "GridViewNarrow")
                    {
                        VisualStateManager.GoToState(this, "GridViewWide", false);
                    }
                    else if (LayoutStates.CurrentState.Name == "NoteDetailViewNarrow")
                    {
                        VisualStateManager.GoToState(this, "NoteDetailViewWide", false);
                    }

                }

            }
            else
            {
                if (_currentView == "Grid")
                {
                    if (LayoutStates.CurrentState.Name == "GridViewWide")
                    {
                        VisualStateManager.GoToState(this, "GridViewNarrow", false);
                    }
                    else if (LayoutStates.CurrentState.Name == "NoteDetailViewWide")
                    {
                        VisualStateManager.GoToState(this, "NoteDetailViewNarrow", false);
                    }

                }
                else if (_currentView == "List")
                {
                    VisualStateManager.GoToState(this, "ListViewSmall", false);
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NoteBookView), null, new SuppressNavigationTransitionInfo());
        }



        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            _viewModel?.SortNotes();
        }


        private void TagsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            _viewModel.FilterByTag((e.ClickedItem as Tag));
            var flyout = TagsButton.Flyout;
            flyout.Hide();
        }





        private void ShowListView()
        {
            if (Window.Current.Bounds.Width > 1100)
            {

                VisualStateManager.GoToState(this, "ListViewWide", false);
            }
            else if (Window.Current.Bounds.Width > 600)
            {
                VisualStateManager.GoToState(this, "ListViewNarrow", false);
            }
            else
            {

                VisualStateManager.GoToState(this, "ListViewSmall", false);
            }




        }
        private void ShowGridView()
        {
            if (Window.Current.Bounds.Width > 800)
            {
                VisualStateManager.GoToState(this, "GridViewWide", false);

            }
            else
            {
                VisualStateManager.GoToState(this, "GridViewNarrow", false);
            }

        }

        private void NoteListViewControl_ToggleFavorite(NotesVobj note)
        {
            _viewModel.ToggleFavoriteStatus(note);
        }

        private void NoteListViewControl_DeleteNote(int noteId)
        {
            _viewModel.DeleteNote(noteId);
        }

        private void CreateNoteButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.CreateNote();
        }


        private void NotesList_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SwitchViewButton_Click(object sender, RoutedEventArgs e)
        {

            if (_currentView == "List")
            {
                ShowGridView();
                localSettings.Values["View"] = "Grid";
                Viewicon.Glyph = "\uE14C";
                _currentView = "Grid";
            }
            else if (_currentView == "Grid")
            {
                ShowListView();
                localSettings.Values["View"] = "List";
                Viewicon.Glyph = "\uF0E2";
                _currentView = "List";
            }

        }

        private void NoteDisplayBackButton_Click(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "GridView", false);
        }



        private void NoteListViewDisplayControl_Minimize()
        {
            ShowGridView();
        }

        private void DeleteTagButton_Click(object sender, RoutedEventArgs e)
        {

            var button = (Button)sender;
            var tag = button.DataContext as Tag;
            _viewModel.DeleteTag(tag.Id);
            var flyout = TagsButton.Flyout;
            flyout.Hide();

        }

        private void CloseTagList_Click(object sender, RoutedEventArgs e)
        {
            var index = TagsList.SelectedIndex;
            if (index != -1)
            {
                _viewModel.ClearTagsSelection();
            }
            var flyout = TagsButton.Flyout;
            flyout.Hide();
        }

        private void TagsButton_Click(object sender, RoutedEventArgs e)
        {
            var index = TagsList.SelectedIndex;
            if (index != -1)
                return;
            _viewModel.GetAllNoteTags();

        }

        private void GridViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var columns = Math.Ceiling(ActualWidth / 500);
            ((ItemsWrapGrid)NotesGridView.ItemsPanelRoot).ItemWidth = e.NewSize.Width / columns;
        }

        private void NotesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Window.Current.Bounds.Width < 800)
            {

                VisualStateManager.GoToState(this, "ListDetailedView", false);
            }

        }

        private void TogglePane_Click(object sender, RoutedEventArgs e)
        {

            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            SetToggleButtonIcon();
        }
        public void SetToggleButtonIcon()
        {
            if (MySplitView.IsPaneOpen)
            {
                TogglePaneButtonIcon.Glyph = "\uE89F";
            }
            else
            {
                TogglePaneButtonIcon.Glyph = "\uE8A0";
            }
        }
        private void NoteDisplayUserControl_GoBack()
        {
            if (_currentView == "List")
            {
                ShowListView();
            }
            else if (_currentView == "Grid")
            {
                ShowGridView();
            }
        }

        private void MySplitView_PaneClosing(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            TogglePaneButtonIcon.Glyph = "\uE8A0";
        }

        private void NotesListControl_UnshareNote(int noteId)
        {
            _viewModel.UnshareNote(noteId);
        }
    }
}
