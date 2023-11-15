
class AudioPlayerTest
{
    public static void Run()
    {
        AudioPlayer player = new AudioPlayer();
        Console.WriteLine("Press any key to load audio.");
        Console.ReadKey(intercept: true);
        byte[] audioBytes1 = File.ReadAllBytes("audio/sample-3s.mp3");
        byte[] audioBytes2 = File.ReadAllBytes("audio/sample-6s.mp3");
        byte[] audioBytes3 = File.ReadAllBytes("audio/sample-9s.mp3");

        player.PlayAudio(audioBytes1).Wait();
        player.PlayAudio(audioBytes2).Wait();
        player.PlayAudio(audioBytes3).Wait();

    }

}