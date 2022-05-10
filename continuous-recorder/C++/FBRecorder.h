#ifndef __FBRECORDER_H__
#define __FBRECORDER_H__

#include "Windows.h"
#include <stdint.h>
#include <string>

namespace FBRecorder
{
	//log levels for internal SDK code
	enum class TLogKind : int32_t
	{
		Info = 0,//minimum
		Debug = 1,//medium
		Verbose = 2,//maximum
		None = 3
	};

	enum class TFBGlobalError
	{
		Unknown = 0,
		AudioDeviceIsInUseByAnotherApplication = 1
	};

	//predefined profiles
	enum class TFBRecorderProfile : int32_t
	{
		SAFE_SCREEN = 0,//GDI mode, screen only, software h264 encoder
		HIGH_SCREEN = 1,//Duplication Output mode, screen only, hardware h264 encoder
		SAFE_SCREEN_AUDIO = 2,//GDI mode, screen, PC Sounds, Mic, software h264 encoder
		HIGH_SCREEN_AUDIO = 3,//Duplication Output mode, screen, PC Sounds, Mic, hardware h264 encoder
		SAFE_SCREEN_AUDIO_VIDEOCAPTURE = 4,//GDI mode, screen, PC Sounds, Mic, WebCam, software h264 encoder
		HIGH_SCREEN_AUDIO_VIDEOCAPTURE = 5,//Duplication Output mode, screen, PC Sounds, Mic, WebCam, hardware h264 encoder
		SAFE_SCREEN_MIC = 6,//GDI mode, screen, Mic, software h264 encoder
		SAFE_SCREEN_PCSOUNDS = 7,//GDI mode, screen, PC Sounds, software h264 encoder
		HIGH_SCREEN_MIC = 8,//Duplication API mode, screen, Mic, hardware h264 encoder
		HIGH_SCREEN_PCSOUNDS = 9,//Duplication API mode, screen, PC Sounds, hardware h264 encoder
		SAFE_SCREEN_MIC_VIDEOCAPTURE = 10,//GDI mode, screen, Mic, WebCam, software h264 encoder
		HIGH_SCREEN_MIC_VIDEOCAPTURE = 11,//Duplication API mode, screen, Mic, WebCam, hardware h264 encoder
		TOTAL_NUMBER = 12
	};

	enum class TImageExporterType
	{
		JPG = 0,
		BMP = 1,
		PNG = 2
	};

	//define types for functions to be exported from the SDK Recorder dll
	//create a recorder object and returns a pointer to this object
	typedef void* (__stdcall *CreateRecorderType)(wchar_t* pwcLicence);
	//destroys a recorder object
	typedef void(__stdcall *DestroyRecorderType)(void* pRecorderObject);
	//initializes log using a custom folder, max files size and log level
	typedef void(__stdcall *InitializeLogType)(const wchar_t* sLogFolderName, int i32LogFolderSizeMb, TLogKind logKind);
	//initializes the recorder object, creates all required internal structures
	typedef bool(__stdcall *InitRecorderType)(void* pRecorderObject, const wchar_t* pwsMP4Filename);
	//starts the recording
	typedef bool(__stdcall *StartRecordingType)(void* pRecorderObject);
	//stops the recording
	typedef void(__stdcall *StopRecordingType)(void* pRecorderObject);
	//retrieves a JSON text string with the default settings, UTF-8 format
	typedef bool(__stdcall *GetDefaultConfigJSONType)(void* pRecorderObject, char** ppsProfileConfigJSON);
	//retrieves a JSON text string for predefined profie, UTF-8 format
	typedef bool(__stdcall *GetConfigJSONForProfileType)(void* pRecorderObject, int32_t i32ProfileId, char** ppsProfileConfigJSON);
	//applies a JSON text string with the recorder settings to the recorder object, UTF-8 format
	typedef bool(__stdcall *SetConfigJSONType)(void* pRecorderObject, const char* psProfileConfigJSON);
	//checks the availability of the video encoder object. SDK supports many encoders but only part of them can be available on a local computer
	typedef bool(__stdcall *CheckVideoEncoderAvailabilityType)(void* pRecorderObject, int32_t i32VideoEncoderType, const wchar_t* pwsVideoEncoderName);
	//returns Error type. If the return value is 0 then no more errors exist in the list
	typedef int32_t(__stdcall *GetGlobalErrorType)(void* pRecorderObject, int32_t i32ErrorIndex);
	typedef void(__stdcall *ResetGlobalErrorsType)(void* pRecorderObject);
	typedef bool(__stdcall *CaptureScreenType)(const wchar_t* pwcImageFilename, TImageExporterType ImageFormat, int i32Compression_0_9, int i32Left, int i32Top, int i32Width, int i32Height);
	//
	typedef bool(__stdcall *PauseType)(void* pRecorderObject, bool bPause);
	typedef bool(__stdcall *IsRecordingType)(void* pRecorderObject);
	typedef bool(__stdcall *IsPausedType)(void* pRecorderObject);
	typedef bool(__stdcall *MergeMP4FilesType)(const char* pcMP4FilesJSON, const wchar_t* pwcResultFilename);
	typedef bool(__stdcall *CreateMP4ClipType)(const wchar_t* pwcSourceFilename, int64_t i64StartTimeMs, int64_t i64LengthMs, const wchar_t* pwcClipFilename);


	//C++ class - a wrapper for the SDK Recorder object
	class TFBRecorder
	{
	private:
		//recorder dll handle
		HMODULE hFBRecorderDll = nullptr;
		//a pointer to a recorder object
		void* pRecorderObject = nullptr;
		//pointers to imported functions from the SDK Recorder dll
		CreateRecorderType pCreateRecorder = nullptr;
		DestroyRecorderType pDestroyRecorder = nullptr;
		InitializeLogType pInitializeLog = nullptr;
		InitRecorderType pInitRecorder = nullptr;
		StartRecordingType pStartRecording = nullptr;
		StopRecordingType pStopRecording = nullptr;
		GetDefaultConfigJSONType pGetDefaultConfigJSON = nullptr;
		GetConfigJSONForProfileType pGetConfigJSONForProfile = nullptr;
		SetConfigJSONType pSetConfigJSON = nullptr;
		CheckVideoEncoderAvailabilityType pCheckVideoEncoderAvailability = nullptr;
		GetGlobalErrorType pGetGlobalError = nullptr;
		ResetGlobalErrorsType pResetGlobalErrors = nullptr;
		CaptureScreenType pCaptureScreen = nullptr;
		PauseType pPause = nullptr;
		IsRecordingType pIsRecording = nullptr;
		IsPausedType pIsPaused = nullptr;
		static MergeMP4FilesType pMergeMP4Files;
		static CreateMP4ClipType pCreateMP4Clip;
		//a property to indicate the recorder object availability
		bool bCanUse = false;
	public:
		//a constructor of the wrapper. Set the logKind to TLogKind::None to disable logging
		TFBRecorder(std::wstring wsRecorderDll, wchar_t* pwcLicence, std::wstring sLogFolderName, int i32LogFolderSizeMb, TLogKind logKind);
		//a destructor
		~TFBRecorder();
		//is the recorder object available
		bool CanUse();
		//returns a JSON string with the default SDK Recorder settings
		std::string GetDefaultConfigJSON();
		//returns a JSON string with the SDK Recorder settings for a predefined profile
		std::string GetConfigJSONForProfile(TFBRecorderProfile FBRecorderProfile);
		//applies the settings in a JSON string for this SDK Recorder object
		bool SetConfigJSON(std::string sConfigJSON);
		//initializes the internal recorder structures
		bool InitRecorder(std::wstring wsMP4Filename);
		//starts the recording
		bool StartRecording();
		//stops the recording
		void StopRecording();
		//applies a predefined profile for this SDK Recorder object
		bool ActivateProfile(TFBRecorderProfile FBRecorderProfile);
		//is the video encoder available
		//i32VideoEncoderType: Unknown = 0, MSMF = 1, NativeIntel = 2, NativeNvidia = 3, NativeAMD = 4
		//wsVideoEncoderName depends on the i32VideoEncoderType value
		//all available encoders can be found in the JSON settings string: ["VideoEncoderParameters"]["SupportedEncoders"][array of supported encoders]
		//for example:
		//    "SupportedEncoders": [
		//      {
		//        "Hardware": true,
		//        "Name": "NVIDIA H.264 Encoder MFT",
		//        "Type": 1
		//      },
		//      {
		//        "Hardware": true,
		//        "Name": "Intel® Quick Sync Video H.264 Encoder MFT",
		//        "Type": 1
		//      },
		//	...
		bool CheckVideoEncoderAvailability(int32_t i32VideoEncoderType, std::wstring wsVideoEncoderName);
		TFBGlobalError GetGlobalError(int32_t i32ErrorIndex);
		void ResetGlobalErrors();
		bool CaptureScreen(const wchar_t* pwcImageFilename, TImageExporterType ImageFormat, int i32Compression_0_9, int i32Left, int i32Top, int i32Width, int i32Height);
		bool Pause(bool bPause);
		bool IsRecording();
		bool IsPaused();
		static bool MergeMP4Files(const char* pcMP4FilesJSON, const wchar_t* pwcResultFilename);
		static bool CreateMP4Clip(const wchar_t* pwcSourceFilename, int64_t i64StartTimeMs, int64_t i64LengthMs, const wchar_t* pwcClipFilename);
	};

	//Using the recorder object:
	//	std::unique_ptr<FBRecorder::TFBRecorder> pFBRecorder(new FBRecorder::TFBRecorder(wsLogFolder, 10, FBRecorder::TLogKind::Verbose));
	//	if (!pFBRecorder->CanUse())
	//	{
	//		printf("Error: unable to use a recorder wrapper.\n");
	//		goto end_of_func;
	//	}
	//      if(!pFBRecorder->ActivateProfile(FBRecorder::TFBRecorderProfile::SAFE_SCREEN))
	//	{
	//		printf("Error: unable to activate a predefined profile.\n");
	//		goto end_of_func;
	//	}
	//	if (!pFBRecorder->InitRecorder(wsMP4Filename))
	//	{
	//		printf("Error: unable to init recorder.\n");
	//		goto end_of_func;
	//	}
	//	
	//	if (!pFBRecorder->StartRecording())
	//	{
	//		printf("Error: unable to start the recording.\n");
	//		goto end_of_func;
	//	}
	//	printf("Recording... To stop the recording press Esc...\n");
	//	while(true)
	//	{
	//		if(_getch() == 27)
	//			break;
	//	}
	//	printf("Stopping the recording...\n");
	//	pFBRecorder->StopRecording();
	//end_of_func:
	//	pFBRecorder = nullptr;//auto delete

};

#endif
