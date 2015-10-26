using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerDataTool
{
    public partial class QuestDialog : Form
    {
        Dictionary<uint, NPC> _npcs;
        Dictionary<uint, ItemTemplate> _items;
        Quest _selectedQuest;
        bool _selectingQuest;

        public QuestDialog()
        {
            InitializeComponent();
        }

        private void QuestDialog_Load(object sender, EventArgs e)
        {
            cbCompletionType.DataSource = Enum.GetValues(typeof(QuestStep.CompletionType));
            cbRequirementType.DataSource = Enum.GetValues(typeof(QuestRequirement.Type));

            foreach (IntStrID icon in Program.s_npcIcons.Values)
                cbIcon.Items.Add(icon);
            foreach( IntStrID staticText in Program.s_staticText.Values )
                cbStaticText.Items.Add(staticText);

            _items = new Dictionary<uint, ItemTemplate>();
            ItemTemplate[] items = Database.FetchItems();
            foreach (ItemTemplate it in items)
            {
                _items[it.ID] = it;
                IntStrID itemID = Program.s_items.ContainsKey((int)it.ID) ? Program.s_items[(int)it.ID] : new IntStrID("Unknown", (int)it.ID);
                cbItem.Items.Add(itemID);
            }


            // Pull all the NPCs
            NPC[] npcs = Database.FetchNPCs(0);
            _npcs = new Dictionary<uint, NPC>();
            foreach (NPC npc in npcs)
            {
                _npcs[npc.ID] = npc;
                cbQuestGiver.Items.Add(npc);
                cbStepOwner.Items.Add(npc);
            }

            // Pull quests from DB
            Quest[] quests = Database.FetchQuests();
            foreach (Quest q in quests)
            {
                AddQuest(q);
            }

            SelectQuest(null);
            btnSave.Enabled = false;
        }

        void AddQuest(Quest q)
        {
            ListViewItem lvi = lvQuests.Items.Add(q.ID.ToString());
            lvi.SubItems.Add(q.Name);
            if (_npcs.ContainsKey(q.GiverID))
            {
                NPC giver = _npcs[q.GiverID];
                NPCNameID giverName = Program.s_npcNameIDs[giver.GameID];
                lvi.SubItems.Add(giver.ID.ToString() + ": " + giverName.ToString());
            }
            else
                lvi.SubItems.Add(q.GiverID.ToString());
            lvi.SubItems.Add(q.GiverMapID.ToString());
            lvi.Tag = q;
        }

        void SelectQuest(Quest q)
        {
            _selectingQuest = true;
            btnDeleteQuest.Enabled = false;
            tbQuestName.Text = "";
            cbQuestGiver.SelectedItem = null;

            lvSteps.Items.Clear();
            lvSteps.Enabled = false;
            btnDeleteStep.Enabled = false;
            btnStepUp.Enabled = false;
            btnStepDown.Enabled = false;
            btnNewStep.Enabled = false;
            cbCompletionType.Enabled = false;
            cbCompletionType.SelectedItem = null;
            tbCompletionCount.Enabled = false;
            tbCompletionCount.Text = "";
            tbCompletionTarget.Enabled = false;
            tbCompletionTarget.Text = "";
            cbCompTarget.Visible = false;
            cbStepOwner.Enabled = false;
            cbStepOwner.SelectedItem = null;

            lvRequirements.Items.Clear();
            lvRequirements.Enabled = false;
            cbRequirementType.Enabled = false;
            cbRequirementType.SelectedItem = null;
            tbRequirementParam.Enabled = false;
            tbRequirementParam.Text = "";
            cbReqParam.Visible = false;
            btnDeleteRequirement.Enabled = false;
            btnNewRequirement.Enabled = false;

            lvRewards.Items.Clear();
            lvRewards.Enabled = false;
            tbGold.Enabled = false;
            tbGold.Text = "";
            tbExp.Enabled = false;
            tbExp.Text = "";
            tbFame.Enabled = false;
            tbFame.Text = "";
            cbItem.Enabled = false;
            cbItem.SelectedItem = null;
            btnDeleteReward.Enabled = false;
            btnAddReward.Enabled = false;

            lvLines.Items.Clear();
            lvLines.Enabled = false;
            cbIcon.Enabled = false;
            cbIcon.SelectedItem = null;
            cbStaticText.Enabled = false;
            cbStaticText.SelectedItem = null;
            tbDynamicText.Enabled = false;
            tbDynamicText.Text = "";
                        
            _selectedQuest = q;
            if (q != null)
            {
                // Set properties
                tbQuestName.Text = q.Name;            
                cbQuestGiver.SelectedItem = q.GiverID == 0 ? null : _npcs[q.GiverID];

                // Set steps
                foreach (QuestStep qs in q.Steps)
                {
                    ListViewItem lvi = lvSteps.Items.Add(qs.CompType.ToString());
                    lvi.SubItems.Add(StepContextString(qs));
                    lvi.SubItems.Add(qs.CompCount.ToString());
                    lvi.Tag = qs;
                }

                // Set Requirements
                foreach (QuestRequirement qr in q.Requirements)
                {
                    ListViewItem lvi = lvRequirements.Items.Add(qr.TheType.ToString());
                    lvi.SubItems.Add(RequirmentContext(qr));
                    lvi.Tag = qr;
                }

                btnDeleteQuest.Enabled = true;
                lvSteps.Enabled = true;
                lvRequirements.Enabled = true;
                btnNewStep.Enabled = true;
                btnNewRequirement.Enabled = true;
            }
            _selectingQuest = false;
        }

        bool CheckForDirtyQuests()
        {
            foreach (ListViewItem lvi in lvQuests.Items)
            {
                Quest lvq = (Quest)lvi.Tag;
                if (lvq.Dirty)
                {
                    return true;
                }
            }
            return false;
        }

        void SetDirty(bool dirty)
        {
            if(_selectingQuest )
                return;

            if( _selectedQuest != null )
                _selectedQuest.Dirty = dirty;
            
            bool anyQuestsDirty = CheckForDirtyQuests();
            
            Text = "Quest Dialog";
            if( anyQuestsDirty )
                Text += "*";
            btnSave.Enabled = anyQuestsDirty;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveDirtyQuests();

            // Clear dirty flag
            SetDirty(false);
        }

        void SaveDirtyQuests()
        {
            foreach (ListViewItem lvi in lvQuests.Items)
            {
                Quest lvq = (Quest)lvi.Tag;
                if (lvq.Dirty)
                {
                    Database.SaveQuest(lvq);
                    lvq.Dirty = false;
                }
            }
        }

        private void QuestDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CheckForDirtyQuests())
            {
                DialogResult res = MessageBox.Show("There are unsaved changes. Would you like to save them now?", "Close Quests", MessageBoxButtons.YesNoCancel);
                if( res == DialogResult.Yes )
                    SaveDirtyQuests();
                if( res == DialogResult.Cancel )
                    e.Cancel = true;
            }

        }

        #region Quest
        private void lvQuests_SelectedIndexChanged(object sender, EventArgs e)
        {
            Quest q = null;
            if (lvQuests.SelectedItems.Count > 0)
            {
                q = (Quest)lvQuests.SelectedItems[0].Tag;
            }
            SelectQuest(q);
        }

        private void tbQuestName_TextChanged(object sender, EventArgs e)
        {
            if (lvQuests.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvQuests.SelectedItems[0];
                Quest q = (Quest)lvi.Tag;
                q.Name = tbQuestName.Text;
                lvi.SubItems[1].Text = q.Name;
                SetDirty(true);
            }
        }

        private void cbQuestGiver_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvQuests.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvQuests.SelectedItems[0];
                Quest q = (Quest)lvi.Tag;
                NPC giver = (NPC)cbQuestGiver.SelectedItem;
                q.GiverID = giver.ID;
                q.GiverMapID = giver.MapID;
                lvi.SubItems[2].Text = giver.ToString();
                lvi.SubItems[3].Text = q.GiverMapID.ToString();
                SetDirty(true);
            }
        }

        private void btnDeleteQuest_Click(object sender, EventArgs e)
        {
            if (lvQuests.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Are you sure you want to delete this quest?", "Delete Quest", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    ListViewItem lvi = lvQuests.SelectedItems[0];
                    Quest q = (Quest)lvi.Tag;

                    // Deselect the quest
                    SelectQuest(null);

                    // Remove the quest from the display
                    lvQuests.Items.Remove(lvi);

                    // Delete the quest from the database
                    Database.DeleteQuest(q);
                }
            }
        }

        private void btnNewQuest_Click(object sender, EventArgs e)
        {
            // Add a new quest to the database
            NPC giver = (NPC)cbQuestGiver.SelectedItem;
            Quest q = new Quest(0, giver == null ? 0 : giver.ID, giver == null ? (ushort)0 : giver.MapID);
            q.New = true;            

            // Put it in the display
            AddQuest(q);

            SetDirty(true);
        }
        #endregion

        #region Steps
        void SelectCompTarget(QuestStep qs)
        {
            tbCompletionTarget.Enabled = false;
            cbCompTarget.Visible = false;
            tbCompletionCount.Enabled = false;

            tbCompletionCount.Text = qs.CompCount.ToString();
            switch (qs.CompType)
            {
                case QuestStep.CompletionType.KillMonster:
                    // TODO: Monsters
                    tbCompletionCount.Enabled = true;
                    tbCompletionTarget.Enabled = true;
                    tbCompletionTarget.Text = qs.CompTargetID.ToString();
                    break;
                case QuestStep.CompletionType.GoToLocation:
                    // TODO: Locations
                    tbCompletionTarget.Enabled = true;
                    tbCompletionTarget.Text = qs.CompTargetID.ToString();
                    break;
                case QuestStep.CompletionType.CollectItem:
                    // TODO: Items
                    tbCompletionCount.Enabled = true;
                    tbCompletionTarget.Enabled = true;
                    tbCompletionTarget.Text = qs.CompTargetID.ToString();
                    break;                
                case QuestStep.CompletionType.ReachLevel:
                    tbCompletionCount.Enabled = true;
                    break;
                case QuestStep.CompletionType.TalkToNPC:
                    cbCompTarget.Visible = true;
                    cbCompTarget.Items.Clear();
                    foreach (NPC npc in _npcs.Values)
                        cbCompTarget.Items.Add(npc);
                    cbCompTarget.SelectedItem = _npcs.ContainsKey(qs.CompTargetID) ? _npcs[qs.CompTargetID] : null;
                    break;
            }
        }

        string StepContextString(QuestStep qs)
        {
            string str = "";
            switch (qs.CompType)
            {
                case QuestStep.CompletionType.KillMonster:
                case QuestStep.CompletionType.GoToLocation:
                case QuestStep.CompletionType.CollectItem:
                    str = qs.CompTargetID.ToString();
                    break;
                case QuestStep.CompletionType.ReachLevel:                    
                    break;  // Empty
                case QuestStep.CompletionType.TalkToNPC:
                    str = _npcs.ContainsKey(qs.CompTargetID) ? _npcs[qs.CompTargetID].ToString() : "Unknown";
                    break;
            }
            return str;
        }

        void FixStepButtons(QuestStep qs)
        {
            btnStepUp.Enabled = false;
            btnStepDown.Enabled = false;
            int index = _selectedQuest.Steps.IndexOf(qs);
            btnStepUp.Enabled = (index > 0);
            btnStepDown.Enabled = (index < (_selectedQuest.Steps.Count - 1));
        }

        private void lvSteps_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbCompletionType.Enabled = false;
            tbCompletionCount.Enabled = false;
            tbCompletionTarget.Enabled = false;
            cbCompTarget.Visible = false;
            cbStepOwner.Enabled = false;
            btnDeleteStep.Enabled = false;
            btnStepUp.Enabled = false;
            btnStepDown.Enabled = false;

            if (lvSteps.SelectedItems.Count != 0)
            {
                ListViewItem lvi = lvSteps.SelectedItems[0];
                QuestStep qs = (QuestStep)lvi.Tag;
                
                cbCompletionType.SelectedIndex = (int)qs.CompType;
                cbStepOwner.SelectedItem = _npcs.ContainsKey(qs.OwnerID) ? _npcs[qs.OwnerID] : null;

                cbCompletionType.Enabled = true;
                tbCompletionCount.Enabled = true;
                cbStepOwner.Enabled = true;
                btnDeleteStep.Enabled = true;
                FixStepButtons(qs);
                SelectCompTarget(qs);

                // Populate Rewards
                lvRewards.Enabled = true;
                lvRewards.Items.Clear();
                btnAddReward.Enabled = true;
                foreach (QuestReward qr in qs.Rewards)
                {
                    AddRewardToDisplay(qr);
                }

                // Populate Lines
                lvLines.Enabled = true;
                lvLines.Items.Clear();
                btnNewLine.Enabled = true;
                foreach (QuestLine ql in qs.Lines)
                {
                    AddLineToDisplay(ql);
                }
            }
        }

        private void cbCompletionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSteps.SelectedItems.Count != 0)
            {
                ListViewItem lvi = lvSteps.SelectedItems[0];
                QuestStep qs = (QuestStep)lvi.Tag;

                qs.CompType = (QuestStep.CompletionType)cbCompletionType.SelectedIndex;
                lvi.Text = qs.CompType.ToString();
                lvi.SubItems[1].Text = StepContextString(qs);
                SelectCompTarget(qs);


                SetDirty(true);
            }
        }

        private void tbCompletionCount_TextChanged(object sender, EventArgs e)
        {
            if (lvSteps.SelectedItems.Count != 0)
            {
                ListViewItem lvi = lvSteps.SelectedItems[0];
                QuestStep qs = (QuestStep)lvi.Tag;

                try
                {
                    uint count = Convert.ToUInt32(tbCompletionCount.Text);
                    qs.CompCount = count;
                    SetDirty(true);
                }
                catch (Exception)
                {
                }
            }
        }

        private void tbCompletionTarget_TextChanged(object sender, EventArgs e)
        {
            if (lvSteps.SelectedItems.Count != 0)
            {
                ListViewItem lvi = lvSteps.SelectedItems[0];
                QuestStep qs = (QuestStep)lvi.Tag;

                try
                {
                    uint target = Convert.ToUInt32(tbCompletionTarget.Text);
                    qs.CompTargetID = target;
                    SetDirty(true);
                }
                catch (Exception)
                {
                }
            }
        }

        private void cbCompTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCompTarget.SelectedItem != null)
            {
                ListViewItem lvi = lvSteps.SelectedItems[0];
                QuestStep qs = (QuestStep)lvi.Tag;

                switch (qs.CompType)
                {
                    case QuestStep.CompletionType.KillMonster:
                    case QuestStep.CompletionType.GoToLocation:
                    case QuestStep.CompletionType.CollectItem:
                    case QuestStep.CompletionType.ReachLevel:
                        break;
                    case QuestStep.CompletionType.TalkToNPC:
                        NPC npc = (NPC)cbCompTarget.SelectedItem;
                        qs.CompTargetID = npc.ID;
                        SetDirty(true);
                        break;
                }
            }
        }

        private void cbStepOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStepOwner.SelectedItem != null && lvSteps.SelectedItems.Count != 0)
            {
                ListViewItem lvi = lvSteps.SelectedItems[0];
                QuestStep qs = (QuestStep)lvi.Tag;

                NPC owner = (NPC)cbStepOwner.SelectedItem;
                qs.OwnerID = owner.ID;

                SetDirty(true);
            }
        }

        private void btnDeleteStep_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this step?", "Delete Step", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Get the step and display
                ListViewItem lvi = lvSteps.SelectedItems[0];
                QuestStep qs = (QuestStep)lvi.Tag;

                // Deselect
                lvi.Selected = false;

                // Remove the step from the quest
                _selectedQuest.DeleteStep(qs);

                // Remove the step from the display
                lvSteps.Items.Remove(lvi);
            }
        }

        private void btnStepUp_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = lvSteps.SelectedItems[0];
            QuestStep qs = (QuestStep)lvi.Tag;

            // Reorder in the array
            int stepIndex = _selectedQuest.Steps.IndexOf(qs);
            _selectedQuest.Steps.RemoveAt(stepIndex);
            _selectedQuest.Steps.Insert(stepIndex - 1, qs);

            // Reorder in the list view
            stepIndex = lvSteps.Items.IndexOf(lvi);
            lvSteps.Items.Remove(lvi);
            lvSteps.Items.Insert(stepIndex - 1, lvi);
            
            // mark the quest dirty
            SetDirty(true);

            // Fix the buttons
            FixStepButtons(qs);
        }

        private void btnStepDown_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = lvSteps.SelectedItems[0];
            QuestStep qs = (QuestStep)lvi.Tag;

            // Reorder in the array
            int stepIndex = _selectedQuest.Steps.IndexOf(qs);
            _selectedQuest.Steps.RemoveAt(stepIndex);
            _selectedQuest.Steps.Insert(stepIndex + 1, qs);

            // Reorder in the list view
            stepIndex = lvSteps.Items.IndexOf(lvi);
            lvSteps.Items.Remove(lvi);
            lvSteps.Items.Insert(stepIndex + 1, lvi);

            // mark the quest dirty
            SetDirty(true);

            // Fix the buttons
            FixStepButtons(qs);
        }

        private void btnNewStep_Click(object sender, EventArgs e)
        {
            // Create a new step
            QuestStep step = new QuestStep((byte)_selectedQuest.Steps.Count, QuestStep.CompletionType.TalkToNPC, 0, 0, 0, 0);
            step.New = true;

            // Add it to the quest
            _selectedQuest.Steps.Add(step);
            
            // Add it to the display
            SelectQuest(_selectedQuest);
        }
        #endregion

        #region Requirements
        string RequirmentContext(QuestRequirement qr)
        {
            string str = "";
            switch (qr.TheType)
            {
                case QuestRequirement.Type.Level:
                case QuestRequirement.Type.Fame:
                case QuestRequirement.Type.Item:
                    str = qr.Context.ToString();
                    break;
                case QuestRequirement.Type.Race:
                    str = qr.Context == 0 ? "Millena" : "Rain";
                    break;
                case QuestRequirement.Type.Gender:
                    str = qr.Context == 0 ? "Male" : "Female";
                    break;
                case QuestRequirement.Type.Job:
                    str = Program.s_jobs.ContainsKey((int)qr.Context) ?  Program.s_jobs[(int)qr.Context].ToString() : "unknown";
                    break;
            }
            return str;
        }

        void SelectContext(QuestRequirement qr)
        {
            tbRequirementParam.Enabled = false;
            cbReqParam.Visible = false;
            switch (qr.TheType)
            {
                case QuestRequirement.Type.Level:
                    tbRequirementParam.Text = qr.Context.ToString();
                    tbRequirementParam.Enabled = true;
                    break;
                case QuestRequirement.Type.Race:
                    cbReqParam.Visible = true;
                    cbReqParam.Items.Clear();
                    cbReqParam.Items.Add("Millena");
                    cbReqParam.Items.Add("Rain");
                    cbReqParam.SelectedIndex = (int)qr.Context;
                    break;
                case QuestRequirement.Type.Gender:
                    cbReqParam.Visible = true;
                    cbReqParam.Items.Clear();
                    cbReqParam.Items.Add("Male");
                    cbReqParam.Items.Add("Female");
                    cbReqParam.SelectedIndex = (int)qr.Context;
                    break;
                case QuestRequirement.Type.Job:
                    cbReqParam.Visible = true;
                    cbReqParam.Items.Clear();
                    foreach (IntStrID job in Program.s_jobs.Values)
                        cbReqParam.Items.Add(job);
                    cbReqParam.SelectedIndex = (int)qr.Context;
                    break;
                case QuestRequirement.Type.Fame:
                    tbRequirementParam.Text = qr.Context.ToString();
                    tbRequirementParam.Enabled = true;
                    break;
                case QuestRequirement.Type.Item:
                    tbRequirementParam.Text = qr.Context.ToString();
                    tbRequirementParam.Enabled = true;
                    break;
            }
        }

        private void lvRequirements_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbRequirementParam.Enabled = false;
            cbRequirementType.Enabled = false;
            btnDeleteQuest.Enabled = false;
            cbReqParam.Visible = false;

            if (lvRequirements.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvRequirements.SelectedItems[0];
                QuestRequirement qr = (QuestRequirement)lvi.Tag;

                cbRequirementType.SelectedIndex = (int)qr.TheType;
                cbRequirementType.Enabled = true;
                btnDeleteQuest.Enabled = true;

                SelectContext(qr);
            }
        }

        private void cbRequirementType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvRequirements.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvRequirements.SelectedItems[0];
                QuestRequirement qr = (QuestRequirement)lvi.Tag;

                qr.TheType = (QuestRequirement.Type)cbRequirementType.SelectedIndex;
                lvi.SubItems[1].Text = RequirmentContext(qr);
                lvi.Text = qr.TheType.ToString();
                SelectContext(qr);
                SetDirty(true);
            }
        }

        private void cbReqParam_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvRequirements.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvRequirements.SelectedItems[0];
                QuestRequirement qr = (QuestRequirement)lvi.Tag;

                switch ( qr.TheType )
                {
                    case QuestRequirement.Type.Gender:
                    case QuestRequirement.Type.Race:
                        qr.Context = (uint)cbReqParam.SelectedIndex;
                        break;
                    case QuestRequirement.Type.Job:
                        IntStrID job = (IntStrID)cbReqParam.SelectedItem;
                        qr.Context = (uint)job.ID;
                        break;
                }
                lvi.SubItems[1].Text = RequirmentContext(qr);
                SetDirty(true);
            }
        }

        private void tbRequirementParam_TextChanged(object sender, EventArgs e)
        {
            if (lvRequirements.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvRequirements.SelectedItems[0];
                QuestRequirement qr = (QuestRequirement)lvi.Tag;

                try
                {
                    uint param = Convert.ToUInt32(tbRequirementParam.Text);
                    qr.Context = param;
                    lvi.SubItems[1].Text = RequirmentContext(qr);
                    SetDirty(true);
                }
                catch (Exception) { }
            }
        }

        private void btnDeleteRequirement_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this requirement?", "Delete Requirement", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ListViewItem lvi = lvRequirements.SelectedItems[0];
                QuestRequirement qr = (QuestRequirement)lvi.Tag;

                // Deselect
                lvi.Selected = false;

                // Remove from the quest
                _selectedQuest.DeleteRequirement(qr);

                // Remove from the list view
                lvRequirements.Items.Remove(lvi);

                SetDirty(true);
            }
        }

        private void btnNewRequirement_Click(object sender, EventArgs e)
        {
            if (_selectedQuest != null)
            {
                // Create the requirement
                QuestRequirement qr = new QuestRequirement(0, QuestRequirement.Type.Level, 0);
                qr.New = true;

                // Add it to the quest
                _selectedQuest.Requirements.Add(qr);

                // Add it to the display
                SelectQuest(_selectedQuest);

                // Mark dirty
                SetDirty(true);
            }
        }
        #endregion

        #region Rewards
        void AddRewardToDisplay(QuestReward qr)
        {
            ListViewItem rvi = lvRewards.Items.Add(qr.Gold.ToString());
            rvi.SubItems.Add(qr.Exp.ToString());
            rvi.SubItems.Add(qr.Fame.ToString());
            rvi.SubItems.Add(qr.Item.ToString());
            rvi.Tag = qr;
        }

        private void lvRewards_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbGold.Text = "";
            tbExp.Text = "";
            tbFame.Text = "";
            cbItem.SelectedItem = null;
            tbGold.Enabled = false;
            tbExp.Enabled = false;
            tbFame.Enabled = false;
            cbItem.Enabled = false;
            btnDeleteReward.Enabled = false;

            if (lvRewards.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvRewards.SelectedItems[0];
                QuestReward qr = (QuestReward)lvi.Tag;

                tbGold.Text = qr.Gold.ToString();
                tbExp.Text = qr.Exp.ToString();
                tbFame.Text = qr.Fame.ToString();               
                                
                if (_items.ContainsKey(qr.Item))
                {
                    ItemTemplate item = _items[qr.Item];
                    if (Program.s_items.ContainsKey((int)item.ID))
                        cbItem.SelectedItem = Program.s_items[(int)item.ID];
                    else
                    {
                        cbItem.SelectedItem = null;
                        foreach (object obj in cbItem.Items)
                        {
                            IntStrID id = (IntStrID)obj;
                            if (id != null && id.ID == item.ID)
                            {
                                cbItem.SelectedItem = obj;
                                break;
                            }
                        }
                    }
                }
                else
                    cbItem.SelectedItem = null;

                tbGold.Enabled = true;
                tbExp.Enabled = true;
                tbFame.Enabled = true;
                cbItem.Enabled = true;
                btnDeleteReward.Enabled = true;
            }
        }

        private void tbGold_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ListViewItem lvi = lvRewards.SelectedItems[0];
                QuestReward qr = (QuestReward)lvi.Tag;

                qr.Gold = Convert.ToUInt32(tbGold.Text);
                lvi.Text = tbGold.Text;
                SetDirty(true);
            }
            catch (Exception) { }
        }

        private void tbExp_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ListViewItem lvi = lvRewards.SelectedItems[0];
                QuestReward qr = (QuestReward)lvi.Tag;

                qr.Exp = Convert.ToUInt32(tbExp.Text);
                lvi.SubItems[1].Text = tbExp.Text;
                SetDirty(true);
            }
            catch (Exception) { }
        }

        private void tbFame_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ListViewItem lvi = lvRewards.SelectedItems[0];
                QuestReward qr = (QuestReward)lvi.Tag;

                qr.Fame = Convert.ToUInt32(tbFame.Text);
                lvi.SubItems[2].Text = tbFame.Text;
                SetDirty(true);
            }
            catch (Exception) { }
        }

        private void cbItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO: Implement items
        }

        private void btnDeleteReward_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this reward?", "Delete Reward", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Get the reward
                ListViewItem lvi = lvRewards.SelectedItems[0];
                QuestReward qr = (QuestReward)lvi.Tag;

                // Deselect
                lvi.Selected = false;
                                
                // Remove from the quest step
                ListViewItem lvs = lvSteps.SelectedItems[0];
                QuestStep qs = (QuestStep)lvs.Tag;
                qs.DeleteReward(qr);                

                // Remove from the display
                lvRewards.Items.Remove(lvi);

                SetDirty(true);
            }
        }

        private void btnAddReward_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = lvSteps.SelectedItems[0];
            QuestStep qs = (QuestStep)lvi.Tag;

            // Create reward
            QuestReward qr = new QuestReward(0, 0, 0, 0);
            qr.New = true;

            // Add to the step
            qs.Rewards.Add(qr);

            // Add to the display
            AddRewardToDisplay(qr);

            SetDirty(true);
        }
        #endregion

        #region Lines
        void AddLineToDisplay(QuestLine ql)
        {
            string icon = Program.s_npcIcons.ContainsKey(ql.Icon) ? Program.s_npcIcons[ql.Icon].ToString() : "None";
            string text = Program.s_staticText.ContainsKey(ql.StaticText) ? Program.s_staticText[ql.StaticText].ToString() : ql.DynamicText;
            ListViewItem lvl = lvLines.Items.Add(icon);
            lvl.SubItems.Add(text);
            lvl.Tag = ql;
        }

        void FixLineButtons(QuestLine ql)
        {
            btnLineUp.Enabled = false;
            btnLineDown.Enabled = false;
            ListViewItem lvs = lvSteps.SelectedItems[0];
            QuestStep qs = (QuestStep)lvs.Tag;
            int index = qs.Lines.IndexOf(ql);
            btnLineUp.Enabled = (index > 0);
            btnLineDown.Enabled = (index < (qs.Lines.Count - 1));
        }

        private void lvLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbIcon.Enabled = false;
            cbStaticText.Enabled = false;
            tbDynamicText.Enabled = false;
            btnDeleteLine.Enabled = false;
            btnLineUp.Enabled = false;
            btnLineDown.Enabled = false;

            if (lvLines.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvLines.SelectedItems[0];
                QuestLine ql = (QuestLine)lvi.Tag;

                cbIcon.Enabled = true;
                cbStaticText.Enabled = true;
                tbDynamicText.Enabled = true;

                cbIcon.SelectedItem = Program.s_npcIcons.ContainsKey(ql.Icon) ? Program.s_npcIcons[ql.Icon] : null;
                cbStaticText.SelectedItem = Program.s_staticText.ContainsKey(ql.StaticText) ? Program.s_staticText[ql.StaticText] : null;
                tbDynamicText.Text = ql.DynamicText;

                FixLineButtons(ql);
                btnDeleteLine.Enabled = true;
            }
        }

        private void btnDeleteLine_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this line?", "Delete Line", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ListViewItem lvi = lvLines.SelectedItems[0];
                QuestLine ql = (QuestLine)lvi.Tag;
                
                // Deselect
                lvi.Selected = false;

                // Delete from step
                ListViewItem lvs = lvSteps.SelectedItems[0];
                QuestStep qs = (QuestStep)lvs.Tag;
                qs.DeleteLine(ql);

                // Delete from display
                lvLines.Items.Remove(lvi);    
                
                SetDirty(true);                           
            }
        }

        private void btnLineUp_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = lvLines.SelectedItems[0];
            QuestLine ql = (QuestLine)lvi.Tag;

            ListViewItem lvs = lvSteps.SelectedItems[0];
            QuestStep qs = (QuestStep)lvs.Tag;

            // Reorder in the array
            int index = qs.Lines.IndexOf(ql);
            qs.Lines.RemoveAt(index);
            qs.Lines.Insert(index - 1, ql);

            // Reorder in the list view
            index = lvLines.Items.IndexOf(lvi);
            lvLines.Items.Remove(lvi);
            lvLines.Items.Insert(index - 1, lvi);

            // mark the quest dirty
            SetDirty(true);

            // Fix the buttons
            FixLineButtons(ql);
        }

        private void btnLineDown_Click(object sender, EventArgs e)
        {

            ListViewItem lvi = lvLines.SelectedItems[0];
            QuestLine ql = (QuestLine)lvi.Tag;

            ListViewItem lvs = lvSteps.SelectedItems[0];
            QuestStep qs = (QuestStep)lvs.Tag;

            // Reorder in the array
            int index = qs.Lines.IndexOf(ql);
            qs.Lines.RemoveAt(index);
            qs.Lines.Insert(index + 1, ql);

            // Reorder in the list view
            index = lvLines.Items.IndexOf(lvi);
            lvLines.Items.Remove(lvi);
            lvLines.Items.Insert(index + 1, lvi);

            // mark the quest dirty
            SetDirty(true);

            // Fix the buttons
            FixLineButtons(ql);
        }

        private void btnNewLine_Click(object sender, EventArgs e)
        {
            // Create a quest line
            QuestLine ql = new QuestLine(0, 0, 0, "", 0);
            ql.New = true;

            // Add it to the step
            ListViewItem lvs = lvSteps.SelectedItems[0];
            QuestStep qs = (QuestStep)lvs.Tag;
            qs.Lines.Add(ql);

            // Add it to the display
            AddLineToDisplay(ql);

            // Mark quest dirty
            SetDirty(true);
        }

        private void cbIcon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbIcon.SelectedItem != null)
            {
                ListViewItem lvi = lvLines.SelectedItems[0];
                QuestLine ql = (QuestLine)lvi.Tag;

                IntStrID icon = (IntStrID)cbIcon.SelectedItem;
                ql.Icon = (ushort)icon.ID;
                lvi.Text = Program.s_npcIcons.ContainsKey(ql.Icon) ? Program.s_npcIcons[ql.Icon].ToString() : "None";

                SetDirty(true);
            }
        }

        private void cbStaticText_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem lvi = lvLines.SelectedItems[0];
            QuestLine ql = (QuestLine)lvi.Tag;

            IntStrID staticText = (IntStrID)cbStaticText.SelectedItem;
            ql.StaticText = (ushort)staticText.ID;
            lvi.SubItems[1].Text = Program.s_staticText.ContainsKey(ql.StaticText) ? Program.s_staticText[ql.StaticText].ToString() : ql.DynamicText;

            SetDirty(true);
        }

        private void tbDynamicText_TextChanged(object sender, EventArgs e)
        {
            ListViewItem lvi = lvLines.SelectedItems[0];
            QuestLine ql = (QuestLine)lvi.Tag;
            
            ql.DynamicText = tbDynamicText.Text;
            lvi.SubItems[1].Text = Program.s_staticText.ContainsKey(ql.StaticText) ? Program.s_staticText[ql.StaticText].ToString() : ql.DynamicText;

            SetDirty(true);
        }
        #endregion
    }
}
