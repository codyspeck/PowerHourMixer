using System.Text;
using Xabe.FFmpeg;

namespace PowerHourMixer.Extensions;

public static class FfmpegExtensions
{
    public static IConversion ConcatenateAudio(this Snippets snippets, string output, params string[] input)
    {
        var clause = Enumerable.Range(0, input.Length)
            .Aggregate(new StringBuilder(), (current, i) => current.Append($"[{i}:0]"))
            .ToString();

        return input
            .Aggregate(FFmpeg.Conversions.New(), (current, file) => current.AddParameter($"-i {file}"))
            .AddParameter($"-filter_complex \"{clause}concat=n={input.Length}:v=0:a=1[out]\"")
            .AddParameter("-map \"[out]\"")
            .SetOutput(output);
    }
}