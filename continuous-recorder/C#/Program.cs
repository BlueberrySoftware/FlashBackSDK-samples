using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FBRecorder;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ContinuousRecording
{
    class Program
    {


        static void PrintGlobalErrors(TFBRecorder pFBRecorder)
        {
            int i32ErrorIndex = 0;
            while (true)
            {
                TFBRecorder.TFBGlobalErrors Error = pFBRecorder.GetGlobalError(i32ErrorIndex);
                if (Error == TFBRecorder.TFBGlobalErrors.Unknown)
                    break;
                Console.WriteLine("Global error: {0}\n", Error);
                i32ErrorIndex++;
            }
        }

        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(int vKey);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        static int i32FileIndex = 0;
        //
        // CreateRecorder()
        // Create a new instance of TFBRecorder and get it ready to record to a new MP4 file. Return the instance pointer and filename back to ContinuousRecording()
        //
        static Tuple<FBRecorder.TFBRecorder, string> CreateRecorder()
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string wsLogFolder = Path.Combine(appPath, "MultipleRecorders");
            string sMP4Filename = "";
            FBRecorder.TFBRecorder pResult = null;


            //create log folder if doesn't exist
            if (!Directory.Exists(wsLogFolder))
                Directory.CreateDirectory(wsLogFolder);

            // Construct the filename for the new MP4, using i32FileIndex. 
            sMP4Filename = Path.Combine(appPath, "test_" + i32FileIndex + ".mp4");
            i32FileIndex++;

            // Run on trial, no licence.
            string wsLicence = "";

            // Create an instance of TFBRecorder. FBRecorder.dll needs to be in the same folder
            pResult = new FBRecorder.TFBRecorder(wsLicence, wsLogFolder, 10, TFBRecorder.TLogKind.Verbose);

            if (!pResult.CanUse())
            {
                Console.WriteLine("Error: unable to use a recorder wrapper.\n");
                goto end_of_func;
            }

            // We record just the screen, no webcam or audio. No hardware accelerated recording.
            if (!pResult.ActivateProfile(FBRecorder.TFBRecorder.TFBRecorderProfiles.SAFE_SCREEN))
            {
                Console.WriteLine("Error: ActivateProfile error.\n");
                goto end_of_func;
            }

            pResult.ResetGlobalErrors();

            // Get it ready to record to the new MP4 file
            if (!pResult.InitRecorder(sMP4Filename))
            {
                Console.WriteLine("Error: unable to init recorder.\n");
                PrintGlobalErrors(pResult);
                goto end_of_func;
            }

            // Return the new recorder instance and the new filename back to ContinuousRecording()
        end_of_func:
            return new Tuple<FBRecorder.TFBRecorder, string>(pResult, sMP4Filename);
        }


        //
        // ContinuousRecording
        // Record the screen continuously into a number of MP4 files (i32NumberOfSegments parameter) of a given duration (i32SegmentLengthSecs).
        // When the user hits the Esc key, combine all the MP4 files into one.
        // This enables functionality to record forever and save the last N minutes.
        // 
        static void ContinuousRecording(int i32SegmentLengthSecs, int i32NumberOfSegments)
        {
            Console.WriteLine("Recording parameters: segment duration {0} secs, {1} segments\n", i32SegmentLengthSecs, i32NumberOfSegments);
            string wsMP4Filename = "";
            List<string> vecMP4Filenames = new List<string>();
            FBRecorder.TFBRecorder pFBRecorder = null;
            FBRecorder.TFBRecorder pNextFBRecorder = null;

            // Create a new instance of TFBRecorder. It will record into the file name returned in wsMP4Filename
            var CreateRecorderResult = CreateRecorder();
            pFBRecorder = CreateRecorderResult.Item1;
            wsMP4Filename = CreateRecorderResult.Item2;
            if (pFBRecorder == null)
            {
                Console.WriteLine("Error: unable to use a recorder.\n");
                goto end_of_func;
            }

            PrintGlobalErrors(pFBRecorder);
            pFBRecorder.ResetGlobalErrors();

            // Start recording
            if (!pFBRecorder.StartRecording())
            {
                Console.WriteLine("Error: unable to start the current recording.\n");
                PrintGlobalErrors(pFBRecorder);
                goto end_of_func;
            }

            // Save the MP4 file name for later, when we combine 
            Console.WriteLine("New MP4 file name is '{0}'\n", wsMP4Filename);
            vecMP4Filenames.Add(wsMP4Filename);
            PrintGlobalErrors(pFBRecorder);
            Console.WriteLine("Recording multiple MP4 files... To stop the recording press Esc...\n");
            int dwStartTime = Environment.TickCount;

            while (true)
            {
                //
                // Create the next instance of TFBRecorder, if we previously completed a recording file.
                //
                // We use two instances of TFBRecorder for performance reasons - it enables us to pause one recording and immediately start the next, so
                // we get very little missing time between MP4s. If we were to use only one instance, it would take some time to save the current MP4 before 
                // starting the next, creating small gaps in the final recording.
                //
                if (pNextFBRecorder == null)
                {
                    CreateRecorderResult = CreateRecorder();
                    pNextFBRecorder = CreateRecorderResult.Item1;
                    wsMP4Filename = CreateRecorderResult.Item2;
                }

                if (pNextFBRecorder == null)
                {
                    Console.WriteLine("Error: unable to use a second recorder.\n");
                    goto end_of_func;
                }

                // If we've been recording for more than X seconds, stop and save. 
                if (Environment.TickCount - dwStartTime >= i32SegmentLengthSecs * 1000)
                {
                    Console.WriteLine("Trying to stop an active recording and to start a new one..\n");

                    // Pause the current recording.
                    if (!pFBRecorder.Pause(true))
                    {
                        Console.WriteLine("Error: unable to pause an active recorder.\n");
                        goto end_of_func;
                    }

                    // Start the next recording.
                    if (!pNextFBRecorder.StartRecording())
                    {
                        Console.WriteLine("Error: unable to start the next recording.\n");
                        PrintGlobalErrors(pFBRecorder);
                        goto end_of_func;
                    }

                    // Stop and save the current/old recording.
                    dwStartTime = Environment.TickCount;
                    pFBRecorder.StopRecording();
                    pFBRecorder.Destroy();//must be called
                    pFBRecorder = pNextFBRecorder;

                    // get ready to make the next recorder instance.
                    pNextFBRecorder = null;

                    // Save the filename for later, when we combine files, or delete old ones.
                    Console.WriteLine("New MP4 file name is '{0}'\n", wsMP4Filename);
                    vecMP4Filenames.Add(wsMP4Filename);

                    // Keep only the last i32NumberOfSegments files.
                    if (vecMP4Filenames.Count > i32NumberOfSegments)
                    {
                        File.Delete(vecMP4Filenames[0]);
                        vecMP4Filenames.RemoveAt(0);
                    }
                }

                // When the user hits the Esc key, exit.
                if ((GetAsyncKeyState(0x1B) & 0x8000) > 0 && GetConsoleWindow() == GetForegroundWindow())
                {
                    Console.WriteLine("Exiting...\n");
                    break;
                }

                Thread.Sleep(10);
            }

            // Combine the MP4 files we've recorded (and not deleted) into one.
        end_of_func:

            if (pFBRecorder != null)
                pFBRecorder.Destroy();
            pFBRecorder = null;
            if (pNextFBRecorder != null)
                pNextFBRecorder.Destroy();
            pNextFBRecorder = null;

            if (vecMP4Filenames.Count > 1)
            {

                // The files will be merged to test.mp4
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                string wsFinalFilename = Path.Combine(appPath, "test.mp4");

                Console.WriteLine("{0} MP4 files have been create -> merge them '{1}'.\n", vecMP4Filenames.Count, wsFinalFilename);
                dynamic jsonFilenames = new JArray();
                // Check that the MP4 files we've created still exist and are larger than a minimum size.
                for (int i = 0; i < vecMP4Filenames.Count; i++)
                {
                    if (File.Exists(vecMP4Filenames[i]))
                    {
                        //check file size
                        if (new System.IO.FileInfo(vecMP4Filenames[i]).Length > 1024)
                            jsonFilenames.Add(vecMP4Filenames[i]);
                        else
                            Console.WriteLine("File '{0}' is too small.\n", vecMP4Filenames[i]);
                    }
                }
                // Go through the checked files and merge them using the TFBRecorder::MergeMP4Files method.
                if (jsonFilenames.Count > 0)
                {
                    bool bResult = FBRecorder.TFBRecorder.MergeMP4Files(JsonConvert.SerializeObject(jsonFilenames, Formatting.Indented), wsFinalFilename);
                    if (!bResult)
                        Console.WriteLine("MergeMP4Files error.\n");
                    else
                        Console.WriteLine("MP4 files have been merged.\n");
                }
                else
                    Console.WriteLine("No need to merge files.");
            }
            else
                Console.WriteLine("No need to merge files.");
        }


        //
        // CreateMP4Clip
        // The function demonstrates the clip creation functionality.
        //
        static void CreateMP4Clip()
        {
            string wsSourceMP4File = "C:\\dev\\FBGames\\FBv6\\FinalDebug\\test.mp4";
            string wsMP4ClipFile = "C:\\dev\\FBGames\\FBv6\\FinalDebug\\test_clip.mp4";
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string wsLogFolder = Path.Combine(appPath, "MultipleRecorders");

            //create log folder if doesn't exist
            if (!Directory.Exists(wsLogFolder))
                Directory.CreateDirectory(wsLogFolder);

            // Run on trial, no licence.
            string wsLicence = "";

            // Create an instance of TFBRecorder. FBRecorder.dll needs to be in the same folder
            FBRecorder.TFBRecorder pResult = new FBRecorder.TFBRecorder(wsLicence, wsLogFolder, 10, TFBRecorder.TLogKind.Verbose);

            bool bResult = FBRecorder.TFBRecorder.CreateMP4Clip(wsSourceMP4File, 20 * 1000, 20 * 1000, wsMP4ClipFile);
            if (!bResult)
                Console.WriteLine("Unable to create a clip.");
        }

        //
        // The main console function. 
        // The user enters values for the duration of the 'segment' MP4 files and the number to keep. 
        // These two parameters define the maximum duration of the final MP4 file created when the segments are combined.
        //
        static void Main(string[] args)
        {
            //a small test to extract the part of the MP4 movie
            //CreateMP4Clip(); return;

            bool bUseDefaultValues = false;
            int i32SegmentLengthSecs = 10;
            int i32NumberOfSegments = 30;

            // Give the user the option of using default values.
            // The defaults above record a maximum of 30 MP4 files of 10 seconds length. The final, combined, MP4 file will have a maximum duration of 5 minutes.
            //
            while (true)
            {
                Console.WriteLine("Use default values (many segments, each 10 secs)? (Y/N [Y is a default value])");
                ConsoleKey i32Key = Console.ReadKey().Key;

                if (i32Key == ConsoleKey.Enter)
                    i32Key = ConsoleKey.Y;

                if (i32Key == ConsoleKey.Y)
                {
                    bUseDefaultValues = true;
                    break;
                }

                if (i32Key == ConsoleKey.N)
                {
                    bUseDefaultValues = false;
                    break;
                }

                Console.WriteLine("\nInvalid input.\n");
            }

            Console.WriteLine("\n");

            if (!bUseDefaultValues)
            {
                while (true)
                {
                    Console.WriteLine("Segment length in seconds (10 secs <= X <= 600 secs): ");
                    string sLine = Console.ReadLine();
                    if(int.TryParse(sLine, out i32SegmentLengthSecs))
                    {
                        if (i32SegmentLengthSecs >= 10 && i32SegmentLengthSecs <= 600)
                            break;
                    }
                    Console.WriteLine("\nInvalid input.\n");
                }

                while (true)
                {
                    Console.WriteLine("The number of segments (2 <= X <= 30): ");
                    string sLine = Console.ReadLine();
                    if (int.TryParse(sLine, out i32NumberOfSegments))
                    {
                        if (i32NumberOfSegments >= 2 && i32NumberOfSegments <= 30)
                            break;
                    }
                    Console.WriteLine("\nInvalid input.\n");
                }
            }

            // Start recording
            ContinuousRecording(i32SegmentLengthSecs, i32NumberOfSegments);

            return;
        }
    }
}
