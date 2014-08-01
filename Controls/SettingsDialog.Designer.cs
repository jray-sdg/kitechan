namespace Kitechan.Controls
{
    partial class SettingsDialog
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.mutedUsersListBox = new System.Windows.Forms.ListBox();
            this.removeMutedUserButton = new System.Windows.Forms.Button();
            this.removeAllMutedUsersButton = new System.Windows.Forms.Button();
            this.mutedUsersGroupBox = new System.Windows.Forms.GroupBox();
            this.loadHistoryCheckBox = new System.Windows.Forms.CheckBox();
            this.mutedUsersGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(260, 185);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "[OK]";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(341, 185);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "[CANCEL]";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // mutedUsersListBox
            // 
            this.mutedUsersListBox.FormattingEnabled = true;
            this.mutedUsersListBox.Location = new System.Drawing.Point(6, 19);
            this.mutedUsersListBox.Name = "mutedUsersListBox";
            this.mutedUsersListBox.Size = new System.Drawing.Size(392, 82);
            this.mutedUsersListBox.TabIndex = 3;
            // 
            // removeMutedUserButton
            // 
            this.removeMutedUserButton.Location = new System.Drawing.Point(242, 107);
            this.removeMutedUserButton.Name = "removeMutedUserButton";
            this.removeMutedUserButton.Size = new System.Drawing.Size(75, 23);
            this.removeMutedUserButton.TabIndex = 4;
            this.removeMutedUserButton.Text = "[REMOVE]";
            this.removeMutedUserButton.UseVisualStyleBackColor = true;
            this.removeMutedUserButton.Click += new System.EventHandler(this.removeMutedUserButton_Click);
            // 
            // removeAllMutedUsersButton
            // 
            this.removeAllMutedUsersButton.Location = new System.Drawing.Point(323, 107);
            this.removeAllMutedUsersButton.Name = "removeAllMutedUsersButton";
            this.removeAllMutedUsersButton.Size = new System.Drawing.Size(75, 23);
            this.removeAllMutedUsersButton.TabIndex = 5;
            this.removeAllMutedUsersButton.Text = "[REMOVE ALL]";
            this.removeAllMutedUsersButton.UseVisualStyleBackColor = true;
            this.removeAllMutedUsersButton.Click += new System.EventHandler(this.removeAllMutedUsersButton_Click);
            // 
            // mutedUsersGroupBox
            // 
            this.mutedUsersGroupBox.Controls.Add(this.mutedUsersListBox);
            this.mutedUsersGroupBox.Controls.Add(this.removeAllMutedUsersButton);
            this.mutedUsersGroupBox.Controls.Add(this.removeMutedUserButton);
            this.mutedUsersGroupBox.Location = new System.Drawing.Point(12, 12);
            this.mutedUsersGroupBox.Name = "mutedUsersGroupBox";
            this.mutedUsersGroupBox.Size = new System.Drawing.Size(404, 139);
            this.mutedUsersGroupBox.TabIndex = 6;
            this.mutedUsersGroupBox.TabStop = false;
            this.mutedUsersGroupBox.Text = "[MUTED USERS]";
            // 
            // loadHistoryCheckBox
            // 
            this.loadHistoryCheckBox.AutoSize = true;
            this.loadHistoryCheckBox.Location = new System.Drawing.Point(12, 157);
            this.loadHistoryCheckBox.Name = "loadHistoryCheckBox";
            this.loadHistoryCheckBox.Size = new System.Drawing.Size(154, 17);
            this.loadHistoryCheckBox.TabIndex = 7;
            this.loadHistoryCheckBox.Text = "[LOAD PAST MESSAGES]";
            this.loadHistoryCheckBox.UseVisualStyleBackColor = true;
            this.loadHistoryCheckBox.CheckedChanged += new System.EventHandler(this.loadHistoryCheckBox_CheckedChanged);
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 220);
            this.Controls.Add(this.loadHistoryCheckBox);
            this.Controls.Add(this.mutedUsersGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsDialog";
            this.Text = "Settings";
            this.mutedUsersGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ListBox mutedUsersListBox;
        private System.Windows.Forms.Button removeMutedUserButton;
        private System.Windows.Forms.Button removeAllMutedUsersButton;
        private System.Windows.Forms.GroupBox mutedUsersGroupBox;
        private System.Windows.Forms.CheckBox loadHistoryCheckBox;
    }
}