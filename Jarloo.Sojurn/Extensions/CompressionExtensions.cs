using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Jarloo.Sojurn.Extensions
{
    public static class CompressionExtensions
    {
        public static string Compress(this string s, CompressionLevel level = CompressionLevel.Fastest)
        {
            var bytesToEncode = Encoding.UTF8.GetBytes(s);
            return Convert.ToBase64String(bytesToEncode.Compress());
        }

        public static byte[] Compress(this byte[] bytesToEncode, CompressionLevel level = CompressionLevel.Fastest)
        {
            using (var input = new MemoryStream(bytesToEncode))
            using (var output = new MemoryStream())
            {
                using (var zip = new GZipStream(output, level, false))
                {
                    input.CopyTo(zip);
                }
                return output.ToArray();
            }
        }

        public static string Decompress(this string s)
        {
            var compressedBytes = Convert.FromBase64String(s);
            return Encoding.UTF8.GetString(compressedBytes.Decompress());
        }

        public static byte[] Decompress(this byte[] compressedBytes)
        {
            using (var input = new MemoryStream(compressedBytes))
            using (var output = new MemoryStream())
            {
                using (var zip = new GZipStream(input, CompressionMode.Decompress))
                {
                    zip.CopyTo(output);
                }
                return output.ToArray();
            }
        }
    }
}