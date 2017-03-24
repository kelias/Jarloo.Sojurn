using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Jarloo.Sojurn.Extensions;
using Jarloo.Sojurn.Models;

namespace Jarloo.Sojurn.StreamProviders
{
    public class StreamProviderManager
    {
        public List<StreamProvider> StreamProviders { get; set; }

        public StreamProviderManager()
        {
            LoadStreamProviders();
        }

        private void LoadStreamProviders()
        {
            var fileLoc = ConfigurationManager.AppSettings["STREAM_PROVIDERS"];
            var data = File.ReadAllText(fileLoc);
            StreamProviders = data.FromJson<List<StreamProvider>>();
        }

        public void CallStreamProvider(StreamProvider s, Episode e)
        {
            var name = e.ShowName.Replace(" ", s.SpaceSeperator)
                .Replace("'","");

            var url = s.Url.Replace("{show}", name)
                .Replace("{season}",e.SeasonNumber.ToString())
                .Replace("{episode}",e.EpisodeNumber.ToString());
            
            Process.Start(url);
        }
    }
}