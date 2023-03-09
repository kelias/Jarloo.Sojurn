using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Jarloo.Sojurn.Models;

[DataContract]
[ObservableObject]
public partial class Show 
{

    [DataMember]
    [ObservableProperty]
    private int airTimeHour;

    [DataMember]
    [ObservableProperty]
    private int airTimeMinute;
    
    [DataMember]
    [ObservableProperty]
    private string country;

    [DataMember]
    [ObservableProperty]
    private DateTime? ended;
    
    [IgnoreDataMember]
    [ObservableProperty]
    private ImageSource imageSource;

    [DataMember]
    [ObservableProperty]
    private string imageUrl;

    [IgnoreDataMember]
    [ObservableProperty]
    private bool isLoading;

    [DataMember]
    [ObservableProperty]
    private DateTime? lastUpdated;

    [DataMember]
    [ObservableProperty]
    private string link;

    [DataMember]
    [ObservableProperty]
    private string name;

    [IgnoreDataMember]
    [ObservableProperty]
    private Season selectedSeason;

    [DataMember]
    [ObservableProperty]
    private int showId;

    [DataMember]
    [ObservableProperty]
    private DateTime? started;

    [DataMember]
    [ObservableProperty]
    private string status;

    [DataMember]
    private int timezone;

    [IgnoreDataMember]
    [ObservableProperty]
    private int unwatchedCount;

    [DataMember] public List<Season> Seasons { get; set; }

    public Show()
    {
        Seasons = new List<Season>();
    }
}