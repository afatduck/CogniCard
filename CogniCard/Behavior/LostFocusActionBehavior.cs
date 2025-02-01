using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using Microsoft.Xaml.Behaviors;

namespace CogniCard.Behavior
{
    public class FocusLostActionBehavior : Behavior<TextBox>
    {

        public ICommand ConfirmCommand
        {
            get => (ICommand)GetValue(ConfirmCommandProperty);
            set => SetValue(ConfirmCommandProperty, value);
        }

        public static readonly DependencyProperty ConfirmCommandProperty =
            DependencyProperty.Register(nameof(ConfirmCommand), typeof(ICommand), typeof(FocusLostActionBehavior));

        public ICommand CancelCommand
        {
            get => (ICommand)GetValue(CancelCommandProperty);
            set => SetValue(CancelCommandProperty, value);
        }

        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register(nameof(CancelCommand), typeof(ICommand), typeof(FocusLostActionBehavior));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.LostKeyboardFocus -= OnLostFocus;
            AssociatedObject.KeyDown -= OnKeyDown;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {

            if (AssociatedObject is Control control)
            {
                control.LostKeyboardFocus += OnLostFocus;
                AssociatedObject.KeyDown += OnKeyDown;
            }

            AssociatedObject.Loaded -= OnLoaded;
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.Visibility == Visibility.Collapsed) return;
            if (ConfirmCommand?.CanExecute(null) == true)
            {
                ConfirmCommand.Execute(null);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (AssociatedObject.Visibility == Visibility.Collapsed) return;
            if (e.Key == Key.Enter)
            {
                if (ConfirmCommand?.CanExecute(null) == true)
                {
                    ConfirmCommand.Execute(null);
                }
                e.Handled = true; 
                Keyboard.ClearFocus();
            }
            else if (e.Key == Key.Escape)
            {
                if (CancelCommand?.CanExecute(null) == true)
                {
                    CancelCommand.Execute(null);
                }
                e.Handled = true;
                Keyboard.ClearFocus();
            }
        }
    }
}
