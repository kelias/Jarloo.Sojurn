using Caliburn.Micro;

namespace Jarloo.Sojurn.Models
{
    public class BacklogItem : PropertyChangedBase
    {
        private Show show;
        private Episode episode;
        private Season season;

        public string ShowName
        {
            get { return show.Name; }
        }

        public int SeasonNumber
        {
            get { return season.SeasonNumber; }
        }

        public int EpisodeNumberThisSeason
        {
            get { return episode.EpisodeNumberThisSeason; }
        }

        public Episode Episode
        {
            get { return episode; }
            set
            {
                episode = value;
                NotifyOfPropertyChange(() => Episode);
                NotifyOfPropertyChange(() => EpisodeNumberThisSeason);
            }
        }

        public Show Show
        {
            get { return show; }
            set
            {
                show = value;
                NotifyOfPropertyChange(() => Show);
                NotifyOfPropertyChange(() => ShowName);
            }
        }

        public Season Season
        {
            get { return season; }
            set
            {
                season = value;
                NotifyOfPropertyChange(() => Season);
                NotifyOfPropertyChange(() => SeasonNumber);
            }
        }
    }
}