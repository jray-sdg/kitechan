namespace Kitechan.Controls
{
    partial class UserInfoDialog
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
            this.userImagePictureBox = new Kitechan.SmoothPictureBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.userIdLabel = new System.Windows.Forms.Label();
            this.aliasLabel = new System.Windows.Forms.Label();
            this.aliasTextBox = new TextBoxWithoutCaret();
            ((System.ComponentModel.ISupportInitialize)(this.userImagePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // userImagePictureBox
            // 
            this.userImagePictureBox.Location = new System.Drawing.Point(12, 12);
            this.userImagePictureBox.Name = "userImagePictureBox";
            this.userImagePictureBox.Size = new System.Drawing.Size(75, 75);
            this.userImagePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.userImagePictureBox.TabIndex = 0;
            this.userImagePictureBox.TabStop = false;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(93, 32);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(44, 13);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "[NAME]";
            // 
            // userIdLabel
            // 
            this.userIdLabel.AutoSize = true;
            this.userIdLabel.Location = new System.Drawing.Point(93, 58);
            this.userIdLabel.Name = "userIdLabel";
            this.userIdLabel.Size = new System.Drawing.Size(57, 13);
            this.userIdLabel.TabIndex = 2;
            this.userIdLabel.Text = "[USER ID]";
            // 
            // aliasLabel
            // 
            this.aliasLabel.AutoSize = true;
            this.aliasLabel.Location = new System.Drawing.Point(9, 107);
            this.aliasLabel.Name = "aliasLabel";
            this.aliasLabel.Size = new System.Drawing.Size(57, 13);
            this.aliasLabel.TabIndex = 3;
            this.aliasLabel.Text = "[ALIASES]";
            // 
            // aliasTextBox
            // 
            this.aliasTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.aliasTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.aliasTextBox.Location = new System.Drawing.Point(12, 123);
            this.aliasTextBox.Multiline = true;
            this.aliasTextBox.Name = "aliasTextBox";
            this.aliasTextBox.ReadOnly = true;
            this.aliasTextBox.Size = new System.Drawing.Size(298, 57);
            this.aliasTextBox.TabIndex = 4;
            // 
            // UserInfoDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 192);
            this.Controls.Add(this.aliasTextBox);
            this.Controls.Add(this.aliasLabel);
            this.Controls.Add(this.userIdLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.userImagePictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserInfoDialog";
            this.Text = "User Info";
            ((System.ComponentModel.ISupportInitialize)(this.userImagePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SmoothPictureBox userImagePictureBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label userIdLabel;
        private System.Windows.Forms.Label aliasLabel;
        private TextBoxWithoutCaret aliasTextBox;
    }
}