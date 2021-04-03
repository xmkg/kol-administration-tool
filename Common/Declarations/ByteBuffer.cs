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
using System.Text;

namespace KAI.Declarations
{
    /// <summary>
    /// ByteBuffer class for I/O operations.
    /// </summary>
    public class ByteBuffer
    {
        public ByteBuffer()
        {
            _readFunctions = new Dictionary<Type, Delegate>
                            {
                                {typeof (char), new Func<char>(ReadChar)},
                                {typeof (sbyte), new Func<sbyte>(ReadInt8)},
                                {typeof (Int16), new Func<Int16>(ReadInt16)},
                                {typeof (Int32), new Func<Int32>(ReadInt32)},
                                {typeof (Int64), new Func<Int64>(ReadInt64)},
                                {typeof (byte), new Func<byte>(ReadByte)},
                                {typeof (UInt16), new Func<UInt16>(ReadUInt16)},
                                {typeof (UInt32), new Func<UInt32>(ReadUInt32)},
                                {typeof (UInt64), new Func<UInt64>(ReadUInt64)},
                                {typeof (Single), new Func<Single>(ReadFloat)},
                                {typeof (Double), new Func<Double>(ReadDouble)},
                                {typeof (String), new Func<bool, String>(ReadString)},
                                {typeof (Boolean), new Func<Boolean>(ReadBool)}
                            };
        }

        private readonly List<byte> _buffer = new List<byte>();
        private readonly Dictionary<Type, Delegate> _readFunctions;
        public override string ToString() { return BitConverter.ToString(_buffer.ToArray()); }
        public List<byte> GetBuffer { get { return _buffer; } }
        public int Size { get { return _buffer.Count; } }
        public int ReadPosition { get; private set; }
        public bool End() { return ReadPosition >= _buffer.Count - 1; }
        public void Clear() { _buffer.Clear(); ReadPosition = 0; }
        public int SkipBytes(int val) { if (val > Size - ReadPosition) return 0; ReadPosition += val; return 0; }

        // Signed
        public void Append(sbyte value) { _buffer.Add((byte)value); }
        public void Append(Int16 value) { byte[] val = BitConverter.GetBytes(value); foreach (byte b in val) { _buffer.Add(b); } }
        public void Append(Int32 value) { byte[] val = BitConverter.GetBytes(value); foreach (byte b in val) { _buffer.Add(b); } }
        public void Append(Int64 value) { byte[] val = BitConverter.GetBytes(value); foreach (byte b in val) { _buffer.Add(b); } }
        public void AppendZero(int count) { for (int i = 0; i < count; i++) Append((byte)0x00); }
        //UNSIGNED
        public void Append(byte value) { _buffer.Add(value); }
        public void Append(UInt16 value) { byte[] val = BitConverter.GetBytes(value); foreach (byte b in val) { _buffer.Add(b); } }
        public void Append(UInt32 value) { byte[] val = BitConverter.GetBytes(value); foreach (byte b in val) { _buffer.Add(b); } }
        public void Append(UInt64 value) { byte[] val = BitConverter.GetBytes(value); foreach (byte b in val) { _buffer.Add(b); } }

        //ARRAY
        public void Append(byte[] value, int offset, int length) { for (int i = offset; i < (offset + length); i++)_buffer.Add(value[i]); }
        public void Append(IEnumerable<byte> value) { foreach (byte b in value) { _buffer.Add(b); } }
        public void Append(float value) { byte[] val = BitConverter.GetBytes(value); foreach (byte b in val) { _buffer.Add(b); } }
        public void Append(double value) { byte[] val = BitConverter.GetBytes(value); foreach (byte b in val) { _buffer.Add(b); } }
        public void Append(Packet pkt) { foreach (byte b in pkt.GetArray())_buffer.Add(b); }

        public void Append(string value, bool isDoubleByte = true)
        {
            Append(isDoubleByte ? (UInt16)value.Length : (byte)value.Length);
#if ASCII_DEFINED
            byte[] val = Encoding.ASCII.GetBytes(value);
#else
            byte[] val = Encoding.GetEncoding(1252).GetBytes(value);
#endif
            foreach (byte b in val)
                _buffer.Add(b);
        }



        public T Read<T>(bool v)
        {
            Type type = typeof(T);
            if (!_readFunctions.ContainsKey(type))
                throw new Exception("Type is not defined!");
            return (T)_readFunctions[type].DynamicInvoke(v);
        }

        public T Read<T>()
        {
            Type type = typeof(T);
            if (!_readFunctions.ContainsKey(type))
                throw new Exception("Type is not defined!");
            return (T)_readFunctions[type].DynamicInvoke();
        }

        private char ReadChar() { var val = BitConverter.ToChar(_buffer.ToArray(), ReadPosition); ReadPosition += sizeof(char); return val; }
        private byte ReadByte() { var val = _buffer[ReadPosition]; ReadPosition += sizeof(byte); return val; }
        private bool ReadBool() { var val = BitConverter.ToBoolean(_buffer.ToArray(), ReadPosition); ReadPosition += 1; return val; }
        private ushort ReadUInt16() { var val = BitConverter.ToUInt16(_buffer.ToArray(), ReadPosition); ReadPosition += 2; return val; }
        private uint ReadUInt32() { var val = BitConverter.ToUInt32(_buffer.ToArray(), ReadPosition); ReadPosition += 4; return val; }
        private ulong ReadUInt64() { var val = BitConverter.ToUInt64(_buffer.ToArray(), ReadPosition); ReadPosition += 8; return val; }
        private sbyte ReadInt8() { var val = (sbyte)_buffer[ReadPosition]; ReadPosition += sizeof(sbyte); return val; }
        private short ReadInt16() { var val = BitConverter.ToInt16(_buffer.ToArray(), ReadPosition); ReadPosition += 2; return val; }
        private int ReadInt32() { var val = BitConverter.ToInt32(_buffer.ToArray(), ReadPosition); ReadPosition += 4; return val; }
        private long ReadInt64() { var val = BitConverter.ToInt64(_buffer.ToArray(), ReadPosition); ReadPosition += 8; return val; }
        private float ReadFloat() { var val = BitConverter.ToSingle(_buffer.ToArray(), ReadPosition); ReadPosition += 4; return val; }
        private double ReadDouble() { var val = BitConverter.ToDouble(_buffer.ToArray(), ReadPosition); ReadPosition += 8; return val; }
        public byte[] ReadRemainingBytes() { var val = new byte[Size - ReadPosition]; for (int i = ReadPosition; i < Size; i++)val[i - ReadPosition] = _buffer[i]; return val; }

        private string ReadString(bool isDoubleByte)
        {
            var len = isDoubleByte ? ReadUInt16() : ReadByte();
            if (len == 0) return string.Empty;
            var temp = new byte[len];
            if (ReadPosition + len <= Size)
            {
                for (UInt16 i = 0; i < len; i++)
                    temp[i] = ReadByte();
            }
            return Encoding.GetEncoding(1252).GetString(temp);
        }

        public byte[] GetArray() { return _buffer.ToArray(); }

        public byte[] GetArray(int startoffset, int endoffset)
        {
            if (endoffset > _buffer.Count - 1 || startoffset > _buffer.Count - 1)
                return null;
            var newarray = new List<byte>();
            for (var i = startoffset; i < endoffset; i++)
                newarray.Add(_buffer[i]);
            return newarray.ToArray();
        }

        /// <summary>
        /// Returns number of bytes from current point.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public byte[] GetArray(int length)
        {
            if (length + ReadPosition > _buffer.Count - 1)
                return null;
            var newarray = new List<byte>();
            for (var i = ReadPosition; i <= ReadPosition + length; i++)
                newarray.Add(_buffer[i]);
            ReadPosition += length + 1;
            return newarray.ToArray();
        }




    }
}
