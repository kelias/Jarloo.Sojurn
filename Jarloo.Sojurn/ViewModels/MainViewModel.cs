using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml;
using Jarloo.Sojurn.Data;
using Jarloo.Sojurn.Helpers;
using Jarloo.Sojurn.InformationProviders;
using Jarloo.Sojurn.Models;
using Jarloo.Sojurn.StreamProviders;
using Jarloo.Sojurn.Views;

namespace Jarloo.Sojurn.ViewModels
{
    public sealed class MainViewModel : ViewModel
    {
        #region Properties

        private readonly IInformationProvider ip;
        private readonly IPersistenceManager pm;
        private readonly StreamProviderManager spm;
        public IStreamProvider StreamProvider { get; set; }

        private readonly ObservableCollection<BacklogItem> backlog = new ObservableCollection<BacklogItem>();
        private readonly ObservableCollection<Show> shows = new ObservableCollection<Show>();
        private readonly ObservableCollection<TimeLineItem> timeLine = new ObservableCollection<TimeLineItem>();
        private Show selectedShow;
        private string version;

        public CollectionViewSource Shows { get; set; }
        public CollectionViewSource TimeLine { get; set; }
        public CollectionViewSource Backlog { get; set; }

        public ICommand AddShowCommand { get; set; }
        public ICommand RefreshAllShowsCommand { get; set; }
        public ICommand RefreshShowCommand { get; set; }
        public ICommand DeleteShowCommand { get; set; }
        public ICommand MarkAllEpisodesAsWatchedCommand { get; set; }
        public ICommand MarkAllEpisodesAsUnWatchedCommand { get; set; }
        public ICommand ToggleViewedBackLogCommand { get; set; }
        public ICommand ShowEpisodesCommand { get; set; }
        public ICommand ShowStreamProvidersCommand { get; set; }
        public ICommand CallStreamProviderCommand { get; set; }

        public ObservableCollection<StreamProvider> StreamProviders { get; set; }

        public BacklogItem selectedBackLogItem;

        public BacklogItem SelectedBackLogItem
        {
            get => selectedBackLogItem;
            set
            {
                selectedBackLogItem = value;
                NotifyOfPropertyChange(() => SelectedBackLogItem);
            }
        }

        public string Version
        {
            get => version;
            set
            {
                version = value;
                NotifyOfPropertyChange(() => Version);
            }
        }

        public Show SelectedShow
        {
            get => selectedShow;
            set
            {
                selectedShow = value;
                NotifyOfPropertyChange(() => SelectedShow);
            }
        }

        #endregion

        public MainViewModel()
            : this(
                (IInformationProvider)
                Activator.CreateInstance(Type.GetType(ConfigurationManager.AppSettings["InformationProvider"])),
                (IPersistenceManager)
                Activator.CreateInstance(Type.GetType(ConfigurationManager.AppSettings["PersistanceManager"])))
        {
        }

        public MainViewModel(IInformationProvider infoProvider, IPersistenceManager persistenceManager)
        {
            try
            {
                Title = "Sojurn";

                pm = persistenceManager;
                ip = infoProvider;

                Shows = new CollectionViewSource {Source = shows};
                Shows.SortDescriptions.Add(new SortDescription("UnwatchedCount", ListSortDirection.Descending));
                Shows.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

                TimeLine = new CollectionViewSource {Source = timeLine};
                TimeLine.SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Ascending));
                TimeLine.GroupDescriptions.Add(new PropertyGroupDescription("Date"));

                Backlog = new CollectionViewSource {Source = backlog};
                Backlog.GroupDescriptions.Add(new PropertyGroupDescription("ShowName"));
                Backlog.SortDescriptions.Add(new SortDescription("ShowName", ListSortDirection.Ascending));
                Backlog.SortDescriptions.Add(new SortDescription("SeasonNumber", ListSortDirection.Ascending));
                Backlog.SortDescriptions.Add(new SortDescription("EpisodeNumberThisSeason", ListSortDirection.Ascending));

                Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

                spm = new StreamProviderManager();

                StreamProviders = new ObservableCollection<StreamProvider>(spm.StreamProviders);

                BindCommands();
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        public override void Show()
        {
            try
            {
                base.Show();

                LoadShows();
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        protected override void Closing()
        {
            base.Closing();

            SaveShows();
        }

        public void BindCommands()
        {
            try
            {
                AddShowCommand = new RelayCommand(t => AddShow());
                RefreshAllShowsCommand = new RelayCommand(t => RefreshAllShows());
                RefreshShowCommand = new RelayCommand(t => RefreshShow(t as Show));
                DeleteShowCommand = new RelayCommand(t => RemoveShow(t as Show));
                MarkAllEpisodesAsUnWatchedCommand = new RelayCommand(t => MarkAllAsNotViewed(t as Show));
                MarkAllEpisodesAsWatchedCommand = new RelayCommand(t => MarkAllAsViewed(t as Show));
                ToggleViewedBackLogCommand = new RelayCommand(t => ToggleViewedBacklog(t as BacklogItem));
                ShowEpisodesCommand = new RelayCommand(t => ShowEpisodes(t as Show));
                ShowStreamProvidersCommand = new RelayCommand(ShowStreamProviders);
                CallStreamProviderCommand = new RelayCommand(p => CallStreamProvider(p as StreamProvider));
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        private void CallStreamProvider(StreamProvider s)
        {
            try
            {
                if (s == null) return;
                if (SelectedBackLogItem == null) return;

                spm.CallStreamProvider(s, SelectedBackLogItem.Episode);
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        private void ShowStreamProviders(object t)
        {
            try
            {
                SelectedBackLogItem = (BacklogItem)t;
                var v = (MainView)View;

                var pop = v.StreamProviderPopup;

                pop.PlacementTarget = t as ListBoxItem;
                pop.Placement = PlacementMode.MousePoint;

                v.StreamProviderPopup.IsOpen = true;
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        private void ShowEpisodes(Show show)
        {
            try
            {
                if (show == null) return;

                var callback = new Action<Episode>(UpdateViewedOnBacklog);

                ViewModelManager.Create<SeasonViewModel>().Show(show, callback);
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        private void AddShow()
        {
            try
            {
                var vm = ViewModelManager.Create<AddShowViewModel>();
                vm.View.Owner = View;
                vm.InformationProvider = ip;
                vm.CurrentShows = shows.ToList();

                if (vm.ShowDialog() != true) return;
                if (vm.NewShow == null) return;

                var show = vm.NewShow;
                if (show.Seasons.Count > 0) show.SelectedSeason = show.Seasons[show.Seasons.Count - 1];

                shows.Add(show);
                SelectedShow = show;

                ImageHelper.LoadDefaultImages(show);
                ImageHelper.GetShowImageUrl(show);

                UpdateTimeline();
                UpdateBacklog();
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        private void SaveShows()
        {
            try
            {
                var userSettings = new UserSettings {Shows = shows.ToList()};
                pm.Save("index", userSettings);
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        private void LoadShows()
        {
            try
            {
                shows.Clear();

                var userSettings = pm.Retrieve<UserSettings>("index");

                if (userSettings == null) return;

                foreach (var show in userSettings.Shows)
                {
                    if (show.Seasons.Count > 0) show.SelectedSeason = show.Seasons[show.Seasons.Count - 1];

                    shows.Add(show);

                    ImageHelper.LoadDefaultImages(show);
                    ImageHelper.GetShowImageUrl(show);
                }

                UpdateTimeline();
                UpdateBacklog();
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        public void UpdateTimeline()
        {
            try
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
                        timeLine.Add(new TimeLineItem {Show = show, Episode = episode});
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        public void UpdateBacklog()
        {
            try
            {
                backlog.Clear();

                foreach (var show in shows)
                {
                    show.UnwatchedCount = 0;

                    foreach (var season in show.Seasons)
                    {
                        foreach (var episode in season.Episodes)
                        {
                            if (episode.HasBeenViewed || episode.AirDate > DateTime.Today || episode.AirDate == null)
                                continue;

                            backlog.Add(new BacklogItem {Show = show, Episode = episode, Season = season});
                            show.UnwatchedCount++;
                        }
                    }
                }

                Shows.View.Refresh();
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        public void RefreshAllShows()
        {
            try
            {
                foreach (var show in shows)
                {
                    RefreshShow(show);
                }

                UpdateTimeline();
                UpdateBacklog();
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        public async void RefreshShow(Show oldShow)
        {
            try
            {
                oldShow.IsLoading = true;

                try
                {
                    var newShow = await Task.Run(() => ip.GetFullDetails(oldShow.ShowId));

                    if (newShow == null) return;

                    oldShow.Country = newShow.Country;
                    oldShow.Ended = newShow.Ended;
                    oldShow.Link = newShow.Link;
                    oldShow.Name = newShow.Name;
                    oldShow.Started = newShow.Started;
                    oldShow.Status = newShow.Status;
                    oldShow.ImageUrl = newShow.ImageUrl;
                    oldShow.LastUpdated = newShow.LastUpdated;

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
                    ImageHelper.GetShowImageUrl(oldShow);
                }
                finally
                {
                    oldShow.IsLoading = false;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        public void RemoveShow(Show s)
        {
            try
            {
                if (
                    MessageBox.Show($"Delete the show {s.Name} and all associated data?", "Sojurn",
                        MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel) ==
                    MessageBoxResult.Cancel) return;

                RemoveFromTimeLine(s);
                RemoveFromBacklog(s);
                shows.Remove(s);
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        private void RemoveFromTimeLine(Show show)
        {
            try
            {
                for (var i = timeLine.Count - 1; i >= 0; i--)
                {
                    if (timeLine[i].Show == show) timeLine.RemoveAt(i);
                }
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        private void RemoveFromBacklog(Show show)
        {
            try
            {
                for (var i = backlog.Count - 1; i >= 0; i--)
                {
                    if (backlog[i].Show == show) backlog.RemoveAt(i);
                }
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        public void MarkAllAsViewed(Show s)
        {
            try
            {
                foreach (var episode in s.Seasons.SelectMany(season => season.Episodes))
                {
                    if (episode.AirDate > DateTime.Today) continue;

                    episode.HasBeenViewed = true;
                }

                s.UnwatchedCount = 0;

                UpdateBacklog();
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        public void MarkAllAsNotViewed(Show s)
        {
            try
            {
                s.UnwatchedCount = 0;

                foreach (var episode in s.Seasons.SelectMany(season => season.Episodes))
                {
                    episode.HasBeenViewed = false;
                    s.UnwatchedCount++;
                }

                UpdateBacklog();
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        public void UpdateViewedOnBacklog(Episode e)
        {
            try
            {
                var s = shows.FirstOrDefault(w => w.Name == e.ShowName);

                if (e.HasBeenViewed)
                {
                    for (var i = 0; i < backlog.Count; i++)
                    {
                        if (backlog[i].Episode != e) continue;
                        backlog.RemoveAt(i);

                        if (s != null) s.UnwatchedCount--;

                        break;
                    }
                }
                else
                {
                    var show = shows.FirstOrDefault(w => w.Name == e.ShowName);

                    if (show == null) return;

                    var season = show.Seasons.FirstOrDefault(w => w.SeasonNumber == e.SeasonNumber);

                    backlog.Add(new BacklogItem {Show = show, Episode = e, Season = season});

                    if (s != null) s.UnwatchedCount++;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }

        public void ToggleViewedBacklog(BacklogItem i)
        {
            try
            {
                i.Episode.HasBeenViewed = !i.Episode.HasBeenViewed;

                UpdateViewedOnBacklog(i.Episode);
            }
            catch (Exception ex)
            {
                ErrorManager.Log(ex);
            }
        }
    }
}