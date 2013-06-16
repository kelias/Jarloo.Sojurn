using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Caliburn.Micro;
using Jarloo.Sojurn.Data;
using Jarloo.Sojurn.Helpers;
using Jarloo.Sojurn.InformationProviders;
using Jarloo.Sojurn.Models;
using DateTime = System.DateTime;

namespace Jarloo.Sojurn.ViewModels
{
    [Export]
    public class MainViewModel : Screen
    {
        private readonly IInformationProvider infoProvider;
        private readonly IPersistenceManager pm;
        private readonly IWindowManager wm;

        #region Properties

        private BindableCollection<Show> shows = new BindableCollection<Show>();
        private BindableCollection<TimeLineItem> timeLine = new BindableCollection<TimeLineItem>();

        public CollectionViewSource Shows { get; set; }
        public CollectionViewSource TimeLine { get; set; }
        

        private Show selectedShow;
        
        public Show SelectedShow
        {
            get { return selectedShow; }
            set
            {
                selectedShow = value;
                NotifyOfPropertyChange(()=>SelectedShow);
            }
        }
        
        #endregion

        [ImportingConstructor]
        public MainViewModel(IWindowManager windowManager) : this(windowManager, new TvRageInformationProvider(), new LocalJsonPersistenceManager())
        {
        }

        //Here to support dependency injection
        public MainViewModel(IWindowManager windowManager, IInformationProvider infoProvider, IPersistenceManager persistenceManager)
        {
            DisplayName = "Sojurn";
            wm = windowManager;
            pm = persistenceManager;
            this.infoProvider = infoProvider;

            Shows = new CollectionViewSource {Source = shows};
            Shows.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            TimeLine = new CollectionViewSource {Source = timeLine};
            TimeLine.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Ascending));
            TimeLine.GroupDescriptions.Add(new PropertyGroupDescription("Date"));
        }
        
        public void AddShow()
        {
            AddShowViewModel win = new AddShowViewModel(infoProvider, shows.ToList());
            if (wm.ShowDialog(win) != true) return;
            if (win.Show == null) return;
            
            Show show = win.Show;
            if (show.Seasons.Count > 0)
            {
                show.SelectedSeason = show.Seasons[show.Seasons.Count-1];
            }

            shows.Add(show);
            SelectedShow = show;
            
            SaveShows();

            ImageHelper.LoadDefaultImages(show);
            ImageHelper.GetShowImage(show);
            ImageHelper.GetEpisodeImages(show);

            UpdateTimeline();
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

            UserSettings userSettings = pm.Retrieve<UserSettings>("index");
            if (userSettings == null) return;
            if (userSettings.Shows == null) return;

            foreach (var show in userSettings.Shows)
            {
                if (show.Seasons.Count > 0) show.SelectedSeason = show.Seasons[show.Seasons.Count-1];

                shows.Add(show);
                
                ImageHelper.LoadDefaultImages(show);
                ImageHelper.GetShowImage(show);
                ImageHelper.GetEpisodeImages(show);
            }

            UpdateTimeline();
        }

        private void SaveShows()
        {
            UserSettings userSettings = new UserSettings {Shows = shows.ToList()};
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
            shows.Remove(s);
        }

        public void RefreshAllShows()
        {
            foreach (Show show in shows)
            {
                RefreshShow(show);
            }
        }

        public async void RefreshShow(Show oldShow)
        {
            oldShow.IsLoading = true;

            Show newShow = await Task<Show>.Factory.StartNew(() => infoProvider.GetFullDetails(oldShow.ShowId));

            oldShow.Country = newShow.Country;
            oldShow.Ended = newShow.Ended;
            oldShow.Link = newShow.Link;
            oldShow.Name = newShow.Name;
            oldShow.Started = newShow.Started;
            oldShow.Status = newShow.Status;
            oldShow.ImageUrl = newShow.ImageUrl;

            foreach (var newSeason in newShow.Seasons)
            {
                Season oldSeason = oldShow.Seasons.FirstOrDefault(w => w.SeasonNumber == newSeason.SeasonNumber);
                
                if (oldSeason == null)
                {
                    oldShow.Seasons.Add(newSeason);
                    continue;
                }

                foreach (var newEpisode in newSeason.Episodes)
                {
                    var oldEpisode = oldSeason.Episodes.FirstOrDefault(w => w.EpisodeNumber == newEpisode.EpisodeNumber);
                    
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

            oldShow.IsLoading = false;
        }

        public void ScrollShowIntoView(object o, SelectionChangedEventArgs e)
        {
            ListBoxItem item = (ListBoxItem)((ListBox)o).ItemContainerGenerator.ContainerFromItem(SelectedShow);
            if (item != null) item.BringIntoView();
        }

        public void MarkAllAsViewed(Show s)
        {
            foreach (var episode in s.Seasons.SelectMany(season => season.Episodes))
            {
                episode.HasBeenViewed = true;
            }
        }

        public void MarkAllAsNotViewed(Show s)
        {
            foreach (var episode in s.Seasons.SelectMany(season => season.Episodes))
            {
                episode.HasBeenViewed = false;
            }
        }

        public void ToggleViewed(Episode e)
        {
            e.HasBeenViewed = !e.HasBeenViewed;
        }

        public void UpdateTimeline()
        {
            foreach (var show in shows)
            {
                Season latestSeason = show.Seasons[show.Seasons.Count - 1];

                List<Episode> futureEpisodes = latestSeason.Episodes.Where(w => w.AirDate!=null && w.AirDate > DateTime.Now).OrderBy(w=>w.AirDate).ToList();

                foreach (Episode episode in futureEpisodes)
                {
                    if (timeLine.Any(w => w.Episode == episode)) continue;
                    timeLine.Add(new TimeLineItem{Show=show,Episode = episode});
                }
            }
        }

        private void RemoveFromTimeLine(Show show)
        {
            for (int i = timeLine.Count-1; i >= 0; i--)
            {
                if(timeLine[i].Show==show) timeLine.RemoveAt(i);
            }
        }
    }
}