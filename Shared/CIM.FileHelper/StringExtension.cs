using System.IO;
using System.IO.Compression;
using System.Text;

namespace CIM.FileHelper
{
    internal static class StringExtension
    {
        public static byte[] GZipComptess(this string s)
        {
            var encoding = Encoding.UTF8;
            using (var outputStream = new MemoryStream())
            {
                using (var compressionStream = new GZipStream(outputStream, CompressionMode.Compress))
                using (var inputStream = new MemoryStream(encoding.GetBytes(s)))
                    inputStream.CopyTo(compressionStream);
                return outputStream.ToArray();
            }
        }

        public static string GZipDecompress(this byte[] s)
        {
            var encoding = Encoding.UTF8;
            using (var outputStream = new MemoryStream())
            {
                using (var inputStream = new MemoryStream(s))
                using (var compressionStream = new GZipStream(inputStream, CompressionMode.Decompress))
                    compressionStream.CopyTo(outputStream);
                return encoding.GetString(outputStream.ToArray());
            }
        }
    }
}
