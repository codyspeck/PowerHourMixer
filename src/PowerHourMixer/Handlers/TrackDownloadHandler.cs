using MediatR;
using PowerHourMixer.Models;
using PowerHourMixer.Requests;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace PowerHourMixer.Handlers;

public class TrackDownloadHandler : IPipelineBehavior<PowerHourRequest, Unit>
{
    private readonly YoutubeClient _youtube;

    public TrackDownloadHandler(YoutubeClient youtube)
    {
        _youtube = youtube;
    }

    public async Task<Unit> Handle(PowerHourRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
    {
        var tracks = new List<IDownloadable>(request.Tracks)
            .Append(request.Transition);
        
        foreach (var track in tracks)
        {
            var video = await _youtube.Videos.GetAsync(track.Uri.ToString(), cancellationToken);

            var videoFilePath = Path.Combine(Constants.ApplicationTrackDataPath, $"{video.Id}.mp3");

            if (File.Exists(videoFilePath))
                continue;

            var stream = (await _youtube.Videos.Streams.GetManifestAsync(video.Id, cancellationToken))
                .GetAudioOnlyStreams()
                .GetWithHighestBitrate();

            Console.WriteLine($"Downloading {videoFilePath}");
            
            await _youtube.Videos.Streams.DownloadAsync(stream, videoFilePath, cancellationToken: cancellationToken);
        }
        
        return await next();
    }
}