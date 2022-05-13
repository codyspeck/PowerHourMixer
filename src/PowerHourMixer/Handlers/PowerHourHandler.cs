using MediatR;
using PowerHourMixer.Extensions;
using PowerHourMixer.Requests;
using Xabe.FFmpeg;

namespace PowerHourMixer.Handlers;

public class PowerHourHandler : IRequestHandler<PowerHourRequest>
{
    public async Task<Unit> Handle(PowerHourRequest request, CancellationToken cancellationToken)
    {
        var output = Path.Combine(Constants.ApplicationDataPath, "power_hour.mp3");
        
        File.Delete(output);

        var input = request.Cuts
            .Alternate(request.TransitionCut)
            .ToArray();

        Console.WriteLine($"Building {output}");

        await FFmpeg.Conversions.FromSnippet
            .ConcatenateAudio(output, input)
            .Start(cancellationToken);
        
        return Unit.Value;
    }
}