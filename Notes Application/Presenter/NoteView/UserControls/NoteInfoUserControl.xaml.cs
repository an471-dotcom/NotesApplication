using Notes_Application.ViewModel;
using Notes_Application.ViewModel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Appointments;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Notes_Application.UserControls
{
    public sealed partial class NoteInfoUserControl : UserControl
    {
        

        public NotesVobj Note
        {
            get { return (NotesVobj)GetValue(NoteProperty); }
            set { SetValue(NoteProperty, value); }
        }

     
        public static readonly DependencyProperty NoteProperty =
            DependencyProperty.Register("Note", typeof(NotesVobj), typeof(NoteInfoUserControl), new PropertyMetadata(null));


        public NoteInfoUserControl()
        {
            this.InitializeComponent();
           
          
        }
    }
}
