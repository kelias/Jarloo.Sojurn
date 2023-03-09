using System.Collections.Generic;
using System.Runtime.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Jarloo.Sojurn.Models;

[DataContract]
[ObservableObject]
public partial class Season 
{
    [DataMember]
    [ObservableProperty]
    private int seasonNumber;

    [IgnoreDataMember]
    [ObservableProperty]
    private Episode selectedEpisode;

    public Season()
    {
        Episodes = new List<Episode>();
    }

    [DataMember] 
    public List<Episode> Episodes { get; set; }

    
    
}