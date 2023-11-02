﻿// using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

class AudioRecorder
{
    private Process process;
    private MemoryStream memoryStream;
    private bool isRecording;

    public AudioRecorder()
    {
        process = new Process();
        process.StartInfo.FileName = "ffmpeg"; // Use "ffmpeg" or "ffmpeg.exe" based on your system configuration.
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;

        memoryStream = new MemoryStream();
    }

    public bool IsRecording => isRecording;

    public void StartRecording(string outputPath = "", string baseAudioSoftware = "alsa")
    {
        if (!isRecording)
        {
            isRecording = true;
            memoryStream.SetLength(0); // Clear the memory stream
            process.StartInfo.Arguments = $"-f {baseAudioSoftware} -i default {outputPath}";
            process.Start();

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    byte[] audioData = Encoding.Default.GetBytes(e.Data);
                    memoryStream.Write(audioData, 0, audioData.Length);
                }
            };

            process.ErrorDataReceived += (sender, e) => { /* Handle standard error if needed */ };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
        }
    }

    public byte[] StopRecording()
    {
        if (isRecording)
        {
            isRecording = false;
            process.Kill();
            return memoryStream.ToArray();
        }

        return Array.Empty<byte>();
    }

    public void Dispose()
    {
        process.Dispose();
        memoryStream.Dispose();
    }
}