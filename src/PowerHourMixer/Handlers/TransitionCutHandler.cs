using MediatR;
using PowerHourMixer.Extensions;
using PowerHourMixer.Requests;
using Xabe.FFmpeg;
using YoutubeExplode;

namespace PowerHourMixer.Handlers;

public class TransitionCutHandler : IPipelineBehavior<PowerHourRequest, Unit>
{
    private readonly YoutubeClient _youtube;

    public TransitionCutHandler(YoutubeClient youtube)
    {
        _youtube = youtube;
    }

    public async Task<Unit> Handle(PowerHourRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<Unit> next)
    {
        var video = await _youtube.Videos.GetAsync(request.Transition.Uri.ToString(), cancellationToken);
        var videoFilePath = video.TrackFilePath();
        var cutFilePath = video.CutFilePath(request.Transition.Start, request.Transition.Duration);

        request.TransitionCut = cutFilePath;

        if (File.Exists(cutFilePath))
            return await next();

        Console.WriteLine($"Cutting {cutFilePath}");
        
        var stream = await FFmpeg
            .GetMediaInfo(videoFilePath, cancellationToken)
            .Then(x => x.AudioStreams.First())
            .Then(x => x.Split(request.Transition.Start, request.Transition.Duration));

        await FFmpeg.Conversions.New()
            .AddStream(stream)
            .SetOutput(cutFilePath)
            .Start(cancellationToken);
        
        return await next();
    }
}