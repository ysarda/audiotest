
class AudioRecorderTest
{
    public static void Run()
    {
        using var audioRecorder = new AudioRecorder();
        bool spaceBarPressed = false;
        Console.WriteLine("Press the space bar to start recording. Press it again to stop.");
        var i = 0;
        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Spacebar)
                {
                    spaceBarPressed = !spaceBarPressed;
                    if (spaceBarPressed)
                    {
                        audioRecorder.StartRecording();
                    }
                    else
                    {
                        byte[] audioBytes = audioRecorder.StopRecording();
                        Console.WriteLine($"Audio length: {audioBytes.Length}");
                        var outputPath = $"audio/sample-{i++}.mp3";
                        File.WriteAllBytes(outputPath, audioBytes);
                        Console.WriteLine($"Saved audio to {outputPath}");
                    }
                }
            }
        }

    }
}
