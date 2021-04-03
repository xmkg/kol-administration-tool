namespace KAI.Interface
{
    partial class frmPunishPlayer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPunishPlayer));
            this.tcMain = new DevExpress.XtraTab.XtraTabControl();
            this.tpAccount = new DevExpress.XtraTab.XtraTabPage();
            this.pbWaiting = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.chChangeAccountName = new DevExpress.XtraEditors.CheckButton();
            this.chChangePlayerName = new DevExpress.XtraEditors.CheckButton();
            this.lblExpireDate = new DevExpress.XtraEditors.LabelControl();
            this.lblCharactersRemaining = new DevExpress.XtraEditors.LabelControl();
            this.btnCancelPenalty = new DevExpress.XtraEditors.SimpleButton();
            this.btnSubmitPenalty = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.meReason = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.ceDurationType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.seTime = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.cbPenaltyType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.tbPlayerID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.tbAccountID = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.tpBanHistory = new DevExpress.XtraTab.XtraTabPage();
            this.accordionControl1 = new DevExpress.XtraBars.Navigation.AccordionControl();
            this.accordionContentContainer1 = new DevExpress.XtraBars.Navigation.AccordionContentContainer();
            this.accordionControlElement1 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement8 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement9 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement10 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement11 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement2 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement3 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement4 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement5 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement6 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            this.accordionControlElement7 = new DevExpress.XtraBars.Navigation.AccordionControlElement();
            ((System.ComponentModel.ISupportInitialize)(this.tcMain)).BeginInit();
            this.tcMain.SuspendLayout();
            this.tpAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWaiting.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.meReason.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceDurationType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbPenaltyType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPlayerID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbAccountID.Properties)).BeginInit();
            this.tpBanHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).BeginInit();
            this.accordionControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcMain
            // 
            this.tcMain.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.tcMain.Appearance.Options.UseBackColor = true;
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedTabPage = this.tpAccount;
            this.tcMain.Size = new System.Drawing.Size(622, 442);
            this.tcMain.TabIndex = 9;
            this.tcMain.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tpAccount,
            this.tpBanHistory});
            // 
            // tpAccount
            // 
            this.tpAccount.Controls.Add(this.pbWaiting);
            this.tpAccount.Controls.Add(this.labelControl8);
            this.tpAccount.Controls.Add(this.chChangeAccountName);
            this.tpAccount.Controls.Add(this.chChangePlayerName);
            this.tpAccount.Controls.Add(this.lblExpireDate);
            this.tpAccount.Controls.Add(this.lblCharactersRemaining);
            this.tpAccount.Controls.Add(this.btnCancelPenalty);
            this.tpAccount.Controls.Add(this.btnSubmitPenalty);
            this.tpAccount.Controls.Add(this.labelControl7);
            this.tpAccount.Controls.Add(this.labelControl6);
            this.tpAccount.Controls.Add(this.meReason);
            this.tpAccount.Controls.Add(this.labelControl5);
            this.tpAccount.Controls.Add(this.ceDurationType);
            this.tpAccount.Controls.Add(this.seTime);
            this.tpAccount.Controls.Add(this.labelControl4);
            this.tpAccount.Controls.Add(this.cbPenaltyType);
            this.tpAccount.Controls.Add(this.labelControl3);
            this.tpAccount.Controls.Add(this.tbPlayerID);
            this.tpAccount.Controls.Add(this.labelControl2);
            this.tpAccount.Controls.Add(this.tbAccountID);
            this.tpAccount.Controls.Add(this.labelControl1);
            this.tpAccount.Image = global::KAI.Properties.Resources._16x16_application;
            this.tpAccount.Name = "tpAccount";
            this.tpAccount.Size = new System.Drawing.Size(615, 408);
            this.tpAccount.Text = "Account Details";
            this.tpAccount.Paint += new System.Windows.Forms.PaintEventHandler(this.tpAccount_Paint);
            // 
            // pbWaiting
            // 
            this.pbWaiting.EditValue = "Waiting response from server..";
            this.pbWaiting.Location = new System.Drawing.Point(26, 336);
            this.pbWaiting.Name = "pbWaiting";
            this.pbWaiting.Properties.DisplayFormat.FormatString = "asd";
            this.pbWaiting.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.pbWaiting.Properties.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.pbWaiting.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pbWaiting.Properties.MarqueeAnimationSpeed = 20;
            this.pbWaiting.Properties.Paused = true;
            this.pbWaiting.Properties.ProgressAnimationMode = DevExpress.Utils.Drawing.ProgressAnimationMode.PingPong;
            this.pbWaiting.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid;
            this.pbWaiting.Properties.ShowTitle = true;
            this.pbWaiting.Size = new System.Drawing.Size(562, 18);
            this.pbWaiting.TabIndex = 20;
            this.pbWaiting.UseWaitCursor = true;
            this.pbWaiting.Visible = false;
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 5.8F);
            this.labelControl8.Appearance.ForeColor = System.Drawing.Color.Gray;
            this.labelControl8.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl8.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl8.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl8.Location = new System.Drawing.Point(0, 391);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(612, 16);
            this.labelControl8.TabIndex = 19;
            this.labelControl8.Text = "Account ID is not required when using Character ID, and vice versa.";
            // 
            // chChangeAccountName
            // 
            this.chChangeAccountName.Location = new System.Drawing.Point(282, 13);
            this.chChangeAccountName.Name = "chChangeAccountName";
            this.chChangeAccountName.Size = new System.Drawing.Size(38, 23);
            this.chChangeAccountName.TabIndex = 18;
            this.chChangeAccountName.Text = "D";
            this.chChangeAccountName.CheckedChanged += new System.EventHandler(this.chChangeAccountName_CheckedChanged);
            // 
            // chChangePlayerName
            // 
            this.chChangePlayerName.Location = new System.Drawing.Point(282, 41);
            this.chChangePlayerName.Name = "chChangePlayerName";
            this.chChangePlayerName.Size = new System.Drawing.Size(38, 23);
            this.chChangePlayerName.TabIndex = 17;
            this.chChangePlayerName.Text = "D";
            this.chChangePlayerName.CheckedChanged += new System.EventHandler(this.chChangePlayerName_CheckedChanged);
            // 
            // lblExpireDate
            // 
            this.lblExpireDate.Appearance.Font = new System.Drawing.Font("Tahoma", 5.8F);
            this.lblExpireDate.Appearance.ForeColor = System.Drawing.Color.OliveDrab;
            this.lblExpireDate.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblExpireDate.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblExpireDate.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblExpireDate.Location = new System.Drawing.Point(27, 123);
            this.lblExpireDate.Name = "lblExpireDate";
            this.lblExpireDate.Size = new System.Drawing.Size(562, 16);
            this.lblExpireDate.TabIndex = 16;
            this.lblExpireDate.Text = "the penalty will expire on 19.07.2016, Friday 18:30";
            // 
            // lblCharactersRemaining
            // 
            this.lblCharactersRemaining.Appearance.Font = new System.Drawing.Font("Tahoma", 5.8F);
            this.lblCharactersRemaining.Appearance.ForeColor = System.Drawing.Color.Chartreuse;
            this.lblCharactersRemaining.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lblCharactersRemaining.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.lblCharactersRemaining.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lblCharactersRemaining.Location = new System.Drawing.Point(26, 223);
            this.lblCharactersRemaining.Name = "lblCharactersRemaining";
            this.lblCharactersRemaining.Size = new System.Drawing.Size(124, 16);
            this.lblCharactersRemaining.TabIndex = 15;
            this.lblCharactersRemaining.Text = "150 character(s) remaining";
            // 
            // btnCancelPenalty
            // 
            this.btnCancelPenalty.Image = global::KAI.Properties.Resources._16x16_cancel;
            this.btnCancelPenalty.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnCancelPenalty.Location = new System.Drawing.Point(353, 362);
            this.btnCancelPenalty.Name = "btnCancelPenalty";
            this.btnCancelPenalty.Size = new System.Drawing.Size(132, 23);
            this.btnCancelPenalty.TabIndex = 14;
            this.btnCancelPenalty.Text = "CANCEL";
            // 
            // btnSubmitPenalty
            // 
            this.btnSubmitPenalty.Image = global::KAI.Properties.Resources._16x16_auction;
            this.btnSubmitPenalty.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.btnSubmitPenalty.Location = new System.Drawing.Point(118, 362);
            this.btnSubmitPenalty.Name = "btnSubmitPenalty";
            this.btnSubmitPenalty.Size = new System.Drawing.Size(132, 23);
            this.btnSubmitPenalty.TabIndex = 13;
            this.btnSubmitPenalty.Text = "SUBMIT";
            this.btnSubmitPenalty.Click += new System.EventHandler(this.btnSubmitPenalty_Click);
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 5.8F);
            this.labelControl7.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl7.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl7.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl7.Location = new System.Drawing.Point(27, 198);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(562, 16);
            this.labelControl7.TabIndex = 12;
            this.labelControl7.Text = "(if you punish anybody without a valid reason, you will lose your authority perma" +
    "nently)";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 5.8F);
            this.labelControl6.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl6.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl6.Location = new System.Drawing.Point(27, 184);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(562, 16);
            this.labelControl6.TabIndex = 11;
            this.labelControl6.Text = "(minimum 50, maximum 150 characters. DO NOT PUNISH ANYBODY WITHOUT A VALID REASON" +
    "!!)";
            // 
            // meReason
            // 
            this.meReason.Location = new System.Drawing.Point(27, 242);
            this.meReason.Name = "meReason";
            this.meReason.Properties.MaxLength = 150;
            this.meReason.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.meReason.Size = new System.Drawing.Size(562, 88);
            this.meReason.TabIndex = 10;
            this.meReason.EditValueChanged += new System.EventHandler(this.meReason_EditValueChanged);
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl5.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl5.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl5.Location = new System.Drawing.Point(27, 162);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(562, 16);
            this.labelControl5.TabIndex = 9;
            this.labelControl5.Text = "Reason for penalty (IMPORTANT)";
            // 
            // ceDurationType
            // 
            this.ceDurationType.EditValue = "day(s)";
            this.ceDurationType.Location = new System.Drawing.Point(432, 98);
            this.ceDurationType.Name = "ceDurationType";
            this.ceDurationType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ceDurationType.Properties.Items.AddRange(new object[] {
            "second(s)",
            "minute(s)",
            "hour(s)",
            "day(s)",
            "week(s)",
            "month(s)",
            "year(s)",
            "PERMANENT"});
            this.ceDurationType.Size = new System.Drawing.Size(157, 22);
            this.ceDurationType.TabIndex = 8;
            this.ceDurationType.SelectedIndexChanged += new System.EventHandler(this.ceDurationType_SelectedIndexChanged);
            // 
            // seTime
            // 
            this.seTime.EditValue = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.seTime.Location = new System.Drawing.Point(326, 94);
            this.seTime.Name = "seTime";
            this.seTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seTime.Properties.MaxValue = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.seTime.Size = new System.Drawing.Size(100, 30);
            this.seTime.TabIndex = 7;
            this.seTime.EditValueChanged += new System.EventHandler(this.seTime_EditValueChanged);
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.labelControl4.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl4.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl4.Location = new System.Drawing.Point(27, 101);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(263, 16);
            this.labelControl4.TabIndex = 6;
            this.labelControl4.Text = "Time Period";
            // 
            // cbPenaltyType
            // 
            this.cbPenaltyType.EditValue = "Send to prison";
            this.cbPenaltyType.Location = new System.Drawing.Point(326, 70);
            this.cbPenaltyType.Name = "cbPenaltyType";
            this.cbPenaltyType.Properties.Appearance.Options.UseTextOptions = true;
            this.cbPenaltyType.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.cbPenaltyType.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.cbPenaltyType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbPenaltyType.Properties.Items.AddRange(new object[] {
            "Send to prison",
            "Forbid from game (BAN)",
            "Prevent from using chat (MUTE)",
            "Prevent from trading items (TRADE BLOCK)",
            "Prevent from merchanting (MERCHANT BLOCK)",
            "Prevent from attacking/using skills (ATTACK BLOCK)",
            "Prevent from changing zone (TELEPORT BLOCK)",
            "Prevent from sending/receiving letter (LETTER BLOCK)"});
            this.cbPenaltyType.Size = new System.Drawing.Size(263, 22);
            this.cbPenaltyType.TabIndex = 5;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.labelControl3.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl3.Location = new System.Drawing.Point(27, 73);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(263, 16);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "Penalty Type";
            // 
            // tbPlayerID
            // 
            this.tbPlayerID.EditValue = "test account";
            this.tbPlayerID.Location = new System.Drawing.Point(326, 42);
            this.tbPlayerID.Name = "tbPlayerID";
            this.tbPlayerID.Properties.Appearance.Options.UseTextOptions = true;
            this.tbPlayerID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tbPlayerID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.tbPlayerID.Properties.ReadOnly = true;
            this.tbPlayerID.Size = new System.Drawing.Size(263, 22);
            this.tbPlayerID.TabIndex = 3;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.labelControl2.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(27, 45);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(263, 16);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Player ID";
            // 
            // tbAccountID
            // 
            this.tbAccountID.EditValue = "test account";
            this.tbAccountID.Location = new System.Drawing.Point(326, 14);
            this.tbAccountID.Name = "tbAccountID";
            this.tbAccountID.Properties.Appearance.Options.UseTextOptions = true;
            this.tbAccountID.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.tbAccountID.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.tbAccountID.Properties.ReadOnly = true;
            this.tbAccountID.Size = new System.Drawing.Size(263, 22);
            this.tbAccountID.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.labelControl1.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(27, 17);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(263, 16);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Account ID  ";
            // 
            // tpBanHistory
            // 
            this.tpBanHistory.Controls.Add(this.accordionControl1);
            this.tpBanHistory.Image = global::KAI.Properties.Resources._16x16_book;
            this.tpBanHistory.Name = "tpBanHistory";
            this.tpBanHistory.Size = new System.Drawing.Size(615, 408);
            this.tpBanHistory.Text = "History";
            // 
            // accordionControl1
            // 
            this.accordionControl1.Controls.Add(this.accordionContentContainer1);
            this.accordionControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.accordionControl1.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.accordionControlElement1,
            this.accordionControlElement2,
            this.accordionControlElement3,
            this.accordionControlElement4,
            this.accordionControlElement5,
            this.accordionControlElement6,
            this.accordionControlElement7});
            this.accordionControl1.Location = new System.Drawing.Point(0, 0);
            this.accordionControl1.Name = "accordionControl1";
            this.accordionControl1.Size = new System.Drawing.Size(615, 408);
            this.accordionControl1.TabIndex = 0;
            this.accordionControl1.Text = "accordionControl1";
            this.accordionControl1.Click += new System.EventHandler(this.accordionControl1_Click);
            // 
            // accordionContentContainer1
            // 
            this.accordionContentContainer1.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.accordionContentContainer1.Appearance.Options.UseBackColor = true;
            this.accordionContentContainer1.Name = "accordionContentContainer1";
            this.accordionContentContainer1.Size = new System.Drawing.Size(594, 76);
            this.accordionContentContainer1.TabIndex = 1;
            // 
            // accordionControlElement1
            // 
            this.accordionControlElement1.Elements.AddRange(new DevExpress.XtraBars.Navigation.AccordionControlElement[] {
            this.accordionControlElement8,
            this.accordionControlElement9,
            this.accordionControlElement10,
            this.accordionControlElement11});
            this.accordionControlElement1.Expanded = true;
            this.accordionControlElement1.Text = "Ban";
            // 
            // accordionControlElement8
            // 
            this.accordionControlElement8.ContentContainer = this.accordionContentContainer1;
            this.accordionControlElement8.Expanded = true;
            this.accordionControlElement8.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement8.Text = "[Expired] (19.07.2011 - 19.08.2011) Applied by : [DevilCraft]";
            // 
            // accordionControlElement9
            // 
            this.accordionControlElement9.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement9.Text = "Expired";
            // 
            // accordionControlElement10
            // 
            this.accordionControlElement10.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement10.Text = "Element10";
            // 
            // accordionControlElement11
            // 
            this.accordionControlElement11.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement11.Text = "Element11";
            // 
            // accordionControlElement2
            // 
            this.accordionControlElement2.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement2.Text = "Mute";
            // 
            // accordionControlElement3
            // 
            this.accordionControlElement3.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement3.Text = "Trade block";
            // 
            // accordionControlElement4
            // 
            this.accordionControlElement4.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement4.Text = "Merchant block";
            // 
            // accordionControlElement5
            // 
            this.accordionControlElement5.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement5.Text = "Attack block";
            // 
            // accordionControlElement6
            // 
            this.accordionControlElement6.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement6.Text = "Teleport Block";
            // 
            // accordionControlElement7
            // 
            this.accordionControlElement7.Style = DevExpress.XtraBars.Navigation.ElementStyle.Item;
            this.accordionControlElement7.Text = "Letter Block";
            // 
            // frmPunishPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 442);
            this.Controls.Add(this.tcMain);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "frmPunishPlayer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Player punishment";
            this.Load += new System.EventHandler(this.frmPunishPlayer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tcMain)).EndInit();
            this.tcMain.ResumeLayout(false);
            this.tpAccount.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbWaiting.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.meReason.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ceDurationType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbPenaltyType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbPlayerID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbAccountID.Properties)).EndInit();
            this.tpBanHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.accordionControl1)).EndInit();
            this.accordionControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl tcMain;
        private DevExpress.XtraTab.XtraTabPage tpAccount;
        private DevExpress.XtraTab.XtraTabPage tpBanHistory;
        private DevExpress.XtraBars.Navigation.AccordionControl accordionControl1;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement1;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement2;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement3;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit tbPlayerID;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit tbAccountID;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cbPenaltyType;
        private DevExpress.XtraEditors.SpinEdit seTime;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.ComboBoxEdit ceDurationType;
        private DevExpress.XtraEditors.SimpleButton btnCancelPenalty;
        private DevExpress.XtraEditors.SimpleButton btnSubmitPenalty;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.MemoEdit meReason;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl lblCharactersRemaining;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement5;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement6;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement7;
        private DevExpress.XtraBars.Navigation.AccordionContentContainer accordionContentContainer1;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement8;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement9;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement10;
        private DevExpress.XtraBars.Navigation.AccordionControlElement accordionControlElement11;
        private DevExpress.XtraEditors.LabelControl lblExpireDate;
        private DevExpress.XtraEditors.CheckButton chChangeAccountName;
        private DevExpress.XtraEditors.CheckButton chChangePlayerName;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.MarqueeProgressBarControl pbWaiting;
    }
}