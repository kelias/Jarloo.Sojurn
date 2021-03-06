﻿using System.Runtime.Serialization;
using Jarloo.Sojurn.Helpers;

namespace Jarloo.Sojurn.Models
{
    [DataContract]
    public class BacklogItem : NotifyPropertyChangedBase
    {
        private Episode episode;
        private Season season;
        private Show show;

        public string ShowName => show.Name;

        public int SeasonNumber => season.SeasonNumber;

        public int EpisodeNumberThisSeason => episode.EpisodeNumberThisSeason;

        public Episode Episode
        {
            get => episode;
            set
            {
                episode = value;
                NotifyOfPropertyChange(() => Episode);
                NotifyOfPropertyChange(() => EpisodeNumberThisSeason);
            }
        }

        public Show Show
        {
            get => show;
            set
            {
                show = value;
                NotifyOfPropertyChange(() => Show);
                NotifyOfPropertyChange(() => ShowName);
            }
        }

        public Season Season
        {
            get => season;
            set
            {
                season = value;
                NotifyOfPropertyChange(() => Season);
                NotifyOfPropertyChange(() => SeasonNumber);
            }
        }
    }
}