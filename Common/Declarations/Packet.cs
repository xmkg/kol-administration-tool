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
using System.IO;

namespace KAI.Declarations
{
    /// <summary>
    /// Packet prototype
    /// </summary>
    public sealed class Packet : ByteBuffer
    {
        public Packet(byte opcode, byte subOpcode) { Opcode = opcode; Append(subOpcode); }

        public Packet(byte opcode) { Opcode = opcode; }
        public Packet(Packet p) { Opcode = p.Opcode; foreach (byte b in p.GetBuffer) Append(b); }
        public Packet(IList<byte> packet)
        {
            Opcode = packet[0];
            for (var i = 1; i < packet.Count; i++)
                Append(packet[i]);
        }
        public Packet() { Opcode = 0; }
        public ushort Len { get; set; }

        public byte Opcode { get; set; }
        public byte SubOpcode { get { return GetBuffer[0]; } }

        public byte[] GetArrayWithOp()
        {
            var newarray = new List<byte> { Opcode };
            newarray.AddRange(GetBuffer);
            return newarray.ToArray();
        }

        public byte[] GetBytes()
        {
            var myP = new byte[Size + 1];
            using (var stream = new MemoryStream(myP))
            {
                using (var bw = new BinaryWriter(stream))
                {
                    bw.Write(Opcode);
                    foreach (byte b in GetBuffer)
                        bw.Write(b);
                }
            }
            return myP;
        }
        public byte[] GetSendBytes()
        {
            var myP = new byte[Size + 7];
            using (var stream = new MemoryStream(myP))
            {
                using (var bw = new BinaryWriter(stream))
                {

                    bw.Write((UInt16)0x55AA);

                    bw.Write((UInt16)(1 + Size));
                    bw.Write(Opcode);
                    foreach (var b in GetBuffer)
                        bw.Write(b);

                    bw.Write((UInt16)0xAA55);
                }
            }
            return myP;
        }
    }
}
