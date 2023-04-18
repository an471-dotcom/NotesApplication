using Notes_Application.ViewModel.Models;
using System.ServiceModel.Channels;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using System;
using Notes_Library.Domain.UseCase;

namespace Notes_Application.UserControls
{
    public sealed partial class NoteListViewUserControl : UserControl
    {
        public NotesVobj Note
        {
            get => this.DataContext as NotesVobj;
        }

        public NoteListViewUserControl()
        {
            this.InitializeComponent();
            this.DataContextChanged += (s, e) => Bindings.Update();
        }



        public event Action<NotesVobj> ToggleFavorite;

        public event Action<int> DeleteNote;

        public event Action<int> UnshareNote;
        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleFavorite?.Invoke(Note);
        }
        private void DeleteItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            DeleteNote?.Invoke(Note.NoteId);
        }



        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            


        }

        private void UnShareNote_Click(object sender, RoutedEventArgs e)
        {
            UnshareNote?.Invoke(Note.NoteId);
        }

       
    }
}
