using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Caliburn.Micro;
using Jarloo.Sojurn.Models;

namespace Jarloo.Sojurn.Helpers
{
    public static class ImageHelper
    {
        public static void LoadDefaultImages(Show show)
        {
            show.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Jarloo.Sojurn;component/Images/image_show.png", UriKind.Absolute));

            foreach (var season in show.Seasons)
            {
                foreach (var episode in season.Episodes)
                {
                    episode.ImageSource = new BitmapImage(new Uri(@"pack://application:,,,/Jarloo.Sojurn;component/Images/image_episode.png", UriKind.Absolute));
                }
            }
        }

        public  static void GetShowImage(Show show)
        {
            if (string.IsNullOrWhiteSpace(show.ImageUrl)) return;

            show.IsLoading = true;

            Task.Factory.StartNew(() =>
                {
                    string extension = Path.GetExtension(show.ImageUrl);
                    string file = string.Format("{0}{1}", show.ShowId, extension);
                    string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["IMAGE_CACHE"]);

                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                    string filename = Path.Combine(folder, file);

                    if (!File.Exists(filename))
                    {
                        using (WebClient web = new WebClient())
                        {
                            web.DownloadFile(show.ImageUrl, filename);
                        }
                    }

                    Execute.BeginOnUIThread(() =>
                        {
                            show.ImageSource = new BitmapImage(new Uri(filename));
                            show.IsLoading = false;
                        });
                });
        }

        public static void GetEpisodeImages(Show show)
        {
            foreach (var episode in show.Seasons.SelectMany(season => season.Episodes))
            {
                episode.IsLoading = true;
            }

            Task.Factory.StartNew(() =>
                {
                    foreach (var season in show.Seasons.OrderByDescending(w=>w.SeasonNumber))
                    {
                        foreach (var episode in season.Episodes.OrderByDescending(w=>w.EpisodeNumber))
                        {
                            Episode e = episode;

                            if (episode.ImageUrl != null)
                            {
                                string extension = Path.GetExtension(e.ImageUrl);
                                string file = string.Format("{0}_{1}_{2}{3}", show.ShowId, season.SeasonNumber, e.EpisodeNumber, extension);
                                string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["IMAGE_CACHE"]);

                                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

                                string filename = Path.Combine(folder, file);

                                if (!File.Exists(filename))
                                {
                                    using (WebClient web = new WebClient())
                                    {
                                        web.DownloadFile(e.ImageUrl, filename);
                                    }
                                }

                                Execute.BeginOnUIThread(() =>
                                    {
                                        try
                                        {
                                            if (extension.ToUpper() == ".PNG")
                                            {
                                                Stream imageStreamSource = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                                                PngBitmapDecoder decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                                                e.ImageSource = decoder.Frames[0];
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    e.ImageSource = new BitmapImage(new Uri(filename));
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
                                    });
                            }

                            Execute.BeginOnUIThread(() => e.IsLoading = false);
                        }
                    }
                });
        }
    }
}