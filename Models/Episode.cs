using System;
using System.Runtime.Serialization;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Jarloo.Sojurn.Helpers;

namespace Jarloo.Sojurn.Models;

[DataContract]
[ObservableObject]
public partial class Episode 
{
    [ObservableProperty]
    [DataMember]
    private DateTime? airDate;

    [ObservableProperty]
    [DataMember]
    private int episodeNumber;

    [ObservableProperty]
    [DataMember]
    private int episodeNumberThisSeason;

    [ObservableProperty]
    [DataMember]
    private bool hasBeenViewed;

    [ObservableProperty]
    [IgnoreDataMember]
    private ImageSource imageSource;

    [ObservableProperty]
    [DataMember]
    private string imageUrl;

    [ObservableProperty]
    [IgnoreDataMember]
    private bool isLoading;

    [ObservableProperty]
    [DataMember]
    private string link;
    
    [ObservableProperty]
    [DataMember]
    private int seasonNumber;

    [ObservableProperty]
    [DataMember]
    private string showName;

    [ObservableProperty]
    [DataMember]
    private string title;

    [ObservableProperty]
    [DataMember]
    private string summary;
}