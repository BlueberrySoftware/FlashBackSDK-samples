using System.Collections.Generic;

namespace RecorderExtended
{
    public class RecorderProfile
    {
        public AudioSettings AudioSettings { get; set; }
        public List<string> AvailableProfiles { get; set; }
        public List<AvailableVideoCaptureDevice> AvailableVideoCaptureDevices { get; set; }
        public object AvailableVideoEncoder { get; set; }
        public List<AvailableVideoEncoder> AvailableVideoEncoders { get; set; }
        public int CaptureMode { get; set; }
        public string CaptureModeDescr { get; set; }
        public string GraphicsEngine { get; set; }
        public OutputSize OutputSize { get; set; }
        public bool RecordAudioInputsToTracks { get; set; }
        public bool RecordMonitorsToTracks { get; set; }
        public bool RecordMouseCursor { get; set; }
        public bool RecordMouseCursorToTrack { get; set; }
        public bool RecordWebcamToTrack { get; set; }
        public Region Region { get; set; }
        public List<SelectedVideoCaptureDevice> SelectedVideoCaptureDevices { get; set; }
        public SelectedVideoEncoder SelectedVideoEncoder { get; set; }
        public VideoEncoderParameters VideoEncoderParameters { get; set; }
    }

    public class AudioSettings
    {
        public AudioEncoderParameters AudioEncoderParameters { get; set; }
        public List<AvailableAACEncoder> AvailableAACEncoders { get; set; }
        public List<AvailableAudioSource> AvailableAudioSources { get; set; }
        public List<SelectedAudioSource> SelectedAudioSources { get; set; }
    }

    public class AudioEncoderParameters
    {
        public EncoderInfo EncoderInfo { get; set; }
    }

    public class EncoderInfo
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public string TypeDescr { get; set; }
    }

    public class AvailableAACEncoder
    {
        public string Name { get; set; }
        public int Type { get; set; }
    }

    public class AvailableAudioSource
    {
        public bool Default { get; set; }
        public string DeviceId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string TypeDescr { get; set; }
    }

    public class SelectedAudioSource
    {
        public string DeviceId { get; set; }
    }

    public class OutputSize
    {
        public int Height { get; set; }
        public int Width { get; set; }
    }

    public class Region
    {
        public int Height { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
    }

    public class SelectedVideoEncoder
    {
        public bool? Hardware { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string TypeDescr { get; set; }
    }

    public class VideoEncoderParameters
    {
        public int BFramesNum { get; set; }
        public int CompressionMode { get; set; }
        public string CompressionModeDescr { get; set; }
        public bool EnableCabac { get; set; }
        public FPS FPS { get; set; }
        public int GOPSize { get; set; }
        public int H264Profile { get; set; }
        public string H264ProfileDescr { get; set; }
        public bool LowLatency { get; set; }
        public int QualityLevel { get; set; }
        public int RefFramesNum { get; set; }
    }

    public class FPS
    {
        public int Den { get; set; }
        public int Num { get; set; }
    }

    public class AvailableVideoCaptureDevice
    {
        public string FriendlyName { get; set; }
        public List<Stream> Streams { get; set; }
        public string SymbolicName { get; set; }
    }

    public class Stream
    {
        public List<Format> Formats { get; set; }
        public string MajorType { get; set; }
        public int UniqueStreamId { get; set; }
    }

    public class Format
    {
        public string FormatTypeName { get; set; }
        public int Height { get; set; }
        public string MajorTypeName { get; set; }
        public string SubTypeName { get; set; }
        public int Width { get; set; }
    }

    public class AvailableVideoEncoder
    {
        public bool Hardware { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
    }

    public class SelectedVideoCaptureDevice
    {
        public int DeviceFormatIndex { get; set; }
        public int DeviceIndex { get; set; }
        public int DeviceStreamIndex { get; set; }
        public int DrawAtX { get; set; }
        public int DrawAtY { get; set; }
        public string FiltersDescr { get; set; }
    }
}
