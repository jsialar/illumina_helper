using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Kbg.NppPluginNET
{
    public partial class frmMyDlg : Form
    {
        public frmMyDlg()
        {
            InitializeComponent();
        }

        private void btn_copyrunparam_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(parsedText.Text);
        }
    }
}
