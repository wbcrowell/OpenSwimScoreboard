namespace OpenSwimScoreboard.Forms
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            namesDataLabel = new System.Windows.Forms.Label();
            namesDataText = new System.Windows.Forms.TextBox();
            namesDataCancelButton = new System.Windows.Forms.Button();
            titleLabel = new System.Windows.Forms.Label();
            doneButton = new System.Windows.Forms.Button();
            namesDataFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            panel1 = new System.Windows.Forms.Panel();
            namesDataResultLabel = new System.Windows.Forms.Label();
            namesDataSubmitButton = new System.Windows.Forms.Button();
            namesDataWarningLabel = new System.Windows.Forms.Label();
            namesDataChooseButton = new System.Windows.Forms.Button();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            panel2 = new System.Windows.Forms.Panel();
            copyLinkButton = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            linkLabel = new System.Windows.Forms.LinkLabel();
            scoreboardActiveLabel = new System.Windows.Forms.Label();
            panel3 = new System.Windows.Forms.Panel();
            numLanesText = new System.Windows.Forms.TextBox();
            lanesLabel = new System.Windows.Forms.Label();
            lanesTrackBar = new System.Windows.Forms.TrackBar();
            panel4 = new System.Windows.Forms.Panel();
            label1 = new System.Windows.Forms.Label();
            serviceStatusLabel = new System.Windows.Forms.Label();
            panel5 = new System.Windows.Forms.Panel();
            readerComboBox = new System.Windows.Forms.ComboBox();
            writeStatusProgressBar = new System.Windows.Forms.ProgressBar();
            testFileCheckBox = new System.Windows.Forms.CheckBox();
            dataConnectionLabel = new System.Windows.Forms.Label();
            portNameComboBox = new System.Windows.Forms.ComboBox();
            serialPortButton = new System.Windows.Forms.Button();
            baudRateLabel = new System.Windows.Forms.Label();
            portNameLabel = new System.Windows.Forms.Label();
            baudRateComboBox = new System.Windows.Forms.ComboBox();
            panel6 = new System.Windows.Forms.Panel();
            label3 = new System.Windows.Forms.Label();
            buttonMinusEventHeat = new System.Windows.Forms.Button();
            buttonPlusEventHeat = new System.Windows.Forms.Button();
            labelHeat = new System.Windows.Forms.Label();
            labelEvent = new System.Windows.Forms.Label();
            textBoxHeat = new System.Windows.Forms.TextBox();
            textBoxEvent = new System.Windows.Forms.TextBox();
            checkBoxUseOfflineDataOnly = new System.Windows.Forms.CheckBox();
            buttonShowStatusMsgs = new System.Windows.Forms.Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)lanesTrackBar).BeginInit();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            SuspendLayout();
            // 
            // namesDataLabel
            // 
            namesDataLabel.AutoSize = true;
            namesDataLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            namesDataLabel.Location = new System.Drawing.Point(16, 13);
            namesDataLabel.Name = "namesDataLabel";
            namesDataLabel.Size = new System.Drawing.Size(457, 17);
            namesDataLabel.TabIndex = 0;
            namesDataLabel.Text = "If there is a directory of start list (.scb) files, enter the full path (blank if none):";
            // 
            // namesDataText
            // 
            namesDataText.Enabled = false;
            namesDataText.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            namesDataText.Location = new System.Drawing.Point(16, 42);
            namesDataText.Name = "namesDataText";
            namesDataText.Size = new System.Drawing.Size(368, 27);
            namesDataText.TabIndex = 0;
            // 
            // namesDataCancelButton
            // 
            namesDataCancelButton.Location = new System.Drawing.Point(471, 43);
            namesDataCancelButton.Name = "namesDataCancelButton";
            namesDataCancelButton.Size = new System.Drawing.Size(75, 26);
            namesDataCancelButton.TabIndex = 2;
            namesDataCancelButton.Text = "Delete";
            namesDataCancelButton.UseVisualStyleBackColor = true;
            namesDataCancelButton.Click += namesDataCancelButton_Click;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Font = new System.Drawing.Font("Impact", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            titleLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            titleLabel.Location = new System.Drawing.Point(95, 40);
            titleLabel.Margin = new System.Windows.Forms.Padding(0);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new System.Drawing.Size(320, 39);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Open Swim Scoreboard";
            // 
            // doneButton
            // 
            doneButton.ForeColor = System.Drawing.SystemColors.HotTrack;
            doneButton.Location = new System.Drawing.Point(515, 969);
            doneButton.Name = "doneButton";
            doneButton.Size = new System.Drawing.Size(75, 26);
            doneButton.TabIndex = 10;
            doneButton.Text = "Stop";
            doneButton.UseVisualStyleBackColor = true;
            doneButton.Click += doneButton_Click;
            // 
            // namesDataFolderBrowser
            // 
            namesDataFolderBrowser.ShowNewFolderButton = false;
            // 
            // panel1
            // 
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Controls.Add(namesDataResultLabel);
            panel1.Controls.Add(namesDataSubmitButton);
            panel1.Controls.Add(namesDataWarningLabel);
            panel1.Controls.Add(namesDataChooseButton);
            panel1.Controls.Add(namesDataCancelButton);
            panel1.Controls.Add(namesDataLabel);
            panel1.Controls.Add(namesDataText);
            panel1.Location = new System.Drawing.Point(26, 113);
            panel1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            panel1.Name = "panel1";
            panel1.Padding = new System.Windows.Forms.Padding(13);
            panel1.Size = new System.Drawing.Size(564, 174);
            panel1.TabIndex = 5;
            // 
            // namesDataResultLabel
            // 
            namesDataResultLabel.AutoSize = true;
            namesDataResultLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            namesDataResultLabel.Location = new System.Drawing.Point(17, 122);
            namesDataResultLabel.MaximumSize = new System.Drawing.Size(407, 50);
            namesDataResultLabel.Name = "namesDataResultLabel";
            namesDataResultLabel.Size = new System.Drawing.Size(0, 17);
            namesDataResultLabel.TabIndex = 9;
            // 
            // namesDataSubmitButton
            // 
            namesDataSubmitButton.ForeColor = System.Drawing.SystemColors.HotTrack;
            namesDataSubmitButton.Location = new System.Drawing.Point(471, 76);
            namesDataSubmitButton.Name = "namesDataSubmitButton";
            namesDataSubmitButton.Size = new System.Drawing.Size(75, 26);
            namesDataSubmitButton.TabIndex = 3;
            namesDataSubmitButton.Text = "Submit";
            namesDataSubmitButton.UseVisualStyleBackColor = true;
            namesDataSubmitButton.Click += namesDataSubmitButton_Click;
            // 
            // namesDataWarningLabel
            // 
            namesDataWarningLabel.AutoSize = true;
            namesDataWarningLabel.Location = new System.Drawing.Point(17, 81);
            namesDataWarningLabel.Name = "namesDataWarningLabel";
            namesDataWarningLabel.Size = new System.Drawing.Size(239, 17);
            namesDataWarningLabel.TabIndex = 0;
            namesDataWarningLabel.Text = "After choosing or deleting, click Submit.";
            // 
            // namesDataChooseButton
            // 
            namesDataChooseButton.Location = new System.Drawing.Point(390, 42);
            namesDataChooseButton.Name = "namesDataChooseButton";
            namesDataChooseButton.Size = new System.Drawing.Size(75, 26);
            namesDataChooseButton.TabIndex = 1;
            namesDataChooseButton.Text = "Choose";
            namesDataChooseButton.UseVisualStyleBackColor = true;
            namesDataChooseButton.Click += namesDataChooseButton_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.icon;
            pictureBox1.Location = new System.Drawing.Point(26, 28);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(56, 54);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel2.Controls.Add(copyLinkButton);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(linkLabel);
            panel2.Controls.Add(scoreboardActiveLabel);
            panel2.Location = new System.Drawing.Point(26, 428);
            panel2.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            panel2.Name = "panel2";
            panel2.Padding = new System.Windows.Forms.Padding(13);
            panel2.Size = new System.Drawing.Size(564, 136);
            panel2.TabIndex = 6;
            // 
            // copyLinkButton
            // 
            copyLinkButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            copyLinkButton.Location = new System.Drawing.Point(471, 101);
            copyLinkButton.Name = "copyLinkButton";
            copyLinkButton.Size = new System.Drawing.Size(75, 23);
            copyLinkButton.TabIndex = 6;
            copyLinkButton.Text = "Copy Link";
            copyLinkButton.UseVisualStyleBackColor = true;
            copyLinkButton.Click += copyLinkButton_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label2.Location = new System.Drawing.Point(16, 106);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(301, 15);
            label2.TabIndex = 0;
            label2.Text = "Scoreboard is designed for 1920 (w) x 1080 (h) windows.";
            // 
            // linkLabel
            // 
            linkLabel.AutoSize = true;
            linkLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            linkLabel.Location = new System.Drawing.Point(17, 45);
            linkLabel.MaximumSize = new System.Drawing.Size(538, 50);
            linkLabel.Name = "linkLabel";
            linkLabel.Size = new System.Drawing.Size(54, 15);
            linkLabel.TabIndex = 5;
            linkLabel.TabStop = true;
            linkLabel.Text = "linkLabel";
            linkLabel.LinkClicked += linkLabel_LinkClicked;
            // 
            // scoreboardActiveLabel
            // 
            scoreboardActiveLabel.AutoSize = true;
            scoreboardActiveLabel.Location = new System.Drawing.Point(17, 13);
            scoreboardActiveLabel.Name = "scoreboardActiveLabel";
            scoreboardActiveLabel.Size = new System.Drawing.Size(322, 17);
            scoreboardActiveLabel.TabIndex = 0;
            scoreboardActiveLabel.Text = "Scoreboard server is running. Click this link to display:";
            // 
            // panel3
            // 
            panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel3.Controls.Add(numLanesText);
            panel3.Controls.Add(lanesLabel);
            panel3.Controls.Add(lanesTrackBar);
            panel3.Location = new System.Drawing.Point(26, 297);
            panel3.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            panel3.Name = "panel3";
            panel3.Padding = new System.Windows.Forms.Padding(13);
            panel3.Size = new System.Drawing.Size(564, 121);
            panel3.TabIndex = 7;
            // 
            // numLanesText
            // 
            numLanesText.Location = new System.Drawing.Point(491, 16);
            numLanesText.Name = "numLanesText";
            numLanesText.ReadOnly = true;
            numLanesText.Size = new System.Drawing.Size(55, 25);
            numLanesText.TabIndex = 0;
            numLanesText.Text = "6";
            numLanesText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lanesLabel
            // 
            lanesLabel.AutoSize = true;
            lanesLabel.Location = new System.Drawing.Point(17, 13);
            lanesLabel.Name = "lanesLabel";
            lanesLabel.Size = new System.Drawing.Size(182, 17);
            lanesLabel.TabIndex = 0;
            lanesLabel.Text = "How many competition lanes?";
            // 
            // lanesTrackBar
            // 
            lanesTrackBar.Location = new System.Drawing.Point(17, 58);
            lanesTrackBar.Minimum = 1;
            lanesTrackBar.Name = "lanesTrackBar";
            lanesTrackBar.Size = new System.Drawing.Size(529, 45);
            lanesTrackBar.TabIndex = 4;
            lanesTrackBar.Value = 6;
            lanesTrackBar.MouseUp += lanesTrackBar_MouseUp;
            // 
            // panel4
            // 
            panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel4.Controls.Add(buttonShowStatusMsgs);
            panel4.Controls.Add(label1);
            panel4.Controls.Add(serviceStatusLabel);
            panel4.Location = new System.Drawing.Point(26, 729);
            panel4.Name = "panel4";
            panel4.Padding = new System.Windows.Forms.Padding(13);
            panel4.Size = new System.Drawing.Size(564, 119);
            panel4.TabIndex = 8;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(16, 13);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(46, 17);
            label1.TabIndex = 0;
            label1.Text = "Status:";
            // 
            // serviceStatusLabel
            // 
            serviceStatusLabel.AutoSize = true;
            serviceStatusLabel.ForeColor = System.Drawing.SystemColors.HotTrack;
            serviceStatusLabel.Location = new System.Drawing.Point(16, 45);
            serviceStatusLabel.MaximumSize = new System.Drawing.Size(538, 50);
            serviceStatusLabel.Name = "serviceStatusLabel";
            serviceStatusLabel.Size = new System.Drawing.Size(119, 17);
            serviceStatusLabel.TabIndex = 0;
            serviceStatusLabel.Text = "Waiting for status...";
            // 
            // panel5
            // 
            panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel5.Controls.Add(readerComboBox);
            panel5.Controls.Add(writeStatusProgressBar);
            panel5.Controls.Add(testFileCheckBox);
            panel5.Controls.Add(dataConnectionLabel);
            panel5.Controls.Add(portNameComboBox);
            panel5.Controls.Add(serialPortButton);
            panel5.Controls.Add(baudRateLabel);
            panel5.Controls.Add(portNameLabel);
            panel5.Controls.Add(baudRateComboBox);
            panel5.ForeColor = System.Drawing.SystemColors.ControlText;
            panel5.Location = new System.Drawing.Point(26, 575);
            panel5.Name = "panel5";
            panel5.Padding = new System.Windows.Forms.Padding(13);
            panel5.Size = new System.Drawing.Size(564, 144);
            panel5.TabIndex = 9;
            // 
            // readerComboBox
            // 
            readerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            readerComboBox.Enabled = false;
            readerComboBox.FormattingEnabled = true;
            readerComboBox.Location = new System.Drawing.Point(390, 10);
            readerComboBox.Name = "readerComboBox";
            readerComboBox.Size = new System.Drawing.Size(156, 25);
            readerComboBox.TabIndex = 14;
            readerComboBox.Visible = false;
            // 
            // writeStatusProgressBar
            // 
            writeStatusProgressBar.Location = new System.Drawing.Point(390, 59);
            writeStatusProgressBar.Name = "writeStatusProgressBar";
            writeStatusProgressBar.Size = new System.Drawing.Size(156, 25);
            writeStatusProgressBar.TabIndex = 13;
            writeStatusProgressBar.Visible = false;
            // 
            // testFileCheckBox
            // 
            testFileCheckBox.AutoSize = true;
            testFileCheckBox.Enabled = false;
            testFileCheckBox.Location = new System.Drawing.Point(390, 61);
            testFileCheckBox.Name = "testFileCheckBox";
            testFileCheckBox.Size = new System.Drawing.Size(104, 21);
            testFileCheckBox.TabIndex = 12;
            testFileCheckBox.Text = "Write test file";
            testFileCheckBox.UseVisualStyleBackColor = true;
            testFileCheckBox.CheckedChanged += testFileCheckBox_CheckedChanged;
            // 
            // dataConnectionLabel
            // 
            dataConnectionLabel.AutoSize = true;
            dataConnectionLabel.Location = new System.Drawing.Point(17, 13);
            dataConnectionLabel.Name = "dataConnectionLabel";
            dataConnectionLabel.Size = new System.Drawing.Size(350, 17);
            dataConnectionLabel.TabIndex = 11;
            dataConnectionLabel.Text = "Timing data connection. Disconnect to change parameters.";
            // 
            // portNameComboBox
            // 
            portNameComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            portNameComboBox.Enabled = false;
            portNameComboBox.FormattingEnabled = true;
            portNameComboBox.Location = new System.Drawing.Point(177, 59);
            portNameComboBox.Name = "portNameComboBox";
            portNameComboBox.Size = new System.Drawing.Size(207, 25);
            portNameComboBox.TabIndex = 10;
            // 
            // serialPortButton
            // 
            serialPortButton.Location = new System.Drawing.Point(390, 101);
            serialPortButton.Name = "serialPortButton";
            serialPortButton.Size = new System.Drawing.Size(156, 25);
            serialPortButton.TabIndex = 9;
            serialPortButton.Text = "Disconnect";
            serialPortButton.UseVisualStyleBackColor = true;
            serialPortButton.Click += serialPortButton_Click;
            // 
            // baudRateLabel
            // 
            baudRateLabel.AutoSize = true;
            baudRateLabel.Location = new System.Drawing.Point(16, 104);
            baudRateLabel.Name = "baudRateLabel";
            baudRateLabel.Size = new System.Drawing.Size(67, 17);
            baudRateLabel.TabIndex = 3;
            baudRateLabel.Text = "Baud rate:";
            // 
            // portNameLabel
            // 
            portNameLabel.AutoSize = true;
            portNameLabel.Location = new System.Drawing.Point(16, 62);
            portNameLabel.Name = "portNameLabel";
            portNameLabel.Size = new System.Drawing.Size(71, 17);
            portNameLabel.TabIndex = 2;
            portNameLabel.Text = "Port name:";
            // 
            // baudRateComboBox
            // 
            baudRateComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            baudRateComboBox.Enabled = false;
            baudRateComboBox.FormattingEnabled = true;
            baudRateComboBox.Location = new System.Drawing.Point(177, 101);
            baudRateComboBox.Name = "baudRateComboBox";
            baudRateComboBox.Size = new System.Drawing.Size(207, 25);
            baudRateComboBox.TabIndex = 7;
            // 
            // panel6
            // 
            panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel6.Controls.Add(label3);
            panel6.Controls.Add(buttonMinusEventHeat);
            panel6.Controls.Add(buttonPlusEventHeat);
            panel6.Controls.Add(labelHeat);
            panel6.Controls.Add(labelEvent);
            panel6.Controls.Add(textBoxHeat);
            panel6.Controls.Add(textBoxEvent);
            panel6.Controls.Add(checkBoxUseOfflineDataOnly);
            panel6.Location = new System.Drawing.Point(26, 858);
            panel6.Name = "panel6";
            panel6.Size = new System.Drawing.Size(564, 86);
            panel6.TabIndex = 11;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = System.Drawing.SystemColors.HotTrack;
            label3.Location = new System.Drawing.Point(37, 40);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(312, 34);
            label3.TabIndex = 4;
            label3.Text = "Shows event number and lane seeds when live data \r\nnot available. Change manually using +/- buttons.";
            // 
            // buttonMinusEventHeat
            // 
            buttonMinusEventHeat.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            buttonMinusEventHeat.Location = new System.Drawing.Point(503, 45);
            buttonMinusEventHeat.Name = "buttonMinusEventHeat";
            buttonMinusEventHeat.Size = new System.Drawing.Size(43, 25);
            buttonMinusEventHeat.TabIndex = 3;
            buttonMinusEventHeat.Text = "-";
            buttonMinusEventHeat.UseVisualStyleBackColor = true;
            buttonMinusEventHeat.Click += buttonMinusEventHeat_Click;
            // 
            // buttonPlusEventHeat
            // 
            buttonPlusEventHeat.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            buttonPlusEventHeat.Location = new System.Drawing.Point(503, 14);
            buttonPlusEventHeat.Name = "buttonPlusEventHeat";
            buttonPlusEventHeat.Size = new System.Drawing.Size(43, 25);
            buttonPlusEventHeat.TabIndex = 3;
            buttonPlusEventHeat.Text = "+";
            buttonPlusEventHeat.UseVisualStyleBackColor = true;
            buttonPlusEventHeat.Click += buttonPlusEventHeat_Click;
            // 
            // labelHeat
            // 
            labelHeat.AutoSize = true;
            labelHeat.Enabled = false;
            labelHeat.Location = new System.Drawing.Point(358, 48);
            labelHeat.Name = "labelHeat";
            labelHeat.Size = new System.Drawing.Size(35, 17);
            labelHeat.TabIndex = 2;
            labelHeat.Text = "Heat";
            // 
            // labelEvent
            // 
            labelEvent.AutoSize = true;
            labelEvent.Enabled = false;
            labelEvent.Location = new System.Drawing.Point(358, 17);
            labelEvent.Name = "labelEvent";
            labelEvent.Size = new System.Drawing.Size(39, 17);
            labelEvent.TabIndex = 2;
            labelEvent.Text = "Event";
            // 
            // textBoxHeat
            // 
            textBoxHeat.Location = new System.Drawing.Point(412, 45);
            textBoxHeat.Name = "textBoxHeat";
            textBoxHeat.ReadOnly = true;
            textBoxHeat.Size = new System.Drawing.Size(74, 25);
            textBoxHeat.TabIndex = 1;
            // 
            // textBoxEvent
            // 
            textBoxEvent.Location = new System.Drawing.Point(412, 14);
            textBoxEvent.Name = "textBoxEvent";
            textBoxEvent.ReadOnly = true;
            textBoxEvent.Size = new System.Drawing.Size(74, 25);
            textBoxEvent.TabIndex = 1;
            // 
            // checkBoxUseOfflineDataOnly
            // 
            checkBoxUseOfflineDataOnly.AutoSize = true;
            checkBoxUseOfflineDataOnly.Location = new System.Drawing.Point(20, 16);
            checkBoxUseOfflineDataOnly.Name = "checkBoxUseOfflineDataOnly";
            checkBoxUseOfflineDataOnly.Size = new System.Drawing.Size(156, 21);
            checkBoxUseOfflineDataOnly.TabIndex = 0;
            checkBoxUseOfflineDataOnly.Text = "Show offline data only";
            checkBoxUseOfflineDataOnly.UseVisualStyleBackColor = true;
            checkBoxUseOfflineDataOnly.CheckedChanged += checkBoxUseOfflineDataOnly_CheckedChanged;
            // 
            // buttonShowStatusMsgs
            // 
            buttonShowStatusMsgs.Location = new System.Drawing.Point(390, 16);
            buttonShowStatusMsgs.Name = "buttonShowStatusMsgs";
            buttonShowStatusMsgs.Size = new System.Drawing.Size(156, 25);
            buttonShowStatusMsgs.TabIndex = 1;
            buttonShowStatusMsgs.Text = "Show Status Msgs";
            buttonShowStatusMsgs.UseVisualStyleBackColor = true;
            buttonShowStatusMsgs.Click += buttonShowStatusMsgs_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Window;
            ClientSize = new System.Drawing.Size(616, 1025);
            Controls.Add(panel6);
            Controls.Add(panel5);
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(doneButton);
            Controls.Add(pictureBox1);
            Controls.Add(titleLabel);
            Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Padding = new System.Windows.Forms.Padding(26);
            Text = "Open Swim Scoreboard";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)lanesTrackBar).EndInit();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label namesDataLabel;
        private System.Windows.Forms.TextBox namesDataText;
        private System.Windows.Forms.Button namesDataCancelButton;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Button doneButton;
        private System.Windows.Forms.FolderBrowserDialog namesDataFolderBrowser;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button namesDataChooseButton;
        private System.Windows.Forms.Button namesDataSubmitButton;
        private System.Windows.Forms.Label namesDataWarningLabel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.LinkLabel linkLabel;
        private System.Windows.Forms.Label scoreboardActiveLabel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lanesLabel;
        private System.Windows.Forms.TrackBar lanesTrackBar;
        private System.Windows.Forms.TextBox numLanesText;
        private System.Windows.Forms.Label namesDataResultLabel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label serviceStatusLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button copyLinkButton;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label baudRateLabel;
        private System.Windows.Forms.Label portNameLabel;
        private System.Windows.Forms.ComboBox baudRateComboBox;
        private System.Windows.Forms.Button serialPortButton;
        private System.Windows.Forms.ComboBox portNameComboBox;
        private System.Windows.Forms.Label dataConnectionLabel;
        private System.Windows.Forms.CheckBox testFileCheckBox;
        private System.Windows.Forms.ProgressBar writeStatusProgressBar;
        private System.Windows.Forms.ComboBox readerComboBox;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button buttonMinusEventHeat;
        private System.Windows.Forms.Button buttonPlusEventHeat;
        private System.Windows.Forms.Label labelHeat;
        private System.Windows.Forms.Label labelEvent;
        private System.Windows.Forms.TextBox textBoxHeat;
        private System.Windows.Forms.TextBox textBoxEvent;
        private System.Windows.Forms.CheckBox checkBoxUseOfflineDataOnly;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonShowStatusMsgs;
    }
}
