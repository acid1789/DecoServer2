namespace ServerDataTool
{
    partial class QuestDialog
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
            this.lvQuests = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.listView2 = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnNewQuest = new System.Windows.Forms.Button();
            this.btnDeleteQuest = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listView3 = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lvQuests
            // 
            this.lvQuests.Location = new System.Drawing.Point(12, 23);
            this.lvQuests.Name = "lvQuests";
            this.lvQuests.Size = new System.Drawing.Size(259, 263);
            this.lvQuests.TabIndex = 0;
            this.lvQuests.UseCompatibleStateImageBehavior = false;
            this.lvQuests.View = System.Windows.Forms.View.List;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Quests";
            // 
            // listView2
            // 
            this.listView2.Location = new System.Drawing.Point(277, 23);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(256, 263);
            this.listView2.TabIndex = 2;
            this.listView2.UseCompatibleStateImageBehavior = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(274, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Steps";
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(539, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 269);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Step Properties";
            // 
            // btnNewQuest
            // 
            this.btnNewQuest.Location = new System.Drawing.Point(196, 605);
            this.btnNewQuest.Name = "btnNewQuest";
            this.btnNewQuest.Size = new System.Drawing.Size(75, 23);
            this.btnNewQuest.TabIndex = 5;
            this.btnNewQuest.Text = "New Quest";
            this.btnNewQuest.UseVisualStyleBackColor = true;
            // 
            // btnDeleteQuest
            // 
            this.btnDeleteQuest.Location = new System.Drawing.Point(12, 605);
            this.btnDeleteQuest.Name = "btnDeleteQuest";
            this.btnDeleteQuest.Size = new System.Drawing.Size(80, 23);
            this.btnDeleteQuest.TabIndex = 5;
            this.btnDeleteQuest.Text = "Delete Quest";
            this.btnDeleteQuest.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(801, 23);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(410, 263);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(801, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Rewards";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(277, 289);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Lines";
            // 
            // listView3
            // 
            this.listView3.Location = new System.Drawing.Point(280, 305);
            this.listView3.Name = "listView3";
            this.listView3.Size = new System.Drawing.Size(931, 323);
            this.listView3.TabIndex = 9;
            this.listView3.UseCompatibleStateImageBehavior = false;
            // 
            // QuestDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1223, 640);
            this.Controls.Add(this.listView3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btnDeleteQuest);
            this.Controls.Add(this.btnNewQuest);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listView2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvQuests);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuestDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QuestDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvQuests;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnNewQuest;
        private System.Windows.Forms.Button btnDeleteQuest;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView listView3;
    }
}