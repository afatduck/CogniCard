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
    /// Interaction logic for TagsDisplay.xaml
    /// </summary>
    public partial class TagsDisplay : UserControl
    {
        public static readonly DependencyProperty TagsStringProperty =
            DependencyProperty.Register(nameof(TagsString), typeof(string), typeof(TagsDisplay),
                new PropertyMetadata(string.Empty, OnInputStringChanged));

        public string TagsString
        {
            get => (string)GetValue(TagsStringProperty);
            set => SetValue(TagsStringProperty, value);
        }

        public static readonly DependencyProperty TagsListProperty =
            DependencyProperty.Register(nameof(TagList), typeof(List<string>), typeof(TagsDisplay),
                new PropertyMetadata(new List<string>()));

        public List<string> TagList
        {
            get => (List<string>)GetValue(TagsListProperty);
            set => SetValue(TagsListProperty, value);
        }

        public TagsDisplay()
        {
            InitializeComponent();
        }

        private static void OnInputStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TagsDisplay control)
            {
                control.ProcessInputString((string)e.NewValue);
            }
        }

        private void ProcessInputString(string input)
        {
            TagList = TagsString.Split(',').Where(t => !String.IsNullOrEmpty(t)).ToList();
        }
    }
}
