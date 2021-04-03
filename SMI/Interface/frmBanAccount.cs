/**
 * ______________________________________________________
 * This file is part of ko-administration-tool project.
 * 
 * @author       Mustafa Kemal Gılor <mustafagilor@gmail.com> (2017)
 * .
 * SPDX-License-Identifier:	MIT
 * ______________________________________________________
 */

using KAI.Declarations;
using System;

namespace KAI.Interface
{
    public partial class frmPunishPlayer : DevExpress.XtraEditors.XtraForm
    {

        public frmPunishPlayer(User usr, AccountPenalty.AccountPenaltyType type)
        {
            InitializeComponent();
            /* Preselect */
            tbAccountID.Text = usr.AccountID;
            tbPlayerID.Text = usr.CharacterID;
            switch (type)
            {

                case AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_PRISON:
                    cbPenaltyType.SelectedIndex = 0;
                    break;
                case AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_BAN:
                    cbPenaltyType.SelectedIndex = 1;
                    break;
                case AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_MUTE:
                    cbPenaltyType.SelectedIndex = 2;
                    break;
                case AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_TRADE:
                    cbPenaltyType.SelectedIndex = 3;
                    break;
                case AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_MERCHANT:
                    cbPenaltyType.SelectedIndex = 4;
                    break;
                case AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_ATTACK:
                    cbPenaltyType.SelectedIndex = 5;
                    break;
                case AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_ZONE_CHANGE:
                    cbPenaltyType.SelectedIndex = 6;
                    break;
                case AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_LETTER:
                    cbPenaltyType.SelectedIndex = 7;
                    break;

            }
        }

        public frmPunishPlayer(PlayerListEntry ple)
        {
            InitializeComponent();
            tbAccountID.Text = ple.AccountID;
            tbPlayerID.Text = ple.PlayerID;
        }

        public frmPunishPlayer(User usr)
        {
            InitializeComponent();
            tbAccountID.Text = usr.AccountID;
            tbPlayerID.Text = usr.CharacterID;

        }
        public frmPunishPlayer()
        {
            InitializeComponent();
            chChangeAccountName.PerformClick();
        }


        private void frmPunishPlayer_Load(object sender, EventArgs e)
        {
            UpdateExpirationLabel();
            StaticReference.PunishFormReference = this;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            
        }

        private void rgDuration_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void tpAccount_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

            
        }

        private void accordionControl1_Click(object sender, EventArgs e)
        {

        }

        private void meReason_EditValueChanged(object sender, EventArgs e)
        {
            lblCharactersRemaining.Text = $"{150 - meReason.Text.Length} character(s) remaining";
        }
        private ulong UpdateExpirationLabel()
        {
            ulong value_seconds = (ulong)seTime.Value;
            switch (ceDurationType.SelectedIndex)
            {
                /* Seconds */
                case 0:
                    break;
                /* Minutes */
                case 1:
                    value_seconds *= 60;
                    break;
                /* Hours */
                case 2:
                    value_seconds *= (60 * 60);
                    break;
                /* Days */
                case 3:
                    value_seconds *= (60 * 60 * 24);
                    break;
                /* Weeks */
                case 4:
                    value_seconds *= (60 * 60 * 24 * 7);
                    break;
                /* Months */
                case 5:
                    value_seconds *= (60 * 60 * 24 * 30);
                    break;
                /* Years */
                case 6:
                    value_seconds *= (60 * 60 * 24 * 365); // 365 days
                    value_seconds += (60 * 60 * 6); // 6 hours
                    break;
                /* Permanent */
                case 7:
                    lblExpireDate.Text = "the penalty will never expire.";
                    return 0;

            }

            DateTime expiration_date = DateTime.Now.AddSeconds(value_seconds);
            lblExpireDate.Text = $"the penalty will expire on {expiration_date.ToString()}";
            return value_seconds;
        }
        private void ceDurationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateExpirationLabel();
        }

        private void seTime_EditValueChanged(object sender, EventArgs e)
        {
            UpdateExpirationLabel();
        }

        private void chChangeAccountName_CheckedChanged(object sender, EventArgs e)
        {
            tbAccountID.Properties.ReadOnly = !chChangeAccountName.Checked;
            chChangeAccountName.Text = chChangeAccountName.Checked ? "E" : "D";
        }

        private void chChangePlayerName_CheckedChanged(object sender, EventArgs e)
        {
            tbPlayerID.Properties.ReadOnly = !chChangePlayerName.Checked;
            chChangePlayerName.Text = chChangePlayerName.Checked ? "E" : "D";
        }

        public void OperationResponseReceive(byte result)
        {
            switch(result)
            {
                case 0:
                    HideMarqueeBarShowResultMessage("Failure!");
                    break;
                case 1:
                    HideMarqueeBarShowResultMessage("Success!");
                    break;
                case 2:
                    HideMarqueeBarShowResultMessage("No authority!");
                    break;
            }
        }

        public void ShowMarqueeBar(string msg)
        {
            this.Enabled = false;
            pbWaiting.Text = msg;
            pbWaiting.Properties.MarqueeWidth = 50;
            pbWaiting.Show(); pbWaiting.Properties.Paused = false;
        }

       

        public void HideMarqueeBarShowResultMessage(string msg)
        {
           new System.Threading.Thread((object o) => {
                System.Threading.Thread.Sleep(1500); pbWaiting.Properties.MarqueeWidth = 0; pbWaiting.Properties.Paused = true; pbWaiting.Reset(); pbWaiting.Text = msg; System.Threading.Thread.Sleep(1500);
                pbWaiting.Hide();
               meReason.Text = "";
               this.Invoke(new Action(() => { this.Enabled = true; }));
               
            }).Start();
            
        }

        private void btnSubmitPenalty_Click(object sender, EventArgs e)
        {

            AccountPenalty newPenalty = new AccountPenalty();

            if(meReason.Text.Length <50)
            {
                Common.Declarations.CommonReference.ShowWarning(this, "Reason should be at least 50 character(s) long.");
                return;
            }
            if(!string.IsNullOrEmpty(tbAccountID.Text))
            {
                newPenalty.TypeOfName = AccountPenalty.NameType.NAME_TYPE_ACCOUNT;
                newPenalty.UserID = tbAccountID.Text;
            }
            else if (!string.IsNullOrEmpty(tbPlayerID.Text))
            {
                newPenalty.TypeOfName = AccountPenalty.NameType.NAME_TYPE_CHARACTER;
                newPenalty.UserID = tbPlayerID.Text;
            }
            else
            {
                Common.Declarations.CommonReference.ShowWarning(this, "You have to provide either an account id or player id.");
                return;
            }

            newPenalty.ManagerID = CurrentManagerInfo.UserID;
            newPenalty.TypeOfOperation = AccountPenalty.OperationType.OPERATION_TYPE_SET;

            switch(cbPenaltyType.SelectedIndex)
            {
                case 0:
                    newPenalty.TypeOfPenalty = AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_PRISON;
                    break;
                case 1:
                    newPenalty.TypeOfPenalty = AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_BAN;
                    break;
                case 2:
                    newPenalty.TypeOfPenalty = AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_MUTE;
                    break;
                case 3:
                    newPenalty.TypeOfPenalty = AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_TRADE;
                    break;
                case 4:
                    newPenalty.TypeOfPenalty = AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_MERCHANT;
                    break;
                case 5:
                    newPenalty.TypeOfPenalty = AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_ATTACK;
                    break;
                case 6:
                    newPenalty.TypeOfPenalty = AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_ZONE_CHANGE;
                    break;
                case 7:
                    newPenalty.TypeOfPenalty = AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_LETTER;
                    break;
            }
            ulong duration = UpdateExpirationLabel();
            newPenalty.StartDate = (ulong)Common.Declarations.CommonReference.GetUnixTimestamp();
            newPenalty.EndDate = newPenalty.StartDate + duration;
            newPenalty.Permanent = (ceDurationType.SelectedIndex == 7);
            newPenalty.Reason = meReason.Text;

            /* Send request to the server */
            StaticReference.ClientCore.SendPenaltyRequest(newPenalty);
            ShowMarqueeBar("Processing your request..");
        }
    }
}