namespace PowerHourMixer.Extensions;

public static class AsyncExtensions
{
    public static async Task<TOutput> Then<TInput, TOutput>(this Task<TInput> task, Func<TInput, TOutput> func)
    {
        return func(await task);
    }
}