using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Translator.Droid.Renderers;
using Translator.CustomElements;
using Android.Text;
using Android.OS;
using Android.Views;
using Translator.Droid.Infrastructure;
using Android.App;
using Android.Views.InputMethods;
using Android.Graphics;
using Android.Runtime;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace Translator.Droid.Renderers
{
    public class CustomEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Element != null)
            {
                var element = Element as CustomEditor;
                Control.Hint = element.Placeholder;     // Выводим замещающий текст
                if (element.IsReadOnly)
                {
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb)
                    {
                        Control.SetRawInputType(InputTypes.ClassText);
                        Control.SetTextIsSelectable(true);
                    }
                    else
                    {
                        Control.SetRawInputType(InputTypes.Null);
                        Control.Focusable = true;
                    }
                    
                    Control.CustomSelectionActionModeCallback = new SelectionActionCallback(Element, Control);
                    Control.CustomInsertionActionModeCallback = new InsertionActionCallback();
                }
            }            
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == CustomEditor.PlaceholderProperty.PropertyName)
            {
                var element = Element as CustomEditor;
                Control.Hint = element.Placeholder;     // Выводим замещающий текст
            }
        }

        private class SelectionActionCallback : Java.Lang.Object, ActionMode.ICallback
        {
            private readonly Editor _editor;
            private readonly EditorEditText _control;

            public SelectionActionCallback(Editor editor, EditorEditText control)
                : base()
            {
                _editor = editor;
                _control = control;
            }

            public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
            {
                CopyToClipboard_Android clipboard = new CopyToClipboard_Android();
                clipboard.Copy(_editor.Text.Substring(_control.SelectionStart, _control.SelectionEnd - _control.SelectionStart));
                _editor.Unfocus();
                return true;
            }

            public bool OnCreateActionMode(ActionMode mode, IMenu menu)
            {
                menu.Clear();
                menu.Add(new Java.Lang.String("Copy")).SetShowAsAction(ShowAsAction.IfRoom);
                return true;
            }

            public void OnDestroyActionMode(ActionMode mode)
            {

            }

            public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
            {
                return true;
            }
        }

        private class InsertionActionCallback : Java.Lang.Object, ActionMode.ICallback
        {
            public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
            {
                return false;
            }

            public bool OnCreateActionMode(ActionMode mode, IMenu menu)
            {
                return false;
            }

            public void OnDestroyActionMode(ActionMode mode)
            {

            }

            public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
            {
                return false;
            }
        }
    }
}
 
