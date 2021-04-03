/**
 * ______________________________________________________
 * This file is part of ko-administration-tool project.
 * 
 * @author       Mustafa Kemal Gılor <mustafagilor@gmail.com> (2017)
 * .
 * SPDX-License-Identifier:	MIT
 * ______________________________________________________
 */

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace KAI.Declarations
{
    class GeoIPEntry
    {
        public IPAddressRange Range;

        public string strCountryCode;
        public string strCountryName;
        public long val1;
        public long val2;
    }
    public class IPAddressRange
    {
        readonly System.Net.Sockets.AddressFamily addressFamily;
        readonly byte[] lowerBytes;
        readonly byte[] upperBytes;

        public IPAddressRange(IPAddress lower, IPAddress upper)
        {
            // Assert that lower.AddressFamily == upper.AddressFamily

            addressFamily = lower.AddressFamily;
            lowerBytes = lower.GetAddressBytes();
            upperBytes = upper.GetAddressBytes();
        }

        public bool IsInRange(IPAddress address)
        {
            if (address.AddressFamily != addressFamily)
            {
                return false;
            }

            byte[] addressBytes = address.GetAddressBytes();

            bool lowerBoundary = true, upperBoundary = true;

            for (int i = 0; i < lowerBytes.Length &&
                (lowerBoundary || upperBoundary); i++)
            {
                if ((lowerBoundary && addressBytes[i] < lowerBytes[i]) ||
                    (upperBoundary && addressBytes[i] > upperBytes[i]))
                {
                    return false;
                }

                lowerBoundary &= (addressBytes[i] == lowerBytes[i]);
                upperBoundary &= (addressBytes[i] == upperBytes[i]);
            }

            return true;
        }
    }
    public static class IPCountryCodeHelper
    {
        private static readonly List<GeoIPEntry> _ipEntryList = new List<GeoIPEntry>();
        public static bool LoadCSVFile(string path)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                using (FileStream fs = new FileStream(path, FileMode.Open,FileAccess.Read,FileShare.Read,4096))
                {
                    using (StreamReader br = new StreamReader(fs, System.Text.Encoding.ASCII, false, 4096))
                    {
                        string line;
                        while ((line = br.ReadLine()) != null)
                        {
                            string[] values = line.Split(',');
                            _ipEntryList.Add(new GeoIPEntry()
                            {
                                Range = (new IPAddressRange(IPAddress.Parse(values[0].Replace("\"", "")), IPAddress.Parse(values[1].Replace("\"", "")))),
                                val1 = long.Parse(values[2].Replace("\"", "")),
                                val2 = long.Parse(values[2].Replace("\"", "")),
                                strCountryCode = values[4].Replace("\"", ""),
                                strCountryName = values[5].Replace("\"", "")
                            });
                        }

                        var localblock1 = new GeoIPEntry()
                        {
                            Range = new IPAddressRange(IPAddress.Parse("10.0.0.0"), IPAddress.Parse("10.255.255.255")),
                            strCountryCode = "unknown",
                            strCountryName = "Local Network"
                        };
                        var localblock2 = new GeoIPEntry()
                        {
                            Range = new IPAddressRange(IPAddress.Parse("172.16.0.0"), IPAddress.Parse("172.31.255.255")),
                            strCountryCode = "unknown",
                            strCountryName = "Local Network"
                        };

                        var localblock3 = new GeoIPEntry()
                        {
                            Range = new IPAddressRange(IPAddress.Parse("192.168.0.0"), IPAddress.Parse("192.168.255.255")),
                            strCountryCode = "unknown",
                            strCountryName = "Local Network"
                        };

                        var loopback = new GeoIPEntry()
                        {
                            Range = new IPAddressRange(IPAddress.Parse("127.0.0.0"), IPAddress.Parse("127.255.255.255")),
                            strCountryCode = "unknown",
                            strCountryName = "(loopback)"
                        };
                        _ipEntryList.Add(localblock1);
                        _ipEntryList.Add(localblock2);
                        _ipEntryList.Add(localblock3);
                        _ipEntryList.Add(loopback);
                    }

                }

                sw.Stop();
                Trace.TraceInformation("Loading took " + sw.ElapsedMilliseconds + " millisecond(s)");
            }
            catch (IOException ex)
            {
                Trace.TraceError(ex.Message);
                return false;
            }
            return true;
        }

        public static KeyValuePair<string,string> QueryIPCountryCodeAndName(IPAddress addr)
        {

            foreach (var v in _ipEntryList)
            {
                if (v.Range.IsInRange(addr))
                {
                    return new KeyValuePair<string, string>(v.strCountryCode, v.strCountryName);
                }
            }
            return new KeyValuePair<string, string>("unknown","Unknown");
        }

        public static string QueryIPCountryName(IPAddress addr)
        {
            foreach (var v in _ipEntryList)
            {
                if (v.Range.IsInRange(addr))
                {
                    return v.strCountryName;
                }
            }
            return "NULL";
        }

        public static string QueryIPCountryCode(IPAddress addr)
        {
            foreach (var v in _ipEntryList)
            {
                if (v.Range.IsInRange(addr))
                {
                    return v.strCountryCode;
                }
            }
            return "NULL";
        }

    }
}
