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
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using KAI.Common.Declarations;
using KAI.Declarations;

namespace KAI.Interface
{
    public partial class frmCharacter : XtraForm
    {

        #region Definition

        private byte _currentBagIndex = 1;

        public bool FromMaster = false;
        private bool _isInventoryReadOnly = true;

       // private InventorySlot _cutSlot = null;
        private InventorySlot _copiedSlot = null;

        private readonly InventorySlot[] _inventoryCopy = new InventorySlot[72];
        /* User status flags */
        public readonly User CurrentUser = new User();
        private readonly mainFrm _myParent;
        private readonly Color _clrIncomingMessage = Color.FromArgb(255, 255, 255, 0);
        private readonly Color _clrOutgoingMessage = Color.FromArgb(255, 0, 0xFF, 0xCE);

        readonly Font _countFont = new Font("Verdana", 8.00F);

        private readonly Dictionary<int, XtraPanel> _equipments = new Dictionary<int, XtraPanel>();
        private readonly Dictionary<int, XtraPanel> _inventory = new Dictionary<int, XtraPanel>();
        private readonly Dictionary<int, XtraPanel> _cosplay = new Dictionary<int, XtraPanel>();
        private readonly Dictionary<int, XtraPanel> _magicBag = new Dictionary<int, XtraPanel>();


        private readonly Button _enter = new Button();
        private readonly Button _escape = new Button();
        private Color _rtfColor;

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



        #endregion

        #region Construction
        public frmCharacter(mainFrm parent)
        {
           
            _myParent = parent;
            InitializeComponent();
            AllowDrop = true;
            CurrentUser.OnSlotChanged += CurrentUser_OnSlotChanged;
            Closing += frmCharacter_Closing;
            /* Initialize backup array */
            for (var i = 0; i < 72; i++)
            {
                _inventoryCopy[i] = new InventorySlot();
            }
            chbBag1.Parent = pbInventory;
            chbBag2.Parent = pbInventory;
            chbBag1.Location = pbInventory.PointToClient(chbBag1.Location);
            chbBag2.Location = pbInventory.PointToClient(chbBag2.Location);
            
            chbBag1.Location = new Point(chbBag1.Location.X + 8, chbBag1.Location.Y + 35);
            chbBag2.Location = new Point(chbBag2.Location.X + 8, chbBag2.Location.Y + 35);
            //  StaticReference.DisableDropRecursive(this);
            CancelButton = _escape;
            AcceptButton = _enter;
            _escape.Click += Escape_Click;
            _enter.Click += Enter_Click;
            TopLevel = true;
            AddPrivateChatLine_Italic("You haven't started a private chat yet.\nTo do so, just type a message below.", Color.White);
            InitializeInventorySlots();
         
            rtfItemData.Parent = pbInventory;
            rtfItemData.WordWrap = false;
            rtfItemData.ContentsResized += RtfItemData_ContentsResized;
            healthBar.Properties.DisplayFormat.FormatType = FormatType.Custom;
            healthBar.Properties.DisplayFormat.Format = new CustomFormatter(healthBar);

            manaBar.Properties.DisplayFormat.FormatType = FormatType.Custom;
            manaBar.Properties.DisplayFormat.Format = new CustomFormatter(manaBar);
        }

     

        private void frmCharacter_Load(object sender, EventArgs e)
        {
            foreach (var v in CommonReference.ZoneList)
            {
                cbZones.Properties.Items.Add(v.Value + ":" + v.Key);

            }
        }

        private void Escape_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void frmCharacter_Closing(object sender, CancelEventArgs e)
        {
            if (FromMaster)
                return;
            StaticReference.RemoveCharacterForm(CurrentUser.SocketID);
            StaticReference.ClientCore.SendTrackingUserUnset(CurrentUser.SocketID);
            // Main form navbar element
            _myParent.Invoke(new Action(() =>
            {
                foreach (Control c in _myParent.npTrackedCharacters.Controls)
                {
                    if(c is HyperlinkLabelControl)
                    {
                        if (Convert.ToInt16(c.Tag) != CurrentUser.SocketID)
                            continue;
                        _myParent.npTrackedCharacters.Controls.Remove(c);
                    }
                    return;
                }
            }));
        }



        public void OnCharacterLogout()
        {
            CommonReference.ShowWarning(this,
                $"Traced character(AID :{CurrentUser.AccountID}, CID : {CurrentUser.CharacterID}) has just logout from the server.\nYou can no longer trace this character.");
            Close();

        }
        #endregion

        #region Inventory related


            private void SetInventoryReadOnly(bool value)
            {
                _isInventoryReadOnly = nbOptions_EnableEdit.Enabled = value;
                if (!_isInventoryReadOnly)
                {
                    /* Backup the inventory data */
                    for (var i = 0; i < 72; i++)
                    {
                        CurrentUser.Inventory[i].CopyTo(_inventoryCopy[i]);
                    }
                }
                ngOptions_DisableEdit.Enabled = !value;
                ngOptions_ImportPlayerInventory.Enabled = !value;
                CommonReference.ShowInformation(this, !_isInventoryReadOnly ? "Edit mode enabled." : "Edit mode disabled.");
            }

            #region Inventory render

            internal void RenderBagSlots()
            {
                int start = 0,end = 0;
                if (_currentBagIndex == 1)
                {
                    start = 48;
                    end = 59;
                }
                else
                {
                    start = 60;
                    end = 71;
                }
                for (var i = start; i <= end; i++)
                {
                    var slot = CurrentUser.Inventory[i];
                    Image img = null;
                    if (!slot.isEmpty())
                    {
                        img = slot.Info == null ? CommonReference.GetIconByIconID((uint)0) : CommonReference.GetIconByIconID((uint)slot.Info.IconID);

                    }
                    if (img != null)
                    {
                        switch ((ItemFlag) slot.Flag)
                        {
                            case ItemFlag.ITEM_FLAG_SEALED:
                                img = ((Bitmap) img).ColorTint(32/100.0f, 0, 0);
                                break;
                            case ItemFlag.ITEM_FLAG_DUPLICATE:
                                img = ((Bitmap) img).ColorTint(0/100.0f, 30/100.0f, 50/100.0f);
                                break;
                        }
                    }

                    _magicBag[i - start].Tag = slot;
                    _magicBag[i - start].BackgroundImage = img;
                }
        
            }

            internal void RenderInventory()
            {
                /* Suspend layout temporarily */
                pbInventory.SuspendLayout();

                for (var i = 0; i < 72; i++)
                {
                    var slot = CurrentUser.Inventory[i];
                    Image img = null;
                    if (!slot.isEmpty())
                    {
                        img = slot.Info == null ? CommonReference.GetIconByIconID((uint)0) : CommonReference.GetIconByIconID((uint)slot.Info.IconID);

                    }

                    if (img != null)
                    {
                        switch ((ItemFlag) slot.Flag)
                        {
                            case ItemFlag.ITEM_FLAG_SEALED:
                                img = ((Bitmap) img).ColorTint(32/100.0f, 0, 0);
                                break;
                            case ItemFlag.ITEM_FLAG_DUPLICATE:
                                img = ((Bitmap) img).ColorTint(0/100.0f, 30/100.0f, 50/100.0f);
                                break;
                        }
                    }


                    if (i < 14)
                    {
                        /* Equipment */
                        _equipments[i].Tag = slot;
                        _equipments[i].BackgroundImage = img;
                    }
                    else if (i >= 14 && i < 42)
                    {
                        /* Inventory */
                        _inventory[i - 14].Tag = slot;
                        _inventory[i - 14].BackgroundImage = img;
                    }
                    else if (i >= 42 && i <= 47)
                    {
                        /* Cosplay */
                        _cosplay[i - 42].Tag = slot; _cosplay[i - 42].BackgroundImage = img;
                    }

            
                }
                chbBag1.Checked = true;
                pbInventory.Controls.SetChildIndex(rtfItemData, 0);pbInventory.ResumeLayout();
            }

            private void OnInventoryPanelPaint(object sender, PaintEventArgs e)
            {

                var senderPanel = sender as XtraPanel;
                Trace.Assert(null != senderPanel);
                var slot = senderPanel.Tag as InventorySlot;
                Trace.Assert(null != slot);
                var stringAlign = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Far };

                if (slot.Count <= 1)
                    goto skip_count_rendering;

                /* Draw item count */
                using (Brush brush = new SolidBrush(Color.FromArgb(255, 255, 255, 255)))
                {
                
                    e.Graphics.DrawString(slot.Count.ToString(), _countFont, brush, senderPanel.DisplayRectangle, stringAlign);
                }

                skip_count_rendering:/* Draw selection rectangle if hovered */
                if (!slot.Hovered)
                    return;

                /* Draw selection rectangle */
                using (var pen = new Pen(Color.Aquamarine, 3))
                    e.Graphics.DrawLines(pen, CommonReference.PanelGetBorder(0, 0, senderPanel.Width - 1, senderPanel.Height - 1));

            
            }


        #endregion

            #region Inventory slot initialization

        private XtraPanel CreateNewInventoryPanel(int x, int y)
        {
            var newPanel = new XtraPanel
            {
                Size = new Size(45, 45),
                Location = new Point(x, y),
                BackColor = Color.Transparent,
                AllowDrop = true,
                Name = $"InventoryPanel{x}{y}",
                ContextMenuStrip = cmsInventory,
            };
            newPanel.MouseHover += OnInventoryPanelMouseHover;
            newPanel.Paint += OnInventoryPanelPaint;
            newPanel.MouseEnter += OnInventoryPanelMouseEnter;
            newPanel.MouseLeave += OnInventoryPanelMouseLeave;
            newPanel.MouseDown += OnInventoryPanelMouseDown;
            newPanel.DragOver += OnInventoryPanelDragOver;
            newPanel.DragEnter += OnInventoryPanelDragEnter;
            newPanel.DragLeave += OnInventoryPanelDragLeave;
            newPanel.DragDrop += OnInventoryPanelDragDrop;

            return newPanel;
        }

        private void InitializeInventorySlots()
            {
                const int equipmentStartX = 420, equipmentStartY = 96;
                const int inventoryStartX = 232, inventoryStartY = 369;
                const int magicBagStartX = 28, magicBagStartY = 335;
                for (var i = 0; i < 28; i++)
                {
                    int plusValueX = 49 * (i % 7), plusValueY = 49 * (i / 7);
                    var newPanel = CreateNewInventoryPanel(inventoryStartX + plusValueX, inventoryStartY + plusValueY);
                    pbInventory.Controls.Add(newPanel);
                    _inventory.Add(i, newPanel);
                }

                for (var i = 0; i < 14; i++)
                {
                    int plusValueX = 52 * (i % 3), plusValueY = 52 * (i / 3);
                    var newPanel = CreateNewInventoryPanel(equipmentStartX + plusValueX, equipmentStartY + plusValueY);
                    pbInventory.Controls.Add(newPanel);
                    _equipments.Add(i, newPanel);
                }

                // for(var i = 0)

                /* Cospre & Magic Bag Slots */

                var cospreHelmetPanel = CreateNewInventoryPanel(80, 63);
                pbInventory.Controls.Add(cospreHelmetPanel);
                _cosplay.Add(0, cospreHelmetPanel);

                var cospreArmLeft = CreateNewInventoryPanel(28, 115);
                pbInventory.Controls.Add(cospreArmLeft);
                _cosplay.Add(1, cospreArmLeft);

                var cospreArmRight = CreateNewInventoryPanel(132, 115);
                pbInventory.Controls.Add(cospreArmRight);
                _cosplay.Add(2, cospreArmRight);

                var cospreArmorPanel = CreateNewInventoryPanel(80, 170);
                pbInventory.Controls.Add(cospreArmorPanel);
                _cosplay.Add(3, cospreArmorPanel);


                var cospreMagicBag1 = CreateNewInventoryPanel(56, 244);
                pbInventory.Controls.Add(cospreMagicBag1);
                _cosplay.Add(4, cospreMagicBag1);

                var cospreMagicBag2 = CreateNewInventoryPanel(110, 244);

                pbInventory.Controls.Add(cospreMagicBag2);
                _cosplay.Add(5, cospreMagicBag2);

                for (int i = 0; i < 12; i++)
                {
                    int plusValueX = 52 * (i % 3), plusValueY = 52 * (i / 3);
                    var newPanel = CreateNewInventoryPanel(magicBagStartX + plusValueX, magicBagStartY+ plusValueY);
                    pbInventory.Controls.Add(newPanel);
                   // newPanel.BackColor = Color.Blue;
                    _magicBag.Add(i, newPanel);
                }
            }

       

            #endregion

            #region Inventory Drag & Drop

            private void OnInventoryPanelDragDrop(object sender, DragEventArgs e)
            {
                var me = sender as XtraPanel;
                if (e.Data.GetDataPresent(DataFormats.Bitmap))
                    e.Effect = e.AllowedEffect & DragDropEffects.Move;
                else
                    e.Effect = DragDropEffects.None;
                DropTargetHelper.Drop(e.Data, new Point(e.X, e.Y), e.Effect);

                if (_isInventoryReadOnly)
                    return;

                if (me == null)
                    return;

                var mySlot = me.Tag as InventorySlot;
                if (mySlot == null)
                {
                    Trace.Assert(false, "mySlot is null.");
                    return;
                }
                string draggedPanel =  (string) e.Data.GetData("dragged_panel");
                var vayaq = pbInventory.Controls[draggedPanel] as XtraPanel;
                if (vayaq == null) // null 
                {
                    // if the "vayaq" is null, then source must be an outsider.
                    // Copying from outside requires an empty slot.
                    if (!mySlot.isEmpty())
                        return;

                    // There must be item information inside drag & drop info. 
                    // Extract it

                    InventorySlot dragged_slot = e.Data.GetData("dragged_slot") as InventorySlot;
                    if (dragged_slot == null)
                    {
                        Trace.WriteLine(
                            "OnInventoryPanelDragDrop() - Drag drop succeeded, but dragged slot information is null.");
                        return;
                    }
                    /* Copy information */
                    dragged_slot.CopyTo(CurrentUser.Inventory[mySlot.Pos]);
                    mySlot.Serial = CommonReference.GenerateItemSerial();
                    mySlot.Durability = mySlot.Info.Duration;
                    mySlot.Count = 1;
                    /* Re-render the slot */
                    CurrentUser_OnSlotChanged(mySlot.Pos);
                }
                else
                {
                    var draggedSlot = vayaq.Tag as InventorySlot;
                    // Internal copy, we should just exchange.
                    if (draggedSlot != null)
                        CurrentUser.DoSlotExchange(mySlot.Pos,draggedSlot.Pos);
                    // That's it.
                }

            }

            private static void OnInventoryPanelDragLeave(object sender, EventArgs e)
            {
                var me = sender as XtraPanel;
                DropTargetHelper.DragLeave(me);
            }

            private static void OnInventoryPanelDragEnter(object sender, DragEventArgs e)
            {
                var me = sender as XtraPanel;
                if (e.Data.GetDataPresent(DataFormats.Bitmap))
                    e.Effect = e.AllowedEffect & DragDropEffects.Move;
                else
                    e.Effect = DragDropEffects.None;
            
                DropTargetHelper.DragEnter(me, e.Data, new Point(e.X, e.Y), e.Effect);
            }

            private static void OnInventoryPanelDragOver(object sender, DragEventArgs e)
            {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.Bitmap))
                    e.Effect = e.AllowedEffect & DragDropEffects.Move;
                else
                    e.Effect = DragDropEffects.None;

                DropTargetHelper.DragOver(new Point(e.X, e.Y), e.Effect);
            }
            catch(Exception ex) { }
              
            }

            private  void OnInventoryPanelMouseDown(object sender, MouseEventArgs e)
            {
                if (_isInventoryReadOnly)
                    return;

                if (e.Button != MouseButtons.Left)
                    return;
                var senderPanel = sender as XtraPanel;
                Trace.Assert(null != senderPanel);
                var slot = senderPanel.Tag as InventorySlot;
                Trace.Assert(null != slot);
                if (slot.isEmpty())
                    return;
                DragSourceHelper.DoDragDrop(senderPanel, (Bitmap)senderPanel.BackgroundImage, new Point(e.X, e.Y),
                                DragDropEffects.Move,
                                new KeyValuePair<string, object>(DataFormats.Bitmap, senderPanel.BackgroundImage),
                                new KeyValuePair<string, object>("dragged_panel",senderPanel.Name),
                                 new KeyValuePair<string, object>("slot_data", slot));

            }

            #endregion

            #region Panel click events

            private void OnInventoryPanelMouseEnter(object sender, EventArgs e)
            {
                var senderPanel = sender as XtraPanel;
                Trace.Assert(null != senderPanel);
                var slot = senderPanel.Tag as InventorySlot;
                Trace.Assert(null != slot);
                slot.Hovered = true;
                /* Re-render the panel */
                senderPanel.Refresh();

                if (slot.ItemID <= 0)
                    return;
                rtfItemData.Tag = senderPanel;
                ShowItemDetails(slot);
                rtfItemData.Show();
            }

            private  void OnInventoryPanelMouseLeave(object sender, EventArgs e)
            {
                var senderPanel = sender as XtraPanel;
                Trace.Assert(null != senderPanel);
                var slot = senderPanel.Tag as InventorySlot;
                Trace.Assert(null != slot);
                slot.Hovered = false;
                senderPanel.Refresh();
                /* Hide item details */
                if (!slot.isEmpty())
                    rtfItemData.Hide();
            }
            private void OnInventoryPanelMouseHover(object sender, EventArgs e)
            {
                XtraPanel senderPanel = sender as XtraPanel;
                Trace.Assert(null != senderPanel);
                InventorySlot slot = senderPanel.Tag as InventorySlot;
                Trace.Assert(null != slot);
            }

            #endregion

        #endregion

        #region Private chat related

        public void PrivateChatMsgRecv(string msg)
        {
            AddPrivateChatLine(msg, _clrIncomingMessage);
        }

        private void Enter_Click(object sender, EventArgs e)
        {
            if (!tePrivateChat.EditorContainsFocus)
                return;
            StaticReference.ClientCore.SendPrivateChat(CurrentUser.SocketID, tePrivateChat.Text);
            AddPrivateChatLine(tePrivateChat.Text, _clrOutgoingMessage);
            tePrivateChat.Text = "";
        }

        private void AddPrivateChatLine_Italic(string text, Color color)
        {
            rtbPrivateMessages.SelectionFont = new Font("Verdana", 7.11f, FontStyle.Italic);
            rtbPrivateMessages.SelectionColor = color;
            rtbPrivateMessages.SelectedText = text + "\n";
            rtbPrivateMessages.SelectionColor = Color.White;
            rtbPrivateMessages.ScrollToCaret();
        }

        private void AddPrivateChatLine(string text, Color color)
        {
            rtbPrivateMessages.SelectionFont = new Font("Verdana", 8.11f, FontStyle.Regular);
            rtbPrivateMessages.SelectionColor = color;
            rtbPrivateMessages.SelectedText = $"{DateTime.Now.ToLongTimeString()}    {text}{"\n"}"; rtbPrivateMessages.SelectionColor = Color.White;
            rtbPrivateMessages.ScrollToCaret();
        }

        #endregion

        #region Packet handling

        internal void HandleTracedCharacterPremiumChange(Packet receivedPacket)
        {
            //throw new NotImplementedException();
            Trace.TraceInformation("PREMIUM CHANGE");
            CurrentUser.PremiumChange(receivedPacket);
            RefreshTracedUserPremiumInformation();
       
        }

        public void HandleTracedCharacterItemMoveResult(Packet receivedPacket)
        {
            byte result = receivedPacket.Read<byte>();
            if (result == 0x02)
                return;
            byte bSrcPos = receivedPacket.Read<byte>();
            byte bDstPos = receivedPacket.Read<byte>();
            CurrentUser.DoSlotExchange(bSrcPos, bDstPos);
        }



        public void HandleTracedCharacterSaveInventoryResponse(Packet receivedPacket)
        {
            byte result = receivedPacket.Read<byte>();
            switch (result)
            {
                case 0x01:

                    pbInventory.Enabled = true;
                    CommonReference.ShowInformation(this, "Inventory is successfully saved.");
                    break;
                case 0x02:

                    /* Restore backup */
            for (int i = 0; i < 72; i++)
                    {
                        _inventoryCopy[i].CopyTo(CurrentUser.Inventory[i]);
                    }
                    /* Render the inventory */
                    RenderInventory();
                    pbInventory.Enabled = true;
                    CommonReference.ShowWarning(this, "Inventory save operation failed.\nPlayer does not exist!");
                    break;
                case 0x03:

                    /* Restore backup */
                    for (int i = 0; i < 72; i++)
                    {
                        _inventoryCopy[i].CopyTo(CurrentUser.Inventory[i]);
                    }
                    /* Render the inventory */
                    RenderInventory();
                    pbInventory.Enabled = true;
                    CommonReference.ShowWarning(this, "Inventory save operation failed.\nInternal inventory error!");
                    break;
            }
        }

       

        internal void HandleTracedCharacterWarp(Packet receivedPacket)
        {

            CurrentUser.Warp(receivedPacket);
            Invoke(new Action(RefreshTracedCharacterCoordinates));
        }

        internal void HandleTracedCharacterTargetHP(Packet receivedPacket)
        {
            CurrentUser.TargetHP(receivedPacket);
        }

        internal void HandleTracedCharacterItemMove(Packet receivedPacket)
        {
            CurrentUser.ItemMove(receivedPacket);
            Invoke(new Action(RefreshTracedCharacterInfo));
        }

        internal void HandleTracedCharacterLevelChange(Packet receivedPacket)
        {
            CurrentUser.LevelChange(receivedPacket);
            Invoke(new Action(RefreshTracedCharacterInfo));
        }

        internal void HandleTracedCharacterLoyaltyChange(Packet receivedPacket)
        {
            CurrentUser.LoyaltyChange(receivedPacket);
            Invoke(new Action(RefreshTracedUserLoyalty));
        }

        internal void HandleTracedCharacterMspChange(Packet receivedPacket)
        {
            CurrentUser.MpChange(receivedPacket);
            Invoke(new Action(RefreshTracedUserHealthAndMana));
        }

        internal void HandleTracedCharacterHpChange(Packet receivedPacket)
        {
            CurrentUser.HpChange(receivedPacket);
            Invoke(new Action(RefreshTracedUserHealthAndMana));
        }

        internal void HandleTracedCharacterGoldChange(Packet receivedPacket)
        {
            CurrentUser.GoldChange(receivedPacket);
            Invoke(new Action(RefreshTracedUserGold));
        }

        internal void HandleTracedCharacterExpChange(Packet receivedPacket)
        {
            CurrentUser.ExpChange(receivedPacket);
            Invoke(new Action(RefreshTracedCharacterExperience));
        }

        internal void HandleTracedCharacterMove(Packet receivedPacket)
        {
            CurrentUser.Move(receivedPacket);
            Invoke(new Action(RefreshTracedCharacterCoordinates));
        }


        public void HandleTracedCharacterSkillPointChange(Packet receivedPacket)
        {
            switch (receivedPacket.Read<byte>())
            {
                case 5:
                    CurrentUser.Skills.Profession1 = receivedPacket.Read<byte>();
                    break;
                case 6:
                    CurrentUser.Skills.Profession2 = receivedPacket.Read<byte>();
                    break;
                case 7:
                    CurrentUser.Skills.Profession3 = receivedPacket.Read<byte>();
                    break;
                case 8:
                    CurrentUser.Skills.Master = receivedPacket.Read<byte>();
                    break;
            }
            RefreshTracedUserSkills();
        }

        public void HandleTracedCharacterStatPointChange(Packet receivedPacket)
        {
            switch (receivedPacket.Read<byte>() - 1)
            {
                case 0:
                    CurrentUser.Stats.UpdateSTR(receivedPacket.Read<ushort>(), CurrentUser.Stats.STRB);
                    break;
                case 1:
                    CurrentUser.Stats.UpdateHP(receivedPacket.Read<ushort>(), CurrentUser.Stats.HPB);
                    break;
                case 2:
                    CurrentUser.Stats.UpdateDEX(receivedPacket.Read<ushort>(), CurrentUser.Stats.DEXB);
                    break;
                case 3:
                    CurrentUser.Stats.UpdateINT(receivedPacket.Read<ushort>(), CurrentUser.Stats.INTB);
                    break;
                case 4:
                    CurrentUser.Stats.UpdateMP(receivedPacket.Read<ushort>(), CurrentUser.Stats.MPB);
                    break;

            }
            RefreshTracedUserStats();
            // throw new NotImplementedException();
        }

        public void HandleTracedCharacterItemCountChange(Packet receivedPacket)
        {
            CurrentUser.ItemCountChange(receivedPacket);
        }

        public void HandleTracedCharacterItemGet(Packet receivedPacket)
        {
            CurrentUser.ItemGet(receivedPacket);
            // throw new NotImplementedException();
        }

        public void HandleTracedCharacterItemBuySell(Packet receivedPacket)
        {
            CurrentUser.ItemBuySell(receivedPacket);
        }

        #endregion

        #region Refresh UI

        private void CurrentUser_OnSlotChanged(byte pos)
        {
            pbInventory.SuspendLayout();
            var slot = CurrentUser.Inventory[pos];
            Image img = null;
            if (!slot.isEmpty())
            {
                img = slot.Info == null
                    ? CommonReference.GetIconByIconID((uint)0)
                    : CommonReference.GetIconByIconID((uint)slot.Info.IconID);

            }

            if (img != null)
            {
                switch ((ItemFlag)slot.Flag)
                {
                    case ItemFlag.ITEM_FLAG_SEALED:
                        img = ((Bitmap)img).ColorTint(32 / 100.0f, 0, 0);
                        break;
                    case ItemFlag.ITEM_FLAG_DUPLICATE:
                        img = ((Bitmap)img).ColorTint(0 / 100.0f, 30 / 100.0f, 50 / 100.0f);
                        break;
                }
            }
            if (pos < 0)
                return;

            if (pos < 14)
            {
                _equipments[pos].Tag = slot;
                _equipments[pos].BackgroundImage = img;
                _equipments[pos].Refresh();
            }
            else if (pos >= 14 && pos < 42)
            {
                _inventory[pos - 14].Tag = slot;
                _inventory[pos - 14].BackgroundImage = img;
                _inventory[pos - 14].Refresh();
            }
            else if (pos >= 42 && pos <= 47)
            {
                /* Cosplay */
                _cosplay[pos - 42].Tag = slot;
                _cosplay[pos - 42].BackgroundImage = img;
                _cosplay[pos - 42].Refresh();
            }

            if (pos >= 48 && pos <= 59 && _currentBagIndex == 1)
            {
                _magicBag[pos - 48].Tag = slot;
                _magicBag[pos - 48].BackgroundImage = img;
                _magicBag[pos - 48].Refresh();
            }
            else if (pos >= 60 && pos <= 71 && _currentBagIndex == 2)
            {
                _magicBag[pos - 60].Tag = slot;
                _magicBag[pos - 60].BackgroundImage = img;
                _magicBag[pos - 60].Refresh();
            }
            /* Ankamall evriveyır avm. */
            /* Only inventory supported */

            pbInventory.ResumeLayout();
        }

        private void RefreshTracedUserGold()
        {
            if (CurrentUser == null)
                return;
            tbGold.Text = $"{CurrentUser.Gold}";
        }

        private void RefreshTracedUserLoyalty()
        {
            if (CurrentUser == null)
                return;
            tbNationalPoints.Text = $"{CurrentUser.NationalPoints}/{CurrentUser.LeaderPoints}";
        }

        private void RefreshTracedCharacterExperience()
        {
            if (CurrentUser == null)
                return;
            tbExperience.Text = $"{CurrentUser.Experience.Current}/{CurrentUser.Experience.Maximum}";
            UpdateExperienceBar((int)CurrentUser.Experience.Current, (int)CurrentUser.Experience.Maximum);
        }
        private void RefreshTracedCharacterCoordinates()
        {
            if (CurrentUser == null)
                return;
            tbPosX.Text = $"{CurrentUser.PosX}";
            tbPosY.Text = $"{CurrentUser.PosY}";
            tbPosZ.Text = $"{CurrentUser.PosZ}";
        }

        internal void RefreshTracedCharacterInfo()
        {
            if (CurrentUser == null)
                return;
            tbAccountID.Text = $"{CurrentUser.AccountID}";
            tbCharacterID.Text = $"{CurrentUser.CharacterID}";

            tbZone.Text = $"{CommonReference.GetZone(CurrentUser.Zone).Value}";
            tbPosX.Text = $"{CurrentUser.PosX}";
            tbPosZ.Text = $"{CurrentUser.PosZ}";

            tbNation.Text = $"{Nation.GetDescription(CurrentUser.Nation)}";
            tbRace.Text = $"{Race.GetDescription(CurrentUser.Race)}";
            tbClass.Text = $"{CharacterClass.GetDescription(CurrentUser.Class)}";
            tbClanName.Text = $"{CurrentUser.ClanName}";
            tbLevel.Text = "LV." + $"{CurrentUser.Level}";
            tbExperience.Text = $"{CurrentUser.Experience.Current}/{CurrentUser.Experience.Maximum}";
            tbNationalPoints.Text = $"{CurrentUser.NationalPoints}/{CurrentUser.LeaderPoints}";
            tbGold.Text = $"{CurrentUser.Gold}";
            tbAuthority.Text = $"{Authority.GetDescription(CurrentUser.Authority)}";

            RefreshTracedUserStats();
            RefreshTracedUserSkills();
            RefreshTracedUserHealthAndMana();
            RefreshTracedUserResistances();
            UpdateTracedUserStatusValues();
            RefreshTracedUserPremiumInformation();

            // Main form navbar element
            _myParent.Invoke(new Action(() =>
            {
              
                foreach (Control ni in _myParent.npTrackedCharacters.Controls)
                {
                    if (ni is HyperlinkLabelControl)
                    {
                        if (Convert.ToInt16(ni.Tag) == CurrentUser.SocketID)
                            return;
                    }
                }
                HyperlinkLabelControl link = new HyperlinkLabelControl();
                link.Text = $"{CurrentUser.AccountID}:{CurrentUser.CharacterID}";
                link.Tag = CurrentUser.SocketID;
                link.HyperlinkClick += _myParent.trackedCharacterLink_HyperlinkClick;
                _myParent.npTrackedCharacters.Controls.Add(link);
            }));

        }

        public void RefreshZoneInformation()
        {
            tbZone.Text = $"{CommonReference.GetZone(CurrentUser.Zone).Value}";
        }

        private void RefreshTracedUserHealthAndMana()
        {
            UpdateHealthBar((int)CurrentUser.Health.Current, (int)CurrentUser.Health.Maximum);
            UpdateManaBar((int)CurrentUser.Mana.Current, (int)CurrentUser.Mana.Maximum);
        }

        private void RefreshTracedUserPremiumInformation()
        {
            string premiumLabel = "NONE";
            switch(CurrentUser.PremiumType)
            {
                case 0:
                    premiumLabel = "NONE";
                    break;
                case 1:
                    premiumLabel = "DISCOUNT PREMIUM";
                    break;
                case 2:
                    premiumLabel = "EXP PREMIUM";
                    break;
                case 3:
                    premiumLabel = "BRONZE PREMIUM";
                    break;
                case 4:
                    premiumLabel = "SILVER PREMIUM";
                    break;
                case 5:
                    premiumLabel = "GOLD PREMIUM";
                    break;
             
                case 6:
                    premiumLabel = "WAR PREMIUM";
                    break;
                case 7:
                    premiumLabel = "PLATINUM PREMIUM";
                    break;
                case 8:
                    premiumLabel = "ROYAL PREMIUM";
                    break;
            }

            tbPremiumType.Text = premiumLabel;

            DateTime start = CommonReference.UnixTimeStampToDateTime(CurrentUser.PremiumStart);
            DateTime end = CommonReference.UnixTimeStampToDateTime(CurrentUser.PremiumEnd);
            var rmtime = end - start;

            tbPremiumStart.Text = start.ToShortDateString() + " " + start.ToShortTimeString();
            tbPremiumEnd.Text = end.ToShortDateString() + " " + end.ToShortTimeString();
           tbPremiumRem.Text = $"{rmtime.Days} day(s) {rmtime.Hours} hour(s) {rmtime.Minutes} minute(s)";
        }
        private void RefreshTracedUserSkills()
        {
            if (CurrentUser == null)
                return;
            tbSkillPro1.Text = $"{CurrentUser.Skills.Profession1}";
            tbSkillPro2.Text = $"{CurrentUser.Skills.Profession2}";
            tbSkillPro3.Text = $"{CurrentUser.Skills.Profession3}";
            tbSkillMaster.Text = $"{CurrentUser.Skills.Master}";
        }

        private void RefreshTracedUserResistances()
        {
            if (CurrentUser == null)
                return;
            tbResFlame.Text = $"{CurrentUser.Resistances.Flame}";
            tbResGlacier.Text = $"{CurrentUser.Resistances.Glacier}";
            tbResLightning.Text = $"{CurrentUser.Resistances.Lightning}";
            tbResMagic.Text = $"{CurrentUser.Resistances.Magic}";
            tbResPoison.Text = $"{CurrentUser.Resistances.Poison}";
            tbResCurse.Text = $"{CurrentUser.Resistances.Curse}";
        }

        private void RefreshTracedUserStats()
        {
            if (CurrentUser == null)
                return;
            tbSTR.Text = $"{CurrentUser.Stats.STR} (+{CurrentUser.Stats.STRB})";
            tbDEX.Text = $"{CurrentUser.Stats.DEX} (+{CurrentUser.Stats.DEXB})";
            tbHP.Text = $"{CurrentUser.Stats.HP} (+{CurrentUser.Stats.HPB})";
            tbMP.Text = $"{CurrentUser.Stats.MP} (+{CurrentUser.Stats.MPB})";
            tbINT.Text = $"{CurrentUser.Stats.INT} (+{CurrentUser.Stats.INTB})";
        }

        private void UpdateHealthBar(int current, int maxHP)
        {
            healthBar.Properties.Maximum = maxHP;
            healthBar.Properties.Minimum = 0;
            healthBar.Position = current;
          //  healthBar.Properties.DisplayFormat.FormatString = $"{current} / {maxHP}";
            

        }

        private void UpdateManaBar(int current, int maxMP)
        {
            manaBar.Properties.Maximum = maxMP;
            manaBar.Properties.Minimum = 0;
            manaBar.Position = current;
           // manaBar.Properties.DisplayFormat.FormatString = $"{current} / {maxMP}";
        }

        private void UpdateExperienceBar(int current, int maxExp)
        {
            expBar.Properties.Maximum = maxExp;
            expBar.Properties.Minimum = 0;
            expBar.Position = current;
        }

        private void SetFlagLabel(LabelControl control, bool status)
        {
            if (status)
            {
                control.BackColor = Color.LimeGreen;
                control.ForeColor = Color.White;
            }
            else
            {
                control.BackColor = Color.IndianRed;
                control.ForeColor = Color.White;
            }
        }


        private void UpdateTracedUserStatusValues()
        {
            SetFlagLabel(lblAlive, CurrentUser.CheckFlagSet(StatusFlags.ALIVE));
            SetFlagLabel(lblSitting, CurrentUser.CheckFlagSet(StatusFlags.SITTING));
            SetFlagLabel(lblMerchanting, CurrentUser.CheckFlagSet(StatusFlags.MERCHANT));
            SetFlagLabel(lblInvisible, CurrentUser.CheckFlagSet(StatusFlags.INVIS));
            SetFlagLabel(lblMuted, CurrentUser.CheckFlagSet(StatusFlags.MUTED));
            SetFlagLabel(lblTransformed, CurrentUser.CheckFlagSet(StatusFlags.TRANSFORM));
            SetFlagLabel(lblMoving, CurrentUser.CheckFlagSet(StatusFlags.MOVE));
            SetFlagLabel(lblBlinking, CurrentUser.CheckFlagSet(StatusFlags.BLINK));
            SetFlagLabel(lblCanAttack, CurrentUser.CheckFlagSet(StatusFlags.ATTACK));
            SetFlagLabel(lblTrading, CurrentUser.CheckFlagSet(StatusFlags.TRADE));
            SetFlagLabel(lblReported, CurrentUser.CheckFlagSet(StatusFlags.REPORT));
            SetFlagLabel(lblKing, CurrentUser.CheckFlagSet(StatusFlags.KING));
            SetFlagLabel(lblIngame, CurrentUser.CheckFlagSet(StatusFlags.IN_GAME));
            SetFlagLabel(lblUsingStore, CurrentUser.CheckFlagSet(StatusFlags.USING_STORE));
            SetFlagLabel(lblGamemaster, CurrentUser.CheckFlagSet(StatusFlags.GAMEMASTER));
            SetFlagLabel(lblStealthDetect, CurrentUser.CheckFlagSet(StatusFlags.STEALTH_DETECTION));
            SetFlagLabel(lblPmBlocked, CurrentUser.CheckFlagSet(StatusFlags.PM_BLOCK));
            SetFlagLabel(lblClanMember, CurrentUser.CheckFlagSet(StatusFlags.CLAN));
            SetFlagLabel(lblClanLeader, CurrentUser.CheckFlagSet(StatusFlags.CLAN_LEADER));
            SetFlagLabel(lblClanAssistant, CurrentUser.CheckFlagSet(StatusFlags.CLAN_ASSIST));
            SetFlagLabel(lblPartyMember, CurrentUser.CheckFlagSet(StatusFlags.PARTY));
            SetFlagLabel(lblPartyLeader, CurrentUser.CheckFlagSet(StatusFlags.PARTY_LEADER));
            SetFlagLabel(lblSeekingParty, CurrentUser.CheckFlagSet(StatusFlags.PARTY_SEEKING));
            /*  
            UNUSED_1 = 16777216,
            UNUSED_2 = 33554432,
            UNUSED_3 = 67108864,
            UNUSED_4 = 134217728,
            UNUSED_5 = 268435456,
            UNUSED_6 = 536870912,
            UNUSED_7 = 1073741824,*/
        }

        #endregion

        #region Control events

        private void navBarItem6_LinkPressed(object sender, NavBarLinkEventArgs e)
        {
            var result = CommonReference.ShowQuestion(this,
                "ATTENTION : \n" +
                "Caveat 1 : Edit mode copies the current state of inventory internally.\n" +
                "Caveat 2 : If you save changes you've made, changes made by user during the edit period will be discarded.\n" +
                "Caveat 3 : If you discard the changes, internal copy will be restored and no changes will be made.\n" +
                "Do you want to continue?");

            /* Change inventory read only flag */
            SetInventoryReadOnly(result == DialogResult.No);
        }

        private void ngOptions_DisableEdit_LinkPressed(object sender, NavBarLinkEventArgs e)
        {
            var result = CommonReference.ShowQuestion(this, "Do you want to commit the changes you've made?");
            if (result == DialogResult.Yes)
            {
                // Send changes to the server
                StaticReference.ClientCore.SendCharacterSaveInventoryRequest(CurrentUser.SocketID, CurrentUser.Inventory);
                // Now, we wait.. :D
                pbInventory.Enabled = false;
            }
            else
            {
                /* Restore backup */
                for (int i = 0; i < 72; i++)
                {
                    _inventoryCopy[i].CopyTo(CurrentUser.Inventory[i]);
                }
                /* Render the inventory */
                RenderInventory();
            }
            /* Change inventory read only flag */
            SetInventoryReadOnly(true);
        }

        private void chbBag1_CheckedChanged(object sender, EventArgs e)
        {
            if (chbBag1.Checked)
            {
                _currentBagIndex = 1;
                chbBag2.Checked = false;
                RenderBagSlots();
            }
        }

        private void chbBag2_CheckedChanged(object sender, EventArgs e)
        {
            if (chbBag2.Checked)
            {
                _currentBagIndex = 2;
                chbBag1.Checked = false;
                RenderBagSlots();
            }
        }

        private void rtfItemData_TextChanged(object sender, EventArgs e)
        {

        }

        private void ngAuthority_EnableAttack_LinkPressed(object sender, NavBarLinkEventArgs e)
        {
            var dr = CommonReference.ShowQuestion(this,"Are you sure you want to give this user's attack ability back?");
            if (dr == DialogResult.Yes)
            {
                StaticReference.ClientCore.SendOperatorRequest(CurrentUser.SocketID, (byte)OperatorRequests.ATTACKENABLE);
            }
        }

        private void ngAuthority_DisableAttack_LinkPressed(object sender, NavBarLinkEventArgs e)
        {
            var dr = CommonReference.ShowQuestion(this,
                "Are you sure you want to take away attack ability of this user?");
            if (dr == DialogResult.Yes)
            {
                StaticReference.ClientCore.SendOperatorRequest(CurrentUser.SocketID, (byte)OperatorRequests.ATTACKDISABLE);
            }
        }

        private void ngAuthority_Mute_LinkPressed(object sender, NavBarLinkEventArgs e)
        {
            var dr = CommonReference.ShowQuestion(this,
             "Are you sure you want to mute this character?");
            if (dr == DialogResult.Yes)
            {
                StaticReference.ClientCore.SendOperatorRequest(CurrentUser.SocketID, (byte)OperatorRequests.MUTE);
            }
        }

        private void ngAuthority_Unmute_LinkPressed(object sender, NavBarLinkEventArgs e)
        {
            var dr = CommonReference.ShowQuestion(this,
             "Are you sure you want to unmute this character?");
            if (dr == DialogResult.Yes)
            {
                StaticReference.ClientCore.SendOperatorRequest(CurrentUser.SocketID, (byte)OperatorRequests.UNMUTE);
            }
        }

        private void ngAuthority_Disconnect_LinkPressed(object sender, NavBarLinkEventArgs e)
        {
            var dr = CommonReference.ShowQuestion(this,
                "Are you sure you want to disconnect this character from the server?");
            if (dr == DialogResult.Yes)
            {
                StaticReference.ClientCore.SendOperatorRequest(CurrentUser.SocketID, (byte)OperatorRequests.DISCONNECT);
            }
        }

        private void ngAuthority_Ban_LinkPressed(object sender, NavBarLinkEventArgs e)
        {
            var dr = CommonReference.ShowQuestion(this,
             "Are you sure you want to ban this character?");
            if (dr == DialogResult.Yes)
            {
                StaticReference.ClientCore.SendOperatorRequest(CurrentUser.SocketID, (byte)OperatorRequests.BAN);
            }
        }

        private void btnGiveGold_Click(object sender, EventArgs e)
        {
            StaticReference.ClientCore.SendValueAlteration(CurrentUser.SocketID, 0x01, 0x01, (uint)seGoldAmount.Value);
        }

        private void btnTakeGold_Click(object sender, EventArgs e)
        {
            StaticReference.ClientCore.SendValueAlteration(CurrentUser.SocketID, 0x01, 0x02, (uint)seGoldAmount.Value);
        }

        private void btnGiveExperience_Click(object sender, EventArgs e)
        {
            StaticReference.ClientCore.SendValueAlteration(CurrentUser.SocketID, 0x02, 0x01, (uint)seExperienceAmount.Value);
        }

        private void btnTakeExperience_Click(object sender, EventArgs e)
        {
            StaticReference.ClientCore.SendValueAlteration(CurrentUser.SocketID, 0x02, 0x02, (uint)seExperienceAmount.Value);
        }

        private void btnGiveLoyalty_Click(object sender, EventArgs e)
        {
            StaticReference.ClientCore.SendValueAlteration(CurrentUser.SocketID, 0x03, 0x01, (uint)seLoyaltyAmount.Value);
        }

        private void btnTakeLoyalty_Click(object sender, EventArgs e)
        {
            StaticReference.ClientCore.SendValueAlteration(CurrentUser.SocketID, 0x03, 0x02, (uint)seLoyaltyAmount.Value);
        }


        private void OnButtonStatEditClick(object sender, EventArgs e)
        {
            var btnSender = sender as SimpleButton;
            Trace.Assert(btnSender != null);
            /*
	STAT_STR = 0,
	STAT_STA = 1,
	STAT_DEX = 2,
	STAT_INT = 3, 
	STAT_CHA = 4, // MP*/

            switch (btnSender.Name)
            {
                case "btnStrUp":
                    StaticReference.ClientCore.SendStatAlteration(CurrentUser.SocketID, 0, 1);
                    break;
                case "btnStrDown":
                    StaticReference.ClientCore.SendStatAlteration(CurrentUser.SocketID, 0, 0);
                    break;
                case "btnHpUp":
                    StaticReference.ClientCore.SendStatAlteration(CurrentUser.SocketID, 1, 1);
                    break;
                case "btnHpDown":
                    StaticReference.ClientCore.SendStatAlteration(CurrentUser.SocketID, 1, 0);
                    break;
                case "btnDexUp":
                    StaticReference.ClientCore.SendStatAlteration(CurrentUser.SocketID, 2, 1);
                    break;
                case "btnDexDown":
                    StaticReference.ClientCore.SendStatAlteration(CurrentUser.SocketID, 2, 0);
                    break;
                case "btnIntUp":
                    StaticReference.ClientCore.SendStatAlteration(CurrentUser.SocketID, 3, 1);
                    break;
                case "btnIntDown":
                    StaticReference.ClientCore.SendStatAlteration(CurrentUser.SocketID, 3, 0);
                    break;
                case "btnMpUp":
                    StaticReference.ClientCore.SendStatAlteration(CurrentUser.SocketID, 4, 1);
                    break;
                case "btnMpDown":
                    StaticReference.ClientCore.SendStatAlteration(CurrentUser.SocketID, 4, 0);
                    break;
                    /*case "btnUnspentStatUp":
                        break;
                    case "btnUnspentStatDown":
                        break;*/

            }
        }

        private void OnButtonSkillEditClick(object sender, EventArgs e)
        {
            var btnSender = sender as SimpleButton;
            Trace.Assert(btnSender != null);

            switch (btnSender.Name)
            {
                case "btnSkill1Up":
                    StaticReference.ClientCore.SendSkillAlteration(CurrentUser.SocketID, 5, 1);
                    break;
                case "btnSkill1Down":
                    StaticReference.ClientCore.SendSkillAlteration(CurrentUser.SocketID, 5, 0);
                    break;
                case "btnSkill2Up":
                    StaticReference.ClientCore.SendSkillAlteration(CurrentUser.SocketID, 6, 1);
                    break;
                case "btnSkill2Down":
                    StaticReference.ClientCore.SendSkillAlteration(CurrentUser.SocketID, 6, 0);
                    break;
                case "btnSkill3Up":
                    StaticReference.ClientCore.SendSkillAlteration(CurrentUser.SocketID, 7, 1);
                    break;
                case "btnSkill3Down":
                    StaticReference.ClientCore.SendSkillAlteration(CurrentUser.SocketID, 7, 0);

                    break;
                case "btnMasterUp":
                    StaticReference.ClientCore.SendSkillAlteration(CurrentUser.SocketID, 8, 1);
                    break;
                case "btnMasterDown":
                    StaticReference.ClientCore.SendSkillAlteration(CurrentUser.SocketID, 8, 0);
                    break;
                case "btnUnspentSkillUp":
                    break;
                case "btnUnspentSkillDown":
                    break;
            }
        }

        private void btnZoneChangeUser_Click(object sender, EventArgs e)
        {
            if (cbZones.SelectedIndex == -1)
            {
                CommonReference.ShowWarning(this, "Select a zone first.");
                return;
            }

            var zone = Convert.ToUInt16(Convert.ToString(cbZones.SelectedItem).Split(':')[1]);
            StaticReference.ClientCore.SendZoneChange(CurrentUser.SocketID, 0x01, zone);

        }

        private void btnZoneChangeParty_Click(object sender, EventArgs e)
        {
            if (cbZones.SelectedIndex == -1)
            {
                CommonReference.ShowWarning(this, "Select a zone first.");
                return;
            }

            var zone = Convert.ToUInt16(Convert.ToString(cbZones.SelectedItem).Split(':')[1]);
            StaticReference.ClientCore.SendZoneChange(CurrentUser.SocketID, 0x02, zone);
        }

        private void btnZoneChangeClan_Click(object sender, EventArgs e)
        {
            if (cbZones.SelectedIndex == -1)
            {
                CommonReference.ShowWarning(this, "Select a zone first.");
                return;
            }
            var zone = Convert.ToUInt16(Convert.ToString(cbZones.SelectedItem).Split(':')[1]);
            StaticReference.ClientCore.SendZoneChange(CurrentUser.SocketID, 0x03, zone);
        }

        #endregion

        #region Show item details

        private void RtfItemData_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            var richTextBox = (RichTextBox)sender;
            var currentPanel = richTextBox.Tag as XtraPanel;
            if (currentPanel == null)
                return;
            richTextBox.Width = e.NewRectangle.Width + 10;
            richTextBox.Height = e.NewRectangle.Height + 10;
            int yPos = 0 , xPos = 0;
            if (currentPanel.Location.Y + richTextBox.Height > pbInventory.Bottom)
            {
                yPos = pbInventory.Bottom - richTextBox.Height;
            }
            else
            {
                yPos = currentPanel.Location.Y;
            }

            if (currentPanel.Left - richTextBox.Width < pbInventory.Left)
            {
                xPos = currentPanel.Right;
            }
            else
            {
                xPos = currentPanel.Left - richTextBox.Width;
            }
            // richTextBox.ClientRectangle
            richTextBox.Location = new Point(xPos, yPos);

            //Cursor.Position

        }

        /// <summary>
        ///     Gets a normal color from hex color.
        /// </summary>
        /// <param name="hexColor"></param>
        /// <returns></returns>
        public Color HexStringToColour(string hexColor)
        {
            var regex = new Regex(@"[abcdefABCDEF\d]+", RegexOptions.Compiled);
            string str = hexColor.Where(ch => regex.IsMatch(ch.ToString())).Aggregate("", (current, ch) => current + ch);
            if (str.Length != 6)
                return Color.Empty;
            string s = str.Substring(0, 2);
            string str3 = str.Substring(2, 2);
            string str4 = str.Substring(4, 2);
            Color empty;
            try
            {
                int red = int.Parse(s, NumberStyles.HexNumber);
                int green = int.Parse(str3, NumberStyles.HexNumber);
                int blue = int.Parse(str4, NumberStyles.HexNumber);
                empty = Color.FromArgb(red, green, blue);
            }
            catch
            {
                return Color.Empty;
            }
            return empty;
        }

        /// <summary>
        ///     Adds text to item detail (with title option)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="isTitle"></param>
        private void AddText(string text, bool isTitle)
        {
            if (isTitle)
                rtfItemData.SelectionFont = new Font("Tahoma", 7.6f, FontStyle.Bold);
            rtfItemData.SelectionColor = _rtfColor;
            rtfItemData.SelectedText = text;
            rtfItemData.SelectionColor = Color.White;
        }

        /// <summary>
        ///     Adds text to item detail.
        /// </summary>
        /// <param name="text"></param>
        private void AddText(string text)
        {
            rtfItemData.SelectionFont = new Font("Tahoma", 7.11f, FontStyle.Regular);
            rtfItemData.SelectionColor = _rtfColor;
            rtfItemData.SelectedText = text;
            rtfItemData.SelectionColor = Color.White;
        }

        string RemoveBetween(string s, char begin, char end)
        {
            Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
            return regex.Replace(s, string.Empty);
        }


        /// <summary>
        ///     Shows an item's details on a rtf.
        /// </summary>
        /// <param name="num"></param>
        private void ShowItemDetails(InventorySlot slot)
        {
            rtfItemData.SuspendLayout();
            ItemInformation currentItem = slot.Info;

            rtfItemData.Clear();
            _rtfColor = Color.White;

            bool isTradeable = true;
            bool isUnique = false;
#pragma warning disable 219
            bool isSealed = false;
#pragma warning restore 219
            bool isReverse = false;


            rtfItemData.Clear();
            rtfItemData.SelectionAlignment = HorizontalAlignment.Center;

            if (currentItem == null)
            {
                AddText($"Item ID : {slot.ItemID}\n not found in TBL!");
                return;
            }

            #region Declare Color

            Color clrUpgrade = HexStringToColour("#c87cc7");
            Color clrMagic = HexStringToColour("#8080ff");
            Color clrRare = HexStringToColour("#ffff00");
            Color clrCraft = HexStringToColour("#80ff00");
            Color clrUnique = HexStringToColour("#DDC77C");
            Color clrReverse = HexStringToColour("#FF84A8");
            Color clrCospre = HexStringToColour("#06FCC9");
            Color clrNonTradeable = HexStringToColour("#922221");
            Color clrBonus = HexStringToColour("#80ff00");
#pragma warning disable 168
            Color clrEvent = HexStringToColour("#00FFCE");
#pragma warning restore 168
            Color clrReverseUnique = HexStringToColour("#FFA500");
            Color clrWhite = Color.White;

            #endregion

            _rtfColor = clrWhite;
            byte type = currentItem.ItemType;
            switch (type)
            {
                #region Parse Type

                case 0:
                    _rtfColor = clrWhite;
                    AddText(currentItem.Name.Trim(), true);
                    AddText("\n\n(Normal Item)\n");
                    break;

                case 1:
                    _rtfColor = clrMagic;
                    AddText(currentItem.Name.Trim(), true);
                    AddText("\n\n(Magic Item)\n");
                    break;

                case 2:
                    _rtfColor = clrRare;
                    AddText(currentItem.Name.Trim(), true);
                    AddText("\n\n(Rare Item)\n");
                    break;

                case 3:
                    _rtfColor = clrCraft;
                    AddText(currentItem.Name.Trim(), true);
                    AddText("\n\n(Craft Item)\n");
                    break;

                case 4:
                    _rtfColor = clrUnique;
                    AddText(currentItem.Name.Trim(), true);
                    AddText("\n\n(Unique Item)\n");
                    isUnique = true;
                    break;

                case 5:
                    _rtfColor = clrUpgrade;
                    AddText(currentItem.Name.Trim(), true);
                    AddText("\n\n(Upgrade Item)\n");
                    break;
                case 6:
                    _rtfColor = clrUpgrade;
                    AddText(currentItem.Name.Trim(), true);
                    AddText("\n\n(Event Item)\n");
                    break;
                case 8:
                    _rtfColor = clrCospre;
                    AddText(currentItem.Name.Trim(), true);
                    _rtfColor = clrWhite;
                    AddText("\n\n(Cospre)\n");
                    isTradeable = false;
                    break;
                case 11:
                    _rtfColor = clrReverse;
                    AddText(currentItem.Name.Trim(), true);
                    AddText("\n\n(Reverse Item)\n");
                    isReverse = true;
                    break;
                case 12:
                    _rtfColor = clrReverseUnique;
                    AddText(currentItem.Name.Trim(), true);
                    AddText("\n\n(Reverse Unique Item)\n");
                    isUnique = true;
                    isReverse = true;
                    break;
                default:
                    AddText(currentItem.Name.Trim() + "\n\n type = " + type);
                    break;

                    #endregion
            }
            if (slot.ItemID > 899999999) isTradeable = false;
            byte kind = currentItem.Kind;
            switch (kind)
            {
                #region Parse Kind

                case 0:
                    _rtfColor = clrWhite;
                    AddText("Reward");
                    break;
                case 11:
                    _rtfColor = clrWhite;
                    AddText("Dagger");
                    break;
                case 21:
                    _rtfColor = clrWhite;
                    AddText("One Handed Sword");
                    break;
                case 22:
                    _rtfColor = clrWhite;
                    AddText("Two Handed Sword");
                    break;
                case 31:
                    _rtfColor = clrWhite;
                    AddText("One Handed Axe");
                    break;
                case 32:
                    _rtfColor = clrWhite;
                    AddText("Two-Handed Axe");
                    break;
                case 41:
                    _rtfColor = clrWhite;
                    AddText("Club");
                    break;
                case 42:
                    _rtfColor = clrWhite;
                    AddText("Two Handed Club");
                    break;
                case 51:
                    _rtfColor = clrWhite;
                    AddText("Spear");
                    break;
                case 52:
                    _rtfColor = clrWhite;
                    AddText("Long Spear");
                    break;
                case 60:
                    _rtfColor = clrWhite;
                    AddText("Shield");
                    break;
                case 70:
                    _rtfColor = clrWhite;
                    AddText("Bow");
                    break;
                case 71:
                    _rtfColor = clrWhite;
                    AddText("Crossbow");
                    break;
                case 91:
                    _rtfColor = clrWhite;
                    AddText("Earring");
                    break;
                case 92:
                    _rtfColor = clrWhite;
                    AddText("Necklace");
                    break;
                case 93:
                    _rtfColor = clrWhite;
                    AddText("Ring");
                    break;
                case 94:
                    _rtfColor = clrWhite;
                    AddText("Belt");
                    break;
                case 95:
                    _rtfColor = clrWhite;
                    AddText("Quest Item");
                    break;
                case 97:
                    _rtfColor = clrWhite;
                    AddText("Lune Item");
                    break;
                case 98:
                    _rtfColor = clrWhite;
                    AddText("Upgrade Scroll");
                    break;
                case 101:
                    _rtfColor = clrWhite;
                    AddText("Monster's Stone");
                    break;
                case 110:
                    _rtfColor = clrWhite;
                    AddText("Staff");
                    break;
                case 120:
                    _rtfColor = clrWhite;
                    AddText("Arrow");
                    break;
                case 130:
                    _rtfColor = clrWhite;
                    AddText("Javelin");
                    break;
                case 150:
                    _rtfColor = clrWhite;
                    AddText("Familiar Egg");
                    break;
                case 151:
                case 152:
                    _rtfColor = clrWhite;
                    AddText("Familiar");
                    break;
                case 160:
                    _rtfColor = clrWhite;
                    AddText("Cypher Ring");
                    break;
                case 170:
                    _rtfColor = clrWhite;
                    AddText("Autoloot");
                    break;
                case 171:
                    _rtfColor = clrWhite;
                    AddText("Image Change Scroll");
                    break;
                case 172:
                    _rtfColor = clrWhite;
                    AddText("Familiar Attack");
                    break;
                case 173:
                    _rtfColor = clrWhite;
                    AddText("Familiar Defense");
                    break;
                case 174:
                    _rtfColor = clrWhite;
                    AddText("Familiar Loyalty");
                    break;
                case 175:
                    _rtfColor = clrWhite;
                    AddText("Familiar Speciality Food");
                    break;
                case 176:
                    _rtfColor = clrWhite;
                    AddText("Familiar  Food");
                    break;
                case 181:
                    _rtfColor = clrWhite;
                    AddText("Priest Mace");
                    break;
                case 210:
                    _rtfColor = clrWhite;
                    AddText("Warrior Armor");
                    break;
                case 220:
                    _rtfColor = clrWhite;
                    AddText("Rogue Armor");
                    break;
                case 230:
                    _rtfColor = clrWhite;
                    AddText("Mage Armor");
                    break;
                case 240:
                    _rtfColor = clrWhite;
                    AddText("Priest Armor");
                    break;
                case 250:
                    _rtfColor = clrWhite;
                    AddText("Seal Scroll");
                    break;
                case 253:
                    isSealed = true;
                    break;
                case 254:
                    _rtfColor = clrWhite;
                    AddText("Chaos Skill Item");
                    break;
                case 255:
                    _rtfColor = clrWhite;
                    AddText("Power Up Item");
                    break;

                    #endregion
            }
            byte cls = currentItem.Class;
            switch (cls)
            {
                case 6:
                    _rtfColor = clrWhite;
                    AddText("\nDefensive Warrior");
                    break;
            }

            #region Parse bonus(es)

            rtfItemData.SelectedText = "\n";
            _rtfColor = clrWhite;
            rtfItemData.SelectionAlignment = HorizontalAlignment.Left;
            if ((((currentItem.Kind >= 11) && (currentItem.Kind <= 0x47)) &&
                 (currentItem.Kind != 60)) || (currentItem.Kind == 110))
            {
                AddText("Attack Power : " + currentItem.Damage + "\n");
                AddText("Attack Speed : " +
                        ((currentItem.Delay < 110)
                            ? "Fast"
                            : ((currentItem.Delay > 150) ? "Very Slow" : "Slow")) + "\n");
                AddText("Effective Range : " + string.Format("{0:0.00}", currentItem.Range) + "\n");
            }
            if (currentItem.Weight > 0)
                AddText("Weight : " + string.Format("{0:0.00}", currentItem.Weight / 10) + "\n");
            if ((short)currentItem.Duration > 1)
                AddText("Max Durability : " + currentItem.Duration + "\n");
            if (currentItem.Hitrate != 0)
                AddText(string.Format("Increase Attack Power by {0}%\n", currentItem.Hitrate));
            if (currentItem.EvasionRate != 0)
                AddText(string.Format("Increase Evasion by {0}%\n", currentItem.EvasionRate));
            if (currentItem.DropRate != 0)
                AddText(string.Format("Increase Drop Rate by {0}%\n", currentItem.DropRate));
            if (currentItem.Ac != 0)
                AddText("Defense Ability : " + currentItem.Ac + "\n");
            if ((((((currentItem.FireDamage > 0) || (currentItem.IceDamage > 0)) ||
                   ((currentItem.LightningDamage > 0) || (currentItem.PoisonDamage > 0))) ||
                  (((currentItem.StrB > 0) || (currentItem.StaB > 0)) ||
                   ((currentItem.DexB > 0) || (currentItem.IntelB > 0)))) ||
                 ((((currentItem.ChaB > 0) || (currentItem.DaggerAc > 0)) ||
                   ((currentItem.SwordAc > 0) || (currentItem.MaceAc > 0))) ||
                  (((currentItem.AxeAc > 0) || (currentItem.SpearAc > 0)) ||
                   ((currentItem.BowAc > 0) || (currentItem.HpDrain > 0))))) ||
                ((((currentItem.MpDamage > 0) || (currentItem.MpDrain > 0)) ||
                  ((currentItem.MaxHpB > 0) || (currentItem.MaxMpB > 0))) ||
                 ((((currentItem.FireR > 0) || (currentItem.ColdR > 0)) ||
                   ((currentItem.LightningR > 0) || (currentItem.MagicR > 0))) ||
                  ((currentItem.PoisonR > 0) || (currentItem.CurseR > 0)))))
            {
                _rtfColor = clrBonus;
                if (currentItem.FireDamage > 0)
                    AddText("Fire Damage : " + currentItem.FireDamage + "\n");
                if (currentItem.IceDamage > 0)
                    AddText("Ice Damage : " + currentItem.IceDamage + "\n");
                if (currentItem.LightningDamage > 0)
                    AddText("Lightning Damage : " + currentItem.LightningDamage + "\n");
                if (currentItem.PoisonDamage > 0)
                    AddText("Poison Damage : " + currentItem.PoisonDamage + "\n");
                if (currentItem.DaggerAc > 0)
                    AddText("Defense Ability (Dagger) : " + currentItem.DaggerAc + "\n");
                if (currentItem.SwordAc > 0)
                    AddText("Defense Ability (Sword) : " + currentItem.SwordAc + "\n");
                if (currentItem.MaceAc > 0)
                    AddText("Defense Ability (Club) : " + currentItem.MaceAc + "\n");
                if (currentItem.AxeAc > 0)
                    AddText("Defense Ability (Ax): " + currentItem.AxeAc + "\n");
                if (currentItem.SpearAc > 0)
                    AddText("Defense Ability (Spear) : " + currentItem.SpearAc + "\n");
                if (currentItem.BowAc > 0)
                    AddText("Defense Ability (Bow) : " + currentItem.BowAc + "\n");
                if (currentItem.HpDrain > 0)
                    AddText("HP Absorbed : " + currentItem.HpDrain + "\n");
                if (currentItem.MpDamage > 0)
                    AddText("MP Damage : " + currentItem.MpDamage + "\n");
                if (currentItem.MpDrain > 0)
                    AddText("MP Absorbed : " + currentItem.MpDrain + "\n");
                if (currentItem.MaxHpB > 0)
                    AddText("HP Bonus : " + currentItem.MaxHpB + "\n");
                if (currentItem.MaxMpB > 0)
                    AddText("MP Bonus : " + currentItem.MaxMpB + "\n");
                if (currentItem.StrB > 0)
                    AddText("Strength Bonus : " + currentItem.StrB + "\n");
                if (currentItem.StaB > 0)
                    AddText("Health Bonus : " + currentItem.StaB + "\n");
                if (currentItem.DexB > 0)
                    AddText("Dexterity Bonus : " + currentItem.DexB + "\n");
                if (currentItem.IntelB > 0)
                    AddText("Intelligence Bonus : " + currentItem.IntelB + "\n");
                if (currentItem.ChaB > 0)
                    AddText("Magic Power Bonus : " + currentItem.ChaB + "\n");
                if (currentItem.FireR > 0)
                    AddText("Resistance to Flame : " + currentItem.FireR + "\n");
                if (currentItem.ColdR > 0)
                    AddText("Resistance to Glacier : " + currentItem.ColdR + "\n");
                if (currentItem.LightningR > 0)
                    AddText("Resistance to Lightning : " + currentItem.LightningR + "\n");
                if (currentItem.MagicR > 0)
                    AddText("Resistance to Magic : " + currentItem.MagicR + "\n");
                if (currentItem.PoisonR > 0)
                    AddText("Resistance to Poison : " + currentItem.PoisonR + "\n");
                if (currentItem.CurseR > 0)
                    AddText("Resistance to Curse : " + currentItem.CurseR + "\n");
                _rtfColor = Color.White;
            }
            if ((((byte)currentItem.ReqStr > 0) || ((byte)currentItem.ReqSta > 0)) ||
                ((((byte)currentItem.ReqDex > 0) || ((byte)currentItem.ReqIntel > 0)) ||
                 ((byte)currentItem.ReqCha > 0)))
            {
                if ((byte)currentItem.ReqStr > 0)
                    AddText("Required Strength : " + currentItem.ReqStr + "\n");
                if ((byte)currentItem.ReqSta > 0)
                    AddText("Required Health : " + currentItem.ReqSta + "\n");
                if ((byte)currentItem.ReqDex > 0)
                    AddText("Required Dexterity : " + currentItem.ReqDex + "\n");
                if ((byte)currentItem.ReqIntel > 0)
                    AddText("Required Intelligence : " + currentItem.ReqIntel + "\n");
                if ((byte)currentItem.ReqCha > 0)
                    AddText("Required Magic Power : " + currentItem.ReqCha + "\n");
            }

            #endregion

            _rtfColor = clrMagic;
            AddText($"Item ID : {slot.ItemID}\n");
          /*  if (slot.ExpirationTime > 0)
            {
                DateTime dt = DateTime.FromFileTime(slot.ExpirationTime);
            }*/
            if (isUnique && isReverse)
            {
                _rtfColor = clrBonus;
                AddText("Reverse unique item");
                goto skip;
            }
            if (isUnique)
            {
                _rtfColor = clrBonus;
                AddText("Unique");
            }
            if (isReverse)
            {
                _rtfColor = clrBonus;
                AddText("Item rank : Reverse");
            }
            skip:
            ;
            if (!isTradeable)
            {
                rtfItemData.SelectionAlignment = HorizontalAlignment.Center;
                _rtfColor = clrNonTradeable;
                AddText("\nCannot be traded or sold");
            }

            switch ((ItemFlag)slot.Flag)
            {
                case ItemFlag.ITEM_FLAG_RENTED:
                    rtfItemData.SelectionAlignment = HorizontalAlignment.Center;
                    _rtfColor = clrRare;
                    AddText("\nRented Item");
                    break;
                case ItemFlag.ITEM_FLAG_SEALED:
                    rtfItemData.SelectionAlignment = HorizontalAlignment.Center;
                    _rtfColor = clrRare;
                    AddText("\nSealed Item");
                    break;
                case ItemFlag.ITEM_FLAG_DUPLICATE:
                    rtfItemData.SelectionAlignment = HorizontalAlignment.Center;
                    _rtfColor = clrRare;
                    AddText("\nDuplicate Item");
                    break;
            }
            rtfItemData.SelectionAlignment = HorizontalAlignment.Center;
            _rtfColor = clrWhite;
            AddText($"\n*{RemoveBetween(slot.Info.Description.Replace('|', '\n'),'<','>')}*");
            if (slot.Serial == 0)
            {
                _rtfColor = clrNonTradeable;
                AddText($"\nSerial is zero!\n");
            }

            rtfItemData.ResumeLayout(true);
        }
        #endregion

        private void ctsChangeCount_Click(object sender, EventArgs e)
        {
            if (_isInventoryReadOnly)
            {
                CommonReference.ShowWarning(this, "Inventory is in read only mode.\nPlease enable edit mode first.");
                return;
            }
            var senderPanel = cmsInventory.SourceControl as XtraPanel;

            var senderSlot = senderPanel?.Tag as InventorySlot;
            if (senderSlot == null)
                return;

            if (senderSlot.Info.Countable == 0)
            {
                CommonReference.ShowInformation(this, "Selected item is not countable.");
                return;
            }

            var inpbx = new InputBox("Enter the new amount", senderSlot.Count);
            inpbx.OnDialogClosed += delegate(object o)
            {
                ushort newValue;
                if (ushort.TryParse(Convert.ToString(o), out newValue))
                {
                    senderSlot.Count = newValue;
                }
                else
                {
                    CommonReference.ShowWarning(this, "Either input contains non-numerical characters or the value exceeds the min-max range.");
                }
                
            };
            inpbx.Show();
            CurrentUser_OnSlotChanged(senderSlot.Pos);
        }

        private void ctsChangeSerial_Click(object sender, EventArgs e)
        {
            if (_isInventoryReadOnly)
            {
                CommonReference.ShowWarning(this, "Inventory is in read only mode.\nPlease enable edit mode first.");
                return;
            }
            var senderPanel = cmsInventory.SourceControl as XtraPanel;

            var senderSlot = senderPanel?.Tag as InventorySlot;
            if (senderSlot == null)
                return;

            

            var inpbx = new InputBox("Enter the new serial", senderSlot.Serial);
            inpbx.OnDialogClosed += delegate (object o)
            {
                ulong newValue;
                if (ulong.TryParse(Convert.ToString(o), out newValue))
                {
                    senderSlot.Serial = newValue;
                }
                else
                {
                    CommonReference.ShowWarning(this, "Either input contains non-numerical characters or the value exceeds the min-max range.");
                }

            };
            inpbx.Show();
            CurrentUser_OnSlotChanged(senderSlot.Pos);
        }

        private void ctsCopy_Click(object sender, EventArgs e)
        {
            if (_isInventoryReadOnly)
            {
                CommonReference.ShowWarning(this, "Inventory is in read only mode.\nPlease enable edit mode first.");
                return;
            }
            var senderPanel = cmsInventory.SourceControl as XtraPanel;

            var senderSlot = senderPanel?.Tag as InventorySlot;
            if (senderSlot == null)
                return;

            _copiedSlot = senderSlot;

        }

        private void ctsPaste_Click(object sender, EventArgs e)
        {
            if (_isInventoryReadOnly)
            {
                CommonReference.ShowWarning(this, "Inventory is in read only mode.\nPlease enable edit mode first.");
                return;
            }
            var senderPanel = cmsInventory.SourceControl as XtraPanel;

            var senderSlot = senderPanel?.Tag as InventorySlot;
            if (senderSlot == null)
                return;


            if (_copiedSlot == null)
            {
                CommonReference.ShowWarning(this, "There is no copied item.\nCopy an item from inventory first.");
                return;
            }
            if (!senderSlot.isEmpty())
            {
               var result = CommonReference.ShowQuestion(this,"There is an item exist in this slot.\nIf you continue, the existing item will be overwriten.\nDo you want to continue?");
                if (result == DialogResult.No)
                    return;
            }

            _copiedSlot.CopyTo(senderSlot);
            CurrentUser_OnSlotChanged(senderSlot.Pos);
        }

        private void ctsDelete_Click(object sender, EventArgs e)
        {
            if (_isInventoryReadOnly)
            {
                CommonReference.ShowWarning(this, "Inventory is in read only mode.\nPlease enable edit mode first.");
                return;
            }
            var senderPanel = cmsInventory.SourceControl as XtraPanel;

            var senderSlot = senderPanel?.Tag as InventorySlot;
            if (senderSlot == null)
                return;


            var result = CommonReference.ShowQuestion(this, "This will remove the item permanently.\nDo you want to continue?");
            if (result == DialogResult.No)
                return;

            /* Easy peasy.*/
            new InventorySlot().CopyTo(senderSlot);
            CurrentUser_OnSlotChanged(senderSlot.Pos);
        }

        private void ctsCopyToClipboardPlain_Click(object sender, EventArgs e)
        {
            var senderPanel = cmsInventory.SourceControl as XtraPanel;

            var senderSlot = senderPanel?.Tag as InventorySlot;
            if (senderSlot == null)
                return;
            Clipboard.SetText(senderSlot.ItemID.ToString());
        }

        private void ctsCopyToClipboardGiveItem_Click(object sender, EventArgs e)
        {
            var senderPanel = cmsInventory.SourceControl as XtraPanel;

            var senderSlot = senderPanel?.Tag as InventorySlot;
            if (senderSlot == null)
                return;
            Clipboard.SetText($"+give_item {senderSlot.ItemID}");
        }

        private void ngAuthority_MakeGM_LinkPressed(object sender, NavBarLinkEventArgs e)
        {
            // TODO : Make it only available to administrators
            var dr = CommonReference.ShowQuestion(this, "Are you sure you want to make this user game master?");
            if (dr == DialogResult.Yes)
            {
                StaticReference.ClientCore.SendOperatorRequest(CurrentUser.SocketID, (byte) OperatorRequests.MAKEGM);
            }
        }

        private void ngAuthority_Reset_LinkPressed(object sender, NavBarLinkEventArgs e)
        {
            var dr = CommonReference.ShowQuestion(this, "Are you sure you want to reset this user's authority?");
            if (dr == DialogResult.Yes)
            {
                StaticReference.ClientCore.SendOperatorRequest(CurrentUser.SocketID, (byte)OperatorRequests.UNMUTE);
            }
        }


        private void dePremiumStart_EditValueChanged(object sender, EventArgs e)
        {
            //CurrentUser.
            var result = dePremiumEnd.DateTime - DateTime.Now;
            lblPremiumRem.Text = $"{result.Days} day(s) {result.Hours} hour(s) {result.Minutes} minute(s) from now on.";
            
        }

        public void OnPremiumChangeResult(byte result)
        {
            btnUpdatePremium.Text = "Confirm";
            btnUpdatePremium.Enabled = true;
            if (result == 0x01)
            {
                CommonReference.ShowInformation(this, "Premium information successfully changed.");
            }
            else
                CommonReference.ShowWarning(this, "Premium information change failed!");
        }

        private void btnUpdatePremium_Click(object sender, EventArgs e)
        {
            if (cbPremiumType.SelectedIndex == -1)
            {
                CommonReference.ShowWarning(this, "Select a premium type.");
                return;
            }

            byte type = (byte)cbPremiumType.SelectedIndex;
            ulong start = (ulong)CommonReference.DateTimeToUnixTime(dePremiumStart.DateTime);
            ulong end = (ulong)CommonReference.DateTimeToUnixTime(dePremiumEnd.DateTime);

            StaticReference.ClientCore.SendPremiumChangeReq(CurrentUser.AccountID, type, start, end);
            btnUpdatePremium.Text = "Processing..";
            btnUpdatePremium.Enabled = false;
        }

        private void dePremiumEnd_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if((DateTime)e.NewValue < dePremiumStart.DateTime)
            {
                CommonReference.ShowWarning(this, "Expiration date cannot be less than starting date.");
                e.Cancel = true;
                return;
            }
        }

        private void dePremiumStart_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
           
            if (dePremiumEnd.DateTime < (DateTime)e.NewValue)
            {
                CommonReference.ShowWarning(this, "Starting date cannot be higher than expiration date.");
                e.Cancel = true;
                return;
            }
        }

        private void btnGiveManner_Click(object sender, EventArgs e)
        {
            StaticReference.ClientCore.SendValueAlteration(CurrentUser.SocketID, 0x04, 0x01, (uint)seMannerAmount.Value);
        }

        private void btnTakeManner_Click(object sender, EventArgs e)
        {
            StaticReference.ClientCore.SendValueAlteration(CurrentUser.SocketID, 0x04, 0x00, (uint)seMannerAmount.Value);
        }

        private void accordionControl1_Click(object sender, EventArgs e)
        {

        }

        private void ngAuthority_btnDisconnect_Click(object sender, EventArgs e)
        {
           
        }

        private void ngAuthority_btnSendPrison_Click(object sender, EventArgs e)
        {
            using (frmPunishPlayer fpp = new frmPunishPlayer(CurrentUser, AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_PRISON))
                fpp.ShowDialog();
        }

        private void ngAuthority_btnBan_Click(object sender, EventArgs e)
        {
            using (frmPunishPlayer fpp = new frmPunishPlayer(CurrentUser, AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_BAN))
                fpp.ShowDialog();
        }

        private void ngAuthority_btnMute_Click(object sender, EventArgs e)
        {
            using (frmPunishPlayer fpp = new frmPunishPlayer(CurrentUser, AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_MUTE))
                fpp.ShowDialog();
        }

        private void ngAuthority_btnTradeBlock_Click(object sender, EventArgs e)
        {
            using (frmPunishPlayer fpp = new frmPunishPlayer(CurrentUser, AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_TRADE))
                fpp.ShowDialog();
        }

        private void ngAuthority_btnMerchantBlock_Click(object sender, EventArgs e)
        {
            using (frmPunishPlayer fpp = new frmPunishPlayer(CurrentUser, AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_MERCHANT))
                fpp.ShowDialog();
        }

        private void ngAuthority_btnAttackBlock_Click(object sender, EventArgs e)
        {
            using (frmPunishPlayer fpp = new frmPunishPlayer(CurrentUser, AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_ATTACK))
                fpp.ShowDialog();
        }

        private void ngAuthority_btnLetterBlock_Click(object sender, EventArgs e)
        {
            using (frmPunishPlayer fpp = new frmPunishPlayer(CurrentUser, AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_LETTER))
                fpp.ShowDialog();
        }

        private void ngAuthority_btnZoneChangeBlock_Click(object sender, EventArgs e)
        {
            using (frmPunishPlayer fpp = new frmPunishPlayer(CurrentUser, AccountPenalty.AccountPenaltyType.ACCOUNT_PENALTY_ZONE_CHANGE))
                fpp.ShowDialog();
        }
    }
}