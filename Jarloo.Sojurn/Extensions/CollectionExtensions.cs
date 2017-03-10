using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;

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

        /// <summary>
        /// Provides a helper method to quickly add elements to an observable collection without breaking the binding and can be done from another thread where the PropertyChanged events get marshalled to the UI thread for you
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public static void AddRangeFromAnotherThread<T>(this ObservableCollection<T> parent, IEnumerable child)
        {
            BindingOperations.EnableCollectionSynchronization(parent, parent);

            foreach (var v in child)
            {
                parent.Add((T) v);
            }
        }

        /// <summary>
        /// Returns the Median of a collection of numbers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T Median<T>(this IEnumerable<T> collection)
        {
            var orderedEnumerable = collection.OrderBy(w => w).ToList();

            var size = orderedEnumerable.Count();
            var mid = size/2;

            dynamic a = orderedEnumerable[mid];
            dynamic b = orderedEnumerable[mid - 1];

            T median = (size%2 != 0) ? orderedEnumerable[mid] : (a + b)/2;
            return median;
        }

        /// <summary>
        /// Returns the mode of a collection of numbers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T Mode<T>(this IEnumerable<T> collection)
        {
            var mode = collection.GroupBy(v => v)
                .OrderByDescending(g => g.Count())
                .First()
                .Key;
            return mode;
        }

        /// <summary>
        /// Computes the weighted average of a list of items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <param name="weightings"></param>
        /// <returns></returns>
        public static T WAvg<T>(this IList<T> values, IList<T> weightings)
        {
            if (values.Count() != weightings.Count())
                throw new ArgumentException("Values and weightings a have different number of elements.");

            dynamic totalWeightings = 0;
            dynamic totalResults = 0;

            for (var i = 0; i < values.Count; i++)
            {
                totalWeightings += weightings[i];
                totalResults += ((dynamic) weightings[i])*((dynamic) values[i]);
            }

            return totalResults/totalWeightings;
        }
    }
}