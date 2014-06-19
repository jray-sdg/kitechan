using Kitechan.Controls;
using Kitechan.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kitechan
{
    public partial class MainForm : Form
    {
        private Engine engine;

        private LoginDialog loginDialog;

        private Queue<CommentControl> controlPool;

        private Dictionary<int, List<int>> userLookup;

        private Dictionary<int, CommentControl> commentLookup;

        public MainForm()
        {
            InitializeComponent();

            this.userLookup = new Dictionary<int, List<int>>();
            this.commentLookup = new Dictionary<int, CommentControl>();

            this.engine = new Engine();
            this.engine.NewMessageEvent += engine_NewMessageEvent;
            this.engine.SystemMessageEvent += engine_SystemMessageEvent;
            this.engine.BroadcastStartEvent += engine_BroadcastStartEvent;
            this.engine.ImageLoadedEvent += engine_ImageLoadedEvent;
            this.engine.CommentDeletedEvent += engine_CommentDeletedEvent;
            this.engine.CommentHeartedEvent += engine_CommentHeartedEvent;
            this.engine.StreamHeartedEvent += engine_StreamHeartedEvent;

            this.loginDialog = new LoginDialog(this.engine.ValidateCredentials);

            this.controlPool = new Queue<CommentControl>(105);
            for (int x = 0; x < 105; x++)
            {
                CommentControl commentControl = new CommentControl(this.engine.GetUserInfo);
                commentControl.HeartCommentEvent += commentControl_HeartCommentEvent;
                commentControl.UnheartCommentEvent += commentControl_UnheartCommentEvent;
                this.controlPool.Enqueue(commentControl);
            }
            this.streamNameLabel.Text = "Jeff Gerstmann's Mixlr";
            this.streamHeartsLabel.Text = "♥ 0";
            this.engine.GetUserInfo(Engine.JeffUserId).LoadImage();
            if (this.engine.LoggedIn)
            {
                UserInfo loggedInUser = this.engine.GetUserInfo(this.engine.LoggedInUserId);
                loggedInUser.LoadImage();
                this.loginLabel.Text = "Logged in as";
                this.loggedInLabel.Text = loggedInUser.Name;
            }
            else
            {
                this.loginLabel.Text = "Login";
                this.loggedInLabel.Text = string.Empty;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.engine.Connect();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.engine.Disconnect();
            this.engine.SaveState();
        }

        private void engine_NewMessageEvent(object sender, NewMessageEventArgs e)
        {
            this.NewComment(e.Comment);
        }

        private void engine_SystemMessageEvent(object sender, SystemMessageEventArgs e)
        {
            this.PrintLine(e.Message);
        }

        private void engine_BroadcastStartEvent(object sender, BroadcastStartEventArgs e)
        {
            this.streamNameLabel.Text = e.BroadcastTitle;
        }

        private void engine_CommentDeletedEvent(object sender, CommentDeletedEventArgs e)
        {
            this.DeleteComment(e.CommentId);
        }

        private void engine_CommentHeartedEvent(object sender, CommentHeartedEventArgs e)
        {
            this.UpdateCommentHearts(e.CommentId, e.UserIds);
        }

        private void engine_StreamHeartedEvent(object sender, StreamHeartedEventArgs e)
        {
            this.UpdateStreamHearts(e.GrandTotal);
        }

        private void engine_ImageLoadedEvent(object sender, ImageLoadedEventArgs e)
        {
            this.UpdateUserImage(e.UserId);
        }

        private void commentControl_HeartCommentEvent(object sender, HeartCommentEventArgs e)
        {
            this.engine.HeartComment(e.CommentId);
        }

        private void commentControl_UnheartCommentEvent(object sender, UnheartCommentEventArgs e)
        {
            this.engine.UnheartComment(e.CommentId);
        }

        private void streamHeartsLabel_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.engine.LoggedIn && !string.IsNullOrEmpty(this.engine.BroadcastId))
            {
                if (e.Button == MouseButtons.Left)
                {
                    this.engine.HeartStream();
                }
                else if (e.Button == MouseButtons.Right)
                {
                    this.autoheartTimer.Start();
                }
            }
        }

        private void autoheartTimer_Tick(object sender, EventArgs e)
        {
            this.engine.HeartStream();
        }

        private void PrintLine(string message)
        {
            if (this.bodyPanel.InvokeRequired)
            {
                this.bodyPanel.Invoke((MethodInvoker)(() => this.PrintLine(message)));
            }
            else
            {
                CommentControl c = this.GetCommentControl();
                c.Width = this.bodyPanel.Width;
                c.SetSystemMessage(message);
                c.Dock = DockStyle.Top;
                this.bodyPanel.Controls.Add(c);
            }
        }

        private void UpdateStreamHearts(int grandTotal)
        {
            if (this.streamHeartsLabel.InvokeRequired)
            {
                this.streamHeartsLabel.Invoke((MethodInvoker)(() => this.UpdateStreamHearts(grandTotal)));
            }
            else
            {
                this.streamHeartsLabel.Text = "♥ " + grandTotal;
            }
        }

        private void UpdateUserImage(int userId)
        {
            if (this.jeffPictureBox.InvokeRequired)
            {
                this.jeffPictureBox.Invoke((MethodInvoker)(() => this.UpdateUserImage(userId)));
            }
            else
            {
                Image image = this.engine.GetUserInfo(userId).UserImage;
                if (userId == Engine.JeffUserId)
                {
                    this.jeffPictureBox.Image = image;
                }
                if (this.engine.LoggedIn && userId == this.engine.LoggedInUserId)
                {
                    this.loggedInPictureBox.BorderStyle = BorderStyle.None;
                    this.loggedInPictureBox.Image = image;
                }
                if (this.userLookup.ContainsKey(userId))
                {
                    foreach (int comment in this.userLookup[userId])
                    {
                        this.commentLookup[comment].UpdateImage(image);
                    }
                }
            }
        }

        private void NewComment(Comment comment)
        {
            if (this.bodyPanel.InvokeRequired)
            {
                this.bodyPanel.Invoke((MethodInvoker)(() => this.NewComment(comment)));
            }
            else
            {
                CommentControl c = this.GetCommentControl();
                c.Width = this.bodyPanel.Width;
                c.SetComment(comment);
                c.Dock = DockStyle.Top;
                this.bodyPanel.Controls.Add(c);
                if (!this.userLookup.ContainsKey(comment.UserId))
                {
                    this.userLookup.Add(comment.UserId, new List<int>());
                }
                this.userLookup[comment.UserId].Add(comment.Id);
                this.commentLookup.Add(comment.Id, c);
            }
        }

        public void UpdateCommentHearts(int commentId, IEnumerable<int> heartingUsers)
        {
            if (this.bodyPanel.InvokeRequired)
            {
                this.bodyPanel.Invoke((MethodInvoker)(() => this.UpdateCommentHearts(commentId, heartingUsers)));
            }
            else
            {
                if (this.commentLookup.ContainsKey(commentId))
                {
                    this.commentLookup[commentId].UpdateHearts(heartingUsers);
                }
            }
        }

        private void DeleteComment(int commentId)
        {
            if (this.commentLookup.ContainsKey(commentId))
            {
                if (this.commentLookup[commentId].InvokeRequired)
                {
                    this.commentLookup[commentId].Invoke((MethodInvoker)(() => this.DeleteComment(commentId)));
                }
                else
                {
                    this.commentLookup[commentId].CommentDeleted();
                }
            }
        }

        private CommentControl GetCommentControl()
        {
            if (this.controlPool.Count < 5)
            {
                for (int x = 0; x < 5; x++)
                {
                    CommentControl c = (CommentControl)this.bodyPanel.Controls[0];
                    this.bodyPanel.Controls.RemoveAt(0);
                    this.controlPool.Enqueue(c);
                    if (c.CommentId > -1)
                    {
                        this.commentLookup.Remove(c.CommentId);
                    }
                    if (c.UserId > -1)
                    {
                        this.userLookup[c.UserId].Remove(c.CommentId);
                    }
                }
            }
            return this.controlPool.Dequeue();
        }

        private void postButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.commentTextBox.Text))
            {
                this.engine.PostComment(this.commentTextBox.Text);
                this.commentTextBox.Text = string.Empty;
            }
        }

        private void commentTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Control)
                {
                    this.commentTextBox.Text += Environment.NewLine;
                    this.commentTextBox.SelectionStart += Environment.NewLine.Length;
                }
                else
                {
                    this.postButton.PerformClick();
                }
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
            else if (e.Control)
            {
                if (e.KeyCode == Keys.A)
                {
                    this.commentTextBox.SelectAll();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
        }

        private void loginLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult loginResult = this.loginDialog.ShowDialog();
            this.loginDialog.Clear();
            if (loginResult == DialogResult.OK)
            {
                this.loginLabel.Text = "Logged in as";
                this.loggedInLabel.Text = this.engine.GetUserInfo(this.engine.LoggedInUserId).Name;
            }
        }
    }
}
