namespace Kbg.NppPluginNET
{
    partial class frmMyDlg
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
            this.parsedText = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // parsedText
            // 
            this.parsedText.BackColor = System.Drawing.SystemColors.Window;
            this.parsedText.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.parsedText.DetectUrls = false;
            this.parsedText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parsedText.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.parsedText.Location = new System.Drawing.Point(0, 0);
            this.parsedText.Name = "parsedText";
            this.parsedText.ReadOnly = true;
            this.parsedText.Size = new System.Drawing.Size(284, 735);
            this.parsedText.TabIndex = 0;
            this.parsedText.Text = "";
            // 
            // frmMyDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 735);
            this.Controls.Add(this.parsedText);
            this.Name = "frmMyDlg";
            this.Text = "frmMyDlg";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.RichTextBox parsedText;
    }
}