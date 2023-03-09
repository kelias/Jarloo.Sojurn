using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jarloo.Sojurn.Extensions;
using Jarloo.Sojurn.InformationProviders;
using Jarloo.Sojurn.Models;

namespace Jarloo.Sojurn.ViewModels;

public partial class AddShowViewModel : ViewModel
{
    public IInformationProvider InformationProvider;
    
    public ObservableCollection<Show> Shows { get; set; } = new();
    
    [ObservableProperty]
    private Show selectedShow;

    [ObservableProperty]
    private Show newShow;

    [ObservableProperty]
    private string error;

    [ObservableProperty]
    private bool isSearchCompleted;

    [ObservableProperty]
    private string showName;

    public List<Show> CurrentShows;

    [ObservableProperty]
    private bool isShowNameFocused = true;
    
    [RelayCommand]
    public async void Search()
    {
        try
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

            Shows.AddRange(shows);

            IsWorking = false;
        }
        catch (Exception ex)
        {
            ErrorManager.Log(ex);
        }
    }

    [RelayCommand]
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

        var ns = await Task.Run(() =>
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

        NewShow = ns;

        if (ns != null)
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

    [RelayCommand]
    private void Cancel()
    {
        View.DialogResult = false;
        Close();
    }
}