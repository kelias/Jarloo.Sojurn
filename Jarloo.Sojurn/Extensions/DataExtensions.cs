using System;

namespace Jarloo.Sojurn.Extensions
{
    public static class DataExtensions
    {
        /// <summary>
        /// Escapes single quotes in a sql string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Escape(this string input)
        {
            return input?.Replace("'", "''");
        }

        /// <summary>
        /// Sanitizes user input prior to sending it to the database
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SanitizeInput(this string input)
        {
            if (input == null) return null;
            var d = input.Replace("'", "''");
            d = d.Replace(";", "");

            return d;
        }

        /// <summary>
        /// If DBNull returns null, otherwise returns the value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static T? GetOrNull<T>(this object o) where T : struct
        {
            if (!(o is DBNull)) return (T) o;
            return null;
        }
    }
}