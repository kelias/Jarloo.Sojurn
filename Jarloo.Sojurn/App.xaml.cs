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
            ViewModelManager.Create<MainViewModel>().Show();
        }
    }
}
