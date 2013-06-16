using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Jarloo.Sojurn.Models;

namespace Jarloo.Sojurn.InformationProviders
{
    public interface IInformationProvider
    {
        List<Show> GetShows(string search);
        Show GetFullDetails(int showId);
    }
}