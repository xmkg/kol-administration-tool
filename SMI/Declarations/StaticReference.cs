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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using KAI.Core;
using KAI.Interface;
using System.Drawing;

namespace KAI.Declarations
{
    public static class StaticReference
    {

        public static byte[] HandshakeKey;
        public static mainFrm MainFormReference;
       // public static frmOperatorChat OperatorChatForm;
        public static frmPunishPlayer PunishFormReference;
        public static Credentials LoginCredentials;
        public static List<ServerEntry> Servers = new List<ServerEntry>();
        public static Credentials StoredCredentials;
        public static TechnicalInformation TechInfo = new TechnicalInformation();
        public static MIB_TCPSTATS TCPStats = new MIB_TCPSTATS();
        public static MIB_UDPSTATS UDPStats = new MIB_UDPSTATS();
        public static _PERFORMANCE_INFORMATION PerformanceInformation = new _PERFORMANCE_INFORMATION();
        public static InterfaceInformation NetworkInterface = new InterfaceInformation();
        public static Int64 SystemUptime = 0;
        public static DataTable PlayerTable = new DataTable("player_table");
        public static DataTable PartyTable = new DataTable("party_table");
        public static Dictionary<ushort, PartyInfo> Party = new Dictionary<ushort, PartyInfo>();
        private static ReaderWriterLock _partyRWLock = new ReaderWriterLock();
        private static Dictionary<string, Image> CountryFlags = new Dictionary<string, Image>();

        public static COMSink frmCOMSink;

     

        public static void DisableDropRecursive(Control parentCtrl)
        {
            foreach (Control c in parentCtrl.Controls)
            {
                c.AllowDrop = false;

                DisableDropRecursive(c);
            }
        }





        public static void LoadCountryFlags()
        {
            string _path = "./flagicons";
            if (Directory.Exists("./flagicons"))
            {
                foreach (var v in Directory.EnumerateFiles(_path))
                {
                    FileInfo fi = new FileInfo(v);
                    if (fi.Extension == ".png")
                    {
                        try
                        {
                            CountryFlags.Add(fi.Name.Replace(fi.Extension,""), Image.FromFile(fi.FullName));
                        }
                        catch { }

                    }
                }
            }
        }



        #region Chat interface operator typing list
        private static readonly HashSet<string> _operatorTyping = new HashSet<string>();
        private static ReaderWriterLock _operatorTypingLock = new ReaderWriterLock();

        public static void InsertOperatorTyping(string operatorName)
        {
            _operatorTypingLock.AcquireWriterLock(Timeout.Infinite);
            if(_operatorTyping.Contains(operatorName))
            {
                _operatorTypingLock.ReleaseWriterLock();
                return;
            }

            _operatorTyping.Add(operatorName);
            _operatorTypingLock.ReleaseWriterLock();
        }

        public static void RemoveOperatorTyping(string operatorName)
        {
            _operatorTypingLock.AcquireWriterLock(Timeout.Infinite);
            _operatorTyping.Remove(operatorName);
            _operatorTypingLock.ReleaseWriterLock();
        }

        public static void GetTypingOperatorList(ref HashSet<string> list)
        {
            _operatorTypingLock.AcquireReaderLock(Timeout.Infinite);
            foreach (var s in _operatorTyping)
                list.Add(s);
            _operatorTypingLock.ReleaseReaderLock();
        }
        #endregion


        #region Tracked user forms

        private static readonly Dictionary<int, frmCharacter> CharacterForms = new Dictionary<int, frmCharacter>();
        private static ReaderWriterLock _characterFormRwLock = new ReaderWriterLock();
        private static readonly object CharacterFormArrayLock = new object();

        public static frmCharacter CreateNewCharacterForm(short socketID, mainFrm parent)
        {
            _characterFormRwLock.AcquireWriterLock(Timeout.Infinite);

            if (CharacterForms.ContainsKey(socketID))
                goto skip;


            CharacterForms.Add(socketID, new frmCharacter(parent));

            skip:
            _characterFormRwLock.ReleaseWriterLock();
            return CharacterForms[socketID];
        }

        public static bool RemoveCharacterForm(short socketID)
        {

            _characterFormRwLock.AcquireWriterLock(Timeout.Infinite);

            if (!CharacterForms.ContainsKey(socketID))
            {
                _characterFormRwLock.ReleaseWriterLock();
                return false;
            }

            CharacterForms.Remove(socketID);
            _characterFormRwLock.ReleaseWriterLock();
            return true;

        }

        public static void CloseAllCharacterForms()
        {
            _characterFormRwLock.AcquireWriterLock(Timeout.Infinite);

            foreach (var v in CharacterForms)
            {
                v.Value.FromMaster = true;
                v.Value.Close();
                v.Value.Dispose();
            }
            CharacterForms.Clear();
            _characterFormRwLock.ReleaseWriterLock();
        }

        public static frmCharacter GetCharacterForm(short socketID)
        {
            _characterFormRwLock.AcquireReaderLock(Timeout.Infinite);
            if (CharacterForms.ContainsKey(socketID))
            {
                _characterFormRwLock.ReleaseReaderLock();
                return CharacterForms[socketID];
            }
            _characterFormRwLock.ReleaseReaderLock(); return null;
        }



        #endregion

        #region Tracked party forms

        private static readonly Dictionary<ushort, frmParty> PartyForms = new Dictionary<ushort, frmParty>();
        private static ReaderWriterLock _partyFormRwLock = new ReaderWriterLock();


        public static frmParty CreateNewPartyForm(ushort socketID, mainFrm parent)
        {
            _partyFormRwLock.AcquireWriterLock(Timeout.Infinite);

            if (PartyForms.ContainsKey(socketID))
                goto skip;


            PartyForms.Add(socketID, new frmParty(parent,socketID));

            skip:
            _partyFormRwLock.ReleaseWriterLock();
            return PartyForms[socketID];
        }

        public static bool RemovePartyForm(ushort socketID,bool userClosing = false)
        {

            _partyFormRwLock.AcquireWriterLock(Timeout.Infinite);

            if (!PartyForms.ContainsKey(socketID))
            {
                _partyFormRwLock.ReleaseWriterLock();
                return false;
            }

            frmParty fParty = GetPartyForm(socketID);
            /* I need this operator in C++ too */
            frmCOMSink.BeginInvoke(new Action(() =>
            {
                fParty?.OnPartyDisband(userClosing);
            }));
            PartyForms.Remove(socketID);
            _partyFormRwLock.ReleaseWriterLock();
            return true;

        }

        public static void CloseAllPartyForms()
        {
            _partyFormRwLock.AcquireWriterLock(Timeout.Infinite);

            foreach (var v in PartyForms)
            {
                v.Value.FromMaster = true;
                v.Value.Close();
                v.Value.Dispose();
            }
            PartyForms.Clear();
            _partyFormRwLock.ReleaseWriterLock();
        }

        public static frmParty GetPartyForm(ushort partyID)
        {
            _partyFormRwLock.AcquireReaderLock(Timeout.Infinite);
            if (PartyForms.ContainsKey(partyID))
            {
                _partyFormRwLock.ReleaseReaderLock();
                return PartyForms[partyID];
            }
            _partyFormRwLock.ReleaseReaderLock(); return null;
        }

        #endregion

        #region Client

        public static TcpClient Client = new TcpClient();

        public static ClientCore ClientCore = null;

        public static string GetServerIPAddress()
        {
            return ((IPEndPoint)Client.Client.RemoteEndPoint).Address.ToString();
        }
        public static void RecreateClient()
        {
            if (ClientCore != null)
                ClientCore.Dispose();
            if (Client != null)
                Client.Close();
            Client = new TcpClient();
        }

        #endregion

        #region Party operation

        public static void InitializePartyTable()
        {
            /* Reset primary key(s) */
            PartyTable.PrimaryKey = new DataColumn[] { };
            /* Erase columns */
            PartyTable.Columns.Clear();
            /* Erase row(s) */
            PartyTable.Rows.Clear();
            /* Don't know wtf is that command doing, but execute it.*/
            PartyTable.Clear();
            PartyTable.Columns.Add("Party ID", typeof(long));
            PartyTable.Columns.Add("Leader Name", typeof(string));
            PartyTable.Columns.Add("Zone", typeof(string));
            PartyTable.Columns.Add("Member Count", typeof(byte));
            PartyTable.PrimaryKey = new[] {PartyTable.Columns[0] };
        }

        public static bool UpdatePartyMemberCount(UInt16 sPartyID, byte newCount)
        {
            DataRow dr = PartyTable.Rows.Find(sPartyID);
            if (dr == null)
                return false;

            dr["Member Count"] = newCount;
            return true;
        }
        public static bool InsertParty(PartyInfo party)
        {

            if (MainFormReference == null)
                return false;
            
            PartyTable.BeginLoadData();
            var dr = PartyTable.NewRow();
            dr["Party ID"] = party.PartyID;
            dr["Leader Name"] = GetPlayerName((ushort)party.LeaderID);
            dr["Zone"] = Common.Declarations.CommonReference.GetZone(party.Zone).Value;
            dr["Member Count"] = party.MemberCount;

            MainFormReference.Invoke(new Action(() => { PartyTable.Rows.Add(dr); }));

            PartyTable.EndLoadData();
            return true;
        }

        public static PartyInfo GetParty(ushort id)
        {
            /*    DataRow dr = PlayerTable.Rows.Find(id);
                PartyInfo pi = new PartyInfo()
                {
                    PartyID = 
                };
                return p;*/
            return null;
        }

        public static bool RemoveParty(ushort id)
        {
            if (MainFormReference == null)
                return false;
            
            MainFormReference.Invoke(new Action(() =>
            {
                var row = PartyTable.Rows.Find(id);
                row?.BeginEdit();
                if (row != null)
                {
                   PartyTable.Rows.Remove(row);
                }
            }
            ));


            return true;
        }

        public static void ClearPartyList()
        {
            _partyRWLock.AcquireWriterLock(Timeout.Infinite);
            Party.Clear();
            _partyRWLock.ReleaseWriterLock();
        }





        #endregion

        #region Online user list operation (new)


        public static void InitializePlayerTable()
        {
            /* Reset primary key(s) */
            PlayerTable.PrimaryKey = new DataColumn[] {};
            /* Erase columns */
            PlayerTable.Columns.Clear();
            /* Erase row(s) */
            PlayerTable.Rows.Clear();
            /* Don't know wtf is that command doing, but execute it.*/
            PlayerTable.Clear();
            PlayerTable.Columns.Add("SID", typeof (long));
            PlayerTable.Columns.Add("Player ID", typeof(string));
            PlayerTable.Columns.Add("Account ID", typeof(string));
            PlayerTable.Columns.Add("Level", typeof(byte));
            PlayerTable.Columns.Add("Nation", typeof(string));
            PlayerTable.Columns.Add("Class", typeof(string));
            PlayerTable.Columns.Add("Clan", typeof(string));
            PlayerTable.Columns.Add("Fame",typeof(string));
            PlayerTable.Columns.Add("Authority", typeof(string));
            PlayerTable.Columns.Add("Premium", typeof(string));
            PlayerTable.Columns.Add("Zone", typeof(string));
            PlayerTable.Columns.Add("Instance", typeof(string));
            PlayerTable.Columns.Add("IP", typeof(string));
            PlayerTable.Columns.Add("Country", typeof(string));
            PlayerTable.Columns.Add("Flag", typeof(Image));
            PlayerTable.Columns.Add("Ping", typeof(string));
            /* TODO : Add country flag */
            PlayerTable.PrimaryKey = new[] { PlayerTable.Columns[0] };
        }

        public static string GetPlayerName(UInt16 playerID)
        {
             DataRow dr =  PlayerTable.Rows.Find(playerID);

            return (dr != null) ? Convert.ToString(dr["Player ID"]) : "unknown";
        }

        public static DataRow GetPlayer(UInt16 playerID)
        {
            return PlayerTable.Rows.Find(playerID);
        }

        public static bool InsertPlayer(PlayerListEntry ple)
        {

            if (MainFormReference == null)
                return false;
            KeyValuePair<string, string> country_code = new KeyValuePair<string, string>("europeanunion", "Unknown");
            try
            {
                country_code = IPCountryCodeHelper.QueryIPCountryCodeAndName(IPAddress.Parse(ple.IP));
            }
            catch (Exception ex){ }
            
            PlayerTable.BeginLoadData();
            var dr = PlayerTable.NewRow();
            dr["SID"] = ple.SocketID;
            dr["Player ID"] = ple.PlayerID;
            dr["Account ID"] = ple.AccountID;
            dr["Level"] = ple.Level;
            dr["Nation"] = ple.Nation;
            dr["Class"] = ple.Job;
            dr["Clan"] = ple.ClanID;
            dr["Fame"] = ple.ClanFame;
            dr["Authority"] = ple.Authority;
            dr["Premium"] = ple.PremiumType;
            dr["Zone"] = ple.Zone;
            dr["Instance"] = ple.InstanceID;
            dr["IP"] = ple.IP;
            dr["Country"] = country_code.Value;
           // if(CountryFlags.ContainsKey(country_code.Key.ToLowerInvariant()))
            dr["Flag"] = CountryFlags[country_code.Key.ToLowerInvariant()];
            dr["Ping"] = ple.Ping;

         //   

            OnlineCounts.InGame++;
           
            switch (Convert.ToString(dr["Authority"]))
            {
                case "Administrator":
                    OnlineCounts.Administrator++;
                    break;
                case "Gamemaster":
                    OnlineCounts.Gamemaster++;
                    break;
            }

            switch (Convert.ToString(dr["Nation"]))
            {
                case "Karus":
                    OnlineCounts.Karus++;
                    break;
                case "El Morad":
                    OnlineCounts.Elmorad++;
                    break;
            }
            MainFormReference.Invoke(new Action(() => { MainFormReference.RefreshOnlineUserPanel(); }));
            



            /* 
            dr.
                Data tables does not like multithreading. 
                So, our best shot is executing all commands in main ui thread. 
            */
            MainFormReference.Invoke(new Action(() => { PlayerTable.Rows.Add(dr); }));
            
            PlayerTable.EndLoadData();
            return true;
        }

        public static bool RemovePlayer(long playerID)
        {

            if (MainFormReference == null)
                return false;

            MainFormReference.Invoke(new Action(() =>
                {
                    var row = PlayerTable.Rows.Find(playerID);
                    row?.BeginEdit();
                    if (row != null)
                    {
                        OnlineCounts.InGame--;

                        switch (Convert.ToString(row["Authority"]))
                        {
                            case "Administrator":
                                OnlineCounts.Administrator--;
                                break;
                            case "Gamemaster":
                                OnlineCounts.Gamemaster--;
                                break;
                        }

                        switch (Convert.ToString(row["Nation"]))
                        {
                            case "Karus":
                                OnlineCounts.Karus--;
                                break;
                            case "El Morad":
                                OnlineCounts.Elmorad--;
                                break;
                        }
                        MainFormReference.Invoke(new Action(() => { MainFormReference.RefreshOnlineUserPanel(); }));
                        PlayerTable.Rows.Remove(row);
                    }
                }
                ));
             
            
            return true;
        }

        #endregion

        #region Online operator list operation 

        private static Dictionary<ulong, OperatorInfo> _onlineOperatorList = new Dictionary<ulong, OperatorInfo>();
        private readonly static ReaderWriterLock _onlineOperatorRW = new ReaderWriterLock();
        public static void InsertOnlineOperator(OperatorInfo info)
        {
            _onlineOperatorRW.AcquireWriterLock(Timeout.Infinite);

            if(!_onlineOperatorList.ContainsKey(info.SessionID))
            {
                _onlineOperatorList.Add(info.SessionID, info);
            }
            _onlineOperatorRW.ReleaseWriterLock();
        }

        public static void RemoveOnlineOperator(ulong sessID)
        {
            _onlineOperatorRW.AcquireWriterLock(Timeout.Infinite);
            if (_onlineOperatorList.ContainsKey(sessID))
            {
                _onlineOperatorList.Remove(sessID);
            }
            _onlineOperatorRW.ReleaseWriterLock();
        }

        public static void GetOnlineOperatorList(ref List<OperatorInfo> infos)
        {
            _onlineOperatorRW.AcquireReaderLock(Timeout.Infinite);
            foreach (var v in _onlineOperatorList)
                infos.Add(v.Value);
            _onlineOperatorRW.ReleaseReaderLock();
        }

        public static OperatorInfo GetOperatorInfo(ulong sessid)
        {
            _onlineOperatorRW.AcquireReaderLock(Timeout.Infinite);
            OperatorInfo oi = null;
            if (_onlineOperatorList.ContainsKey(sessid))
                oi = _onlineOperatorList[sessid];
            _onlineOperatorRW.ReleaseReaderLock();
            return oi;
        }

        #endregion

        #region User in/out


        public static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var stuff = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(),
                typeof(T));
            handle.Free();
            return stuff;
        }

        

        public static void UserLogin(OnlineUser newUser)
        {
           
        }

        public static void UserLogout(int socketID)
        {
            RemovePlayer(socketID);
         }

        public static void UserZoneChange(int socketID, ushort newZone)
        {
            /* Todo : implement support again */
        }

        #endregion

        #region Save / Load

        private const string CredentialsFilename = @".\Credentials.xml";
        private const string HandshakeFilename = @".\handshake.smikey";
        private const string ServerDataFilename = @".\ServerData.xml";
        public static bool DeserializeCredentials()
        {
            if (!File.Exists(CredentialsFilename))
                return false;

            var x = new XmlSerializer(typeof(Credentials));
            using (TextReader reader = new StreamReader(CredentialsFilename))
            {
                StoredCredentials = (Credentials)x.Deserialize(reader);
            }

            return true;
        }

        public static bool DeserializeServerData()
        {
            if (!File.Exists(ServerDataFilename))
                return false;

            var x = new XmlSerializer(typeof(List<ServerEntry>));
            using (TextReader reader = new StreamReader(ServerDataFilename))
            {
                var dsr = x.Deserialize(reader);

                foreach (var v in (List<ServerEntry>)dsr)
                {
                    Servers.Add(v);
                }
            }

            return true;
        }

        public static bool ReadHandshakeKey()
        {
            if (!File.Exists(HandshakeFilename))
                return false;
            using (var fs = new FileStream(HandshakeFilename, FileMode.Open))
            {
                if (!fs.CanRead) return false;
                using (var br = new BinaryReader(fs))
                {
                    HandshakeKey = new byte[fs.Length];
                    for (var i = 0; i < fs.Length; i++)
                    {
                        HandshakeKey[i] = br.ReadByte();
                    }
                    return true;
                }
            }
        }

        public static bool SerializeCredentials()
        {
            var x = new XmlSerializer(typeof(Credentials));
            using (TextWriter writer = new StreamWriter(CredentialsFilename))
            {
                x.Serialize(writer, StoredCredentials);
            }
            return true;
        }
        public static bool SerializeServerData()
        {
            var x = new XmlSerializer(typeof(List<ServerEntry>));

            using (TextWriter writer = new StreamWriter(ServerDataFilename))
            {
                x.Serialize(writer, Servers);
            }
            return true;
        }
        #endregion

       

    }
}
