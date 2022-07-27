

// 
// Function uses FBRecorder::ConvertMP4File to convert an MP4 to MKV format
// Parameters: 
//   pFBRecorder - pointer to an FBRecorder object
//   bVideo - bool - set to true to convert video
//   bAudio - bool - set to true to convert audio
// 
bool ConvertExportMKV(FBRecorder::TFBRecorder* pFBRecorder, bool bExportVideo, bool bExportAudio)
{
    // Get a settings object to get the index of video and audio encoders
    std::string sProfile = pFBRecorder->GetConfigJSONForProfile(FBRecorder::TFBRecorderProfile::SAFE_SCREEN);
    json::JSON jsonProfile = json::JSON::Load(sProfile);

    // We will pass the jsonSettings object to ConvertMP4File()
    json::JSON jsonSettings;
    jsonSettings["export_type"] = (int32_t)FBRecorder::TExportType::mkv;

    if(bVideo)
    {  
        // This video settings object will be passed to ConvertMP4File in jsonSettings['VideoEncoderParameters']
        auto Params = json::JSON();
        int32_t i32VideoEncoderType = -1;//Unknown = 0, MSMF = 1, NativeIntel = 2, NativeNvidia = 3, NativeAMD = 4
        std::string sVideoEncoderName = "";

        // We are going to iterate over the available video encoders in the settings object
        auto& jsonVideoEncoders = jsonProfile["AvailableVideoEncoders"];
        int32_t i32VideoEncodersCount = jsonVideoEncoders.size();

        // iterate over video encoders and use the Type and Name from the first available one
        for (auto i = 0; i < i32VideoEncodersCount; i++)
        {
            auto& jsonVideoEncoder = jsonVideoEncoders[i];
            bool bOk = false;
            i32VideoEncoderType = jsonVideoEncoder["Type"].ToInt(bOk);

            if (!bOk)
                continue;

            sVideoEncoderName = jsonVideoEncoder["Name"].ToString(bOk);

            if (!bOk)
               continue;

            std::wstring wsName = json::utf8_wstring(sVideoEncoderName);

            if (pFBRecorder->CheckVideoEncoderAvailability(i32VideoEncoderType, wsName))
                break;

            i32VideoEncoderType = -1;
            sVideoEncoderName = "";
        }

        // Pass the video encoder type and name. This is required, if converting video.
        Params["VideoEncoder"]["Type"] = i32VideoEncoderType;
        Params["VideoEncoder"]["Name"] = sVideoEncoderName;

        // Set the other vidoe settings. This is not required - they will default to something sensible, and FPS defaults to 30fps if they are not set here.
        Params["EnableCabac"] = i32VideoEncoderType != 1;//cabac is not available for MSMF software video encoder
        Params["H264Profile"] = 77;//Base = 66, Main = 77, High = 100
        Params["BFramesNum"] = 2;
        Params["GOPSize"] = 30;
        Params["QualityLevel"] = 80;
        Params["RefFramesNum"] = 2;
        Params["FPS"]["Num"] = 15;
        Params["FPS"]["Den"] = 1;
        Params["CompressionMode"] = 1;//bitrate = 0, quality = 1

        // Set the height and width of the converted video. These both default to -1, which keeps the width/height ofthe original video
        Params["Width"] = 640;
        Params["Height"] = 480;

        jsonSettings["VideoEncoderParameters"] = Params;
    }

    if (bAudio)
    {
        // This audio settings object will be passed to ConvertMP4File in convertSettings['AudioEncoderParameters']
        auto Params = json::JSON();
        auto& AudioEncoders = jsonProfile["AudioSettings"]["AvailableAACEncoders"];

        if (AudioEncoders.size() == 0)
        {
            printf("Audio encoders are not found");
            return false;
        }

        //use the first available AAC encoder
        Params["Type"] = AudioEncoders[0]["Type"].ToInt();
        Params["Name"] = AudioEncoders[0]["Name"].ToString();

        jsonSettings["AudioEncoderParameters"] = Params;
    }

    std::string sSettings = jsonSettings.dump();
    bool bResult = FBRecorder::TFBRecorder::ConvertMP4File(L"D:\\original.mp4", L"D:\\exported.mkv" , -1, -1, sSettings.c_str(), &Progressor);

    if (!bResult)
    {
        printf("ConvertMP4File failed\n");
    }
    else
    {
        printf("ConvertMP4File succeeded\n");
    }

    return bResult;
}


//
// Progressor function used by sample code. Return false to stop conversion.
//
bool Progressor(int64_t i64Current, int64_t i64Total)
{
    printf("%I64d of %I64d\n", i64Current, i64Total);
    return true;
}
 


// 
// Function uses FBRecorder::ConvertMP4File to convert an MP4 to AVI format
// Parameters: 
//   pFBRecorder - pointer to an FBRecorder object
//   bVideo - bool - set to true to convert video
//   bAudio - bool - set to true to convert audio
// 
// Take a look at ConvertExportMKV above for more comments
//
bool ConvertExportAVI(FBRecorder::TFBRecorder* pFBRecorder, bool bVideo, bool bAudio)
{
    // Get an FBRecorder setting object to get the index of a video encoder.
    std::string sProfile = pFBRecorder->GetConfigJSONForProfile(FBRecorder::TFBRecorderProfile::SAFE_SCREEN);
    json::JSON jsonProfile = json::JSON::Load(sProfile);

    // We will pass this settings object to ConvertMP4File
    json::JSON jsonSettings;
    jsonSettings["ExportType"] = (int32_t)FBRecorder::TExportType::avi;

    if (bVideo)
    {
        auto Params = json::JSON();

        //find any appropriate video encoder
        int32_t i32VideoEncoderType = -1;//Unknown = 0, MSMF = 1, NativeIntel = 2, NativeNvidia = 3, NativeAMD = 4
        std::string sVideoEncoderName = "";

        auto& jsonVideoEncoders = jsonProfile["AvailableVideoEncoders"];
        int32_t i32VideoEncodersCount = jsonVideoEncoders.size();

        // Get the index of the first available video encoder
        for (auto i = 0; i < i32VideoEncodersCount; i++)
        {
            auto& jsonVideoEncoder = jsonVideoEncoders[i];
            bool bOk = false;

            i32VideoEncoderType = jsonVideoEncoder["Type"].ToInt(bOk);

            if (!bOk)
                continue;

            sVideoEncoderName = jsonVideoEncoder["Name"].ToString(bOk);

            if (!bOk)
                continue;

            std::wstring wsName = json::utf8_wstring(sVideoEncoderName);

            if (pFBRecorder->CheckVideoEncoderAvailability(i32VideoEncoderType, wsName))
                break;

            i32VideoEncoderType = -1;
            sVideoEncoderName = "";
        }

        // Set some video parameters. We will pass this to ConvertMP4File in jsonSettings["VideoEncoderParameters"]
        Params["VideoEncoder"]["Type"] = i32VideoEncoderType;
        Params["VideoEncoder"]["Name"] = sVideoEncoderName;

        Params["EnableCabac"] = i32VideoEncoderType != 1;//cabac is not available for MSMF software video encoder
        Params["H264Profile"] = 77;//Base = 66, Main = 77, High = 100
        Params["BFramesNum"] = 2;
        Params["GOPSize"] = 30;
        Params["QualityLevel"] = 80;
        Params["RefFramesNum"] = 2;
        Params["FPS"]["Num"] = 15;
        Params["FPS"]["Den"] = 1;
        Params["CompressionMode"] = 1;//bitrate = 0, quality = 1

        Params["Width"] = 640;
        Params["Height"] = 480;

        jsonSettings["VideoEncoderParameters"] = Params;
    }

    // This is different from the MKV export code, which uses AAC audio. For AVI export, a default MP3 encoder is used, so we just pass an empty json object.
    if (bAudio)
    {
        auto Params = json::JSON();
        jsonSettings["AudioEncoderParameters"] = Params;
    }

    std::string sSettings = jsonSettings.dump();
    bool bResult = FBRecorder::TFBRecorder::ConvertMP4File(L"D:\\buzova.MP4", L"D:\\buzova.avi", -1, -1, sSettings.c_str(), &Progressor);

    if (!bResult)
    {
        printf("ConvertMP4File failed\n");
    }
    else
    {
        printf("ConvertMP4File succeeded\n");
    }

    return bResult;
}



// 
// Function uses FBRecorder::ConvertMP4File to convert an MP4 to WMV format
// Parameters: 
//   bVideo - bool - set to true to convert video
//   bAudio - bool - set to true to convert audio
//
// The default Windows encoders for WMV video and WMA audio is used, so unlike MKV and AVI conversion, there is no need to use the FBRecorder settings object to get encoder details.
// 
bool ConvertExportWMV(FBRecorder::TFBRecorder* pFBRecorder, bool bVideo, bool bAudio)
{
    std::string sProfile = pFBRecorder->GetConfigJSONForProfile(FBRecorder::TFBRecorderProfile::SAFE_SCREEN);
    json::JSON jsonProfile = json::JSON::Load(sProfile);

    json::JSON jsonSettings;
    jsonSettings["ExportType"] = (int32_t)FBRecorder::TExportType::wmv;

    // WMV conversion uses the default Windows WMV encoder, so we don't need to pass encoder Type and Name, only - optionally - the dimensions, fps, quality
    if (bVideo)
    {
        auto Params = json::JSON();
        Params["VideoEncoder"]["Type"] = -1;
        Params["VideoEncoder"]["Name"] = "";

        // Width and Height default to -1, which keeps the dimensions of the original video
        Params["Width"] = 640;
        Params["Height"] = 480;

        // FPS defaults to 30
        Params["FPS"]["Num"] = 15;
        Params["FPS"]["Den"] = 1;
        Params["CompressionMode"] = 1;//bitrate = 0, quality = 1
        Params["BitrateKbps"] = 1000;

        //or

        Params["QualityLevel"] = 50;//[1..100]

        jsonSettings["VideoEncoderParameters"] = Params;
    }

    // WMV conversion uses a default Windows WMA encoder, so we only need pass an empty object - no need to specify an AAC audio encoder as in MKV conversion. 
    if (bAudio)
    {
        auto Params = json::JSON();
        jsonSettings["AudioEncoderParameters"] = Params;
    }

    std::string sSettings = jsonSettings.dump();
    bool bResult = FBRecorder::TFBRecorder::ConvertMP4File(L"D:\\original.mp4", L"D:\\exported.wmv", -1, -1, sSettings.c_str(), &Progressor);

    if (!bResult)
    {
        printf("ConvertMP4File failed\n");
    }
    else
    {
        printf("ConvertMP4File succeeded\n");
    }

    return bResult;
}



// 
// Function uses FBRecorder::ConvertMP4File to convert an MP4 to WMV format
//
bool ConvertExportGIF(FBRecorder::TFBRecorder* pFBRecorder)
{
    std::string sProfile = pFBRecorder->GetConfigJSONForProfile(FBRecorder::TFBRecorderProfile::SAFE_SCREEN);
    json::JSON jsonProfile = json::JSON::Load(sProfile);

    // No need to specify an H264 encoder for GIF conversion. We just set the dimensions and FPS. 
    json::JSON jsonSettings;
    jsonSettings["ExportType"] = (int32_t)FBRecorder::TExportType::gif;

    auto Params = json::JSON();
    Params["Width"] = 640;
    Params["Height"] = 480;

    Params["FPS"]["Num"] = 2;
    Params["FPS"]["Den"] = 1;

    jsonSettings["VideoEncoderParameters"] = Params;

    std::string sSettings = jsonSettings.dump();
    bool bResult = FBRecorder::TFBRecorder::ConvertMP4File(L"D:\\original.mp4", L"D:\\exported.gif", -1, -1, sSettings.c_str(), &Progressor);

    if (!bResult)
    {
        printf("ConvertMP4File failed\n");
    }
    else
    {
        printf("ConvertMP4File succeeded\n");
    }

    return bResult;
}



 

// 
// Function - converts the audio in the MP4 file to an AAC file
//
bool ConvertExportAAC(FBRecorder::TFBRecorder* pFBRecorder)
{
    // Use the settings object to get the details of an AAC encoder. 
    std::string sProfile = pFBRecorder->GetConfigJSONForProfile(FBRecorder::TFBRecorderProfile::SAFE_SCREEN);
    json::JSON jsonProfile = json::JSON::Load(sProfile);
    
    json::JSON jsonSettings;
    jsonSettings["export_type"] = (int32_t)FBRecorder::TExportType::aac;

    auto Params = json::JSON();

    // Get an AAC encoder from the settings object
    auto& AudioEncoders = jsonProfile["AudioSettings"]["AvailableAACEncoders"];

    if (AudioEncoders.size() == 0)
    {
        printf("Audio encoders are not found");
        return false;
    }

    // Use the first available AAC encoder
    Params["Type"] = AudioEncoders[0]["Type"].ToInt();
    Params["Name"] = AudioEncoders[0]["Name"].ToString();

    jsonSettings["AudioEncoderParameters"] = Params;

    std::string sSettings = jsonSettings.dump();
    bool bResult = FBRecorder::TFBRecorder::ConvertMP4File(L"D:\\buzova.MP4", L"D:\\buzova.aac", -1, -1, sSettings.c_str(), &Progressor);

    if (!bResult)
    {
        printf("ConvertMP4File failed\n");
    }
    else
    {
        printf("ConvertMP4File succeeded\n");
    }

    return bResult;
}



// 
// Function - converts the audio in the MP4 file to an MP3 file
//
bool ConvertExportMP3()
{
    json::JSON jsonSettings;

    jsonSettings["ExportType"] = (int32_t)FBRecorder::TExportType::mp3;

    // ConvertMP4File uses a default Windows MP3 encoder, so no details of an encoder need be passed over, just an empty object
    jsonSettings["AudioEncoderParameters"] = json::JSON();

    std::string sSettings = jsonSettings.dump();
    bool bResult = FBRecorder::TFBRecorder::ConvertMP4File(L"D:\\original.mp4", L"D:\\exported.mp3", -1, -1, sSettings.c_str(), &Progressor);

    if (!bResult)
    {
        printf("ConvertMP4File failed\n");
    }
    else
    {
        printf("ConvertMP4File succeeded\n");
    }

    return bResult;
}
