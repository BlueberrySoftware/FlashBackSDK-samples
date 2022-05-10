//if you don't use the MSVC++ precompiled headers then comment the following line
#include "stdafx.h"
#include "FBRecorder.h"

namespace FBRecorder
{
	MergeMP4FilesType TFBRecorder::pMergeMP4Files = nullptr;
	CreateMP4ClipType TFBRecorder::pCreateMP4Clip = nullptr;

	TFBRecorder::TFBRecorder(std::wstring wsRecorderDll, wchar_t* pwcLicence, std::wstring sLogFolderName, int i32LogFolderSizeMb, TLogKind logKind)
	{
		hFBRecorderDll = LoadLibrary(wsRecorderDll.c_str());
		if (!hFBRecorderDll)
		{
			//printf("FBRecorder.dll not found.\n");
			goto end_of_func;
		}
		pCreateRecorder = (CreateRecorderType)GetProcAddress(hFBRecorderDll, "CreateRecorder");
		pDestroyRecorder = (DestroyRecorderType)GetProcAddress(hFBRecorderDll, "DestroyRecorder");
		pInitializeLog = (InitializeLogType)GetProcAddress(hFBRecorderDll, "InitializeLog");
		pInitRecorder = (InitRecorderType)GetProcAddress(hFBRecorderDll, "InitRecorder");
		pStartRecording = (StartRecordingType)GetProcAddress(hFBRecorderDll, "StartRecording");
		pStopRecording = (StopRecordingType)GetProcAddress(hFBRecorderDll, "StopRecording");
		pGetDefaultConfigJSON = (GetDefaultConfigJSONType)GetProcAddress(hFBRecorderDll, "GetDefaultConfigJSON");
		pGetConfigJSONForProfile = (GetConfigJSONForProfileType)GetProcAddress(hFBRecorderDll, "GetConfigJSONForProfile");
		pSetConfigJSON = (SetConfigJSONType)GetProcAddress(hFBRecorderDll, "SetConfigJSON");
		pCheckVideoEncoderAvailability = (CheckVideoEncoderAvailabilityType)GetProcAddress(hFBRecorderDll, "CheckVideoEncoderAvailability");
		pGetGlobalError = (GetGlobalErrorType)GetProcAddress(hFBRecorderDll, "GetGlobalError");
		pResetGlobalErrors = (ResetGlobalErrorsType)GetProcAddress(hFBRecorderDll, "ResetGlobalErrors");
		pCaptureScreen = (CaptureScreenType)GetProcAddress(hFBRecorderDll, "CaptureScreen");
		pPause = (PauseType)GetProcAddress(hFBRecorderDll, "Pause");
		pIsRecording = (IsRecordingType)GetProcAddress(hFBRecorderDll, "IsRecording");
		pIsPaused = (IsPausedType)GetProcAddress(hFBRecorderDll, "IsPaused");
		pMergeMP4Files = (MergeMP4FilesType)GetProcAddress(hFBRecorderDll, "MergeMP4Files");
		pCreateMP4Clip = (CreateMP4ClipType)GetProcAddress(hFBRecorderDll, "CreateMP4Clip");
		//
		if (!pCreateRecorder || !pDestroyRecorder || !pInitializeLog || !pInitRecorder || !pStartRecording || !pStopRecording || !pGetDefaultConfigJSON ||
			!pGetConfigJSONForProfile || !pSetConfigJSON || !pCheckVideoEncoderAvailability || !pGetGlobalError || !pResetGlobalErrors || !pCaptureScreen ||
			!pMergeMP4Files)
		{
			//printf("Error: some exported functions are not found.\n");
			goto end_of_func;
		}
		if (logKind != TLogKind::None)
			pInitializeLog(sLogFolderName.c_str(), i32LogFolderSizeMb, logKind);
		pRecorderObject = pCreateRecorder(pwcLicence);
		if (!pRecorderObject)
		{
			//printf("Error: unable to create a recorder object.\n");
			goto end_of_func;
		}
		bCanUse = true;
end_of_func:
		return;
	}

	TFBRecorder::~TFBRecorder()
	{
		if (pRecorderObject)
			pDestroyRecorder(pRecorderObject);
	}

	bool TFBRecorder::CanUse()
	{
		return bCanUse;
	}

	std::string TFBRecorder::GetDefaultConfigJSON()
	{
		char* pConfigJSON = nullptr;
		if (!pGetDefaultConfigJSON(pRecorderObject, &pConfigJSON))
			return "";
		return pConfigJSON;
	}

	std::string TFBRecorder::GetConfigJSONForProfile(TFBRecorderProfile FBRecorderProfile)
	{
		char* pConfigJSON = nullptr;
		if (!pGetConfigJSONForProfile(pRecorderObject, (int32_t)FBRecorderProfile, &pConfigJSON))
			return "";
		return pConfigJSON;
	}

	bool TFBRecorder::SetConfigJSON(std::string sConfigJSON)
	{
		return pSetConfigJSON(pRecorderObject, sConfigJSON.c_str());
	}

	bool TFBRecorder::InitRecorder(std::wstring wsMP4Filename)
	{
		return pInitRecorder(pRecorderObject, wsMP4Filename.c_str());
	}

	bool TFBRecorder::StartRecording()
	{
		return pStartRecording(pRecorderObject);
	}

	void TFBRecorder::StopRecording()
	{
		pStopRecording(pRecorderObject);
	}

	bool TFBRecorder::ActivateProfile(TFBRecorderProfile FBRecorderProfile)
	{
		return SetConfigJSON(GetConfigJSONForProfile(FBRecorderProfile));
	}

	bool TFBRecorder::CheckVideoEncoderAvailability(int32_t i32VideoEncoderType, std::wstring wsVideoEncoderName)
	{
		return pCheckVideoEncoderAvailability(pRecorderObject, i32VideoEncoderType, wsVideoEncoderName.c_str());
	}

	TFBGlobalError TFBRecorder::GetGlobalError(int32_t i32ErrorIndex)
	{
		return (TFBGlobalError)pGetGlobalError(pRecorderObject, i32ErrorIndex);
	}

	void TFBRecorder::ResetGlobalErrors()
	{
		pResetGlobalErrors(pRecorderObject);
	}

	bool TFBRecorder::CaptureScreen(const wchar_t* pwcImageFilename, TImageExporterType ImageFormat, int i32Compression_0_9, int i32Left, int i32Top, int i32Width, int i32Height)
	{
		return pCaptureScreen(pwcImageFilename, ImageFormat, i32Compression_0_9, i32Left, i32Top, i32Width, i32Height);
	}

	bool TFBRecorder::Pause(bool bPause)
	{
		return pPause(pRecorderObject, bPause);
	}

	bool TFBRecorder::IsRecording()
	{
		return pIsRecording(pRecorderObject);
	}

	bool TFBRecorder::IsPaused()
	{
		return pIsPaused(pRecorderObject);
	}

	bool TFBRecorder::MergeMP4Files(const char* pcMP4FilesJSON, const wchar_t* pwcResultFilename)
	{
		return pMergeMP4Files(pcMP4FilesJSON, pwcResultFilename);
	}

	bool TFBRecorder::CreateMP4Clip(const wchar_t* pwcSourceFilename, int64_t i64StartTimeMs, int64_t i64LengthMs, const wchar_t* pwcClipFilename)
	{
		return pCreateMP4Clip(pwcSourceFilename, i64StartTimeMs, i64LengthMs, pwcClipFilename);
	}
};
