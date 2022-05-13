using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using PowerHourMixer.Models;
using PowerHourMixer.Requests;

namespace PowerHourMixer.Commands;

[Command]
public class DefaultCommand : ICommand
{
    private readonly IMediator _mediator;

    public DefaultCommand(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async ValueTask ExecuteAsync(IConsole console)
    {
        var configuration = new ConfigurationBuilder()
            .AddYamlFile("appsettings.yaml")
            .Build();

        var transition = configuration
            .GetSection("Transition")
            .Get<Transition>();
        
        var tracks = configuration
            .GetSection("Tracks")
            .Get<List<Track>>();

        await _mediator.Send(new PowerHourRequest
        {
            Transition = transition,
            Tracks = tracks
        });
    }
}