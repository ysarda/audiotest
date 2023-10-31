using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        AudioRecorder audioRecorder = new AudioRecorder();

        Console.WriteLine("Press Enter to start recording...");
        Console.ReadLine();

        string outputPath = "recorded_audio.wav";

        // Start recording
        Console.WriteLine("Recording... Press Enter to stop recording.");
        audioRecorder.StartRecording(outputPath);

        Console.ReadLine(); // Press Enter to stop recording

        // Stop recording and get the audio buffer
        byte[] audioBuffer = audioRecorder.StopRecording();

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
