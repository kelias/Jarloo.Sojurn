using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Jarloo.Sojurn.Extensions;

namespace Jarloo.Sojurn.Helpers
{
    [DataContract]
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyOfPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property)
        {
            NotifyOfPropertyChange(property.GetMemberInfo().Name);
        }
    }
}