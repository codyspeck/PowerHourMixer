namespace PowerHourMixer.Services;

public class TemporaryFileRepository : IDisposable
{
    private readonly List<string> _files = new();

    public string Create(string extension)
    {
        var file = Path.ChangeExtension(Path.GetTempFileName(), extension);
        
        _files.Add(file);
        
        return file;
    }
    
    public void Dispose()
    {
        foreach (var file in _files)
        {
            Console.WriteLine($"Deleting {file}");
            File.Delete(file);
        }
            
    }
}