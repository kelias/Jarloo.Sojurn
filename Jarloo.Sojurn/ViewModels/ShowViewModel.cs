using System.ComponentModel.Composition;
using Caliburn.Micro;
using Jarloo.Sojurn.Models;

namespace Jarloo.Sojurn.ViewModels
{
    [Export]
    public class ShowViewModel : PropertyChangedBase, IHaveDisplayName
    {
        #region Properties

        private Show show;

        public Show Show
        {
            get { return show; }
            set
            {
                show = value;
                NotifyOfPropertyChange(() => Show);
            }
        }

        #endregion

        [ImportingConstructor]
        public ShowViewModel(Show s)
        {
            Show = s;
            DisplayName = s.Name;
        }

        public string DisplayName { get; set; }
    }
}