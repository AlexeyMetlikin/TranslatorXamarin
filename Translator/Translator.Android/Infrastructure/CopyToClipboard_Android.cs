using Android.Content;
using Translator.Droid.Infrastructure;
using Translator.Infrastructure;
using Xamarin.Forms;

[assembly: Dependency(typeof(CopyToClipboard_Android))]
namespace Translator.Droid.Infrastructure
{
    public class CopyToClipboard_Android : ICopyToClipboard
    {
        public void Copy(string text)
        {
            ClipboardManager clipboardManager = (ClipboardManager)Forms.Context.GetSystemService(Context.ClipboardService);
            ClipData clip = ClipData.NewPlainText("Clipboard", text);
            clipboardManager.PrimaryClip = clip;
        }
    }
}