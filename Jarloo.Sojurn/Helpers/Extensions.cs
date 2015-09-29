using System;

namespace Jarloo.Sojurn.Helpers
{
    public static class Extensions
    {
        /// <summary>
        ///     Converts an object to the type specified.
        ///     If the object is a string and null or blank it will return null
        ///     If the object is = DBNull it will return the default type for that object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <returns></returns>
        public static T To<T>(this object text)
        {
            if (text == null) return default(T);
            if (text.Equals(DBNull.Value)) return default(T);
            if (text is string) if (string.IsNullOrWhiteSpace(text as string)) return default(T);

            var type = typeof (T);

            var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
            return (T) Convert.ChangeType(text, underlyingType);
        }
    }
}