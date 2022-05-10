import time
import json
import os
import sys
from FBRecorder import FBRecorder, LogKind, FBRecorderProfile, FBGlobalError, FBRecorderImageFormat

def PrintGlobalErrors(pFBRecorder):
	ErrorIndex = 0
	while True:
		Error = pFBRecorder.GetGlobalError(ErrorIndex)
		if Error == FBGlobalError.Unknown:
			break
		print("Global error: %d" % (Error.value))
		ErrorIndex += 1

sFBRecorderPath = 'C:\\Program Files\\Blueberry Software\\FlashBack SDK 5\\'
#sFBRecorderPath = 'C:\\dev\\FBGames\\FBv6\\FinalDebug\\'

if not os.path.exists(sFBRecorderPath):
    print("The path '%s' doesn't exist." % (sFBRecorderPath))
    sys.exit(0)

sFBRecorderDLL = os.path.join(sFBRecorderPath, "FBRecorder.dll")
sFBRecorderLogPath = '.\\Logs\\'

if not os.path.exists(sFBRecorderDLL):
    print("The FBRecorder.dll couldn't be found.")
    sys.exit(0)

#create a directory for log files
if not os.path.exists(sFBRecorderLogPath):
    os.mkdir(sFBRecorderLogPath)

sLicence = ""

fbRecorder = FBRecorder(sFBRecorderDLL, sLicence,
    sFBRecorderLogPath, 32, LogKind.Verbose)

"""
Filenames = ["C:\\dev\\FBGames\\FBv6\\FinalDebug\\test_0.mp4", 
    "C:\\dev\\FBGames\\FBv6\\FinalDebug\\test_1.mp4", 
    "C:\\dev\\FBGames\\FBv6\\FinalDebug\\test_2.mp4"
]
fbRecorder.MergeMP4Files(json.dumps(Filenames), "C:\\dev\\FBGames\\FBv6\\FinalDebug\\test.mp4")
"""

if not fbRecorder.CreateRecorder():
    print("Unable to create FBRecorder")
    exit(0)

fbRecorder.CaptureScreen("test.png", FBRecorderImageFormat.PNG, 0, 0, 0, 800, 600)

#how to find the existing video encoders
bresult, sjsonconfig = fbRecorder.GetDefaultConfigJSON()
if not bresult:
    print("fbRecorder.GetDefaultConfigJSON error")
    exit(0)
jsonconfig = json.loads(sjsonconfig)
print("Existing video encoders:")
for video_encoder in jsonconfig['AvailableVideoEncoders']:
    if fbRecorder.CheckVideoEncoderAvailability(video_encoder['Type'], video_encoder['Name']):
        print(video_encoder)

if not fbRecorder.ActivateProfile(FBRecorderProfile.SAFE_SCREEN):
    print("fbRecorder.ActivateProfile error")
    exit(0)

fbRecorder.ResetGlobalErrors()
if not fbRecorder.InitRecorder('test.mp4'):
    print("fbRecorder.InitRecorder error")
    PrintGlobalErrors(fbRecorder)
    exit(0)
PrintGlobalErrors(fbRecorder)

fbRecorder.ResetGlobalErrors()
if not fbRecorder.StartRecording():
    print("fbRecorder.StartRecording error")
    PrintGlobalErrors(fbRecorder)
    exit(0)
PrintGlobalErrors(fbRecorder)

print("recording for 10 secs...")
time.sleep(10)
fbRecorder.Pause(True)
print("paused for 5 secs...")
time.sleep(5)
fbRecorder.Pause(False)
print("Continue... for 10 secs")
time.sleep(10)

fbRecorder.StopRecording()
print("recording has been stopped")

fbRecorder.DestroyRecorder()
print("Exit")
