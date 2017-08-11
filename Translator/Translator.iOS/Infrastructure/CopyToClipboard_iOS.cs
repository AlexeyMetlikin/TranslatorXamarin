using Xamarin.Forms;
using UIKit;
using Translator.Infrastructure;
using Translator.iOS.Infrastructure;

[assembly: Dependency(typeof(CopyToClipboard_iOS))]
namespace Translator.iOS.Infrastructure
{
    public class CopyToClipboard_iOS : ICopyToClipboard
    {
        public void Copy(string text)
        {
            UIPasteboard clipboard = UIPasteboard.General;
            clipboard.String = text;
        }
    }
}