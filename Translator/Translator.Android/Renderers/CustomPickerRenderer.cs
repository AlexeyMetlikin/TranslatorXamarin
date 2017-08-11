using Android.Views;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Translator.Droid.Renderers;
using Translator.CustomElements;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace Translator.Droid.Renderers
{
    public class CustomPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Gravity = GravityFlags.CenterHorizontal;    // Выравнивание текста по центру элемента
            }
        }
    }
}