
using Notes_Application.Presenter.ViewModel.Models;
using Notes_Application.UserControls;
using Notes_Application.Utility_Classes;
using Notes_Application.ViewModel;
using Notes_Application.ViewModel.Models;
using Notes_Library.DataManager;
using Notes_Library.Domain.UseCase;
using Notes_Library.Model;
using Notes_Library.Model.BussinessObjects;
using Notes_Library.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Notes_Application.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NoteBookView : Page
    {
        private NoteBookPageViewModel _viewModel ;

        public NoteBookView()
        {
            this.InitializeComponent();
            _viewModel = new NoteBookPageViewModel();


        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            _viewModel.GetNotebooks();
            base.OnNavigatedTo(e);

        }


        private void NotebookGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var notebookId = ((NoteBook)e.ClickedItem).NotebookId;
            this.Frame.Navigate(typeof(NotesView), notebookId, new SuppressNavigationTransitionInfo());
        }
      
        
        private void NotebookUserControl_DeleteNote(int noteBookId)
        { 
            _viewModel.DeleteNotebook(noteBookId);
        }

        private void NotebookUserControl_ChangeNoteBookCover(NoteBookVobj notebook)
        {
            NoteBookPopup.IsOpen= true;
            _viewModel.GetNoteBookCoverImages(notebook);
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            _viewModel.ChangeNotebookCover(((CoverImage)e.ClickedItem).Name);
        }

        private void ClosePopupButton_Click(object sender, RoutedEventArgs e)
        {
            NoteBookPopup.IsOpen = false;
        }
    }
}
