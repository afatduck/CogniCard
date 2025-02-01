using CogniCard.Model;
using CogniCard.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CogniCard
{
    public partial class MainWindow : Window
    {
        private FlashcardContext? _flashcardContext;
        public DataService? DataService { get; private set; }

        public MainWindow()
        {
            _flashcardContext = new FlashcardContext();

            _flashcardContext.Database.EnsureCreated();

            _flashcardContext.Collections.Load();

            DataService = new DataService(_flashcardContext);

            MainViewModel mainViewModel = new(DataService);

            DataContext = mainViewModel;

            InitializeComponent();

            Style = (Style)FindResource(typeof(Window));
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement frameworkElement && frameworkElement.Focusable)
            {
                return;
            }

            if (e.OriginalSource is DependencyObject clickedElement)
            {
                while (clickedElement != null)
                {
                    if (clickedElement is ComboBox || clickedElement is ComboBoxItem)
                    {
                        return;
                    }
                    try
                    {
                        clickedElement = VisualTreeHelper.GetParent(clickedElement);
                    }
                    catch (InvalidOperationException) { return; }
                }
            }

            FocusElement.Focus();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Minimise_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}