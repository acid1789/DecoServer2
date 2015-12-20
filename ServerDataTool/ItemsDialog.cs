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
    public partial class ItemsDialog : Form
    {
        ItemTemplate[] _items;

        public ItemsDialog()
        {
            InitializeComponent();
        }

        private void ItemsDialog_Load(object sender, EventArgs e)
        {
            foreach (IntStrID item in Program.s_items.Values)
                cbModel.Items.Add(item);
            
            cbType.DataSource = Enum.GetValues(typeof(ItemTemplate.ItemType));

            _items = Database.FetchItems();
            foreach (ItemTemplate it in _items)
            {
                AddItem(it);
            }

            btnDeleteItem.Enabled = false;
        }

        void AddItem(ItemTemplate it)
        {
            string itModelName = Program.s_items.ContainsKey(it.Model) ? Program.s_items[it.Model].ToString() : "Unknown";
            ListViewItem lvi = lvItems.Items.Add(itModelName);
            lvi.SubItems.Add(it.Type.ToString());
            lvi.SubItems.Add(it.GenDQMin.ToString());
            lvi.SubItems.Add(it.GenDQMax.ToString());
            lvi.SubItems.Add(it.DQMax.ToString());
            lvi.Tag = it;
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this item template?", "Delete Item", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    ListViewItem lvi = lvItems.SelectedItems[0];
                    ItemTemplate it = (ItemTemplate)lvi.Tag;

                    lvItems.Items.Remove(lvi);
                    Database.DeleteItem(it);
                }
            }
        }

        private void btnNewItem_Click(object sender, EventArgs e)
        {
            byte genDQMin = 0;
            byte genDQMax = 0;
            byte dqMax = 0;
            try { genDQMin = Convert.ToByte(tbDurabilityMin.Text); } catch (Exception) { }
            try { genDQMax = Convert.ToByte(tbDurabilityMax.Text); } catch (Exception) { }
            try { dqMax = Convert.ToByte(tbDurationMax.Text); } catch (Exception) { }

            ItemTemplate it = new ItemTemplate(0, (ushort)Math.Max(cbModel.SelectedIndex, 0), (byte)Math.Max(cbType.SelectedIndex, 0), genDQMin, genDQMax, dqMax);
            it.New = true;
            AddItem(it);
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDeleteItem.Enabled = false;

            if (lvItems.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvItems.SelectedItems[0];
                ItemTemplate it = (ItemTemplate)lvi.Tag;
                IntStrID model = Program.s_items.ContainsKey(it.Model) ? Program.s_items[it.Model] : null;
                cbModel.SelectedItem = model;
                cbType.SelectedIndex = (int)it.Type;
                tbDurabilityMin.Text = it.GenDQMin.ToString();
                tbDurabilityMax.Text = it.GenDQMax.ToString();
                tbDurationMax.Text = it.DQMax.ToString();

                btnDeleteItem.Enabled = true;
            }
        }        

        private void cbModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count > 0 && cbModel.SelectedItem != null)
            {
                ListViewItem lvi = lvItems.SelectedItems[0];
                ItemTemplate it = (ItemTemplate)lvi.Tag;

                IntStrID model = (IntStrID)cbModel.SelectedItem;
                it.Model = (ushort)model.ID;
                lvi.Text = model.ToString();

                it.Dirty = true;
            }
        }

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvItems.SelectedItems[0];
                ItemTemplate it = (ItemTemplate)lvi.Tag;

                it.Type = (ItemTemplate.ItemType)cbType.SelectedIndex;
                lvi.SubItems[1].Text = cbType.SelectedItem.ToString();

                it.Dirty = true;
            }
        }

        private void tbDurabilityMin_TextChanged(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvItems.SelectedItems[0];
                ItemTemplate it = (ItemTemplate)lvi.Tag;

                try
                {
                    it.GenDQMin = Convert.ToByte(tbDurabilityMin.Text);
                    lvi.SubItems[2].Text = tbDurabilityMin.Text;
                    it.Dirty = true;
                }
                catch (Exception) { }
            }
        }

        private void tbDurabilityMax_TextChanged(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvItems.SelectedItems[0];
                ItemTemplate it = (ItemTemplate)lvi.Tag;

                try
                {
                    it.GenDQMax = Convert.ToByte(tbDurabilityMax.Text);
                    lvi.SubItems[3].Text = tbDurabilityMax.Text;
                    it.Dirty = true;
                }
                catch (Exception) { }
            }
        }

        private void tbDurationMax_TextChanged(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count > 0)
            {
                ListViewItem lvi = lvItems.SelectedItems[0];
                ItemTemplate it = (ItemTemplate)lvi.Tag;

                try
                {
                    it.DQMax = Convert.ToByte(tbDurationMax.Text);
                    lvi.SubItems[5].Text = tbDurationMax.Text;
                    it.Dirty = true;
                }
                catch (Exception) { }
            }
        }

        private void ItemsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool anyDirty = false;
            foreach (ListViewItem lvi in lvItems.Items)
            {
                ItemTemplate it = (ItemTemplate)lvi.Tag;
                if (it.Dirty)
                {
                    anyDirty = true;
                    break;
                }
            }

            if (anyDirty)
            {
                DialogResult res = MessageBox.Show("There are unsaved changes. Would you like to save them now?", "Save Items", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Yes)
                {
                    foreach (ListViewItem lvi in lvItems.Items)
                    {
                        ItemTemplate it = (ItemTemplate)lvi.Tag;
                        if (it.Dirty)
                        {
                            Database.SaveItem(it);
                        }
                    }
                }
                else if( res == DialogResult.Cancel )
                    e.Cancel = true;
            }
        }
    }
}
