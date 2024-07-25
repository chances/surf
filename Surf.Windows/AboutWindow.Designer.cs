namespace Surf.Windows
{
    partial class AboutWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.IconAttribution = new System.Windows.Forms.Label();
            this.Icons8Link = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            //
            // IconAttribution
            //
            this.IconAttribution.Text =  "Icons by ";
            this.IconAttribution.Location = new System.Drawing.Point(12,236);
            this.IconAttribution.Size = new System.Drawing.Size(52,16);
            this.IconAttribution.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            //
            // Icons8Link
            //
            this.Icons8Link.TabStop =  true;
            this.Icons8Link.Text =  "Icons8";
            this.Icons8Link.AutoSize =  true;
            this.Icons8Link.Location = new System.Drawing.Point(60,236);
            this.Icons8Link.Size = new System.Drawing.Size(41,15);
            this.Icons8Link.TabIndex = 1;
            this.Icons8Link.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         //
         // form
         //
            this.Size = new System.Drawing.Size(250,300);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text =  "About Surf";
            this.Controls.Add(this.IconAttribution);
            this.Controls.Add(this.Icons8Link);
            this.ResumeLayout(false);
        } 

        #endregion 

        private System.Windows.Forms.Label IconAttribution;
        private System.Windows.Forms.LinkLabel Icons8Link;
    }
}

