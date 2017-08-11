using System;
using Xamarin.Forms;

namespace Translator.CustomElements
{
    // Кастомный Editor с плейсхолдером
    public class CustomEditor : Editor
    {
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create("Placeholder", typeof(string), typeof(CustomEditor), String.Empty);
        public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create("IsReadOnly", typeof(bool), typeof(CustomEditor), false);

        public CustomEditor()
        {
        }

        public string Placeholder
        {
            get
            {
                return (string)GetValue(PlaceholderProperty);
            }

            set
            {
                SetValue(PlaceholderProperty, value);
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return (bool)GetValue(IsReadOnlyProperty);
            }

            set
            {
                SetValue(IsReadOnlyProperty, value);
            }
        }
    }
}
