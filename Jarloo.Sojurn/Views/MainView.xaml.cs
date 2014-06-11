using System.Windows.Controls;

namespace Jarloo.Sojurn.Views
{
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
        }

        public void ScrollIntoView(object o, SelectionChangedEventArgs e)
        {
            var b = (ListBox) o;

            if (b.SelectedItem == null) return;

            var item = (ListBoxItem) ((ListBox) o).ItemContainerGenerator.ContainerFromItem(((ListBox) o).SelectedItem);
            if (item != null) item.BringIntoView();
        }
    }
}