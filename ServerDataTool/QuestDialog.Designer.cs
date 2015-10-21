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
            this.ID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.lvSteps = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnNewQuest = new System.Windows.Forms.Button();
            this.btnDeleteQuest = new System.Windows.Forms.Button();
            this.lvRewards = new System.Windows.Forms.ListView();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lvLines = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbQuestName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbQuestGiver = new System.Windows.Forms.ComboBox();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.cbCompletionType = new System.Windows.Forms.ComboBox();
            this.tbCompletionCount = new System.Windows.Forms.TextBox();
            this.tbCompletionTarget = new System.Windows.Forms.TextBox();
            this.cbStepOwner = new System.Windows.Forms.ComboBox();
            this.btnNewStep = new System.Windows.Forms.Button();
            this.btnDeleteStep = new System.Windows.Forms.Button();
            this.lvRequirements = new System.Windows.Forms.ListView();
            this.label12 = new System.Windows.Forms.Label();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnDeleteRequirement = new System.Windows.Forms.Button();
            this.btnNewRequirement = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cbRequirementType = new System.Windows.Forms.ComboBox();
            this.tbRequirementParam = new System.Windows.Forms.TextBox();
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnDeleteLine = new System.Windows.Forms.Button();
            this.btnNewLine = new System.Windows.Forms.Button();
            this.btnStepUp = new System.Windows.Forms.Button();
            this.btnStepDown = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.cbIcon = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.cbStaticText = new System.Windows.Forms.ComboBox();
            this.tbDynamicText = new System.Windows.Forms.TextBox();
            this.btnLineUp = new System.Windows.Forms.Button();
            this.btnLineDown = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.cbItem = new System.Windows.Forms.ComboBox();
            this.tbGold = new System.Windows.Forms.TextBox();
            this.tbExp = new System.Windows.Forms.TextBox();
            this.tbFame = new System.Windows.Forms.TextBox();
            this.btnDeleteReward = new System.Windows.Forms.Button();
            this.btnAddReward = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvQuests
            // 
            this.lvQuests.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader1});
            this.lvQuests.FullRowSelect = true;
            this.lvQuests.GridLines = true;
            this.lvQuests.Location = new System.Drawing.Point(12, 23);
            this.lvQuests.Name = "lvQuests";
            this.lvQuests.Size = new System.Drawing.Size(382, 263);
            this.lvQuests.TabIndex = 0;
            this.lvQuests.UseCompatibleStateImageBehavior = false;
            this.lvQuests.View = System.Windows.Forms.View.Details;
            this.lvQuests.SelectedIndexChanged += new System.EventHandler(this.lvQuests_SelectedIndexChanged);
            // 
            // ID
            // 
            this.ID.Text = "ID";
            this.ID.Width = 26;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 160;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Giver";
            this.columnHeader3.Width = 119;
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
            // lvSteps
            // 
            this.lvSteps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
            this.lvSteps.FullRowSelect = true;
            this.lvSteps.GridLines = true;
            this.lvSteps.Location = new System.Drawing.Point(404, 23);
            this.lvSteps.Name = "lvSteps";
            this.lvSteps.Size = new System.Drawing.Size(214, 263);
            this.lvSteps.TabIndex = 2;
            this.lvSteps.UseCompatibleStateImageBehavior = false;
            this.lvSteps.View = System.Windows.Forms.View.Details;
            this.lvSteps.SelectedIndexChanged += new System.EventHandler(this.lvSteps_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(401, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Steps";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbStepOwner);
            this.groupBox1.Controls.Add(this.tbCompletionTarget);
            this.groupBox1.Controls.Add(this.tbCompletionCount);
            this.groupBox1.Controls.Add(this.cbCompletionType);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(404, 321);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(214, 125);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Step Properties";
            // 
            // btnNewQuest
            // 
            this.btnNewQuest.Location = new System.Drawing.Point(319, 292);
            this.btnNewQuest.Name = "btnNewQuest";
            this.btnNewQuest.Size = new System.Drawing.Size(75, 23);
            this.btnNewQuest.TabIndex = 5;
            this.btnNewQuest.Text = "New Quest";
            this.btnNewQuest.UseVisualStyleBackColor = true;
            this.btnNewQuest.Click += new System.EventHandler(this.btnNewQuest_Click);
            // 
            // btnDeleteQuest
            // 
            this.btnDeleteQuest.Location = new System.Drawing.Point(12, 292);
            this.btnDeleteQuest.Name = "btnDeleteQuest";
            this.btnDeleteQuest.Size = new System.Drawing.Size(80, 23);
            this.btnDeleteQuest.TabIndex = 5;
            this.btnDeleteQuest.Text = "Delete Quest";
            this.btnDeleteQuest.UseVisualStyleBackColor = true;
            this.btnDeleteQuest.Click += new System.EventHandler(this.btnDeleteQuest_Click);
            // 
            // lvRewards
            // 
            this.lvRewards.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11});
            this.lvRewards.FullRowSelect = true;
            this.lvRewards.GridLines = true;
            this.lvRewards.Location = new System.Drawing.Point(624, 23);
            this.lvRewards.Name = "lvRewards";
            this.lvRewards.Size = new System.Drawing.Size(257, 263);
            this.lvRewards.TabIndex = 6;
            this.lvRewards.UseCompatibleStateImageBehavior = false;
            this.lvRewards.View = System.Windows.Forms.View.Details;
            this.lvRewards.SelectedIndexChanged += new System.EventHandler(this.lvRewards_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(624, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Rewards";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(624, 305);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Lines";
            // 
            // lvLines
            // 
            this.lvLines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader12,
            this.columnHeader13,
            this.columnHeader14});
            this.lvLines.FullRowSelect = true;
            this.lvLines.GridLines = true;
            this.lvLines.Location = new System.Drawing.Point(627, 321);
            this.lvLines.Name = "lvLines";
            this.lvLines.Size = new System.Drawing.Size(584, 172);
            this.lvLines.TabIndex = 9;
            this.lvLines.UseCompatibleStateImageBehavior = false;
            this.lvLines.View = System.Windows.Forms.View.Details;
            this.lvLines.SelectedIndexChanged += new System.EventHandler(this.lvLines_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Map";
            this.columnHeader1.Width = 39;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbQuestGiver);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.tbQuestName);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(12, 321);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(382, 78);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Quest Properties";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Name:";
            // 
            // tbQuestName
            // 
            this.tbQuestName.Location = new System.Drawing.Point(53, 19);
            this.tbQuestName.Name = "tbQuestName";
            this.tbQuestName.Size = new System.Drawing.Size(323, 20);
            this.tbQuestName.TabIndex = 1;
            this.tbQuestName.TextChanged += new System.EventHandler(this.tbQuestName_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Giver:";
            // 
            // cbQuestGiver
            // 
            this.cbQuestGiver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbQuestGiver.FormattingEnabled = true;
            this.cbQuestGiver.Location = new System.Drawing.Point(53, 45);
            this.cbQuestGiver.MaxDropDownItems = 20;
            this.cbQuestGiver.Name = "cbQuestGiver";
            this.cbQuestGiver.Size = new System.Drawing.Size(323, 21);
            this.cbQuestGiver.TabIndex = 3;
            this.cbQuestGiver.SelectedIndexChanged += new System.EventHandler(this.cbQuestGiver_SelectedIndexChanged);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Step #";
            this.columnHeader4.Width = 44;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Comp. Type";
            this.columnHeader5.Width = 151;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Completion Type:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Completion Count:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 78);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Completion Target:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(34, 104);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(66, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Step Owner:";
            // 
            // cbCompletionType
            // 
            this.cbCompletionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCompletionType.FormattingEnabled = true;
            this.cbCompletionType.Location = new System.Drawing.Point(106, 18);
            this.cbCompletionType.Name = "cbCompletionType";
            this.cbCompletionType.Size = new System.Drawing.Size(102, 21);
            this.cbCompletionType.TabIndex = 5;
            this.cbCompletionType.SelectedIndexChanged += new System.EventHandler(this.cbCompletionType_SelectedIndexChanged);
            // 
            // tbCompletionCount
            // 
            this.tbCompletionCount.Location = new System.Drawing.Point(106, 45);
            this.tbCompletionCount.Name = "tbCompletionCount";
            this.tbCompletionCount.Size = new System.Drawing.Size(102, 20);
            this.tbCompletionCount.TabIndex = 6;
            this.tbCompletionCount.TextChanged += new System.EventHandler(this.tbCompletionCount_TextChanged);
            // 
            // tbCompletionTarget
            // 
            this.tbCompletionTarget.Location = new System.Drawing.Point(106, 71);
            this.tbCompletionTarget.Name = "tbCompletionTarget";
            this.tbCompletionTarget.Size = new System.Drawing.Size(102, 20);
            this.tbCompletionTarget.TabIndex = 7;
            this.tbCompletionTarget.TextChanged += new System.EventHandler(this.tbCompletionTarget_TextChanged);
            // 
            // cbStepOwner
            // 
            this.cbStepOwner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStepOwner.FormattingEnabled = true;
            this.cbStepOwner.Location = new System.Drawing.Point(106, 97);
            this.cbStepOwner.Name = "cbStepOwner";
            this.cbStepOwner.Size = new System.Drawing.Size(102, 21);
            this.cbStepOwner.TabIndex = 8;
            this.cbStepOwner.SelectedIndexChanged += new System.EventHandler(this.cbStepOwner_SelectedIndexChanged);
            // 
            // btnNewStep
            // 
            this.btnNewStep.Location = new System.Drawing.Point(543, 292);
            this.btnNewStep.Name = "btnNewStep";
            this.btnNewStep.Size = new System.Drawing.Size(75, 23);
            this.btnNewStep.TabIndex = 11;
            this.btnNewStep.Text = "New Step";
            this.btnNewStep.UseVisualStyleBackColor = true;
            this.btnNewStep.Click += new System.EventHandler(this.btnNewStep_Click);
            // 
            // btnDeleteStep
            // 
            this.btnDeleteStep.Location = new System.Drawing.Point(404, 292);
            this.btnDeleteStep.Name = "btnDeleteStep";
            this.btnDeleteStep.Size = new System.Drawing.Size(80, 23);
            this.btnDeleteStep.TabIndex = 5;
            this.btnDeleteStep.Text = "Delete Step";
            this.btnDeleteStep.UseVisualStyleBackColor = true;
            this.btnDeleteStep.Click += new System.EventHandler(this.btnDeleteStep_Click);
            // 
            // lvRequirements
            // 
            this.lvRequirements.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7});
            this.lvRequirements.FullRowSelect = true;
            this.lvRequirements.GridLines = true;
            this.lvRequirements.Location = new System.Drawing.Point(12, 426);
            this.lvRequirements.Name = "lvRequirements";
            this.lvRequirements.Size = new System.Drawing.Size(174, 202);
            this.lvRequirements.TabIndex = 12;
            this.lvRequirements.UseCompatibleStateImageBehavior = false;
            this.lvRequirements.View = System.Windows.Forms.View.Details;
            this.lvRequirements.SelectedIndexChanged += new System.EventHandler(this.lvRequirements_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 410);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "Requirements";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Type";
            this.columnHeader6.Width = 102;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Param";
            // 
            // btnDeleteRequirement
            // 
            this.btnDeleteRequirement.Location = new System.Drawing.Point(194, 499);
            this.btnDeleteRequirement.Name = "btnDeleteRequirement";
            this.btnDeleteRequirement.Size = new System.Drawing.Size(80, 23);
            this.btnDeleteRequirement.TabIndex = 5;
            this.btnDeleteRequirement.Text = "Delete Req";
            this.btnDeleteRequirement.UseVisualStyleBackColor = true;
            this.btnDeleteRequirement.Click += new System.EventHandler(this.btnDeleteRequirement_Click);
            // 
            // btnNewRequirement
            // 
            this.btnNewRequirement.Location = new System.Drawing.Point(319, 499);
            this.btnNewRequirement.Name = "btnNewRequirement";
            this.btnNewRequirement.Size = new System.Drawing.Size(75, 23);
            this.btnNewRequirement.TabIndex = 5;
            this.btnNewRequirement.Text = "New Req";
            this.btnNewRequirement.UseVisualStyleBackColor = true;
            this.btnNewRequirement.Click += new System.EventHandler(this.btnNewRequirement_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbRequirementParam);
            this.groupBox3.Controls.Add(this.cbRequirementType);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Location = new System.Drawing.Point(192, 419);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 74);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Requirements Properties";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 27);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(34, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Type:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 53);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Param:";
            // 
            // cbRequirementType
            // 
            this.cbRequirementType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbRequirementType.FormattingEnabled = true;
            this.cbRequirementType.Location = new System.Drawing.Point(52, 19);
            this.cbRequirementType.Name = "cbRequirementType";
            this.cbRequirementType.Size = new System.Drawing.Size(142, 21);
            this.cbRequirementType.TabIndex = 1;
            this.cbRequirementType.SelectedIndexChanged += new System.EventHandler(this.cbRequirementType_SelectedIndexChanged);
            // 
            // tbRequirementParam
            // 
            this.tbRequirementParam.Location = new System.Drawing.Point(52, 46);
            this.tbRequirementParam.Name = "tbRequirementParam";
            this.tbRequirementParam.Size = new System.Drawing.Size(142, 20);
            this.tbRequirementParam.TabIndex = 2;
            this.tbRequirementParam.TextChanged += new System.EventHandler(this.tbRequirementParam_TextChanged);
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Gold";
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Exp";
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Fame";
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Item";
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Line #";
            this.columnHeader12.Width = 43;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Icon";
            this.columnHeader13.Width = 39;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Text";
            this.columnHeader14.Width = 466;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tbDynamicText);
            this.groupBox4.Controls.Add(this.cbStaticText);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Controls.Add(this.cbIcon);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Location = new System.Drawing.Point(627, 525);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(584, 103);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Line Properties";
            // 
            // btnDeleteLine
            // 
            this.btnDeleteLine.Location = new System.Drawing.Point(627, 499);
            this.btnDeleteLine.Name = "btnDeleteLine";
            this.btnDeleteLine.Size = new System.Drawing.Size(80, 23);
            this.btnDeleteLine.TabIndex = 5;
            this.btnDeleteLine.Text = "Delete Line";
            this.btnDeleteLine.UseVisualStyleBackColor = true;
            this.btnDeleteLine.Click += new System.EventHandler(this.btnDeleteLine_Click);
            // 
            // btnNewLine
            // 
            this.btnNewLine.Location = new System.Drawing.Point(1136, 499);
            this.btnNewLine.Name = "btnNewLine";
            this.btnNewLine.Size = new System.Drawing.Size(75, 23);
            this.btnNewLine.TabIndex = 11;
            this.btnNewLine.Text = "New Line";
            this.btnNewLine.UseVisualStyleBackColor = true;
            this.btnNewLine.Click += new System.EventHandler(this.btnNewLine_Click);
            // 
            // btnStepUp
            // 
            this.btnStepUp.Location = new System.Drawing.Point(490, 292);
            this.btnStepUp.Name = "btnStepUp";
            this.btnStepUp.Size = new System.Drawing.Size(23, 23);
            this.btnStepUp.TabIndex = 16;
            this.btnStepUp.Text = "▲";
            this.btnStepUp.UseVisualStyleBackColor = true;
            this.btnStepUp.Click += new System.EventHandler(this.btnStepUp_Click);
            // 
            // btnStepDown
            // 
            this.btnStepDown.Location = new System.Drawing.Point(514, 292);
            this.btnStepDown.Name = "btnStepDown";
            this.btnStepDown.Size = new System.Drawing.Size(23, 23);
            this.btnStepDown.TabIndex = 16;
            this.btnStepDown.Text = "▼";
            this.btnStepDown.UseVisualStyleBackColor = true;
            this.btnStepDown.Click += new System.EventHandler(this.btnStepDown_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Icon:";
            // 
            // cbIcon
            // 
            this.cbIcon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIcon.FormattingEnabled = true;
            this.cbIcon.Location = new System.Drawing.Point(60, 19);
            this.cbIcon.Name = "cbIcon";
            this.cbIcon.Size = new System.Drawing.Size(129, 21);
            this.cbIcon.TabIndex = 1;
            this.cbIcon.SelectedIndexChanged += new System.EventHandler(this.cbIcon_SelectedIndexChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 54);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(45, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "Text ID:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(23, 80);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(31, 13);
            this.label16.TabIndex = 3;
            this.label16.Text = "Text:";
            // 
            // cbStaticText
            // 
            this.cbStaticText.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStaticText.FormattingEnabled = true;
            this.cbStaticText.Location = new System.Drawing.Point(60, 46);
            this.cbStaticText.Name = "cbStaticText";
            this.cbStaticText.Size = new System.Drawing.Size(518, 21);
            this.cbStaticText.TabIndex = 4;
            this.cbStaticText.SelectedIndexChanged += new System.EventHandler(this.cbStaticText_SelectedIndexChanged);
            // 
            // tbDynamicText
            // 
            this.tbDynamicText.Enabled = false;
            this.tbDynamicText.Location = new System.Drawing.Point(60, 73);
            this.tbDynamicText.Name = "tbDynamicText";
            this.tbDynamicText.Size = new System.Drawing.Size(518, 20);
            this.tbDynamicText.TabIndex = 5;
            this.tbDynamicText.TextChanged += new System.EventHandler(this.tbDynamicText_TextChanged);
            // 
            // btnLineUp
            // 
            this.btnLineUp.Location = new System.Drawing.Point(893, 499);
            this.btnLineUp.Name = "btnLineUp";
            this.btnLineUp.Size = new System.Drawing.Size(23, 23);
            this.btnLineUp.TabIndex = 16;
            this.btnLineUp.Text = "▲";
            this.btnLineUp.UseVisualStyleBackColor = true;
            this.btnLineUp.Click += new System.EventHandler(this.btnLineUp_Click);
            // 
            // btnLineDown
            // 
            this.btnLineDown.Location = new System.Drawing.Point(917, 499);
            this.btnLineDown.Name = "btnLineDown";
            this.btnLineDown.Size = new System.Drawing.Size(23, 23);
            this.btnLineDown.TabIndex = 16;
            this.btnLineDown.Text = "▼";
            this.btnLineDown.UseVisualStyleBackColor = true;
            this.btnLineDown.Click += new System.EventHandler(this.btnLineDown_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.tbFame);
            this.groupBox5.Controls.Add(this.tbExp);
            this.groupBox5.Controls.Add(this.tbGold);
            this.groupBox5.Controls.Add(this.cbItem);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.label18);
            this.groupBox5.Controls.Add(this.label17);
            this.groupBox5.Location = new System.Drawing.Point(887, 13);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(318, 131);
            this.groupBox5.TabIndex = 17;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Reward Properties";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(16, 26);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(32, 13);
            this.label17.TabIndex = 0;
            this.label17.Text = "Gold:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(20, 52);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(28, 13);
            this.label18.TabIndex = 0;
            this.label18.Text = "Exp:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(12, 78);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(36, 13);
            this.label19.TabIndex = 0;
            this.label19.Text = "Fame:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(20, 105);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(30, 13);
            this.label20.TabIndex = 0;
            this.label20.Text = "Item:";
            // 
            // cbItem
            // 
            this.cbItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbItem.FormattingEnabled = true;
            this.cbItem.Location = new System.Drawing.Point(54, 97);
            this.cbItem.Name = "cbItem";
            this.cbItem.Size = new System.Drawing.Size(258, 21);
            this.cbItem.TabIndex = 1;
            this.cbItem.SelectedIndexChanged += new System.EventHandler(this.cbItem_SelectedIndexChanged);
            // 
            // tbGold
            // 
            this.tbGold.Location = new System.Drawing.Point(54, 19);
            this.tbGold.Name = "tbGold";
            this.tbGold.Size = new System.Drawing.Size(258, 20);
            this.tbGold.TabIndex = 2;
            this.tbGold.TextChanged += new System.EventHandler(this.tbGold_TextChanged);
            // 
            // tbExp
            // 
            this.tbExp.Location = new System.Drawing.Point(54, 45);
            this.tbExp.Name = "tbExp";
            this.tbExp.Size = new System.Drawing.Size(258, 20);
            this.tbExp.TabIndex = 2;
            this.tbExp.TextChanged += new System.EventHandler(this.tbExp_TextChanged);
            // 
            // tbFame
            // 
            this.tbFame.Location = new System.Drawing.Point(54, 71);
            this.tbFame.Name = "tbFame";
            this.tbFame.Size = new System.Drawing.Size(258, 20);
            this.tbFame.TabIndex = 2;
            this.tbFame.TextChanged += new System.EventHandler(this.tbFame_TextChanged);
            // 
            // btnDeleteReward
            // 
            this.btnDeleteReward.Location = new System.Drawing.Point(887, 150);
            this.btnDeleteReward.Name = "btnDeleteReward";
            this.btnDeleteReward.Size = new System.Drawing.Size(87, 23);
            this.btnDeleteReward.TabIndex = 18;
            this.btnDeleteReward.Text = "Delete Reward";
            this.btnDeleteReward.UseVisualStyleBackColor = true;
            this.btnDeleteReward.Click += new System.EventHandler(this.btnDeleteReward_Click);
            // 
            // btnAddReward
            // 
            this.btnAddReward.Location = new System.Drawing.Point(1130, 150);
            this.btnAddReward.Name = "btnAddReward";
            this.btnAddReward.Size = new System.Drawing.Size(75, 23);
            this.btnAddReward.TabIndex = 19;
            this.btnAddReward.Text = "Add Reward";
            this.btnAddReward.UseVisualStyleBackColor = true;
            this.btnAddReward.Click += new System.EventHandler(this.btnAddReward_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(478, 579);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(134, 48);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Save Quests";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // QuestDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1223, 640);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnAddReward);
            this.Controls.Add(this.btnDeleteReward);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnLineDown);
            this.Controls.Add(this.btnLineUp);
            this.Controls.Add(this.btnStepDown);
            this.Controls.Add(this.btnStepUp);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.lvRequirements);
            this.Controls.Add(this.btnNewLine);
            this.Controls.Add(this.btnNewStep);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lvLines);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnDeleteLine);
            this.Controls.Add(this.lvRewards);
            this.Controls.Add(this.btnDeleteStep);
            this.Controls.Add(this.btnDeleteRequirement);
            this.Controls.Add(this.btnDeleteQuest);
            this.Controls.Add(this.btnNewRequirement);
            this.Controls.Add(this.btnNewQuest);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lvSteps);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvQuests);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "QuestDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quest Dialog";
            this.Load += new System.EventHandler(this.QuestDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvQuests;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvSteps;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnNewQuest;
        private System.Windows.Forms.Button btnDeleteQuest;
        private System.Windows.Forms.ListView lvRewards;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView lvLines;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cbQuestGiver;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbQuestName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbStepOwner;
        private System.Windows.Forms.TextBox tbCompletionTarget;
        private System.Windows.Forms.TextBox tbCompletionCount;
        private System.Windows.Forms.ComboBox cbCompletionType;
        private System.Windows.Forms.Button btnNewStep;
        private System.Windows.Forms.Button btnDeleteStep;
        private System.Windows.Forms.ListView lvRequirements;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnDeleteRequirement;
        private System.Windows.Forms.Button btnNewRequirement;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tbRequirementParam;
        private System.Windows.Forms.ComboBox cbRequirementType;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnDeleteLine;
        private System.Windows.Forms.Button btnNewLine;
        private System.Windows.Forms.TextBox tbDynamicText;
        private System.Windows.Forms.ComboBox cbStaticText;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cbIcon;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnStepUp;
        private System.Windows.Forms.Button btnStepDown;
        private System.Windows.Forms.Button btnLineUp;
        private System.Windows.Forms.Button btnLineDown;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox tbFame;
        private System.Windows.Forms.TextBox tbExp;
        private System.Windows.Forms.TextBox tbGold;
        private System.Windows.Forms.ComboBox cbItem;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button btnDeleteReward;
        private System.Windows.Forms.Button btnAddReward;
        private System.Windows.Forms.Button btnSave;
    }
}