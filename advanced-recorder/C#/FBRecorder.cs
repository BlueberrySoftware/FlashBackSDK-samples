using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace FBRecorder
{
    /// <summary>
    /// A wrapper for the SDK Recorder object
    /// </summary>
    public class TFBRecorder
    {
        public enum TFBGlobalErrors : int
        {
            /// <summary>
            /// No more errors
            /// </summary>
            Unknown = 0,
            /// <summary>
            /// AudioDeviceIsInUseByAnotherApplication
            /// </summary>
            AudioDeviceIsInUseByAnotherApplication = 1
        };

        /// <summary>
        /// Log levels for internal SDK code
        /// </summary>
        public enum TLogKind : int
        {
            /// <summary>
            /// Minimum
            /// </summary>
            Info = 0,
            /// <summary>
            /// Medium
            /// </summary>
            Debug = 1,
            /// <summary>
            /// Maximum
            /// </summary>
            Verbose = 2,
            /// <summary>
            /// Disabled
            /// </summary>
            None = 3
        };

        /// <summary>
        /// Predefined profiles
        /// </summary>
        public enum TFBRecorderProfiles : int
        {
            /// <summary>
            /// GDI mode, screen only, software h264 encoder
            /// </summary>
            SAFE_SCREEN = 0,
            /// <summary>
            /// Duplication Output mode, screen only, hardware h264 encoder
            /// </summary>
            HIGH_SCREEN = 1,
            /// <summary>
            /// GDI mode, screen, PC Sounds, Mic, software h264 encoder
            /// </summary>
            SAFE_SCREEN_AUDIO = 2,
            /// <summary>
            /// Duplication Output mode, screen, PC Sounds, Mic, hardware h264 encoder
            /// </summary>
            HIGH_SCREEN_AUDIO = 3,
            /// <summary>
            /// GDI mode, screen, PC Sounds, Mic, WebCam, software h264 encoder
            /// </summary>
            SAFE_SCREEN_AUDIO_VIDEOCAPTURE = 4,
            /// <summary>
            /// Duplication Output mode, screen, PC Sounds, Mic, WebCam, hardware h264 encoder
            /// </summary>
            HIGH_SCREEN_AUDIO_VIDEOCAPTURE = 5,
            /// <summary>
            /// GDI mode, screen, Mic, software h264 encoder
            /// </summary>
            SAFE_SCREEN_MIC = 6,
            /// <summary>
            /// GDI mode, screen, PC Sounds, software h264 encoder
            /// </summary>
            SAFE_SCREEN_PCSOUNDS = 7,
            /// <summary>
            /// Duplication API mode, screen, Mic, hardware h264 encoder
            /// </summary>
            HIGH_SCREEN_MIC = 8,
            /// <summary>
            /// Duplication API mode, screen, PC Sounds, hardware h264 encoder
            /// </summary>
            HIGH_SCREEN_PCSOUNDS = 9,
            /// <summary>
            /// GDI mode, screen, Mic, WebCam, software h264 encoder
            /// </summary>
            SAFE_SCREEN_MIC_VIDEOCAPTURE = 10,
            /// <summary>
            /// Duplication API mode, screen, Mic, WebCam, hardware h264 encoder
            /// </summary>
            HIGH_SCREEN_MIC_VIDEOCAPTURE = 11,
            /// <summary>
            /// For internal use only
            /// </summary>
            TOTAL_NUMBER = 12
        };

        public enum TFBRecorderImageFormat
        {
            JPG = 0,
            BMP = 1,
            PNG = 2
        };

        /// <summary>
        /// Create a recorder object and returns a pointer to this object
        /// </summary>
        /// <returns>Pointer to recorder object</returns>
        [DllImport("FBRecorder.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr CreateRecorder(string sLicence);

        /// <summary>
        /// Destroys a recorder object
        /// </summary>
        /// <param name="pRecorderObject">Pointer to recorder object</param>
        [DllImport("FBRecorder.dll")]
        static extern void DestroyRecorder(IntPtr pRecorderObject);

        /// <summary>
        /// Initializes log using a custom folder, max files size and log level
        /// </summary>
        /// <param name="sLogFolderName">Path to folder</param>
        /// <param name="i32LogFolderSizeMb">Maximum file size in megabytes</param>
        /// <param name="logKind">Log level</param>
        [DllImport("FBRecorder.dll", CharSet = CharSet.Unicode)]
        public static extern void InitializeLog(string sLogFolderName, int i32LogFolderSizeMb, TLogKind logKind);

        /// <summary>
        /// Initializes the recorder object, creates all required internal structures
        /// </summary>
        /// <param name="pRecorderObject">Pointer to recorder object</param>
        /// <param name="sMP4Filename">Path to MP4 output file</param>
        /// <returns>True on success</returns>
        [DllImport("FBRecorder.dll", CharSet = CharSet.Unicode)]
        static extern bool InitRecorder(IntPtr pRecorderObject, string sMP4Filename);

        /// <summary>
        /// Starts the recording
        /// </summary>
        /// <param name="pRecorderObject">Pointer to recorder object</param>
        /// <returns>True on success</returns>
        [DllImport("FBRecorder.dll")]
        static extern bool StartRecording(IntPtr pRecorderObject);

        /// <summary>
        /// Stops the recording
        /// </summary>
        /// <param name="pRecorderObject">Pointer to recorder object</param>
        [DllImport("FBRecorder.dll")]
        static extern void StopRecording(IntPtr pRecorderObject);

        /// <summary>
        /// Retrieves a JSON text string with the default settings, UTF-8 format
        /// </summary>
        /// <param name="pRecorderObject">Pointer to recorder object</param>
        /// <param name="pProfileConfigJSON">Pointer to JSON text string with the default settings, UTF-8 format</param>
        /// <returns>True on success</returns>
        [DllImport("FBRecorder.dll")]
        static extern bool GetDefaultConfigJSON(IntPtr pRecorderObject, out IntPtr pProfileConfigJSON);

        /// <summary>
        /// Retrieves a JSON text string for predefined profie, UTF-8 format
        /// </summary>
        /// <param name="pRecorderObject">Pointer to recorder object</param>
        /// <param name="iProfileid">Predefined profile Id</param>
        /// <param name="pProfileConfigJSON">Pointer to JSON text string for predefined profie, UTF-8 format</param>
        /// <returns>True on success</returns>
        [DllImport("FBRecorder.dll")]
        static extern bool GetConfigJSONForProfile(IntPtr pRecorderObject, int iProfileid, out IntPtr pProfileConfigJSON);

        /// <summary>
        /// Applies a JSON text string with the recorder settings to the recorder object, UTF-8 format
        /// </summary>
        /// <param name="pRecorderObject">Pointer to recorder object</param>
        /// <param name="pProfileConfigJSON">JSON text string with the recorder settings, UTF-8 format</param>
        /// <returns>True on success</returns>
        [DllImport("FBRecorder.dll", CharSet = CharSet.Ansi)]
        static extern bool SetConfigJSON(IntPtr pRecorderObject, string pProfileConfigJSON);

        /// <summary>
        /// Checks the availability of the video encoder object. 
        /// SDK supports many encoders but only part of them can be available on a local computer
        /// </summary>
        /// <param name="pRecorderObject">Pointer to recorder object</param>
        /// <param name="i32VideoEncoderType"></param>
        /// <param name="pwsVideoEncoderName"></param>
        /// <returns>True on success</returns>
        [DllImport("FBRecorder.dll", CharSet = CharSet.Unicode)]
        static extern bool CheckVideoEncoderAvailability(IntPtr pRecorderObject, int i32VideoEncoderType, string pwsVideoEncoderName);

        /// <summary>
        /// Returns Error type. If the return value is 0 then no more errors exist in the list.
        /// </summary>
        /// <param name="pRecorderObject">Pointer to recorder object</param>
        /// <param name="i32Index">Errors list index</param>
        /// <returns>Error type</returns>
        [DllImport("FBRecorder.dll")]
        static extern int GetGlobalError(IntPtr pRecorderObject, int i32Index);

        /// <summary>
        /// Reset errors
        /// </summary>
        /// <param name="pRecorderObject">Pointer to recorder object</param>
        [DllImport("FBRecorder.dll")]
        static extern void ResetGlobalErrors(IntPtr pRecorderObject);

        [DllImport("FBRecorder.dll", CharSet = CharSet.Unicode)]
        static extern bool CaptureScreen(string wsImageFilename, int ImageFormat, int i32Compression_0_9, int i32Left, int i32Top, int i32Width, int i32Height);

        /// <summary>
        /// Pause the recording
        /// </summary>
        /// <param name="pRecorderObject">Pointer to recorder object</param>
        /// <returns>True on success</returns>
        [DllImport("FBRecorder.dll")]
        static extern bool Pause(IntPtr pRecorderObject, bool bPause);

        /// <summary>
        /// Is recoridng in progress
        /// </summary>
        /// <param name="pRecorderObject">Pointer to recorder object</param>
        /// <returns>True on success</returns>
        [DllImport("FBRecorder.dll")]
        static extern bool IsRecording(IntPtr pRecorderObject);

        /// <summary>
        /// Is paused
        /// </summary>
        /// <param name="pRecorderObject">Pointer to recorder object</param>
        /// <returns>True on success</returns>
        [DllImport("FBRecorder.dll")]
        static extern bool IsPaused(IntPtr pRecorderObject);

        /// <summary>
        /// MergeMP4Files
        /// </summary>
        /// <param name="sMP4FilesJSON">UTF8 json string should contain an array of file names</param>
        /// <returns>True on success</returns>
        [DllImport("FBRecorder.dll", CharSet = CharSet.Unicode)]
        public static extern bool MergeMP4Files([MarshalAs(UnmanagedType.LPArray)]  byte[] sMP4FilesJSON, string sResultFilename);

        [DllImport("FBRecorder.dll", CharSet = CharSet.Unicode)]
        public static extern bool CreateMP4Clip(string pwcSourceFilename, long i64StartTimeMs, long i64LengthMs, string pwcClipFilename);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr LoadLibrary(string sFilenameDll);

        /// <summary>
        /// Convert UTF8 to UTF16 string
        /// </summary>
        /// <param name="utf8String">UTF8 string</param>
        /// <returns>UTF16 string</returns>
        public static string Utf8ToUtf16(string utf8String)
        {
            // Get UTF-8 bytes by reading each byte with ANSI encoding
            byte[] utf8Bytes = Encoding.Default.GetBytes(utf8String);

            // Convert UTF-8 bytes to UTF-16 bytes
            byte[] utf16Bytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, utf8Bytes);

            // Return UTF-16 bytes as UTF-16 string
            return Encoding.Unicode.GetString(utf16Bytes);
        }

        /// <summary>
        /// Allocates a managed String and copies all characters up to the first null character 
        /// from an unmanaged UTF-8 string into it.
        /// </summary>
        public static string PtrToStringUTF8(IntPtr ptr)
        {
            int len = 0;
            while (Marshal.ReadByte(ptr, len) != 0) ++len;
            byte[] buffer = new byte[len];
            Marshal.Copy(ptr, buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }

        bool bCanUse = false;
        IntPtr pFBRecorder = IntPtr.Zero;

        /// <summary>
        /// Constructor
        /// </summary>
        public TFBRecorder() :
            this("", "", 0, TLogKind.None)
        {
        }

        public TFBRecorder(string sLicence) :
            this(sLicence, "", 0, TLogKind.None)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sLogFolderName">Path to log folder (if logKind != None)</param>
        /// <param name="i32LogFolderSizeMb">Max log file size in megabytes</param>
        /// <param name="logKind">Log level</param>
        public TFBRecorder(string sLicence, string sLogFolderName, int i32LogFolderSizeMb, TLogKind logKind)
        {
            string sFBRecorderDll = "FBRecorder.dll";
            IntPtr hFBRecorderDllHandle = LoadLibrary(sFBRecorderDll);
            if (hFBRecorderDllHandle == IntPtr.Zero)
            {
#if DEBUG
                sFBRecorderDll = "C:\\dev\\FBGames\\FBv6\\FinalDebug\\FBRecorder.dll";
#else
                sFBRecorderDll = "C:\\Program Files\\Blueberry Software\\FlashBack SDK 5\\FBRecorder.dll";
#endif
                hFBRecorderDllHandle = LoadLibrary(sFBRecorderDll);
            }
            if (hFBRecorderDllHandle == IntPtr.Zero)
                new Exception("The FBRecorder.dll is not found.");
            Marshal.PrelinkAll(typeof(TFBRecorder));
            if (logKind != TLogKind.None)
                InitializeLog(sLogFolderName, i32LogFolderSizeMb, logKind);
            pFBRecorder = CreateRecorder(sLicence);
            if (pFBRecorder != IntPtr.Zero)
                bCanUse = true;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sLogFolderName">Path to log folder (if logKind != None)</param>
        /// <param name="i32LogFolderSizeMb">Max log file size in megabytes</param>
        /// <param name="logKind">Log level</param>
        public TFBRecorder(string sFBRecorderDllPath, string sLicence, string sLogFolderName, int i32LogFolderSizeMb, TLogKind logKind)
        {
            IntPtr hFBRecorderDllHandle = LoadLibrary(sFBRecorderDllPath);
            if (hFBRecorderDllHandle == IntPtr.Zero)
                new Exception("The FBRecorder.dll is not found.");
            Marshal.PrelinkAll(typeof(TFBRecorder));
            if (logKind != TLogKind.None)
                InitializeLog(sLogFolderName, i32LogFolderSizeMb, logKind);
            pFBRecorder = CreateRecorder(sLicence);
            if (pFBRecorder != IntPtr.Zero)
                bCanUse = true;
        }

        /// <summary>
        /// Is the recorder object available
        /// </summary>
        /// <returns>True if recorder object successfully created</returns>
        public bool CanUse()
        {
            return bCanUse;
        }

        /// <summary>
        /// Destroys a recorder object
        /// </summary>
        public void Destroy()
        {
            if (pFBRecorder != IntPtr.Zero)
                DestroyRecorder(pFBRecorder);
            pFBRecorder = IntPtr.Zero;
        }

        /// <summary>
        /// Initializes the internal recorder structures
        /// </summary>
        /// <param name="sMP4Filename">Path to MP4 output file/param>
        /// <returns>True on success</returns>
        public bool InitRecorder(string sMP4Filename)
        {
            return InitRecorder(pFBRecorder, sMP4Filename);
        }

        /// <summary>
        /// Starts the recording
        /// </summary>
        /// <returns>True on success</returns>
        public bool StartRecording()
        {
            return StartRecording(pFBRecorder);
        }

        /// <summary>
        /// Stops the recording
        /// </summary>
        public void StopRecording()
        {
            StopRecording(pFBRecorder);
        }

        /// <summary>
        /// Returns a JSON string with the default SDK Recorder settings
        /// </summary>
        /// <returns>JSON string with the default SDK Recorder settings or empty string on error</returns>
        public string GetDefaultConfigJSON()
        {
            IntPtr pProfileConfigJSON = IntPtr.Zero;
            if (!GetDefaultConfigJSON(pFBRecorder, out pProfileConfigJSON))
                return "";
            //convert a pointer to a text string
            return PtrToStringUTF8(pProfileConfigJSON);
        }

        /// <summary>
        /// Returns a JSON string with the SDK Recorder settings for a predefined profile
        /// </summary>
        /// <param name="FBrecorderProfile">Predefined recorder profile</param>
        /// <returns>JSON string with the SDK Recorder settings for a predefined profile or empty string on error</returns>
        public string GetConfigJSONForProfile(TFBRecorderProfiles FBrecorderProfile)
        {
            return GetConfigJSONForProfile((int)FBrecorderProfile);
        }

        /// <summary>
        /// Returns a JSON string with the SDK Recorder settings for a predefined profile
        /// </summary>
        /// <param name="iFBrecorderProfile">Predefined recorder profile</param>
        /// <returns>JSON string with the SDK Recorder settings for a predefined profile or empty string on error</returns>
        public string GetConfigJSONForProfile(int iFBrecorderProfile)
        {
            IntPtr pProfileConfigJSON = IntPtr.Zero;
            if (!GetConfigJSONForProfile(pFBRecorder, iFBrecorderProfile, out pProfileConfigJSON))
            {
                Console.WriteLine("Error: unable to get a profile config json.");
                return "";
            }
            //convert a pointer to a text string
            return PtrToStringUTF8(pProfileConfigJSON);
        }

        /// <summary>
        /// Applies the settings in a JSON string for this SDK Recorder object
        /// </summary>
        /// <param name="sConfigJSON">JSON string with the SDK Recorder settings</param>
        /// <returns>True on success</returns>
        public bool SetConfigJSON(string sConfigJSON)
        {
            return SetConfigJSON(pFBRecorder, sConfigJSON);
        }

        /// <summary>
        /// checks the availability of the video encoder object. 
        /// SDK supports many encoders but only part of them can be available on a local computer
        /// </summary>
        /// <param name="i32VideoEncoderType"></param>
        /// <param name="sVideoEncoderName"></param>
        /// <returns>True if encoder is available</returns>
        public bool CheckVideoEncoderAvailability(int i32VideoEncoderType, string sVideoEncoderName)
        {
            return CheckVideoEncoderAvailability(pFBRecorder, i32VideoEncoderType, sVideoEncoderName);
        }

        /// <summary>
        /// Applies a predefined profile for this SDK Recorder object
        /// </summary>
        /// <param name="FBrecorderProfile">Predefined profile</param>
        /// <returns>True on success</returns>
        public bool ActivateProfile(TFBRecorderProfiles FBrecorderProfile)
        {
            string sProfileJSON = GetConfigJSONForProfile(FBrecorderProfile);
            if (sProfileJSON.Length == 0)
                return false;
            return SetConfigJSON(pFBRecorder, sProfileJSON);
        }

        /// <summary>
        /// Returns Error type. If the return value is 0 then no more errors exist in the list.
        /// </summary>
        /// <param name="i32Index">Errors list index</param>
        /// <returns>Error type or 0 if no more errors avalable</returns>
        public TFBGlobalErrors GetGlobalError(int i32Index)
        {
            return (TFBGlobalErrors)GetGlobalError(pFBRecorder, i32Index);
        }

        /// <summary>
        /// Reset errors
        /// </summary>
        public void ResetGlobalErrors()
        {
            ResetGlobalErrors(pFBRecorder);
        }

        public bool CaptureScreen(string sImageFilename, TFBRecorderImageFormat ImageFormat, int i32Compression_0_9, int i32Left, int i32Top, int i32Width, int i32Height)
        {
            return CaptureScreen(sImageFilename, (int)ImageFormat, i32Compression_0_9, i32Left, i32Top, i32Width, i32Height);
        }

        /// <summary>
        /// Pause the recording
        /// </summary>
        /// <param name="pRecorderObject">Pointer to recorder object</param>
        /// <returns>True on success</returns>
        public bool Pause(bool bPause)
        {
            return Pause(pFBRecorder, bPause);
        }

        /// <summary>
        /// Is recoridng in progress
        /// </summary>
        /// <param name="pRecorderObject">Pointer to recorder object</param>
        /// <returns>True on success</returns>
        public bool IsRecording()
        {
            return IsRecording(pFBRecorder);
        }

        /// <summary>
        /// Is paused
        /// </summary>
        /// <param name="pRecorderObject">Pointer to recorder object</param>
        /// <returns>True on success</returns>
        public bool IsPaused()
        {
            return IsPaused(pFBRecorder);
        }

        public static bool MergeMP4Files(string sMP4FilesJSON, string sResultFilename)
        {
            byte[] utfBytes = Encoding.UTF8.GetBytes(sMP4FilesJSON);
            return MergeMP4Files(utfBytes, sResultFilename);
        }

    };
}