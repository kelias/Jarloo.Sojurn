using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Jarloo.Sojurn.Helpers;
using Jarloo.Sojurn.Models;

namespace Jarloo.Sojurn.ViewModels
{
    public class SeasonViewModel : ViewModel
    {
        public CollectionViewSource Seasons { get; set; }
        private Action<Episode> callback;

        public ICommand ToggleViewedCommand { get; set; }
        
        public SeasonViewModel()
        {
            Seasons = new CollectionViewSource();

            ToggleViewedCommand = new RelayCommand(t => ToggleViewed(t as Episode));
        }

        public void Show(Show show, Action<Episode> cb)
        {
            callback = cb;
            
            Show();

            Title = $"{show.Name} - Seasons and Episodes";


            Seasons.Source = show.Seasons;
            Seasons.SortDescriptions.Add(new SortDescription("SeasonNumber", ListSortDirection.Descending));

            Seasons.View.Refresh();

            Task.Run(() => ImageHelper.GetEpisodeImages(show, View.Dispatcher));
        }

        private void ToggleViewed(Episode e)
        {
            e.HasBeenViewed = !e.HasBeenViewed;

            callback?.Invoke(e);
        }
    }
}