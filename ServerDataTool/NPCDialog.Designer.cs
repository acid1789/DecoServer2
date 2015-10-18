namespace ServerDataTool
{
    partial class NPCDialog
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
            this.cbMapID = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pbMap = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbGameID = new System.Windows.Forms.ComboBox();
            this.tbDirection = new System.Windows.Forms.TextBox();
            this.tbY = new System.Windows.Forms.TextBox();
            this.tbX = new System.Windows.Forms.TextBox();
            this.tbHP = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lvNPCs = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addNPCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteNPCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pbMap)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbMapID
            // 
            this.cbMapID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMapID.FormattingEnabled = true;
            this.cbMapID.Location = new System.Drawing.Point(57, 12);
            this.cbMapID.MaxDropDownItems = 20;
            this.cbMapID.Name = "cbMapID";
            this.cbMapID.Size = new System.Drawing.Size(121, 21);
            this.cbMapID.TabIndex = 0;
            this.cbMapID.SelectedIndexChanged += new System.EventHandler(this.cbMapID_SelectedIndexChanged);
            this.cbMapID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbMapID_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Map ID:";
            // 
            // pbMap
            // 
            this.pbMap.Location = new System.Drawing.Point(9, 39);
            this.pbMap.Name = "pbMap";
            this.pbMap.Size = new System.Drawing.Size(512, 512);
            this.pbMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbMap.TabIndex = 2;
            this.pbMap.TabStop = false;
            this.pbMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbMap_MouseClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbGameID);
            this.groupBox1.Controls.Add(this.tbDirection);
            this.groupBox1.Controls.Add(this.tbY);
            this.groupBox1.Controls.Add(this.tbX);
            this.groupBox1.Controls.Add(this.tbHP);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(527, 401);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(219, 150);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "NPC Info";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // cbGameID
            // 
            this.cbGameID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGameID.FormattingEnabled = true;
            this.cbGameID.Location = new System.Drawing.Point(73, 18);
            this.cbGameID.Name = "cbGameID";
            this.cbGameID.Size = new System.Drawing.Size(140, 21);
            this.cbGameID.TabIndex = 10;
            this.cbGameID.SelectedIndexChanged += new System.EventHandler(this.cbGameID_SelectedIndexChanged);
            // 
            // tbDirection
            // 
            this.tbDirection.Location = new System.Drawing.Point(73, 123);
            this.tbDirection.Name = "tbDirection";
            this.tbDirection.Size = new System.Drawing.Size(140, 20);
            this.tbDirection.TabIndex = 9;
            this.tbDirection.TextChanged += new System.EventHandler(this.tbDirection_TextChanged);
            // 
            // tbY
            // 
            this.tbY.Location = new System.Drawing.Point(73, 97);
            this.tbY.Name = "tbY";
            this.tbY.Size = new System.Drawing.Size(140, 20);
            this.tbY.TabIndex = 8;
            this.tbY.TextChanged += new System.EventHandler(this.tbY_TextChanged);
            // 
            // tbX
            // 
            this.tbX.Location = new System.Drawing.Point(73, 71);
            this.tbX.Name = "tbX";
            this.tbX.Size = new System.Drawing.Size(140, 20);
            this.tbX.TabIndex = 7;
            this.tbX.TextChanged += new System.EventHandler(this.tbX_TextChanged);
            // 
            // tbHP
            // 
            this.tbHP.Location = new System.Drawing.Point(73, 45);
            this.tbHP.Name = "tbHP";
            this.tbHP.Size = new System.Drawing.Size(140, 20);
            this.tbHP.TabIndex = 6;
            this.tbHP.TextChanged += new System.EventHandler(this.tbHP_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Direction:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(42, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "HP:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(50, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Y:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "X:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "GameID:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 554);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(35, 13);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "label2";
            // 
            // lvNPCs
            // 
            this.lvNPCs.Location = new System.Drawing.Point(527, 39);
            this.lvNPCs.Name = "lvNPCs";
            this.lvNPCs.Size = new System.Drawing.Size(219, 356);
            this.lvNPCs.TabIndex = 5;
            this.lvNPCs.UseCompatibleStateImageBehavior = false;
            this.lvNPCs.View = System.Windows.Forms.View.List;
            this.lvNPCs.SelectedIndexChanged += new System.EventHandler(this.lvNPCs_SelectedIndexChanged);
            this.lvNPCs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvNPCs_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(527, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "NPCs";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNPCToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(124, 26);
            // 
            // addNPCToolStripMenuItem
            // 
            this.addNPCToolStripMenuItem.Name = "addNPCToolStripMenuItem";
            this.addNPCToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.addNPCToolStripMenuItem.Text = "Add NPC";
            this.addNPCToolStripMenuItem.Click += new System.EventHandler(this.addNPCToolStripMenuItem_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteNPCToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(135, 26);
            // 
            // deleteNPCToolStripMenuItem
            // 
            this.deleteNPCToolStripMenuItem.Name = "deleteNPCToolStripMenuItem";
            this.deleteNPCToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.deleteNPCToolStripMenuItem.Text = "Delete NPC";
            this.deleteNPCToolStripMenuItem.Click += new System.EventHandler(this.deleteNPCToolStripMenuItem_Click);
            // 
            // NPCDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 573);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lvNPCs);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pbMap);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbMapID);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NPCDialog";
            this.Text = "NPCs";
            this.Load += new System.EventHandler(this.NPCDialog_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NPCDialog_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pbMap)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbMapID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbMap;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ListView lvNPCs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbDirection;
        private System.Windows.Forms.TextBox tbY;
        private System.Windows.Forms.TextBox tbX;
        private System.Windows.Forms.TextBox tbHP;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addNPCToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem deleteNPCToolStripMenuItem;
        private System.Windows.Forms.ComboBox cbGameID;
    }
}