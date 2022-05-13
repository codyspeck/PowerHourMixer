namespace PowerHourMixer.Extensions;

public static class LinqExtensions
{
    public static IEnumerable<T> Alternate<T>(this IEnumerable<T> source, T item)
    {
        foreach (var (element, index) in source.Select((x, i) => (x, i)))
        {
            if (index > 0)
                yield return item;

            yield return element;
        }
    }
}