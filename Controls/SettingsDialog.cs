using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kitechan.Types;
using Kitechan.Properties;

namespace Kitechan.Controls
{
    public partial class SettingsDialog : Form
    {
        private class MutedUserEntry
        {
            public int Id { get; private set; }
            public string Name { get; private set; }

            public MutedUserEntry(int i, string n)
            {
                this.Id = i;
                this.Name = n;
            }

            public override string ToString()
            {
                return !string.IsNullOrEmpty(this.Name) ? this.Name : this.Id.ToString();
            }
        }

        private UserInfoDelegate getUserInfo;

        private ClientSettings settings;

        public ClientSettings Settings
        {
            get
            {
                return this.settings;
            }
            set
            {
                if (value == null)
                {
                    this.settings = null;
                }
                else
                {
                    this.settings = value.Clone();
                }
                this.UpdateSettings();
            }
        }

        public SettingsDialog(UserInfoDelegate userInfo)
        {
            InitializeComponent();

            this.getUserInfo = userInfo;

            this.okButton.Text = Resources.OkButton;
            this.cancelButton.Text = Resources.CancelButton;

            this.mutedUsersGroupBox.Text = Resources.MutedUsersPrompt;
            this.removeMutedUserButton.Text = Resources.RemoveButton;
            this.removeAllMutedUsersButton.Text = Resources.RemoveAllButton;

            this.loadHistoryCheckBox.Text = Resources.LoadPastMessagesCheckbox;
        }

        private void UpdateSettings()
        {
            this.mutedUsersListBox.Items.Clear();
            foreach (int user in this.Settings.MutedUsers)
            {
                UserInfo info = null;
                if (this.getUserInfo != null)
                {
                    info = this.getUserInfo(user);
                }
                this.mutedUsersListBox.Items.Add(new MutedUserEntry(user, info != null ? info.Name : null));
            }

            this.loadHistoryCheckBox.Checked = this.Settings.LoadChatHistory;
        }

        private void removeMutedUserButton_Click(object sender, EventArgs e)
        {
            if (this.mutedUsersListBox.SelectedItem != null)
            {
                MutedUserEntry entry = (MutedUserEntry)this.mutedUsersListBox.SelectedItem;
                this.mutedUsersListBox.Items.Remove(this.mutedUsersListBox.SelectedItem);
                this.Settings.MutedUsers.Remove(entry.Id);
            }
        }

        private void removeAllMutedUsersButton_Click(object sender, EventArgs e)
        {
            this.mutedUsersListBox.Items.Clear();
            this.Settings.MutedUsers.Clear();
        }

        private void loadHistoryCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.Settings.LoadChatHistory = this.loadHistoryCheckBox.Checked;
        }
    }
}
