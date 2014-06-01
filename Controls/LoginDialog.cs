using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kitechan.Controls
{
    public partial class LoginDialog : Form
    {
        private ValidateCredentialsDelegate validateCredentials;

        public LoginDialog(ValidateCredentialsDelegate credentials)
        {
            InitializeComponent();
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
                MessageBox.Show("Could not log in with the specified credentials", "Log in failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
