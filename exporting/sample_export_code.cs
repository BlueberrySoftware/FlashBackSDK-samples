
// 
// Function uses FBRecorder::ConvertMP4File to convert an MP4 to MKV format
// Parameters: 
//   pFBRecorder - pointer to an FBRecorder object
//   bVideo - bool - set to true to convert video
//   bAudio - bool - set to true to convert audio
// 
static bool ConvertExportMKV(TFBRecorder pFBRecorder, bool bVideo, bool bAudio)
{
    // Get a settings object to get the index of video and audio encoders
    string sProfile = pFBRecorder.GetConfigJSONForProfile(FBRecorder.TFBRecorder.TFBRecorderProfiles.SAFE_SCREEN);
    dynamic jsonProfile = JsonConvert.DeserializeObject(sProfile);

    // We will pass the convertSettings object to ConvertMP4File()
    JObject convertSettings = new JObject();
    convertSettings["ExportType"] = (int)FBRecorder.TFBRecorder.TExportType.mkv;

    if (bVideo) {
        // This video settings object will be passed to ConvertMP4File in convertSettings['VideoEncoderParameters']
        JObject Params = new JObject();

        int i32VideoEncoderType = -1;//Unknown = 0, MSMF = 1, NativeIntel = 2, NativeNvidia = 3, NativeAMD = 4
        string sVideoEncoderName = "";

        // We are going to iterate over the available video encoders in the settings object
        dynamic jsonVideoEncoders = jsonProfile["AvailableVideoEncoders"];
        int i32VideoEncodersCount = jsonVideoEncoders.Count;

        // iterate over video encoders and use the Type and Name from the first available one
        for (int i = 0; i < i32VideoEncodersCount; i++) {
            dynamic jsonVideoEncoder = jsonVideoEncoders[i];
            i32VideoEncoderType = jsonVideoEncoder["Type"];
            sVideoEncoderName = jsonVideoEncoder["Name"];

            if (pFBRecorder.CheckVideoEncoderAvailability(i32VideoEncoderType, sVideoEncoderName))
                break; 

            i32VideoEncoderType = -1;
            sVideoEncoderName = "";
        }

        // Pass the video encoder type and name. This is required, if converting video.
        Params["VideoEncoder"] = new JObject();
        Params["VideoEncoder"]["Type"] = i32VideoEncoderType;
        Params["VideoEncoder"]["Name"] = sVideoEncoderName;

        // Set the other vidoe settings. This is not required - they will default to something sensible, and FPS defaults to 30fps if they are not set here.
        Params["EnableCabac"] = i32VideoEncoderType != 1; //cabac is not available for MSMF software video encoder
        Params["H264Profile"] = 77;//Base = 66, Main = 77, High = 100
        Params["BFramesNum"] = 2;
        Params["GOPSize"] = 30;
        Params["QualityLevel"] = 80;
        Params["RefFramesNum"] = 2;
        Params["FPS"] = new JObject(); // Set 15fps
        Params["FPS"]["Num"] = 15;
        Params["FPS"]["Den"] = 1;
        Params["CompressionMode"] = 1;//bitrate = 0, quality = 1

        // Set the height and width of the converted video. These both default to -1, which keeps the width/height ofthe original video
        Params["Width"] = 640;
        Params["Height"] = 480;

        convertSettings["VideoEncoderParameters"] = Params;
    }

    if (bAudio) {
        // This audio settings object will be passed to ConvertMP4File in convertSettings['AudioEncoderParameters']
        JObject Params = new JObject();
        dynamic AudioEncoders = jsonProfile["AudioSettings"]["AvailableAACEncoders"];

        if (AudioEncoders.Count == 0) {
            Console.WriteLine("Audio encoders are not found");
            return false;
        }

        //use the first available AAC encoder
        Params["Type"] = AudioEncoders[0]["Type"];
        Params["Name"] = AudioEncoders[0]["Name"];

        convertSettings["AudioEncoderParameters"] = Params;
    }

    string sSettings = JsonConvert.SerializeObject(convertSettings);

    bool bResult = FBRecorder.TFBRecorder.ConvertMP4File("D:\\original.MP4", "D:\\exported.mkv", -1, -1, sSettings, new FBRecorder.TFBRecorder.Progressor(Progressor));

    if (!bResult) {
        Console.WriteLine("ConvertMP4File failed\n");
    }
    else {
        Console.WriteLine("ConvertMP4File succeeded\n");
    }

    return bResult;
}

 

// 
// Progressor function used by code in this file. Return false to stop conversion.
//
static bool Progressor(long i64Current, long i64Total)
{
    Console.WriteLine(String.Format("{0} of {1}", i64Current, i64Total));
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
static bool ConvertExportAVI(TFBRecorder pFBRecorder, bool bVideo, bool bAudio)
{
    // Get an FBRecorder setting object to get the index of a video encoder.
    string sProfile = pFBRecorder.GetConfigJSONForProfile(FBRecorder.TFBRecorder.TFBRecorderProfiles.SAFE_SCREEN);
    dynamic jsonProfile = JsonConvert.DeserializeObject(sProfile);

    // We will pass this settings object to ConvertMP4File
    JObject jsonSettings = new JObject();
    jsonSettings["ExportType"] = (int)FBRecorder.TFBRecorder.TExportType.avi;

    if (bVideo)
    {
        JObject Params = new JObject();
            
        int i32VideoEncoderType = -1;//Unknown = 0, MSMF = 1, NativeIntel = 2, NativeNvidia = 3, NativeAMD = 4
        string sVideoEncoderName = "";
        dynamic jsonVideoEncoders = jsonProfile["AvailableVideoEncoders"];
        int i32VideoEncodersCount = jsonVideoEncoders.Count;

        // Get the index of the first available video encoder
        for (int i = 0; i < i32VideoEncodersCount; i++)
        {
            dynamic jsonVideoEncoder = jsonVideoEncoders[i];
            i32VideoEncoderType = jsonVideoEncoder["Type"];
            sVideoEncoderName = jsonVideoEncoder["Name"];

            if (pFBRecorder.CheckVideoEncoderAvailability(i32VideoEncoderType, sVideoEncoderName))
                break;

            i32VideoEncoderType = -1;
            sVideoEncoderName = "";
        }

        // Set some video parameters. We will pass this to ConvertMP4File in jsonSettings["VideoEncoderParameters"]
        Params["VideoEncoder"] = new JObject();
        Params["VideoEncoder"]["Type"] = i32VideoEncoderType;
        Params["VideoEncoder"]["Name"] = sVideoEncoderName;

        Params["EnableCabac"] = i32VideoEncoderType != 1;//cabac is not available for MSMF software video encoder
        Params["H264Profile"] = 77;//Base = 66, Main = 77, High = 100
        Params["BFramesNum"] = 2;
        Params["GOPSize"] = 30;
        Params["QualityLevel"] = 80;
        Params["RefFramesNum"] = 2;
        Params["FPS"] = new JObject();
        Params["FPS"]["Num"] = 15;
        Params["FPS"]["Den"] = 1;
        Params["CompressionMode"] = 1;//bitrate = 0, quality = 1

        Params["Width"] = 640;
        Params["Height"] = 480;
        jsonSettings["VideoEncoderParameters"] = Params;
    }

    if (bAudio)
    {
        // This is different from the MKV export code, which uses AAC audio. For AVI export, a default MP3 encoder is used, so we just pass an empty json object.
        JObject Params = new JObject();
        jsonSettings["AudioEncoderParameters"] = Params;
    }

    string sSettings = JsonConvert.SerializeObject(jsonSettings);
    bool bResult = FBRecorder.TFBRecorder.ConvertMP4File("D:\\original.mp4", "D:\\exported.avi", -1, -1, sSettings, new FBRecorder.TFBRecorder.Progressor(Progressor));

    if (!bResult)
    {
        Console.WriteLine("ConvertMP4File failed\n");
    }
    else
    {
        Console.WriteLine("ConvertMP4File succeeded\n");
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
static bool ConvertExportWMV(bool bVideo, bool bAudio)
{
    JObject jsonSettings = new JObject();

    jsonSettings["ExportType"] = (int)FBRecorder.TFBRecorder.TExportType.wmv;

    // WMV conversion uses the default Windows WMV encoder, so we don't need to specify an encoder, only - optionally - the dimensions, fps, quality
    if (bVideo)
    {
        JObject Params = new JObject();

        // Width and Height default to -1, which keeps the dimensions of the original video
        Params["Width"] = 640;
        Params["Height"] = 480;

        // FPS defaults to 30
        Params["FPS"] = new JObject();
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
        JObject Params = new JObject();
        jsonSettings["AudioEncoderParameters"] = Params;
    }

    string sSettings = JsonConvert.SerializeObject(jsonSettings);
    bool bResult = FBRecorder.TFBRecorder.ConvertMP4File("D:\\original.mp4", "D:\\exported.wmv", -1, -1, sSettings, new FBRecorder.TFBRecorder.Progressor(Progressor));

    if (!bResult)
    {
        Console.WriteLine("ConvertMP4File failed\n");
    }
    else
    {
        Console.WriteLine("ConvertMP4File succeeded\n");
    }

    return bResult;
}

 

// 
// Function uses FBRecorder::ConvertMP4File to convert an MP4 to WMV format
// There is no need to specify a video encoder 
//
static bool ConvertExportGIF()
{
    JObject jsonSettings = new JObject();
    jsonSettings["ExportType"] = (int)FBRecorder.TFBRecorder.TExportType.gif;

    // No need to specify an H264 encoder for GIF conversion. We just set the dimensions and FPS. 
    JObject Params = new JObject();

    Params["Width"] = 640;
    Params["Height"] = 480;

    Params["FPS"] = new JObject();
    Params["FPS"]["Num"] = 2;
    Params["FPS"]["Den"] = 1;

    jsonSettings["VideoEncoderParameters"] = Params;


    string sSettings = JsonConvert.SerializeObject(jsonSettings);
    bool bResult = FBRecorder.TFBRecorder.ConvertMP4File("D:\\original.mp4", "D:\\exported.gif", -1, -1, sSettings, new FBRecorder.TFBRecorder.Progressor(Progressor));

    if (!bResult)
    {
        Console.WriteLine("ConvertMP4File failed\n");
    }
    else
    {
        Console.WriteLine("ConvertMP4File succeeded\n");
    }

    return bResult;
}

 
// 
// Function - converts the audio in the MP4 file to an AAC file
//
static bool ConvertExportAAC(TFBRecorder pFBRecorder)
{
    // Use the settings object to get the details of an AAC encoder. 
    string sProfile = pFBRecorder.GetConfigJSONForProfile(FBRecorder.TFBRecorder.TFBRecorderProfiles.SAFE_SCREEN);
    dynamic jsonProfile = JsonConvert.DeserializeObject(sProfile);

    JObject jsonSettings = new JObject();
    jsonSettings["ExportType"] = (int)FBRecorder.TFBRecorder.TExportType.aac;

    JObject Params = new JObject();

    // Get an AAC encoder from the settings object
    dynamic AudioEncoders = jsonProfile["AudioSettings"]["AvailableAACEncoders"];

    if (AudioEncoders.Count == 0)
    {
        Console.WriteLine("Audio encoders are not found");
        return false;
    }

    // Use the first available AAC encoder
    Params["Type"] = AudioEncoders[0]["Type"];
    Params["Name"] = AudioEncoders[0]["Name"];

    jsonSettings["AudioEncoderParameters"] = Params;

    string sSettings = JsonConvert.SerializeObject(jsonSettings);
    bool bResult = FBRecorder.TFBRecorder.ConvertMP4File("D:\\original.mp4", "D:\\exported.aac", -1, -1, sSettings, new FBRecorder.TFBRecorder.Progressor(Progressor));

    if (!bResult)
    {
        Console.WriteLine("ConvertMP4File failed\n");
    }
    else
    {
        Console.WriteLine("ConvertMP4File succeeded\n");
    }

    return bResult;
}

 

// 
// Function - converts the audio in the MP4 file to an MP3 file
//
static bool ConvertExportMP3()
{
    JObject jsonSettings = new JObject();

    jsonSettings["ExportType"] = (int)FBRecorder.TFBRecorder.TExportType.mp3;

    // ConvertMP4File uses a default Windows MP3 encoder, so no details of an encoder need be passed over, just an empty object
    jsonSettings["AudioEncoderParameters"] = new JObject();

    string sSettings = JsonConvert.SerializeObject(jsonSettings);
    bool bResult = FBRecorder.TFBRecorder.ConvertMP4File("D:\\original.mp4", "D:\\exported.mp3", -1, -1, sSettings, new FBRecorder.TFBRecorder.Progressor(Progressor));

    if (!bResult)
    {
        Console.WriteLine("ConvertMP4File failed\n");
    }
    else
    {
        Console.WriteLine("ConvertMP4File succeeded\n");
    }

    return bResult;
}



 

 
