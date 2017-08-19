using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryFoundation.Helper.EncodeHelper
{
    public static class EncodingHelper
    {
        public static byte[] GetAsciiBytes(string plainText)
        {
            return Encoding.ASCII.GetBytes(plainText);
        }
        public static string GetAsciiString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }
        public static byte[] GetUtf8Bytes(string plainText)
        {
            return Encoding.UTF8.GetBytes(plainText);
        }
        public static string GetUtf8String(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
        public static byte[] GetUtf32Bytes(string plainText)
        {
            return Encoding.UTF32.GetBytes(plainText);
        }
        public static string GetUtf32String(byte[] bytes)
        {
            return Encoding.UTF32.GetString(bytes);
        }
    }
}
