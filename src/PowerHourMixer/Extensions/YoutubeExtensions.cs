using YoutubeExplode.Videos;

namespace PowerHourMixer.Extensions;

public static class YoutubeExtensions
{
    public static string CutFilePath(this IVideo video, TimeSpan start, TimeSpan duration)
    {
        return Path.Combine(Constants.ApplicationCutDataPath, $"{video.Id}-{start.Ticks}-{duration.Ticks}.mp3");
    }
    
    public static string TrackFilePath(this IVideo video)
    {
        return Path.Combine(Constants.ApplicationTrackDataPath, $"{video.Id}.mp3");
    }
}