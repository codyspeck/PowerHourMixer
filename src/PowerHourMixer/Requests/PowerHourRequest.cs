using MediatR;
using PowerHourMixer.Models;

namespace PowerHourMixer.Requests;

public class PowerHourRequest : IRequest
{
    public Transition Transition { get; set; }
    public string TransitionCut { get; set; }
    public List<string> Cuts { get; set; } = new();
    public List<Track> Tracks { get; set; } = new();
}
