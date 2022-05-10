//
//
// Sample code that records the screen continuously, saving only the last N minutes.
// 
// Creates a console application that records continuously into multiple MP4 files, keeping the last X files. On completion, it merges the files into one MP4.
// The user enters two values into the application, to define the length of the 'segment' MP4 files and the maximum number to combine when the application exits. 
//
//

#include "stdafx.h"
#include "FBRecorder.h"
#include <filesystem>
#include <sstream>
#include <conio.h>
#include "json.hpp"


void PrintGlobalErrors(FBRecorder::TFBRecorder *pFBRecorder)
{
	int32_t i32ErrorIndex = 0;
	while (true)
	{
		FBRecorder::TFBGlobalError Error = pFBRecorder->GetGlobalError(i32ErrorIndex);
		if (Error == FBRecorder::TFBGlobalError::Unknown)
			break;
		printf("Global error: %d\n", (int32_t)Error);
		i32ErrorIndex++;
	}
}


//
// CreateRecorder()
// Create a new instance of TFBRecorder and get it ready to record to a new MP4 file. Return the instance pointer and filename back to ContinuousRecording()
//
std::tuple<std::shared_ptr<FBRecorder::TFBRecorder>, std::wstring> CreateRecorder()
{
	std::string sProfileConfigJSON;
	json::JSON jsonSettings;
	std::experimental::filesystem::path appPath(__argv[0]);
	std::wstring wsLogFolder = (std::wstring)appPath.parent_path() + L"\\ContinuousRecording";
	std::wostringstream swsMP4Filename;
	std::shared_ptr<FBRecorder::TFBRecorder> pResult;

	static int32_t i32FileIndex = 0;

	CreateDirectory(wsLogFolder.c_str(), nullptr);

	// Construct the filename for the new MP4, using i32FileIndex. 
	swsMP4Filename << appPath.parent_path() << L"\\test_" << i32FileIndex++ << L".mp4";
	std::wstring wsMP4Filename = swsMP4Filename.str();


	// Run on trial, no licence.
	wchar_t wsLicence[] = L"";
	
	// Create an instance of TFBRecorder. FBRecorder.dll needs to be in the same folder
	pResult = std::shared_ptr<FBRecorder::TFBRecorder>(new FBRecorder::TFBRecorder(L"FBRecorder.dll", wsLicence, wsLogFolder, 10, FBRecorder::TLogKind::Verbose));

	if (!pResult->CanUse())	{
		printf("Error: unable to use a recorder wrapper.\n");
		goto end_of_func;
	}

	// We record just the screen, no webcam or audio. No hardware accelerated recording.
	if (!pResult->ActivateProfile(FBRecorder::TFBRecorderProfile::SAFE_SCREEN))	{
		printf("Error: ActivateProfile error.\n");
		goto end_of_func;
	}

	pResult->ResetGlobalErrors();

	// Get it ready to record to the new MP4 file
	if (!pResult->InitRecorder(wsMP4Filename))	{
		printf("Error: unable to init recorder.\n");
		PrintGlobalErrors(pResult.get());
		goto end_of_func;
	}

	// Return the new recorder instance and the new filename back to ContinuousRecording()
end_of_func:
	return std::tuple<std::shared_ptr<FBRecorder::TFBRecorder>, std::wstring>(pResult, wsMP4Filename);
}

//
// ContinuousRecording
// Record the screen continuously into a number of MP4 files (i32NumberOfSegments parameter) of a given duration (i32SegmentLengthSecs).
// When the user hits the Esc key, combine all the MP4 files into one.
// This enables functionality to record forever and save the last N minutes.
// 
void ContinuousRecording(int32_t i32SegmentLengthSecs, int32_t i32NumberOfSegments)
{
	printf("Recording parameters: segment duration %d secs, %d segments\n", i32SegmentLengthSecs, i32NumberOfSegments);
	std::wstring wsMP4Filename;
	std::vector<std::wstring> vecMP4Filenames;
	std::shared_ptr<FBRecorder::TFBRecorder> pFBRecorder;
	std::shared_ptr<FBRecorder::TFBRecorder> pNextFBRecorder;

	// Create a new instance of TFBRecorder. It will record into the file name returned in wsMP4Filename
	std::tie(pFBRecorder, wsMP4Filename) = CreateRecorder();
	if (!pFBRecorder) {
		printf("Error: unable to use a recorder.\n");
		goto end_of_func;
	}

	PrintGlobalErrors(pFBRecorder.get());
	pFBRecorder->ResetGlobalErrors();

	// Start recording
	if (!pFBRecorder->StartRecording())	{
		printf("Error: unable to start the current recording.\n");
		PrintGlobalErrors(pFBRecorder.get());
		goto end_of_func;
	}

	// Save the MP4 file name for later, when we combine 
	wprintf(L"New MP4 file name is '%s'\n", wsMP4Filename.c_str());
	vecMP4Filenames.push_back(wsMP4Filename);
	PrintGlobalErrors(pFBRecorder.get());
	printf("Recording multiple MP4 files... To stop the recording press Esc...\n");
	DWORD dwStartTime = GetTickCount();

	while (true)
	{
		//
		// Create the next instance of TFBRecorder, if we previously completed a recording file.
		//
		// We use two instances of TFBRecorder for performance reasons - it enables us to pause one recording and immediately start the next, so
		// we get very little missing time between MP4s. If we were to use only one instance, it would take some time to save the current MP4 before 
		// starting the next, creating small gaps in the final recording.
		//
		if (!pNextFBRecorder)
			std::tie(pNextFBRecorder, wsMP4Filename) = CreateRecorder();

		if (!pNextFBRecorder) {
			printf("Error: unable to use a second recorder.\n");
			goto end_of_func;
		}

		// If we've been recording for more than X seconds, stop and save. 
		if (GetTickCount() - dwStartTime >= (DWORD)i32SegmentLengthSecs * 1000)	{
			printf("Trying to stop an active recording and to start a new one..\n");

			// Pause the current recording.
			if (!pFBRecorder->Pause(true)) {
				printf("Error: unable to pause an active recorder.\n");
				goto end_of_func;
			}

			// Start the next recording.
			if (!pNextFBRecorder->StartRecording()) {
				printf("Error: unable to start the next recording.\n");
				PrintGlobalErrors(pFBRecorder.get());
				goto end_of_func;
			}

			// Stop and save the current/old recording.
			dwStartTime = GetTickCount();
			pFBRecorder->StopRecording();
			pFBRecorder = pNextFBRecorder;

			// get ready to make the next recorder instance.
			pNextFBRecorder = nullptr;

			// Save the filename for later, when we combine files, or delete old ones.
			wprintf(L"New MP4 file name is '%s'\n", wsMP4Filename.c_str());
			vecMP4Filenames.push_back(wsMP4Filename);

			// Keep only the last i32NumberOfSegments files.
			if (vecMP4Filenames.size() > i32NumberOfSegments) {
				DeleteFile(vecMP4Filenames[0].c_str());
				vecMP4Filenames.erase(vecMP4Filenames.begin());
			}
		}

		// When the user hits the Esc key, exit.
		if ((GetAsyncKeyState(0x1B) & 0x8000) > 0 && GetConsoleWindow() == GetForegroundWindow()) {
			printf("Exiting...\n");
			break;
		}

		Sleep(10);
	}

// Combine the MP4 files we've recorded (and not deleted) into one.
end_of_func:

	pFBRecorder = nullptr;
	pNextFBRecorder = nullptr;

	if (vecMP4Filenames.size() > 1)	{

		// The files will be merged to test.mp4
		std::experimental::filesystem::path appPath(__argv[0]);
		std::wstring wsFinalFilename = (std::wstring)appPath.parent_path() + L"\\test.mp4";

		wprintf(L"%d MP4 files have been create -> merge them '%s'.\n", (int32_t)vecMP4Filenames.size(), wsFinalFilename.c_str());
		json::JSON jsonMP4Files;

		// Check that the MP4 files we've created still exist and are larger than a minimum size.
		for (size_t i = 0; i < vecMP4Filenames.size(); i++)
		{
			if (std::experimental::filesystem::v1::exists(vecMP4Filenames[i])) {
				//check file size
				if (std::experimental::filesystem::v1::file_size(vecMP4Filenames[i]) > 1024)
					jsonMP4Files[(int32_t)i] = json::wstring_utf8(vecMP4Filenames[i]);
				else
					wprintf(L"File '%s' is too small.\n", vecMP4Filenames[i].c_str());
			}
		}

		// Go through the checked files and merge them using the TFBRecorder::MergeMP4Files method.
		if (jsonMP4Files.size() > 1) {
			std::string sJSON = jsonMP4Files.dump();
			bool bResult = FBRecorder::TFBRecorder::MergeMP4Files(sJSON.c_str(), wsFinalFilename.c_str());
			if (!bResult)
				printf("MergeMP4Files error.\n");
			else
				printf("MP4 files have been merged.\n");
		}
		else
			printf("No need to merge files.");
	}
	else
		printf("No need to merge files.");
}


//
// CreateMP4Clip
// The function demonstrates the clip creation functionality.
//
void CreateMP4Clip()
{
	std::wstring wsSourceMP4File = L"C:\\dev\\FBGames\\FBv6\\FinalDebug\\test.mp4";
	std::wstring wsMP4ClipFile = L"C:\\dev\\FBGames\\FBv6\\FinalDebug\\test_clip.mp4";
	std::experimental::filesystem::path appPath(__argv[0]);
	std::wstring wsLogFolder = (std::wstring)appPath.parent_path() + L"\\ContinuousRecording";

	//create log folder if doesn't exist
	if(!std::experimental::filesystem::v1::exists(wsLogFolder))
		CreateDirectory(wsLogFolder.c_str(), nullptr);

	// Run on trial, no licence.
	wchar_t wsLicence[] = L"";

	// Create an instance of TFBRecorder. FBRecorder.dll needs to be in the same folder
	std::shared_ptr<FBRecorder::TFBRecorder> pResult = std::shared_ptr<FBRecorder::TFBRecorder>(new FBRecorder::TFBRecorder(L"FBRecorder.dll", wsLicence, wsLogFolder, 10, FBRecorder::TLogKind::Verbose));

	bool bResult = pResult->CreateMP4Clip(wsSourceMP4File.c_str(), 20 * 1000, 20 * 1000, wsMP4ClipFile.c_str());
	if (!bResult)
		printf("Unable to create a clip.");
}

//
// The main console function. 
// The user enters values for the duration of the 'segment' MP4 files and the number to keep. 
// These two parameters define the maximum duration of the final MP4 file created when the segments are combined.
//
int main()
{
	//a small test to extract the part of the MP4 movie
	//CreateMP4Clip(); return 0;

	bool bUseDefaultValues = false;
	int32_t i32SegmentLengthSecs = 10;
	int32_t i32NumberOfSegments = 30;

	// Give the user the option of using default values.
	// The defaults above record a maximum of 30 MP4 files of 10 seconds length. The final, combined, MP4 file will have a maximum duration of 5 minutes.
	//
	while (true)
	{
		printf("Use default values (many segments, each 10 secs)? (Y/N [Y is a default value])");
		int32_t i32Key = _getch();

		if (i32Key == 13)
			i32Key = 'y';

		if (i32Key == 'y' || i32Key == 'Y') {
			bUseDefaultValues = true;
			break;
		}

		if (i32Key == 'n' || i32Key == 'N') {
			bUseDefaultValues = false;
			break;
		}

		printf("\nInvalid input.\n");
	}

	printf("\n");

	if (!bUseDefaultValues) {
		while (true)
		{
			printf("Segment length in seconds (10 secs <= X <= 600 secs): ");

			if (scanf_s("%d", &i32SegmentLengthSecs) == 1) {
				if (i32SegmentLengthSecs >= 10 && i32SegmentLengthSecs <= 600)
					break;
			}

			fseek(stdin, 0, SEEK_END);
			printf("\nInvalid input.\n");
		}

		while (true)
		{
			printf("The number of segments (2 <= X <= 30): ");

			if (scanf_s("%d", &i32NumberOfSegments) == 1) {
				if (i32NumberOfSegments >= 2 && i32NumberOfSegments <= 30)
					break;
			}

			fseek(stdin, 0, SEEK_END);
			printf("\nInvalid input.\n");
		}
	}


	// Start recording
	ContinuousRecording(i32SegmentLengthSecs, i32NumberOfSegments);

	return 0;
}

