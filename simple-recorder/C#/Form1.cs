using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using FBRecorder;

namespace RecorderSimple
{
    public partial class Form1 : Form
    {
        TFBRecorder _Recorder;
        bool _IsRecording;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbProfile.SelectedIndex = 0;
            
            tbOutputFile.Text = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "fbrecordersample.mp4");

            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            try
            {
                string log_folder = Path.Combine(Path.GetTempPath(), "FBRecorderSample");

                if (!Directory.Exists(log_folder))
                    Directory.CreateDirectory(log_folder);

                _Recorder = new TFBRecorder("", log_folder, 10, TFBRecorder.TLogKind.Verbose);
                
                cbProfile_SelectedIndexChanged(null, null); // set default profile
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);            
                pnlAll.Enabled = false;
            }
        }

        private void tbOutputFile_TextChanged(object sender, EventArgs e)
        {
            btnStartStop.Enabled = Directory.Exists(Path.GetDirectoryName(tbOutputFile.Text));
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try { saveFileDialog1.InitialDirectory = Path.GetDirectoryName(tbOutputFile.Text); }
            catch { }

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbOutputFile.Text = saveFileDialog1.FileName;
                btnStartStop.Enabled = Directory.Exists(Path.GetDirectoryName(tbOutputFile.Text));
            }
        }

        private void cbProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_Recorder != null && _Recorder.CanUse())
            {
                try
                {
                    _Recorder.ActivateProfile((TFBRecorder.TFBRecorderProfiles)cbProfile.SelectedIndex);
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_IsRecording)
                {
                    string output_file = tbOutputFile.Text;

                    if (!Directory.Exists(Path.GetDirectoryName(output_file)))
                    {
                        MessageBox.Show($"Directory should exist: {Path.GetDirectoryName(output_file)}");
                        return;
                    }

                    if (!_Recorder.InitRecorder(output_file))
                    {
                        ShowError($"Error initialize recorder: {GetRecorderErrors()}");
                        return;
                    }

                    if (!_Recorder.StartRecording())
                    {
                        ShowError($"Error start recording: {GetRecorderErrors()}");
                        return;
                    }

                    _IsRecording = true;

                    ssLabel1.Text = "Recording...";
                    ssLabel1.ForeColor = SystemColors.ControlText;

                    btnStartStop.Text = "Stop Recording";
                    btnStartStop.ForeColor = Color.Red;

                    pnlParameters.Enabled = false;
                }
                else
                {
                    _Recorder.StopRecording();

                    _IsRecording = false;

                    ssLabel1.Text = "";
                    ssLabel1.ForeColor = SystemColors.ControlText;

                    btnStartStop.Text = "Start Recording";
                    btnStartStop.ForeColor = SystemColors.ControlText;

                    pnlParameters.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void ShowError(string msg)
        {
            MessageBox.Show(this, msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            ssLabel1.Text = msg;
            ssLabel1.ForeColor = Color.Red;
        }

        private string GetRecorderErrors()
        {
            int index = 0;
            StringBuilder sb = new StringBuilder();

            do
            {
                TFBRecorder.TFBGlobalErrors error = _Recorder.GetGlobalError(index++);

                if (error == 0)
                    break;

                sb.AppendLine(error.ToString());
            }
            while (true);

            _Recorder.ResetGlobalErrors();

            return sb.ToString();
        }
    }
}
