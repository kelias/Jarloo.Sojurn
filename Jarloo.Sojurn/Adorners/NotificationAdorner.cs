using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Jarloo.Sojurn.Adorners
{
    public class NotificationAdorner : Adorner
    {
        public NotificationAdorner(UIElement adornedElement) : base(adornedElement)
        {
            var vc = new VisualCollection(this);

            var b = new Border
            {
                Background = new SolidColorBrush(Colors.Red),
                CornerRadius = new CornerRadius(9, 9, 9, 9),
                Padding = new Thickness(4, 4, 4, 4)
            };

            var t = new TextBlock
            {
                Foreground = new SolidColorBrush(Colors.White),
                FontWeight = FontWeights.Bold,
                FontSize = 12
            };

            b.Child = t;

            vc.Add(b);
        }
    }
}