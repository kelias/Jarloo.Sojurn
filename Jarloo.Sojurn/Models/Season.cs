using System.Collections.Generic;
using System.Runtime.Serialization;
using Jarloo.Sojurn.Helpers;

namespace Jarloo.Sojurn.Models
{
    [DataContract]
    public class Season : NotifyPropertyChangedBase
    {
        private int seasonNumber;

        private Episode selectedEpisode;

        public Season()
        {
            Episodes = new List<Episode>();
        }

        [DataMember]
        public List<Episode> Episodes { get; set; }

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
    }
}