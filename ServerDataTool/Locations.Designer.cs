namespace ServerDataTool
{
    partial class Locations
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
            this.components = new System.ComponentModel.Container();
            this.label2 = new System.Windows.Forms.Label();
            this.gbLocationInfo = new System.Windows.Forms.GroupBox();
            this.tbRadius = new System.Windows.Forms.TextBox();
            this.tbY = new System.Windows.Forms.TextBox();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbX = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pbMap = new System.Windows.Forms.PictureBox();
            this.cbMapID = new System.Windows.Forms.ComboBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lvLocations = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbLocationInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMap)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(533, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Locations";
            // 
            // gbLocationInfo
            // 
            this.gbLocationInfo.Controls.Add(this.tbRadius);
            this.gbLocationInfo.Controls.Add(this.tbY);
            this.gbLocationInfo.Controls.Add(this.tbName);
            this.gbLocationInfo.Controls.Add(this.tbX);
            this.gbLocationInfo.Controls.Add(this.label7);
            this.gbLocationInfo.Controls.Add(this.label5);
            this.gbLocationInfo.Controls.Add(this.label4);
            this.gbLocationInfo.Controls.Add(this.label3);
            this.gbLocationInfo.Location = new System.Drawing.Point(533, 423);
            this.gbLocationInfo.Name = "gbLocationInfo";
            this.gbLocationInfo.Size = new System.Drawing.Size(219, 126);
            this.gbLocationInfo.TabIndex = 10;
            this.gbLocationInfo.TabStop = false;
            this.gbLocationInfo.Text = "Location Info";
            // 
            // tbRadius
            // 
            this.tbRadius.Location = new System.Drawing.Point(73, 97);
            this.tbRadius.Name = "tbRadius";
            this.tbRadius.Size = new System.Drawing.Size(140, 20);
            this.tbRadius.TabIndex = 9;
            this.tbRadius.TextChanged += new System.EventHandler(this.tbRadius_TextChanged);
            // 
            // tbY
            // 
            this.tbY.Location = new System.Drawing.Point(73, 71);
            this.tbY.Name = "tbY";
            this.tbY.Size = new System.Drawing.Size(140, 20);
            this.tbY.TabIndex = 8;
            this.tbY.TextChanged += new System.EventHandler(this.tbY_TextChanged);
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(73, 19);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(140, 20);
            this.tbName.TabIndex = 7;
            this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
            // 
            // tbX
            // 
            this.tbX.Location = new System.Drawing.Point(73, 45);
            this.tbX.Name = "tbX";
            this.tbX.Size = new System.Drawing.Size(140, 20);
            this.tbX.TabIndex = 7;
            this.tbX.TextChanged += new System.EventHandler(this.tbX_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 104);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Radius:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(50, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Y:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "X:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Name:";
            // 
            // pbMap
            // 
            this.pbMap.Location = new System.Drawing.Point(15, 37);
            this.pbMap.Name = "pbMap";
            this.pbMap.Size = new System.Drawing.Size(512, 512);
            this.pbMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMap.TabIndex = 9;
            this.pbMap.TabStop = false;
            this.pbMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbMap_MouseClick);
            // 
            // cbMapID
            // 
            this.cbMapID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMapID.FormattingEnabled = true;
            this.cbMapID.Location = new System.Drawing.Point(63, 10);
            this.cbMapID.MaxDropDownItems = 20;
            this.cbMapID.Name = "cbMapID";
            this.cbMapID.Size = new System.Drawing.Size(121, 21);
            this.cbMapID.TabIndex = 7;
            this.cbMapID.SelectedIndexChanged += new System.EventHandler(this.cbMapID_SelectedIndexChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(18, 552);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(35, 13);
            this.lblStatus.TabIndex = 11;
            this.lblStatus.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Map ID:";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addLocationToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(146, 26);
            // 
            // addLocationToolStripMenuItem
            // 
            this.addLocationToolStripMenuItem.Name = "addLocationToolStripMenuItem";
            this.addLocationToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.addLocationToolStripMenuItem.Text = "Add Location";
            this.addLocationToolStripMenuItem.Click += new System.EventHandler(this.addLocationToolStripMenuItem_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteLocationToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(157, 26);
            // 
            // deleteLocationToolStripMenuItem
            // 
            this.deleteLocationToolStripMenuItem.Name = "deleteLocationToolStripMenuItem";
            this.deleteLocationToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.deleteLocationToolStripMenuItem.Text = "Delete Location";
            this.deleteLocationToolStripMenuItem.Click += new System.EventHandler(this.deleteLocationToolStripMenuItem_Click);
            // 
            // lvLocations
            // 
            this.lvLocations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvLocations.FullRowSelect = true;
            this.lvLocations.GridLines = true;
            this.lvLocations.Location = new System.Drawing.Point(533, 37);
            this.lvLocations.Name = "lvLocations";
            this.lvLocations.Size = new System.Drawing.Size(219, 380);
            this.lvLocations.TabIndex = 16;
            this.lvLocations.UseCompatibleStateImageBehavior = false;
            this.lvLocations.View = System.Windows.Forms.View.Details;
            this.lvLocations.SelectedIndexChanged += new System.EventHandler(this.lvLocations_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            this.columnHeader1.Width = 37;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 144;
            // 
            // Locations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(767, 570);
            this.Controls.Add(this.lvLocations);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gbLocationInfo);
            this.Controls.Add(this.pbMap);
            this.Controls.Add(this.cbMapID);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Locations";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Locations";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Locations_FormClosing);
            this.Load += new System.EventHandler(this.Locations_Load);
            this.gbLocationInfo.ResumeLayout(false);
            this.gbLocationInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMap)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbLocationInfo;
        private System.Windows.Forms.TextBox tbRadius;
        private System.Windows.Forms.TextBox tbY;
        private System.Windows.Forms.TextBox tbX;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pbMap;
        private System.Windows.Forms.ComboBox cbMapID;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addLocationToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem deleteLocationToolStripMenuItem;
        private System.Windows.Forms.ListView lvLocations;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}