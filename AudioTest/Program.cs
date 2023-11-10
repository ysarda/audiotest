using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        var outputPath = "output.wav"; // Specify the output path if needed

        using var audioRecorder = new AudioRecorder();
        bool spaceBarPressed = false;
        Console.WriteLine("Press the space bar to start recording. Press it again to stop.");
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
                        audioRecorder.StartRecording("alsa");
                    }
                    else
                    {
                        byte[] audioBytes = audioRecorder.StopRecording();
                        Console.WriteLine($"Audio length: {audioBytes.Length}");
                        File.WriteAllBytes(outputPath, audioBytes);
                        Console.WriteLine($"Saved audio to {outputPath}");
                    }
                }
            }
        }

    }
}
