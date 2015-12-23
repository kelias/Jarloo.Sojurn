using Jarloo.Sojurn.Models;

namespace Jarloo.Sojurn.StreamProviders
{
    public class PutLockerStreamProvider : IStreamProvider
    {
        public string Name { get; set; } = "PutLocker";

        public string GetUrl(Show show)
        {
            var name = show.Name.Replace(" ", "-");
            name = name.Replace("'", "");

            var url = $"http://putlocker.is/watch-{name}-tvshow-online-free-putlocker.html";

            return url;
        }
    }
}