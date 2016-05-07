namespace EgaViewer_v2
{
    partial class FormMain
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelFileInfo = new System.Windows.Forms.Label();
            this.listBoxFiles = new System.Windows.Forms.ListBox();
            this.statusStripApp = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.trackBarZoom = new System.Windows.Forms.TrackBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pixelFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cGAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eGAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CGAPaletteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.type2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.type3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.type4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.palette2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.palette2HighIntensityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBoxEGA = new EgaViewer_v2.CustomPictureBox();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            this.statusStripApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEGA)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.labelFileInfo);
            this.groupBox1.Controls.Add(this.listBoxFiles);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 507);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Files";
            // 
            // labelFileInfo
            // 
            this.labelFileInfo.Location = new System.Drawing.Point(6, 419);
            this.labelFileInfo.Name = "labelFileInfo";
            this.labelFileInfo.Size = new System.Drawing.Size(188, 85);
            this.labelFileInfo.TabIndex = 2;
            this.labelFileInfo.Text = "N/A";
            // 
            // listBoxFiles
            // 
            this.listBoxFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxFiles.FormattingEnabled = true;
            this.listBoxFiles.Location = new System.Drawing.Point(6, 22);
            this.listBoxFiles.Name = "listBoxFiles";
            this.listBoxFiles.Size = new System.Drawing.Size(188, 394);
            this.listBoxFiles.TabIndex = 1;
            this.listBoxFiles.DoubleClick += new System.EventHandler(this.listBoxFiles_DoubleClick);
            // 
            // statusStripApp
            // 
            this.statusStripApp.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusText});
            this.statusStripApp.Location = new System.Drawing.Point(0, 537);
            this.statusStripApp.Name = "statusStripApp";
            this.statusStripApp.Size = new System.Drawing.Size(941, 22);
            this.statusStripApp.TabIndex = 2;
            this.statusStripApp.Text = "statusStrip1";
            // 
            // toolStripStatusText
            // 
            this.toolStripStatusText.Name = "toolStripStatusText";
            this.toolStripStatusText.Size = new System.Drawing.Size(143, 17);
            this.toolStripStatusText.Text = "Double click a file to load.";
            // 
            // trackBarZoom
            // 
            this.trackBarZoom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarZoom.Location = new System.Drawing.Point(219, 489);
            this.trackBarZoom.Maximum = 100;
            this.trackBarZoom.Minimum = 1;
            this.trackBarZoom.Name = "trackBarZoom";
            this.trackBarZoom.Size = new System.Drawing.Size(710, 45);
            this.trackBarZoom.TabIndex = 4;
            this.trackBarZoom.Value = 1;
            this.trackBarZoom.ValueChanged += new System.EventHandler(this.trackBarZoom_ValueChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.formatToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(941, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openPathToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openPathToolStripMenuItem
            // 
            this.openPathToolStripMenuItem.Name = "openPathToolStripMenuItem";
            this.openPathToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.openPathToolStripMenuItem.Text = "Open Path...";
            this.openPathToolStripMenuItem.Click += new System.EventHandler(this.openPathToolStripMenuItem_Click);
            // 
            // formatToolStripMenuItem
            // 
            this.formatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pixelFormatToolStripMenuItem,
            this.CGAPaletteToolStripMenuItem});
            this.formatToolStripMenuItem.Name = "formatToolStripMenuItem";
            this.formatToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.formatToolStripMenuItem.Text = "Format";
            // 
            // pixelFormatToolStripMenuItem
            // 
            this.pixelFormatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cGAToolStripMenuItem,
            this.eGAToolStripMenuItem});
            this.pixelFormatToolStripMenuItem.Name = "pixelFormatToolStripMenuItem";
            this.pixelFormatToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.pixelFormatToolStripMenuItem.Text = "GET Pixel Format";
            // 
            // cGAToolStripMenuItem
            // 
            this.cGAToolStripMenuItem.Name = "cGAToolStripMenuItem";
            this.cGAToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.cGAToolStripMenuItem.Text = "CGA (Screen 1)";
            this.cGAToolStripMenuItem.Click += new System.EventHandler(this.MenuItem_Options_Click);
            // 
            // eGAToolStripMenuItem
            // 
            this.eGAToolStripMenuItem.Checked = true;
            this.eGAToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.eGAToolStripMenuItem.Name = "eGAToolStripMenuItem";
            this.eGAToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.eGAToolStripMenuItem.Text = "EGA (Screen 7)";
            this.eGAToolStripMenuItem.Click += new System.EventHandler(this.MenuItem_Options_Click);
            // 
            // CGAPaletteToolStripMenuItem
            // 
            this.CGAPaletteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.type2ToolStripMenuItem,
            this.type3ToolStripMenuItem,
            this.type4ToolStripMenuItem,
            this.palette2ToolStripMenuItem,
            this.palette2HighIntensityToolStripMenuItem});
            this.CGAPaletteToolStripMenuItem.Name = "CGAPaletteToolStripMenuItem";
            this.CGAPaletteToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.CGAPaletteToolStripMenuItem.Text = "CGA Palette";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(196, 22);
            this.toolStripMenuItem2.Text = "Palette 0";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.MenuItem_Options_Click);
            // 
            // type2ToolStripMenuItem
            // 
            this.type2ToolStripMenuItem.Checked = true;
            this.type2ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.type2ToolStripMenuItem.Name = "type2ToolStripMenuItem";
            this.type2ToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.type2ToolStripMenuItem.Text = "Palette 0 High Intensity";
            this.type2ToolStripMenuItem.Click += new System.EventHandler(this.MenuItem_Options_Click);
            // 
            // type3ToolStripMenuItem
            // 
            this.type3ToolStripMenuItem.Name = "type3ToolStripMenuItem";
            this.type3ToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.type3ToolStripMenuItem.Text = "Palette 1";
            this.type3ToolStripMenuItem.Click += new System.EventHandler(this.MenuItem_Options_Click);
            // 
            // type4ToolStripMenuItem
            // 
            this.type4ToolStripMenuItem.Name = "type4ToolStripMenuItem";
            this.type4ToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.type4ToolStripMenuItem.Text = "Palette 1 High Intensity";
            this.type4ToolStripMenuItem.Click += new System.EventHandler(this.MenuItem_Options_Click);
            // 
            // palette2ToolStripMenuItem
            // 
            this.palette2ToolStripMenuItem.Name = "palette2ToolStripMenuItem";
            this.palette2ToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.palette2ToolStripMenuItem.Text = "Palette 2";
            this.palette2ToolStripMenuItem.Click += new System.EventHandler(this.MenuItem_Options_Click);
            // 
            // palette2HighIntensityToolStripMenuItem
            // 
            this.palette2HighIntensityToolStripMenuItem.Name = "palette2HighIntensityToolStripMenuItem";
            this.palette2HighIntensityToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.palette2HighIntensityToolStripMenuItem.Text = "Palette 2 High Intensity";
            this.palette2HighIntensityToolStripMenuItem.Click += new System.EventHandler(this.MenuItem_Options_Click);
            // 
            // pictureBoxEGA
            // 
            this.pictureBoxEGA.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxEGA.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            this.pictureBoxEGA.Location = new System.Drawing.Point(219, 27);
            this.pictureBoxEGA.Name = "pictureBoxEGA";
            this.pictureBoxEGA.Size = new System.Drawing.Size(710, 456);
            this.pictureBoxEGA.TabIndex = 3;
            this.pictureBoxEGA.TabStop = false;
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.aboutToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 559);
            this.Controls.Add(this.trackBarZoom);
            this.Controls.Add(this.pictureBoxEGA);
            this.Controls.Add(this.statusStripApp);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.groupBox1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "PantherTek CGA/EGA BSAVE Viewer v2";
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            this.groupBox1.ResumeLayout(false);
            this.statusStripApp.ResumeLayout(false);
            this.statusStripApp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarZoom)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEGA)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBoxFiles;
        private System.Windows.Forms.StatusStrip statusStripApp;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusText;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private CustomPictureBox pictureBoxEGA;
        private System.Windows.Forms.TrackBar trackBarZoom;
        private System.Windows.Forms.Label labelFileInfo;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem formatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pixelFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cGAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eGAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CGAPaletteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem type2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem type3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem type4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem palette2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem palette2HighIntensityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
    }
}

