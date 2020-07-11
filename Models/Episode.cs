using System;
using System.Runtime.Serialization;
using System.Windows.Media;
using Jarloo.Sojurn.Helpers;

namespace Jarloo.Sojurn.Models
{
    [DataContract]
    public class Episode : NotifyPropertyChangedBase
    {
        private DateTime? airDate;
        private int episodeNumber;
        private int episodeNumberThisSeason;
        private bool hasBeenViewed;
        private ImageSource imageSource;
        private string imageUrl;
        private bool isLoading;
        private string link;
        private int seasonNumber;
        private string showName;
        private string title;
        private string summary;

        [DataMember]
        public string Summary
        {
            get => summary;
            set
            {
                summary = value;
                NotifyOfPropertyChange(() => Summary);
            }
        }

        [DataMember]
        public int EpisodeNumberThisSeason
        {
            get => episodeNumberThisSeason;
            set
            {
                episodeNumberThisSeason = value;
                NotifyOfPropertyChange(() => HasBeenViewed);
            }
        }

        [DataMember]
        public bool HasBeenViewed
        {
            get => hasBeenViewed;
            set
            {
                hasBeenViewed = value;
                NotifyOfPropertyChange(() => HasBeenViewed);
            }
        }

        [DataMember]
        public int SeasonNumber
        {
            get => seasonNumber;
            set
            {
                seasonNumber = value;
                NotifyOfPropertyChange(() => SeasonNumber);
            }
        }

        [DataMember]
        public string ShowName
        {
            get => showName;
            set
            {
                showName = value;
                NotifyOfPropertyChange(() => ShowName);
            }
        }

        [IgnoreDataMember]
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }

        [IgnoreDataMember]
        public ImageSource ImageSource
        {
            get => imageSource;
            set
            {
                imageSource = value;
                NotifyOfPropertyChange(() => ImageSource);
            }
        }

        [DataMember]
        public string ImageUrl
        {
            get => imageUrl;
            set
            {
                imageUrl = value;


                NotifyOfPropertyChange(() => ImageUrl);
                NotifyOfPropertyChange(() => ImageSource);
            }
        }

        [DataMember]
        public string Link
        {
            get => link;
            set
            {
                link = value;
                NotifyOfPropertyChange(() => Link);
            }
        }

        [DataMember]
        public DateTime? AirDate
        {
            get => airDate;
            set
            {
                airDate = value;
                NotifyOfPropertyChange(() => AirDate);
            }
        }

        [DataMember]
        public int EpisodeNumber
        {
            get => episodeNumber;
            set
            {
                episodeNumber = value;
                NotifyOfPropertyChange(() => EpisodeNumber);
            }
        }

        [DataMember]
        public string Title
        {
            get => title;
            set
            {
                title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }
    }
}