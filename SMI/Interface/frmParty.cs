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
using System.ComponentModel;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using KAI.Declarations;
using System.Diagnostics;
using System.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;
using System.Drawing;

namespace KAI.Interface
{
  
    public partial class frmParty : XtraForm
    {
        class CustomFormatter : IFormatProvider, ICustomFormatter
        {
            private ProgressBarControl ProgressBar { get; set; }

            public CustomFormatter(ProgressBarControl progressBarControl)
            {
                ProgressBar = progressBarControl;
            }
            // implementing the GetFormat method of the IFormatProvider interface
            public object GetFormat(System.Type type)
            {
                return this;
            }

            // implementing the Format method of the ICustomFormatter interface
            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                var pbControl = ((CustomFormatter)formatProvider).ProgressBar;
                return $"{pbControl.Position}/{pbControl.Properties.Maximum}";
            }
        }
        public enum PartySubOpcode
        {
            PARTY_CREATE = 0x01,    // Party Group Create
            PARTY_PERMIT = 0x02,    // Party Insert Permit
            PARTY_INSERT = 0x03,    // Party Member Insert
            PARTY_REMOVE = 0x04,    // Party Member Remove
            PARTY_DELETE = 0x05,    // Party Group Delete
            PARTY_HPCHANGE = 0x06,  // Party Member HP change
            PARTY_LEVELCHANGE = 0x07,   // Party Member Level change
            PARTY_CLASSCHANGE = 0x08,   // Party Member Class Change
            PARTY_STATUSCHANGE = 0x09,  // Party Member Status ( disaster, poison ) Change
            PARTY_REGISTER = 0x0A,  // Party Message Board Register
            PARTY_REPORT = 0x0B,    // Party Request Message Board Messages
            PARTY_PROMOTE = 0x1C,   // Promotes user to party leader
            PARTY_ALL_STATUSCHANGE = 0x1D,	// Sets the specified status of the selected party members to 1.
        }
        private readonly Button _enter = new Button();
        private readonly Button _escape = new Button();
        private readonly System.Data.DataTable PartyMembers = new System.Data.DataTable("party_members");
        private readonly ushort party_id ;
        private readonly BindingList<PartyMember> _partyMembers = new BindingList<PartyMember>();
        public bool FromMaster = false;

        private ProgressBarControl[] HealthBars, ManaBars;
        private GroupControl[] MemberGroups;

        int _kcount = 0, _ecount = 0;
        int _kwarcnt = 0, _krogcnt = 0, _kmagcnt = 0, _kprcnt = 0;
        int _ewarcnt = 0, _erogcnt = 0, _emagcnt = 0, _eprcnt = 0;
        int _sumOfLevels = 0, _sumOfHP = 0;

        public frmParty(mainFrm parent,ushort party_id)
        {
            this.party_id = party_id; 
            InitializeComponent();
            InitializeMemberTable();
            InitializeMemberGrid();
            InitializePartyBar();

            CancelButton = _escape;
            AcceptButton = _enter;
            _escape.Click += Escape_Click;
            _enter.Click += Enter_Click;

        }


        private void frmParty_Load(object sender, EventArgs e)
        {
            ForAllControls(this, c =>
            {
                if (c.GetType() != typeof(LabelControl))
                    return;
                // c.Parent = pbParty;
                // c.Parent = xtraUserControl1;
                c.BringToFront();
            });

            // pbParty.SendToBack();
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            if (!teChat.EditorContainsFocus)
                return;
            //  StaticReference.ClientCore.SendPrivateChat(CurrentUser.SocketID, tePrivateChat.Text);
            StaticReference.ClientCore.SendPartyChat(party_id, string.Format("(Operator){0}", CurrentManagerInfo.UserID), teChat.Text);
            AddPartyChatLine(teChat.Text, Color.FromArgb(0, 0, 192, 192));
            teChat.Text = "";
        }

        private void Escape_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        #region UI Operation 
        private void InitializePartyBar()
        {
            HealthBars = new[] { hpBar1, hpBar2, hpBar3, hpBar4, hpBar5, hpBar6, hpBar7, hpBar8 };
            ManaBars = new[] { mpBar1, mpBar2, mpBar3, mpBar4, mpBar5, mpBar6, mpBar7, mpBar8 };
            MemberGroups = new[] { gcPartyMember1, gcPartyMember2, gcPartyMember3, gcPartyMember4, gcPartyMember5, gcPartyMember6, gcPartyMember7, gcPartyMember8 };

            foreach(ProgressBarControl pbc in HealthBars)
            {
                pbc.Properties.DisplayFormat.FormatType = FormatType.Custom;
                pbc.Properties.DisplayFormat.Format = new CustomFormatter(pbc);
            }

            foreach (ProgressBarControl pbc in ManaBars)
            {
                pbc.Properties.DisplayFormat.FormatType = FormatType.Custom;
                pbc.Properties.DisplayFormat.Format = new CustomFormatter(pbc);
            }
        }

        private void UpdateStatistics()
        {
            byte membercount = 0;
            foreach (var v in MemberGroups)
                if (v.Visible) membercount++;

            teAvgLevel.Text = (_sumOfLevels / membercount).ToString();
            teAvgHP.Text = (_sumOfHP / membercount).ToString();

            teElmo.Text = _ecount.ToString();
            teKarus.Text = _kcount.ToString();

            teKarusWarrior.Text = _kwarcnt.ToString();
            teKarusRogue.Text = _krogcnt.ToString();
            teKarusMage.Text = _kmagcnt.ToString();
            teKarusPriest.Text = _kprcnt.ToString();

            teElmoWarrrior.Text = _ewarcnt.ToString();
            teElmoRogue.Text = _erogcnt.ToString();
            teElmoMage.Text = _emagcnt.ToString();
            teElmoPriest.Text = _eprcnt.ToString();
        }

        private void UpdatePartyMemberHP(byte member_index,ushort current,ushort max)
        {
            if (member_index > 7 || member_index < 0)
                return;
            HealthBars[member_index].Properties.Maximum = max;
            HealthBars[member_index].Properties.Minimum = 0;
            HealthBars[member_index].Position = current;
        }
        private void UpdatePartyMemberMP(byte member_index, ushort current, ushort max)
        {
            if (member_index > 7 || member_index < 0)
                return;
            ManaBars[member_index].Properties.Maximum = max;
            ManaBars[member_index].Properties.Minimum = 0;
            ManaBars[member_index].Position = current;
        }

        private void UpdatePartyMemberName(byte member_index, string name, byte level)
        {
            if (member_index > 7 || member_index < 0)
                return;
            MemberGroups[member_index].Text = string.Format("{0} LV. {1}", name, level);
        }

        private void SetPartyMemberVisible(byte member_index,bool value)
        {
            if (member_index > 7 || member_index < 0)
                return;
            MemberGroups[member_index].Visible = value;
        }



        private void InitializeMemberGrid()
        {
            gvParty.Columns.Clear();
            gcParty.BeginUpdate();
            gcParty.DataSource = PartyMembers;

            foreach (GridColumn col in gvParty.Columns)
            {
                col.AppearanceCell.Options.UseTextOptions = true;
                col.AppearanceHeader.Options.UseTextOptions = true;

                col.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
                col.AppearanceCell.TextOptions.VAlignment = VertAlignment.Center;
                col.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                col.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;
                col.BestFit();
            }

            gcParty.Refresh();
            gcParty.EndUpdate();

            gvParty.RefreshData();
            gvParty.OptionsBehavior.Editable = false;
            gvParty.OptionsBehavior.ReadOnly = true;
            gvParty.OptionsSelection.EnableAppearanceFocusedCell = false;
            gcParty.Refresh();
        }
        #endregion
    
        #region Member table operation
        private void InitializeMemberTable()
        {
            /* Reset primary key(s) */
            PartyMembers.PrimaryKey = new DataColumn[] { };
            /* Erase columns */
            PartyMembers.Columns.Clear();
            /* Erase row(s) */
            PartyMembers.Rows.Clear();
            /* Don't know wtf is that command doing, but execute it.*/
            PartyMembers.Clear();

            /*  /*
	        Packet result(WIZ_PARTY, uint8(PARTY_INSERT));
	        result << pNewPlayer->GetSocketID()
		        << uint8(3) // success
		        << pNewPlayer->GetName()
		        << pNewPlayer->m_sMaxHp << pNewPlayer->m_sCurrentHp
		        << pNewPlayer->GetLevel() << pNewPlayer->m_sClass
		        << pNewPlayer->m_sMaxMp << pNewPlayer->m_sMp;*/

            PartyMembers.Columns.Add("Socket ID", typeof(short));
            PartyMembers.Columns.Add("Player ID", typeof(string));
            PartyMembers.Columns.Add("Account ID", typeof(string));
            PartyMembers.Columns.Add("Clan name", typeof(string));
            PartyMembers.Columns.Add("Class", typeof(string));
            PartyMembers.Columns.Add("Level", typeof(string));
            PartyMembers.Columns.Add("Nation", typeof(string));
            PartyMembers.Columns.Add("HP", typeof(int));
            PartyMembers.Columns.Add("MaxHP", typeof(int));
            PartyMembers.Columns.Add("MP", typeof(int));
            PartyMembers.Columns.Add("MaxMP", typeof(int));


            PartyMembers.PrimaryKey = new[] { PartyMembers.Columns[0] };
            
        }

        private void InsertPartyMember(PartyMember mem)
        {
            PartyMembers.BeginLoadData();
            var dr = PartyMembers.NewRow();

            DataRow playerRow = StaticReference.GetPlayer(mem.SocketID);

            if (playerRow == null)
                return;

            dr["Socket ID"] = mem.SocketID;
            dr["Player ID"] = mem.Name;
            dr["Account ID"] = playerRow["Account ID"];
            dr["Clan name"] = playerRow["Clan"];
            dr["Class"] = playerRow["Class"];
            dr["Level"] = mem.Level;
            dr["Nation"] = playerRow["Nation"];
            dr["HP"] = mem.HP;
            dr["MaxHP"] = mem.MaxHP;
            dr["MP"] = mem.MP;
            dr["MaxMP"] = mem.MaxMP;

            PartyMembers.Rows.Add(dr);
            PartyMembers.EndLoadData();
        }

        private void RemovePartyMember(ushort id)
        {
            var row = PartyMembers.Rows.Find(id);
           // row?.BeginEdit();
            if (row != null)
            {
                PartyMembers.Rows.Remove(row);
            }
           // row?.EndEdit();
        }

        #endregion

        #region Packet handling

        public void HandlePartyChat(Packet receivedPacket)
        {
            byte bType = receivedPacket.Read<byte>();
            byte bNation = receivedPacket.Read<byte>();
            short sender_id = receivedPacket.Read<short>();
            string sender_name = receivedPacket.Read<string>(false);
            string message = receivedPacket.Read<string>(true);
            PartyChatMsgRecv(string.Format("{0} : {1}", sender_name, message));
        }

        public void HandlePartyMemberUpdate(Packet receivedPacket)
        {
            /* skip wiz_party */
         // receivedPacket.Read<byte>();
            byte opcode = receivedPacket.Read<byte>();
            switch((PartySubOpcode)opcode)
            {
                case PartySubOpcode.PARTY_INSERT:
                    HandlePartyInsert(receivedPacket);
                    break;
                case PartySubOpcode.PARTY_REMOVE:
                    HandlePartyRemove(receivedPacket);
                    break;
                case PartySubOpcode.PARTY_STATUSCHANGE:
                    HandlePartyStatusChange(receivedPacket);
                    break;
                case PartySubOpcode.PARTY_LEVELCHANGE:
                    HandlePartyLevelChange(receivedPacket);
                    break;
                case PartySubOpcode.PARTY_HPCHANGE:
                    HandlePartyHpChange(receivedPacket);
                    break;
                case PartySubOpcode.PARTY_CLASSCHANGE:
                    HandlePartyClassChange(receivedPacket);
                    break;
                default:
                    Trace.TraceWarning("frmParty::HandlePartyPacket - Unhandled party opcode {0}", opcode);
                    break;

            }
        }

        public void LoadInitialPartyInfo(Packet pkt)
        {
            ushort leader_id = pkt.Read<ushort>();
            byte member_count = pkt.Read<byte>();

           

            for (byte i = 0; i < member_count; i++)
            {
                byte pos = pkt.Read<byte>();
                PartyMember member = new PartyMember()
                {
                    SocketID = pkt.Read<ushort>(),
                    Name = pkt.Read<string>(true),
                    Nation = pkt.Read<byte>(),
                    Level = pkt.Read<byte>(),
                    Class = pkt.Read<ushort>(),
                    ClanID = pkt.Read<ushort>(),
                    HP = pkt.Read<ushort>(),
                    MaxHP = pkt.Read<ushort>(),
                    MP = pkt.Read<ushort>(),
                    MaxMP = pkt.Read<ushort>()
                };

                if(member.SocketID == leader_id)
                {
                    teLeader.Text = member.Name;
                    Text = $"Party Tracking | {member.Name}'s party";
                }

                #region Statistics 

                if (member.Nation == 1)
                    _kcount++;
                else
                    _ecount++;

                switch(member.Class)
                {
                    case 101:
                    case 105:
                    case 106:
                        _kwarcnt++;
                        break;
                    case 102:
                    case 107:
                    case 108:
                        _krogcnt++;
                        break;
                    case 103:
                    case 109:
                    case 110:
                        _kmagcnt++;
                        break;
                    case 104:
                    case 111:
                    case 112:
                        _kprcnt++;
                        break;

                    case 201:
                    case 205:
                    case 206:
                        _ewarcnt++;
                        break;
                    case 202:
                    case 207:
                    case 208:
                        _erogcnt++;
                        break;
                    case 203:
                    case 209:
                    case 210:
                        _emagcnt++;
                        break;
                    case 204:
                    case 211:
                    case 212:
                        _eprcnt++;
                        break;
                }

                _sumOfLevels += member.Level;
                _sumOfHP += member.MaxHP;
                #endregion


                SetPartyMemberVisible(pos, true);
                UpdatePartyMemberName(pos, member.Name, member.Level);
                UpdatePartyMemberHP(pos, member.HP, member.MaxHP);
                UpdatePartyMemberMP(pos, member.MP, member.MaxMP);
                InsertPartyMember(member);
            }

            UpdateStatistics();
        }

        private void HandlePartyClassChange(Packet receivedPacket)
        {
            /*Packet result(WIZ_PARTY);
	result << uint8(PARTY_CLASSCHANGE) << pWho->GetSocketID() << uint16(pWho->m_sClass);
	SendAllMembers(&result, pWho);*/
        }

        private void HandlePartyHpChange(Packet receivedPacket)
        {
            /*Packet result(WIZ_PARTY, uint8(PARTY_HPCHANGE));
	            result << pPlayer->GetSocketID()
		            << pPlayer->m_sMaxHp << pPlayer->m_sCurrentHp
		            << pPlayer->m_sMaxMp << pPlayer->m_sMp;

	            SendAllMembers(&result);*/
            short socket_id = receivedPacket.Read<short>();
            ushort hp, max_hp, mp, max_mp;
            max_hp = receivedPacket.Read<ushort>();
            hp = receivedPacket.Read<ushort>();
            max_mp = receivedPacket.Read<ushort>();
            mp = receivedPacket.Read<ushort>();
            byte member_index = receivedPacket.Read<byte>();

            UpdatePartyMemberHP(member_index, hp, max_hp);
            UpdatePartyMemberMP(member_index, mp, max_mp);
        }

        private void HandlePartyLevelChange(Packet receivedPacket)
        {
            /*Packet result(WIZ_PARTY, uint8(PARTY_LEVELCHANGE));
	result << pPlayer->GetSocketID() << pPlayer->GetLevel();
	SendAllMembers(&result);*/
            UInt16 leaver_id = receivedPacket.Read<ushort>();
            byte newLevel = receivedPacket.Read<byte>();
            byte member_index = receivedPacket.Read<byte>();


        }

        private void HandlePartyStatusChange(Packet receivedPacket)
        {
            /*Packet result(WIZ_PARTY, uint8(PARTY_STATUSCHANGE));
	result << pWho->GetSocketID() << bStatus << bResult;
	SendAllMembers(&result);*/
        }

        private void HandlePartyRemove(Packet receivedPacket)
        {
            UInt16 leaver_id = receivedPacket.Read<ushort>();
            byte member_index = receivedPacket.Read<byte>();
            if (member_index >= 0 && member_index <= 7)
            {

                SetPartyMemberVisible(member_index, false);
                PartyChatMemberLeave(MemberGroups[member_index].Text);
                UpdatePartyMemberName(member_index, "", 0);
                UpdatePartyMemberHP(member_index, 0, 0);
                UpdatePartyMemberMP(member_index, 0, 0);
            }
            RemovePartyMember(leaver_id);
        }

        private void HandlePartyInsert(Packet receivedPacket)
        {
            UInt16 sockid = receivedPacket.Read<UInt16>();
            receivedPacket.Read<byte>(); // skip indicator
            PartyMember newMember = new PartyMember()
            {
                SocketID = sockid,
                Name = receivedPacket.Read<string>(true),
                MaxHP = receivedPacket.Read<ushort>(),
                HP = receivedPacket.Read<ushort>(),
                Level = receivedPacket.Read<byte>(),
                Class = receivedPacket.Read<ushort>(),
                MaxMP = receivedPacket.Read<ushort>(),
                MP = receivedPacket.Read<ushort>()
            };
           
            byte member_index = receivedPacket.Read<byte>();

            StaticReference.frmCOMSink.BeginInvoke(new Action(() => {
                SetPartyMemberVisible(member_index, true);
                UpdatePartyMemberName(member_index, newMember.Name, newMember.Level);
                UpdatePartyMemberHP(member_index, newMember.HP, newMember.MaxHP);
                UpdatePartyMemberMP(member_index, newMember.MP, newMember.MaxMP);
                PartyChatMemberJoin(newMember.Name);
            }));
          
            InsertPartyMember(newMember);
        }

        private void frmParty_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FromMaster)
                return;
            StaticReference.RemovePartyForm(party_id, true);
            StaticReference.ClientCore.SendPartyTrackRequest(party_id, false);
        }

        private void btnDisband_Click(object sender, EventArgs e)
        {
            if(Common.Declarations.CommonReference.ShowQuestion(this,"Are you sure you want to disband this party?") == DialogResult.Yes)
            {
                StaticReference.ClientCore.SendPartyDisbandReq(party_id);
                /* No response required since disband will automatically received by the program */
            }
        }

        #endregion

        #region Chat UI

        private void PartyChatMsgRecv(string msg)
        {
            AddPartyChatLine(msg, Color.FromArgb(0, 0, 192, 192));
        }

        private void PartyChatMemberJoin(string name)
        {
            AddPartyChatLine_Italic(string.Format("{0} has joined to the party.", name), Color.FromArgb(0, 0, 192, 192));
        }

        private void PartyChatMemberLeave(string name)
        {
            AddPartyChatLine_Italic(string.Format("{0} has left the party.", name), Color.FromArgb(0, 0, 192, 192));
        }

        private void AddPartyChatLine_Italic(string text, Color color)
        {
            rtbChat.SelectionFont = new Font("Verdana", 6.11f, FontStyle.Italic);
            rtbChat.SelectionColor = color;
            rtbChat.SelectedText = text + "\n";
            rtbChat.SelectionColor = Color.White;
            rtbChat.ScrollToCaret();
        }

        private void AddPartyChatLine(string text, Color color)
        {
            rtbChat.SelectionFont = new Font("Verdana", 7.11f, FontStyle.Regular);
            rtbChat.SelectionColor = color;
            rtbChat.SelectedText = $"({DateTime.Now.ToLongTimeString()})    {text}{"\n"}"; rtbChat.SelectionColor = Color.White;
            rtbChat.ScrollToCaret();
        }

        #endregion

        public void OnPartyDisband(bool userClosing)
        {
            FromMaster = userClosing;
            if (!userClosing)
            {
             //   StaticReference.ClientCore.SendPartyTrackRequest(party_id, false);
                Common.Declarations.CommonReference.ShowWarning(this, "This party has been disbanded.");
                Text += " [DISBANDED]";
                //Close();
            }
        }


        public static void ForAllControls(Control parent, Action<Control> action)
        {
            foreach (Control c in parent.Controls)
            {
                action(c);
                ForAllControls(c, action);
            }
        }

      

    


        private void textEdit2_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}