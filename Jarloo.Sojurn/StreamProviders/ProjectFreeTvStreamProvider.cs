using Jarloo.Sojurn.Models;

namespace Jarloo.Sojurn.StreamProviders
{
    public class ProjectFreeTvStreamProvider : IStreamProvider
    {
        public string Name { get; set; } = "Project Free TV";

        public string GetUrl(Show show)
        {
            var name = show.Name.Replace(" ", "-");
            name = name.Replace("'", "");

            var url = $"http://projectfreetv.so/free/{name}/";

            return url;
        }
    }
}