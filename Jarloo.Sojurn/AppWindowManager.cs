using System.Windows;
using Caliburn.Micro;
using Jarloo.Sojurn.Windows;

namespace Jarloo.Sojurn
{
    internal class AppWindowManager : WindowManager
    {
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            Window window = view as BaseWindow;

            if (window == null)
            {
                if (isDialog)
                {
                    window = new BaseDialogWindow
                    {
                        Content = view,
                        SizeToContent = SizeToContent.WidthAndHeight
                    };
                }
                else
                {
                    window = new BaseWindow
                    {
                        Content = view,
                        SizeToContent = SizeToContent.Manual
                    };
                }

                window.SetValue(View.IsGeneratedProperty, true);
            }
            else
            {
                var owner2 = InferOwnerOf(window);
                if (owner2 != null && isDialog)
                {
                    window.Owner = owner2;
                }
            }
            return window;
        }
    }
}