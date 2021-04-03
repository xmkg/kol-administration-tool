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
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using KAI.Common.Declarations;
using KAI.Core;
using KAI.Declarations;


namespace KAI.Interface
{
    public sealed partial class frmLogin : XtraForm
    {

        private ServerEntry _selectedServerEntry;

        #region Topmost enforcement

      
        #endregion

        #region UI Related

        public frmLogin()
        {
            // Font = new Font(Font.Name, 8.25f * 96f / CreateGraphics().DpiX, Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            mainBarManager.ItemClick += BarManager_OnItemClick;
            Width -= 445;
           // CommonReference.MakeTopmost(this);
            this.AutoScaleMode = AutoScaleMode.None;

        }

        public void RestoreLoginUI()
        {
            Invoke(new Action(() => {
                gcCredentials.Visible = true;
                gcServers.Visible = true;
                dbbConnect.Visible = true;
                gcConnecting.Visible = false;
                gcConnecting.Dock = DockStyle.None;
                Width = gcCredentials.Width + gcServers.Width - 445;
                CenterToScreen();
                Show();
                BringToFront();
            }));
        
        }

        private void AddHeader(string header)
        {
            BarItem bi = new BarHeaderItem();
            bi.Caption = header;
            serversMenu.AddItem(bi);
        }

        private void AddSubMenuButton(string caption)
        {
            serversMenu.AddItem(new BarButtonItem(mainBarManager, caption));
        }

        void BarManager_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var bi = e.Item;
            foreach (var v in StaticReference.Servers.Where(v => string.Compare(v.Name, 0, bi.Caption, 0, v.Name.Length) == 0))
            {
                _selectedServerEntry = v;
            }
        }

        private void btnAddServer_Click(object sender, EventArgs e)
        {
            ushort port;
            if (string.IsNullOrEmpty(tbServerName.Text) || string.IsNullOrEmpty(tbServerDomain.Text) ||
                string.IsNullOrEmpty(tbServerPort.Text))
            {
                CommonReference.ShowWarning(this, "One of the required fields are empty.\nFill it and try again.");
                return;
            }
            if (!ushort.TryParse(tbServerPort.Text, out port))
            {
                CommonReference.ShowWarning(this, string.Format("{0} is not a valid port number.", tbServerPort.Text));
                return;
            }
            var se = new ServerEntry
            {
                Name = tbServerName.Text,
                Host = tbServerDomain.Text,
                Port = port
            };
            StaticReference.Servers.Add(se);
            StaticReference.SerializeServerData(); RefreshServersList();
            CommonReference.ShowInformation(this, "New server information successfully added.");
        
        }

        private void btnRemoveSelectedServers_Click(object sender, EventArgs e)
        {

            foreach (var v in clbServersToRemove.CheckedItems)
            {
                foreach (var z in StaticReference.Servers)
                {
                    if (v.ToString() == z.Name)
                    {
                        StaticReference.Servers.Remove(z);
                        break;
                    }
                }
            }
            StaticReference.SerializeServerData(); RefreshServersList();
        }

        private void cbExtendServers_CheckedChanged(object sender, EventArgs e)
        {

            if (!gcNewServer.Visible)
            {
                RefreshServersList();
                gcNewServer.Visible = true;
                gcRemove.Visible = true;
                cbExtendServers.Text = @"<";
            }
            else
            {
                tbServerPort.Text = string.Empty;
                tbServerName.Text = string.Empty;
                tbServerDomain.Text = string.Empty; gcNewServer.Visible = false;
                gcRemove.Visible = false;
                Width -= 445;
                cbExtendServers.Text = @">";
            }
            CenterToScreen();
        }

        private void dbbConnect_Click(object sender, EventArgs e)
        {
            if (_selectedServerEntry == null)
            {
                CommonReference.ShowInformation(this, "Please select a server from drop-down menu below the button.\n");
                return;
            }
            if (chbRemember.Checked)
            {
                StaticReference.StoredCredentials = new Credentials
                {
                    ManagerID = tbManagerID.Text,
                    Password = tbPassword.Text
                };
                StaticReference.SerializeCredentials();
            }
            else
            {
                try
                {
                    File.Delete(".\\Credentials.xml");
                }
                catch
                {
                    // ignored
                }
            }
            DisplayConnectUI(_selectedServerEntry);
            ConnectRemoteHost(_selectedServerEntry);
        }

        private void DisplayConnectUI(ServerEntry se)
        {

            ppLogin.Caption = @"Establishing connection";
            ppLogin.Description = @"Please wait...";
            gcCredentials.Visible = false;
            gcServers.Visible = false;
            dbbConnect.Visible = false;
            gcConnecting.Visible = true;
            gcConnecting.Dock = DockStyle.Left;
            Width = gcConnecting.Width;
            lblServerName.Text = se.Host + "(" + se.Port + ")";

            CenterToScreen();

        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            StaticReference.DeserializeServerData();
            StaticReference.DeserializeCredentials();
            if (!StaticReference.ReadHandshakeKey())
            {
                CommonReference.ShowWarning(this, "Handshake key does not exist.\nObtain a proper handshake key from administrator.");
                Application.Exit();
            }
            RefreshServersList();


            if (StaticReference.StoredCredentials == null) return;

            chbRemember.Checked = true;
            tbManagerID.Text = StaticReference.StoredCredentials.ManagerID;
            tbPassword.Text = StaticReference.StoredCredentials.Password;
           
        }
        private void RefreshServersList()
        {
            serversMenu.ClearLinks();
            clbServersToRemove.Items.Clear();
            AddHeader("Available SMI servers");
            foreach (var v in StaticReference.Servers)
            {
                AddSubMenuButton(string.Format("{0} ({1}:{2})", v.Name, v.Host, v.Port)); clbServersToRemove.Items.Add(v.Name);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
           
            
          //  new XtraForm1().Show();
            ushort port;
            if (string.IsNullOrEmpty(tbServerName.Text) || string.IsNullOrEmpty(tbServerDomain.Text) ||
                string.IsNullOrEmpty(tbServerPort.Text))
            {
                CommonReference.ShowWarning(this, "One of the required fields are empty.\nFill it and try again.");
                return;
            }
            if (!ushort.TryParse(tbServerPort.Text, out port))
            {
                CommonReference.ShowWarning(this, string.Format("{0} is not a valid port number.", tbServerPort.Text));
                return;
            }

            using (var tcp = new TcpClient())
            {
                try
                {
                    tcp.Connect(tbServerDomain.Text, port);
                    if (tcp.Connected)
                    {
                        CommonReference.ShowInformation(this, "Connection is successfully established with remote host.");

                    }
                    else
                    {
                        CommonReference.ShowWarning(this, "Connection attempt failed.");
                    }
                }
                catch (Exception)
                {

                    CommonReference.ShowWarning(this, "Connection attempt failed.");
                }
            }
        }

      
        #endregion

        #region Connection related


        void ClientCore_OnDisconnect()
        {
            CommonReference.ShowWarning(this, "Server connection lost.\nYou will be redirected to the login page.");
            StaticReference.RecreateClient();
            RestoreLoginUI();
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                StaticReference.Client.EndConnect(ar);
                StaticReference.ClientCore = new ClientCore(StaticReference.Client);
                StaticReference.ClientCore.OnDataReceived += ClientCore_OnDataReceived;
                StaticReference.ClientCore.OnDisconnect += ClientCore_OnDisconnect;
                DoHandshake();

            }
            catch (Exception)
            {
                CommonReference.ShowError(this,
                 "Connection attempt failed.\nEither server is down or not available right now.");
                StaticReference.RecreateClient();

                Invoke(new Action(() => RestoreLoginUI()));
            }
        }

        private void ConnectRemoteHost(ServerEntry se)
        {
            if (string.IsNullOrEmpty(tbManagerID.Text) || string.IsNullOrEmpty(tbPassword.Text))
            {

                CommonReference.ShowWarning(this, "Either ID or password field is empty.");
                Invoke(new Action(() => RestoreLoginUI()));
                return;
            }
            StaticReference.LoginCredentials = new Credentials { ManagerID = tbManagerID.Text, Password = CommonReference.HashPasswordString(tbPassword.Text)};
            Dns.BeginGetHostEntry(se.Host, ResolveCallbacks, se);
        }

        private void ResolveCallbacks(IAsyncResult ar)
        {

            var se = ar.AsyncState as ServerEntry;
            Debug.Assert(se != null, "se != null");

            try
            {
                var ihe = Dns.EndGetHostEntry(ar);
                if (ihe.AddressList.Length == 0)
                    throw new Exception();
                StaticReference.Client.BeginConnect(ihe.AddressList, se.Port, ConnectCallback, se);
            }
            catch (Exception)
            {

                StaticReference.Client.BeginConnect(se.Host, se.Port, ConnectCallback, se);
            }

        }
        #endregion

        #region Functionality


        private void ClientCore_OnDataReceived(Packet receivedPacket)
        {
            switch (receivedPacket.Opcode)
            {
                case (byte)SMIOpcodes.SMI_UNKNOWN:
                    Trace.WriteLine("ClientCore_OnDataReceived() - Previously sent request was unknown.");
                    CommonReference.ShowWarning(this,
                        "Unknown request acknowledgement : Previously sent request\nwas not recognized by the server.");
                    StaticReference.RecreateClient();
                    RestoreLoginUI();
                    break;
                case (byte)SMIOpcodes.KAI_OPCODE_AUTH:
                    HandleAuthPacket(receivedPacket);
                    break;
     
            }
        }

        private void HandleAuthPacket(Packet receivedPacket)
        {
            switch((KAIAuthSubopcodes)receivedPacket.Read<byte>())
            {
                case KAIAuthSubopcodes.KAI_AUTH_HANDSHAKE:
                    HandleHandshake(receivedPacket);
                    break;
                case KAIAuthSubopcodes.KAI_AUTH_LOGIN:
                    HandleLogin(receivedPacket);
                    break;
                default:
                    Trace.TraceWarning("ClientCore::HandleAuthPacket() - Unknown auth opcode.");
                    break;
            }
        }

        private void DoHandshake()
        {

            ppLogin.Description = @"Sending handshake..";
            ppLogin.Caption = @"Handshaking with server..";
            StaticReference.ClientCore.SendHandshake(StaticReference.HandshakeKey, StaticReference.HandshakeKey.Length);

        }

        private void DoLogin()
        {

            ppLogin.Caption = @"Logging in..";
            ppLogin.Description = @"Sending credentials..";
            StaticReference.ClientCore.SendLogin(StaticReference.LoginCredentials);
        }

        private void HandleHandshake(Packet pkt)
        {

            Trace.WriteLine("HandleHandshake() - Handshake response received.");
            switch (pkt.SubOpcode)
            {
                case (byte)KAIHandshakeSubopcodes.KAI_HANDSHAKE_SUCCESS:
                    DoLogin();
                    break;
                case (byte)KAIHandshakeSubopcodes.KAI_HANDSHAKE_FAILURE:
                    CommonReference.ShowWarning(this,
                        "Handshake failure.\nReason : Handshake key mismatch.");
                    StaticReference.RecreateClient();
                    Invoke(new Action(() => RestoreLoginUI()));
                    break;
                case (byte)KAIHandshakeSubopcodes.KAI_HANDSHAKE_ALREADY:
                    CommonReference.ShowWarning(this, "You are already handshaked.");
                    Invoke(new Action(() => RestoreLoginUI()));
                    break;

            }
        }
        private void HandleLogin(Packet pkt)
        {

            Trace.WriteLine("HandleLogin() - Login response received.");
            switch (pkt.Read<byte>())
            {
                case (byte)KAILoginSubopcodes.KAI_LOGIN_HANDSHAKE_FAILURE:
                    CommonReference.ShowWarning(this, "Login attempt failed, because of improper handshake sequence.\nPlease contact with administration if the problem persists.");
                    StaticReference.RecreateClient();
                    Invoke(new Action(() => RestoreLoginUI()));
                    break;
                case (byte)KAILoginSubopcodes.KAI_LOGIN_INSUFFICIENT_PRIVILEGE:
                    CommonReference.ShowWarning(this,
                        "Your privilege is not enough to use this program.\nPlease contact with administration for further information.");
                    StaticReference.RecreateClient();
                    Invoke(new Action(() => RestoreLoginUI()));
                    break;
                case (byte)KAILoginSubopcodes.KAI_LOGIN_INVALID_CREDENTIALS:
                    CommonReference.ShowWarning(this,
                        "Credentials error : Either ID or password is invalid.\nPlease correct it and try again.");
                    StaticReference.RecreateClient();
                    Invoke(new Action(() => RestoreLoginUI()));
                    break;
                case (byte)KAILoginSubopcodes.KAI_LOGIN_UNAUTHORIZED_IP:
                    CommonReference.ShowWarning(this,
                        "Authorization error : Your Internet Protocol(IP) is not authorized to use this program.\nPlease contact with administration for further information.");
                    StaticReference.RecreateClient();
                    Invoke(new Action(() => RestoreLoginUI()));
                    break;
                case (byte)KAILoginSubopcodes.KAI_LOGIN_ALREADY:
                    CommonReference.ShowWarning(this,
                      "Account is already in use.\nIf you think your account is compromised, please contact with administration IMMEDIATELY.");
                    StaticReference.RecreateClient();
                    Invoke(new Action(() => RestoreLoginUI()));
                    break;
                case (byte)KAILoginSubopcodes.KAI_LOGIN_SUCCESS:
                    Invoke(new Action(() => {
                        ppLogin.Caption = @"Initializing interface";
                        ppLogin.Description = @"Reading data tables..";
                    }));
                   

                    Trace.TraceWarning("LoadTables");

                    if (!CommonReference.InitializeDataTableSet())
                    {
                        StaticReference.RecreateClient();
                        Invoke(new Action(() => RestoreLoginUI()));
                        return;
                    }

                  
                    Invoke(new Action(() => {
                        ppLogin.Description = @"Reading IP country block database..";
                    }));
                    if (!IPCountryCodeHelper.LoadCSVFile("./ip.db"))
                    {
                        Trace.TraceWarning("IP country database file (ip.db) is not found. Program will not be able to query IP locations.");
                    }
                    Invoke(new Action(() => {
                        ppLogin.Description = @"Loading country flag images..";
                    }));
                    

                    StaticReference.LoadCountryFlags();
                    Invoke(new Action(() => {
                        ppLogin.Description = @"Getting ready..";
                    }));

                   
              
                    StaticReference.ClientCore.OnDataReceived -= ClientCore_OnDataReceived;
                    StaticReference.ClientCore.OnDisconnect -= ClientCore_OnDisconnect;
                    new Thread (() => Application.Run(new mainFrm(this) { }))  { IsBackground = false, Name = "Main Window Thread", ApartmentState = ApartmentState.STA }.Start();

                    break;
            }

        }
        #endregion

        private void chbRemember_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void frmLogin_Shown(object sender, EventArgs e)
        {
            this.BringToFront();
            this.Activate();
        }
    }


}
