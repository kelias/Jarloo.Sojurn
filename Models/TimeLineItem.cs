using System;
using System.Runtime.Serialization;
using Jarloo.Sojurn.Helpers;

namespace Jarloo.Sojurn.Models;

[DataContract]
public class TimeLineItem : NotifyPropertyChangedBase
{
    private Episode episode;
    private Show show;

    public DateTime? Date => episode.AirDate;

    public Episode Episode
    {
        get => episode;
        set
        {
            episode = value;
            NotifyOfPropertyChange(() => Episode);
            NotifyOfPropertyChange(() => Date);
        }
    }

    public Show Show
    {
        get => show;
        set
        {
            show = value;
            NotifyOfPropertyChange(() => Show);
        }
    }
}