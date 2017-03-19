using Jarloo.Sojurn.Models;

namespace Jarloo.Sojurn.StreamProviders
{
    public class ProjectFreeTvStreamProvider : IStreamProvider
    {
        public string Name { get; set; } = "Project Free TV";

        public string GetUrl(Episode episode)
        {
            var showName = episode.ShowName.Replace(" ", "-");

            var url = $"http://project-free-tv.im/episode/{showName}-season-{episode.SeasonNumber}-episode-{episode.EpisodeNumberThisSeason}/";

            return url;
        }
    }
}