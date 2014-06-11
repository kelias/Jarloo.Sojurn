using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Jarloo.Sojurn.Controls
{
    public partial class TopmostControl : UserControl
    {
        public TopmostControl()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this)) return;

            imgIcon.MouseUp += imgIcon_MouseUp;
        }

        private void imgIcon_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var win = Window.GetWindow(this);
            MakeTopMost(!win.Topmost);
        }

        public void MakeTopMost(bool topmost)
        {
            var win = Window.GetWindow(this);

            if (!topmost)
            {
                win.Topmost = false;
                imgIcon.Source = (ImageSource) FindResource("Unpinned");
                imgIcon.ToolTip = "Pin window.";
            }
            else
            {
                win.Topmost = true;
                imgIcon.Source = (ImageSource) FindResource("Pinned");
                imgIcon.ToolTip = "Unpin window.";
            }
        }
    }
}