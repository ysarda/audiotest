using CliWrap;

public class AudioPlayer : IDisposable
{
    private MemoryStream? _audioStream;
    private CancellationTokenSource? _cancellationTokenSource;
    private Task? _ffplayProcess;
    private const string NamedPipePath = "pipe";

    public void SetupPlay()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        Cli.Wrap("mkfifo")
            .WithArguments(args => args
                .Add(NamedPipePath)
            )
            .WithValidation(CommandResultValidation.None)
            .ExecuteAsync(CancellationToken.None);
        Cli.Wrap("ffplay")
            .WithArguments(args => args
                .Add("-i").Add(NamedPipePath)
                .Add("-nodisp")
            )
            .WithValidation(CommandResultValidation.None)
            .ExecuteAsync(CancellationToken.None);
    }
    public static void StreamAudio(byte[] audioData)
    {
        try
        {
            using var pipeStream = new FileStream(NamedPipePath, FileMode.Open, FileAccess.Write);
            pipeStream.Write(audioData, 0, audioData.Length);
            Console.WriteLine("Wrote " + audioData.Length + " bytes to named pipe.");
            Task.Delay(audioData.Length / 10).Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error writing to named pipe: " + ex.Message);
        }
    }

        public void Dispose()
    {
        throw new System.NotImplementedException();
    }

}