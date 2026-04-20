namespace QPMS.Win {
    partial class XafSplashScreen
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XafSplashScreen));
            this.progressBarControl = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.labelCopyright = new DevExpress.XtraEditors.LabelControl();
            this.labelStatus = new DevExpress.XtraEditors.LabelControl();
            this.peImage = new DevExpress.XtraEditors.PictureEdit();
            this.peLogo = new DevExpress.XtraEditors.PictureEdit();
            this.pcApplicationName = new DevExpress.XtraEditors.PanelControl();
            this.labelSubtitle = new DevExpress.XtraEditors.LabelControl();
            this.labelApplicationName = new DevExpress.XtraEditors.LabelControl();
            this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.peImage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.peLogo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcApplicationName)).BeginInit();
            this.pcApplicationName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBarControl
            // 
            this.progressBarControl.EditValue = 0;
            this.progressBarControl.Location = new System.Drawing.Point(24, 272);
            this.progressBarControl.Name = "progressBarControl";
            this.progressBarControl.Properties.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(194)))), ((int)(((byte)(194)))));
            this.progressBarControl.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.progressBarControl.Properties.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(114)))), ((int)(((byte)(0)))));
            this.progressBarControl.Properties.LookAndFeel.SkinName = "Visual Studio 2013 Blue";
            this.progressBarControl.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.UltraFlat;
            this.progressBarControl.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.progressBarControl.Properties.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid;
            this.progressBarControl.Properties.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(144)))), ((int)(((byte)(0)))));
            this.progressBarControl.Size = new System.Drawing.Size(563, 19);
            this.progressBarControl.TabIndex = 5;
            // 
            // labelCopyright
            // 
            this.labelCopyright.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.labelCopyright.Location = new System.Drawing.Point(24, 314);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new System.Drawing.Size(47, 13);
            this.labelCopyright.TabIndex = 6;
            this.labelCopyright.Text = "Copyright";
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(24, 253);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(50, 13);
            this.labelStatus.TabIndex = 7;
            this.labelStatus.Text = "Starting...";
            // 
            // peImage
            // 
            this.peImage.EditValue = ((object)(resources.GetObject("peImage.EditValue")));
            this.peImage.Location = new System.Drawing.Point(12, 12);
            this.peImage.Name = "peImage";
            this.peImage.Properties.AllowFocused = false;
            this.peImage.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.peImage.Properties.Appearance.Options.UseBackColor = true;
            this.peImage.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.peImage.Properties.ShowMenu = false;
            this.peImage.Size = new System.Drawing.Size(426, 180);
            this.peImage.TabIndex = 9;
            this.peImage.Visible = false;
            // 
            // peLogo
            // 
            this.peLogo.EditValue = ((object)(resources.GetObject("peLogo.EditValue")));
            this.peLogo.Location = new System.Drawing.Point(223, 314);
            this.peLogo.Name = "peLogo";
            this.peLogo.Properties.AllowFocused = false;
            this.peLogo.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.peLogo.Properties.Appearance.Options.UseBackColor = true;
            this.peLogo.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.peLogo.Properties.PictureAlignment = System.Drawing.ContentAlignment.BottomRight;
            this.peLogo.Properties.ShowMenu = false;
            this.peLogo.Size = new System.Drawing.Size(111, 32);
            this.peLogo.TabIndex = 8;
            // 
            // pcApplicationName
            // 
            this.pcApplicationName.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(114)))), ((int)(((byte)(0)))));
            this.pcApplicationName.Appearance.Options.UseBackColor = true;
            this.pcApplicationName.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pcApplicationName.Controls.Add(this.labelSubtitle);
            this.pcApplicationName.Controls.Add(this.labelApplicationName);
            this.pcApplicationName.Dock = System.Windows.Forms.DockStyle.Top;
            this.pcApplicationName.Location = new System.Drawing.Point(1, 1);
            this.pcApplicationName.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pcApplicationName.Name = "pcApplicationName";
            this.pcApplicationName.Size = new System.Drawing.Size(627, 220);
            this.pcApplicationName.TabIndex = 10;
            // 
            // labelSubtitle
            // 
            this.labelSubtitle.Appearance.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSubtitle.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(216)))), ((int)(((byte)(188)))));
            this.labelSubtitle.Appearance.Options.UseFont = true;
            this.labelSubtitle.Appearance.Options.UseForeColor = true;
            this.labelSubtitle.Location = new System.Drawing.Point(222, 131);
            this.labelSubtitle.Name = "labelSubtitle";
            this.labelSubtitle.Size = new System.Drawing.Size(184, 25);
            this.labelSubtitle.TabIndex = 1;
            this.labelSubtitle.Text = "Technology in Quality";
            // 
            // labelApplicationName
            // 
            this.labelApplicationName.Appearance.Font = new System.Drawing.Font("Segoe UI", 26.25F);
            this.labelApplicationName.Appearance.ForeColor = System.Drawing.SystemColors.Window;
            this.labelApplicationName.Appearance.Options.UseFont = true;
            this.labelApplicationName.Appearance.Options.UseForeColor = true;
            this.labelApplicationName.Location = new System.Drawing.Point(23, 76);
            this.labelApplicationName.Name = "labelApplicationName";
            this.labelApplicationName.Size = new System.Drawing.Size(563, 47);
            this.labelApplicationName.TabIndex = 0;
            this.labelApplicationName.Text = "Quality Project Management System";
            this.labelApplicationName.Click += new System.EventHandler(this.labelApplicationName_Click);
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.EditValue = ((object)(resources.GetObject("pictureEdit1.EditValue")));
            this.pictureEdit1.Location = new System.Drawing.Point(505, 314);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.AllowFocused = false;
            this.pictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pictureEdit1.Properties.PictureAlignment = System.Drawing.ContentAlignment.BottomRight;
            this.pictureEdit1.Properties.ShowMenu = false;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.pictureEdit1.Size = new System.Drawing.Size(82, 32);
            this.pictureEdit1.TabIndex = 11;
            // 
            // XafSplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(629, 370);
            this.Controls.Add(this.pictureEdit1);
            this.Controls.Add(this.pcApplicationName);
            this.Controls.Add(this.peImage);
            this.Controls.Add(this.peLogo);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelCopyright);
            this.Controls.Add(this.progressBarControl);
            this.Name = "XafSplashScreen";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.peImage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.peLogo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcApplicationName)).EndInit();
            this.pcApplicationName.ResumeLayout(false);
            this.pcApplicationName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.MarqueeProgressBarControl progressBarControl;
        private DevExpress.XtraEditors.LabelControl labelCopyright;
        private DevExpress.XtraEditors.LabelControl labelStatus;
        private DevExpress.XtraEditors.PictureEdit peLogo;
        private DevExpress.XtraEditors.PictureEdit peImage;
        private DevExpress.XtraEditors.PanelControl pcApplicationName;
        private DevExpress.XtraEditors.LabelControl labelSubtitle;
        private DevExpress.XtraEditors.LabelControl labelApplicationName;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
    }
}
