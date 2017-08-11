using Xamarin.Forms;
using Translator.CustomElements;
using Translator.iOS.Renderers;
using Xamarin.Forms.Platform.iOS;
using System.ComponentModel;
using Foundation;
using ObjCRuntime;
using UIKit;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace Translator.iOS.Renderers
{
    public class CustomEditorRenderer : EditorRenderer
    {
        private string Placeholder { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Element != null)
            {
                var element = Element as CustomEditor;
                Placeholder = element.Placeholder;
                Control.TextColor = UIColor.LightGray;  // Цвет плейсхолдера
                Control.Text = Placeholder;             

                Control.ShouldBeginEditing += (UITextView textView) =>  // Если элемент получил фокус ввода
                {
                    if (textView.Text == Placeholder)       // Если текст в поле ввода = плейсхолдеру
                    {   
                        textView.Text = "";                 // Очищаем текст
                        textView.TextColor = UIColor.Black; // Устанавливаем цвет текста 
                    }

                    return true;
                };

                Control.ShouldEndEditing += (UITextView textView) =>    // Если с элемента снят фокус
                {
                    if (textView.Text == "")                    // Если поле пусто
                    {
                        textView.Text = Placeholder;            // Выводим плейсхолдер
                        textView.TextColor = UIColor.LightGray; // Устанавливаем цвет плейсхолдера
                    }

                    return true;
                };

                if (element.IsReadOnly)
                {
                    Control.Editable = false;
                    Control.TextColor = UIColor.Black;
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CustomEditor.PlaceholderProperty.PropertyName)
            {
                var element = Element as CustomEditor;
                Control.AccessibilityHint = element.Placeholder;
            }
        }
    }
}