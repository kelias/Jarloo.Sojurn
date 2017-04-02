using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows.Media;
using Jarloo.Sojurn.Helpers;

namespace Jarloo.Sojurn.Models
{
    [DataContract]
    public class Show : NotifyPropertyChangedBase
    {
        private int airTimeHour;
        private int airTimeMinute;

        private string country;
        private DateTime? ended;
        private ImageSource imageSource;
        private string imageUrl;
        private bool isLoading;
        private DateTime? lastUpdated;
        private string link;
        private string name;
        private Season selectedSeason;
        private int showId;
        private DateTime? started;
        private string status;
        private int timezone;
        private int unwatchedCount;

        public Show()
        {
            Seasons = new List<Season>();
        }

        [DataMember]
        public List<Season> Seasons { get; set; }

        [IgnoreDataMember]
        public int UnwatchedCount
        {
            get { return unwatchedCount; }
            set
            {
                unwatchedCount = value;
                NotifyOfPropertyChange(() => UnwatchedCount);
            }
        }

        [DataMember]
        public DateTime? LastUpdated
        {
            get { return lastUpdated; }
            set
            {
                lastUpdated = value;
                NotifyOfPropertyChange(() => LastUpdated);
            }
        }

        [DataMember]
        public int Timezone
        {
            get { return timezone; }
            set
            {
                timezone = value;
                NotifyOfPropertyChange(() => Timezone);
            }
        }

        [DataMember]
        public int AirTimeMinute
        {
            get { return airTimeMinute; }
            set
            {
                airTimeMinute = value;
                NotifyOfPropertyChange(() => AirTimeMinute);
            }
        }

        [DataMember]
        public int AirTimeHour
        {
            get { return airTimeHour; }
            set
            {
                airTimeHour = value;
                NotifyOfPropertyChange(() => AirTimeHour);
            }
        }

        [IgnoreDataMember]
        public Season SelectedSeason
        {
            get { return selectedSeason; }
            set
            {
                selectedSeason = value;
                NotifyOfPropertyChange(() => SelectedSeason);
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

        [DataMember]
        public string ImageUrl
        {
            get { return imageUrl; }
            set
            {
                imageUrl = value;
                NotifyOfPropertyChange(() => ImageUrl);
            }
        }

        [DataMember]
        public DateTime? Started
        {
            get { return started; }
            set
            {
                started = value;
                NotifyOfPropertyChange(() => Started);
            }
        }

        [DataMember]
        public DateTime? Ended
        {
            get { return ended; }
            set
            {
                ended = value;
                NotifyOfPropertyChange(() => Ended);
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
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        [DataMember]
        public string Country
        {
            get { return country; }
            set
            {
                country = value;
                NotifyOfPropertyChange(() => Country);
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
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        [DataMember]
        public int ShowId
        {
            get { return showId; }
            set
            {
                showId = value;
                NotifyOfPropertyChange(() => ShowId);
            }
        }
    }
}