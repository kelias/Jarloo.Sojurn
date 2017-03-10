using System.Collections.Generic;
using System.Linq;

namespace Jarloo.Sojurn.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a jagged array of rows and columns
        /// </summary>
        /// <param name="input"></param>
        /// <param name="rowSeperator"></param>
        /// <param name="colSeperator"></param>
        /// <returns></returns>
        public static List<List<string>> ToRowsAndCols(this string input, char rowSeperator = '\n',
            char colSeperator = ',')
        {
            var rows = input.Split(rowSeperator);
            return rows.Select(row => row.Split(colSeperator).ToList()).ToList();
        }

        /// <summary>
        /// Parses CSV data, also takes into account quotes as per the standard
        /// </summary>
        /// <param name="input"></param>
        /// <param name="headers"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static List<List<string>> ParseCsv(this string input, int headers, char delimiter = ',')
        {
            var output = new List<List<string>>();

            input = input.Replace("\r", "");

            var columns = new List<string>();
            var field = "";
            var inQuotes = false;

            foreach (var c in input)
            {
                if (!inQuotes)
                {
                    if (c == '"')
                    {
                        inQuotes = true;
                    }
                    else if (c == '\n')
                    {
                        columns.Add(field);
                        field = "";
                        output.Add(columns);
                        columns = new List<string>();
                    }
                    else if (c == delimiter)
                    {
                        columns.Add(field);
                        field = "";
                    }
                    else
                    {
                        field += c;
                    }
                }
                else
                {
                    if (c == '"')
                    {
                        inQuotes = false;
                    }
                    else
                    {
                        field += c;
                    }
                }
            }

            if (columns.Count > 0) output.Add(columns);

            if (headers > 0)
            {
                output.RemoveRange(0, headers);
            }

            return output;
        }
    }
}