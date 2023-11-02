using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        AudioRecorder audioRecorder = new AudioRecorder();
        Dictionary<string, string> baseAudioSoftware = new Dictionary<string, string>()
        {
            { "0", "alsa" },
            { "1", "dshow" },
            { "2", "avfoundation" }
        };

        Console.WriteLine("Press 0 for Linux, 1 for Windows, or 2 for macOS.");
        string? input = Console.ReadLine();
        
        if (input is null || 0 > int.Parse(input) || int.Parse(input) > 2)
        {
            Console.WriteLine("Invalid input. Press Enter to exit.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine($"Using {baseAudioSoftware[input.ToString()]} as the base audio software.");
        Console.WriteLine("Press Enter to start recording...");
        Console.ReadLine();

        string outputPath = "recorded_audio.wav";

        // Start recording
        Console.WriteLine("Recording... Press Enter to stop recording.");
        audioRecorder.StartRecording(outputPath, baseAudioSoftware[input.ToString()]);

        Console.ReadLine(); // Press Enter to stop recording

        // Stop recording and get the audio buffer
        byte[] audioBuffer = audioRecorder.StopRecording();
        audioRecorder.Dispose();

        if (audioBuffer != null)
        {
            Console.WriteLine($"Recording stopped. Audio saved to {outputPath}");

            // Save the audio buffer to a file (for testing)
            File.WriteAllBytes("recorded_audio_buffer.wav", audioBuffer);
        }
        else
        {
            Console.WriteLine("No audio recorded.");
        }
    }
}
