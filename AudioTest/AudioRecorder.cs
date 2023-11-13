using System.Text;
using CliWrap;
using CliWrap.EventStream;
public class AudioRecorder : IDisposable
{
    private CancellationTokenSource _cancellationTokenSource;
    private MemoryStream _audioStream;

    public AudioRecorder()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _audioStream = new MemoryStream();
    }

    public void StartRecording()
    {
        _audioStream = new MemoryStream();
        _cancellationTokenSource = new CancellationTokenSource();

        var _ffmpegCommand = (Cli.Wrap("ffmpeg")
            .WithArguments(args => args
                .Add("-f").Add(GetOsInput())
                .Add("-i").Add("default")
                .Add("-f").Add("wav")
                .Add("pipe:1")
            )
            .WithValidation(CommandResultValidation.None)
            | PipeTarget.ToStream(_audioStream))
            .ExecuteAsync(_cancellationTokenSource.Token);
        Console.WriteLine("Recording...");
    }
    public byte[] StopRecording()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        var result = _audioStream.ToArray();
        Console.WriteLine("Stopped recording.");
        return result;
    }
    public void Dispose()
    {
        _audioStream.Dispose();
    }

    private static string GetOsInput()
    {
        // If Mac, return avfoundation, if Linux return alsa, if Windows return dshow
        return OperatingSystem.IsMacOS() ? "avfoundation" : OperatingSystem.IsLinux() ? "alsa" : "dshow";
    }
}
