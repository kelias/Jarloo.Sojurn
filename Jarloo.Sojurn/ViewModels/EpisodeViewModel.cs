using System.ComponentModel.Composition;
using Caliburn.Micro;
using Jarloo.Sojurn.Models;

namespace Jarloo.Sojurn.ViewModels
{
    [Export]
    public class EpisodeViewModel : PropertyChangedBase, IHaveDisplayName
    {
        private Episode episode;
        public string DisplayName { get; set; }

        public Episode Episode
        {
            get { return episode; }
            set
            {
                episode = value;
                NotifyOfPropertyChange(() => Episode);
            }
        }

        [ImportingConstructor]
        public EpisodeViewModel(Episode e)
        {
            episode = e;
            DisplayName = e.ShowName;
        }
    }
}