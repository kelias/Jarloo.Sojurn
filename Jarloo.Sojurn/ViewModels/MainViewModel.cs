using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using Caliburn.Micro;
using Jarloo.Sojurn.Data;
using Jarloo.Sojurn.Helpers;
using Jarloo.Sojurn.InformationProviders;
using Jarloo.Sojurn.Models;

namespace Jarloo.Sojurn.ViewModels
{
    [Export]
    public class MainViewModel : Screen
    {
        private readonly IInformationProvider infoProvider;
        private readonly IPersistenceManager pm;
        private readonly IWindowManager wm;

        #region Properties

        private readonly BindableCollection<BacklogItem> backlog = new BindableCollection<BacklogItem>();
        private readonly BindableCollection<Show> shows = new BindableCollection<Show>();
        private readonly BindableCollection<TimeLineItem> timeLine = new BindableCollection<TimeLineItem>();
        private Show selectedShow;

        public CollectionViewSource Shows { get; set; }
        public CollectionViewSource TimeLine { get; set; }
        public CollectionViewSource Backlog { get; set; }


        public Show SelectedShow
        {
            get { return selectedShow; }
            set
            {
                selectedShow = value;
                NotifyOfPropertyChange(() => SelectedShow);
            }
        }

        #endregion

        [ImportingConstructor]
        public MainViewModel(IWindowManager windowManager)
            : this(windowManager, new TvMazeInformationProvider(), new LocalJsonPersistenceManager()){}

        //Here to support dependency injection
        public MainViewModel(IWindowManager windowManager, IInformationProvider infoProvider,
            IPersistenceManager persistenceManager)
        {
            DisplayName = "Sojurn";
            wm = windowManager;
            pm = persistenceManager;
            this.infoProvider = infoProvider;

            Shows = new CollectionViewSource { Source = shows };
            Shows.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            TimeLine = new CollectionViewSource { Source = timeLine };
            TimeLine.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Ascending));
            TimeLine.GroupDescriptions.Add(new PropertyGroupDescription("Date"));

            Backlog = new CollectionViewSource { Source = backlog };
            Backlog.GroupDescriptions.Add(new PropertyGroupDescription("ShowName"));
            Backlog.SortDescriptions.Add(new SortDescription("ShowName", ListSortDirection.Ascending));
            Backlog.SortDescriptions.Add(new SortDescription("SeasonNumber", ListSortDirection.Ascending));
            Backlog.SortDescriptions.Add(new SortDescription("EpisodeNumberThisSeason", ListSortDirection.Ascending));
        }

        public void AddShow()
        {
            var win = new AddShowViewModel(infoProvider, shows.ToList());
            if (wm.ShowDialog(win) != true) return;
            if (win.Show == null) return;

            var show = win.Show;
            if (show.Seasons.Count > 0)
            {
                show.SelectedSeason = show.Seasons[show.Seasons.Count - 1];
            }

            shows.Add(show);
            SelectedShow = show;

            SaveShows();

            ImageHelper.LoadDefaultImages(show);
            ImageHelper.GetShowImage(show);
            ImageHelper.GetEpisodeImages(show);

            UpdateTimeline();
            UpdateBacklog();
        }

        protected override void OnActivate()
        {
            LoadShows();
        }

        protected override void OnDeactivate(bool close)
        {
            SaveShows();
        }

        private void LoadShows()
        {
            shows.Clear();

            var userSettings = pm.Retrieve<UserSettings>("index");
            if (userSettings == null) return;
            if (userSettings.Shows == null) return;

            ImageHelper.DeleteUnusedImages(userSettings.Shows);

            foreach (var show in userSettings.Shows)
            {
                if (show.Seasons.Count > 0) show.SelectedSeason = show.Seasons[show.Seasons.Count - 1];

                shows.Add(show);

                ImageHelper.LoadDefaultImages(show);
                ImageHelper.GetShowImage(show);
                ImageHelper.GetEpisodeImages(show);
            }

            UpdateTimeline();
            UpdateBacklog();
        }

        private void SaveShows()
        {
            var userSettings = new UserSettings { Shows = shows.ToList() };
            pm.Save("index", userSettings);
        }

        public void ShowEpisode(Episode e)
        {
            if (e == null) return;

            wm.ShowDialog(new EpisodeViewModel(e));
        }

        public void ShowShow(Show s)
        {
            if (s == null) return;

            wm.ShowDialog(new ShowViewModel(s));
        }

        public void RemoveShow(Show s)
        {
            RemoveFromTimeLine(s);
            RemoveFromBacklog(s);
            shows.Remove(s);
        }

        public void RefreshAllShows()
        {
            foreach (var show in shows)
            {
                RefreshShow(show);
            }

            UpdateTimeline();
            UpdateBacklog();
        }

        public async void RefreshShow(Show oldShow)
        {
            oldShow.IsLoading = true;

            try
            {
                var newShow = await Task<Show>.Factory.StartNew(() => infoProvider.GetFullDetails(oldShow.ShowId));

                if (newShow == null) return;

                oldShow.Country = newShow.Country;
                oldShow.Ended = newShow.Ended;
                oldShow.Link = newShow.Link;
                oldShow.Name = newShow.Name;
                oldShow.Started = newShow.Started;
                oldShow.Status = newShow.Status;
                oldShow.ImageUrl = newShow.ImageUrl;

                foreach (var newSeason in newShow.Seasons)
                {
                    var oldSeason = oldShow.Seasons.FirstOrDefault(w => w.SeasonNumber == newSeason.SeasonNumber);

                    if (oldSeason == null)
                    {
                        oldShow.Seasons.Add(newSeason);
                        continue;
                    }

                    foreach (var newEpisode in newSeason.Episodes)
                    {
                        var oldEpisode =
                            oldSeason.Episodes.FirstOrDefault(w => w.EpisodeNumber == newEpisode.EpisodeNumber);

                        if (oldEpisode == null)
                        {
                            oldSeason.Episodes.Add(newEpisode);
                            continue;
                        }

                        oldEpisode.AirDate = newEpisode.AirDate;
                        oldEpisode.ImageUrl = newEpisode.ImageUrl;
                        oldEpisode.Link = newEpisode.Link;
                        oldEpisode.Title = newEpisode.Title;
                    }
                }

                ImageHelper.LoadDefaultImages(oldShow);
                ImageHelper.GetShowImage(oldShow);
                ImageHelper.GetEpisodeImages(oldShow);
            }
            finally
            {
                oldShow.IsLoading = false;
            }
        }

        public void ScrollShowIntoView(object o, SelectionChangedEventArgs e)
        {
            var item = (ListBoxItem)((ListBox)o).ItemContainerGenerator.ContainerFromItem(SelectedShow);
            if (item != null) item.BringIntoView();
        }

        public void MarkAllAsViewed(Show s)
        {
            foreach (var episode in s.Seasons.SelectMany(season => season.Episodes))
            {
                if (episode.AirDate > DateTime.Today) continue;

                episode.HasBeenViewed = true;
            }

            UpdateBacklog();
        }

        public void MarkAllAsNotViewed(Show s)
        {
            foreach (var episode in s.Seasons.SelectMany(season => season.Episodes))
            {
                episode.HasBeenViewed = false;
            }

            UpdateBacklog();
        }

        public void ToggleViewed(Episode e)
        {
            e.HasBeenViewed = !e.HasBeenViewed;

            if (e.HasBeenViewed)
            {
                for (var i = 0; i < backlog.Count; i++)
                {
                    if (backlog[i].Episode != e) continue;
                    backlog.RemoveAt(i);
                    break;
                }
            }
            else
            {
                var show = shows.FirstOrDefault(w => w.Name == e.ShowName);
                var season = show.Seasons.FirstOrDefault(w => w.SeasonNumber == e.SeasonNumber);

                backlog.Add(new BacklogItem { Show = show, Episode = e, Season = season });
            }
        }

        public void ToggleViewedBacklog(BacklogItem i)
        {
            ToggleViewed(i.Episode);
        }


        public void UpdateTimeline()
        {
            timeLine.Clear();

            foreach (var show in shows)
            {
                var latestSeason = show.Seasons[show.Seasons.Count - 1];

                var futureEpisodes =
                    latestSeason.Episodes.Where(w => w.AirDate != null && w.AirDate >= DateTime.Today)
                        .OrderBy(w => w.AirDate)
                        .ToList();

                foreach (var episode in futureEpisodes)
                {
                    if (timeLine.Any(w => w.Episode == episode)) continue;
                    timeLine.Add(new TimeLineItem { Show = show, Episode = episode });
                }
            }
        }

        public void UpdateBacklog()
        {
            backlog.Clear();

            foreach (var show in shows)
            {
                foreach (var season in show.Seasons)
                {
                    foreach (var episode in season.Episodes)
                    {
                        if (episode.HasBeenViewed || episode.AirDate > DateTime.Today || episode.AirDate == null)
                            continue;

                        backlog.Add(new BacklogItem { Show = show, Episode = episode, Season = season });
                    }
                }
            }
        }

        private void RemoveFromTimeLine(Show show)
        {
            for (var i = timeLine.Count - 1; i >= 0; i--)
            {
                if (timeLine[i].Show == show) timeLine.RemoveAt(i);
            }
        }

        private void RemoveFromBacklog(Show show)
        {
            for (var i = backlog.Count - 1; i >= 0; i--)
            {
                if (backlog[i].Show == show) backlog.RemoveAt(i);
            }
        }
    }
}