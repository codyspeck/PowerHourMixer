using System.Diagnostics;
using MediatR;
using PowerHourMixer.Extensions;
using PowerHourMixer.Requests;
using PowerHourMixer.Services;
using Xabe.FFmpeg;
using YoutubeExplode;

namespace PowerHourMixer.Handlers;

public class TrackCutHandler : IPipelineBehavior<PowerHourRequest, Unit>
{
    private readonly TemporaryFileRepository _repository;
    private readonly YoutubeClient _youtube;

    public TrackCutHandler(TemporaryFileRepository repository, YoutubeClient youtube)
    {
        _repository = repository;
        _youtube = youtube;
    }

    public async Task<Unit> Handle(PowerHourRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
    {
        foreach (var track in request.Tracks)
        {
            var video = await _youtube.Videos.GetAsync(track.Uri.ToString(), cancellationToken);
            var videoFilePath = video.TrackFilePath();
            var cutFilePath = video.CutFilePath(track.Start, track.Duration);

            request.Cuts.Add(cutFilePath);

            if (File.Exists(cutFilePath))
                continue;

            Console.WriteLine($"Cutting {cutFilePath}");

            var temp = _repository.Create("mp3");

            await FFmpeg.Conversions.FromSnippet
                .Split(videoFilePath, temp, track.Start, track.Duration)
                .Then(x => x.Start(cancellationToken));

            var stream = await FFmpeg.GetMediaInfo(temp, cancellationToken)
                .Then(x => x.AudioStreams.First());

            var st = track.Duration
                .Subtract(TimeSpan.FromSeconds(2))
                .TotalMilliseconds;

            await FFmpeg.Conversions.New()
                .AddStream(stream)
                .AddParameter($"-af \"afade=t=out:st={st}ms:d=2000ms\"")
                .SetOutput(cutFilePath)
                .Start(cancellationToken);
        }
        
        return await next();
    }
}