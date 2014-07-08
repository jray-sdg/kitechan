using Kitechan.Properties;
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
    public partial class UserInfoDialog : Form
    {
        public UserInfoDialog()
        {
            InitializeComponent();

            this.aliasLabel.Text = Resources.UserInfoAliasLabel;
        }

        public void SetUserInfo(UserInfo userInfo)
        {
            this.userImagePictureBox.BorderStyle = BorderStyle.FixedSingle;
            this.userImagePictureBox.Image = null;
            this.nameLabel.Text = string.Empty;
            this.userIdLabel.Text = string.Empty;
            this.aliasTextBox.Text = string.Empty;
            if (userInfo != null)
            {
                this.nameLabel.Text = userInfo.Name;
                this.userIdLabel.Text = userInfo.Id.ToString();
                if (userInfo.Aliases != null && userInfo.Aliases.Count > 0)
                {
                    this.aliasTextBox.Text = string.Join(Environment.NewLine, userInfo.Aliases);
                }
                if (userInfo.UserImage != null)
                {
                    this.userImagePictureBox.BorderStyle = BorderStyle.None;
                    this.userImagePictureBox.Image = userInfo.UserImage;
                }
            }
        }
    }
}
