using System.Collections.Generic;
using System.Runtime.Serialization;
using Caliburn.Micro;

namespace Jarloo.Sojurn.Models
{
    [DataContract]
    public class Season : PropertyChangedBase
    {
        [DataMember]
        public List<Episode> Episodes { get; set; }

        private int seasonNumber;

        private Episode selectedEpisode;

        [IgnoreDataMember]
        public Episode SelectedEpisode
        {
            get { return selectedEpisode; }
            set
            {
                selectedEpisode = value;
                NotifyOfPropertyChange(() => SelectedEpisode);
            }
        }

        [DataMember]
        public int SeasonNumber
        {
            get { return seasonNumber; }
            set
            {
                seasonNumber = value;
                NotifyOfPropertyChange(() => SeasonNumber);
            }
        }

        public Season()
        {
            Episodes = new List<Episode>();
        }
    }
}