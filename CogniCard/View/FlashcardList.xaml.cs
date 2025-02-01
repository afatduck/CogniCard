using CogniCard.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CogniCard.View
{
    /// <summary>
    /// Interaction logic for FlashcardList.xaml
    /// </summary>
    public partial class FlashcardList : UserControl
    {
        public FlashcardList()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);
            if (window == null) return;
            window.PreviewMouseDown += WindowPreviewMouseDown;
        }

        public void WindowPreviewMouseDown(object? sender, MouseButtonEventArgs e)
        {
            if (!IsMouseOver)
            {
                if (DataContext != null) ((FlashcardListViewModel)DataContext).Selected = null;
            }
        }
    }
}
