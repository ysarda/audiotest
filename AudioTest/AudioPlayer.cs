using CliWrap;

public class AudioPlayer
{
    private MemoryStream? _audioStream;
    private CancellationTokenSource? _cancellationTokenSource;
    private Command? _ffplayProcess;

    public async Task PlayAudio(byte[] audioBytes)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _audioStream = new MemoryStream(audioBytes);
        _ffplayProcess = PipeSource.FromStream(_audioStream) |
            Cli.Wrap("ffplay")
            .WithArguments(args => args
                .Add("-i").Add("pipe:0")
                .Add("-nodisp")
                .Add("-autoexit")
            );
        await _ffplayProcess.ExecuteAsync(_cancellationTokenSource.Token);
        Console.WriteLine("Audio playback complete.");
    }
}