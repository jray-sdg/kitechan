using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Kitechan.Controls
{
    public partial class TextBoxWithoutCaret : TextBox
    {
        [DllImport("user32.dll")]
        private static extern bool HideCaret(IntPtr hwnd);

        [DllImport("user32.dll")]
        private static extern bool ShowCaret(IntPtr hwnd);

        public TextBoxWithoutCaret()
        {
            InitializeComponent();

            this.GotFocus += TextBoxWithoutCaret_GotFocus;
            this.LostFocus += TextBoxWithoutCaret_LostFocus;
        }

        private void TextBoxWithoutCaret_LostFocus(object sender, EventArgs e)
        {
            ShowCaret(this.Handle);
        }

        private void TextBoxWithoutCaret_GotFocus(object sender, EventArgs e)
        {
            HideCaret(this.Handle);
        }
    }
}
