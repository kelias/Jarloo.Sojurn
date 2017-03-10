using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Jarloo.Sojurn.Models;

namespace Jarloo.Sojurn.Helpers
{
    public static class ImageHelper
    {
        public static void LoadDefaultImages(Show show)
        {
            show.ImageSource =
                new BitmapImage(new Uri(@"pack://application:,,,/Jarloo.Sojurn;component/Images/image_show.png",
                    UriKind.Absolute));

            show.ImageSource.Freeze();

            foreach (var season in show.Seasons)
            {
                foreach (var episode in season.Episodes)
                {
                    episode.ImageSource =
                        new BitmapImage(
                            new Uri(@"pack://application:,,,/Jarloo.Sojurn;component/Images/image_episode.png",
                                UriKind.Absolute));

                    episode.ImageSource.Freeze();
                }
            }
        }

        public static void GetShowImageUrl(Show show)
        {
            if (string.IsNullOrWhiteSpace(show.ImageUrl)) return;

            show.IsLoading = true;

            var extension = Path.GetExtension(show.ImageUrl);
            var file = $"{show.ShowId}{extension}";
            var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                ConfigurationManager.AppSettings["IMAGE_CACHE"]);

            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            var filename = Path.Combine(folder, file);

            if (!File.Exists(filename))
            {
                using (var web = new WebClient())
                {
                    web.DownloadFile(show.ImageUrl, filename);
                }
            }

            show.ImageSource = new BitmapImage(new Uri(filename));
            show.ImageSource.Freeze();
            show.IsLoading = false;
        }

        public static void GetEpisodeImages(Show show, Dispatcher dispatcher)
        {
            foreach (var episode in show.Seasons.SelectMany(season => season.Episodes))
            {
                episode.IsLoading = true;
            }


            foreach (var season in show.Seasons.OrderByDescending(w => w.SeasonNumber))
            {
                foreach (var episode in season.Episodes.OrderByDescending(w => w.EpisodeNumber))
                {
                    var e = episode;

                    if (episode.ImageUrl != null)
                    {
                        var extension = Path.GetExtension(e.ImageUrl);
                        var file = $"{show.ShowId}_{season.SeasonNumber}_{e.EpisodeNumber}{extension}";
                        var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                            ConfigurationManager.AppSettings["IMAGE_CACHE"]);

                        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                        var filename = Path.Combine(folder, file);

                        if (!File.Exists(filename))
                        {
                            using (var web = new WebClient())
                            {
                                web.DownloadFile(e.ImageUrl, filename);
                            }
                        }

                        dispatcher.InvokeAsync(() =>
                        {
                            try
                            {
                                if (extension?.ToUpper() == ".PNG")
                                {
                                    using (var imageStreamSource = new FileStream(filename, FileMode.Open,
                                        FileAccess.Read, FileShare.Read))
                                    {
                                        var decoder = new PngBitmapDecoder(imageStreamSource,
                                            BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                                        e.ImageSource = decoder.Frames[0];
                                        e.ImageSource.Freeze();
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        e.ImageSource = new BitmapImage(new Uri(filename));
                                        e.ImageSource.Freeze();
                                    }
                                    catch
                                    {
                                        //File most likely corrupted
                                        File.Delete(filename);
                                    }
                                }
                            }
                            catch
                            {
                                //supress
                            }
                        }, DispatcherPriority.Background);
                    }

                    dispatcher.InvokeAsync(() => { e.IsLoading = false; }, DispatcherPriority.Background);
                }
            }
        }

        public static void DeleteUnusedImages(List<Show> shows)
        {
            var dict = shows.ToDictionary(show => show.ShowId, show => show.ShowId);

            var folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                ConfigurationManager.AppSettings["IMAGE_CACHE"]);

            if (!Directory.Exists(folder)) return;

            var files = Directory.GetFiles(folder);

            foreach (var f in files)
            {
                try
                {
                    var name = Path.GetFileName(f);
                    if (string.IsNullOrWhiteSpace(name)) continue;

                    string substr;

                    var idx = name.IndexOf('_');

                    if (idx > 0)
                    {
                        substr = name.Substring(0, idx);
                        if (string.IsNullOrWhiteSpace(substr)) continue;
                    }
                    else
                    {
                        substr = Path.GetFileNameWithoutExtension(f);
                    }

                    int id;
                    if (!int.TryParse(substr, out id)) continue;
                    if (dict.ContainsKey(id)) continue;

                    File.Delete(f);
                }
                catch
                {
                    //supress
                }
            }
        }
    }
}