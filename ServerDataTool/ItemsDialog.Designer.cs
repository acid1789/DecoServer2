namespace ServerDataTool
{
    partial class ItemsDialog
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbModel = new System.Windows.Forms.ComboBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.tbDurabilityMin = new System.Windows.Forms.TextBox();
            this.tbDurabilityMax = new System.Windows.Forms.TextBox();
            this.tbDurationMin = new System.Windows.Forms.TextBox();
            this.tbDurationMax = new System.Windows.Forms.TextBox();
            this.lvItems = new System.Windows.Forms.ListView();
            this.btnDeleteItem = new System.Windows.Forms.Button();
            this.btnNewItem = new System.Windows.Forms.Button();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 265);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Model:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(515, 265);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Type:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 291);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Minimum Durability:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 317);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Maximum Durability:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(455, 291);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Minimum Duration:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(452, 317);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Maximum Duration:";
            // 
            // cbModel
            // 
            this.cbModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbModel.FormattingEnabled = true;
            this.cbModel.Location = new System.Drawing.Point(109, 257);
            this.cbModel.Name = "cbModel";
            this.cbModel.Size = new System.Drawing.Size(400, 21);
            this.cbModel.TabIndex = 6;
            this.cbModel.SelectedIndexChanged += new System.EventHandler(this.cbModel_SelectedIndexChanged);
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.FormattingEnabled = true;
            this.cbType.Location = new System.Drawing.Point(555, 257);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(114, 21);
            this.cbType.TabIndex = 6;
            this.cbType.SelectedIndexChanged += new System.EventHandler(this.cbType_SelectedIndexChanged);
            // 
            // tbDurabilityMin
            // 
            this.tbDurabilityMin.Location = new System.Drawing.Point(109, 284);
            this.tbDurabilityMin.Name = "tbDurabilityMin";
            this.tbDurabilityMin.Size = new System.Drawing.Size(114, 20);
            this.tbDurabilityMin.TabIndex = 7;
            this.tbDurabilityMin.TextChanged += new System.EventHandler(this.tbDurabilityMin_TextChanged);
            // 
            // tbDurabilityMax
            // 
            this.tbDurabilityMax.Location = new System.Drawing.Point(109, 310);
            this.tbDurabilityMax.Name = "tbDurabilityMax";
            this.tbDurabilityMax.Size = new System.Drawing.Size(114, 20);
            this.tbDurabilityMax.TabIndex = 7;
            this.tbDurabilityMax.TextChanged += new System.EventHandler(this.tbDurabilityMax_TextChanged);
            // 
            // tbDurationMin
            // 
            this.tbDurationMin.Location = new System.Drawing.Point(555, 284);
            this.tbDurationMin.Name = "tbDurationMin";
            this.tbDurationMin.Size = new System.Drawing.Size(114, 20);
            this.tbDurationMin.TabIndex = 7;
            this.tbDurationMin.TextChanged += new System.EventHandler(this.tbDurationMin_TextChanged);
            // 
            // tbDurationMax
            // 
            this.tbDurationMax.Location = new System.Drawing.Point(555, 310);
            this.tbDurationMax.Name = "tbDurationMax";
            this.tbDurationMax.Size = new System.Drawing.Size(114, 20);
            this.tbDurationMax.TabIndex = 7;
            this.tbDurationMax.TextChanged += new System.EventHandler(this.tbDurationMax_TextChanged);
            // 
            // lvItems
            // 
            this.lvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.lvItems.FullRowSelect = true;
            this.lvItems.GridLines = true;
            this.lvItems.Location = new System.Drawing.Point(12, 12);
            this.lvItems.Name = "lvItems";
            this.lvItems.Size = new System.Drawing.Size(657, 210);
            this.lvItems.TabIndex = 8;
            this.lvItems.UseCompatibleStateImageBehavior = false;
            this.lvItems.View = System.Windows.Forms.View.Details;
            this.lvItems.SelectedIndexChanged += new System.EventHandler(this.lvItems_SelectedIndexChanged);
            // 
            // btnDeleteItem
            // 
            this.btnDeleteItem.Location = new System.Drawing.Point(12, 228);
            this.btnDeleteItem.Name = "btnDeleteItem";
            this.btnDeleteItem.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteItem.TabIndex = 9;
            this.btnDeleteItem.Text = "Delete Item";
            this.btnDeleteItem.UseVisualStyleBackColor = true;
            this.btnDeleteItem.Click += new System.EventHandler(this.btnDeleteItem_Click);
            // 
            // btnNewItem
            // 
            this.btnNewItem.Location = new System.Drawing.Point(594, 228);
            this.btnNewItem.Name = "btnNewItem";
            this.btnNewItem.Size = new System.Drawing.Size(75, 23);
            this.btnNewItem.TabIndex = 10;
            this.btnNewItem.Text = "New Item";
            this.btnNewItem.UseVisualStyleBackColor = true;
            this.btnNewItem.Click += new System.EventHandler(this.btnNewItem_Click);
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Model";
            this.columnHeader2.Width = 121;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Type";
            this.columnHeader3.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Durability_Min";
            this.columnHeader4.Width = 78;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Durability_Max";
            this.columnHeader5.Width = 81;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Duration_Min";
            this.columnHeader6.Width = 75;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Duration_Max";
            this.columnHeader7.Width = 81;
            // 
            // ItemsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 344);
            this.Controls.Add(this.btnNewItem);
            this.Controls.Add(this.btnDeleteItem);
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.tbDurationMax);
            this.Controls.Add(this.tbDurationMin);
            this.Controls.Add(this.tbDurabilityMax);
            this.Controls.Add(this.tbDurabilityMin);
            this.Controls.Add(this.cbType);
            this.Controls.Add(this.cbModel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ItemsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Items";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ItemsDialog_FormClosing);
            this.Load += new System.EventHandler(this.ItemsDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbModel;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.TextBox tbDurabilityMin;
        private System.Windows.Forms.TextBox tbDurabilityMax;
        private System.Windows.Forms.TextBox tbDurationMin;
        private System.Windows.Forms.TextBox tbDurationMax;
        private System.Windows.Forms.ListView lvItems;
        private System.Windows.Forms.Button btnDeleteItem;
        private System.Windows.Forms.Button btnNewItem;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
    }
}