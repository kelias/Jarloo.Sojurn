using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Jarloo.Sojurn.Helpers;
using Jarloo.Sojurn.InformationProviders;
using Jarloo.Sojurn.Models;

namespace Jarloo.Sojurn.ViewModels
{
    
    public sealed class AddShowViewModel : ViewModel
    {
        #region Properties

        public IInformationProvider InformationProvider;
        private bool isSearchCompleted;
        private string showName;

        public ObservableCollection<Show> Shows { get; set; } = new ObservableCollection<Show>();

        public ICommand AddShowCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        
        private Show selectedShow;
        private Show newShow;
        private string error;
        public  List<Show> CurrentShows;
        private bool isShowNameFocused = true;

        public string Error
        {
            get { return error; }
            set
            {
                error = value;
                NotifyOfPropertyChange(() => Error);
            }
        }

        public bool IsShowNameFocused
        {
            get { return isShowNameFocused; }
            set
            {
                NotifyOfPropertyChange(() => IsShowNameFocused);
            }
        }

        public bool CanAddShow => SelectedShow != null && IsWorking == false;

        public Show NewShow
        {
            get { return newShow; }
            set
            {
                newShow = value;
                NotifyOfPropertyChange(() => NewShow);
            }
        }

        public Show SelectedShow
        {
            get { return selectedShow; }
            set
            {
                selectedShow = value;
                NotifyOfPropertyChange(() => SelectedShow);
                NotifyOfPropertyChange(() => CanAddShow);
            }
        }

        public bool IsSearchCompleted
        {
            get { return isSearchCompleted; }
            set
            {
                isSearchCompleted = value;
                NotifyOfPropertyChange(() => IsSearchCompleted);
            }
        }


        public string ShowName
        {
            get { return showName; }
            set
            {
                showName = value;
                NotifyOfPropertyChange(() => ShowName);
            }
        }

        #endregion
        
    
        public AddShowViewModel()
        {
            BindCommands();
        }
        
        private void BindCommands()
        {
            AddShowCommand = new RelayCommand(t=> AddShow());
            CancelCommand = new RelayCommand(t =>
            {
                View.DialogResult = false;
                Close();
            });
            SearchCommand = new RelayCommand(t=> SearchShow());
        }

        public async void SearchShow()
        {
            IsSearchCompleted = false;
            IsWorking = true;

            var query = ShowName;
            var shows = await Task.Run(() =>
            {
                try
                {
                    return InformationProvider.GetShows(query);
                }
                catch
                {
                    return null;
                }
            });
            
            Shows.Clear();
            IsSearchCompleted = true;

            if (shows == null)
            {
                Error = "Provider failed to return information.";
                return;
            }

            Error = null;

            foreach (var s in shows)
            {
                Shows.Add(s);
            }

            IsWorking = false;
        }

        public async void AddShow()
        {
            if (SelectedShow == null) return;

            if (CurrentShows.Any(w => w.ShowId == SelectedShow.ShowId))
            {
                Error = "NewShow is already in your collection.";
                ShowName = string.Empty;
                IsSearchCompleted = false;
                SelectedShow = null;
                return;
            }
            
            IsWorking = true;
            IsSearchCompleted = false;
            
            var newShow = await Task.Run(() =>
            {
                try
                {
                    return InformationProvider.GetFullDetails(SelectedShow.ShowId);
                }
                catch
                {
                    return null;
                }
            });

            NewShow = newShow;

            if (newShow != null)
            {
                Error = null;
                View.DialogResult = true;
                Close();
            }
            else
            {
                Error = "Provider failed to return information.";
            }
        }

        public void TextModified(KeyEventArgs e)
        {
            if (e.Key == Key.Return) SearchShow();
        }
    }
}