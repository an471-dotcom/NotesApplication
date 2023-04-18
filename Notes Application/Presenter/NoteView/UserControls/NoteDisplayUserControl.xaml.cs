using Notes_Application.Presenter.NoteView;
using Notes_Application.ViewModel;
using Notes_Application.ViewModel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Notes_Application.UserControls
{
    public sealed partial class NoteDisplayUserControl : UserControl,INoteDisplayView
    {
        private NoteDisplayUserControlViewModel _viewModel;
        
        public NotesVobj Note
        {
            get { return (NotesVobj)GetValue(NoteProperty); }
            set 
            { 
                SetValue(NoteProperty, value);
                
            }
        }


        
        public static readonly DependencyProperty NoteProperty =
            DependencyProperty.Register("Note", typeof(NotesVobj), typeof(NoteDisplayUserControl), new PropertyMetadata(null,OnNotePropertyChanged));

        private static void OnNotePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is NoteDisplayUserControl cntrl)
            {
                cntrl.NoteVobjChanged((NotesVobj)e.NewValue);
            }
        }

        private void NoteVobjChanged(NotesVobj newValue)
        {
            _viewModel.NoteDisplayView = this;
            _viewModel.SetNoteData(newValue);
            
        }


        public bool ShowBackButton
        {
            get { return (bool)GetValue(ShowBackButtonProperty); }
            set { SetValue(ShowBackButtonProperty, value); }
        }

      
        public static readonly DependencyProperty ShowBackButtonProperty =
            DependencyProperty.Register("ShowBackButton", typeof(bool), typeof(NoteDisplayUserControl), new PropertyMetadata(false));


        public bool ShowPopOutButton
        {
            get { return (bool)GetValue(ShowPopOutButtonProperty); }
            set { SetValue(ShowPopOutButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowPopOutButtonProperty =
            DependencyProperty.Register("ShowPopOutButton", typeof(bool), typeof(NoteDisplayUserControl), new PropertyMetadata(false));

        public event Action GoBack;
        public NoteDisplayUserControl()
        {
            this.InitializeComponent();
            _viewModel= NoteDisplayUserControlViewModel.GetInstance();       
        }
      

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.NoteDisplayView = this;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            ContentBox.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out string noteContent);
            _viewModel.UpdateNote(noteContent);
        }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ToggleFavoriteStatus();
        }

        private void MyColorPicker_ColorChanged(ColorPicker sender, ColorChangedEventArgs args)
        {
            var newColor = args.NewColor;
            double luminance = 0.2126 * newColor.R + 0.7152 * newColor.G + 0.0722 * newColor.B;

            if (luminance < 48)
            {
                sender.Color = Color.FromArgb(255, 48, 48, 48);

            }

        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            GoBack?.Invoke();
        }

        public void SetNoteContent(string noteContent)
        {
            ContentBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, noteContent);
        }

        private void AlignCenterButton_Click(object sender, RoutedEventArgs e)
        {
            var selection = ContentBox.Document.Selection;
            var format = selection.ParagraphFormat;

            format.Alignment = ParagraphAlignment.Center;

            selection.ParagraphFormat = format;
        }



        private void AlignLeftButton_Click(object sender, RoutedEventArgs e)
        {
            var selection = ContentBox.Document.Selection;
            var format = selection.ParagraphFormat;

            format.Alignment = ParagraphAlignment.Left;

            selection.ParagraphFormat = format;


        }

        private void AlignRightButton_Click(object sender, RoutedEventArgs e)
        {
            var selection = ContentBox.Document.Selection;
            var format = selection.ParagraphFormat;

            format.Alignment = ParagraphAlignment.Right;

            selection.ParagraphFormat = format;
           
        }

        private async void PopOutButton_Click(object sender, RoutedEventArgs e)
        {
            AppWindow appWindow = await AppWindow.TryCreateAsync();

            Frame appWindowContentFrame = new Frame();
            appWindowContentFrame.Navigate(typeof(SecondaryPage),_viewModel.Note);
            ElementCompositionPreview.SetAppWindowContent(appWindow, appWindowContentFrame);
            await appWindow.TryShowAsync();

            appWindow.Closed += delegate
            {
                appWindowContentFrame.Content = null;
                appWindow = null;
            };


        }


    }

    public interface INoteDisplayView
    {
        void SetNoteContent(string noteContent);
    }
}
