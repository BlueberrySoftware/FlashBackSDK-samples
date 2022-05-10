import ctypes
from enum import Enum

class LogKind(Enum):
    Info = 0 #minimum
    Debug = 1 #medium
    Verbose = 2 #maximum
    NoLog = 3

class FBRecorderProfile(Enum):
    SAFE_SCREEN = 0#//GDI mode, screen only, software h264 encoder
    HIGH_SCREEN = 1#//Duplication Output mode, screen only, hardware h264 encoder
    SAFE_SCREEN_AUDIO = 2#//GDI mode, screen, PC Sounds, Mic, software h264 encoder
    HIGH_SCREEN_AUDIO = 3#//Duplication Output mode, screen, PC Sounds, Mic, hardware h264 encoder
    SAFE_SCREEN_AUDIO_VIDEOCAPTURE = 4#//GDI mode, screen, PC Sounds, Mic, WebCam, software h264 encoder
    HIGH_SCREEN_AUDIO_VIDEOCAPTURE = 5#//Duplication Output mode, screen, PC Sounds, Mic, WebCam, hardware h264 encoder
    SAFE_SCREEN_MIC = 6#//GDI mode, screen, Mic, software h264 encoder
    SAFE_SCREEN_PCSOUNDS = 7#//GDI mode, screen, PC Sounds, software h264 encoder
    HIGH_SCREEN_MIC = 8#//Duplication API mode, screen, Mic, hardware h264 encoder
    HIGH_SCREEN_PCSOUNDS = 9#//Duplication API mode, screen, PC Sounds, hardware h264 encoder
    SAFE_SCREEN_MIC_VIDEOCAPTURE = 10#//GDI mode, screen, Mic, WebCam, software h264 encoder
    HIGH_SCREEN_MIC_VIDEOCAPTURE = 11#//Duplication API mode, screen, Mic, WebCam, hardware h264 encoder

class FBGlobalError(Enum):
    Unknown = 0
    AudioDeviceIsInUseByAnotherApplication = 1

class FBRecorderImageFormat(Enum):
    JPG = 0
    BMP = 1
    PNG = 2

class FBRecorder:

    #log_full_path can be None or empty if the log is not required
    def __init__(self, dll_full_path, sLicence, log_full_path, log_folder_size_mb, log_kind) -> None:
        super().__init__()
        self.sLicence = sLicence
        self.hModule = ctypes.WinDLL (dll_full_path)
        #
        self.CreateRecorderProto = ctypes.WINFUNCTYPE(ctypes.c_void_p, ctypes.c_wchar_p)
        self.CreateRecorderDll = self.CreateRecorderProto(("CreateRecorder", self.hModule), None)
        #
        self.DestroyRecorderProto = ctypes.WINFUNCTYPE(None, ctypes.c_void_p)
        self.DestroyRecorderDll = self.DestroyRecorderProto(("DestroyRecorder", self.hModule))
        #
        self.InitializeLogProto = ctypes.WINFUNCTYPE(None, ctypes.c_wchar_p, ctypes.c_int32, ctypes.c_int32)
        self.InitializeLogDll = self.InitializeLogProto(("InitializeLog", self.hModule))
        if type(log_full_path) == str and len(log_full_path) > 0:
            self.InitializeLogDll(ctypes.c_wchar_p(log_full_path), ctypes.c_int32(log_folder_size_mb), ctypes.c_int32(log_kind.value))
        #
        self.InitRecorderProto = ctypes.WINFUNCTYPE(ctypes.c_bool, ctypes.c_void_p, ctypes.c_wchar_p)
        self.InitRecorderDll = self.InitRecorderProto(("InitRecorder", self.hModule))
        #
        self.GetConfigJSONForProfileProto = ctypes.WINFUNCTYPE(ctypes.c_bool, ctypes.c_void_p, ctypes.c_int32, ctypes.c_void_p)
        self.GetConfigJSONForProfileDll = self.GetConfigJSONForProfileProto(("GetConfigJSONForProfile", self.hModule))
        #
        self.SetConfigJSONProto = ctypes.WINFUNCTYPE(ctypes.c_bool, ctypes.c_void_p, ctypes.c_void_p)
        self.SetConfigJSONDll = self.SetConfigJSONProto(("SetConfigJSON", self.hModule))
        #
        self.StartRecordingProto = ctypes.WINFUNCTYPE(ctypes.c_bool, ctypes.c_void_p)
        self.StartRecordingDll = self.StartRecordingProto(("StartRecording", self.hModule))
        #
        self.StopRecordingProto = ctypes.WINFUNCTYPE(None, ctypes.c_void_p)
        self.StopRecordingDll = self.StopRecordingProto(("StopRecording", self.hModule))
        #
        self.GetDefaultConfigJSONProto = ctypes.WINFUNCTYPE(ctypes.c_bool, ctypes.c_void_p, ctypes.c_void_p)
        self.GetDefaultConfigJSONDll = self.GetDefaultConfigJSONProto(("GetDefaultConfigJSON", self.hModule))
        #
        self.CheckVideoEncoderAvailabilityProto = ctypes.WINFUNCTYPE(ctypes.c_bool, ctypes.c_void_p, ctypes.c_int32, ctypes.c_wchar_p)
        self.CheckVideoEncoderAvailabilityDll = self.CheckVideoEncoderAvailabilityProto(("CheckVideoEncoderAvailability", self.hModule))
        #
        self.GetGlobalErrorProto = ctypes.WINFUNCTYPE(ctypes.c_int32, ctypes.c_void_p, ctypes.c_int32)
        self.GetGlobalErrorDll = self.GetGlobalErrorProto(("GetGlobalError", self.hModule))
        #
        self.ResetGlobalErrorsProto = ctypes.WINFUNCTYPE(None, ctypes.c_void_p)
        self.ResetGlobalErrorsDll = self.ResetGlobalErrorsProto(("ResetGlobalErrors", self.hModule))
        #
        self.CaptureScreenProto = ctypes.WINFUNCTYPE(ctypes.c_bool, ctypes.c_wchar_p, ctypes.c_int32, ctypes.c_int32, ctypes.c_int32, ctypes.c_int32, ctypes.c_int32, ctypes.c_int32)
        self.CaptureScreenDll = self.CaptureScreenProto(("CaptureScreen", self.hModule))
        #
        self.PauseProto = ctypes.WINFUNCTYPE(ctypes.c_bool, ctypes.c_void_p, ctypes.c_bool)
        self.PauseDll = self.PauseProto(("Pause", self.hModule))
        #
        self.IsRecordingProto = ctypes.WINFUNCTYPE(ctypes.c_bool, ctypes.c_void_p)
        self.IsRecordingDll = self.IsRecordingProto(("IsRecording", self.hModule))
        #
        self.IsPausedProto = ctypes.WINFUNCTYPE(ctypes.c_bool, ctypes.c_void_p)
        self.IsPausedDll = self.IsPausedProto(("IsPaused", self.hModule))
        #
        self.MergeMP4FilesProto = ctypes.WINFUNCTYPE(ctypes.c_bool, ctypes.c_void_p, ctypes.c_wchar_p)
        self.MergeMP4FilesDll = self.MergeMP4FilesProto(("MergeMP4Files", self.hModule))

    def __del__(self):
        self.DestroyRecorder()

    def CreateRecorder(self) -> bool:
        self.pRecorder = self.CreateRecorderDll(ctypes.c_wchar_p(self.sLicence))
        if self.pRecorder:
            return True
        return False

    def DestroyRecorder(self):
        if self.pRecorder:
            self.DestroyRecorderDll(self.pRecorder)
            self.pRecorder = 0

    #initializes the recorder object, creates all required internal structures
    def InitRecorder(self, mp4_filename) -> bool:
        return self.InitRecorderDll(self.pRecorder, ctypes.c_wchar_p(mp4_filename))

	#retrieves a JSON text string for predefined profie, UTF-8 format
    def GetConfigJSONForProfile(self, profile):
        pcresult = ctypes.c_char_p()
        bresult = self.GetConfigJSONForProfileDll(self.pRecorder, ctypes.c_int32(profile.value), ctypes.byref(pcresult))
        return bresult, pcresult.value.decode()

    #applies a JSON text string with the recorder settings to the recorder object, UTF-8 format
    def SetConfigJSON(self, jsonconfig) -> bool:
        pcjson = ctypes.c_char_p()
        pcjson.value = jsonconfig.encode()
        return self.SetConfigJSONDll(self.pRecorder, pcjson)

    def ActivateProfile(self, profile) -> bool:
        bresult, sconfig = self.GetConfigJSONForProfile(profile)
        if not bresult:
            return False
        return self.SetConfigJSON(sconfig)

    def StartRecording(self) -> bool:
        return self.StartRecordingDll(self.pRecorder)

    def StopRecording(self):
        self.StopRecordingDll(self.pRecorder)        

    #retrieves a JSON text string with the default settings, UTF-8 format
    def GetDefaultConfigJSON(self):
        pcresult = ctypes.c_char_p()
        bresult = self.GetDefaultConfigJSONDll(self.pRecorder, ctypes.byref(pcresult))
        return bresult, pcresult.value.decode()

    #checks the availability of the video encoder object. SDK supports many encoders but only part of them can be available on a local computer
    def CheckVideoEncoderAvailability(self, videoencodertype, videoencodername) -> bool:
        return self.CheckVideoEncoderAvailabilityDll(self.pRecorder, videoencodertype, ctypes.c_wchar_p(videoencodername))

    #returns FBGlobalError type. If the return value is 0 then no more errors exist in the list
    def GetGlobalError(self, errorindex) -> FBGlobalError:
        return FBGlobalError(self.GetGlobalErrorDll(self.pRecorder, errorindex))

    def ResetGlobalErrors(self):
        self.ResetGlobalErrorsDll(self.pRecorder)
    
    def CaptureScreen(self, filename, image_format, compression_0_9, left, top, width, height) -> bool:
        return self.CaptureScreenDll(filename, ctypes.c_int32(image_format.value), compression_0_9, left, top, width, height)

    def Pause(self, bPause) -> bool:
        return self.PauseDll(self.pRecorder, ctypes.c_bool(bPause))

    def IsRecording(self) -> bool:
        return self.IsRecordingDll(self.pRecorder)

    def IsPaused(self) -> bool:
        return self.IsPausedDll(self.pRecorder)

    def MergeMP4Files(self, sMP4FilesJSON, sResultFilename) -> bool:
        pcjson = ctypes.c_char_p()
        pcjson.value = sMP4FilesJSON.encode()
        return self.MergeMP4FilesDll(pcjson, ctypes.c_wchar_p(sResultFilename))

