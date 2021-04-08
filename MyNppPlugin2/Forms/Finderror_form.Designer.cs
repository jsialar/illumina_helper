
namespace Kbg.NppPluginNET
{
    partial class Finderror_form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listbox_findresults = new System.Windows.Forms.ListBox();
            GenerateBGW();
            this.SuspendLayout();
            // 
            // listbox_findresults
            // 
            this.listbox_findresults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listbox_findresults.FormattingEnabled = true;
            this.listbox_findresults.HorizontalScrollbar = true;
            this.listbox_findresults.ItemHeight = 22;
            this.listbox_findresults.Location = new System.Drawing.Point(0, 0);
            this.listbox_findresults.Margin = new System.Windows.Forms.Padding(4);
            this.listbox_findresults.Name = "listbox_findresults";
            this.listbox_findresults.Size = new System.Drawing.Size(1000, 619);
            this.listbox_findresults.TabIndex = 0;
            this.listbox_findresults.SelectedIndexChanged += new System.EventHandler(this.findresults_SelectedIndexChanged);

            // 
            // Finderror_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1000, 619);
            this.Controls.Add(this.listbox_findresults);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Finderror_form";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        internal void GenerateBGW()
        {
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            
        }

        #endregion

        internal System.Windows.Forms.ListBox listbox_findresults;
        internal System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}