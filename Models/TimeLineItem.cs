using System;
using System.Runtime.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Jarloo.Sojurn.Models;

[DataContract]
[ObservableObject]
public partial class TimeLineItem 
{

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Date))]
    private Episode episode;

    [ObservableProperty]
    
    private Show show;

    public DateTime? Date => episode.AirDate;
}