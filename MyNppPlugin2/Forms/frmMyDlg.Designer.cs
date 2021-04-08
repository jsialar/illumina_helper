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
            this.btn_copyrunparam = new System.Windows.Forms.Button();
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
            this.parsedText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.parsedText.Name = "parsedText";
            this.parsedText.ReadOnly = true;
            this.parsedText.Size = new System.Drawing.Size(379, 905);
            this.parsedText.TabIndex = 0;
            this.parsedText.Text = "";
            // 
            // btn_copyrunparam
            // 
            this.btn_copyrunparam.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_copyrunparam.Location = new System.Drawing.Point(144, 866);
            this.btn_copyrunparam.Name = "btn_copyrunparam";
            this.btn_copyrunparam.Size = new System.Drawing.Size(75, 34);
            this.btn_copyrunparam.TabIndex = 1;
            this.btn_copyrunparam.Text = "Copy";
            this.btn_copyrunparam.UseVisualStyleBackColor = true;
            this.btn_copyrunparam.Click += new System.EventHandler(this.btn_copyrunparam_Click);
            // 
            // frmMyDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 905);
            this.Controls.Add(this.btn_copyrunparam);
            this.Controls.Add(this.parsedText);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmMyDlg";
            this.Text = "frmMyDlg";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.RichTextBox parsedText;
        private System.Windows.Forms.Button btn_copyrunparam;
    }
}