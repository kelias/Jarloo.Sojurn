using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using Jarloo.Sojurn.Helpers;

namespace Jarloo.Sojurn.ViewModels
{
    internal class ErrorViewModel : ViewModel
    {
        public ICommand OkCommand { get; set; }
        public ObservableCollection<string> Errors { get; set; }

        public ErrorViewModel()
        {
            Errors = new ObservableCollection<string>();
            BindingOperations.EnableCollectionSynchronization(Errors, Errors);

            OkCommand = new RelayCommand(p =>
            {
                lock (Errors)
                {
                    Errors.Clear();
                }
                Close();
            });
        }

        public void AddEntry(string entry)
        {
            try
            {
                lock (Errors)
                {
                    Errors.Add(entry);
                    if (Errors.Count > 100) Errors.RemoveAt(0);
                }

                View.Activate();
            }
            catch
            {
                //supress
            }
        }
    }
}