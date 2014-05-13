﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kitechan
{
    public partial class CommentControl : UserControl
    {
        public event EventHandler<HeartCommentEventArgs> HeartCommentEvent;

        private UserInfoDelegate getUserInfo;

        public int CommentId { get; private set; }

        public int UserId { get; private set; }

        public CommentControl(UserInfoDelegate userInfo)
        {
            InitializeComponent();
            this.getUserInfo = userInfo;
        }

        public void SetComment(Comment comment)
        {
            this.CommentId = comment.Id;
            this.UserId = comment.UserId;
            this.nameLabel.Text = comment.Name;
            this.SetMessage(comment.Message);
            this.dateLabel.Text = comment.PostedTime.ToString();
            this.userIcon.Image = null;
            this.statusLabel.Text = "0 hearts";
            this.statusLabel.ForeColor = Color.Black;
            this.toolTip.SetToolTip(this.statusLabel, string.Empty);
            if (this.getUserInfo != null)
            {
                UserInfo info = this.getUserInfo(comment.UserId);
                if (info != null)
                {
                    if (info.UserImage != null)
                    {
                        this.userIcon.Image = info.UserImage;
                    }
                    else
                    {
                        info.LoadImage(); // event firing too soon?
                    }
                }
            }
        }

        public void SetSystemMessage(string message)
        {
            this.CommentId = -1;
            this.UserId = -1;
            this.nameLabel.Text = "SYSTEM";
            this.SetMessage(message);
            this.dateLabel.Text = DateTime.Now.ToString();
            this.userIcon.Image = null;
            this.statusLabel.Text = string.Empty;
            this.statusLabel.ForeColor = Color.Black;
            this.toolTip.SetToolTip(this.statusLabel, string.Empty);
        }

        private void SetMessage(string message)
        {
            this.Height = this.MinimumSize.Height;
            SizeF stringSize = this.messageLabel.CreateGraphics().MeasureString(message, this.messageLabel.Font, this.messageLabel.Width); // failing because width isn't set yet?
            this.messageLabel.Text = message;
            if (stringSize.Height > this.messageLabel.Height)
            {
                int diff = (int)(stringSize.Height - this.messageLabel.Height);
                this.Height = this.Height + diff;
            }
        }

        public void UpdateImage(Image image)
        {
            this.userIcon.Image = image;
        }

        public void UpdateHearts(IEnumerable<int> userIds)
        {
            string heartingUsers;
            if (this.getUserInfo != null)
            {
                heartingUsers = string.Join(", ", userIds.Select((x) => this.getUserInfo(x).Name));
            }
            else
            {
                heartingUsers = string.Join(", ", userIds.Select((x) => x.ToString()));
            }
            this.statusLabel.Text = userIds.Count().ToString() + " hearts";
            this.toolTip.SetToolTip(this.statusLabel, heartingUsers);
        }

        public void CommentDeleted()
        {
            this.statusLabel.Text = "deleted";
            this.statusLabel.ForeColor = Color.DarkRed;
        }

        private void statusLabel_Click(object sender, EventArgs e)
        {
            if (this.HeartCommentEvent != null && this.CommentId != -1)
            {
                this.HeartCommentEvent(this, new HeartCommentEventArgs(this.CommentId));
            }
        }
    }
}