using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Jarloo.Sojurn.Helpers;
using Jarloo.Sojurn.Views;

namespace Jarloo.Sojurn.ViewModels
{
    public abstract class ViewModel : NotifyPropertyChangedBase
    {
        #region Properties

        private string title;
        private bool isWorking;
        private bool isClosing;
        private bool shouldCenter;

        public bool IsWorking
        {
            get { return isWorking; }
            set
            {
                isWorking = value;
                NotifyOfPropertyChange(() => IsWorking);
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                View.Title = title;
                NotifyOfPropertyChange(() => Title);
            }
        }

        #endregion

        protected ViewModel()
        {
            Register();
            View.Icon = (BitmapImage) View.FindResource("Logo");
            Title = "Sojurn";
            View.TitleCharacterCasing = CharacterCasing.Normal;
            shouldCenter = true;
        }

        public View View { get; set; }

        public virtual void Show()
        {
            if (shouldCenter) View.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            View.Show();
        }

        public virtual bool ShowDialog()
        {
            var x = View.ShowDialog();
            return x != null && x.Value;
        }

        protected void Register()
        {
            if (View != null) return;
            View = ViewModelBinder.GetView(this);
            View.DataContext = this;
            View.ViewModel = this;
            View.Closing += ViewOnClosing;
            ViewModelManager.Register(this);
        }

        private void ViewOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if (!TryClosing())
            {
                cancelEventArgs.Cancel = true;
                return;
            }

            cancelEventArgs.Cancel = false;

            Closing();

            View.Closing -= ViewOnClosing;
            View.ViewModel = null;

            ViewModelManager.Deregister(this);

            foreach (var win in View.OwnedWindows)
            {
                var view = win as View;
                view?.Close();
            }

            View = null;
            isClosing = true;
        }

        public void Close()
        {
            if (isClosing) return;

            View?.Close();

            isClosing = true;
        }

        protected virtual bool TryClosing()
        {
            return true;
        }

        protected virtual void Closing()
        {
        }
    }
}