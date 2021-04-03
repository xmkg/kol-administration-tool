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
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using KAI.Declarations;
using KAI.Common.Declarations;

namespace KAI.ISEngine
{
    public partial class frmSearchItem : XtraForm
    {
        readonly AutoCompleteStringCollection AutoCompleteSuggestionList = 
        new AutoCompleteStringCollection { "Shard", "Sherion", "Raptor" };
        private Kind currentKind = Kind.Any;
        private ItemType currentType = ItemType.Any;
        private readonly List<ItemInformation> SearchResults = new List<ItemInformation>();
        private readonly List<XtraPanel> SearchResultPanels = new List<XtraPanel>();
        readonly Font _countFont = new Font("Verdana", 8.00F);
        private Color _rtfColor;
        private readonly Button _enter = new Button();
        private static int RESULT_PANEL_SLOT_COUNT = 32;
        private static int RESULT_PANEL_COUNT = 2;
        private static int SEARCH_RESULTS_PER_PAGE = RESULT_PANEL_SLOT_COUNT * RESULT_PANEL_COUNT;
        private int CURRENT_RESULT_PAGE = 0; // depends..
        private int MAX_RESULT_PAGE = 0;
        public frmSearchItem()
        {
            
            InitializeComponent();
            AcceptButton = _enter;
            _enter.Click += Enter_Click;
            rtfItemData.Parent = this;rtfItemData.WordWrap = false;
            rtfItemData.ContentsResized += RtfItemData_ContentsResized;
            
        }

        

        private void Enter_Click(object sender, EventArgs e)
        {
            btnSearchItem_Click(sender, e);
        }

        private void LoadAutoSuggestionList()
        {
            var itemOrgTable = CommonReference.TableSet.Tables["item_org_us.tbl"];
            if (itemOrgTable == null)
            {
                CommonReference.ShowWarning(this,
                    "Cannot run search operation, because of missing table (item_org_us.tbl)");
                return;
            }
            foreach (DataRow itemOrgRow in itemOrgTable.Rows)
            {
                string baseItemName = Convert.ToString(itemOrgRow[2]);
                int baseItemID = Convert.ToInt32(itemOrgRow[0]);
                byte extTableID = Convert.ToByte(itemOrgRow[1]);

                if (!AutoCompleteSuggestionList.Contains(NormalizeItemName(baseItemName)))
                    AutoCompleteSuggestionList.Add(NormalizeItemName(baseItemName));

                if (baseItemName.ToLowerInvariant().Contains(teItemName.Text.ToLowerInvariant()))
                {
                    DataTable extensionTable = CommonReference.TableSet.Tables[$"item_ext_{extTableID}_us.tbl"];
                    if (extensionTable == null)
                        continue;
                    foreach (DataRow extensionRow in extensionTable.Rows)
                    {
                        if (Convert.ToInt32(extensionRow[2]) != 0)
                            continue;
                        string extensionItemName = extensionRow[1].ToString();
                        if (!AutoCompleteSuggestionList.Contains(NormalizeItemName(extensionItemName)))
                            AutoCompleteSuggestionList.Add(NormalizeItemName(extensionItemName));
                    }
                }
            }
        }

        private string NormalizeItemName(string value)
        {
            return value.Replace("(", "").Replace(")", "").Replace("+", "");
        }

        private void RtfItemData_ContentsResized(object sender, ContentsResizedEventArgs e)
        {
            #if ITEM_INFO_AUTO_RELOCATE
            var richTextBox = (RichTextBox)sender;
            var currentPanel = richTextBox.Tag as XtraPanel;
            if (currentPanel == null)
                return;
            richTextBox.Width = e.NewRectangle.Width + 10;
            richTextBox.Height = e.NewRectangle.Height + 10;
            int xPos = currentPanel.Left - richTextBox.Width + pbSearchResults.Left;
            int yPos = 0;
            if (currentPanel.Location.Y + richTextBox.Height + pbSearchResults.Top > pbSearchResults.Bottom)
            {
                yPos = pbSearchResults.Bottom - richTextBox.Height ;
            }
            else
            {
                yPos = currentPanel.Location.Y  +pbSearchResults.Top;
            }

            if (currentPanel.Location.X - richTextBox.Width  < pbSearchResults.Left)
            {
                xPos = pbSearchResults.Left /*+ richTextBox.Width */+ currentPanel.Right;
            }
            if (currentPanel.Location.X+ richTextBox.Width > pbSearchResults.Width)
            {
                xPos = currentPanel.Location.X+ pbSearchResults.Left - richTextBox.Width;
            }
            // richTextBox.ClientRectangle
            richTextBox.Location = new Point(xPos, yPos);
        #endif

        }


        private void frmSearchItem_Load(object sender, EventArgs e)
        {
            CommonReference.InitializeDataTableSet();
            CreateSearchResultPanels();
            LoadAutoSuggestionList();
            teItemName.MaskBox.AutoCompleteMode = AutoCompleteMode.Append;
            teItemName.MaskBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            teItemName.MaskBox.AutoCompleteCustomSource = AutoCompleteSuggestionList;

        }



        private void RenderSearchResults(int page = 0)
        {
            pbSearchResults.SuspendLayout();
            foreach (XtraPanel p in SearchResultPanels)
            {
                InventorySlot slot = p.Tag as InventorySlot;
                slot.ItemID = 0;
                p.BackgroundImage = null;
            }

            foreach (var info in SearchResults.Skip(CURRENT_RESULT_PAGE * SEARCH_RESULTS_PER_PAGE).Take(SEARCH_RESULTS_PER_PAGE))
            {
                foreach (var pan in SearchResultPanels)
                {InventorySlot slot = pan.Tag as InventorySlot;
                    if (slot.isEmpty())
                    {
                        slot.ItemID = (uint) info.Num;
                        slot.Info = info;
                        Image img = null;
                        if (!slot.isEmpty())
                        {
                            img = slot.Info == null ? CommonReference.GetIconByIconID((uint)0) : CommonReference.GetIconByIconID((uint)slot.Info.IconID);
                            Color clr = GetColorByItemType(slot.Info.ItemType);
                            float factor = 0.003f;
                            Color tintColor = Color.FromArgb((int)(clr.R * factor), (int)(clr.G * factor), (int)(clr.B * factor));
                            float blueTint = ((((float)(clr.B) / 255.0f) * 100.0f) * factor);
                            float redTint = ((((float)(clr.R) / 255.0f) * 100.0f) * factor);
                            float greenTint = ((((float)(clr.G)/ 255.0f) * 100.0f) * factor);
                            pan.BackgroundImage = ((Bitmap)img).ColorTint(blueTint, greenTint, redTint);

                        }
                        break;
                    }
                }
            }
            pbSearchResults.ResumeLayout();
            lcPageIndex.Text = $"Page {CURRENT_RESULT_PAGE + 1} of {MAX_RESULT_PAGE}\n{SearchResults.Count} result(s)";
        }

        private XtraPanel CreateNewSearchResultPanel(int x, int y)
        {
            var newPanel = new XtraPanel
            {
                Size = new Size(45, 45),
                Location = new Point(x, y),
                BackColor = Color.Transparent,
                AllowDrop = true,
                Tag = new InventorySlot(),
                ContextMenuStrip = cmsResultItem
            };
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

        private static void OnInventoryPanelDragLeave(object sender, EventArgs e)
        {
            var me = sender as XtraPanel;
            DropTargetHelper.DragLeave(me);
        }

        private void OnInventoryPanelDragDrop(object sender, DragEventArgs e)
        {
            var me = sender as XtraPanel;
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
                e.Effect = e.AllowedEffect & DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;

            DropTargetHelper.Drop(e.Data, new Point(e.X, e.Y), e.Effect);
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
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
                e.Effect = e.AllowedEffect & DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;

            DropTargetHelper.DragOver(new Point(e.X, e.Y), e.Effect);
        }

        private static void OnInventoryPanelMouseDown(object sender, MouseEventArgs e)
        {
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
                            new KeyValuePair<string, object>("dragged_slot", slot));

        }

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

        private void OnInventoryPanelMouseLeave(object sender, EventArgs e)
        {
            var senderPanel = sender as XtraPanel;
            Trace.Assert(null != senderPanel);
            var slot = senderPanel.Tag as InventorySlot;
            Trace.Assert(null != slot);
            slot.Hovered = false;
            senderPanel.Refresh();
            /* Re-render the panel */
            if (slot.ItemID > 0)
            {
                rtfItemData.Hide();
            }


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

            using (var pen = new Pen(Color.DarkOrange, 4))
            {
                e.Graphics.DrawLines(pen, CommonReference.PanelGetBorder(0, 0, senderPanel.Width - 1, senderPanel.Height - 1));

            }
        }


        void CreateSearchResultPanels()
        {
            const int inventoryStartX = 13, inventoryStartY = 11;
            for (var i = 0; i < RESULT_PANEL_SLOT_COUNT; i++)
            {
                int plusValueX = 49 * (i % 4), plusValueY = 48 * (i / 4);
                var newPanel = CreateNewSearchResultPanel(inventoryStartX + plusValueX, inventoryStartY + plusValueY);
                //newPanel.BackColor = Color.Blue;
                pbSearchResults.Controls.Add(newPanel);
                SearchResultPanels.Add(newPanel);
            }
            const int inventoryStartX2 = 226, inventoryStartY2 = 11;
            
            for(var i = 0; i < RESULT_PANEL_SLOT_COUNT; i++)
            {
                int plusValueX = 49 * (i % 4), plusValueY = 48 * (i / 4);
                var newPanel = CreateNewSearchResultPanel(inventoryStartX2 + plusValueX, inventoryStartY2 + plusValueY);
                //newPanel.BackColor = Color.Blue;
                pbSearchResults.Controls.Add(newPanel);
                SearchResultPanels.Add(newPanel);
            }
        }


        #region Search
        private void ExecuteSearch()
        {
            Text = $"Item search engine - searching {teItemName.Text}";
            var searchStartTime = Environment.TickCount;
            SearchResults.Clear();
            MAX_RESULT_PAGE = 0;
            CURRENT_RESULT_PAGE = 0;
            UInt32 res;
            if (UInt32.TryParse(teItemName.Text, out res))
            {
                SearchByItemNumber();
            }
            else
            {
                switch (currentType)
                {
                    case ItemType.Any:
                        SearchByBase();
                        SearchByExtension();
                        break;
                    case ItemType.CospreItem:

                    case ItemType.ReverseUniqueItem:
                    case ItemType.UniqueItem:
                        SearchByExtension();
                        break;
                    default:
                        SearchByBase();
                        break;
                }
            }

            // foreach(DataRow dr in StaticRefere)
            var endTime = Environment.TickCount;
            MAX_RESULT_PAGE = SearchResults.Count / SEARCH_RESULTS_PER_PAGE;
            if (SearchResults.Count % SEARCH_RESULTS_PER_PAGE != 0)
                MAX_RESULT_PAGE++;
            CURRENT_RESULT_PAGE = 0;


         
            Text = $"Item search engine - found {SearchResults.Count} results in {endTime - searchStartTime} milliseconds";
            var ordered_results = SearchResults.OrderBy(item1 => item1.Num).ToList();
            SearchResults.Clear();
            SearchResults.AddRange(ordered_results);

            RenderSearchResults();
        }

        private void SearchByItemNumber()
        {
            int searchItemNumber = Convert.ToInt32(teItemName.Text);

            int extensionNumber = searchItemNumber % 1000;
            int baseNumber = searchItemNumber - extensionNumber;

            var itemOrgTable = CommonReference.TableSet.Tables["item_org_us.tbl"];
            if (itemOrgTable == null)
            {
                CommonReference.ShowWarning(this,
                    "Cannot run search operation, because of missing table (item_org_us.tbl)");
                return;
            }
            /* Normal item search */
            foreach (DataRow itemOrgRow in itemOrgTable.Rows)
            {

                int baseItemID = Convert.ToInt32(itemOrgRow[0]);
                byte extTableID = Convert.ToByte(itemOrgRow[1]);

                if (baseItemID == baseNumber)
                {
                    DataTable extensionTable = CommonReference.TableSet.Tables[$"item_ext_{extTableID}_us.tbl"];
                    if (extensionTable == null)
                        continue;
                    foreach (DataRow extensionRow in extensionTable.Rows)
                    {
                        if (Convert.ToInt32(extensionRow[2]) != 0)
                            continue;
                        int extensionID = Convert.ToInt32(extensionRow[0]);
                        ItemInformation item = CommonReference.GetItemDetails(baseItemID, extensionNumber);
                        if (item == null)
                            continue;
                        if (!CheckFilterConditions(item))
                            continue;


                        SearchResults.Add(item);
                        break;
                    }
                }


            }
        }


        private void SearchByBase()
        {
            var itemOrgTable = CommonReference.TableSet.Tables["item_org_us.tbl"];
            if (itemOrgTable == null)
            {
                CommonReference.ShowWarning(this,
                    "Cannot run search operation, because of missing table (item_org_us.tbl)");
                return;
            }
            /* Normal item search */
            foreach (DataRow itemOrgRow in itemOrgTable.Rows)
            {
                string baseItemName = Convert.ToString(itemOrgRow[2]);
                int baseItemID = Convert.ToInt32(itemOrgRow[0]);
                byte extTableID = Convert.ToByte(itemOrgRow[1]);

                if (baseItemName.ToLowerInvariant().Contains(teItemName.Text.ToLowerInvariant()))
                {
                    DataTable extensionTable = CommonReference.TableSet.Tables[$"item_ext_{extTableID}_us.tbl"];
                    if (extensionTable == null)
                        continue;
                    foreach (DataRow extensionRow in extensionTable.Rows)
                    {
                        if (Convert.ToInt32(extensionRow[2]) != 0)
                            continue;
                        int extensionID = Convert.ToInt32(extensionRow[0]);
                        ItemInformation item = CommonReference.GetItemDetails(baseItemID, extensionID);
                        if (item == null)
                            continue;
                        if (!CheckFilterConditions(item))
                            continue;


                        SearchResults.Add(item);
                    }
                }


            }
        }

        private void SearchByExtension()
        {
            foreach (DataTable dt in CommonReference.TableSet.Tables)
            {
                if (dt.TableName == "item_org_us.tbl")
                    continue;

                foreach (DataRow extensionRow in dt.Rows)
                {
                    byte itemType = Convert.ToByte(extensionRow[7]);
                    if (itemType != (int)ItemType.ReverseUniqueItem && itemType != (int)ItemType.UniqueItem && itemType!= (int)ItemType.CospreItem) 
                        continue;
                    int extensionID = Convert.ToInt32(extensionRow[0]);
                    string extensionName = Convert.ToString(extensionRow[1]);
                    int baseItemID = Convert.ToInt32(extensionRow[2]);
                    if (extensionName.ToLowerInvariant().Contains(teItemName.Text.ToLowerInvariant()))
                    {
                       ItemInformation itemInfo = CommonReference.GetItemDetails(baseItemID, extensionID);
                      //  ItemInformation itemInfo = StaticReference.GetItemDetails(baseItemID + extensionID);
                        if (itemInfo == null)
                            continue;
                        if (!CheckFilterConditions(itemInfo))
                            continue;
                        SearchResults.Add(itemInfo);
                    }
                }
            }
        }

        #endregion

        #region Control events
        private void btnSearchItem_Click(object sender, EventArgs e)
        {
            if (teItemName.Text.Length == 0)
            {
              var confirmation = CommonReference.ShowQuestion(this,
                    "You have not specified any item name.\nThis can lead search operation to take a long time.\nDo you want to proceed?");
                if (confirmation == DialogResult.No)
                    return;
            }
            ExecuteSearch();
        }

        private void cbKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbKind.Text)
            {

                case "Any":
                    currentKind = Kind.Any;
                    break;
                case "Reward":
                    currentKind = Kind.Reward;
                    break;
                case "Dagger":
                    currentKind = Kind.Dagger;
                    break;
                case "One Handed Sword":
                    currentKind = Kind.OneHandedSword;
                    break;
                case "Two Handed Sword":
                    currentKind = Kind.TwoHandedSword;
                    break;
                case "One Handed Axe":
                    currentKind = Kind.OneHandedAxe;
                    break;
                case "Two-Handed Axe":
                    currentKind = Kind.TwoHandedAxe;
                    break;
                case "Club":
                    currentKind = Kind.Club;
                    break;
                case "Two Handed Club":
                    currentKind = Kind.TwoHandedClub;
                    break;
                case "Spear":
                    currentKind = Kind.Spear;
                    break;
                case "Long Spear":
                    currentKind = Kind.LongSpear;
                    break;
                case "Shield":
                    currentKind = Kind.Shield;
                    break;
                case "Bow":
                    currentKind = Kind.Bow;
                    break;
                case "Crossbow":
                    currentKind = Kind.Crossbow;
                    break;
                case "Earring":
                    currentKind = Kind.Earring;
                    break;
                case "Necklace":
                    currentKind = Kind.Necklace;
                    break;
                case "Ring":
                    currentKind = Kind.Ring;
                    break;
                case "Belt":
                    currentKind = Kind.Belt;
                    break;
                case "Quest Item":
                    currentKind = Kind.QuestItem;
                    break;
                case "Lune Item":
                    currentKind = Kind.LuneItem;
                    break;
                case "Upgrade Scroll":
                    currentKind = Kind.UpgradeScroll;
                    break;
                case "Monster's Stone":
                    currentKind = Kind.MonstersStone;
                    break;
                case "Staff":
                    currentKind = Kind.Staff;
                    break;
                case "Arrow":
                    currentKind = Kind.Arrow;
                    break;
                case "Javelin":
                    currentKind = Kind.Javelin;
                    break;
                case "Familiar Egg":
                    currentKind = Kind.FamiliarEgg;
                    break;
                case "Familiar":
                    currentKind = Kind.Familiar;
                    break;
                case "Cypher Ring":
                    currentKind = Kind.CypherRing;
                    break;
                case "Autoloot":
                    currentKind = Kind.Autoloot;
                    break;
                case "Image Change Scroll":
                    currentKind = Kind.ImageChangeScroll;
                    break;
                case "Familiar Attack":
                    currentKind = Kind.FamiliarAttack;
                    break;
                case "Familiar Defense":
                    currentKind = Kind.FamiliarDefense;
                    break;
                case "Familiar Loyalty":
                    currentKind = Kind.FamiliarLoyalty;
                    break;
                case "Familiar Speciality Food":
                    currentKind = Kind.FamiliarSpecialityFood;
                    break;
                case "Familiar  Food":
                    currentKind = Kind.FamiliarFood;
                    break;
                case "Priest Mace":
                    currentKind = Kind.PriestMace;
                    break;
                case "Warrior Armor":
                    currentKind = Kind.WarriorArmor;
                    break;
                case "Rogue Armor":
                    currentKind = Kind.RogueArmor;
                    break;
                case "Mage Armor":
                    currentKind = Kind.MageArmor;
                    break;
                case "Priest Armor":
                    currentKind = Kind.PriestArmor;
                    break;
                case "Seal Scroll":
                    currentKind = Kind.SealScroll;
                    break;
                case "Chaos Skill Item":
                    currentKind = Kind.ChaosSkillItem;
                    break;
                case "Power Up Item":
                    currentKind = Kind.PowerUpItem;
                    break;
            }
        }

        private void cbItemType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbItemType.Text)
            {
                case "Any":
                currentType = ItemType.Any;
                    break;
                case "Normal Item":
                    currentType = ItemType.NormalItem;
                    break;
                case "Magic Item":
                    currentType = ItemType.MagicItem;
                    break;
                case "Rare Item":
                    currentType = ItemType.RareItem;
                    break;
                case "Craft Item":
                    currentType = ItemType.CraftItem;
                    break;
                case "Unique Item":
                    currentType = ItemType.UniqueItem;
                    break;
                case "Upgrade Item":
                    currentType = ItemType.UpgradeItem;
                    break;
                case "Event Item":
                    currentType = ItemType.EventItem;
                    break;
                case "Cospre Item":
                    currentType = ItemType.CospreItem;
                    break;
                case "Reverse Item":
                    currentType = ItemType.ReverseItem;
                    break;
                case "Reverse Unique Item":
                    currentType = ItemType.ReverseUniqueItem;
                    break;

            }
        }
        #endregion

        #region Filter
        private bool CheckFilterConditions(ItemInformation item)
        {
            if (currentKind != Kind.Any && item.Kind != (byte)currentKind)
                return false;
            if (currentType != ItemType.Any && item.ItemType != (byte)currentType)
                return false;
            if (sfGrade.Checked)
            {
                if(valGrade.Value <= 9)
                {
                    if (!EvaluateCondition(sfGrade, item.Num % 10, cbGradeOp.SelectedIndex, valGrade.Value))
                    {

                        /* if (valGrade.Value == 0 || valGrade.Value == 10 && item.Num%10 == 0)
                             return true;*/
                        return false;
                    }
                }
                else
                {
                    if (!EvaluateCondition(sfGrade, item.Num % 100, cbGradeOp.SelectedIndex, valGrade.Value))
                    {

                        /* if (valGrade.Value == 0 || valGrade.Value == 10 && item.Num%10 == 0)
                             return true;*/
                        return false;
                    }
                }
            }
          

            if (!EvaluateCondition(sfDagger, item.DaggerAc, cbDaggerOp.SelectedIndex, valDagger.Value))
                return false;
            if (!EvaluateCondition(sfSword, item.SwordAc, cbSwordOp.SelectedIndex, valSword.Value))
                return false;
            if (!EvaluateCondition(sfAxe, item.AxeAc, cbAxeOp.SelectedIndex, valAxe.Value))
                return false;
            if (!EvaluateCondition(sfSpear, item.SpearAc, cbSpearOp.SelectedIndex, valSpear.Value))
                return false;
            if (!EvaluateCondition(sfMace, item.MaceAc, cbMaceOp.SelectedIndex, valMace.Value))
                return false;
            if (!EvaluateCondition(sfBow, item.BowAc, cbBowOp.SelectedIndex, valBow.Value))
                return false;

            if (!EvaluateCondition(sfStr, item.StrB, cbStrOp.SelectedIndex, valStr.Value))
                return false;
            if (!EvaluateCondition(sfHp, item.StaB, cbHpOp.SelectedIndex, valHp.Value))
                return false;
            if (!EvaluateCondition(sfDex, item.DexB, cbDexOp.SelectedIndex, valDex.Value))
                return false;
            if (!EvaluateCondition(sfInt, item.IntelB, cbIntOp.SelectedIndex, valInt.Value))
                return false;
            if (!EvaluateCondition(sfMp, item.ChaB, cbMpOp.SelectedIndex, valMp.Value))
                return false;

            if (!EvaluateCondition(sfFlame, item.FireDamage, cbFlameOp.SelectedIndex, valFlame.Value))
                return false;
            if (!EvaluateCondition(sfGlacier, item.IceDamage, cbGlacierOp.SelectedIndex, valGlacier.Value))
                return false;
            if (!EvaluateCondition(sfLightning, item.LightningDamage, cbLightningOp.SelectedIndex, valLightning.Value))
                return false;
            if (!EvaluateCondition(sfPoison, item.PoisonDamage, cbPoisonOp.SelectedIndex, valPoison.Value))
                return false;
            if (!EvaluateCondition(sfAttack, item.Damage, cbAtkOp.SelectedIndex, valAttack.Value))
                return false;
            if (!EvaluateCondition(sfDefense, item.Ac, cbDefOp.SelectedIndex, valDefense.Value))
                return false;

            /* Flame glacier light curse poison magic */

            if (!EvaluateCondition(sfFlameR, item.FireR, cbFlameROp.SelectedIndex, valFlameR.Value))
                return false;
            if (!EvaluateCondition(sfGlacierR, item.ColdR, cbGlacierROp.SelectedIndex, valGlacier.Value))
                return false;
            if (!EvaluateCondition(sfLightningR, item.LightningR, cbLightningROp.SelectedIndex, valLightningR.Value))
                return false;
            if (!EvaluateCondition(sfCurseR, item.CurseR, cbCurseROp.SelectedIndex, valCurseR.Value))
                return false;
            if (!EvaluateCondition(sfPoisonR, item.PoisonR, cbPoisonROp.SelectedIndex, valPoisonR.Value))
                return false;
            if (!EvaluateCondition(sfMagicR, item.MagicR, cbMagicROp.SelectedIndex, valMagicR.Value))
                return false;

            /* Drain & repel */
            if (!EvaluateCondition(sfHpRecovery, item.HpDrain, cbHpRecoveryOp.SelectedIndex, valHpRecovery.Value))
                return false;
            if (!EvaluateCondition(sfMpRecovery, item.MpDrain, cbMpRecoveryOp.SelectedIndex, valMpRecovery.Value))
                return false;
            if (!EvaluateCondition(sfMpDamage, item.MpDamage, cbMpDamageOp.SelectedIndex, valMpDamage.Value))
                return false;
            if (!EvaluateCondition(sfRepel, item.MirrorDamage,cbRepelOp.SelectedIndex,valRepel.Value))
                return false;

            return true;
        }

        private static bool EvaluateCondition(CheckEdit checkEdit, int value, int comparisonType, decimal targetValue)
        {
            if (!checkEdit.Checked)
                return true;
            switch (comparisonType)
            {
                case 0:
                    return value > targetValue;
                case 1:
                    return value < targetValue;
                case 2:
                    return value >= targetValue;
                case 3:
                    return value <= targetValue;
                case 4:
                    return value == targetValue;
                case 5:
                    return value != targetValue;
            }
            return false;
        }
        #endregion

        private Color GetColorByItemType(int itemType)
        {
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

            switch (itemType)
            {
                case 0:
                    return clrWhite;
                case 1:
                    return clrMagic;
                case 2:
                    return clrRare;
                case 3:
                    return clrCraft;
                case 4:
                    return clrUnique;
                case 5:
                    return clrUpgrade;
                case 6:
                    return clrUpgrade;
                case 8:
                    return clrCospre;
                case 11:
                    return clrReverse;
                case 12:
                    return clrReverseUnique;
                default:
                    return clrUpgrade;

            }
        }

        #region Show item details
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
            if (isUnique && isReverse)
            {
                _rtfColor = clrBonus;
                AddText("Item Grade: High Class Unique (Reverse) ");
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
            rtfItemData.ResumeLayout(true);
        }
        #endregion
        private void cmsResultItem_Opening(object sender, CancelEventArgs e)
        {

        }

        private void copyItemIDToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var senderPanel = cmsResultItem.SourceControl as XtraPanel;

            var senderSlot = senderPanel?.Tag as InventorySlot;
            if (senderSlot == null)
                return;
            Clipboard.SetText(senderSlot.ItemID.ToString());
        }

        private void copyAsgiveitemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var senderPanel = cmsResultItem.SourceControl as XtraPanel;

            var senderSlot = senderPanel?.Tag as InventorySlot;
            if (senderSlot == null)
                return;
            Clipboard.SetText($"+give_item {senderSlot.ItemID}");
        }

        private void sbResultPrev_Click(object sender, EventArgs e)
        {
            if (CURRENT_RESULT_PAGE == 0)
            {
                sbResultPrev.ToolTip = "No more pages exist.";
            }
            else
                CURRENT_RESULT_PAGE--;

            RenderSearchResults(CURRENT_RESULT_PAGE);

        }

        private void rtfItemData_TextChanged(object sender, EventArgs e)
        {

        }

        private void sbResultNext_Click(object sender, EventArgs e)
        {
            if (CURRENT_RESULT_PAGE+1 == MAX_RESULT_PAGE)
            {
                sbResultPrev.ToolTip = "No more pages exist.";
            }
            else
                CURRENT_RESULT_PAGE++;

            RenderSearchResults(CURRENT_RESULT_PAGE);

        }
    }
}