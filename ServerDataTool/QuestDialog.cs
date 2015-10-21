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
            cbStepOwner.Enabled = false;
            cbStepOwner.SelectedItem = null;

            lvRequirements.Items.Clear();
            lvRequirements.Enabled = false;
            cbRequirementType.Enabled = false;
            cbRequirementType.SelectedItem = null;
            tbRequirementParam.Enabled = false;
            tbRequirementParam.Text = "";
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
                    ListViewItem lvi = lvSteps.Items.Add(qs.Step.ToString());
                    lvi.SubItems.Add(qs.CompType.ToString());
                    lvi.Tag = qs;
                }

                // Set Requirements
                foreach (QuestRequirement qr in q.Requirements)
                {
                    ListViewItem lvi = lvRequirements.Items.Add(qr.TheType.ToString());
                    lvi.SubItems.Add(qr.Context.ToString());
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

        void SetDirty(bool dirty)
        {
            if(_selectingQuest )
                return;

            if( _selectedQuest != null )
                _selectedQuest.Dirty = dirty;
            
            bool anyQuestsDirty = false;
            foreach (ListViewItem lvi in lvQuests.Items)
            {
                Quest lvq = (Quest)lvi.Tag;
                if (lvq.Dirty)
                {
                    anyQuestsDirty = true;
                    break;
                }
            }
            
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
            Quest q = Database.AddQuest(tbQuestName.Text, giver == null ? 0 : giver.ID, giver == null ? (ushort)0 : giver.MapID);

            // Put it in the display
            AddQuest(q);
        }
        #endregion

        #region Steps
        private void lvSteps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSteps.SelectedItems.Count != 0)
            {
                ListViewItem lvi = lvSteps.SelectedItems[0];
                QuestStep qs = (QuestStep)lvi.Tag;
                
                cbCompletionType.SelectedIndex = (int)qs.CompType;
                tbCompletionCount.Text = qs.CompCount.ToString();
                tbCompletionTarget.Text = qs.CompTargetID.ToString();
                cbStepOwner.SelectedItem = _npcs[qs.OwnerID];
            }
        }

        private void cbCompletionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSteps.SelectedItems.Count != 0)
            {
                ListViewItem lvi = lvSteps.SelectedItems[0];
                QuestStep qs = (QuestStep)lvi.Tag;

                qs.CompType = (QuestStep.CompletionType)cbCompletionType.SelectedIndex;
                lvi.SubItems[1].Text = qs.CompType.ToString();

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

        private void cbStepOwner_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSteps.SelectedItems.Count != 0)
            {
                ListViewItem lvi = lvSteps.SelectedItems[0];
                QuestStep qs = (QuestStep)lvi.Tag;

                NPC owner = (NPC)cbStepOwner.SelectedItem;
                qs.OwnerID = owner.ID;

                _selectedQuest.Dirty = true;
            }
        }

        private void btnDeleteStep_Click(object sender, EventArgs e)
        {

        }

        private void btnStepUp_Click(object sender, EventArgs e)
        {

        }

        private void btnStepDown_Click(object sender, EventArgs e)
        {

        }

        private void btnNewStep_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Requirements
        private void lvRequirements_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvRequirements.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvRequirements.SelectedItems[0];
                QuestRequirement qr = (QuestRequirement)lvi.Tag;

                cbRequirementType.SelectedIndex = (int)qr.TheType;
                tbRequirementParam.Text = qr.Context.ToString();
            }
        }

        private void cbRequirementType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvRequirements.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvRequirements.SelectedItems[0];
                QuestRequirement qr = (QuestRequirement)lvi.Tag;

                qr.TheType = (QuestRequirement.Type)cbRequirementType.SelectedIndex;
                _selectedQuest.Dirty = true;
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
                    _selectedQuest.Dirty = true;
                }
                catch (Exception) { }
            }
        }

        private void btnDeleteRequirement_Click(object sender, EventArgs e)
        {

        }

        private void btnNewRequirement_Click(object sender, EventArgs e)
        {
            if (_selectedQuest != null)
            {
                // Create the requirement
                QuestRequirement qr = new QuestRequirement(QuestRequirement.Type.Level, 0);
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
        private void lvRewards_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tbGold_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbExp_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbFame_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbItem_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnDeleteReward_Click(object sender, EventArgs e)
        {

        }

        private void btnAddReward_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Lines
        private void lvLines_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnDeleteLine_Click(object sender, EventArgs e)
        {

        }

        private void btnLineUp_Click(object sender, EventArgs e)
        {

        }

        private void btnLineDown_Click(object sender, EventArgs e)
        {

        }

        private void btnNewLine_Click(object sender, EventArgs e)
        {

        }

        private void cbIcon_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbStaticText_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tbDynamicText_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion

    }
}
