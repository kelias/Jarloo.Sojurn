using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Jarloo.Sojurn.Helpers;
using Jarloo.Sojurn.Models;

namespace Jarloo.Sojurn.ViewModels;

public partial class SeasonViewModel : ViewModel
{
    public CollectionViewSource Seasons { get; set; }
    private Action<Episode> callback;
    
    public SeasonViewModel()
    {
        try
        {
            Seasons = new CollectionViewSource();
        }
        catch (Exception ex)
        {
            ErrorManager.Log(ex);
        }
    }

    public void Show(Show show, Action<Episode> cb)
    {
        try
        {
            callback = cb;

            Show();

            Title = $"{show.Name} - Seasons and Episodes";


            Seasons.Source = show.Seasons;
            Seasons.SortDescriptions.Add(new SortDescription("SeasonNumber", ListSortDirection.Descending));

            Seasons.View.Refresh();

            Task.Run(() => ImageHelper.GetEpisodeImages(show, View.Dispatcher));
        }
        catch (Exception ex)
        {
            ErrorManager.Log(ex);
        }
    }

    [RelayCommand]
    private void ToggleViewed(Episode e)
    {
        try
        {
            e.HasBeenViewed = !e.HasBeenViewed;
            callback?.Invoke(e);
        }
        catch (Exception ex)
        {
            ErrorManager.Log(ex);
        }
    }
}