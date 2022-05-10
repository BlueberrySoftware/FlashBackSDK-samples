
namespace RecorderExtended
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label2 = new System.Windows.Forms.Label();
            this.cbProfiles = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbOutputFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ssLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlAll = new System.Windows.Forms.Panel();
            this.pnlParameters = new System.Windows.Forms.Panel();
            this.cbVideoDeviceStreamFormats = new System.Windows.Forms.ComboBox();
            this.lblVideoDeviceStreamFormats = new System.Windows.Forms.Label();
            this.cbVideoDeviceStreams = new System.Windows.Forms.ComboBox();
            this.lblVideoDeviceStreams = new System.Windows.Forms.Label();
            this.chkMouseCursor = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.numRegionTop = new System.Windows.Forms.NumericUpDown();
            this.numRegionLeft = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbD3D11 = new System.Windows.Forms.RadioButton();
            this.rbD3D9 = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.numRegionHeight = new System.Windows.Forms.NumericUpDown();
            this.numRegionWidth = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.numOutputHeight = new System.Windows.Forms.NumericUpDown();
            this.numOutputWidth = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.pnlCaptureMode = new System.Windows.Forms.Panel();
            this.rbModeDuplicationAPI = new System.Windows.Forms.RadioButton();
            this.rbModeGDI = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.cbAACEncoders = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbAudioSources = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbVideoEncoders = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbVideoCaptureDevices = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.statusStrip1.SuspendLayout();
            this.pnlAll.SuspendLayout();
            this.pnlParameters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRegionTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRegionLeft)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRegionHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRegionWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOutputHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOutputWidth)).BeginInit();
            this.pnlCaptureMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Profile:";
            // 
            // cbProfiles
            // 
            this.cbProfiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbProfiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfiles.Items.AddRange(new object[] {
            "GDI mode, screen only, software h264 encoder (SAFE_SCREEN)",
            "Duplication Output mode, screen only, hardware h264 encoder (HIGH_SCREEN)",
            "GDI mode, screen, PC Sounds, Mic, software h264 encoder (SAFE_SCREEN_AUDIO)",
            "Duplication Output mode, screen, PC Sounds, Mic, hardware h264 encoder (HIGH_SCRE" +
                "EN_AUDIO)",
            "GDI mode, screen, PC Sounds, Mic, WebCam, software h264 encoder (SAFE_SCREEN_AUDI" +
                "O_VIDEOCAPTURE)",
            "Duplication Output mode, screen, PC Sounds, Mic, WebCam, hardware h264 encoder (H" +
                "IGH_SCREEN_AUDIO_VIDEOCAPTURE)",
            "GDI mode, screen, Mic, software h264 encoder (SAFE_SCREEN_MIC)",
            "GDI mode, screen, PC Sounds, software h264 encoder (SAFE_SCREEN_PCSOUNDS)",
            "Duplication API mode, screen, Mic, hardware h264 encoder (HIGH_SCREEN_MIC)",
            "Duplication API mode, screen, PC Sounds, hardware h264 encoder (HIGH_SCREEN_PCSOU" +
                "NDS)",
            "GDI mode, screen, Mic, WebCam, software h264 encoder (SAFE_SCREEN_MIC_VIDEOCAPTUR" +
                "E)",
            "Duplication API mode, screen, Mic, WebCam, hardware h264 encoder (HIGH_SCREEN_MIC" +
                "_VIDEOCAPTURE)"});
            this.cbProfiles.Location = new System.Drawing.Point(98, 18);
            this.cbProfiles.MaxDropDownItems = 11;
            this.cbProfiles.Name = "cbProfiles";
            this.cbProfiles.Size = new System.Drawing.Size(586, 21);
            this.cbProfiles.TabIndex = 1;
            this.cbProfiles.SelectedIndexChanged += new System.EventHandler(this.cbProfiles_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 317);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Output File:";
            // 
            // tbOutputFile
            // 
            this.tbOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutputFile.Location = new System.Drawing.Point(98, 314);
            this.tbOutputFile.Name = "tbOutputFile";
            this.tbOutputFile.Size = new System.Drawing.Size(560, 20);
            this.tbOutputFile.TabIndex = 29;
            this.tbOutputFile.TextChanged += new System.EventHandler(this.tbOutputFile_TextChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(659, 312);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(25, 22);
            this.btnBrowse.TabIndex = 30;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 399);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(692, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ssLabel1
            // 
            this.ssLabel1.Name = "ssLabel1";
            this.ssLabel1.Size = new System.Drawing.Size(677, 17);
            this.ssLabel1.Spring = true;
            this.ssLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlAll
            // 
            this.pnlAll.Controls.Add(this.pnlParameters);
            this.pnlAll.Controls.Add(this.btnStartStop);
            this.pnlAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAll.Location = new System.Drawing.Point(0, 0);
            this.pnlAll.Name = "pnlAll";
            this.pnlAll.Size = new System.Drawing.Size(692, 399);
            this.pnlAll.TabIndex = 0;
            // 
            // pnlParameters
            // 
            this.pnlParameters.Controls.Add(this.cbVideoDeviceStreamFormats);
            this.pnlParameters.Controls.Add(this.lblVideoDeviceStreamFormats);
            this.pnlParameters.Controls.Add(this.cbVideoDeviceStreams);
            this.pnlParameters.Controls.Add(this.lblVideoDeviceStreams);
            this.pnlParameters.Controls.Add(this.chkMouseCursor);
            this.pnlParameters.Controls.Add(this.label11);
            this.pnlParameters.Controls.Add(this.numRegionTop);
            this.pnlParameters.Controls.Add(this.numRegionLeft);
            this.pnlParameters.Controls.Add(this.panel1);
            this.pnlParameters.Controls.Add(this.label10);
            this.pnlParameters.Controls.Add(this.numRegionHeight);
            this.pnlParameters.Controls.Add(this.numRegionWidth);
            this.pnlParameters.Controls.Add(this.label9);
            this.pnlParameters.Controls.Add(this.numOutputHeight);
            this.pnlParameters.Controls.Add(this.numOutputWidth);
            this.pnlParameters.Controls.Add(this.label8);
            this.pnlParameters.Controls.Add(this.pnlCaptureMode);
            this.pnlParameters.Controls.Add(this.label7);
            this.pnlParameters.Controls.Add(this.cbAACEncoders);
            this.pnlParameters.Controls.Add(this.label5);
            this.pnlParameters.Controls.Add(this.cbAudioSources);
            this.pnlParameters.Controls.Add(this.label6);
            this.pnlParameters.Controls.Add(this.cbVideoEncoders);
            this.pnlParameters.Controls.Add(this.label4);
            this.pnlParameters.Controls.Add(this.cbVideoCaptureDevices);
            this.pnlParameters.Controls.Add(this.label1);
            this.pnlParameters.Controls.Add(this.cbProfiles);
            this.pnlParameters.Controls.Add(this.tbOutputFile);
            this.pnlParameters.Controls.Add(this.label3);
            this.pnlParameters.Controls.Add(this.label2);
            this.pnlParameters.Controls.Add(this.btnBrowse);
            this.pnlParameters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlParameters.Location = new System.Drawing.Point(0, 0);
            this.pnlParameters.Name = "pnlParameters";
            this.pnlParameters.Size = new System.Drawing.Size(692, 347);
            this.pnlParameters.TabIndex = 0;
            // 
            // cbVideoDeviceStreamFormats
            // 
            this.cbVideoDeviceStreamFormats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbVideoDeviceStreamFormats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVideoDeviceStreamFormats.Location = new System.Drawing.Point(212, 153);
            this.cbVideoDeviceStreamFormats.Name = "cbVideoDeviceStreamFormats";
            this.cbVideoDeviceStreamFormats.Size = new System.Drawing.Size(472, 21);
            this.cbVideoDeviceStreamFormats.TabIndex = 19;
            this.cbVideoDeviceStreamFormats.SelectedIndexChanged += new System.EventHandler(this.cbVideoDeviceStreamFormats_SelectedIndexChanged);
            // 
            // lblVideoDeviceStreamFormats
            // 
            this.lblVideoDeviceStreamFormats.AutoSize = true;
            this.lblVideoDeviceStreamFormats.Location = new System.Drawing.Point(164, 156);
            this.lblVideoDeviceStreamFormats.Name = "lblVideoDeviceStreamFormats";
            this.lblVideoDeviceStreamFormats.Size = new System.Drawing.Size(42, 13);
            this.lblVideoDeviceStreamFormats.TabIndex = 18;
            this.lblVideoDeviceStreamFormats.Text = "Format:";
            // 
            // cbVideoDeviceStreams
            // 
            this.cbVideoDeviceStreams.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVideoDeviceStreams.Location = new System.Drawing.Point(98, 153);
            this.cbVideoDeviceStreams.Name = "cbVideoDeviceStreams";
            this.cbVideoDeviceStreams.Size = new System.Drawing.Size(56, 21);
            this.cbVideoDeviceStreams.TabIndex = 17;
            this.cbVideoDeviceStreams.SelectedIndexChanged += new System.EventHandler(this.cbVideoDeviceStreams_SelectedIndexChanged);
            // 
            // lblVideoDeviceStreams
            // 
            this.lblVideoDeviceStreams.AutoSize = true;
            this.lblVideoDeviceStreams.Location = new System.Drawing.Point(41, 156);
            this.lblVideoDeviceStreams.Name = "lblVideoDeviceStreams";
            this.lblVideoDeviceStreams.Size = new System.Drawing.Size(43, 13);
            this.lblVideoDeviceStreams.TabIndex = 16;
            this.lblVideoDeviceStreams.Text = "Stream:";
            // 
            // chkMouseCursor
            // 
            this.chkMouseCursor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkMouseCursor.AutoSize = true;
            this.chkMouseCursor.Location = new System.Drawing.Point(98, 286);
            this.chkMouseCursor.Name = "chkMouseCursor";
            this.chkMouseCursor.Size = new System.Drawing.Size(15, 14);
            this.chkMouseCursor.TabIndex = 27;
            this.chkMouseCursor.UseVisualStyleBackColor = true;
            this.chkMouseCursor.CheckedChanged += new System.EventHandler(this.chkMouseCursor_CheckedChanged);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 285);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 13);
            this.label11.TabIndex = 26;
            this.label11.Text = "Mouse Cursor:";
            // 
            // numRegionTop
            // 
            this.numRegionTop.Location = new System.Drawing.Point(483, 90);
            this.numRegionTop.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numRegionTop.Name = "numRegionTop";
            this.numRegionTop.Size = new System.Drawing.Size(60, 20);
            this.numRegionTop.TabIndex = 11;
            this.numRegionTop.ValueChanged += new System.EventHandler(this.numRegion_ValueChanged);
            // 
            // numRegionLeft
            // 
            this.numRegionLeft.Location = new System.Drawing.Point(414, 90);
            this.numRegionLeft.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numRegionLeft.Name = "numRegionLeft";
            this.numRegionLeft.Size = new System.Drawing.Size(60, 20);
            this.numRegionLeft.TabIndex = 10;
            this.numRegionLeft.ValueChanged += new System.EventHandler(this.numRegion_ValueChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbD3D11);
            this.panel1.Controls.Add(this.rbD3D9);
            this.panel1.Location = new System.Drawing.Point(383, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(128, 21);
            this.panel1.TabIndex = 5;
            // 
            // rbD3D11
            // 
            this.rbD3D11.AutoSize = true;
            this.rbD3D11.Location = new System.Drawing.Point(61, 3);
            this.rbD3D11.Name = "rbD3D11";
            this.rbD3D11.Size = new System.Drawing.Size(59, 17);
            this.rbD3D11.TabIndex = 1;
            this.rbD3D11.TabStop = true;
            this.rbD3D11.Text = "D3D11";
            this.rbD3D11.UseVisualStyleBackColor = true;
            this.rbD3D11.CheckedChanged += new System.EventHandler(this.rbD3D_CheckedChanged);
            // 
            // rbD3D9
            // 
            this.rbD3D9.AutoSize = true;
            this.rbD3D9.Location = new System.Drawing.Point(5, 3);
            this.rbD3D9.Name = "rbD3D9";
            this.rbD3D9.Size = new System.Drawing.Size(53, 17);
            this.rbD3D9.TabIndex = 0;
            this.rbD3D9.TabStop = true;
            this.rbD3D9.Text = "D3D9";
            this.rbD3D9.UseVisualStyleBackColor = true;
            this.rbD3D9.CheckedChanged += new System.EventHandler(this.rbD3D_CheckedChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(289, 62);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Graphics Engine:";
            // 
            // numRegionHeight
            // 
            this.numRegionHeight.Location = new System.Drawing.Point(623, 90);
            this.numRegionHeight.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numRegionHeight.Name = "numRegionHeight";
            this.numRegionHeight.Size = new System.Drawing.Size(60, 20);
            this.numRegionHeight.TabIndex = 13;
            // 
            // numRegionWidth
            // 
            this.numRegionWidth.Location = new System.Drawing.Point(554, 90);
            this.numRegionWidth.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numRegionWidth.Name = "numRegionWidth";
            this.numRegionWidth.Size = new System.Drawing.Size(60, 20);
            this.numRegionWidth.TabIndex = 12;
            this.numRegionWidth.ValueChanged += new System.EventHandler(this.numRegion_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(240, 92);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(169, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "Screen Region [ x y width height ]:";
            // 
            // numOutputHeight
            // 
            this.numOutputHeight.Location = new System.Drawing.Point(167, 90);
            this.numOutputHeight.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numOutputHeight.Name = "numOutputHeight";
            this.numOutputHeight.Size = new System.Drawing.Size(60, 20);
            this.numOutputHeight.TabIndex = 8;
            // 
            // numOutputWidth
            // 
            this.numOutputWidth.Location = new System.Drawing.Point(98, 90);
            this.numOutputWidth.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numOutputWidth.Name = "numOutputWidth";
            this.numOutputWidth.Size = new System.Drawing.Size(60, 20);
            this.numOutputWidth.TabIndex = 7;
            this.numOutputWidth.ValueChanged += new System.EventHandler(this.numOutput_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 92);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Output Size:";
            // 
            // pnlCaptureMode
            // 
            this.pnlCaptureMode.Controls.Add(this.rbModeDuplicationAPI);
            this.pnlCaptureMode.Controls.Add(this.rbModeGDI);
            this.pnlCaptureMode.Location = new System.Drawing.Point(98, 57);
            this.pnlCaptureMode.Name = "pnlCaptureMode";
            this.pnlCaptureMode.Size = new System.Drawing.Size(171, 21);
            this.pnlCaptureMode.TabIndex = 3;
            // 
            // rbModeDuplicationAPI
            // 
            this.rbModeDuplicationAPI.AutoSize = true;
            this.rbModeDuplicationAPI.Location = new System.Drawing.Point(55, 3);
            this.rbModeDuplicationAPI.Name = "rbModeDuplicationAPI";
            this.rbModeDuplicationAPI.Size = new System.Drawing.Size(98, 17);
            this.rbModeDuplicationAPI.TabIndex = 1;
            this.rbModeDuplicationAPI.TabStop = true;
            this.rbModeDuplicationAPI.Text = "Duplication API";
            this.rbModeDuplicationAPI.UseVisualStyleBackColor = true;
            this.rbModeDuplicationAPI.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);
            // 
            // rbModeGDI
            // 
            this.rbModeGDI.AutoSize = true;
            this.rbModeGDI.Location = new System.Drawing.Point(5, 3);
            this.rbModeGDI.Name = "rbModeGDI";
            this.rbModeGDI.Size = new System.Drawing.Size(44, 17);
            this.rbModeGDI.TabIndex = 0;
            this.rbModeGDI.TabStop = true;
            this.rbModeGDI.Text = "GDI";
            this.rbModeGDI.UseVisualStyleBackColor = true;
            this.rbModeGDI.CheckedChanged += new System.EventHandler(this.rbMode_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Capture Mode:";
            // 
            // cbAACEncoders
            // 
            this.cbAACEncoders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAACEncoders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAACEncoders.Location = new System.Drawing.Point(98, 245);
            this.cbAACEncoders.Name = "cbAACEncoders";
            this.cbAACEncoders.Size = new System.Drawing.Size(586, 21);
            this.cbAACEncoders.TabIndex = 25;
            this.cbAACEncoders.SelectedIndexChanged += new System.EventHandler(this.cbAACEncoders_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 248);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "AAC Encoders:";
            // 
            // cbAudioSources
            // 
            this.cbAudioSources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAudioSources.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAudioSources.Location = new System.Drawing.Point(98, 218);
            this.cbAudioSources.Name = "cbAudioSources";
            this.cbAudioSources.Size = new System.Drawing.Size(586, 21);
            this.cbAudioSources.TabIndex = 23;
            this.cbAudioSources.SelectedIndexChanged += new System.EventHandler(this.cbAudioSources_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 221);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Audio Sources:";
            // 
            // cbVideoEncoders
            // 
            this.cbVideoEncoders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbVideoEncoders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVideoEncoders.Location = new System.Drawing.Point(98, 180);
            this.cbVideoEncoders.Name = "cbVideoEncoders";
            this.cbVideoEncoders.Size = new System.Drawing.Size(586, 21);
            this.cbVideoEncoders.TabIndex = 21;
            this.cbVideoEncoders.SelectedIndexChanged += new System.EventHandler(this.cbVideoEncoders_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 183);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "Video Encoders:";
            // 
            // cbVideoCaptureDevices
            // 
            this.cbVideoCaptureDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbVideoCaptureDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbVideoCaptureDevices.Location = new System.Drawing.Point(98, 126);
            this.cbVideoCaptureDevices.Name = "cbVideoCaptureDevices";
            this.cbVideoCaptureDevices.Size = new System.Drawing.Size(586, 21);
            this.cbVideoCaptureDevices.TabIndex = 15;
            this.cbVideoCaptureDevices.SelectedIndexChanged += new System.EventHandler(this.cbVideoCaptureDevices_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Video Sources:";
            // 
            // btnStartStop
            // 
            this.btnStartStop.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnStartStop.Location = new System.Drawing.Point(267, 361);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(152, 23);
            this.btnStartStop.TabIndex = 1;
            this.btnStartStop.Text = "Start Recording";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "MP4 Files (*.mp4)|*.mp4|All Files (*.*)|*.*";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 421);
            this.Controls.Add(this.pnlAll);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BB FlashBack SDK - C# Extended Recorder Sample";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlAll.ResumeLayout(false);
            this.pnlParameters.ResumeLayout(false);
            this.pnlParameters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRegionTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRegionLeft)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRegionHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRegionWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOutputHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOutputWidth)).EndInit();
            this.pnlCaptureMode.ResumeLayout(false);
            this.pnlCaptureMode.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cbProfiles;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbOutputFile;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ssLabel1;
        private System.Windows.Forms.Panel pnlAll;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Panel pnlParameters;
        private System.Windows.Forms.ComboBox cbVideoEncoders;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbVideoCaptureDevices;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbAACEncoders;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbAudioSources;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pnlCaptureMode;
        private System.Windows.Forms.RadioButton rbModeDuplicationAPI;
        private System.Windows.Forms.RadioButton rbModeGDI;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numOutputHeight;
        private System.Windows.Forms.NumericUpDown numOutputWidth;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numRegionHeight;
        private System.Windows.Forms.NumericUpDown numRegionWidth;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbD3D11;
        private System.Windows.Forms.RadioButton rbD3D9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numRegionTop;
        private System.Windows.Forms.NumericUpDown numRegionLeft;
        private System.Windows.Forms.CheckBox chkMouseCursor;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbVideoDeviceStreamFormats;
        private System.Windows.Forms.Label lblVideoDeviceStreamFormats;
        private System.Windows.Forms.ComboBox cbVideoDeviceStreams;
        private System.Windows.Forms.Label lblVideoDeviceStreams;
    }
}

