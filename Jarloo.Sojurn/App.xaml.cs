using System.Collections.Generic;
using System.IO;
using System.Windows;
using Jarloo.Sojurn.Extensions;
using Jarloo.Sojurn.Helpers;
using Jarloo.Sojurn.StreamProviders;
using Jarloo.Sojurn.ViewModels;

namespace Jarloo.Sojurn
{
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            //List<TvStream> streams = new List<TvStream>();

            //streams.Add(new TvStream("ProjectFreeTv", "http://project-free-tv.im/episode/{0}-season-{1}-episode-{1}/",TvStream.SeparationCharacter.Underscore));

            //var json = streams.ToJson();

            //File.WriteAllText("e:\\saved.json",json);


            ViewModelManager.Create<MainViewModel>().Show();
        }
    }
}
