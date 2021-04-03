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
using DevExpress.XtraEditors;
using KAI.Common.Declarations;
using KAI.Declarations;

// ReSharper disable once CheckNamespace
namespace KAI.Interface
{
    public sealed partial class mainFrm : XtraForm
    {
        private void ClientCore_OnDataReceived(Packet receivedPacket)
        {
            if (receivedPacket.Opcode < (int) SMIOpcodes.KAI_OPCODE_AUTH)
            {
                var characterForm = StaticReference.GetCharacterForm(receivedPacket.Read<short>());
                if (characterForm == null)
                    return;

                #region Tracked player datapack

                switch (receivedPacket.Opcode)
                {
                    case (byte) SMIOpcodes.WIZ_MOVE:
                        characterForm.HandleTracedCharacterMove(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.WIZ_EXP_CHANGE:
                        characterForm.HandleTracedCharacterExpChange(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.WIZ_GOLD_CHANGE:
                        characterForm.HandleTracedCharacterGoldChange(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.WIZ_HP_CHANGE:
                        characterForm.HandleTracedCharacterHpChange(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.WIZ_MSP_CHANGE:
                        characterForm.HandleTracedCharacterMspChange(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.WIZ_LOYALTY_CHANGE:
                        characterForm.HandleTracedCharacterLoyaltyChange(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.WIZ_LEVEL_CHANGE:
                        characterForm.HandleTracedCharacterLevelChange(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.WIZ_ITEM_MOVE:
                        characterForm.HandleTracedCharacterItemMove(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.WIZ_TARGET_HP:
                        characterForm.HandleTracedCharacterTargetHP(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.WIZ_WARP:
                        characterForm.HandleTracedCharacterWarp(receivedPacket);
                        break;
                    case (byte)SMIOpcodes.WIZ_SKILLPT_CHANGE:
                        characterForm.HandleTracedCharacterSkillPointChange(receivedPacket);
                        break;
                    case (byte)SMIOpcodes.WIZ_POINT_CHANGE:
                        characterForm.HandleTracedCharacterStatPointChange(receivedPacket);
                        break;
                    case (byte)SMIOpcodes.WIZ_ITEM_COUNT_CHANGE:
                        characterForm.HandleTracedCharacterItemCountChange(receivedPacket);
                        break;
                    case (byte)SMIOpcodes.WIZ_ITEM_GET:
                        characterForm.HandleTracedCharacterItemGet(receivedPacket);
                        break;
                    case (byte)SMIOpcodes.WIZ_ITEM_TRADE:
                        characterForm.HandleTracedCharacterItemBuySell(receivedPacket);
                        break;
                    case (byte)SMIOpcodes.WIZ_PREMIUM:
                        characterForm.HandleTracedCharacterPremiumChange(receivedPacket);
                        break;


                }

                #endregion

            }
            else if ((SMIOpcodes)receivedPacket.Opcode == SMIOpcodes.KAI_OPCODE_PARTY)
                HandlePartyPacket(receivedPacket);
            else
            {
                switch (receivedPacket.Opcode)
                {case (byte) SMIOpcodes.SMI_CHAR_SET_CURRENT:
                        HandleCharSet(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.SMI_CHAR_ALL_INFO:
                        HandleCharAllInfo(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.SMI_ONLINE_COUNTS:
                        HandleOnlineCounts(receivedPacket);
                        Trace.TraceInformation("mainFrm::ClientCore_OnDataReceived() - Online counts packet received.");
                        break;
                    case (byte) SMIOpcodes.SMI_ONLINE_USERS:
                        HandleOnlineUsers(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.SMI_TECHNICAL_INFO:
                        HandleTechnicalInformation(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.SMI_TECHNICAL_INFO_UPDATE:
                        HandleTechnicalInformation(receivedPacket, true);
                        break;


                    case (byte) SMIOpcodes.SMI_AI_INFO:
                        Trace.TraceInformation("mainFrm::ClientCore_OnDataReceived() - AI information packet received.");
                        break;
                    case (byte) SMIOpcodes.SMI_AUTH_INFO:
                        Trace.TraceInformation(
                            "mainFrm::ClientCore_OnDataReceived() - Authorization information packet received.");
                        break;
                    case (byte) SMIOpcodes.SMI_REQUEST_ALL_INFO:
                        _receivedInitialInformation = receivedPacket.Read<bool>();
                        Trace.TraceWarning(
                            "mainFrm::ClientCore_OnDataReceived() - Request all info response received, {0}",
                            _receivedInitialInformation ? "true" : "false");
                        break;

                    case (byte) SMIOpcodes.SMI_NETWORK_INTERFACE_INFORMATION:
                        HandleNetworkInterfaceInformation(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.SMI_NETWORK_STATS_UPDATE:
                        HandleNetworkStatsUpdate(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.SMI_NETWORK_TCP_STATISTICS:
                        StaticReference.TCPStats =
                            StaticReference.ByteArrayToStructure<MIB_TCPSTATS>(receivedPacket.GetArray());
                        SafeAsyncInvoke(new Action(RefreshTCPStatistics));
                        break;
                    case (byte) SMIOpcodes.SMI_NETWORK_UDP_STATISTICS:
                        StaticReference.UDPStats =
                            StaticReference.ByteArrayToStructure<MIB_UDPSTATS>(receivedPacket.GetArray());

                        SafeAsyncInvoke(new Action(RefreshUDPStatistics));
                        break;
                    case (byte) SMIOpcodes.SMI_CHARACTER_LOGIN:
                        HandleCharacterLogin(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.SMI_CHARACTER_LOGOUT:
                        HandleCharacterLogout(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.SMI_CHARACTER_ZONECHANGE:
                        HandleCharacterZonechange(receivedPacket);
                        break;
                    case (byte)SMIOpcodes.SMI_CHAR_ITEMMOVE:
                        HandleCharacterItemMove(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.SMI_OPERATOR:
                        HandleOperatorResponse(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.SMI_ALTER_VALUE:
                        HandleValueAlterationResponse(receivedPacket);
                        break;
                    case (byte) SMIOpcodes.SMI_PRIVATE_CHAT:
                        HandlePrivateChatPacket(receivedPacket);
                        break;
                    case (byte)SMIOpcodes.SMI_CHAR_SAVEINVENALL:
                        HandleCharacterSaveInvenAll(receivedPacket);
                        break;
                    case (byte)SMIOpcodes.SMI_MANAGER_MYINFO:
                        HandleManagerMyinfo(receivedPacket);
                        break;
                    case (byte)SMIOpcodes.SMI_MANAGER_PENALTY:
                        HandleManagerPenalty(receivedPacket);
                        break;
                    case (byte)SMIOpcodes.KAI_OPCODE_PARTY:
                        HandlePartyPacket(receivedPacket);
                        break;
                    case (byte)SMIOpcodes.KAI_OPCODE_PREMIUM:
                        HandlePremiumPacket(receivedPacket);
                        break;
                    case (byte)SMIOpcodes.KAI_OPCODE_OPERATOR_CHAT:
                        HandleOperatorChat(receivedPacket);
                        break;
                    case (byte)SMIOpcodes.KAI_OPCODE_OPERATOR_INOUT:
                        HandleOperatorInout(receivedPacket);
                        break;
   

                    default:
                        Trace.TraceInformation(
                            "mainFrm::ClientCore_OnDataReceived() - Unknown packet received with opcode {0}",
                            receivedPacket.Opcode);
                        break;
                }
            }
        }

        private void HandleOperatorInout(Packet receivedPacket)
        {
           switch((KAIOperatorInoutSubopcodes)receivedPacket.Read<byte>())
            {
                case KAIOperatorInoutSubopcodes.KAI_OPERATOR_INOUT_LIST:
                    HandleOperatorInoutList(receivedPacket);
                    break;
                case KAIOperatorInoutSubopcodes.KAI_OPERATOR_INOUT_IN:
                    HandleOperatorInoutIn(receivedPacket);
                    break;
                case KAIOperatorInoutSubopcodes.KAI_OPERATOR_INOUT_OUT:
                    HandleOperatorInoutOut(receivedPacket);
                    break;
            }
        }

        private void HandleOperatorInoutOut(Packet receivedPacket)
        {
            ulong session_id = receivedPacket.Read<ulong>();
            OperatorInfo oid = StaticReference.GetOperatorInfo(session_id);
            if(oid != null)
            {
                StaticReference.RemoveOperatorTyping(oid.UserID);
                StaticReference.MainFormReference?.NotifyOperatorOffline(oid);
                StaticReference.MainFormReference?.RefreshChatOnlineOperatorList();
            }
            StaticReference.RemoveOnlineOperator(session_id);
        }

        private void HandleOperatorInoutIn(Packet receivedPacket)
        {
            OperatorInfo oi = new OperatorInfo()
            {
                SessionID = receivedPacket.Read<ulong>(),
                UserID = receivedPacket.Read<string>(true),
                Authority = receivedPacket.Read<byte>(),
                AuthDate = CommonReference.UnixTimeStampToDateTime(receivedPacket.Read<long>()),
                AuthorizedBy = receivedPacket.Read<string>(true),
                Name = receivedPacket.Read<string>(true),
                Surname = receivedPacket.Read<string>(true)
            };
            StaticReference.InsertOnlineOperator(oi);
            if (oi.UserID.CompareTo(CurrentManagerInfo.UserID) != 0)
            {
                StaticReference.MainFormReference?.NotifyOperatorOnline(oi);
               
            }
            StaticReference.MainFormReference?.RefreshChatOnlineOperatorList();
        }

        private void HandleOperatorInoutList(Packet receivedPacket)
        {
            uint count = receivedPacket.Read<uint>();
            for(int i = 0; i < count; i++){
                HandleOperatorInoutIn(receivedPacket);
            }
        }

        private void HandleOperatorChat(Packet receivedPacket)
        {
            switch((KAIChatSubopcodes)receivedPacket.Read<byte>())
            {
                case KAIChatSubopcodes.KAI_CHAT_MESSAGE:
                    HandleOperatorChatMessage(receivedPacket);
                    break;
                case KAIChatSubopcodes.KAI_CHAT_TYPING:
                    HandleOperatorChatTyping(receivedPacket);
                    break;
            }
        }

        private void HandleOperatorChatMessage(Packet receivedPacket)
        {
            ushort operator_id = receivedPacket.Read<ushort>();
            string operator_name = receivedPacket.Read<string>(true);
            byte operator_auth = receivedPacket.Read<byte>();
            string message = receivedPacket.Read<string>(true);
            
            StaticReference.MainFormReference?.SafeAsyncInvoke(new Action(() =>
            {
                StaticReference.MainFormReference?.InsertOperatorChat(operator_id, operator_name, operator_auth, message);
            }));
        }

       

        private void HandleOperatorChatTyping(Packet receivedPacket)
        {
            string user_id = receivedPacket.Read<string>(true);
            byte typing = receivedPacket.Read<byte>();
            if (typing == 0x01)
            {
                StaticReference.InsertOperatorTyping(user_id);
            }
            else
                StaticReference.RemoveOperatorTyping(user_id);
            StaticReference.MainFormReference?.SafeAsyncInvoke(new Action(() =>
            {
                StaticReference.MainFormReference.UpdateOperatorChatTypingLabel();
            }));
        }

        private void HandlePremiumPacket(Packet receivedPacket)
        {
            /* socket id */
                short socket_id = receivedPacket.Read<short>();
            frmCharacter chr = StaticReference.GetCharacterForm(socket_id);
            chr?.OnPremiumChangeResult(receivedPacket.Read<byte>());
        }

        private void HandlePartyPacket(Packet receivedPacket)
        {
            KAIPartySubopcodes subOpcode = (KAIPartySubopcodes)receivedPacket.Read<byte>();

            switch (subOpcode)
            {
                /* Individual party info */
                case KAIPartySubopcodes.KAI_PARTY_TRACK:
                    HandlePartyTrackResponse(receivedPacket);
                    break;
                case KAIPartySubopcodes.KAI_PARTY_CHAT:
                    HandlePartyChat(receivedPacket);
                    break;
                case KAIPartySubopcodes.KAI_PARTY_MEMBERUPDATE:
                    HandlePartyMemberUpdate(receivedPacket);
                    break;
                /* General purpose */
                case KAIPartySubopcodes.KAI_PARTY_ALLINFO:
                    HandlePartyAllinfo(receivedPacket);
                    break;
                case KAIPartySubopcodes.KAI_PARTY_CREATED:
                    HandlePartyCreate(receivedPacket);
                    break;
                case KAIPartySubopcodes.KAI_PARTY_DISBANDED:
                    HandlePartyDisband(receivedPacket);
                    break;
                case KAIPartySubopcodes.KAI_PARTY_MEMBERCOUNT_CHANGE:
                    HandlePartyMemberCountChange(receivedPacket);
                    break;
            }
        }

        private void HandlePartyChat(Packet receivedPacket)
        {
            StaticReference.GetPartyForm(receivedPacket.Read<ushort>())?.HandlePartyChat(receivedPacket);
        }

        private void HandlePartyTrackResponse(Packet receivedPacket)
        {
            PartyTrackSub type = (PartyTrackSub)receivedPacket.Read<byte>();
            KAIResultCodes result = (KAIResultCodes)receivedPacket.Read<byte>();
            switch(type)
            {
                case PartyTrackSub.PARTY_TRACK_SET:

                    switch (result)
                    {
                        case KAIResultCodes.KAI_RESULT_SUCCESS:
                            {
                                ushort party_id = receivedPacket.Read<ushort>();


                                StaticReference.frmCOMSink.BeginInvoke(new Action(() =>
                                {
                                    var frm = StaticReference.CreateNewPartyForm(party_id, StaticReference.MainFormReference);
                                    frm.Shown += (sender, e) =>
                                    {
                                        CheckForIllegalCrossThreadCalls = false;
                                        frm.LoadInitialPartyInfo(receivedPacket);
                                    };
                                    frm.Show();
                                    frm.BringToFront();

                                }));
                            }
                            break;
                        case KAIResultCodes.KAI_RESULT_ANOTHER_OPERATOR:
                            CommonReference.ShowWarning(this, "This party is already being tracked by another operator.");
                            break;
                        case KAIResultCodes.KAI_RESULT_FAILURE:
                            CommonReference.ShowError(this, "Party track failure!");
                            break;
                    }
                  
                    break;
                case PartyTrackSub.PARTY_TRACK_CLEAR:
                    break;
            }
          //  throw new NotImplementedException();
        }

        private void HandlePartyMemberUpdate(Packet receivedPacket) {
            StaticReference.GetPartyForm(receivedPacket.Read<ushort>())?.HandlePartyMemberUpdate(receivedPacket);  
        }

        private void HandlePartyMemberCountChange(Packet receivedPacket)
        {
            ushort sPartyId = receivedPacket.Read<ushort>();
            byte newCount = receivedPacket.Read<byte>();
            StaticReference.UpdatePartyMemberCount(sPartyId, newCount);
        }

        private void HandlePartyDisband(Packet receivedPacket)
        {
            ushort sPartyId = receivedPacket.Read<ushort>();

            StaticReference.RemovePartyForm(sPartyId);
         
            bool bResult = StaticReference.RemoveParty(sPartyId);
            Trace.TraceInformation("HandlePartyDisband() - {0}", bResult ? "Party removed" : "Party does not exist");

        }

        private void HandlePartyCreate(Packet receivedPacket)
        {
            ushort sPartyId = receivedPacket.Read<ushort>();


            PartyInfo newParty = new PartyInfo()
            {
                PartyID = sPartyId,
                LeaderID = receivedPacket.Read<short>(),
                Zone = receivedPacket.Read<byte>(),
                MemberCount = receivedPacket.Read<byte>()
            };

            for(var i = 0; i < newParty.MemberCount; i++)
            {
                newParty.MemberIDs[i] = receivedPacket.Read<short>();
            }

            StaticReference.InsertParty(newParty);
        }

      

        private void HandlePartyAllinfo(Packet receivedPacket)
        {
            uint partyCount = receivedPacket.Read<uint>();
            if(partyCount == 0)
            {
                StaticReference.ClearPartyList();
                return;
            }

            for(int i = 0; i < partyCount; i++)
            {
                PartyInfo pInfo = new PartyInfo();
                pInfo.PartyID = receivedPacket.Read<ushort>();
                pInfo.LeaderID = receivedPacket.Read<short>();
                pInfo.Zone = receivedPacket.Read<byte>();
                pInfo.MemberCount = receivedPacket.Read<byte>();
                if(pInfo.MemberCount > 0)
                {
                    for(int j = 0; j< pInfo.MemberCount; j++)
                    {
                        pInfo.MemberIDs[j] = receivedPacket.Read<short>();
                    }
                }
                StaticReference.InsertParty(pInfo);
            }
        }

        private void HandleManagerPenalty(Packet receivedPacket)
        {
            Trace.TraceInformation("Received penalty response from server");
            StaticReference.PunishFormReference?.OperationResponseReceive(receivedPacket.Read<byte>());
        }

        private void HandleManagerMyinfo(Packet receivedPacket)
        {
            CurrentManagerInfo.SessionID = receivedPacket.Read<UInt64>();
            CurrentManagerInfo.Authority = receivedPacket.Read<byte>();
            CurrentManagerInfo.UserID = receivedPacket.Read<string>(true);
            CurrentManagerInfo.AuthorizedBy = receivedPacket.Read<string>(true);
            CurrentManagerInfo.AuthDate = CommonReference.UnixTimeStampToDateTime(receivedPacket.Read<UInt64>());
            CurrentManagerInfo.Name = receivedPacket.Read<string>(true);
            CurrentManagerInfo.Surname = receivedPacket.Read<string>(true);
            StaticReference.MainFormReference?.SafeAsyncInvoke(new Action(() => { StaticReference.MainFormReference?.UpdateSessionInformation(); }));
        }

        private void HandleCharacterSaveInvenAll(Packet receivedPacket)
        {
            var characterForm = StaticReference.GetCharacterForm(receivedPacket.Read<short>());

            characterForm?.HandleTracedCharacterSaveInventoryResponse(receivedPacket);
        }

        private void HandleCharacterItemMove(Packet receivedPacket)
        {
           // Int16 sid = receivedPacket.Read<Int16>();
            var characterForm = StaticReference.GetCharacterForm(receivedPacket.Read<short>());

            characterForm?.HandleTracedCharacterItemMoveResult(receivedPacket);

        }

        private static void HandleOnlineCounts(Packet receivedPacket)
        {
            OnlineCounts.Maximum = receivedPacket.Read<int>();
            OnlineCounts.InGame = receivedPacket.Read<int>();
            OnlineCounts.Karus = receivedPacket.Read<int>();
            OnlineCounts.Elmorad = receivedPacket.Read<int>();
            OnlineCounts.Gamemaster = receivedPacket.Read<int>();
            OnlineCounts.Administrator = receivedPacket.Read<int>();

            
            StaticReference.MainFormReference.SafeAsyncInvoke(new Action(() => { StaticReference.MainFormReference.RefreshOnlineUserPanel(); }));
        }


        private static void HandlePrivateChatPacket(Packet receivedPacket)
        {
            var socketID = receivedPacket.Read<short>();
            var message = receivedPacket.Read<string>(true);
            var z = StaticReference.GetCharacterForm(socketID);
            z?.BeginInvoke(new Action(() => z.PrivateChatMsgRecv(message)));
        }

        private void HandleValueAlterationResponse(Packet receivedPacket)
        {
            switch (receivedPacket.Read<byte>())
            {
                case 0x01:
                case 0x02:
                case 0x03:
                    break;
            }

        }

        private void HandleOperatorResponse(Packet receivedPacket)
        {
            switch ((OperatorRequests)receivedPacket.Read<byte>())
            {
                case OperatorRequests.ATTACKDISABLE:
                    CommonReference.ShowInformation(this, "The user's attack ability is successfully taken.");
                    break;
                case OperatorRequests.ATTACKENABLE:
                    CommonReference.ShowInformation(this, "The user's attack ability is successfully given back.");
                    break;
                case OperatorRequests.BAN:
                    CommonReference.ShowInformation(this, "The user is banned successfully.");
                    break;
                case OperatorRequests.DISCONNECT:
                    CommonReference.ShowInformation(this, "The user is disconnected successfully.");
                    break;
                case OperatorRequests.MUTE:
                    CommonReference.ShowInformation(this, "The user is muted successfully.");
                    break;
                case OperatorRequests.UNMUTE:
                    CommonReference.ShowInformation(this, "The user is unmuted successfully.");
                    break;
            }
        }

        private void HandleCharSet(Packet receivedPacket)
        {
            switch (receivedPacket.Read<byte>())
            {
                case 0:
                    CommonReference.ShowWarning(this, "Character trace request failed.");
                    break;
                case 1:
                    Trace.WriteLine("HandleCharSet() - Character successfully set.");
                    break;
                case 2:
                    CommonReference.ShowWarning(this, "Character is already being traced.");
                    break;
            }
        }


        private static void HandleCharacterLogout(Packet receivedPacket)
        {
            var logoutcharSID = receivedPacket.Read<short>();

            if (logoutcharSID == -1)
                return;

            StaticReference.UserLogout(logoutcharSID);

            var logoutFrm = StaticReference.GetCharacterForm(logoutcharSID);
            logoutFrm?.OnCharacterLogout();
        }

        private static void HandleCharacterLogin(Packet pkt)
        {
            var logincharSID = pkt .Read<short>();

            if (logincharSID == -1)
                return;

            var player = new PlayerListEntry()
            {
                SocketID = logincharSID,
                PlayerID = pkt.Read<string>(true),
                AccountID = pkt.Read<string>(true),
                Level = pkt.Read<byte>(),
                Nation = (pkt.Read<byte>() == 1 ? "Karus" : "El Morad"),
                Job = pkt.Read<short>().ToString(),
                ClanID = pkt.Read<short>(),
                ClanName = pkt.Read<string>(true),
                ClanFame = pkt.Read<byte>().ToString(),
                Authority = pkt.Read<byte>().ToString(),
                PremiumType = pkt.Read<byte>().ToString(),
                Zone = pkt.Read<byte>().ToString(),
                InstanceID = pkt.Read<Int64>(),
                IP = pkt.Read<string>(true),
                Ping = pkt.Read<int>()

            };

            switch (player.Authority)
            {
                case "0":
                    player.Authority = "Administrator";
                    break;
                case "1":
                    player.Authority = "Player";
                    break;
                case "255":
                    player.Authority = "Banned";
                    break;
                case "11":
                    player.Authority = "Muted";
                    break;
                case "12":
                    player.Authority = "Attack Disabled";
                    break;
                case "9":
                    player.Authority = "Gamemaster";
                    break;
                default:
                    player.Authority = "Unknown";
                    break;

            }
            switch (player.PremiumType)
            {
                case "1":
                    player.PremiumType = "EXP";
                    break;
                case "2":
                    player.PremiumType = "DC";
                    break;
                case "3":
                    player.PremiumType = "Bronze";
                    break;
                case "4":
                    player.PremiumType = "Silver";
                    break;
                case "5":
                    player.PremiumType = "Gold";
                    break;
                case "6":
                    player.PremiumType = "WAR";
                    break;
                case "7":
                    player.PremiumType = "PLATINUM";
                    break;
                case "8":
                    player.PremiumType = "ROYAL";
                    break;
            }
            switch (player.ClanFame)
            {
                case "1":
                    player.ClanFame = "Leader";
                    break;

                case "2":
                    player.ClanFame = "Asistant";
                    break;
                case "5":
                    player.ClanFame = "Member";
                    break;
                case "0":
                    player.ClanFame = "None";
                    break;

            }

            var zone = CommonReference.GetZone(Convert.ToInt32(player.Zone));
            player.Zone = zone.Value;

            player.Job = CharacterClass.GetDescription(Convert.ToInt32(player.Job));
            StaticReference.InsertPlayer(player);
            if (null !=StaticReference.MainFormReference)
            {
                StaticReference.MainFormReference.Invoke(new Action(() =>
                {
                    StaticReference.MainFormReference.AlertManager.Show(StaticReference.MainFormReference, "Player online", $"Player {player.AccountID}:{player.PlayerID} ({player.Zone}) is online.");
                }));
            }
           // StaticReference.UserLogin(newChar);
          //  Invoke(new Action(() => OnCharacterLogin(newChar)));
        }

        private void HandleCharacterZonechange(Packet receivedPacket)
        {
            var zonechangeCharSid = receivedPacket.Read<short>();
            if (zonechangeCharSid == -1)
                return;

            var newZone = receivedPacket.Read<ushort>();
            StaticReference.UserZoneChange(zonechangeCharSid, newZone);
            SafeAsyncInvoke(new Action(() => OnCharacterZoneChange(zonechangeCharSid)));

            var chrFrm = StaticReference.GetCharacterForm(zonechangeCharSid);
            if (null != chrFrm)
            {
                chrFrm.CurrentUser.ZoneChange(newZone);
                chrFrm.RefreshZoneInformation();
                // chrFrm.CurrentUser
            }
        }


        private void HandleCharAllInfo(Packet receivedPacket)
        {
            switch (receivedPacket.Read<byte>())
            {
                case 1:
                 //   Thread.CurrentThread.SetApartmentState(ApartmentState.STA);
                    StaticReference.MainFormReference.SafeAsyncInvoke(
                        new Action(() => { new frmCharacterLoading().LoadCharacter(receivedPacket, this); }));
                    break;
                default:
                    CommonReference.ShowWarning(this, "Selected character could not be tracked.");
                    break;
            }
        }


        private void ClientCore_OnDisconnect()
        {
            Trace.TraceWarning("mainFrm::ClientCore_OnDisconnect() - Disconnected from server.");
            CommonReference.ShowWarning(this, "Server connection lost.\nYou will be redirected to the login page.");
            Logout();
        }

        private void Logout()
        {
            DetachEventHandlers();
            StaticReference.RecreateClient();
            // StaticReference.ClearOnlineUserList();
            StaticReference.CloseAllCharacterForms();
            /* Wi şuld ristor da fuken form. */
            _shouldRestoreLoginForm = true;
            Close();
        }


        private void HandleOnlineUsers(Packet pkt)
        {
         //   var onlineUsers = new List<OnlineUser>();
            var count = pkt.Read<UInt16>();
            for (var i = 0; i < count; i++)
            {

                var player = new PlayerListEntry()
                {
                    SocketID = pkt.Read<Int16>(),
                    PlayerID = pkt.Read<string>(true),
                    AccountID = pkt.Read<string>(true),
                    Level = pkt.Read<byte>(),
                    Nation = (pkt.Read<byte>() == 1 ? "Karus" : "El Morad"),
                    Job = pkt.Read<short>().ToString(),ClanID = pkt.Read<short>(),
                    ClanName = pkt.Read<string>(true),
                    ClanFame = pkt.Read<byte>().ToString(),
                    Authority = pkt.Read<byte>().ToString(),
                    PremiumType = pkt.Read<byte>().ToString(),
                    Zone = pkt.Read<byte>().ToString(),
                    InstanceID = pkt.Read<Int64>(),
                    IP = pkt.Read<string>(true),
                    Ping = pkt.Read<int>()

                };

                switch (player.Authority)
                {
                    case "0":
                        player.Authority = "Administrator";
                        break;
                    case "1":
                        player.Authority = "Player";
                        break;
                    case "255":
                        player.Authority = "Banned";
                        break;
                    case "11":
                        player.Authority = "Muted";
                        break;
                    case "12":
                        player.Authority = "Attack Disabled";
                        break;
                    case "9":
                        player.Authority = "Gamemaster";
                        break;
                    default:
                        player.Authority = "Unknown";
                        break;

                }
                switch (player.PremiumType)
                {
                    case "3":
                        player.PremiumType = "Bronze";
                        break;
                    case "4":
                        player.PremiumType = "Silver";
                        break;
                    case "5":
                        player.PremiumType = "Gold";
                        break;
                }
                switch (player.ClanFame)
                {
                    case "1":
                        player.ClanFame = "Leader";
                        break;
                        
                    case "2":
                        player.ClanFame = "Asistant";
                        break;
                    case "5":
                        player.ClanFame = "Member";
                        break;
                    case "0":
                        player.ClanFame = "None";
                        break;

                }

                var zone = CommonReference.GetZone(Convert.ToInt32(player.Zone));
                player.Zone = zone.Value;

                player.Job = CharacterClass.GetDescription(Convert.ToInt32(player.Job));
                StaticReference.InsertPlayer(player);

            }
         //   StaticReference.InitializeOnlineUsers(onlineUsers);
        //    Invoke(new Action(RefreshOnlineUsersList));
            Trace.TraceInformation("mainFrm::HandleOnlineUsers() - Online users packet received, count {0}", count);
        }
        private void HandleTechnicalInformation(Packet pkt, bool isUpdate = false)
        {
            if (!isUpdate)
            {
                StaticReference.TechInfo.CPU.Model = pkt.Read<string>(true);
                StaticReference.TechInfo.CPU.PhysicalCores = pkt.Read<byte>();
                StaticReference.TechInfo.CPU.LogicalCores = pkt.Read<byte>();
                StaticReference.TechInfo.CPU.Speed = pkt.Read<float>();
                StaticReference.TechInfo.CPU.Load = pkt.Read<float>();
                StaticReference.TechInfo.Memory.GameserverUsage = pkt.Read<ulong>();
                StaticReference.TechInfo.Memory.SQLUsage = pkt.Read<ulong>();
                StaticReference.SystemUptime = pkt.Read<long>();
                StaticReference.PerformanceInformation = StaticReference.ByteArrayToStructure<_PERFORMANCE_INFORMATION>(pkt.ReadRemainingBytes());
                SafeAsyncInvoke(new Action(RefreshTechnicalInformation)); Trace.TraceInformation("mainFrm::HandleTechnicalInformation() - Technical information packet received.");
            }
            else
            {
                StaticReference.TechInfo.CPU.Load = pkt.Read<float>();
                StaticReference.TechInfo.Memory.GameserverUsage = pkt.Read<ulong>();
                StaticReference.TechInfo.Memory.SQLUsage = pkt.Read<ulong>();
                StaticReference.PerformanceInformation = StaticReference.ByteArrayToStructure<_PERFORMANCE_INFORMATION>(pkt.ReadRemainingBytes());
                SafeAsyncInvoke(new Action(RefreshTechnicalInformation));
                Trace.TraceInformation(
                    "mainFrm::HandleTechnicalInformation() - Technical information (update) packet received.");
            }
        }

        private void HandleNetworkInterfaceInformation(Packet pkt)
        {
            StaticReference.NetworkInterface.Name = pkt.Read<string>(true);
            StaticReference.NetworkInterface.Type = pkt.Read<string>(true);
            StaticReference.NetworkInterface.Status = pkt.Read<string>(true);
            StaticReference.NetworkInterface.MACAddress = pkt.Read<string>(true);
            StaticReference.NetworkInterface.Speed = pkt.Read<uint>();
            StaticReference.NetworkInterface.MTU = pkt.Read<uint>();
            StaticReference.NetworkInterface.SentBytes = pkt.Read<UInt64>();
            StaticReference.NetworkInterface.RecvBytes = pkt.Read<UInt64>();
            StaticReference.NetworkInterface.SentErrors[0] = pkt.Read<uint>();
            StaticReference.NetworkInterface.SentErrors[1] = pkt.Read<uint>();
            StaticReference.NetworkInterface.RecvErrors[0] = pkt.Read<uint>();
            StaticReference.NetworkInterface.RecvErrors[1] = pkt.Read<uint>();
            StaticReference.NetworkInterface.RecvErrors[2] = pkt.Read<uint>();
            StaticReference.NetworkInterface.NetworkIP = pkt.Read<string>(true);
            StaticReference.NetworkInterface.HostIP = pkt.Read<string>(true);
            StaticReference.NetworkInterface.SubnetMask = pkt.Read<string>(true);
            SafeAsyncInvoke(new Action(RefreshNetworkInterfaceInformation));
        }

        private void HandleNetworkStatsUpdate(Packet pkt)
        {
            var s = pkt.Read<ulong>();
            var r = pkt.Read<ulong>();
            StaticReference.NetworkInterface.SentDelta = s - StaticReference.NetworkInterface.SentBytes;
            StaticReference.NetworkInterface.RecvDelta = r - StaticReference.NetworkInterface.RecvBytes;
            StaticReference.NetworkInterface.SentBytes = s;
            StaticReference.NetworkInterface.RecvBytes = r;
            SafeAsyncInvoke(new Action(RefreshNetworkInterfaceStats));
        }
    }
}
