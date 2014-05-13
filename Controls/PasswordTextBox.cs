using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kitechan.Controls
{
    public partial class PasswordTextBox : TextBox
    {
        private List<char> textChars;

        public IEnumerable<char> PasswordChars { get { return this.textChars; } }

        public PasswordTextBox()
        {
            InitializeComponent();

            this.textChars = new List<char>();
        }
        
        public new void Clear()
        {
            for (int x = 0; x < this.textChars.Count; x++)
            {
                this.textChars[x] = '\0';
            }
            this.textChars.Clear();
            base.Clear();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Back:
                    if (this.SelectionLength > 0)
                    {
                        this.RemoveSelectedChars();
                    }
                    else if (this.SelectionStart > 0)
                    {
                        int start = this.SelectionStart;
                        this.Text = this.Text.Remove(start - 1, 1);
                        this.textChars.RemoveAt(start - 1);
                        this.SelectionStart = start - 1;
                        this.SelectionLength = 0;
                    }
                    break;

                case Keys.Delete:
                    if (this.SelectionLength > 0)
                    {
                        this.RemoveSelectedChars();
                    }
                    else if (this.SelectionStart < this.Text.Length - 1)
                    {
                        int start = this.SelectionStart;
                        this.Text = this.Text.Remove(start, 1);
                        this.textChars.RemoveAt(start);
                        this.SelectionStart = start;
                        this.SelectionLength = 0;
                    }
                    break;

                case Keys.Left:
                    if (this.SelectionStart > 0)
                    {
                        this.SelectionStart--;
                        if (e.Shift)
                        {
                            this.SelectionLength++;
                        }
                        else
                        {
                            this.SelectionLength = 0;
                        }
                    }
                    break;

                case Keys.Right:
                    if (e.Shift)
                    {
                        this.SelectionLength++;
                    }
                    else if (this.SelectionStart < this.Text.Length - 1)
                    {
                        this.SelectionStart++;
                        this.SelectionLength = 0;
                    }
                    break;

                case Keys.Home:
                    int initial = this.SelectionStart;
                    this.SelectionStart = 0;
                    if (e.Shift)
                    {
                        this.SelectionLength = initial;
                    }
                    else
                    {
                        this.SelectionLength = 0;
                    }
                    break;

                case Keys.End:
                    if (e.Shift)
                    {
                        this.SelectionLength = this.Text.Length - this.SelectionStart;
                    }
                    else
                    {
                        this.SelectionStart = this.Text.Length;
                    }
                    break;
            }
            
            e.Handled = true;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (char.IsLetterOrDigit(e.KeyChar) || char.IsPunctuation(e.KeyChar) || char.IsSymbol(e.KeyChar) || e.KeyChar == ' ')
            {
                if (this.SelectionLength > 0)
                {
                    this.RemoveSelectedChars();
                }
                int start = this.SelectionStart;
                this.Text = this.Text.Insert(start, ".");
                this.textChars.Insert(start, e.KeyChar);
                this.SelectionStart = start + 1;
                this.SelectionLength = 0;
            }
            e.Handled = true;
        }

        private void RemoveSelectedChars()
        {
            if (this.SelectionLength > 0)
            {
                int start = this.SelectionStart;
                int length = this.SelectionLength;
                this.Text = this.Text.Remove(start, length);
                this.textChars.RemoveRange(start, length);
                this.SelectionStart = start;
                this.SelectionLength = 0;
            }
        }
    }
}
