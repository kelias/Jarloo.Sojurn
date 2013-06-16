using System;
using System.Runtime.Serialization;
using System.Windows.Media;
using Caliburn.Micro;

namespace Jarloo.Sojurn.Models
{
    [DataContract]
    public class Episode : PropertyChangedBase
    {
        private System.DateTime? airDate;
        private int episodeNumber;
        private ImageSource imageSource;
        private string imageUrl;
        private bool isLoading;
        private string link;
        private string title;
        private string showName;
        private int seasonNumber;
        private bool hasBeenViewed;
        

         
        [DataMember]
        public bool HasBeenViewed
        {
            get { return hasBeenViewed; }
            set
            {
                hasBeenViewed = value;
                NotifyOfPropertyChange(()=>HasBeenViewed);
            }
        }
        
        [DataMember]
        public int SeasonNumber
        {
            get { return seasonNumber; }
            set
            {
                seasonNumber = value;
                NotifyOfPropertyChange(()=>SeasonNumber);
            }
        }
 
        [DataMember]
        public string ShowName
        {
            get { return showName; }
            set
            {
                showName = value;
                NotifyOfPropertyChange(()=>ShowName);
            }
        }
        
        [IgnoreDataMember]
        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                isLoading = value;
                NotifyOfPropertyChange(() => IsLoading);
            }
        }

        [IgnoreDataMember]
        public ImageSource ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                NotifyOfPropertyChange(() => ImageSource);
            }
        }

        [DataMember]
        public string ImageUrl
        {
            get { return imageUrl; }
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
            get { return link; }
            set
            {
                link = value;
                NotifyOfPropertyChange(() => Link);
            }
        }

        [DataMember]
        public System.DateTime? AirDate
        {
            get { return airDate; }
            set
            {
                airDate = value;
                NotifyOfPropertyChange(() => AirDate);
            }
        }

        [DataMember]
        public int EpisodeNumber
        {
            get { return episodeNumber; }
            set
            {
                episodeNumber = value;
                NotifyOfPropertyChange(() => EpisodeNumber);
            }
        }

        [DataMember]
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }
    }
}