using System.Text;
using CliWrap;
using CliWrap.EventStream;
public class AudioRecorder : IDisposable
{
    private CancellationTokenSource _cancellationTokenSource;
    private MemoryStream _audioStream;
    private byte[] _audioBytes;
    private Command? _ffmpegCommand;
    private bool _isRecording;

    public AudioRecorder()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _audioStream = new MemoryStream();
        _isRecording = false;
        _ffmpegCommand = null;
        _audioBytes = Array.Empty<byte>();
    }

    public void StartFfmpeg(string baseAudioSoftware = "alsa")
    {
        _cancellationTokenSource = new CancellationTokenSource();

        _ffmpegCommand = Cli.Wrap("ffmpeg")
            .WithArguments(args => args
                .Add("-f").Add(baseAudioSoftware)
                .Add("-i").Add("default")
                .Add("-f").Add("wav")
                .Add("pipe:1")
            )
            .WithValidation(CommandResultValidation.None)
            | PipeTarget.Null;
        var runEvent = ReturnCommandEvent();
    }

    public async Task ReturnCommandEvent()
    {
        if (_ffmpegCommand == null)
        {
            throw new Exception("FFMPEG command is null");
        }
        await foreach (var cmdEvent in _ffmpegCommand.ListenAsync(_cancellationTokenSource.Token))
        {
            switch (cmdEvent)
            {
                case StartedCommandEvent started:
                    Console.WriteLine($"Process started; ID: {started.ProcessId}");
                    break;
                case StandardOutputCommandEvent stdOut:
                    if (_isRecording)
                    {
                        _audioStream.Write(Encoding.UTF8.GetBytes(stdOut.Text));
                    }
                    break;
                case StandardErrorCommandEvent stdErr:
                    break;
                case ExitedCommandEvent exited:
                    Console.WriteLine($"Process exited; Code: {exited.ExitCode}");
                    break;
            }
        }
    }
    public void StartRecording()
    {
        _isRecording = true;
        ClearStream();
    }
    public byte[] StopRecording()
    {
        
        _isRecording = false;
        var result = _audioStream.ToArray();
        ClearStream();
        return result;
    }
    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _audioStream.Dispose();
    }

    public void ClearStream()
    {
        _audioStream = new MemoryStream();
    }
}
