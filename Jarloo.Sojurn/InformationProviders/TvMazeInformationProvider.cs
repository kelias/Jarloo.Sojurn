using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using Jarloo.Sojurn.Helpers;
using Jarloo.Sojurn.Models;
using Newtonsoft.Json;

namespace Jarloo.Sojurn.InformationProviders
{
    internal class TvMazeInformationProvider : IInformationProvider
    {
        private const string BASE_URL = "http://api.tvmaze.com/";

        public List<Show> GetShows(string search)
        {
            var requestUri = string.Format("{0}search/shows?q={1}", BASE_URL, HttpUtility.HtmlEncode(search));
            var data = GetJsonData(requestUri);

            var shows = new List<Show>();
            foreach (var item in data)
            {
                var show = new Show
                {
                    ShowId = item.show.id,
                    Name = item.show.name
                };
                shows.Add(show);
            }
            return shows;
        }

        public Show GetFullDetails(int showId)
        {
            try
            {
                var requestShowDetailUri = string.Format("{0}shows/{1}", BASE_URL, HttpUtility.HtmlEncode(showId));
                var shdata = GetJsonData(requestShowDetailUri);

                var show = new Show
                {
                    ShowId = shdata.id,
                    Name = shdata.name,
                    Started = GetDate(shdata.premiered),
                    Ended = null,
                    Country = GetCountryCode(shdata),
                    Status = shdata.status,
                    ImageUrl = GetImage(shdata.image),
                    AirTimeHour = GetTime(shdata.schedule.time, 'H'),
                    AirTimeMinute = GetTime(shdata.schedule.time, 'M')
                };

                var requestShowEpisodsUri = string.Format("{0}shows/{1}/episodes", BASE_URL,
                    HttpUtility.HtmlEncode(showId));
                var epdata = GetJsonData(requestShowEpisodsUri);

                //I could not use linq becuase the json data is dynamic
                //use old reliable foreach
                DateTime? lastEpisodeAirDate = null;
                var seasonNumber = 0;
                Season season = null;
                foreach (var ep in epdata)
                {
                    if (ep.season != seasonNumber)
                    {
                        season = new Season {SeasonNumber = ep.season};
                        show.Seasons.Add(season);
                        seasonNumber = ep.season;
                    }
                    //the season can't be null because the ep.season starts from 1 in TvMaze API
                    //and the 'if' statment above initialize the vavriable
                    season.Episodes.Add(new Episode
                    {
                        EpisodeNumber = ep.number,
                        AirDate = GetDate(ep.airdate),
                        Title = ep.name,
                        Link = ep.url,
                        ImageUrl = GetImage(ep.image),
                        ShowName = shdata.name,
                        SeasonNumber = ep.season
                    });
                    //if needed (check by status) get the value for the last Episode AirDate as the show's end date 
                    if (show.Status == "Ended")
                        lastEpisodeAirDate = GetDate(ep.airdate);
                }
                show.Ended = lastEpisodeAirDate;

                //check if there are seasons
                //if not return null for an exception
                if (season == null)
                    return null;

                foreach (var t in show.Seasons)
                {
                    for (var e = 0; e < t.Episodes.Count; e++)
                    {
                        t.Episodes[e].EpisodeNumberThisSeason = e + 1;
                    }
                }

                show.LastUpdated = DateTime.Now;

                return show;
            }
            catch
            {
                return null;
            }
        }

        private static dynamic GetJsonData(string requestUri)
        {
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(requestUri);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            var response = httpWebRequest.GetResponse();

            var json = "";
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                json = sr.ReadToEnd();
            }

            dynamic data = JsonConvert.DeserializeObject(json);
            return data;
        }

        public static string GetCountryCode(dynamic show)
        {
            const string DEFAULT_COUNTRY_CODE = "US";
            if (show == null)
                return DEFAULT_COUNTRY_CODE;

            if (show.network != null)
            {
                return show.network.country.code;
            }
            //support netflix (e.g. daredevil ,house of cards ,orange is the new black ...)
            if (show.webChannel != null)
            {
                return show.webChannel.country.code;
            }
            //failesafe
            return DEFAULT_COUNTRY_CODE;
        }
        
        private static int GetTime(dynamic time, char type)
        {
            if ((time == null) || (time == "")) return 12;

            string t = time;
            var strings = t.Split(':');

            if (strings.Length == 0) return 0;

            return type == 'H' ? strings[0].To<int>() : strings[1].To<int>();
        }

        private static string GetImage(dynamic img)
        {
            return img == null ? null : img.original;
        }

        private static DateTime? GetDate(dynamic e)
        {
            if (e == null)
                return null;
            DateTime t = e;
            return t;
        }
    }
}