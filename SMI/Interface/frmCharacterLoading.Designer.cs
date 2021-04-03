namespace KAI.Interface
{
    partial class frmCharacterLoading
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
            this.ppLoading = new DevExpress.XtraWaitForm.ProgressPanel();
            this.SuspendLayout();
            // 
            // ppLoading
            // 
            this.ppLoading.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.ppLoading.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ppLoading.Appearance.Options.UseBackColor = true;
            this.ppLoading.Appearance.Options.UseFont = true;
            this.ppLoading.AppearanceCaption.Font = new System.Drawing.Font("Verdana", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ppLoading.AppearanceCaption.Options.UseFont = true;
            this.ppLoading.AppearanceDescription.Font = new System.Drawing.Font("Verdana", 6.8F);
            this.ppLoading.AppearanceDescription.Options.UseFont = true;
            this.ppLoading.BarAnimationElementThickness = 2;
            this.ppLoading.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.ppLoading.Caption = "Just a moment";
            this.ppLoading.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.ppLoading.Description = "Loading character data...";
            this.ppLoading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppLoading.Location = new System.Drawing.Point(0, 0);
            this.ppLoading.LookAndFeel.SkinName = "Office 2016 Dark";
            this.ppLoading.LookAndFeel.UseDefaultLookAndFeel = false;
            this.ppLoading.Name = "ppLoading";
            this.ppLoading.Size = new System.Drawing.Size(215, 150);
            this.ppLoading.TabIndex = 1;
            this.ppLoading.Text = "progressPanel1";
            this.ppLoading.WaitAnimationType = DevExpress.Utils.Animation.WaitingAnimatorType.Line;
            // 
            // frmCharacterLoading
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(215, 150);
            this.Controls.Add(this.ppLoading);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "frmCharacterLoading";
            this.Opacity = 0.8D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gathering information..";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraWaitForm.ProgressPanel ppLoading;
    }
}