namespace Kitechan
{
    partial class CommentControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.nameLabel = new System.Windows.Forms.Label();
            this.messageLabel = new System.Windows.Forms.LinkLabel();
            this.dateLabel = new System.Windows.Forms.Label();
            this.userIcon = new Kitechan.SmoothPictureBox();
            this.statusLabel = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showUserInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.muteUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.userIcon)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.Location = new System.Drawing.Point(62, 6);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(186, 22);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "[NAME]";
            // 
            // messageLabel
            // 
            this.messageLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageLabel.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageLabel.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.messageLabel.Location = new System.Drawing.Point(62, 28);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(186, 28);
            this.messageLabel.TabIndex = 1;
            this.messageLabel.Text = "[MESSAGE]";
            this.messageLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.messageLabel_LinkClicked);
            // 
            // dateLabel
            // 
            this.dateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateLabel.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateLabel.Location = new System.Drawing.Point(260, 6);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(114, 22);
            this.dateLabel.TabIndex = 2;
            this.dateLabel.Text = "[DATE]";
            this.dateLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // userIcon
            // 
            this.userIcon.ContextMenuStrip = this.contextMenuStrip;
            this.userIcon.Location = new System.Drawing.Point(6, 6);
            this.userIcon.MaximumSize = new System.Drawing.Size(50, 50);
            this.userIcon.MinimumSize = new System.Drawing.Size(50, 50);
            this.userIcon.Name = "userIcon";
            this.userIcon.Size = new System.Drawing.Size(50, 50);
            this.userIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.userIcon.TabIndex = 3;
            this.userIcon.TabStop = false;
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI Light", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.Location = new System.Drawing.Point(260, 28);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(114, 28);
            this.statusLabel.TabIndex = 4;
            this.statusLabel.Text = "[STATUS]";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.statusLabel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.statusLabel_MouseClick);
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showUserInfoToolStripMenuItem,
            this.muteUserToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(163, 48);
            // 
            // showUserInfoToolStripMenuItem
            // 
            this.showUserInfoToolStripMenuItem.Name = "showUserInfoToolStripMenuItem";
            this.showUserInfoToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.showUserInfoToolStripMenuItem.Text = "Show User Info...";
            this.showUserInfoToolStripMenuItem.Click += new System.EventHandler(this.showUserInfoToolStripMenuItem_Click);
            // 
            // muteUserToolStripMenuItem
            // 
            this.muteUserToolStripMenuItem.Name = "muteUserToolStripMenuItem";
            this.muteUserToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.muteUserToolStripMenuItem.Text = "Mute User";
            this.muteUserToolStripMenuItem.Click += new System.EventHandler(this.muteUserToolStripMenuItem_Click);
            // 
            // CommentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.messageLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.dateLabel);
            this.Controls.Add(this.userIcon);
            this.Controls.Add(this.statusLabel);
            this.MinimumSize = new System.Drawing.Size(380, 62);
            this.Name = "CommentControl";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(380, 62);
            this.Resize += new System.EventHandler(this.CommentControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.userIcon)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.LinkLabel messageLabel;
        private System.Windows.Forms.Label dateLabel;
        private SmoothPictureBox userIcon;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showUserInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem muteUserToolStripMenuItem;
    }
}
