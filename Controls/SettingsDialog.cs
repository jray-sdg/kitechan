using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kitechan.Types;

namespace Kitechan.Controls
{
    public partial class SettingsDialog : Form
    {
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

        public SettingsDialog()
        {
            InitializeComponent();
        }

        private void UpdateSettings()
        {
        }
    }
}
