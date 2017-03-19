using Jarloo.Sojurn.Models;

namespace Jarloo.Sojurn.StreamProviders
{
    public interface IStreamProvider
    {
        string Name { get; set; }

        string GetUrl(Episode episode);
    }
}