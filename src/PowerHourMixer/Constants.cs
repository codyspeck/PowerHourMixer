using System.Reflection;

namespace PowerHourMixer;

public static class Constants
{
    public static string ApplicationDataPath { get; }
    public static string ApplicationTrackDataPath { get; }
    public static string ApplicationCutDataPath { get; }
    public static string FfmpegPath { get; }

    static Constants()
    {
        var applicationExecutablePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var localApplicationDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        ApplicationDataPath = Path.Combine(localApplicationDataPath, "PowerHourMixer");
        ApplicationTrackDataPath = Path.Combine(ApplicationDataPath, "tracks");
        ApplicationCutDataPath = Path.Combine(ApplicationDataPath, "cuts");
        FfmpegPath = Path.Combine(applicationExecutablePath!, "lib", "ffmpeg", "bin");
    }
}