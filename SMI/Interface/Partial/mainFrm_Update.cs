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
using System.Linq;
using KAI.Declarations;
using System.Drawing;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace KAI.Interface
{
    public sealed partial class mainFrm
    {

        #region Operator chat UI
        private void AddOperatorChatLine_Italic(string text, Color color)
        {
           
            rtbOperatorChat.SelectionFont = new Font("Verdana", 6.11f, FontStyle.Italic);
            rtbOperatorChat.SelectionColor = color;
            rtbOperatorChat.SelectedText = text + "\n";
            rtbOperatorChat.SelectionColor = Color.White;
            rtbOperatorChat.ScrollToCaret();

           
        }

        private void AddOperatorName(string text, Color color)
        {
            rtbOperatorChat.SelectionFont = new Font("Lucida Sans Unicode", 8.5f, FontStyle.Regular);
            rtbOperatorChat.SelectionColor = color;
            rtbOperatorChat.SelectedText = $">> {text}{"\n"}";
            rtbOperatorChat.SelectionColor = Color.White;
            rtbOperatorChat.ScrollToCaret();
        }

        private void AddOperatorChatLine(string op_name,string text, Color color)
        {
            bool textContainsFocus = teOperatorChat.ContainsFocus;
            rtbOperatorChat.Focus();
            rtbOperatorChat.SelectionStart = rtbOperatorChat.Text.Length;
            rtbOperatorChat.SelectionFont = new Font("Lucida Sans Unicode", 8.5f, FontStyle.Regular);
            rtbOperatorChat.SelectionColor = Color.White;
            rtbOperatorChat.SelectedText = $"({DateTime.Now.ToLongTimeString()}) ";
            rtbOperatorChat.SelectionColor = color;
            rtbOperatorChat.SelectedText = $"{op_name} : ";
            rtbOperatorChat.SelectionColor = Color.White;
            rtbOperatorChat.SelectedText = text + Environment.NewLine;
           
            rtbOperatorChat.ScrollToCaret();
            if (textContainsFocus)
                teOperatorChat.Focus();
        }
        #endregion

        public void UpdateOperatorChatTypingLabel()
        {
            HashSet<string> typingUsers = new HashSet<string>();
            StaticReference.GetTypingOperatorList(ref typingUsers);
            if(typingUsers.Count == 0)
            {
                lblOperatorChatTyping.Visible = false;
            }
            else
            {

                string result = "";
                foreach(var v in typingUsers)
                {
                    result += v + ",";
                }
                /* Remove the last comma */
                result = result.Remove(result.Length - 1, 1);
                if (typingUsers.Count == 1)
                {
                    result += " is typing..";
                }
                else
                    result += " are typing..";
        
                lblOperatorChatTyping.Text = result;
                lblOperatorChatTyping.Visible = true;
            }
        }

        private void RefreshChatOnlineOperatorList()
        {
            lbOperators.Items.Clear();
            List<OperatorInfo> oinfo = new List<OperatorInfo>();
            StaticReference.GetOnlineOperatorList(ref oinfo);
            foreach (var v in oinfo)
            {
                lbOperators.Items.Add($"{v.UserID} ({v.Name} {v.Surname})");
            }
        }

        public void NotifyOperatorOffline(OperatorInfo oi)
        {
            switch (oi.Authority)
            {
                case 0:
                    AlertManager.AppearanceCaption.ForeColor = Color.FromArgb(0, 213, 50, 0);
                    break;
                case 1:
                    AlertManager.AppearanceCaption.ForeColor = Color.OrangeRed;
                    break;
                case 2:
                    AlertManager.AppearanceCaption.ForeColor = Color.RoyalBlue;
                    break;
            }

            StaticReference.frmCOMSink.BeginInvoke(new Action(() =>
            {
                AlertManager.FormLocation = DevExpress.XtraBars.Alerter.AlertFormLocation.BottomRight;
                AlertManager.Show(this, "Operator " + oi.UserID, $"has logged out");

            }));
        }

        public void NotifyOperatorOnline(OperatorInfo oi)
        {
            switch (oi.Authority)
            {
                case 0:
                    AlertManager.AppearanceCaption.ForeColor = Color.FromArgb(0, 213, 50, 0);
                    break;
                case 1:
                    AlertManager.AppearanceCaption.ForeColor = Color.OrangeRed;
                    break;
                case 2:
                    AlertManager.AppearanceCaption.ForeColor = Color.RoyalBlue;
                    break;
            }

            StaticReference.frmCOMSink.BeginInvoke(new Action(() =>
            {
                AlertManager.FormLocation = DevExpress.XtraBars.Alerter.AlertFormLocation.BottomRight;
                AlertManager.Show(this, "Operator " + oi.UserID, $"has just signed in");

            }));
            
        }

        private void InsertOperatorChat(ushort operator_id, string operator_name, byte operator_auth, string message)
        {
            string result;

            result = $"[{operator_name}] : {message}";

            switch (operator_auth)
            {
                case 0:
                    AlertManager.AppearanceCaption.ForeColor = Color.FromArgb(0, 213, 50, 0);

                    AddOperatorChatLine(operator_name,message, Color.FromArgb(0, 213, 50, 0));
                    break;
                case 1:
                    AlertManager.AppearanceCaption.ForeColor = Color.OrangeRed;

                    AddOperatorChatLine(operator_name, message, Color.OrangeRed);
                    break;
                case 2:
                    AlertManager.AppearanceCaption.ForeColor = Color.RoyalBlue;
                    AddOperatorChatLine(operator_name, message, Color.RoyalBlue);
                    break;
            }

            if (CurrentManagerInfo.UserID.CompareTo(operator_name) != 0)
            {
                /* Global thread */
                StaticReference.frmCOMSink.BeginInvoke(new Action(() =>
                {
                    AlertManager.FormLocation = DevExpress.XtraBars.Alerter.AlertFormLocation.BottomRight;
                    AlertManager.Show(this, "New message from " + operator_name, $"\"{message}\"");

                }));
            }
         
        }

        private void OnCharacterLogin(OnlineUser newUser)
        {
          //  InsertOnlineUserNode(newUser);
        }

        private void OnCharacterLogout(short sid)
        {
          //  RemoveLeafByTag(sid);
        }

        private void OnCharacterZoneChange(short sid)
        {
          /*  var usr = StaticReference.GetOnlineUserBySocketID(sid);
            if (usr == null || sid == -1)
                return;*/

          //  RemoveLeafByTag(sid);
           // InsertOnlineUserNode(usr);
        }

        public void RefreshOnlineUserPanel()
        {
  
            ngOnlineCounts_AdminOnline.Text = OnlineCounts.Administrator.ToString();
            ngOnlineCounts_CurrentOnline.Text = OnlineCounts.Total.ToString();
            ngOnlineCounts_ElMoradOnline.Text = OnlineCounts.Elmorad.ToString();
            ngOnlineCounts_KarusOnline.Text = OnlineCounts.Karus.ToString();
            ngOnlineCounts_GMOnline.Text = OnlineCounts.Gamemaster.ToString();
        }



        private void RefreshNetworkInterfaceInformation()
        {
            tbNetworkInterfaceName.Text = StaticReference.NetworkInterface.Name;
            tbNetworkInterfaceType.Text = StaticReference.NetworkInterface.Type;
            tbNetworkInterfaceMAC.Text = StaticReference.NetworkInterface.MACAddress;
            tbNetworkInterfaceSpeed.Text = string.Format("{0} mbps", StaticReference.NetworkInterface.Speed / 1000000);
            tbNetworkInterfaceMTU.Text = string.Format("{0} byte(s)", StaticReference.NetworkInterface.MTU);
            tbNetworkInterfaceNIP.Text = StaticReference.NetworkInterface.NetworkIP;
            tbNetworkInterfaceHIP.Text = StaticReference.NetworkInterface.HostIP;
            tbNetworkInterfaceSM.Text = StaticReference.NetworkInterface.SubnetMask;
        }

        private void RefreshNetworkInterfaceStats()
        {
            tbNetworkTotalReceived.Text = string.Format("{0} byte(s), {1:F1} megabyte(s)",
                StaticReference.NetworkInterface.RecvBytes, StaticReference.NetworkInterface.RecvBytes / 1048576);
            tbNetworkRecvDelta.Text = string.Format("{0} byte(s), {1:F1} kilobyte(s)", StaticReference.NetworkInterface.RecvDelta, StaticReference.NetworkInterface.RecvDelta / 1024);
            tbNetworkTransmitTotal.Text = string.Format("{0} byte(s), {1:F1} megabyte(s)", StaticReference.NetworkInterface.SentBytes, StaticReference.NetworkInterface.SentBytes / 1048576);
            tbNetworkTransmitDelta.Text = string.Format("{0} byte(s), {1:F1} kilobyte(s)", StaticReference.NetworkInterface.SentDelta, StaticReference.NetworkInterface.SentDelta / 1024);//  tbnetworktotal

            NetworkGraph_RecvNewPoint(StaticReference.NetworkInterface.RecvDelta);
            NetworkGraph_TransmitNewPoint(StaticReference.NetworkInterface.SentDelta);
        }

        private void RefreshTechnicalInformation()
        {
            const int toMb = 1048576;
            var uptime = TimeSpan.FromMilliseconds(StaticReference.SystemUptime);

            float memLoadPhys = ((float)StaticReference.PerformanceInformation.GetPhysicalUsed() / (float)StaticReference.PerformanceInformation.GetPhysicalTotal()) * 100;
            float commit = ((float)StaticReference.PerformanceInformation.GetCommitTotal() / (float)StaticReference.PerformanceInformation.GetCommitLimit()) * 100;



            tbSystemUptime.Text =
                string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3}second(s) {4} millisecond(s)", uptime.Days, uptime.Hours, uptime.Minutes, uptime.Seconds, uptime.Milliseconds);
            dispCores.Text = string.Format("{0}/{1}", StaticReference.TechInfo.CPU.PhysicalCores, StaticReference.TechInfo.CPU.LogicalCores);
            dispProcesses.Text = string.Format("{0}", StaticReference.PerformanceInformation.GetProcessCount());
            tbCPUModel.Text = string.Format("{0}", StaticReference.TechInfo.CPU.Model);
            dispCoreSpeed.Text = string.Format("{0:F0} mhz", StaticReference.TechInfo.CPU.Speed);
            dispThreads.Text = string.Format("{0}", StaticReference.PerformanceInformation.GetThreadCount());


            arcMemLoadPhys.Value = memLoadPhys;
            arcCommit.Value = commit;
            arcScaleCPULoad.Value = StaticReference.TechInfo.CPU.Load;
            dispCPUUtilization.Text = string.Format("{0:F0}", StaticReference.TechInfo.CPU.Load);


            dispTotal.Text = string.Format("{0:D5} MB", (StaticReference.PerformanceInformation.GetPhysicalTotal() / toMb));
            dispAvailable.Text = string.Format("{0:D5} MB", StaticReference.PerformanceInformation.GetPhysicalAvailable() / toMb);
            dispInUse.Text = string.Format("{0:D5} MB", StaticReference.PerformanceInformation.GetPhysicalUsed() / toMb);
            dispGameserver.Text = string.Format("{0:D5} MB", StaticReference.TechInfo.Memory.GameserverUsage / toMb);
            dispCommitLimit.Text = string.Format("{0:D5} MB", StaticReference.PerformanceInformation.GetCommitLimit() / toMb);
            dispCommitTotal.Text = string.Format("{0:D5} MB", StaticReference.PerformanceInformation.GetCommitTotal() / toMb);
            dispCommitPeak.Text = string.Format("{0:D5} MB", StaticReference.PerformanceInformation.GetCommitPeak() / toMb);
            dispSystemCache.Text = string.Format("{0:D5} MB", StaticReference.PerformanceInformation.GetSystemCached() / toMb);
            dispKernelTotal.Text = string.Format("{0:D5} MB", StaticReference.PerformanceInformation.GetKernelTotal() / toMb);
            dispSQL.Text = string.Format("{0:D5} MB", StaticReference.TechInfo.Memory.SQLUsage / toMb);

            CPUGraph_AddNewPoint(StaticReference.TechInfo.CPU.Load);
            MemoryGraph_AddNewPoint(StaticReference.PerformanceInformation.GetPhysicalUsed() / toMb);

            arcScaleCpuAvg.Value = (float)_last15CpuUsage.Sum() / (float)_last15CpuUsage.Count;

        }

        private void RefreshTCPStatistics()
        {

            tbTCPEstablishedCount.Text = StaticReference.TCPStats.dwCurrEstab.ToString();
            tbTCPEstablishedReset.Text = StaticReference.TCPStats.dwEstabResets.ToString();
            tbTCPFailedCount.Text = StaticReference.TCPStats.dwAttemptFails.ToString();
            tbTCPListenerCount.Text = StaticReference.TCPStats.dwPassiveOpens.ToString();
            tbTCPOutgoingConnections.Text = StaticReference.TCPStats.dwActiveOpens.ToString();
            tbTCPSegmentRecv.Text = StaticReference.TCPStats.dwInSegs.ToString();
            tbTCPSegmentSent.Text = StaticReference.TCPStats.dwOutSegs.ToString();
            tbTCPTotalCount.Text = StaticReference.TCPStats.dwNumConns.ToString();
        }

        private void RefreshUDPStatistics()
        {
            tbUDPDatagramRecv.Text = StaticReference.UDPStats.dwInDatagrams.ToString();
            tbUDPDatagramSent.Text = StaticReference.UDPStats.dwOutDatagrams.ToString();
            tbUDPInErrors.Text = StaticReference.UDPStats.dwInErrors.ToString();
            tbUDPInvalidPort.Text = StaticReference.UDPStats.dwNoPorts.ToString();
            tbUDPListenerCount.Text = StaticReference.UDPStats.dwNumAddrs.ToString();
        }


        private void UpdateSessionInformation()
        {
            teOperatorID.Text = string.Format("{0}", CurrentManagerInfo.UserID);
            teOperatorAuthBy.Text = string.Format("{0}", CurrentManagerInfo.AuthorizedBy);
            teOperatorAuthSince.Text = string.Format("{0}", CurrentManagerInfo.AuthDate.ToShortDateString());
            switch (CurrentManagerInfo.Authority)
            {
                case 0:
                    teOperatorAuthLevel.Text = "Root Administrator";
                    break;
                case 1:
                    teOperatorAuthLevel.Text = "Administrator";
                    break;
                case 2:
                    teOperatorAuthLevel.Text = "Gamemaster";
                    break;
                default:
                    teOperatorAuthLevel.Text = "Unknown";
                    break; 
            }

            teOperatorName.Text= string.Format("{0}", CurrentManagerInfo.Name);
            teOperatorSurname.Text = string.Format("{0}", CurrentManagerInfo.Surname);
        /*    ngSession_UserID.Caption = string.Format("Logged in as {0}", loginName);
            ngSession_AuthorizedBy.Caption = string.Format("Authorized by {0}", authUser);
            ngSession_AuthDate.Caption = string.Format("Authorization date {0}", authDate.ToShortDateString());*/
        }



    }
}
