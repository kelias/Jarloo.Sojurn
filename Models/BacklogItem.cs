using System.Runtime.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Jarloo.Sojurn.Models;

[DataContract]
[ObservableObject]
public partial class BacklogItem 
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EpisodeNumberThisSeason))]
    private Episode episode;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SeasonNumber))]
    private Season season;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ShowName))]
    private Show show;

    public string ShowName => Show.Name;

    public int SeasonNumber => Season.SeasonNumber;

    public int EpisodeNumberThisSeason => Episode.EpisodeNumberThisSeason;

}