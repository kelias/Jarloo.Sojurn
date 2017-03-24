using System.Windows;
using Jarloo.Sojurn.Helpers;
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