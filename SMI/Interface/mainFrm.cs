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
using System.Diagnostics;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraNavBar;
using KAI.Common.Declarations;
using KAI.Core;
using KAI.Declarations;

namespace KAI.Interface
{
    public sealed partial class mainFrm : XtraForm
    {

        private readonly Button _enter = new Button();
        private readonly Button _escape = new Button();

        private UInt64 _graphIndexCPU, _graphIndexMEM, _graphIndexNetSent, _graphIndexNetRecv;
        private bool _receivedInitialInformation;
        private bool _serverInfoUpdateEnabled, _userTrackingUpdateEnabled, _networkStatsUpdateEnabled;

        private int _serverInfoUpdateInterval = 500,
            _networkStatsUpdateInterval = 1000;

        private bool _shouldRestoreLoginForm = false;
        private readonly List<double> _last15CpuUsage = new List<double>();
        private readonly frmLogin _loginForm;

        public void SafeAsyncInvoke(Delegate method)
        {
            if (IsHandleCreated)
            {
                BeginInvoke(method);
            }
        }

       // private System.Drawing.Point startuplocation;
        public mainFrm(frmLogin lgn)
        {
            StaticReference.MainFormReference = this;
            HandleCreated += mainFrm_HandleCreated;
            Shown += MainFrm_Shown;
            _loginForm = lgn;
            InitializeComponent();

       /*     this.startuplocation = this.Location;
            this.Location = new System.Drawing.Point(-1000, -1000);*/

            AttachEventHandlers();

            Text = string.Format("KAI version {0}   ({1}@{2})", System.Reflection.Assembly.GetExecutingAssembly()
                                           .GetName()
                                           .Version
                                           .ToString(),
                StaticReference.LoginCredentials.ManagerID, StaticReference.GetServerIPAddress());
            //cbAIZone.Properties.Items.AddRange(CommonReference.ZoneList.Values);
            tbServiceHost.Text = StaticReference.GetServerIPAddress();

            FormClosed += mainFrm_FormClosed;
            FormClosing += mainFrm_FormClosing;
            InitializePlayerGrid();
            InitializePartyGrid();

            CancelButton = _escape;
            AcceptButton = _enter;
            _escape.Click += Escape_Click;
            _enter.Click += Enter_Click;
        }

        private void MainFrm_Shown(object sender, EventArgs e)
        {
           // this.Location = this.startuplocation;
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            if (!teOperatorChat.EditorContainsFocus)
                return;
            StaticReference.ClientCore.SendOperatorChat(teOperatorChat.Text);
            teOperatorChat.Text = string.Empty;
            teOperatorChat.Focus();
        }

        private void Escape_Click(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
        }




        private void mainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_shouldRestoreLoginForm)
                return;var result = CommonReference.ShowQuestion(this, "Do you want to exit application?");
            if (result != DialogResult.Yes)
                e.Cancel = true;}

        private void mainFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_shouldRestoreLoginForm)
                _loginForm.Invoke(new Action(() => _loginForm.RestoreLoginUI()));
            else
            {
                
                StaticReference.ClientCore.Disconnect();
                StaticReference.RecreateClient();
                /* Close the COM sink */
                StaticReference.frmCOMSink.Close();
                Environment.Exit(0);
            }
            StaticReference.MainFormReference = null;
        }

        private void InitializePartyGrid()
        {
            StaticReference.InitializePartyTable();
            gvPartyMainView.Columns.Clear();
            gcPartyGridControl.BeginUpdate();
            gcPartyGridControl.DataSource = StaticReference.PartyTable;

            foreach(GridColumn col in gvPartyMainView.Columns)
            {
                col.AppearanceCell.Options.UseTextOptions = true;
                col.AppearanceHeader.Options.UseTextOptions = true;

                col.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                col.AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                col.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                col.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                col.BestFit();
            }

            gcPartyGridControl.Refresh();
            gcPartyGridControl.EndUpdate();

            gvPartyMainView.RefreshData();
            gvPartyMainView.OptionsBehavior.Editable = false;
            gvPartyMainView.OptionsBehavior.ReadOnly = true;
            gvPartyMainView.OptionsSelection.EnableAppearanceFocusedCell = false;
            gcPartyGridControl.Refresh();
        }

        private void InitializePlayerGrid()
        {
            StaticReference.InitializePlayerTable();


            gcOnlinePlayersMainView.Columns.Clear();
            gcOnlinePlayers.BeginUpdate();
            gcOnlinePlayers.DataSource = StaticReference.PlayerTable;foreach (GridColumn col in gcOnlinePlayersMainView.Columns)
            {
                col.AppearanceCell.Options.UseTextOptions = true;
                col.AppearanceHeader.Options.UseTextOptions = true;

                col.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                col.AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                col.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                col.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                if (col.Name == "IP")
                {
                    col.MinWidth = col.ColumnType != typeof (string) ? 150 : 200;
                    col.OptionsColumn.FixedWidth = true;
                }
                else
                {
                    col.BestFit();}
                // col.MinWidth = col.ColumnType != typeof(string) ? 150 : 200;
               // col.OptionsColumn.FixedWidth = true;
            }

            gcOnlinePlayers.Refresh();
            gcOnlinePlayers.EndUpdate();


            gcOnlinePlayersMainView.RefreshData();
            gcOnlinePlayersMainView.OptionsBehavior.Editable = false;
            gcOnlinePlayersMainView.OptionsBehavior.ReadOnly = true;
            gcOnlinePlayersMainView.OptionsSelection.EnableAppearanceFocusedCell = false;
            gcOnlinePlayers.Refresh();
         
        }

        private void mainFrm_Load(object sender, EventArgs e)
        {
            _loginForm.Hide();

            //cbChatZone.Properties.Items.AddRange(CommonReference.ZoneList.Values);
            cbTaxZone.Properties.Items.AddRange(CommonReference.ZoneList.Values);
            ToastManager.TryCreateApplicationShortcut();
           

        }

        private static void mainFrm_HandleCreated(object sender, EventArgs e)
        {
            /* 
                 * As the BeginInvoke and Invoke calls require a valid window handle,
                 * we must assure that the window handle is created before requesting anything.
                 */
            StaticReference.ClientCore.SendAllInfoRequest();

        }


        #region UI Related



       

        //   private Int16 GetUserIDByName()

        private void NetworkGraph_TransmitNewPoint(float val)
        {
            chartNetworkSent.Series.BeginUpdate();

            if (chartNetworkSent.Series[0].Points.Count >= 50)
                chartNetworkSent.Series[0].Points.RemoveAt(0);
            chartNetworkSent.Series[0].Points.Add(new SeriesPoint((long) _graphIndexNetSent++, val));

            chartNetworkSent.Series.EndUpdate();
        }

        private void NetworkGraph_RecvNewPoint(float val)
        {
            chartNetworkRecv.Series.BeginUpdate();

            if (chartNetworkRecv.Series[0].Points.Count >= 50)
                chartNetworkRecv.Series[0].Points.RemoveAt(0);
            chartNetworkRecv.Series[0].Points.Add(new SeriesPoint((long) _graphIndexNetRecv++, val));

            chartNetworkSent.Series.EndUpdate();
        }

        private void CPUGraph_AddNewPoint(float val)
        {
            chartCPU.Series.BeginUpdate();

            if (chartCPU.Series[0].Points.Count >= 50)
                chartCPU.Series[0].Points.RemoveAt(0);

            if (_last15CpuUsage.Count >= 15)
                _last15CpuUsage.RemoveAt(0);

            chartCPU.Series[0].Points.Add(new SeriesPoint((long) _graphIndexCPU++,
                val));
            chartCPU.Series.EndUpdate();
            _last15CpuUsage.Add(StaticReference.TechInfo.CPU.Load);
        }

        private void MemoryGraph_AddNewPoint(float val)
        {
            chartMemory.Series.BeginUpdate();

            if (chartMemory.Series[0].Points.Count >= 50)
                chartMemory.Series[0].Points.RemoveAt(0);
            chartMemory.Series[0].Points.Add(new SeriesPoint((long) _graphIndexMEM++,
                val));
            chartMemory.Series.EndUpdate();
        }



        #endregion

 

        #region Events

        private void AttachEventHandlers()
        {
            StaticReference.ClientCore.OnDataReceived += ClientCore_OnDataReceived;
            StaticReference.ClientCore.OnDisconnect += ClientCore_OnDisconnect;
        }



        private void btnRefreshAvailablity_Click(object sender, EventArgs e)
        {
            State_LoginServer.StateIndex = 2;
            State_FTPServer.StateIndex = 2;
            State_GameServer.StateIndex = 2;
            State_HTTPSServer.StateIndex = 2;
            State_HTTPServer.StateIndex = 2;
            State_SQLRemote.StateIndex = 2;
            new PortChecker(StaticReference.GetServerIPAddress(), 15001, GameserverAvailabilityCheck_Callback).Check();
            new PortChecker(StaticReference.GetServerIPAddress(), 15100, LoginAvailabilityCheck_Callback).Check();
            new PortChecker(StaticReference.GetServerIPAddress(), 21, FTPAvailabilityCheck_Callback).Check();
            new PortChecker(StaticReference.GetServerIPAddress(), 80, HTTPAvailablityCheck_Callback).Check();
            new PortChecker(StaticReference.GetServerIPAddress(), 443, HTTPSAvailablityCheck_Callback).Check();
            new PortChecker(StaticReference.GetServerIPAddress(), 1433, SQLRemoteAvailabilityChekc_Callback).Check();
        }

        private void DetachEventHandlers()
        {
            StaticReference.ClientCore.OnDataReceived -= ClientCore_OnDataReceived;
            StaticReference.ClientCore.OnDisconnect -= ClientCore_OnDisconnect;
        }

        private void SQLRemoteAvailabilityChekc_Callback(PortChecker _ref, bool state)
        {
            State_SQLRemote.StateIndex = state ? 3 : 1;
            _ref.Dispose();
            Trace.TraceInformation("mainFrm::SQLRemoteAvailabilityCheck_Callback() - FTP server state : {0}",
                state ? "online" : "offline");
        }

        private void FTPAvailabilityCheck_Callback(PortChecker _ref, bool state)
        {
            State_FTPServer.StateIndex = state ? 3 : 1;
            _ref.Dispose();
            Trace.TraceInformation("mainFrm::FTPAvailabilityCheck_Callback() - FTP server state : {0}",
                state ? "online" : "offline");
        }

        private void GameserverAvailabilityCheck_Callback(PortChecker _ref, bool state)
        {
            State_GameServer.StateIndex = state ? 3 : 1;
            _ref.Dispose();
            Trace.TraceInformation("mainFrm::GameserverAvailabilityCheck_Callback() - Gameserver state : {0}",
                state ? "online" : "offline");
        }

        private void hlAbout_Click(object sender, EventArgs e)
        {
            using (var a = new frmAbout())
                a.ShowDialog();
        }

        private void HTTPAvailablityCheck_Callback(PortChecker _ref, bool state)
        {
            State_HTTPServer.StateIndex = state ? 3 : 1;
            _ref.Dispose();
            Trace.TraceInformation("mainFrm::HTTPAvailabilityCheck_Callback() - HTTP server state : {0}",
                state ? "online" : "offline");
        }

        private void HTTPSAvailablityCheck_Callback(PortChecker _ref, bool state)
        {
            State_HTTPSServer.StateIndex = state ? 3 : 1;
            _ref.Dispose();
            Trace.TraceInformation("mainFrm::HTTPSAvailabilityCheck_Callback() - HTTPS server state : {0}",
                state ? "online" : "offline");
        }

        private void LoginAvailabilityCheck_Callback(PortChecker _ref, bool state)
        {
            State_LoginServer.StateIndex = state ? 3 : 1;
            _ref.Dispose();
            Trace.TraceInformation("mainFrm::LoginAvailabilityCheck_Callback() - LoginServer state : {0}",
                state ? "online" : "offline");
        }

    
        private void cmsOnlineUserList_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void nbOptions_ChangeSkin_LinkPressed(object sender, NavBarLinkEventArgs e)
        {
           ItemInformation ii = CommonReference.GetItemDetails(119111121);
           return;
        }



   

   

        private void ngOperations_btnPunish_Click(object sender, EventArgs e)
        {
            using (frmPunishPlayer fpp = new frmPunishPlayer())
                fpp.ShowDialog();
        }

        private void tcMain_Click(object sender, EventArgs e)
        {

        }

        private void groupControl26_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmsPartyTrack_Click(object sender, EventArgs e)
        {
            int[] rows = gvPartyMainView.GetSelectedRows();
            if (rows.Length == 0)
            {
                CommonReference.ShowWarning(this, "There is no party selected for this operation.");
                return;
            }

            var firstSelectedRow = gvPartyMainView.GetDataRow(rows[0]);
            ///firstSelectedRow["SID"]
            StaticReference.ClientCore.SendPartyTrackRequest(Convert.ToUInt16(firstSelectedRow["Party ID"]));
        }

        private void cmsParty_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        public void trackedCharacterLink_HyperlinkClick(object sender, DevExpress.Utils.HyperlinkClickEventArgs e)
        {
            HyperlinkLabelControl it = sender as HyperlinkLabelControl;
            if (it == null)
                return;
            frmCharacter form = StaticReference.GetCharacterForm(Convert.ToInt16(it.Tag));
            if (form == null) return;
            form.BringToFront();
            form.Focus();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            if (CommonReference.ShowQuestion(this, "Do you want to log out?") == DialogResult.Yes)
                Logout();
        }

        static bool operator_typing = false;
        private void teOperatorChat_EditValueChanged(object sender, EventArgs e)
        {
            
            if(teOperatorChat.Text == string.Empty)
            {
                StaticReference.ClientCore.SendOperatorTyping(0x00);
                operator_typing = false;
            }
            else
            {
                if (!operator_typing)
                {
                    StaticReference.ClientCore.SendOperatorTyping(0x01);
                    operator_typing = true;
                }
            }
        }

   

        private void rtbOperatorChat_LinkClicked(object sender, LinkClickedEventArgs e){
            Process.Start(e.LinkText);
        }

        private void AlertManager_AlertClick(object sender, DevExpress.XtraBars.Alerter.AlertClickEventArgs e)
        {
            // StaticReference.MainFormReference.ForceActivateWindow(null);
            CommonReference.Restore(this);
            StaticReference.MainFormReference?.BringToFront();
            StaticReference.MainFormReference.tcMain.SelectedTabPage = tpChat;
        }

        private void tsNetworkLiveFeed_Toggled(object sender, EventArgs e)
        {
            _networkStatsUpdateEnabled = tsNetworkLiveFeed.IsOn;
            _networkStatsUpdateInterval = (int)seNetworkLiveFeedInt.Value;
            StaticReference.ClientCore.SendNetworkUpdate((byte)(tsNetworkLiveFeed.IsOn ? 1 : 0),
                (int)seNetworkLiveFeedInt.Value);
        }

        private void tsTechnicalDetailsLiveFeed_Toggled(object sender, EventArgs e)
        {
            _serverInfoUpdateEnabled = tsTechnicalDetailsLiveFeed.IsOn;
            _serverInfoUpdateInterval = (int)seTechnicalDetailsInt.Value;
            StaticReference.ClientCore.SendTechnicalUpdate((byte)(tsTechnicalDetailsLiveFeed.IsOn ? 1 : 0),
                (int)seTechnicalDetailsInt.Value);

        }

        private void sbUpdateNetworkLiveFeed_Click(object sender, EventArgs e)
        {
            _serverInfoUpdateInterval = (int)seNetworkLiveFeedInt.Value;
            StaticReference.ClientCore.SendTechnicalUpdate((byte)(tsNetworkLiveFeed.IsOn ? 1 : 0),
                (int)seNetworkLiveFeedInt.Value);
        }

        private void sbUpdateTechInfoLiveFeed_Click(object sender, EventArgs e)
        {
            _serverInfoUpdateInterval = (int)seTechnicalDetailsInt.Value;
            StaticReference.ClientCore.SendTechnicalUpdate((byte)(tsTechnicalDetailsLiveFeed.IsOn ? 1 : 0),
                (int)seTechnicalDetailsInt.Value);
        }





        #endregion

  

        private void tsmiTrack_Click(object sender, EventArgs e)
        {
            int[] rows = gcOnlinePlayersMainView.GetSelectedRows();
            if (rows.Length == 0)
            {
                CommonReference.ShowWarning(this, "There is no player selected for this operation.");
                return;
            }

            var firstSelectedRow = gcOnlinePlayersMainView.GetDataRow(rows[0]);StaticReference.ClientCore.SendTrackingUserSet(Convert.ToInt16(firstSelectedRow["SID"]));
        }



    }
}