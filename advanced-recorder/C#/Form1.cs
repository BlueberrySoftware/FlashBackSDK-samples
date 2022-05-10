using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using FBRecorder;
using Newtonsoft.Json;

namespace RecorderExtended
{
    public partial class Form1 : Form
    {
        #region Variables

        TFBRecorder _Recorder;
        bool _IsRecording;

        RecorderProfile _Profile;
        RecorderProfile _ModifiedProfile;

        bool _IsProfileUpdating;
        bool _IsProfileModified;

        string _LogsPath;
        bool _IsLogJson = true;

        #endregion

        #region Form events

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tbOutputFile.Text = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "fbrecordersample.mp4");

            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            CreateRecorder();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_IsRecording)
                e.Cancel = true;
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

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            ToggleRecording();
        }

        #endregion

        #region Initialization

        private void CreateRecorder()
        {
            try
            {
                _LogsPath = Path.Combine(Path.GetTempPath(), "FBRecorderSample");

                if (!Directory.Exists(_LogsPath))
                    Directory.CreateDirectory(_LogsPath);

                _Recorder = new TFBRecorder("", _LogsPath, 10, TFBRecorder.TLogKind.Verbose);

                if (_IsLogJson)
                {
                    string def_json = _Recorder.GetDefaultConfigJSON();
                    File.WriteAllText(Path.Combine(_LogsPath, "default.json"), def_json);
                }

                cbProfiles.SelectedIndex = 0; // activate first profile
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
                pnlAll.Enabled = false;
            }
        }

        #endregion

        #region Recording

        private void ToggleRecording()
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

                    if (_ModifiedProfile == null)
                        ActivateProfile((TFBRecorder.TFBRecorderProfiles)cbProfiles.SelectedIndex);

                    string profile_json = JsonConvert.SerializeObject(_ModifiedProfile);

                    if (_IsLogJson)
                    {
                        File.WriteAllText(Path.Combine(_LogsPath, "current.json"),
                           JsonConvert.SerializeObject(_ModifiedProfile, Formatting.Indented,
                           new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
                    }

                    if (!_Recorder.SetConfigJSON(profile_json))
                    {
                        ShowError($"Error set modified profile: {GetRecorderErrors()}");
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

        #endregion

        #region Activate profile

        private void ActivateProfile(TFBRecorder.TFBRecorderProfiles profiles)
        {
            try
            {
                _Recorder.ActivateProfile(profiles);

                string profile_json = _Recorder.GetConfigJSONForProfile(profiles);

                if (!string.IsNullOrEmpty(profile_json))
                {
                    _Profile = JsonConvert.DeserializeObject<RecorderProfile>(profile_json);
                    _ModifiedProfile = JsonConvert.DeserializeObject<RecorderProfile>(profile_json);
                    _IsProfileModified = false;

                    // For demo purpose record all sources to single track
                    _ModifiedProfile.RecordAudioInputsToTracks = false;
                    _ModifiedProfile.RecordMonitorsToTracks = false;
                    _ModifiedProfile.RecordMouseCursorToTrack = false;
                    _ModifiedProfile.RecordWebcamToTrack = false;

                    ShowProfile();

                    if (_IsLogJson)
                    {
                        File.WriteAllText(Path.Combine(_LogsPath, $"{profiles.ToString()}.json"), profile_json);
                        File.WriteAllText(Path.Combine(_LogsPath, $"{profiles.ToString()}_fmt.json"),
                            JsonConvert.SerializeObject(_Profile, Formatting.Indented,
                            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        #endregion

        #region Show profile details

        private void ShowProfile()
        {
            if (_Profile == null)
                return;

            try
            {
                _IsProfileUpdating = true;

                rbModeGDI.Checked = _Profile.CaptureMode == 1;
                rbModeDuplicationAPI.Checked = _Profile.CaptureMode == 2;

                rbD3D9.Checked = _Profile.GraphicsEngine == "D3D9";
                rbD3D11.Checked = _Profile.GraphicsEngine == "D3D11";

                numOutputWidth.Value = _Profile.OutputSize.Width;
                numOutputHeight.Value = _Profile.OutputSize.Height;

                numRegionLeft.Value = _Profile.Region.Left;
                numRegionTop.Value = _Profile.Region.Top;
                numRegionWidth.Value = _Profile.Region.Width;
                numRegionHeight.Value = _Profile.Region.Height;

                chkMouseCursor.Checked = _Profile.RecordMouseCursor;

                cbVideoCaptureDevices.Items.Clear();
                cbVideoCaptureDevices.Items.Add("");
                cbVideoCaptureDevices.SelectedIndex = 0;

                if (_Profile.AvailableVideoCaptureDevices != null)
                {
                    cbVideoCaptureDevices.Items.AddRange(_Profile.AvailableVideoCaptureDevices.Select(d => d.FriendlyName).ToArray());

                    if (_Profile.SelectedVideoCaptureDevices?.Any() ?? false)
                        cbVideoCaptureDevices.SelectedIndex = _Profile.SelectedVideoCaptureDevices[0].DeviceIndex + 1;
                }

                ShowStreams();

                cbVideoEncoders.Items.Clear();
                cbVideoEncoders.Items.Add("");
                cbVideoEncoders.SelectedIndex = 0;

                if (_Profile.AvailableVideoEncoders != null)
                {
                    cbVideoEncoders.Items.AddRange(_Profile.AvailableVideoEncoders.Select(e => e.Name).ToArray());

                    if (_Profile.SelectedVideoEncoder != null)
                        cbVideoEncoders.SelectedIndex = _Profile.AvailableVideoEncoders.FindIndex(e => e.Name == _Profile.SelectedVideoEncoder.Name) + 1;
                }

                cbAudioSources.Items.Clear();
                cbAudioSources.Items.Add("");
                cbAudioSources.SelectedIndex = 0;

                if (_Profile.AudioSettings.AvailableAudioSources != null)
                {
                    cbAudioSources.Items.AddRange(_Profile.AudioSettings.AvailableAudioSources.Select(e => e.Name).ToArray());

                    if (_Profile.AudioSettings.SelectedAudioSources?.Any() ?? false)
                        cbAudioSources.SelectedIndex = _Profile.AudioSettings.AvailableAudioSources.FindIndex(s => s.DeviceId == _Profile.AudioSettings.SelectedAudioSources[0].DeviceId) + 1;
                }

                cbAACEncoders.Items.Clear();
                cbAACEncoders.Items.Add("");
                cbAACEncoders.SelectedIndex = 0;

                if (_Profile.AudioSettings.AvailableAACEncoders != null)
                {
                    cbAACEncoders.Items.AddRange(_Profile.AudioSettings.AvailableAACEncoders.Select(e => e.Name).ToArray());

                    if (_Profile.AudioSettings.AudioEncoderParameters?.EncoderInfo != null)
                        cbAACEncoders.SelectedIndex = _Profile.AudioSettings.AvailableAACEncoders.FindIndex(e => e.Name == _Profile.AudioSettings.AudioEncoderParameters.EncoderInfo.Name) + 1;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                _IsProfileUpdating = false;
            }
        }

        private void ShowStreams()
        {
            if (_Profile == null)
                return;

            bool isProfileUpdating = _IsProfileUpdating;
            try
            {
                _IsProfileUpdating = true;

                cbVideoDeviceStreams.Items.Clear();
                cbVideoDeviceStreams.Items.Add(" ");
                cbVideoDeviceStreams.SelectedIndex = 0;

                if (cbVideoCaptureDevices.SelectedIndex > 0)
                {
                    var device = _Profile.AvailableVideoCaptureDevices[cbVideoCaptureDevices.SelectedIndex - 1];

                    if (device.Streams?.Any() ?? false)
                    {
                        cbVideoDeviceStreams.Items.Remove(" ");
                        cbVideoDeviceStreams.Items.AddRange(Enumerable.Range(0, device.Streams.Count).Select(n => n.ToString()).ToArray());

                        if (_ModifiedProfile.SelectedVideoCaptureDevices?.Any() ?? false)
                            cbVideoDeviceStreams.SelectedIndex = _ModifiedProfile.SelectedVideoCaptureDevices[0].DeviceStreamIndex;
                    }

                    cbVideoDeviceStreams.Enabled = true;
                }
                else
                {
                    cbVideoDeviceStreams.Enabled = false;
                }

                ShowStreamFormats();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                _IsProfileUpdating = isProfileUpdating;
            }
        }

        private void ShowStreamFormats()
        {
            if (_Profile == null)
                return;

            bool isProfileUpdating = _IsProfileUpdating;
            try
            {
                _IsProfileUpdating = true;

                cbVideoDeviceStreamFormats.Items.Clear();
                cbVideoDeviceStreamFormats.Items.Add(" ");
                cbVideoDeviceStreamFormats.SelectedIndex = 0;

                int stream_index;
                if (int.TryParse((string)cbVideoDeviceStreams.SelectedItem, out stream_index))
                {
                    var device = _Profile.AvailableVideoCaptureDevices[cbVideoCaptureDevices.SelectedIndex - 1];
                    var stream = device.Streams[stream_index];

                    if (stream.Formats?.Any() ?? false)
                    {
                        cbVideoDeviceStreamFormats.Items.Remove(" ");
                        cbVideoDeviceStreamFormats.Items.AddRange(stream.Formats.Select(f => $"{f.SubTypeName} [{f.Width}x{f.Height}]").ToArray());

                        if (_ModifiedProfile.SelectedVideoCaptureDevices?.Any() ?? false)
                            cbVideoDeviceStreamFormats.SelectedIndex = _ModifiedProfile.SelectedVideoCaptureDevices[0].DeviceFormatIndex;
                    }
                 
                    cbVideoDeviceStreamFormats.Enabled = true;
                }
                else
                {
                    cbVideoDeviceStreamFormats.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
            finally
            {
                _IsProfileUpdating = isProfileUpdating;
            }
        }

        #endregion

        #region Update profile

        private void UpdateProfile(Action action)
        {
            if (_Profile == null || _IsProfileUpdating)
                return;

            try
            {
                action();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }

            if (!_IsProfileModified)
            {
                _IsProfileModified = true;

                cbProfiles.Items.Add("(modified)");
                cbProfiles.SelectedIndex = cbProfiles.Items.Count - 1;
            }
        }

        private void rbMode_CheckedChanged(object sender, EventArgs e)
        {
            UpdateProfile(() =>
            {
                _ModifiedProfile.CaptureMode = rbModeGDI.Checked ? 1 : 2;
            });
        }

        private void rbD3D_CheckedChanged(object sender, EventArgs e)
        {
            UpdateProfile(() =>
            {
                _ModifiedProfile.GraphicsEngine = rbD3D9.Checked ? "D3D9" : "D3D11";
            });
        }

        private void numOutput_ValueChanged(object sender, EventArgs e)
        {
            UpdateProfile(() =>
            {
                _ModifiedProfile.OutputSize = new OutputSize()
                {
                    Width = (int)numOutputWidth.Value,
                    Height = (int)numOutputHeight.Value,
                };
            });
        }

        private void numRegion_ValueChanged(object sender, EventArgs e)
        {
            UpdateProfile(() =>
            {
                _ModifiedProfile.Region = new Region()
                {
                    Left = (int)numRegionLeft.Value,
                    Top = (int)numRegionTop.Value,
                    Width = (int)numRegionWidth.Value,
                    Height = (int)numRegionHeight.Value
                };
            });
        }

        private void cbProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_Recorder != null && _Recorder.CanUse())
            {
                if (cbProfiles.SelectedIndex >= 0 && cbProfiles.SelectedIndex < (int)TFBRecorder.TFBRecorderProfiles.TOTAL_NUMBER)
                {
                    cbProfiles.Items.Remove("(modified)");
                    ActivateProfile((TFBRecorder.TFBRecorderProfiles)cbProfiles.SelectedIndex);
                }
            }
        }

        private void cbVideoCaptureDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProfile(() =>
            {
                _ModifiedProfile.SelectedVideoCaptureDevices = new List<SelectedVideoCaptureDevice>();

                if (cbVideoCaptureDevices.SelectedIndex > 0)
                {
                    var device = _Profile.AvailableVideoCaptureDevices[cbVideoCaptureDevices.SelectedIndex - 1];

                    _ModifiedProfile.SelectedVideoCaptureDevices.Add(new SelectedVideoCaptureDevice()
                    {
                        DeviceIndex = _Profile.AvailableVideoCaptureDevices.IndexOf(device),
                        DeviceFormatIndex = 0,
                        DeviceStreamIndex = 0,
                        DrawAtX = 0,
                        DrawAtY = 0,
                    });
                }

                ShowStreams();
            });
        }

        private void cbVideoDeviceStreams_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProfile(() =>
            {
                int stream_index;
                if (int.TryParse((string)cbVideoDeviceStreams.SelectedItem, out stream_index))
                {
                    _ModifiedProfile.SelectedVideoCaptureDevices[0].DeviceStreamIndex = stream_index;
                    _ModifiedProfile.SelectedVideoCaptureDevices[0].DeviceFormatIndex = 0;
                }

                ShowStreamFormats();
            });
        }

        private void cbVideoDeviceStreamFormats_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProfile(() =>
            {
                if (!string.IsNullOrWhiteSpace((string)cbVideoDeviceStreams.SelectedItem))
                {
                    _ModifiedProfile.SelectedVideoCaptureDevices[0].DeviceFormatIndex = cbVideoDeviceStreamFormats.SelectedIndex;
                }
            });
        }

        private void cbVideoEncoders_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProfile(() =>
            {
                _ModifiedProfile.SelectedVideoEncoder = null;

                if (cbVideoEncoders.SelectedIndex > 0)
                {
                    var encoder = _Profile.AvailableVideoEncoders[cbVideoEncoders.SelectedIndex - 1];

                    _ModifiedProfile.SelectedVideoEncoder = new SelectedVideoEncoder()
                    {
                        Hardware = encoder.Hardware,
                        Name = encoder.Name,
                        Type = encoder.Type,
                    };
                }
            });
        }

        private void cbAudioSources_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProfile(() =>
            {
                _ModifiedProfile.AudioSettings.SelectedAudioSources = new List<SelectedAudioSource>();

                if (cbAudioSources.SelectedIndex > 0)
                {
                    var source = _Profile.AudioSettings.AvailableAudioSources[cbAudioSources.SelectedIndex - 1];

                    _ModifiedProfile.AudioSettings.SelectedAudioSources.Add(new SelectedAudioSource()
                    {
                        DeviceId = source.DeviceId
                    });
                }
            });
        }

        private void cbAACEncoders_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProfile(() =>
            {
                _ModifiedProfile.AudioSettings.AudioEncoderParameters = new AudioEncoderParameters();

                if (cbAACEncoders.SelectedIndex > 0)
                {
                    var encoder = _Profile.AudioSettings.AvailableAACEncoders[cbAACEncoders.SelectedIndex - 1];

                    _ModifiedProfile.AudioSettings.AudioEncoderParameters.EncoderInfo = new EncoderInfo()
                    {
                        Name = encoder.Name,
                        Type = encoder.Type,
                    };
                }
            });
        }

        private void chkMouseCursor_CheckedChanged(object sender, EventArgs e)
        {
            UpdateProfile(() =>
            {
                _ModifiedProfile.RecordMouseCursor = chkMouseCursor.Checked;
            });
        }

        #endregion

        #region Errors handling

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

        #endregion
    }
}
