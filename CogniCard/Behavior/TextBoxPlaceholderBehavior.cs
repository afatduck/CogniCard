using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace CogniCard.Behavior
{
    class TextBoxPlaceholderBehavior : Behavior<TextBox>
    {
        SolidColorBrush? DimmedBrush;
        SolidColorBrush? TextBrush;

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
                    nameof(Placeholder),
                    typeof(string),
                    typeof(TextBoxPlaceholderBehavior),
                    new PropertyMetadata(null)
                );
        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }
        protected override void OnAttached()
        {
            base.OnAttached();

            TextBrush = new ((Color)ColorConverter.ConvertFromString("#EEF6FF"));
            DimmedBrush = new((Color)ColorConverter.ConvertFromString("#3B6178"));

            BindingOperations.SetBinding(this, TextBox.TextProperty,
                    new Binding
                    {
                        Source = AssociatedObject,
                        Path = new PropertyPath(TextBox.TextProperty),
                        Mode = BindingMode.OneWay,
                        NotifyOnTargetUpdated = true
                    }
                );

            AssociatedObject.Loaded += OnLoad;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.GotFocus -= RemovePlaceholder;
            AssociatedObject.LostKeyboardFocus -= AddPlaceholder;
            AssociatedObject.TargetUpdated -= AddPlaceholder;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            AddPlaceholder(null, null);

            AssociatedObject.GotFocus += RemovePlaceholder;
            AssociatedObject.LostKeyboardFocus += AddPlaceholder;
            AssociatedObject.TargetUpdated += AddPlaceholder;

            AssociatedObject.Loaded -= OnLoad;
        }

        private void RemovePlaceholder(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.Text == Placeholder)
            {
                AssociatedObject.Text = string.Empty;
                AssociatedObject.Foreground = TextBrush;
            }
        }

        private void AddPlaceholder(object? sender, RoutedEventArgs? e)
        {

            if (AssociatedObject.Text == string.Empty)
            {
                if (!AssociatedObject.IsKeyboardFocused)
                {
                    AssociatedObject.Text = Placeholder;
                }
            }

            AssociatedObject.Foreground = AssociatedObject.Text == Placeholder ? DimmedBrush : TextBrush;
        }
    }
}
