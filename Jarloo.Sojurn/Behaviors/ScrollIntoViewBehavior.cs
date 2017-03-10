using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Jarloo.Sojurn.Behaviors
{
    public sealed class ScrollIntoViewBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += ScrollIntoView;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= ScrollIntoView;
            base.OnDetaching();
        }

        private void ScrollIntoView(object o, SelectionChangedEventArgs e)
        {
            var b = (ListBox)o;
            if (b == null) return;
            if (b.SelectedItem == null) return;

            var item = (ListBoxItem)((ListBox)o).ItemContainerGenerator.ContainerFromItem(((ListBox)o).SelectedItem);
            if (item != null) item.BringIntoView();
        }
    }
}