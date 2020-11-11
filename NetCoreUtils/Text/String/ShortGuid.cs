using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreUtils.Text.String
{
    // Modified from Mads Kristensen's work: https://madskristensen.net/blog/A-shorter-and-URL-friendly-GUID
    // The encoded string is url friendly
    public static class ShortGuid
    {
        public static string NewGuid()
        {
            Guid guid = Guid.NewGuid();
            return Encode(guid);
        }

        public static string Encode(string guidText)
        {
            Guid guid = new Guid(guidText);
            return Encode(guid);
        }

        public static string Encode(Guid guid)
        {
            string enc = Convert.ToBase64String(guid.ToByteArray());
            enc = enc.Replace("/", "_");
            enc = enc.Replace("+", "-");
            return enc.Substring(0, 22);
        }

        public static Guid Decode(string encoded)
        {
            encoded = encoded.Replace("_", "/");
            encoded = encoded.Replace("-", "+");
            byte[] buffer = Convert.FromBase64String(encoded + "==");
            return new Guid(buffer);
        }
    }
}
