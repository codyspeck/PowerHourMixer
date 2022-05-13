using CliFx;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using PowerHourMixer;
using PowerHourMixer.Commands;
using PowerHourMixer.Handlers;
using PowerHourMixer.Requests;
using PowerHourMixer.Services;
using Xabe.FFmpeg;
using YoutubeExplode;

FFmpeg.SetExecutablesPath(Constants.FfmpegPath);
Directory.CreateDirectory(Constants.ApplicationDataPath);
Directory.CreateDirectory(Constants.ApplicationTrackDataPath);
Directory.CreateDirectory(Constants.ApplicationCutDataPath);

await using var services = new ServiceCollection()
    .AddMediatR(typeof(Program))
    .AddTransient<DefaultCommand>()
    .AddTransient<IPipelineBehavior<PowerHourRequest, Unit>, TrackDownloadHandler>()
    .AddTransient<IPipelineBehavior<PowerHourRequest, Unit>, TrackCutHandler>()
    .AddTransient<IPipelineBehavior<PowerHourRequest, Unit>, TransitionCutHandler>()
    .AddSingleton<TemporaryFileRepository>()
    .AddSingleton<YoutubeClient>()
    .BuildServiceProvider();

await new CliApplicationBuilder()
    .AddCommandsFromThisAssembly()
    .UseTypeActivator(services.GetService)
    .Build()
    .RunAsync();
    