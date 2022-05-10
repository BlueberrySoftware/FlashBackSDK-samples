
namespace RecorderSimple
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
            this.cbProfile = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbOutputFile = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ssLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlAll = new System.Windows.Forms.Panel();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pnlParameters = new System.Windows.Forms.Panel();
            this.statusStrip1.SuspendLayout();
            this.pnlAll.SuspendLayout();
            this.pnlParameters.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Profile:";
            // 
            // cbProfile
            // 
            this.cbProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbProfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbProfile.FormattingEnabled = true;
            this.cbProfile.Items.AddRange(new object[] {
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
            this.cbProfile.Location = new System.Drawing.Point(67, 18);
            this.cbProfile.Name = "cbProfile";
            this.cbProfile.Size = new System.Drawing.Size(521, 21);
            this.cbProfile.TabIndex = 2;
            this.cbProfile.SelectedIndexChanged += new System.EventHandler(this.cbProfile_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Output File:";
            // 
            // tbOutputFile
            // 
            this.tbOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutputFile.Location = new System.Drawing.Point(67, 45);
            this.tbOutputFile.Name = "tbOutputFile";
            this.tbOutputFile.Size = new System.Drawing.Size(495, 20);
            this.tbOutputFile.TabIndex = 4;
            this.tbOutputFile.TextChanged += new System.EventHandler(this.tbOutputFile_TextChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(563, 43);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(25, 23);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 122);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(596, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ssLabel1
            // 
            this.ssLabel1.Name = "ssLabel1";
            this.ssLabel1.Size = new System.Drawing.Size(581, 17);
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
            this.pnlAll.Size = new System.Drawing.Size(596, 122);
            this.pnlAll.TabIndex = 0;
            // 
            // btnStartStop
            // 
            this.btnStartStop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartStop.Location = new System.Drawing.Point(227, 86);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(152, 23);
            this.btnStartStop.TabIndex = 0;
            this.btnStartStop.Text = "Start Recording";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "MP4 Files (*.mp4)|*.mp4|All Files (*.*)|*.*";
            // 
            // pnlParameters
            // 
            this.pnlParameters.Controls.Add(this.cbProfile);
            this.pnlParameters.Controls.Add(this.tbOutputFile);
            this.pnlParameters.Controls.Add(this.label3);
            this.pnlParameters.Controls.Add(this.label2);
            this.pnlParameters.Controls.Add(this.btnBrowse);
            this.pnlParameters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlParameters.Location = new System.Drawing.Point(0, 0);
            this.pnlParameters.Name = "pnlParameters";
            this.pnlParameters.Size = new System.Drawing.Size(596, 80);
            this.pnlParameters.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 144);
            this.Controls.Add(this.pnlAll);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BB FlashBack SDK - C# Recorder Sample";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.pnlAll.ResumeLayout(false);
            this.pnlParameters.ResumeLayout(false);
            this.pnlParameters.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cbProfile;
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
    }
}

