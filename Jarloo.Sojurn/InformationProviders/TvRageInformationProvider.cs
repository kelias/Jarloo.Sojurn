using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Jarloo.Sojurn.Models;
using DateTime = System.DateTime;

namespace Jarloo.Sojurn.InformationProviders
{
    public class TvRageInformationProvider : IInformationProvider
    {
        private const string BASE_URL = "http://services.tvrage.com/feeds/";

        public List<Show> GetShows(string search)
        {
            string url = string.Format("{0}search.php?show={1}", BASE_URL, HttpUtility.HtmlEncode(search));
            XDocument doc = XDocument.Load(url);

            var shows = (from s in doc.Root.Elements("show")
                         select new Show
                             {
                                 ShowId = Convert.ToInt32(s.Element("showid").Value),
                                 Name = s.Element("name").Value
                             }).ToList();

            return shows;
        }

        public Show GetFullDetails(int showId)
        {
            string url = string.Format("{0}full_show_info.php?sid={1}", BASE_URL, showId);
            XDocument doc = XDocument.Load(url);

            var seas = doc.Root;

            Show show = new Show
                {
                    ShowId = Get<int>(seas.Element("showid")),
                    Name = Get<string>(seas.Element("name")),
                    Started = GetDate(seas.Element("started")),
                    Ended = GetDate(seas.Element("ended")),
                    Country = Get<string>(seas.Element("origin_country")),
                    Status = Get<string>(seas.Element("status")),
                    ImageUrl = Get<string>(seas.Element("image")),
                    AirTimeHour = GetTime(seas.Element("airtime"),'H'),
                    AirTimeMinute = GetTime(seas.Element("airtime"),'M'),

                    Seasons = (from season in seas.Element("Episodelist").Elements("Season")
                               select new Season
                                   {
                                       SeasonNumber = Convert.ToInt32(season.Attribute("no").Value),
                                       Episodes = (from e in season.Elements("episode")
                                                   select new Episode
                                                       {
                                                           EpisodeNumber = Get<int>(e.Element("epnum")),
                                                           AirDate = GetDate(e.Element("airdate")),
                                                           Title = Get<string>(e.Element("title")),
                                                           Link = Get<string>(e.Element("link")),
                                                           ImageUrl = Get<string>(e.Element("screencap")),
                                                           ShowName = Get<string>(seas.Element("name")),
                                                           SeasonNumber = Convert.ToInt32(season.Attribute("no").Value)
                                                       }).OrderBy(w => w.EpisodeNumber).ToList()
                                   }).ToList()
                };



            for (int s = 0 ; s < show.Seasons.Count; s++)
            {
                for (int e = 0; e < show.Seasons[s].Episodes.Count; e++)
                {
                    show.Seasons[s].Episodes[e].EpisodeNumberThisSeason = e+1;
                }
            }
            
            return show;
        }


        private static T Get<T>(XElement e)
        {
            if (e == null) return default(T);
            return (T) Convert.ChangeType(e.Value, typeof (T));
        }

        private static DateTime? GetDate(XElement e)
        {
            if (e == null) return null;

            DateTime d;

            if (DateTime.TryParse(e.Value, out d)) return d;
            if (DateTime.TryParseExact(e.Value, "MMM/dd/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out d)) return d;
            if (DateTime.TryParseExact(e.Value, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out d)) return d;

            return null;
        }

        public static int GetTime(XElement time, char type)
        {
            if (string.IsNullOrEmpty(time.Value)) return 12;

            var strings = time.Value.Split(':');

            if (strings.Length==0) return 0;

            return type == 'H' ? Convert.ToInt32(strings[0]) : Convert.ToInt32(strings[1]);
        }
    }
}