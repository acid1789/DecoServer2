namespace ServerDataTool
{
    partial class Form1
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
            this.btnAccounts = new System.Windows.Forms.Button();
            this.btnNPCs = new System.Windows.Forms.Button();
            this.btnQuests = new System.Windows.Forms.Button();
            this.btnItems = new System.Windows.Forms.Button();
            this.btnLocations = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAccounts
            // 
            this.btnAccounts.Location = new System.Drawing.Point(159, 94);
            this.btnAccounts.Name = "btnAccounts";
            this.btnAccounts.Size = new System.Drawing.Size(75, 23);
            this.btnAccounts.TabIndex = 0;
            this.btnAccounts.Text = "Accounts";
            this.btnAccounts.UseVisualStyleBackColor = true;
            this.btnAccounts.Click += new System.EventHandler(this.btnAccounts_Click);
            // 
            // btnNPCs
            // 
            this.btnNPCs.Location = new System.Drawing.Point(159, 248);
            this.btnNPCs.Name = "btnNPCs";
            this.btnNPCs.Size = new System.Drawing.Size(75, 23);
            this.btnNPCs.TabIndex = 2;
            this.btnNPCs.Text = "NPCs";
            this.btnNPCs.UseVisualStyleBackColor = true;
            this.btnNPCs.Click += new System.EventHandler(this.btnNPCs_Click);
            // 
            // btnQuests
            // 
            this.btnQuests.Location = new System.Drawing.Point(518, 122);
            this.btnQuests.Name = "btnQuests";
            this.btnQuests.Size = new System.Drawing.Size(75, 23);
            this.btnQuests.TabIndex = 3;
            this.btnQuests.Text = "Quests";
            this.btnQuests.UseVisualStyleBackColor = true;
            this.btnQuests.Click += new System.EventHandler(this.btnQuests_Click);
            // 
            // btnItems
            // 
            this.btnItems.Location = new System.Drawing.Point(518, 227);
            this.btnItems.Name = "btnItems";
            this.btnItems.Size = new System.Drawing.Size(75, 23);
            this.btnItems.TabIndex = 4;
            this.btnItems.Text = "Items";
            this.btnItems.UseVisualStyleBackColor = true;
            this.btnItems.Click += new System.EventHandler(this.btnItems_Click);
            // 
            // btnLocations
            // 
            this.btnLocations.Location = new System.Drawing.Point(310, 380);
            this.btnLocations.Name = "btnLocations";
            this.btnLocations.Size = new System.Drawing.Size(75, 23);
            this.btnLocations.TabIndex = 5;
            this.btnLocations.Text = "Locations";
            this.btnLocations.UseVisualStyleBackColor = true;
            this.btnLocations.Click += new System.EventHandler(this.btnLocations_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(871, 498);
            this.Controls.Add(this.btnLocations);
            this.Controls.Add(this.btnItems);
            this.Controls.Add(this.btnQuests);
            this.Controls.Add(this.btnNPCs);
            this.Controls.Add(this.btnAccounts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Deco Server2 Data Tool";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAccounts;
        private System.Windows.Forms.Button btnNPCs;
        private System.Windows.Forms.Button btnQuests;
        private System.Windows.Forms.Button btnItems;
        private System.Windows.Forms.Button btnLocations;
    }
}

