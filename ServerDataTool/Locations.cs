using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace ServerDataTool
{
    public partial class Locations : Form
    {
        int _mapSize;
        List<PictureBox> _mapMarkers;
        ListViewItem _currentSelection;
        ushort _mapId;
        bool _dragging;
        PictureBox _deleteContext;
        Point _contextClickPoint;


        public Locations()
        {
            _mapMarkers = new List<PictureBox>();
            InitializeComponent();
        }

        private void Locations_Load(object sender, EventArgs e)
        {
            ushort[] maps = Database.FetchMaps();
            foreach (ushort mapid in maps)
            {
                cbMapID.Items.Add("map" + mapid.ToString("D2"));
            }

            cbMapID.SelectedIndex = 0;
            cbMapID.Focus();
        }

        private void cbMapID_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveDirtyLocations();
            foreach (PictureBox pb in _mapMarkers)
            {
                Controls.Remove(pb);
            }
            _mapMarkers.Clear();
            lvLocations.Items.Clear();
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

            // Get the locations on this map
            string mapIDStr = mapImageName.Substring(3, 2);
            _mapId = Convert.ToUInt16(mapIDStr);
            Location[] locations = Database.FetchLocations(_mapId);

            // Put them in the list and the map
            foreach (Location loc in locations)
            {
                AddLocation(loc);
            }
        }

        void SaveDirtyLocations()
        {
            foreach (ListViewItem lvi in lvLocations.Items)
            {
                Location loc = (Location)lvi.Tag;
                if (loc.Dirty)
                {
                    Database.UpdateLocation(loc);
                }
            }
        }

        void AddLocation(Location loc)
        {
            // Add npc to the list
            ListViewItem lvi = lvLocations.Items.Add(loc.ID.ToString());
            lvi.SubItems.Add(loc.Name);
            lvi.Tag = loc;

            // Add npc to the map
            PictureBox pb = new PictureBox();
            SetMapMarkerDark(pb, loc);
            _mapMarkers.Add(pb);
            Controls.Add(pb);
            pb.BringToFront();
            pb.Tag = lvi;
            pb.MouseClick += Pb_MouseClick;
            pb.MouseMove += Pb_MouseMove;
        }

        void SelectLocation(ListViewItem lvi)
        {
            // Revert selected map marker
            if (_currentSelection != null)
            {
                PictureBox pb = FindPictureBox(_currentSelection);
                SetMapMarkerDark(pb, (Location)_currentSelection.Tag);
            }

            _currentSelection = lvi;
            if (lvi != null)
            {
                // Set the Location info
                Location loc = (Location)lvi.Tag;
                tbName.Text = loc.Name;
                tbX.Text = loc.X.ToString();
                tbY.Text = loc.Y.ToString();
                tbRadius.Text = loc.Radius.ToString();

                // Change the map marker box color and size
                PictureBox pb = FindPictureBox(lvi);
                SetMapMarkerLight(pb, loc);

                lvLocations.Focus();
            }
            else
            {
                tbName.Text = "";
                tbX.Text = "";
                tbY.Text = "";
                tbRadius.Text = "";
            }
        }

        void SetStatus(string status)
        {
            lblStatus.Text = status;
        }

        private void lvLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvLocations.SelectedItems.Count > 0)
                SelectLocation(lvLocations.SelectedItems[0]);
            else
                SelectLocation(null);
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            if (_currentSelection != null)
            {
                Location loc = (Location)_currentSelection.Tag;
                loc.Name = tbName.Text;
                loc.Dirty = true;

                _currentSelection.SubItems[1].Text = loc.Name;
            }
        }

        private void tbX_TextChanged(object sender, EventArgs e)
        {
            if (_currentSelection != null && !_dragging)
            {
                Location loc = (Location)_currentSelection.Tag;
                loc.X = Convert.ToUInt32(tbX.Text);

                SetMapMarkerLight(FindPictureBox(_currentSelection), loc);
                loc.Dirty = true;
            }
        }

        private void tbY_TextChanged(object sender, EventArgs e)
        {
            if (_currentSelection != null && !_dragging)
            {
                Location loc = (Location)_currentSelection.Tag;
                loc.Y = Convert.ToUInt32(tbY.Text);
                SetMapMarkerLight(FindPictureBox(_currentSelection), loc);
                loc.Dirty = true;
            }
        }

        private void tbRadius_TextChanged(object sender, EventArgs e)
        {
            if (_currentSelection != null)
            {
                Location loc = (Location)_currentSelection.Tag;
                try
                {
                    loc.Radius = Math.Max(0, Convert.ToUInt32(tbRadius.Text));
                    SetMapMarkerLight(FindPictureBox(_currentSelection), loc);
                    loc.Dirty = true;
                }
                catch (Exception) { }
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

        private void Locations_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool anyDirty = false;
            foreach (ListViewItem lvi in lvLocations.Items)
            {
                Location loc = (Location)lvi.Tag;
                if (loc.Dirty)
                {
                    anyDirty = true;
                    break;
                }
            }

            if (anyDirty)
            {
                DialogResult res = MessageBox.Show("There are unsaved changes.\nWould you like to save them now?", "Save", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Yes)
                    SaveDirtyLocations();
                else if (res == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

        PictureBox FindPictureBox(ListViewItem lvi)
        {
            foreach (PictureBox pb in _mapMarkers)
            {
                if (pb.Tag == lvi)
                    return pb;
            }
            return null;
        }

        void SetMapMarkerDark(PictureBox pb, Location loc)
        {
            int mapScale = 512 / _mapSize;
            int X = (int)loc.X;
            int Y = (int)loc.Y;
            int size = 6 + (int)loc.Radius;
            int halfSize = size >> 1;
            pb.Size = new Size(size, size);
            int x = (X * mapScale) - halfSize;
            int y = (512 - (Y * mapScale)) - halfSize;
            pb.Location = new Point(pbMap.Location.X + x, pbMap.Location.Y + y);
            pb.BackColor = Color.DarkBlue;
        }

        void SetMapMarkerLight(PictureBox pb, Location loc)
        {
            int mapScale = 512 / _mapSize;
            int X = (int)loc.X;
            int Y = (int)loc.Y;
            int size = 6 + (int)loc.Radius;
            int halfSize = size >> 1;
            pb.Size = new Size(size, size);
            int x = (X * mapScale) - halfSize;
            int y = (512 - (Y * mapScale)) - halfSize;
            pb.Location = new Point(pbMap.Location.X + x, pbMap.Location.Y + y);
            pb.BackColor = Color.LightBlue;
        }

        private void Pb_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                PictureBox pb = (PictureBox)sender;
                ListViewItem lvi = (ListViewItem)pb.Tag;
                Location loc = (Location)(lvi.Tag);

                int locSize = 6 + (int)loc.Radius;
                int halfSize = -(locSize >> 1);

                pb.Location = new Point(pb.Location.X + e.X + halfSize, pb.Location.Y + e.Y + halfSize);

                int mapScale = 512 / _mapSize;
                loc.X = (uint)(((pb.Location.X - halfSize) - pbMap.Location.X) / mapScale);
                loc.Y = (uint)((512 - ((pb.Location.Y - halfSize) - pbMap.Location.Y)) / mapScale);
                loc.Dirty = true;

                if (lvi == _currentSelection)
                {
                    _dragging = true;
                    tbX.Text = loc.X.ToString();
                    tbY.Text = loc.Y.ToString();
                    _dragging = false;
                }
            }
        }

        private void Pb_MouseClick(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // Select NPC
                PictureBox pb = (PictureBox)sender;
                SelectLocation((ListViewItem)pb.Tag);
            }
            else if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
            {
                _deleteContext = (PictureBox)sender;
                contextMenuStrip2.Show((Control)sender, e.X, e.Y);
            }
        }

        Point ImageToWorld(Point image)
        {

            int centerOffset = -4;

            int mapScale = 512 / _mapSize;
            Point world = new Point();
            world.X = (((image.X - centerOffset) - pbMap.Location.X) / mapScale);
            world.Y = ((512 - ((image.Y - centerOffset) - pbMap.Location.Y)) / mapScale);
            return world;
        }

        private void addLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create an NPC here and get the data from the database
            Point world = ImageToWorld(_contextClickPoint);
            Location loc = Database.CreateLocation((uint)world.X, (uint)world.Y, _mapId);
            AddLocation(loc);
        }

        private void deleteLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = (ListViewItem)_deleteContext.Tag;

            DeleteLocation(lvi);
        }

        void DeleteLocation(ListViewItem del)
        {
            if (MessageBox.Show("Are you sure you want to delete this location?", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                // Remove the location from the database                
                Database.DeleteLocation((Location)del.Tag);

                // Deselect the npc if it is selected
                if (del == _currentSelection)
                    SelectLocation(null);

                // Remove the npc from the map
                PictureBox pb = FindPictureBox(del);
                Controls.Remove(pb);
                _mapMarkers.Remove(pb);
                pb.Tag = null;

                // Remove the npc from the listview
                lvLocations.Items.Remove(del);
                del.Tag = null;
            }
        }
    }
}
