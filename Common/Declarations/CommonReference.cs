/**
 * ______________________________________________________
 * This file is part of ko-administration-tool project.
 * 
 * @author       Mustafa Kemal Gılor <mustafagilor@gmail.com> (2017)
 * .
 * SPDX-License-Identifier:	MIT
 * ______________________________________________________
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using KAI.Declarations;

namespace KAI.Common.Declarations
{
    public static class CommonReference
    {
        public static DataSet TableSet = new DataSet("data_tables");
        public static bool TableSetInitialized = false;
        public static Dictionary<string, Dictionary<int, int>> TableSetLookupDictionary = new Dictionary<string, Dictionary<int, int>>();

        #region Make control topmost

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const UInt32 SWP_NOSIZE = 0x0001;
        private const UInt32 SWP_NOMOVE = 0x0002;
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        public static void MakeTopmost(Control c)
        {
            SetWindowPos(c.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }

        #endregion

        #region Flash window
        public static class FlashWindow
        {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

            [StructLayout(LayoutKind.Sequential)]
            private struct FLASHWINFO
            {
                /// <summary>
                /// The size of the structure in bytes.
                /// </summary>
                public uint cbSize;
                /// <summary>
                /// A Handle to the Window to be Flashed. The window can be either opened or minimized.
                /// </summary>
                public IntPtr hwnd;
                /// <summary>
                /// The Flash Status.
                /// </summary>
                public uint dwFlags;
                /// <summary>
                /// The number of times to Flash the window.
                /// </summary>
                public uint uCount;
                /// <summary>
                /// The rate at which the Window is to be flashed, in milliseconds. If Zero, the function uses the default cursor blink rate.
                /// </summary>
                public uint dwTimeout;
            }

            /// <summary>
            /// Stop flashing. The system restores the window to its original stae.
            /// </summary>
            public const uint FLASHW_STOP = 0;

            /// <summary>
            /// Flash the window caption.
            /// </summary>
            public const uint FLASHW_CAPTION = 1;

            /// <summary>
            /// Flash the taskbar button.
            /// </summary>
            public const uint FLASHW_TRAY = 2;

            /// <summary>
            /// Flash both the window caption and taskbar button.
            /// This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags.
            /// </summary>
            public const uint FLASHW_ALL = 3;

            /// <summary>
            /// Flash continuously, until the FLASHW_STOP flag is set.
            /// </summary>
            public const uint FLASHW_TIMER = 4;

            /// <summary>
            /// Flash continuously until the window comes to the foreground.
            /// </summary>
            public const uint FLASHW_TIMERNOFG = 12;


            /// <summary>
            /// Flash the spacified Window (Form) until it recieves focus.
            /// </summary>
            /// <param name="form">The Form (Window) to Flash.</param>
            /// <returns></returns>
            public static bool Flash(System.Windows.Forms.Form form)
            {
                // Make sure we're running under Windows 2000 or later
                if (Win2000OrLater)
                {
                    FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_ALL | FLASHW_TIMERNOFG, uint.MaxValue, 0);
                    return FlashWindowEx(ref fi);
                }
                return false;
            }

            private static FLASHWINFO Create_FLASHWINFO(IntPtr handle, uint flags, uint count, uint timeout)
            {
                FLASHWINFO fi = new FLASHWINFO();
                fi.cbSize = Convert.ToUInt32(Marshal.SizeOf(fi));
                fi.hwnd = handle;
                fi.dwFlags = flags;
                fi.uCount = count;
                fi.dwTimeout = timeout;
                return fi;
            }

            /// <summary>
            /// Flash the specified Window (form) for the specified number of times
            /// </summary>
            /// <param name="form">The Form (Window) to Flash.</param>
            /// <param name="count">The number of times to Flash.</param>
            /// <returns></returns>
            public static bool Flash(System.Windows.Forms.Form form, uint count)
            {
                if (Win2000OrLater)
                {
                    FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_ALL, count, 0);
                    return FlashWindowEx(ref fi);
                }
                return false;
            }

            /// <summary>
            /// Start Flashing the specified Window (form)
            /// </summary>
            /// <param name="form">The Form (Window) to Flash.</param>
            /// <returns></returns>
            public static bool Start(System.Windows.Forms.Form form)
            {
                if (Win2000OrLater)
                {
                    FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_ALL, uint.MaxValue, 0);
                    return FlashWindowEx(ref fi);
                }
                return false;
            }

            /// <summary>
            /// Stop Flashing the specified Window (form)
            /// </summary>
            /// <param name="form"></param>
            /// <returns></returns>
            public static bool Stop(System.Windows.Forms.Form form)
            {
                if (Win2000OrLater)
                {
                    FLASHWINFO fi = Create_FLASHWINFO(form.Handle, FLASHW_STOP, uint.MaxValue, 0);
                    return FlashWindowEx(ref fi);
                }
                return false;
            }

            /// <summary>
            /// A boolean value indicating whether the application is running on Windows 2000 or later.
            /// </summary>
            private static bool Win2000OrLater
            {
                get { return System.Environment.OSVersion.Version.Major >= 5; }
            }
        }
        #endregion

        #region Restore window
        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, uint Msg);

        private const uint SW_RESTORE = 0x09;

        public static void Restore(Form form)
        {
            if (form.WindowState == FormWindowState.Minimized)
            {
                ShowWindow(form.Handle, SW_RESTORE);
            }
        }
        #endregion

        #region Password Hashing
        private static readonly uint[] _encodingArray =
        {
            0x1a,
            0x1f, 0x11, 0x0a, 0x1e,
            0x10, 0x18, 0x02, 0x1d,
            0x08, 0x14, 0x0f, 0x1c,
            0x0b, 0x0d, 0x04, 0x13,
            0x17, 0x00, 0x0c, 0x0e,
            0x1b, 0x06, 0x12, 0x15,
            0x03, 0x09, 0x07, 0x16,
            0x01, 0x19, 0x05, 0x12,
            0x1d, 0x07, 0x19, 0x0f,
            0x1f, 0x16, 0x1b, 0x09,
            0x1a, 0x03, 0x0d, 0x13,
            0x0e, 0x14, 0x0b, 0x05,
            0x02, 0x17, 0x10, 0x0a,
            0x18, 0x1c, 0x11, 0x06,
            0x1e, 0x00, 0x15, 0x0c,
            0x08, 0x04, 0x01
        };

        private static readonly byte[] _alphabetArray =
        {
            0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39,
            0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d,
            0x4e, 0x4f, 0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a
        };

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }


        public static long GetUnixTimestamp()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

        public static long DateTimeToUnixTime(DateTime tm)
        {
            var timespan = tm - new DateTime(1970, 1, 1, 0, 0, 0);
            return (long)timespan.TotalSeconds;
        }


        public static string HashPasswordString(string inputPassword)
        {

            const uint startKey = 0x03e8;

            var stringByteList = new List<byte>(Encoding.UTF8.GetBytes(inputPassword));
            while (stringByteList.Count % 4 != 0)
            {
                stringByteList.Add(0);
            }
            byte[] stringBytes = stringByteList.ToArray();
            int counter = 0;
            uint tmp = 0;
            uint inputKey = 0;
            uint outHash = 0;
            var outStringBytes = new List<byte>();
            for (int i = 0; i < stringBytes.Length; i += 4)
            {
                uint encoded = BitConverter.ToUInt32(stringBytes, i);
                byte bl = 0x01; //even/odd thing?

                tmp = encoded + startKey; //input
                inputKey = tmp;
                counter = 0;
                outHash = 0;
                do
                {
                    tmp = inputKey;

                    inputKey = inputKey >> 1;
                    if (tmp % 2 != 0)
                    {
                        tmp = bl == 0 ? _encodingArray[(counter / 4) + 32] : _encodingArray[counter / 4];
                        outHash += (uint)1 << (int)tmp;
                    }
                    counter += 4;
                } while (inputKey > 0);

                long tmpPut = outHash;
                for (int tmpInt = 0; tmpInt < 7; tmpInt++)
                {
                    long tmpProduct = tmpPut * 0x38e38e39;
                    var upper = (int)(tmpProduct >> 35);
                    var anotherTmp = (uint)((upper * 8) + upper);
                    anotherTmp <<= 2;
                    var difference = (uint)(tmpPut - anotherTmp);
                    outStringBytes.Add(_alphabetArray[difference]);
                    tmpPut = upper;
                }
            }

            return Encoding.UTF8.GetString(outStringBytes.ToArray());
        }
        #endregion

        #region TBL operation

        private const string DataPath = @"./Data/";

        #region Control related

        public static Point[] PanelGetBorder(int nLeftEdge, int nTopEdge, int nWidth, int nHeight)
        {
            var x = nWidth;
            var y = nHeight;
            Point[] points =
            {
                new Point(1, 0),
                new Point(x - 1, 0),
                new Point(x - 1, 1),
                new Point(x, 1),
                new Point(x, y - 1),
                new Point(x - 1, y - 1),
                new Point(x - 1, y),
                new Point(1, y),
                new Point(1, y - 1),
                new Point(0, y - 1),
                new Point(0, 1),
                new Point(1, 1)
            };
            for (var i = 0; i < points.Length; i++)
                points[i].Offset(nLeftEdge, nTopEdge);
            return points;
        }

        public static Bitmap ColorTint(this Bitmap sourceBitmap, float blueTint,
                              float greenTint, float redTint)
        {

            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                    sourceBitmap.Width, sourceBitmap.Height),
                                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);

            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                var blue = pixelBuffer[k] + (255 - pixelBuffer[k]) * blueTint;
                var green = pixelBuffer[k + 1] + (255 - pixelBuffer[k + 1]) * greenTint;
                var red = pixelBuffer[k + 2] + (255 - pixelBuffer[k + 2]) * redTint;

                if (blue > 255)
                { blue = 255; }

                if (green > 255)
                { green = 255; }

                if (red > 255)
                { red = 255; }

                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;

            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                    resultBitmap.Width, resultBitmap.Height),
                                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }

        #endregion

        #region Encryption
        internal class EncryptionKOStandard
        {
            internal void Decode(ref byte[] data)
            {
                uint num = 0x816;
                for (var i = 0; i < data.Length; i++)
                {
                    var num3 = data[i];
                    var num4 = num;
                    byte num5 = 0;
                    num4 &= 0xff00;
                    num4 = num4 >> 8;
                    num5 = (byte)(num4 ^ num3);
                    num4 = num3;
                    num4 += num;
                    num4 &= 0xffff;
                    num4 *= 0x6081;
                    num4 &= 0xffff;
                    num4 += 0x1608;
                    num4 &= 0xffff;
                    num = num4;
                    data[i] = num5;
                }
            }

            public void Encode(FileStream stream)
            {
                var num = stream.ReadByte();
                uint num2 = 0x816;
                while (num != -1)
                {
                    stream.Seek(-1L, SeekOrigin.Current);
                    var num3 = (byte)(num & 0xff);
                    byte num4 = 0;
                    var num5 = num2;
                    num5 &= 0xff00;
                    num5 = num5 >> 8;
                    num4 = (byte)(num5 ^ num3);
                    num5 = num4;
                    num5 += num2;
                    num5 &= 0xffff;
                    num5 *= 0x6081;
                    num5 &= 0xffff;
                    num5 += 0x1608;
                    num5 &= 0xffff;
                    num2 = num5;
                    stream.WriteByte(num4);
                    num = stream.ReadByte();
                }
            }

        }
        #endregion

        #region Load table

        public static bool InitializeDataTableSet()
        {
            /* Check if we've initialized the table set before */
            if (TableSetInitialized)
                return true;
            TableSet.Tables.Clear();
            TableSet.Clear();

            if (!LoadTable("item_org_us.tbl"))
            {

                ShowError(null,
                      $"An error occured while loading data table [item_org_us.tbl].\nPlease make sure the required file exist, and try again.");
                return false;

            }
            for (var i = 0; i <= 42; i++)
            {
                if (LoadTable($"item_ext_{i}_us.tbl"))
                    continue;

                ShowError(null,
                    $"An error occured while loading data table [item_ext{i}_us.tbl].\nPlease make sure the required file exist, and try again.");
                return false;
            }

            return (TableSetInitialized = PrepareTableLookupDictionaries());
        }

        public static bool PrepareTableLookupDictionaries()
        {
            TableSetLookupDictionary.Clear();
            foreach (DataTable t in TableSet.Tables)
            {
                var i = 0;
                var lookupDictionary = t.Rows.Cast<DataRow>().ToDictionary(r => Convert.ToInt32(r[0]), r => i++);
                TableSetLookupDictionary.Add(t.TableName, lookupDictionary);
            }
            TableSetInitialized = true; return true;
        }

        public static bool LoadTable(string fname)
        {

            var anyError = false;

            if (File.Exists(DataPath + fname))
            {
                LoadByteDataIntoView(LoadAndDecodeFile(DataPath + fname), fname);
                Trace.TraceInformation(fname + " loaded");
            }
            else
            {
                anyError = true;
            }


            return !anyError;
        }

        private static byte[] LoadAndDecodeFile(string fileName)
        {
            var encDec = new EncryptionKOStandard();

            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                int offset = 0;
                var buffer = new byte[stream.Length];
                while (offset < stream.Length)
                {
                    offset += stream.Read(buffer, offset, ((int)stream.Length) - offset);
                }
                stream.Close();
                encDec.Decode(ref buffer);
                return buffer;
            }
        }


        private static void LoadByteDataIntoView(byte[] fileData, string Name)
        {
            int startIndex = 0;
            int num2 = BitConverter.ToInt32(fileData, startIndex);
            string tablename = Name;
            startIndex += 4;
            var numArray = new int[num2];
            var table = new DataTable(tablename);
            for (int i = 0; i < num2; i++)
            {
                DataColumn column;
                int num4 = BitConverter.ToInt32(fileData, startIndex);
                numArray[i] = num4;
                string prefix = i.ToString(CultureInfo.InvariantCulture);
                switch (num4)
                {
                    case 1:
                        column = new DataColumn(prefix + "\n(Signed Byte)", typeof(sbyte))
                        {
                            DefaultValue = (sbyte)0
                        };
                        break;

                    case 2:
                        column = new DataColumn(prefix + "\n(Byte)", typeof(byte))
                        {
                            DefaultValue = (byte)0
                        };
                        break;

                    case 3:
                        column = new DataColumn(prefix + "\n(Int16)", typeof(short))
                        {
                            DefaultValue = (short)0
                        };
                        break;

                    case 5:
                        column = new DataColumn(prefix + "\n(Int32)", typeof(int))
                        {
                            DefaultValue = 0
                        };
                        break;

                    case 6:
                        column = new DataColumn(prefix + "\n(UInt32)", typeof(uint))
                        {
                            DefaultValue = 0
                        };
                        break;

                    case 7:
                        column = new DataColumn(prefix + "\n(String)", typeof(string))
                        {
                            DefaultValue = ""
                        };
                        break;

                    case 8:
                        column = new DataColumn(prefix + "\n(Float)", typeof(float))
                        {
                            DefaultValue = 0f
                        };
                        break;

                    default:
                        column = new DataColumn(prefix + "\n(Unknown) " + num4.ToString(CultureInfo.InvariantCulture))
                        {
                            DefaultValue = 0
                        };
                        break;
                }
                table.Columns.Add(column);
                startIndex += 4;
            }

            int num5 = BitConverter.ToInt32(fileData, startIndex);
            startIndex += 4;
            for (int j = 0; (j < num5) && (startIndex < fileData.Length); j++)
            {
                DataRow row = table.NewRow();
                for (int k = 0; (k < num2) && (startIndex < fileData.Length); k++)
                {
                    int num8;
                    switch (numArray[k])
                    {
                        case 1:
                            {
                                row[k] = (fileData[startIndex] > 0x7f)
                                    ? (fileData[startIndex] - 0x100)
                                    : fileData[startIndex];
                                startIndex++;
                                continue;
                            }
                        case 2:
                            {
                                row[k] = fileData[startIndex];
                                startIndex++;
                                continue;
                            }
                        case 3:
                            {
                                row[k] = BitConverter.ToInt16(fileData, startIndex);
                                startIndex += 2;
                                continue;
                            }
                        case 5:
                            {
                                row[k] = BitConverter.ToInt32(fileData, startIndex);
                                startIndex += 4;
                                continue;
                            }
                        case 6:
                            {
                                row[k] = BitConverter.ToUInt32(fileData, startIndex);
                                startIndex += 4;
                                continue;
                            }
                        case 7:
                            {
                                num8 = BitConverter.ToInt32(fileData, startIndex);
                                startIndex += 4;
                                if (num8 > 0)
                                {
                                    break;
                                }
                                continue;
                            }
                        case 8:
                            {
                                row[k] = BitConverter.ToSingle(fileData, startIndex);
                                startIndex += 4;
                                continue;
                            }
                        default:
                            goto Label_03F5;
                    }
                    var chArray = new char[num8];
                    for (int m = 0; m < num8; m++)
                    {
                        chArray[m] = (char)fileData[startIndex];
                        startIndex++;
                    }
                    row[k] = new string(chArray);
                    continue;
                    Label_03F5:
                    row[k] = BitConverter.ToInt32(fileData, startIndex);
                    startIndex += 4;
                }
                table.Rows.Add(row);
            }

            TableSet.Tables.Add(table);
        }

        #endregion

        /// <summary>
        /// The number of ticks per Nanosecond.
        /// </summary>
        public const int NanosecondsPerTick = 100;
        /// <summary>
        /// The number of ticks per microsecond.
        /// </summary>
        public const int TicksPerMicrosecond = 10;
        /// <summary>
        /// Gets the microsecond fraction of a DateTime.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int Microseconds(this DateTime self)
        {
            return (int)Math.Floor(
               (self.Ticks
               % TimeSpan.TicksPerMillisecond)
               / (double)TicksPerMicrosecond);
        }
        /// <summary>
        /// Gets the Nanosecond fraction of a DateTime.  Note that the DateTime
        /// object can only store nanoseconds at resolution of 100 nanoseconds.
        /// </summary>
        /// <param name="self">The DateTime object.</param>
        /// <returns>the number of Nanoseconds.</returns>
        public static int Nanoseconds(this DateTime self)
        {
            return (int)(self.Ticks % TimeSpan.TicksPerMillisecond % TicksPerMicrosecond)
               * NanosecondsPerTick;
        }

        /// <summary>
        ///     Generates a new item serial.
        /// </summary>
        /// <returns></returns>
        public static ulong GenerateItemSerial()
        {

            return BitConverter.ToUInt64(
                new[]
                {
                    (byte) DateTime.Now.Millisecond,
                    (byte) DateTime.Now.Second,
                    (byte) DateTime.Now.Minute,
                    (byte) DateTime.Now.Hour,
                    (byte) DateTime.Now.Day,
                    (byte) DateTime.Now.Month,
                    (byte) (DateTime.Now.Year%100),
                    (byte) 1
                }, 0);
        }

        public static Image GetIconByIconID(uint iconID)
        {
            try { return Image.FromFile($".\\itemicons\\{iconID}.jpg"); }
            catch (Exception)
            {
                Trace.WriteLine("StaticReference::GetItemIcon() - Item icon with ID " + iconID + " does not exist.");
                return Image.FromFile($".\\itemicons\\{0}.jpg");
            }
        }


        public static ItemInformation GetItemDetails(int base_item, int extension_id)
        {
            try
            {
                DataTable item_org = TableSet.Tables["item_org_us.tbl"];
                Trace.Assert(item_org != null);

                DataRow base_row = item_org.Rows[TableSetLookupDictionary["item_org_us.tbl"][base_item]];

                if (base_row == null)
                    return null;


                byte extid = Convert.ToByte(base_row[1]);

                DataTable itemext = TableSet.Tables[$"item_ext_{extid}_us.tbl"];
                DataRow extension_row = itemext.Rows[TableSetLookupDictionary[$"item_ext_{extid}_us.tbl"][extension_id]];

                if (extension_row == null)
                    return null;

                // dark knight items
                if (base_item > 999999999 && extension_id > 999)
                    extension_id -= 1000;

                bool isNormalItem = Convert.ToInt32(extension_row[2]) == 0;
                return new ItemInformation
                {
                    Num = base_item + extension_id,
                    Name =
                        (isNormalItem
                            ? Convert.ToString(base_row[2]) + $" +({extension_id % 10})"
                            : Convert.ToString(extension_row[1])),
                    Description = Convert.ToString(base_row[3]),
                    IconID = Convert.ToInt32(base_row[7]),
                    Class = Convert.ToByte(base_row[14]),
                    Ac = (short)(Convert.ToInt16(base_row[22]) + Convert.ToInt16(extension_row[14])),
                    ItemType = Convert.ToByte(extension_row[7]),
                    Damage = (short)(Convert.ToInt16(extension_row[8]) + Convert.ToInt16(base_row[15])),
                    Range = Convert.ToInt16(base_row[17]),
                    Delay = Convert.ToInt16(base_row[16]),
                    Kind = Convert.ToByte(base_row[10]),
                    Slot = Convert.ToByte(base_row[12]),
                    SellingGroup = Convert.ToByte(base_row[35]),
                    Countable = Convert.ToByte(base_row[23]),
                    Hitrate = Convert.ToInt16(extension_row[10]),
                    EvasionRate = Convert.ToInt16(extension_row[11]),
                    Weight = Convert.ToInt16(base_row[18]),
                    Duration = (short)(Convert.ToInt16(base_row[19]) + Convert.ToInt16(extension_row[12])),
                    Race = Convert.ToByte(base_row[13]),
                    DaggerAc = Convert.ToInt16(extension_row[15]),
                    SwordAc = Convert.ToInt16(extension_row[16]),
                    MaceAc = Convert.ToInt16(extension_row[17]),
                    AxeAc = Convert.ToInt16(extension_row[18]),
                    SpearAc = Convert.ToInt16(extension_row[19]),
                    BowAc = Convert.ToInt16(extension_row[20]),
                    FireDamage = Convert.ToByte(extension_row[21]),
                    IceDamage = Convert.ToByte(extension_row[22]),
                    LightningDamage = Convert.ToByte(extension_row[23]),
                    PoisonDamage = Convert.ToByte(extension_row[24]),
                    HpDrain = Convert.ToByte(extension_row[25]),
                    MpDamage = Convert.ToByte(extension_row[26]),
                    MpDrain = Convert.ToByte(extension_row[27]),
                    StrB = Convert.ToInt16(extension_row[30]),
                    StaB = Convert.ToInt16(extension_row[31]),
                    DexB = Convert.ToInt16(extension_row[32]),
                    IntelB = Convert.ToInt16(extension_row[33]),
                    ChaB = Convert.ToInt16(extension_row[34]),
                    MaxHpB = Convert.ToInt16(extension_row[35]),
                    MaxMpB = Convert.ToInt16(extension_row[36]),
                    FireR = Convert.ToInt16(extension_row[37]),
                    ColdR = Convert.ToInt16(extension_row[38]),
                    LightningR = Convert.ToInt16(extension_row[39]),
                    MagicR = Convert.ToInt16(extension_row[40]),
                    PoisonR = Convert.ToInt16(extension_row[41]),
                    CurseR = Convert.ToInt16(extension_row[42]),
                    Effect1 = Convert.ToInt32(extension_row[43]),
                    Effect2 = Convert.ToInt32(extension_row[44]),
                    ReqLevel = (byte)(Convert.ToInt16(extension_row[45]) + Convert.ToInt16(base_row[26])),
                    ReqRank = (byte)Convert.ToInt16(extension_row[46]),
                    ReqTitle = (byte)Convert.ToInt16(extension_row[47]),
                    ReqStr = (byte)(Convert.ToInt16(extension_row[48]) + Convert.ToInt16(base_row[30])),
                    ReqSta = (byte)(Convert.ToInt16(extension_row[49]) + Convert.ToInt16(base_row[31])),
                    ReqDex = (byte)(Convert.ToInt16(extension_row[50]) + Convert.ToInt16(base_row[32])),
                    ReqIntel = (byte)(Convert.ToInt16(extension_row[51]) + Convert.ToInt16(base_row[33])),
                    ReqCha = (byte)(Convert.ToInt16(extension_row[52]) + Convert.ToInt16(base_row[34]))
                };

            }
            catch
            {
                return null;
            }

        }

        public static ItemInformation GetItemDetails_Parallel(int item_id)
        {
            if (item_id <= 0)
                return null;
            int extension_id = item_id % 1000;
            int base_item = item_id - extension_id;
            DataTable item_org = TableSet.Tables["item_org_us.tbl"];
            Trace.Assert(item_org != null);


            Parallel.ForEach(item_org.AsEnumerable(), current_row =>
            {
                //   if(current_row[0])
            }
            );
            return null;
        }
        public static ItemInformation GetItemDetails(int item_id)
        {
            if (item_id <= 0)
                return null;
            int extension_id = item_id % 1000;
            int base_item = item_id - extension_id;
            DataTable item_org = TableSet.Tables["item_org_us.tbl"];
            Trace.Assert(item_org != null);


            foreach (DataRow base_row in item_org.Rows)
            {
                if (Convert.ToInt32(base_row[0]) != base_item)
                    continue;
                byte extid = Convert.ToByte(base_row[1]);
                DataTable itemext = TableSet.Tables[$"item_ext_{extid}_us.tbl"]; Trace.Assert(itemext != null);

                foreach (DataRow extension_row in itemext.Rows)
                {
                    if (Convert.ToInt32(extension_row[0]) != extension_id) continue;
                    bool isNormalItem = Convert.ToInt32(extension_row[2]) == 0;
                    return new ItemInformation
                    {
                        Num = item_id,
                        Name = (isNormalItem ? Convert.ToString(base_row[2]) + $" +({extension_id % 10})" : Convert.ToString(extension_row[1])),
                        Description = Convert.ToString(base_row[3]),
                        IconID = Convert.ToInt32(base_row[7]),
                        Class = Convert.ToByte(base_row[14]),
                        Ac = (short)(Convert.ToInt16(base_row[22]) + Convert.ToInt16(extension_row[14])),
                        ItemType = Convert.ToByte(extension_row[7]),
                        Damage = (short)(Convert.ToInt16(extension_row[8]) + Convert.ToInt16(base_row[15])),
                        Range = Convert.ToInt16(base_row[17]),
                        Delay = Convert.ToInt16(base_row[16]),
                        Kind = Convert.ToByte(base_row[10]),
                        Slot = Convert.ToByte(base_row[12]),
                        SellingGroup = Convert.ToByte(base_row[35]),
                        Countable = Convert.ToByte(base_row[23]),
                        Hitrate = Convert.ToInt16(extension_row[10]),
                        EvasionRate = Convert.ToInt16(extension_row[11]),
                        Weight = Convert.ToInt16(base_row[18]),
                        Duration = (short)(Convert.ToInt16(base_row[19]) + Convert.ToInt16(extension_row[12])),
                        Race = Convert.ToByte(base_row[13]),
                        DaggerAc = Convert.ToInt16(extension_row[15]),
                        SwordAc = Convert.ToInt16(extension_row[16]),
                        MaceAc = Convert.ToInt16(extension_row[17]),
                        AxeAc = Convert.ToInt16(extension_row[18]),
                        SpearAc = Convert.ToInt16(extension_row[19]),
                        BowAc = Convert.ToInt16(extension_row[20]),
                        FireDamage = Convert.ToByte(extension_row[21]),
                        IceDamage = Convert.ToByte(extension_row[22]),
                        LightningDamage = Convert.ToByte(extension_row[23]),
                        PoisonDamage = Convert.ToByte(extension_row[24]),
                        HpDrain = Convert.ToByte(extension_row[25]),
                        MpDamage = Convert.ToByte(extension_row[26]),
                        MpDrain = Convert.ToByte(extension_row[27]),
                        StrB = Convert.ToInt16(extension_row[30]),
                        StaB = Convert.ToInt16(extension_row[31]),
                        DexB = Convert.ToInt16(extension_row[32]),
                        IntelB = Convert.ToInt16(extension_row[33]),
                        ChaB = Convert.ToInt16(extension_row[34]),
                        MaxHpB = Convert.ToInt16(extension_row[35]),
                        MaxMpB = Convert.ToInt16(extension_row[36]),
                        FireR = Convert.ToInt16(extension_row[37]),
                        ColdR = Convert.ToInt16(extension_row[38]),
                        LightningR = Convert.ToInt16(extension_row[39]),
                        MagicR = Convert.ToInt16(extension_row[40]),
                        PoisonR = Convert.ToInt16(extension_row[41]),
                        CurseR = Convert.ToInt16(extension_row[42]),
                        Effect1 = Convert.ToInt32(extension_row[43]),
                        Effect2 = Convert.ToInt32(extension_row[44]),
                        ReqLevel = (byte)(Convert.ToInt16(extension_row[45]) + Convert.ToInt16(base_row[26])),
                        ReqRank = (byte)Convert.ToInt16(extension_row[46]),
                        ReqTitle = (byte)Convert.ToInt16(extension_row[47]),
                        ReqStr = (byte)(Convert.ToInt16(extension_row[48]) + Convert.ToInt16(base_row[30])),
                        ReqSta = (byte)(Convert.ToInt16(extension_row[49]) + Convert.ToInt16(base_row[31])),
                        ReqDex = (byte)(Convert.ToInt16(extension_row[50]) + Convert.ToInt16(base_row[32])),
                        ReqIntel = (byte)(Convert.ToInt16(extension_row[51]) + Convert.ToInt16(base_row[33])),
                        ReqCha = (byte)(Convert.ToInt16(extension_row[52]) + Convert.ToInt16(base_row[34]))
                    };
                }
            }
            return null;
        }

        #endregion

        #region Zone list

        public static Dictionary<int, string> ZoneList = new Dictionary<int, string>()
        {
            {1, "Luferson Castle"},
            {2, "El Morad Castle"},
            {3, "War Karus"},
            {4, "War Elmorad"},
            {11, "Esland (El Morad)"},
            {12, "Eslant (Karus)"},
            {18, "Breth"},
            {28, "Piana"},
            {21, "Moradon"},
            {30, "Delos"},
            {31, "Bifrost"},
            {32, "Desperation Abyss"},
            {33, "Hell Abyss"},
            {34, "Felankor's Lair"},
            {35, "Delos Basement"},
            {48, "Battle Arena"},
            {51, "Quest Arena (Orc Prisoner)"},
            {52, "Quest Arena (Blood Don)"},
            {53, "Quest Arena (Goblin)"},
            {54, "Quest Arena (Caithoros)"},
            {55, "Forgotten Temple"},
            {62, "Forgotten Temple"},
            {63, "Nieds Triangle"},
            {64, "Nereid Island"},
            {65, "Zipang"},
            {66, "Oreads"},
            {71, "Colony Zone"},
            {72, "Ardream"},
            {73, "Ronark Land Base"},
            {75, "Krowaz's Dominion"},
            {81, "Monster Suppression Squad I"},
            {82, "Monster Suppression Squad II"},
            {83, "Monster Suppression Squad III"},
            {84, "Border Defense War"},
            {85, "Chaos Dungeon"},
            {86, "Under The Castle"},
            {87, "Juraid Mountain"},
        };

        public static KeyValuePair<int, string> GetZone(int id)
        {
            return ZoneList.ContainsKey(id) ? new KeyValuePair<int, string>(id, ZoneList[id]) : new KeyValuePair<int, string>(id, "Unknown Zone(" + id + ")");
        }
        #endregion

        #region Messagebox

        public static DialogResult ShowQuestion(XtraForm sender, string question)
        {
            return XtraMessageBox.Show(sender, question, "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        public static void ShowError(XtraForm sender, string message)
        {
            XtraMessageBox.Show(sender, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowInformation(XtraForm sender, string message)
        {

            XtraMessageBox.Show(sender, message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void ShowWarning(XtraForm sender, string message)
        {
            XtraMessageBox.Show(sender, message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /*   internal static ItemInformation GetItemDetails(uint ıtemID)
           {
               throw new NotImplementedException();
           }*/
        #endregion
    }
}
