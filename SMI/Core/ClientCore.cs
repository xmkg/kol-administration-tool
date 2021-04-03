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
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using KAI.Declarations;

namespace KAI.Core
{
    public class ClientCore : IDisposable
    {
        public ClientCore(TcpClient client)
        {
            _client = client;
            _stream = _client.GetStream();/* Start receive worker thread */
            new Thread(RecvWorker)
            {
                Name = "Client core receive worker thread",
                Priority = ThreadPriority.AboveNormal,
                IsBackground = true,
                // ApartmentState = ApartmentState.STA
            }.Start();

        }

        #region IDisposable Inherited Members

        public void Dispose()
        {
            Disconnect();
            Trace.TraceWarning("ClientCore::Dispose() - Object disposed.");
            /* Invoke garbage collection as well. */
            GC.Collect();
        }

        #endregion

        #region Thread Worker

        private void RecvWorker(object p)
        {
            try
            {
                while (_working)
                {
                    var rSize = _stream.Read(_tempBuffer, 0, _tempBuffer.Length);

                    if (rSize <= 0)
                    {
                        Disconnect();
                        return;
                    }
                    _readBuffer.Put(_tempBuffer, 0, rSize);
                    while (ParseBuffer()) { }
                }
            }
            catch (IOException ie)
            {
                Trace.WriteLine(string.Format("IO exception occured.\nDetails: {0}\nStack trace : {1}", ie.Message,
                  ie.StackTrace));
                Disconnect();
            }
            catch (SocketException se)
            {
                Trace.WriteLine(string.Format("Socket exception occured.\nDetails: {0}\nStack trace : {1}", se.Message,
                    se.StackTrace));
                Disconnect();
            }
            catch (Exception ex)
            {
                /* We should investigate any other kind of errors. */
                Trace.Assert(false, ex.Message, ex.StackTrace);
                Disconnect();
            }
        }

        #endregion

        #region Packet Parser

        private bool ParseBuffer()
        {
            var bFoundTail = false;
            var iCount = _readBuffer.GetValidCount();

            /* Check if we can issue a read */
            if (iCount < 7)
                goto fail_return;

            var header = BitConverter.ToUInt16(_readBuffer.Get(2), 0);
            var rByteCnt = 2;
            if (header == 0xAA55)
            {
                var siCore = BitConverter.ToInt16(_readBuffer.Get(2), 0);
                rByteCnt += 2;

                if (siCore == 0)
                {
                    Trace.TraceError("ClientCore::ParseBuffer() - Packet size is either too small or too large!");
                }

                if (siCore > iCount - 6)
                {
                    _readBuffer.Rewind(rByteCnt);
                    Trace.TraceWarning("ClientCore::ParseBuffer() - Rewind - Header : {0}",
                        BitConverter.ToInt16(_readBuffer.Get(2), 0).ToString("X"));
                    _readBuffer.Rewind(2);
                    goto fail_return;
                }

                var pData = new byte[siCore + 2];
                _readBuffer.Get(pData, 0, siCore + 2);

                var tail = BitConverter.ToUInt16(pData, pData.Length - 2);

                if (tail != 0x55AA)
                {
                    Trace.TraceWarning("ClientCore::ParseBuffer() - Tail mismatch.");
                    goto fail_return;
                }

                /* Invoke data receive callback */
                if (OnDataReceived != null)
                {
                    var receivedPacket = new Packet(pData[0]);
                    receivedPacket.Append(pData, 1, siCore - 1);
                    OnDataReceived.Invoke(receivedPacket);
                }
                bFoundTail = true;
            }
            else
            {
                Trace.TraceWarning("ClientCore::ParseBuffer() - Broken packet, got header [{0}].", header);
                _readBuffer.Clear();
            }
            fail_return:
            return bFoundTail;
        }

        #endregion

        #region Declare

        private readonly TcpClient _client;
        private readonly CircularBuffer<byte> _readBuffer = new CircularBuffer<byte>(65536, true);
        private readonly NetworkStream _stream;
        private readonly byte[] _tempBuffer = new byte[32768];
        private Boolean _disconnected;
        private Boolean _working = true;

        public delegate void DisconnectCallback();

        public delegate void ReceiveCallback(Packet receivedPacket);

        public event ReceiveCallback OnDataReceived;
        public event DisconnectCallback OnDisconnect;

        #endregion

        #region Functionality

        public void Disconnect()
        {
            if (_disconnected) return;
            _disconnected = true;
            _working = false;
            _stream.ReadTimeout = 1;

            _stream.Close();
            _client.Close();
            OnDisconnect?.Invoke();

            _stream.Dispose();
            Trace.TraceWarning("ClientCore::Disconnect() - Disconnected.");
        }

        public void Send(Packet pkt)
        {
            _stream.Write(pkt.GetSendBytes(), 0, pkt.GetSendBytes().Length);
        }

        #endregion

        #region Protocol

        public void SendOperatorTyping(byte value)
        {
            var pkt = new Packet((byte)SMIOpcodes.KAI_OPCODE_OPERATOR_CHAT, (byte)KAIChatSubopcodes.KAI_CHAT_TYPING);
            pkt.Append(value);
            Send(pkt);
        }

        public void SendOperatorChat(string message)
        {
            var pkt = new Packet((byte)SMIOpcodes.KAI_OPCODE_OPERATOR_CHAT, (byte)KAIChatSubopcodes.KAI_CHAT_MESSAGE);
            pkt.Append(message);
            Send(pkt);
        }
        public void SendPremiumChangeReq(string strAccountID, byte type, ulong startDate, ulong expirationDate)
        {
            var pkt = new Packet((byte)SMIOpcodes.KAI_OPCODE_PREMIUM);
            pkt.Append(strAccountID, true);
            pkt.Append(type);
            pkt.Append(startDate);
            pkt.Append(expirationDate);
            Send(pkt);
        }

        public void SendPartyDisbandReq(ushort sPartyID)
        {
            var pkt = new Packet((byte)SMIOpcodes.KAI_OPCODE_PARTY, (byte)KAIPartySubopcodes.KAI_PARTY_OPERATOR);
            pkt.Append((byte)KAIPartyOperationSub.KAI_PARTY_OP_DISBAND);
            Send(pkt);
        }

        public void SendPartyChat(ushort sPartyID, string sender, string message)
        {
            var pkt = new Packet((byte)SMIOpcodes.KAI_OPCODE_PARTY, (byte)KAIPartySubopcodes.KAI_PARTY_CHAT);
            pkt.Append(sPartyID);
            pkt.Append(sender, true);
            pkt.Append(message, true);
            Send(pkt);
        }

        public void SendPrivateChat(Int16 socketID, string message)
        {
            var pkt = new Packet((byte)SMIOpcodes.SMI_PRIVATE_CHAT);
            pkt.Append(socketID);
            pkt.Append(message, true);
            Send(pkt);
        }

        public void SendZoneChange(Int16 socketID, byte zoneChangeType, ushort newZone)
        {
            var pkt = new Packet((byte)SMIOpcodes.SMI_ZONE_CHANGE);
            pkt.Append(zoneChangeType);
            pkt.Append(socketID);
            pkt.Append(newZone);
            Send(pkt);
        }

        public void SendValueAlteration(Int16 socketID, byte valueType, byte operationType, UInt32 amount)
        {
            // 0x1 gold, 0x2 exp, 0x3 loyalty |||| 0x1 give, 0x2 take
            var pkt = new Packet((byte)SMIOpcodes.SMI_ALTER_VALUE);
            pkt.Append(valueType);
            pkt.Append(operationType);
            pkt.Append(socketID);
            pkt.Append(amount);
            Send(pkt);
        }

        public void SendStatAlteration(short socketID, byte statType, byte flag)
        {
            var pkt = new Packet((byte) SMIOpcodes.SMI_CHAR_STAT_ALTERATION);
            pkt.Append(socketID);
            pkt.Append(statType);
            pkt.Append(flag);
            Send(pkt);
        }

        public void SendSkillAlteration(short socketID, byte skillType, byte flag)
        {
            var pkt = new Packet((byte)SMIOpcodes.SMI_CHAR_SKILL_ALTERATION);
            pkt.Append(socketID);
            pkt.Append(skillType);
            pkt.Append(flag);
            Send(pkt);
        }


        public void SendOperatorRequest(Int16 socketID, byte operationCode)
        {
            var pkt = new Packet((byte)SMIOpcodes.SMI_OPERATOR);
            pkt.Append((byte)operationCode);
            pkt.Append(socketID);
            Send(pkt);
        }

        public void SendItemMoveRequest(Int16 socketID, byte srcPos, byte dstPos)
        {
            var pkt = new Packet((byte) SMIOpcodes.SMI_CHAR_ITEMMOVE);
            pkt.Append(socketID);
            pkt.Append(srcPos);
            pkt.Append(dstPos);
            Send(pkt);
        }

        public void SendPenaltyRequest(AccountPenalty thePenalty)
        {
            var pkt = new Packet((byte)SMIOpcodes.SMI_MANAGER_PENALTY);
            pkt.Append((byte)thePenalty.TypeOfOperation);
            pkt.Append((byte)thePenalty.TypeOfPenalty);
            pkt.Append((byte)thePenalty.TypeOfName);
            pkt.Append(thePenalty.UserID, true);
            pkt.Append(thePenalty.Reason, true);
            pkt.Append(thePenalty.StartDate);
            pkt.Append(thePenalty.EndDate);
            pkt.Append(Convert.ToByte(thePenalty.Permanent));
            Send(pkt);
            Trace.Write("ClientCore::SendAccontPenaltyRequest() - Penalty request sent.");
        }


        public void SendTrackingUserUnset(Int16 socketID)
        {
            var pkt = new Packet((byte)SMIOpcodes.SMI_CHAR_UNSET_TRACKING);
            pkt.Append(socketID);
            Send(pkt);
            Trace.WriteLine("ClientCore::SendTrackingUserUnset() - Tracking user unset request sent.");
        }

        public void SendTrackingUserSet(Int16 socketID)
        {
            var pkt = new Packet((byte)SMIOpcodes.SMI_CHAR_SET_CURRENT);
            pkt.Append(socketID);
            Send(pkt);
            Trace.WriteLine("ClientCore::SendTrackingUserSet() - Tracking user set request sent.");
        }

        public void SendCharacterInfoRequest(Int16 socketID)
        {
            var pkt = new Packet((byte)SMIOpcodes.SMI_CHAR_ALL_INFO);
            Send(pkt);
            Trace.WriteLine("ClientCore::SendCharacterInfoRequest() - Character info request sent.");
        }

        public void SendCharacterSaveInventoryRequest(Int16 socketID, InventorySlot[] slots)
        {
            var pkt = new Packet((byte) SMIOpcodes.SMI_CHAR_SAVEINVENALL);
         //   Trace.Assert(slots.Length >= 71);
            pkt.Append(socketID);
            foreach (var v in slots)
            {
                pkt.Append(v.ItemID);
                pkt.Append((short)v.Durability);
                pkt.Append((short)v.Count);
                pkt.Append(v.Flag);
                pkt.Append(v.RemainingRentalTime);
                pkt.Append(v.ExpirationTime);
                pkt.Append(v.Serial);
            }
            Send(pkt);
        }

        public void SendAllInfoRequest()
        {
            var pkt = new Packet((byte)SMIOpcodes.SMI_REQUEST_ALL_INFO);
            Send(pkt);
            Trace.WriteLine("ClientCore::SendAllInfoRequest() - All info request sent.");
        }

        public void SendHandshake(byte[] key, int len)
        {
            var pkt = new Packet((byte)SMIOpcodes.KAI_OPCODE_AUTH,(byte)KAIAuthSubopcodes.KAI_AUTH_HANDSHAKE);
            pkt.Append(key, 0, len);
            Send(pkt);
            Trace.WriteLine("ClientCore::SendHandshake() - Handshake request sent.");
        }

        public void SendLogin(Credentials ci)
        {
            var pkt = new Packet((byte)SMIOpcodes.KAI_OPCODE_AUTH, (byte)KAIAuthSubopcodes.KAI_AUTH_LOGIN);
            //  pkt.
            pkt.Append(ci.ManagerID);
            pkt.Append(ci.Password);
            Send(pkt);
            Trace.WriteLine("ClientCore::SendLogin() - Login request sent.");
        }

        public void SendNetworkUpdate(byte enabled, int interval)
        {
            var pkt = new Packet((byte)SMIOpcodes.SMI_NETWORK_STATS_UPDATE);
            pkt.Append(enabled);
            pkt.Append(interval);
            Send(pkt);
        }
        public void SendTechnicalUpdate(byte enabled, int interval)
        {
            var pkt = new Packet((byte)SMIOpcodes.SMI_TECHNICAL_INFO_UPDATE);
            pkt.Append(enabled);
            pkt.Append(interval);
            Send(pkt);
        }


        public void SendPartyTrackRequest(ushort sPartyID,bool set = true)
        {
            var pkt = new Packet((byte)SMIOpcodes.KAI_OPCODE_PARTY, (byte)KAIPartySubopcodes.KAI_PARTY_TRACK);
            pkt.Append(set ? (byte)PartyTrackSub.PARTY_TRACK_SET : (byte)PartyTrackSub.PARTY_TRACK_CLEAR);
            pkt.Append(sPartyID);
            Send(pkt);
            Trace.WriteLine("ClientCore::SendPartyTrackRequest() - Party track request sent.");
        }


        #endregion
    }
}