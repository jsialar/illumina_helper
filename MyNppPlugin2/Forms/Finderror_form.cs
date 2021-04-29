using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kbg.NppPluginNET
{
    public partial class Finderror_form : Form
    {
        public Finderror_form()
        {
            InitializeComponent();
        }

        private void findresults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Finderror.foundpos_l.Count > 0)            
            {
                int linepos = Finderror.foundpos_l[listbox_findresults.SelectedIndex];
                Finderror.editor.GotoLine(linepos);
            }
            
            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Finderror.find(worker, e);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            if (e.Cancelled)
            {
                return;
            }
            Finderror.updatedisplay();
        }
    }
}
