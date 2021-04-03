using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;

namespace KAI.Interface
{
    partial class frmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem4 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem3 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipSeparatorItem toolTipSeparatorItem2 = new DevExpress.Utils.ToolTipSeparatorItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem5 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.ToolTipSeparatorItem toolTipSeparatorItem1 = new DevExpress.Utils.ToolTipSeparatorItem();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem3 = new DevExpress.Utils.ToolTipTitleItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.dbbConnect = new DevExpress.XtraEditors.DropDownButton();
            this.serversMenu = new DevExpress.XtraBars.PopupMenu();
            this.mainBarManager = new DevExpress.XtraBars.BarManager();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem3 = new DevExpress.XtraBars.BarStaticItem();
            this.tbManagerID = new DevExpress.XtraEditors.TextEdit();
            this.tbPassword = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.chbRemember = new DevExpress.XtraEditors.CheckButton();
            this.gcCredentials = new DevExpress.XtraEditors.GroupControl();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.gcServers = new DevExpress.XtraEditors.GroupControl();
            this.gcRemove = new DevExpress.XtraEditors.GroupControl();
            this.btnRemoveSelectedServers = new DevExpress.XtraEditors.SimpleButton();
            this.clbServersToRemove = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.cbExtendServers = new DevExpress.XtraEditors.CheckButton();
            this.gcNewServer = new DevExpress.XtraEditors.GroupControl();
            this.btnTestConnection = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddServer = new DevExpress.XtraEditors.SimpleButton();
            this.tbServerPort = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.tbServerDomain = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.tbServerName = new DevExpress.XtraEditors.TextEdit();
            this.gcConnecting = new DevExpress.XtraEditors.GroupControl();
            this.lblServerName = new System.Windows.Forms.Label();
            this.ppLogin = new DevExpress.XtraWaitForm.ProgressPanel();
            ((System.ComponentModel.ISupportInitialize)(this.serversMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainBarManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbManagerID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcCredentials)).BeginInit();
            this.gcCredentials.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcServers)).BeginInit();
            this.gcServers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcRemove)).BeginInit();
            this.gcRemove.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clbServersToRemove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcNewServer)).BeginInit();
            this.gcNewServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbServerPort.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbServerDomain.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbServerName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcConnecting)).BeginInit();
            this.gcConnecting.SuspendLayout();
            this.SuspendLayout();
            // 
            // dbbConnect
            // 
            this.dbbConnect.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dbbConnect.DropDownArrowStyle = DevExpress.XtraEditors.DropDownArrowStyle.SplitButton;
            this.dbbConnect.DropDownControl = this.serversMenu;
            this.dbbConnect.ImageOptions.Image = global::KAI.Properties.Resources._1457302718_Lightning;
            this.dbbConnect.Location = new System.Drawing.Point(0, 227);
            this.dbbConnect.Name = "dbbConnect";
            this.dbbConnect.Size = new System.Drawing.Size(786, 34);
            toolTipTitleItem4.Text = "Connect button";
            toolTipItem3.LeftIndent = 6;
            toolTipItem3.Text = "Connects to the selected server with given\r\ncredentials.";
            toolTipTitleItem5.LeftIndent = 6;
            toolTipTitleItem5.Text = "(requires valid internet connection)\r\n(requires valid credentials)";
            superToolTip3.Items.Add(toolTipTitleItem4);
            superToolTip3.Items.Add(toolTipItem3);
            superToolTip3.Items.Add(toolTipSeparatorItem2);
            superToolTip3.Items.Add(toolTipTitleItem5);
            this.dbbConnect.SuperTip = superToolTip3;
            this.dbbConnect.TabIndex = 1;
            this.dbbConnect.Text = "Connect";
            this.dbbConnect.Click += new System.EventHandler(this.dbbConnect_Click);
            // 
            // serversMenu
            // 
            this.serversMenu.Manager = this.mainBarManager;
            this.serversMenu.Name = "serversMenu";
            // 
            // mainBarManager
            // 
            this.mainBarManager.DockControls.Add(this.barDockControlTop);
            this.mainBarManager.DockControls.Add(this.barDockControlBottom);
            this.mainBarManager.DockControls.Add(this.barDockControlLeft);
            this.mainBarManager.DockControls.Add(this.barDockControlRight);
            this.mainBarManager.Form = this;
            this.mainBarManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barHeaderItem1,
            this.barStaticItem1,
            this.barStaticItem2,
            this.barStaticItem3});
            this.mainBarManager.MaxItemId = 4;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.mainBarManager;
            this.barDockControlTop.Size = new System.Drawing.Size(786, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 261);
            this.barDockControlBottom.Manager = this.mainBarManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(786, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.mainBarManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 261);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(786, 0);
            this.barDockControlRight.Manager = this.mainBarManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 261);
            // 
            // barHeaderItem1
            // 
            this.barHeaderItem1.Caption = "Servers";
            this.barHeaderItem1.Id = 0;
            this.barHeaderItem1.Name = "barHeaderItem1";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "Server1";
            this.barStaticItem1.Id = 1;
            this.barStaticItem1.Name = "barStaticItem1";
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Caption = "Server 2";
            this.barStaticItem2.Id = 2;
            this.barStaticItem2.Name = "barStaticItem2";
            // 
            // barStaticItem3
            // 
            this.barStaticItem3.Caption = "Server 3";
            this.barStaticItem3.Id = 3;
            this.barStaticItem3.Name = "barStaticItem3";
            // 
            // tbManagerID
            // 
            this.tbManagerID.Location = new System.Drawing.Point(15, 112);
            this.tbManagerID.Name = "tbManagerID";
            this.tbManagerID.Size = new System.Drawing.Size(214, 22);
            toolTipTitleItem1.Text = "Manager ID";
            toolTipItem1.LeftIndent = 6;
            toolTipItem1.Text = "Your unique identifier which assigned by administration.";
            superToolTip1.Items.Add(toolTipTitleItem1);
            superToolTip1.Items.Add(toolTipItem1);
            this.tbManagerID.SuperTip = superToolTip1;
            this.tbManagerID.TabIndex = 2;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(15, 162);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Properties.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(214, 22);
            toolTipTitleItem2.Text = "Password";
            toolTipItem2.LeftIndent = 6;
            toolTipItem2.Text = "Your pre-determined password to login.";
            toolTipTitleItem3.LeftIndent = 6;
            toolTipTitleItem3.Text = "(note : if you enter your password wrong three\r\ntimes in a row, you will not be a" +
    "ble to login for\r\nthirty minutes.)";
            superToolTip2.Items.Add(toolTipTitleItem2);
            superToolTip2.Items.Add(toolTipItem2);
            superToolTip2.Items.Add(toolTipSeparatorItem1);
            superToolTip2.Items.Add(toolTipTitleItem3);
            this.tbPassword.SuperTip = superToolTip2;
            this.tbPassword.TabIndex = 3;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(15, 93);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(66, 16);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "Manager ID";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(15, 140);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(55, 16);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "Password";
            // 
            // chbRemember
            // 
            this.chbRemember.Appearance.Font = new System.Drawing.Font("Tahoma", 6.8F);
            this.chbRemember.Appearance.Options.UseFont = true;
            this.chbRemember.ImageOptions.Image = global::KAI.Properties.Resources._16x16_lightbulb;
            this.chbRemember.Location = new System.Drawing.Point(15, 194);
            this.chbRemember.Name = "chbRemember";
            this.chbRemember.Size = new System.Drawing.Size(214, 23);
            this.chbRemember.TabIndex = 11;
            this.chbRemember.Text = "Remember next time?";
            this.chbRemember.CheckedChanged += new System.EventHandler(this.chbRemember_CheckedChanged);
            // 
            // gcCredentials
            // 
            this.gcCredentials.Controls.Add(this.pictureBox1);
            this.gcCredentials.Controls.Add(this.chbRemember);
            this.gcCredentials.Controls.Add(this.tbManagerID);
            this.gcCredentials.Controls.Add(this.tbPassword);
            this.gcCredentials.Controls.Add(this.labelControl2);
            this.gcCredentials.Controls.Add(this.labelControl1);
            this.gcCredentials.Dock = System.Windows.Forms.DockStyle.Left;
            this.gcCredentials.Location = new System.Drawing.Point(0, 0);
            this.gcCredentials.Name = "gcCredentials";
            this.gcCredentials.Size = new System.Drawing.Size(248, 227);
            this.gcCredentials.TabIndex = 18;
            this.gcCredentials.Text = "Credentials";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::KAI.Properties.Resources._1457570863_Locked;
            this.pictureBox1.Location = new System.Drawing.Point(15, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(214, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // gcServers
            // 
            this.gcServers.AutoSize = true;
            this.gcServers.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gcServers.Controls.Add(this.gcRemove);
            this.gcServers.Controls.Add(this.cbExtendServers);
            this.gcServers.Controls.Add(this.gcNewServer);
            this.gcServers.Dock = System.Windows.Forms.DockStyle.Right;
            this.gcServers.Location = new System.Drawing.Point(249, 0);
            this.gcServers.Name = "gcServers";
            this.gcServers.Size = new System.Drawing.Size(537, 227);
            this.gcServers.TabIndex = 19;
            this.gcServers.Text = "Servers";
            // 
            // gcRemove
            // 
            this.gcRemove.Controls.Add(this.btnRemoveSelectedServers);
            this.gcRemove.Controls.Add(this.clbServersToRemove);
            this.gcRemove.Location = new System.Drawing.Point(330, 29);
            this.gcRemove.Name = "gcRemove";
            this.gcRemove.Size = new System.Drawing.Size(200, 166);
            this.gcRemove.TabIndex = 3;
            this.gcRemove.Text = "Remove";
            this.gcRemove.Visible = false;
            // 
            // btnRemoveSelectedServers
            // 
            this.btnRemoveSelectedServers.ImageOptions.Image = global::KAI.Properties.Resources._16x16_cancel;
            this.btnRemoveSelectedServers.Location = new System.Drawing.Point(5, 139);
            this.btnRemoveSelectedServers.Name = "btnRemoveSelectedServers";
            this.btnRemoveSelectedServers.Size = new System.Drawing.Size(190, 25);
            this.btnRemoveSelectedServers.TabIndex = 1;
            this.btnRemoveSelectedServers.Text = "Remove Checked";
            this.btnRemoveSelectedServers.Click += new System.EventHandler(this.btnRemoveSelectedServers_Click);
            // 
            // clbServersToRemove
            // 
            this.clbServersToRemove.Location = new System.Drawing.Point(5, 35);
            this.clbServersToRemove.Name = "clbServersToRemove";
            this.clbServersToRemove.Size = new System.Drawing.Size(190, 95);
            this.clbServersToRemove.TabIndex = 0;
            // 
            // cbExtendServers
            // 
            this.cbExtendServers.ImageOptions.Image = global::KAI.Properties.Resources._1457570770_EditDocument;
            this.cbExtendServers.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.cbExtendServers.Location = new System.Drawing.Point(5, 26);
            this.cbExtendServers.Name = "cbExtendServers";
            this.cbExtendServers.Size = new System.Drawing.Size(68, 191);
            this.cbExtendServers.TabIndex = 2;
            this.cbExtendServers.CheckedChanged += new System.EventHandler(this.cbExtendServers_CheckedChanged);
            // 
            // gcNewServer
            // 
            this.gcNewServer.Controls.Add(this.btnTestConnection);
            this.gcNewServer.Controls.Add(this.btnAddServer);
            this.gcNewServer.Controls.Add(this.tbServerPort);
            this.gcNewServer.Controls.Add(this.labelControl5);
            this.gcNewServer.Controls.Add(this.tbServerDomain);
            this.gcNewServer.Controls.Add(this.labelControl4);
            this.gcNewServer.Controls.Add(this.labelControl3);
            this.gcNewServer.Controls.Add(this.tbServerName);
            this.gcNewServer.Location = new System.Drawing.Point(79, 29);
            this.gcNewServer.Name = "gcNewServer";
            this.gcNewServer.Size = new System.Drawing.Size(245, 164);
            this.gcNewServer.TabIndex = 1;
            this.gcNewServer.Text = "New";
            this.gcNewServer.Visible = false;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Location = new System.Drawing.Point(10, 136);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(122, 23);
            this.btnTestConnection.TabIndex = 7;
            this.btnTestConnection.Text = "Test Connection";
            this.btnTestConnection.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // btnAddServer
            // 
            this.btnAddServer.Location = new System.Drawing.Point(138, 136);
            this.btnAddServer.Name = "btnAddServer";
            this.btnAddServer.Size = new System.Drawing.Size(91, 23);
            this.btnAddServer.TabIndex = 6;
            this.btnAddServer.Text = "Insert to list";
            this.btnAddServer.Click += new System.EventHandler(this.btnAddServer_Click);
            // 
            // tbServerPort
            // 
            this.tbServerPort.Location = new System.Drawing.Point(81, 105);
            this.tbServerPort.MenuManager = this.mainBarManager;
            this.tbServerPort.Name = "tbServerPort";
            this.tbServerPort.Size = new System.Drawing.Size(148, 22);
            this.tbServerPort.TabIndex = 5;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(10, 108);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(23, 16);
            this.labelControl5.TabIndex = 4;
            this.labelControl5.Text = "Port";
            // 
            // tbServerDomain
            // 
            this.tbServerDomain.Location = new System.Drawing.Point(81, 70);
            this.tbServerDomain.MenuManager = this.mainBarManager;
            this.tbServerDomain.Name = "tbServerDomain";
            this.tbServerDomain.Size = new System.Drawing.Size(148, 22);
            this.tbServerDomain.TabIndex = 3;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(10, 73);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(43, 16);
            this.labelControl4.TabIndex = 2;
            this.labelControl4.Text = "Domain";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(10, 37);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(33, 16);
            this.labelControl3.TabIndex = 1;
            this.labelControl3.Text = "Name";
            // 
            // tbServerName
            // 
            this.tbServerName.Location = new System.Drawing.Point(81, 34);
            this.tbServerName.MenuManager = this.mainBarManager;
            this.tbServerName.Name = "tbServerName";
            this.tbServerName.Size = new System.Drawing.Size(148, 22);
            this.tbServerName.TabIndex = 0;
            // 
            // gcConnecting
            // 
            this.gcConnecting.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gcConnecting.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gcConnecting.Controls.Add(this.lblServerName);
            this.gcConnecting.Controls.Add(this.ppLogin);
            this.gcConnecting.Location = new System.Drawing.Point(793, 3);
            this.gcConnecting.Name = "gcConnecting";
            this.gcConnecting.Size = new System.Drawing.Size(276, 237);
            this.gcConnecting.TabIndex = 24;
            this.gcConnecting.Visible = false;
            // 
            // lblServerName
            // 
            this.lblServerName.Font = new System.Drawing.Font("Tahoma", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblServerName.ForeColor = System.Drawing.Color.Gray;
            this.lblServerName.Location = new System.Drawing.Point(5, 218);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(266, 17);
            this.lblServerName.TabIndex = 1;
            this.lblServerName.Text = "label1";
            this.lblServerName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ppLogin
            // 
            this.ppLogin.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.ppLogin.Appearance.Font = new System.Drawing.Font("Tahoma", 6.8F);
            this.ppLogin.Appearance.Options.UseBackColor = true;
            this.ppLogin.Appearance.Options.UseFont = true;
            this.ppLogin.Appearance.Options.UseTextOptions = true;
            this.ppLogin.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ppLogin.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ppLogin.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ppLogin.AppearanceCaption.Options.UseFont = true;
            this.ppLogin.AppearanceCaption.Options.UseTextOptions = true;
            this.ppLogin.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ppLogin.AppearanceCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ppLogin.AppearanceDescription.Font = new System.Drawing.Font("Tahoma", 7.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ppLogin.AppearanceDescription.ForeColor = System.Drawing.Color.Silver;
            this.ppLogin.AppearanceDescription.Options.UseFont = true;
            this.ppLogin.AppearanceDescription.Options.UseForeColor = true;
            this.ppLogin.AppearanceDescription.Options.UseTextOptions = true;
            this.ppLogin.AppearanceDescription.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.ppLogin.AppearanceDescription.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.ppLogin.BarAnimationElementThickness = 2;
            this.ppLogin.Caption = "Connecting";
            this.ppLogin.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.ppLogin.Description = "Please wait ...";
            this.ppLogin.Location = new System.Drawing.Point(19, 90);
            this.ppLogin.LookAndFeel.SkinName = "The Asphalt World";
            this.ppLogin.LookAndFeel.UseDefaultLookAndFeel = false;
            this.ppLogin.Name = "ppLogin";
            this.ppLogin.Size = new System.Drawing.Size(246, 66);
            this.ppLogin.TabIndex = 29;
            this.ppLogin.Text = "asd";
            this.ppLogin.WaitAnimationType = DevExpress.Utils.Animation.WaitingAnimatorType.Line;
            // 
            // frmLogin
            // 
            this.Appearance.Options.UseFont = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(786, 261);
            this.Controls.Add(this.gcConnecting);
            this.Controls.Add(this.gcServers);
            this.Controls.Add(this.gcCredentials);
            this.Controls.Add(this.dbbConnect);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Font = new System.Drawing.Font("Tahoma", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "[KAI::Login]";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLogin_FormClosing);
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.Shown += new System.EventHandler(this.frmLogin_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.serversMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainBarManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbManagerID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcCredentials)).EndInit();
            this.gcCredentials.ResumeLayout(false);
            this.gcCredentials.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcServers)).EndInit();
            this.gcServers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcRemove)).EndInit();
            this.gcRemove.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.clbServersToRemove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcNewServer)).EndInit();
            this.gcNewServer.ResumeLayout(false);
            this.gcNewServer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbServerPort.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbServerDomain.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbServerName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcConnecting)).EndInit();
            this.gcConnecting.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DropDownButton dbbConnect;
        private TextEdit tbManagerID;
        private TextEdit tbPassword;
        private LabelControl labelControl1;
        private LabelControl labelControl2;
        private PopupMenu serversMenu;
        private BarHeaderItem barHeaderItem1;
        private BarStaticItem barStaticItem1;
        private BarStaticItem barStaticItem2;
        private BarStaticItem barStaticItem3;
        private BarManager mainBarManager;
        private BarDockControl barDockControlTop;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        private CheckButton chbRemember;
        private GroupControl gcCredentials;
        private GroupControl gcServers;
        private CheckButton cbExtendServers;
        private GroupControl gcNewServer;
        private LabelControl labelControl3;
        private TextEdit tbServerName;
        private SimpleButton btnAddServer;
        private TextEdit tbServerPort;
        private LabelControl labelControl5;
        private TextEdit tbServerDomain;
        private LabelControl labelControl4;
        private GroupControl gcRemove;
        private SimpleButton btnRemoveSelectedServers;
        private CheckedListBoxControl clbServersToRemove;
        private SimpleButton btnTestConnection;
        private GroupControl gcConnecting;
        private Label lblServerName;
        private DevExpress.XtraWaitForm.ProgressPanel ppLogin;
        private PictureBox pictureBox1;
    }
}

