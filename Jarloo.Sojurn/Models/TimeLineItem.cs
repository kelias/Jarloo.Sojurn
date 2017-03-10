using System;
using Jarloo.Sojurn.Helpers;

namespace Jarloo.Sojurn.Models
{
    public class TimeLineItem : NotifyPropertyChangedBase
    {
        private Episode episode;
        private Show show;

        public DateTime? Date
        {
            get { return episode.AirDate; }
        }

        public Episode Episode
        {
            get { return episode; }
            set
            {
                episode = value;
                NotifyOfPropertyChange(() => Episode);
                NotifyOfPropertyChange(() => Date);
            }
        }

        public Show Show
        {
            get { return show; }
            set
            {
                show = value;
                NotifyOfPropertyChange(() => Show);
            }
        }
    }
}