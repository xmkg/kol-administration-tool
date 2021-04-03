namespace KAI.Interface
{
    partial class frmNotification
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
            this.gcNotification = new DevExpress.XtraEditors.GroupControl();
            this.ppOnline = new DevExpress.XtraWaitForm.ProgressPanel();
            ((System.ComponentModel.ISupportInitialize)(this.gcNotification)).BeginInit();
            this.gcNotification.SuspendLayout();
            this.SuspendLayout();
            // 
            // gcNotification
            // 
            this.gcNotification.Controls.Add(this.ppOnline);
            this.gcNotification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcNotification.Location = new System.Drawing.Point(0, 0);
            this.gcNotification.Name = "gcNotification";
            this.gcNotification.Size = new System.Drawing.Size(320, 140);
            this.gcNotification.TabIndex = 0;
            this.gcNotification.Text = "Knight Administration Interface";
            // 
            // ppOnline
            // 
            this.ppOnline.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.ppOnline.Appearance.Font = new System.Drawing.Font("Verdana", 10F);
            this.ppOnline.Appearance.Options.UseBackColor = true;
            this.ppOnline.Appearance.Options.UseFont = true;
            this.ppOnline.AppearanceCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ppOnline.AppearanceCaption.Options.UseFont = true;
            this.ppOnline.AppearanceDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.ppOnline.AppearanceDescription.Options.UseFont = true;
            this.ppOnline.Caption = "mkg";
            this.ppOnline.Description = "is now online";
            this.ppOnline.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppOnline.Location = new System.Drawing.Point(2, 26);
            this.ppOnline.LookAndFeel.SkinName = "The Asphalt World";
            this.ppOnline.LookAndFeel.UseDefaultLookAndFeel = false;
            this.ppOnline.Name = "ppOnline";
            this.ppOnline.Size = new System.Drawing.Size(316, 112);
            this.ppOnline.TabIndex = 0;
            this.ppOnline.Text = "is now online";
            this.ppOnline.Click += new System.EventHandler(this.ppOnline_Click);
            // 
            // frmNotification
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 140);
            this.Controls.Add(this.gcNotification);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "frmNotification";
            this.Text = "frmNotification";
            this.Load += new System.EventHandler(this.frmNotification_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gcNotification)).EndInit();
            this.gcNotification.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl gcNotification;
        private DevExpress.XtraWaitForm.ProgressPanel ppOnline;
    }
}