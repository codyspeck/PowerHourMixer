namespace PowerHourMixer.Models;

public class Transition : IDownloadable
{
    public Uri Uri { get; set; }
    public TimeSpan Start { get; set; } = TimeSpan.Zero;
    public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(1);
}