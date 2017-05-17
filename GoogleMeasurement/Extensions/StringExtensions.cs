using System;
using System.Text;

namespace Google.Data.Measurement
{
    internal static class StringExtensions
    {
        internal static byte[] GetBytes(this string givenString)
        {
            var bytes = new byte[givenString.Length * sizeof(char)];
            Buffer.BlockCopy(givenString.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        internal static byte[] GetBytes(this string givenString, Encoding encoding)
        {
            return encoding.GetBytes(givenString);
        }

        internal static string UrlEncode(this string givenString)
        {
            return Uri.EscapeDataString(givenString);
        }

        internal static bool IsNullOrEmpty(this string givenString)
        {
            return string.IsNullOrEmpty(givenString);
        }

        internal static bool NotNullOrEmpty(this string givenString)
        {
            return !string.IsNullOrEmpty(givenString);
        }

        internal static int GetSafeLength(this string givenString)
        {
            return givenString.NotNullOrEmpty() ? givenString.Length : 0;
        }
    }
}
