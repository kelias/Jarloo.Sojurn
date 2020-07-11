using System.Collections;
using System.Collections.ObjectModel;

namespace Jarloo.Sojurn.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Provides a helper method to quickly add elements to an observable collection without breaking the binding.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public static void AddRange<T>(this ObservableCollection<T> parent, IEnumerable child)
        {
            foreach (var v in child)
            {
                parent.Add((T) v);
            }
        }
    }
}