using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kitechan.Properties;

namespace Kitechan.Controls
{
    public partial class LoginDialog : Form
    {
        private ValidateCredentialsDelegate validateCredentials;

        public LoginDialog(ValidateCredentialsDelegate credentials)
        {
            InitializeComponent();

            this.Text = Resources.LoginTitle;
            this.userLabel.Text = Resources.UserNamePrompt;
            this.passwordLabel.Text = Resources.PasswordPrompt;
            this.okButton.Text = Resources.LoginButton;
            this.cancelButton.Text = Resources.CancelButton;

            this.validateCredentials = credentials;
        }

        public void Clear()
        {
            this.userTextBox.Text = string.Empty;
            this.passwordTextBox.Clear();
        }

        private void userTextBox_TextChanged(object sender, EventArgs e)
        {
            this.AllowOkButton();
        }

        private void passwordTextBox_TextChanged(object sender, EventArgs e)
        {
            this.AllowOkButton();
        }

        private void userTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && this.okButton.Enabled)
            {
                this.okButton.PerformClick();
            }
        }

        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && this.okButton.Enabled)
            {
                this.okButton.PerformClick();
            }
        }

        private void AllowOkButton()
        {
            this.okButton.Enabled = this.userTextBox.Text.Length > 0 && this.passwordTextBox.Text.Length > 0;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            bool validated = this.validateCredentials(this.userTextBox.Text, this.passwordTextBox.PasswordChars);
            this.Enabled = true;
            if (validated)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(Resources.LoginFailedMessage, Resources.LoginFailedTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
