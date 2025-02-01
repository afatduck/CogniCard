using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace CogniCard.Behavior
{
    public class FocusOnVisibilityBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.IsVisibleChanged += AssociatedObject_IsVisibleChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.IsVisibleChanged -= AssociatedObject_IsVisibleChanged;
        }

        private void AssociatedObject_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.IsVisible)
                {
                    textBox.Focus();
                }
                else
                {
                    Keyboard.ClearFocus();
                }
            }
        }
    }
}
