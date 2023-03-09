using System.Collections.ObjectModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.Input;

namespace Jarloo.Sojurn.ViewModels;

internal partial class ErrorViewModel : ViewModel
{
    public ObservableCollection<string> Errors { get; set; }

    public ErrorViewModel()
    {
        Errors = new ObservableCollection<string>();
        BindingOperations.EnableCollectionSynchronization(Errors, Errors);
    }

    [RelayCommand]
    public void Ok()
    {
        lock (Errors)
        {
            Errors.Clear();
        }

        Close();
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