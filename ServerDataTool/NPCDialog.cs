using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace ServerDataTool
{
    public partial class NPCDialog : Form
    {
        int _mapSize;
        List<PictureBox> _mapMarkers;
        ListViewItem _currentSelection;
        ushort _mapId;
        Point _contextClickPoint;
        PictureBox _deleteContext;

        public NPCDialog()
        {
            _mapMarkers = new List<PictureBox>();
            InitializeComponent();
        }

        private void NPCDialog_Load(object sender, EventArgs e)
        {
            ushort[] maps = Database.FetchMaps();
            foreach (ushort mapid in maps)
            {
                cbMapID.Items.Add("map" + mapid.ToString("D2"));
            }

            cbMapID.SelectedIndex = 0;
            cbMapID.Focus();

            FormClosing += NPCDialog_FormClosing;
        }

        private void NPCDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool anyDirty = false;
            foreach (ListViewItem lvi in lvNPCs.Items)
            {
                NPC npc = (NPC)lvi.Tag;
                if (npc.Dirty)
                {
                    anyDirty = true;
                    break;
                }
            }

            if (anyDirty)
            {
                DialogResult res = MessageBox.Show("There are unsaved changes.\nWould you like to save them now?", "Save", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Yes)
                    SaveDirtyNPCs();
                else if( res == DialogResult.Cancel )
                    e.Cancel = true;
            }
        }

        private void cbMapID_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveDirtyNPCs();
            foreach (PictureBox pb in _mapMarkers)
            {
                Controls.Remove(pb);
            }
            _mapMarkers.Clear();
            lvNPCs.Clear();
            _currentSelection = null;

            string mapImageName = (string)cbMapID.SelectedItem;
            
            string dir = "MapImages/";
            if (!Directory.Exists(dir))
            {
                dir = "../../MapImages/";
                if (!Directory.Exists(dir))
                {
                    SetStatus("Cant find MapImages folder");
                    return;
                }
            }

            _mapSize = 512;
            string filePath = dir + mapImageName + "_512.png";
            if (!File.Exists(filePath))
            {
                _mapSize = 256;
                filePath = dir + mapImageName + "_256.png";
                if (!File.Exists(filePath))
                {
                    _mapSize = 128;
                    filePath = dir + mapImageName + "_128.png";
                    if (!File.Exists(filePath))
                    {
                        SetStatus("Missing map file: " + mapImageName);
                        return;
                    }
                }
            }

            // Load the map image
            pbMap.ImageLocation = filePath;
            SetStatus(string.Format("MapSize: {0} x {0}", _mapSize));

            // Get the NPCs on this map
            string mapIDStr = mapImageName.Substring(3, 2);
            _mapId = Convert.ToUInt16(mapIDStr);
            NPC[] npcs = Database.FetchNPCs(_mapId);

            // Put them in the list and the map
            foreach (NPC npc in npcs)
            {
                AddNPC(npc);
            }
        }

        void SaveDirtyNPCs()
        {
            foreach (ListViewItem lvi in lvNPCs.Items)
            {
                NPC npc = (NPC)lvi.Tag;
                if (npc.Dirty)
                {
                    Database.UpdateNPC(npc);
                }
            }
        }

        bool dragging;
        private void Pb_MouseMove(object sender, MouseEventArgs e)
        {            
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                PictureBox pb = (PictureBox)sender;
                ListViewItem lvi = (ListViewItem)pb.Tag;
                NPC npc = (NPC)(lvi.Tag);

                int centerOffset = -4;
                if (lvi == _currentSelection)
                    centerOffset = -7;

                pb.Location = new Point(pb.Location.X + e.X + centerOffset, pb.Location.Y + e.Y + centerOffset);

                int mapScale = 512 / _mapSize;
                npc.X = (uint)(((pb.Location.X - centerOffset) - pbMap.Location.X) / mapScale);
                npc.Y = (uint)((512 - ((pb.Location.Y - centerOffset) - pbMap.Location.Y)) / mapScale);
                npc.Dirty = true;

                if (lvi == _currentSelection)
                {
                    dragging = true;
                    tbX.Text = npc.X.ToString();
                    tbY.Text = npc.Y.ToString();
                    dragging = false;
                }
            }
        }

        private void Pb_MouseClick(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // Select NPC
                PictureBox pb = (PictureBox)sender;
                SelectNPC((ListViewItem)pb.Tag);
            }
            else if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                _deleteContext = (PictureBox)sender;
                contextMenuStrip2.Show((Control)sender, e.X, e.Y);
            }

        }

        void SetStatus(string status)
        {
            lblStatus.Text = status;
        }

        private void lvNPCs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvNPCs.SelectedItems.Count > 0)
            {
                SelectNPC(lvNPCs.SelectedItems[0]);
            }
        }

        void AddNPC(NPC npc)
        {
            // Add npc to the list
            ListViewItem lvi = lvNPCs.Items.Add(npc.ToString());
            lvi.Tag = npc;

            // Add npc to the map
            PictureBox pb = new PictureBox();
            SetMapMarkerRed(pb, npc);
            _mapMarkers.Add(pb);
            Controls.Add(pb);
            pb.BringToFront();
            pb.Tag = lvi;
            pb.MouseClick += Pb_MouseClick;
            pb.MouseMove += Pb_MouseMove;
        }

        void SelectNPC(ListViewItem lvi)
        {
            // Revert selected map marker
            if (_currentSelection != null)
            {
                PictureBox pb = FindPictureBox(_currentSelection);
                SetMapMarkerRed(pb, (NPC)_currentSelection.Tag);
            }

            _currentSelection = lvi;
            if (lvi != null)
            {
                // Set the NPC info
                NPC npc = (NPC)lvi.Tag;
                tbGameID.Text = npc.GameID.ToString();
                tbHP.Text = npc.HP.ToString();
                tbX.Text = npc.X.ToString();
                tbY.Text = npc.Y.ToString();
                tbDirection.Text = npc.Direction.ToString();

                // Change the map marker box color and size
                PictureBox pb = FindPictureBox(lvi);
                SetMapMarkerGreen(pb, npc);

                lvNPCs.Focus();
            }
            else
            {
                tbGameID.Text = "";
                tbHP.Text = "";
                tbX.Text = "";
                tbY.Text = "";
                tbDirection.Text = "";
            }
        }

        PictureBox FindPictureBox(ListViewItem lvi)
        {
            foreach (PictureBox pb in _mapMarkers)
            {
                if( pb.Tag == lvi )
                    return pb;
            }
            return null;
        }

        void SetMapMarkerRed(PictureBox pb, NPC npc)
        {
            int mapScale = 512 / _mapSize;
            int X = (int)npc.X;
            int Y = (int)npc.Y;
            pb.Size = new Size(8, 8);
            int x = (X * mapScale) - 4;
            int y = (512 - (Y * mapScale)) - 4;
            pb.Location = new Point(pbMap.Location.X + x, pbMap.Location.Y + y);
            pb.BackColor = Color.Red;
        }

        void SetMapMarkerGreen(PictureBox pb, NPC npc)
        {
            int mapScale = 512 / _mapSize;
            int X = (int)npc.X;
            int Y = (int)npc.Y;
            pb.Size = new Size(14, 14);
            int x = (X * mapScale) - 7;
            int y = (512 - (Y * mapScale)) - 7;
            pb.Location = new Point(pbMap.Location.X + x, pbMap.Location.Y + y);
            pb.BackColor = Color.LightGreen;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {            
        }

        private void tbGameID_TextChanged(object sender, EventArgs e)
        {
            if (_currentSelection != null)
            {
                NPC npc = (NPC)_currentSelection.Tag;
                npc.GameID = Convert.ToUInt16(tbGameID.Text);
                _currentSelection.Text = npc.ToString();
                npc.Dirty = true;
            }
        }

        private void tbHP_TextChanged(object sender, EventArgs e)
        {
            if (_currentSelection != null)
            {
                NPC npc = (NPC)_currentSelection.Tag;
                npc.HP = Convert.ToUInt32(tbHP.Text);
                npc.Dirty = true;
            }
        }

        private void tbX_TextChanged(object sender, EventArgs e)
        {
            if (_currentSelection != null && !dragging)
            {
                NPC npc = (NPC)_currentSelection.Tag;
                npc.X = Convert.ToUInt32(tbX.Text);

                SetMapMarkerGreen(FindPictureBox(_currentSelection), npc);
                npc.Dirty = true;
            }
        }

        private void tbY_TextChanged(object sender, EventArgs e)
        {
            if (_currentSelection != null && !dragging)
            {
                NPC npc = (NPC)_currentSelection.Tag;
                npc.Y = Convert.ToUInt32(tbY.Text);
                SetMapMarkerGreen(FindPictureBox(_currentSelection), npc);
                npc.Dirty = true;
            }
        }

        private void tbDirection_TextChanged(object sender, EventArgs e)
        {
            if (_currentSelection != null)
            {
                NPC npc = (NPC)_currentSelection.Tag;
                npc.Direction = Convert.ToUInt32(tbDirection.Text);
                npc.Dirty = true;
            }
        }

        private void pbMap_MouseClick(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                _contextClickPoint = PointToClient(pbMap.PointToScreen(new Point(e.X, e.Y)));
                contextMenuStrip1.Show((Control)sender, e.X, e.Y);
            }
        }

        Point ImageToWorld(Point image, bool selected)
        {
            int centerOffset = -4;
            if (selected)
                centerOffset = -7;

            int mapScale = 512 / _mapSize;
            Point world = new Point();
            world.X = (((image.X - centerOffset) - pbMap.Location.X) / mapScale);
            world.Y = ((512 - ((image.Y - centerOffset) - pbMap.Location.Y)) / mapScale);
            return world;
        }

        private void addNPCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an NPC here and get the data from the database
            Point world = ImageToWorld(_contextClickPoint, false);
            NPC npc = Database.CreateNPC(world.X, world.Y, _mapId);
            AddNPC(npc);
        }

        private void deleteNPCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = (ListViewItem)_deleteContext.Tag;

            DeleteNPC(lvi);
        }

        void DeleteNPC(ListViewItem del)
        {
            if (MessageBox.Show("Are you sure you want to delete this NPC?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Remove the npc from the database
                Database.DeleteNPC((NPC)del.Tag);

                // Deselect the npc if it is selected
                if( del == _currentSelection )
                    SelectNPC(null);

                // Remove the npc from the map
                PictureBox pb = FindPictureBox(del);
                Controls.Remove(pb);
                _mapMarkers.Remove(pb);
                pb.Tag = null;

                // Remove the npc from the listview
                lvNPCs.Items.Remove(del);
                del.Tag = null;
            }
        }

        private void NPCDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (_currentSelection != null)
                {
                    DeleteNPC(_currentSelection);
                }
            }
        }

        private void cbMapID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (_currentSelection != null)
                {
                    DeleteNPC(_currentSelection);
                }
            }
        }

        private void lvNPCs_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (_currentSelection != null)
                {
                    DeleteNPC(_currentSelection);
                }
            }
        }
    }
}
